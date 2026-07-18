using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Extensions;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Relics;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Relics;



public class HOSBookOfVengeance()
	: TheMiddleNurseFatherOutisRelic
{
	public override RelicRarity Rarity =>
		RelicRarity.Starter;

	public int StartingFever = 1;
	private const string _RisingFeverStartKey = "RisingFever";
	private const string _EnergyNextTurnStartKey = "EnergyNextTurn";

	protected override IEnumerable<DynamicVar> CanonicalVars => (
	[
		new DynamicVar("RisingFever", 1m),
		new DynamicVar("SwordStage0", 1m),
		new DynamicVar("CombatStartGrudgePower",5m),
		new DynamicVar("MaxGrudgeProcsFromDamage",3m),
		new DynamicVar("GrudgeGainPerDamage",2m),
		new DynamicVar("GrudgeGainPerHit",1m),
		new DynamicVar("MaxGrudgeProcsFromHits",5m),
		new DynamicVar("MaxGrudgeAmount",15m)
	]); //Made for values to be easily changable
	
	public static readonly SavedSpireField<PlayerCombatState, int> MaxTattoos = new(() => 4,"MaxTattoos");
	public static readonly SavedSpireField<PlayerCombatState, int> ConversionRate = new(() => 5,"ConversionRate");
	public static readonly SavedSpireField<PlayerCombatState, int> RecordedConsumedGrudge = new(() => 0,"RecordedConsumedGrudge");


	protected override IEnumerable<IHoverTip> ExtraHoverTips =>
	[
		HoverTipFactory.FromPower<RisingFeverPower>(), 
		HoverTipFactory.FromPower<SealedSwordPower>()
		
	]; //Get the hover tips from the Json file and display it on the relic

	public override async Task AfterRoomEntered(AbstractRoom room)
	{
		if(room is CombatRoom)
		{
			
			Flash();
			await PowerCmd.Apply<SealedSwordPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature,
				base.DynamicVars["SwordStage0"].BaseValue, base.Owner.Creature, null); //cahnge this back to first seal when done testing
			await PowerCmd.Apply<RisingFeverPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature,
				base.DynamicVars["RisingFever"].BaseValue, base.Owner.Creature, null); //Throwingplayercontext so not caring about any player choice, and this applies the rising fever power at 1
			await PowerCmd.Apply<GrudgePower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature,
				base.DynamicVars["CombatStartGrudgePower"].BaseValue, base.Owner.Creature, null); //Throwingplayercontext so not caring about any player choice, and this applies the rising fever power at 1
		}
		
	}
	
	public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
	{
		if (player == base.Owner && base.Owner.PlayerCombatState?.TurnNumber == 1)
		{
			CardModel? Card = base.Owner.Creature?.CombatState?.CreateCard<Unpacking>(base.Owner.Creature.Player!);
			await CardPileCmd.AddGeneratedCardToCombat(Card!, PileType.Hand, base.Owner);
		}
	}
	public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (participants.Contains(base.Owner.Creature) && base.Owner.PlayerCombatState?.TurnNumber <= 1)
		{
			PlayerCombatState? playerCombatState = base.Owner.Creature?.Player?.PlayerCombatState;
			DamageTakenHook.PaybackAcitvated[playerCombatState!] = 0;
			DamageTakenHook.TookDamageLastTurn[playerCombatState!] = false;
			HOSBookOfVengeance.RecordedConsumedGrudge[playerCombatState!] = 0;
		}
	}

	private int _currentGainedGrudgeFromHits = 0;
	private int _currentGainedGrudgeFromDamage = 0;
	private bool _allowGrudgeFromDamage = true;
	private bool _allowGrudgeFromHits = true;
	
	public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
		Creature? dealer, CardModel? cardSource)
	{
		//MainFile.Logger.Info("11111");
		if (dealer == null || dealer == base.Owner.Creature || dealer.Side == base.Owner.Creature.Side || !props.IsPoweredAttack())
		{
			return;
		}

		if (base.Owner.Creature.HasPower<GrudgePower>())
		{
			if (base.Owner.Creature.GetPowerAmount<GrudgePower>() >= base.DynamicVars["MaxGrudgeAmount"].BaseValue)
			{
				return;
			}
		}
		
		if (target.IsPlayer && _allowGrudgeFromHits)
		{
			await FixGrudgeCount(base.Owner.Creature, choiceContext, base.DynamicVars["GrudgeGainPerHit"].BaseValue,
				base.DynamicVars["MaxGrudgeAmount"].BaseValue);
			_currentGainedGrudgeFromHits++;
			if (_currentGainedGrudgeFromHits >= base.DynamicVars["MaxGrudgeProcsFromHits"].BaseValue)
			{
				_allowGrudgeFromHits = false;
			}
		}
		
		if (target.IsPlayer && result.UnblockedDamage > 0 && _allowGrudgeFromDamage)
		{ 
			await FixGrudgeCount(base.Owner.Creature, choiceContext, base.DynamicVars["GrudgeGainPerDamage"].BaseValue,
				base.DynamicVars["MaxGrudgeAmount"].BaseValue);
			_currentGainedGrudgeFromDamage++;
			if (_currentGainedGrudgeFromDamage >= base.DynamicVars["MaxGrudgeProcsFromDamage"].BaseValue)
			{
				_allowGrudgeFromDamage = false;
			}
		}
	}

	public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (!participants.Contains(base.Owner.Creature) || base.Owner.Creature.Side != side) return Task.CompletedTask;
		_currentGainedGrudgeFromHits = 0;
		_currentGainedGrudgeFromDamage = 0;
		_allowGrudgeFromDamage = true;
		_allowGrudgeFromHits = true;
		return Task.CompletedTask;
	}
//kill me

	private static async Task FixGrudgeCount(Creature creature, PlayerChoiceContext choiceContext, decimal AmountofGrudgeGained, decimal MaxGrudgeAllowed)
	{
		if (creature.HasPower<GrudgePower>())
		{
			if (AmountofGrudgeGained + creature.GetPowerAmount<GrudgePower>() >= MaxGrudgeAllowed && creature.GetPowerAmount<GrudgePower>() < MaxGrudgeAllowed)
			{
				decimal _amountCorrected = MaxGrudgeAllowed - creature.GetPowerAmount<GrudgePower>();
				await PowerCmd.Apply<GrudgePower>(choiceContext, creature, _amountCorrected, creature, null);
				//say you have 11 grudge, and you're set to gain 5, i should only allow 4 gained from this
				//how i would do this is just simply do max amount - current amount then add that amount regardless?
			}
			else if (AmountofGrudgeGained + creature.GetPowerAmount<GrudgePower>() < MaxGrudgeAllowed)
			{
				decimal _amountCorrected = MaxGrudgeAllowed - creature.GetPowerAmount<GrudgePower>();
				await PowerCmd.Apply<GrudgePower>(choiceContext, creature, AmountofGrudgeGained, creature, null);
			}
			
		}
		else
		{
			decimal _amountCorrected = MaxGrudgeAllowed - creature.GetPowerAmount<GrudgePower>();
			await PowerCmd.Apply<GrudgePower>(choiceContext, creature, AmountofGrudgeGained, creature, null);
		}
	}

	

	//protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<RisingFeverPower>(), HoverTipFactory.Static(StaticHoverTip.Block),HoverTipFactory.FromPower<StrengthPower>()];
}
