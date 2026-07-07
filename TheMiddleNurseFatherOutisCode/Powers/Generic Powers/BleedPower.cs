using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class BleedPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[1]
    {
        new DynamicVar("BleedLost", 0m), 
    });

    public Color BleedColor = new Color("#f00e0e");



    public decimal CalculateBleedLost(decimal BleedAmount) //bleedamount is for the amount of bleed that we currently have
    {
        int AmountToBeLost = 0;
        if (BleedAmount <= 1)
        {
            return AmountToBeLost = 1;
        }

        AmountToBeLost = (int)BleedAmount * (2 / 3);
        return Math.Round((decimal)AmountToBeLost);
        
    }
    
    public int AmountOfBleedDamageToBeTaken(Creature? creature)
    {
        int TotalDamageToBeTaken = 0;
        if (creature != null && !(creature.IsPlayer))
        {
            if (Owner.Monster != null)
            {
                int attacks = Owner.Monster.NextMove.Intents.Sum(intent => intent is AttackIntent attackIntent ? attackIntent.Repeats : 0); //checks how many times this creature is attacking
                int BaseBleed = base.Amount;

            }
        }
        return TotalDamageToBeTaken;
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power,
        decimal amount, Creature? applier,
        CardModel? cardSource) //calculate the amount of bleed that is set to be lost
    {
        if (power == this)
        {
            base.DynamicVars["BleedLost"].BaseValue = CalculateBleedLost(base.Amount);
        }
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        await CreatureCmd.Damage(choiceContext, base.Owner, base.Amount, ValueProp.Unpowered | ValueProp.SkipHurtAnim, null);
        await PowerCmd.ModifyAmount(choiceContext, this, -CalculateBleedLost(base.Amount), null, null);
    }
}