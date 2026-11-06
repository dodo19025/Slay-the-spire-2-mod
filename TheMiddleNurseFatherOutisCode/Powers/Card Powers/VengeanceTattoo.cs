using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Localization;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Relics;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;


public class VengeanceTattoo()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DynamicVar("MaxAmount", 4m),
        new DynamicVar("DamageReduction",1m),
        new DynamicVar("CalculatedPReduction",0m),
        new DynamicVar("CalculatedFReduction",0m),
        new SustainVar(3)

    ]); //Made for values to be easily changable

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power != this || power.Owner != Owner)
        {
            return Task.CompletedTask;
        }

        if (Amount > DynamicVars["MaxAmount"].BaseValue)
        {
            Amount = (int)DynamicVars["MaxAmount"].BaseValue;
        }
        DynamicVars["CalculatedPReduction"].BaseValue = (decimal)((0.25 + (Amount * 0.1))) * 100;
        DynamicVars["CalculatedFReduction"].BaseValue = Math.Max(0m, (5 - Amount));
        return Task.CompletedTask;
    }

    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != base.Owner)
        {
            return amount;
        }
        return Math.Max(0m, amount - (5 - Amount));
    }
    
    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (target != Owner)
        {
            return 1m;
        }
        if (!props.IsPoweredAttack())
        {
            return 1m;
        }
        if (dealer == null)
        {
            return 1m;
        }
        return (decimal)(1 - (0.25 + (Amount * 0.1)));
    }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner))
        {
            await PowerCmd.Apply<SustainPower>(choiceContext, Owner, DynamicVars["Sustain"].BaseValue, Owner, null);
        }
    }
}