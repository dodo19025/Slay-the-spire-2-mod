using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;



public class GrudgePower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    private const string _damageReductionKey = "DamageReduction";

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("DamageReduction",1m),
        new DynamicVar("MaxGrudgeAmount", 15m)

    ];
    
    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != base.Owner)
        {
            return amount;
        }
        return Math.Max(0m, amount - base.DynamicVars["DamageReduction"].BaseValue);
    }
    
    public override Task AfterModifyingHpLostAfterOsty()
    {
        Flash();
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Enemy)
        {
            PowerCmd.Decrement(this);
        }
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power == this && base.Amount > base.DynamicVars["MaxGrudgeAmount"].BaseValue && power.Owner == base.Owner)
        {
            decimal _correctionCacl = base.Amount - base.DynamicVars["MaxGrudgeAmount"].BaseValue;
            await PowerCmd.ModifyAmount(choiceContext, power, -(_correctionCacl), base.Owner, null);
        }
    }
}