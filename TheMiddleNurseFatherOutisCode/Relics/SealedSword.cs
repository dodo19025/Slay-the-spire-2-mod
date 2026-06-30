using MegaCrit.Sts2.Core.Commands;
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
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Relics;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Relics;


public class SealedSword()
	: TheMiddleNurseFatherOutisRelic
{
	public override RelicRarity Rarity =>
		RelicRarity.Starter;

	public int StartingFever = 1;
	private const string _RisingFeverStartKey = "RisingFever";
	private const string _EnergyNextTurnStartKey = "EnergyNextTurn";

	protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[3]
	{
		new DynamicVar("RisingFever", 1m),
		new DynamicVar("EnergyNextTurn", 1m),
		new DynamicVar("SwordStage0", 1m)
	}); //Made for values to be easily changable

	protected override IEnumerable<IHoverTip> ExtraHoverTips =>
	[
		HoverTipFactory.FromPower<RisingFeverPower>(), HoverTipFactory.Static(StaticHoverTip.Block),
		HoverTipFactory.FromPower<StrengthPower>()
	]; //Get the hover tips from the Json file and display it on the relic

	public override async Task AfterRoomEntered(AbstractRoom room)
	{
		if(room is CombatRoom)
		{
			Flash();
			await PowerCmd.Apply<RisingFeverPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature,
				base.DynamicVars["RisingFever"].BaseValue, base.Owner.Creature, null); //Throwingplayercontext so not caring about any player choice, and this applies the rising fever power at 1
		}
		
	}

	public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
		Creature? dealer, CardModel? cardSource)
	{
		if (dealer != base.Owner.Creature && target.IsPlayer && result.WasBlockBroken)
		{
			Flash();
			await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature,
				base.DynamicVars["EnergyNextTurn"].BaseValue, base.Owner.Creature, null);
		}
	}

	//protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<RisingFeverPower>(), HoverTipFactory.Static(StaticHoverTip.Block),HoverTipFactory.FromPower<StrengthPower>()];
}
