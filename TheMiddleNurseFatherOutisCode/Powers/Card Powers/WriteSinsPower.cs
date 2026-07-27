using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Hooks;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;


public class WriteSinsPower()
    : TheMiddleNurseFatherOutisPower, IAfterPaybackDone
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("VunPower", 1m),
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    public async Task AfterPaybackDone(PlayerChoiceContext choiceContext ,Player player, int amountPayback,  Creature target)
    {
        if (player != base.Owner.Player)
        {
            return; //so if another player does payback as well
        }
        await PowerCmd.Apply<VulnerableNextTurn>(choiceContext, target, DynamicVars["VunPower"].BaseValue, Owner, null);
        Amount--;
        if (Amount <= 0)
        {
            await PowerCmd.Remove(this);
        }
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.HasPower<WriteSinsPower>())
        {
            await PowerCmd.Remove(this);
        }
    }
}