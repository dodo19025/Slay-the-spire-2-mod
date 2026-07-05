using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
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
    
    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command.Attacker == base.Owner || command.TargetSide != base.Owner.Side ||
            !command.DamageProps.IsPoweredAttack())
        {
            return;
        }
        DamageResult damageResult = command.Results.SelectMany((List<DamageResult> r) => r).FirstOrDefault((DamageResult r) => r.Receiver == base.Owner);
        if (damageResult.UnblockedDamage != 0 || damageResult != null)
        {
            //await DamageCmd.Attack(base.Amount).Targeting(command.Attacker).Execute(choiceContext);
            //Modsounds.legattack.Play();
            DidPayback = true;
            //WithAttackerAnim("legattack",0.2f)
        }
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (DidPayback)
        {
            PowerCmd.Remove(this);
            DidPayback = false;
        }
    }
}