using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class ParalyzePower() : TheMiddleNurseFatherOutisPower
{
    
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("ReducedDamagePresen",25m),
        new DynamicVar("ReducedDamage",0.75m),
        new DynamicVar("MonsterDamage",0m),
        new DynamicVar("MonsterHits",0m)
    ];
    
    
    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (dealer != base.Owner)
        {
            return 1m;
        }
        if (!props.IsPoweredAttack())
        {
            return 1m;
        }
        if (base.Owner.IsMonster)
        {

            int? attacks = Owner.Monster.NextMove.Intents.Sum(intent => intent is AttackIntent attackIntent ? attackIntent.Repeats : 0); //checks how many times this creature is attacking
            //int? damage = Owner?.Monster?.NextMove?.Intents.Where(static i => i is AttackIntent).Cast<AttackIntent>().FirstOrDefault()?.GetSingleDamage([target], Owner.Monster.Creature);
            base.DynamicVars["MonsterHits"].BaseValue = (decimal)attacks;
            //base.DynamicVars["MonsterDamage"].BaseValue = (decimal)damage;
            //
            //{OnPlayer:|Original damage ({MonsterDamage:diff()} x {MonsterHits:diff()})}.
        }
        decimal num = base.DynamicVars["ReducedDamage"].BaseValue;
        return num;
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (dealer != null && dealer.HasPower<ParalyzePower>())
        {
            PowerCmd.Decrement(this);
        }
    }
}