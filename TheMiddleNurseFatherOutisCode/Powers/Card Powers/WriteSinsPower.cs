using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
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

    public async Task AfterPaybackDone(PlayerChoiceContext choiceContext ,Player player, int amountpayback,  Creature target)
    { 
        await PowerCmd.Apply<VulnerableNextTurn>(choiceContext, target, Amount, Owner, null);
        Amount--;
        if (Amount <= 0)
        {
            PowerCmd.Remove(this);
        }
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.HasPower<WriteSinsPower>())
        {
            PowerCmd.Remove(this);

        }
    }
}