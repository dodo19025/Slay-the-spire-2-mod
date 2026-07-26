using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Assets;
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
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
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
		new DynamicVar("CombatStartGrudgePower",5m),
		new DynamicVar("MaxGrudgeProcsFromDamage",3m),
		new DynamicVar("GrudgeGainPerDamage",2m),
		new DynamicVar("GrudgeGainPerHit",1m),
		new DynamicVar("MaxGrudgeProcsFromHits",5m),
		new DynamicVar("CombatStartHealth",0m)
	]); //Made for values to be easily changable
	
	public static readonly SpireField<PlayerCombatState, int> MaxTattoos = new(() => 4);
	public static readonly SpireField<PlayerCombatState, int> ConversionRate = new(() => 5);
	public static readonly SpireField<PlayerCombatState, int> RecordedConsumedGrudge = new(() => 0);
	
	
	


	protected override IEnumerable<IHoverTip> ExtraHoverTips =>
	[

		HoverTipFactory.FromPower<SealedSwordPower>(),
		HoverTipFactory.FromPower<RisingFeverPower>(), 
	]; //Get the hover tips from the Json file and display it on the relic

	
	
	public override async Task AfterRoomEntered(AbstractRoom room)
	{
		if (room is CombatRoom)
		{

			Flash();
			await PowerCmd.Apply<SealedSwordPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature,
				1m, base.Owner.Creature, null); //cahnge this back to first seal when done testing
			await PowerCmd.Apply<RisingFeverPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature,
				1m, base.Owner.Creature,
				null); //Throwingplayercontext so not caring about any player choice, and this applies the rising fever power at 1}
			await TheMiddleNursefatherOutisCmd.ChangeFeverAmount(new BlockingPlayerChoiceContext(), base.Owner.Creature, 1,false);

		}
	}
	
	public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
	{
		if (player == Owner && Owner.PlayerCombatState?.TurnNumber == 1)
		{
			CardModel? Card = Owner.Creature?.CombatState?.CreateCard<Unpacking>(Owner.Creature.Player!);
			await CardPileCmd.AddGeneratedCardToCombat(Card!, PileType.Hand, Owner);
		}
	}
	public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (participants.Contains(Owner.Creature) && Owner.PlayerCombatState?.TurnNumber <= 1)
		{
			await GrudgeResource.GainGrudge((int)DynamicVars["CombatStartGrudgePower"].BaseValue, Owner!.Creature!.Player!);
			DynamicVars["CombatStartHealth"].BaseValue = Owner.Creature.CurrentHp;
			MainFile.Logger.Info($"Current HP --> {DynamicVars["CombatStartHealth"].BaseValue}");
			PlayerCombatState? playerCombatState = Owner.Creature?.Player?.PlayerCombatState;
			DamageTakenHook.PaybackAcitvated[playerCombatState!] = 0;
			DamageTakenHook.TookDamageLastTurn[playerCombatState!] = false;
			RecordedConsumedGrudge[playerCombatState!] = 0;
		}
	}

	
	

	private int _currentGainedGrudgeFromHits = 0;
	private int _currentGainedGrudgeFromDamage = 0;
	private bool _allowGrudgeFromDamage = true;
	private bool _allowGrudgeFromHits = true;
	
	public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
		Creature? dealer, CardModel? cardSource) //this is to handle how grudge is gained
	{
		if (dealer == null || dealer == Owner.Creature || dealer.Side == Owner.Creature.Side)
		{
			return;
		}
		
		if (target.IsPlayer && result.UnblockedDamage >= 0 && _allowGrudgeFromDamage && target.Block <= 0)
		{
			await GrudgeResource.GainGrudge((int)DynamicVars["GrudgeGainPerDamage"].BaseValue,
				Owner.Creature.Player);
			_currentGainedGrudgeFromDamage++;
			if (_currentGainedGrudgeFromDamage >= DynamicVars["MaxGrudgeProcsFromDamage"].BaseValue)
			{
				_allowGrudgeFromDamage = false;
			}
			return; //so if the player is taking any form of damage, it does not activate the part when you are just getting targetted
		}
		
		if (target.IsPlayer && _allowGrudgeFromHits && result.UnblockedDamage <= 0 && props.IsPoweredAttack())
		{
			await GrudgeResource.GainGrudge((int)DynamicVars["GrudgeGainPerHit"].BaseValue,
				Owner.Creature.Player);
			_currentGainedGrudgeFromHits++;
			if (_currentGainedGrudgeFromHits >= DynamicVars["MaxGrudgeProcsFromHits"].BaseValue)
			{
				_allowGrudgeFromHits = false;
			}
		}
		

	}

	public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (!participants.Contains(Owner.Creature) || Owner.Creature.Side != side) return Task.CompletedTask;
		_currentGainedGrudgeFromHits = 0;
		_currentGainedGrudgeFromDamage = 0;
		_allowGrudgeFromDamage = true;
		_allowGrudgeFromHits = true;
		return Task.CompletedTask;
	}
//kill me!


	

	//protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<RisingFeverPower>(), HoverTipFactory.Static(StaticHoverTip.Block),HoverTipFactory.FromPower<StrengthPower>()];
}
