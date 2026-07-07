using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;



public class LaevateinnPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[1]
    {
        new DynamicVar("AppliedBurn", 3m)
    });
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BurnPower>(),
    ];

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(base.Owner))
        {
            return;
        }
        Flash();
        await Cmd.CustomScaledWait(0.2f, 0.4f);
        await PowerCmd.Apply<Powers.BurnPower>(new ThrowingPlayerChoiceContext(), base.Owner,
            base.DynamicVars["AppliedBurn"].BaseValue, base.Owner, null);
        foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<Powers.BurnPower>(new ThrowingPlayerChoiceContext(), hittableEnemy,
                base.DynamicVars["AppliedBurn"].BaseValue, base.Owner, null);
        }

    }
}