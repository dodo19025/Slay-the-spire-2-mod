using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Hooks;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;


public class RetaliatePower()
    : TheMiddleNurseFatherOutisPower, IAfterPaybackDone
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public async Task AfterPaybackDone(PlayerChoiceContext choiceContext, Player player, int amountPayback, Creature target)
    {
        var multiplier =  (Amount / 100);
        await PowerCmd.Apply<BleedPower>(choiceContext, target, amountPayback * multiplier, Owner, null);
        if (Owner.HasPower<LaevateinnPower>())
        {
            await PowerCmd.Apply<BurnPower>(choiceContext, target, amountPayback * multiplier, Owner, null);
        }
    }
}