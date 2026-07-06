using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;



public class PaybackPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];
    

    private bool DidPayback = false;
    
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (dealer == base.Owner || dealer.Side == base.Owner.Side ||
            !props.IsPoweredAttack())
        {
            return;
        }

        if (target.IsPlayer && result.UnblockedDamage > 0)
            await CreatureCmd.Damage(choiceContext, dealer, base.Amount, ValueProp.Unpowered,base.Owner,null,null);
            await Cmd.CustomScaledWait(0.15f, 0.25f);
            await CreatureCmd.TriggerAnim(base.Owner, "legattack", 0.05f);
            DidPayback = true;
            MainFile.Logger.Info("Did Payback");
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (DidPayback)
        {
            await PowerCmd.Remove(this);
            DidPayback = false;
        }
    }
}