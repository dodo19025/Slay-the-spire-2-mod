using BaseLib.Hooks;
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

    public Color BleedColor = new Color("#5e0702");

    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(AmountOfBleedDamageToBeTaken(base.Owner), BleedColor, HealthBarForecastDirection.FromRight)];
    }


    public decimal CalculateBleedLost(decimal BleedAmount) //bleedamount is for the amount of bleed that we currently have
    {
        if (BleedAmount <= 1)
        {
            return  1m;
        }

        return ((decimal)Math.Round((decimal)(BleedAmount * (1m / 2m))));

    }
    
    public int AmountOfBleedDamageToBeTaken(Creature? creature) //calculates the total damage a creature will take with the amount of hits it does
    {
        int TotalDamageToBeTaken = 0;
        if (creature != null && !(creature.IsPlayer))
        {
            if (Owner.Monster != null)
            {
                int attacks = Owner.Monster.NextMove.Intents.Sum(intent => intent is AttackIntent attackIntent ? attackIntent.Repeats : 0); //checks how many times this creature is attacking
                int BaseBleed = base.Amount;
                int LostBleed = (int)CalculateBleedLost(base.Amount);
                for (int i = 0; i < attacks; i++) //runs a loop for the amount of times the creature will be hitting
                {
                    TotalDamageToBeTaken += BaseBleed; //adds the damage of the bleed 
                    BaseBleed -= LostBleed; //then reduces it based on the amount set to be lost after the attack
                    LostBleed = (int)CalculateBleedLost(BaseBleed); //runs a new calculation to find the new amount of bleed that has to be lost
                }

            }
        }
        MainFile.Logger.Info("Damage To be Taken >", TotalDamageToBeTaken);
        MainFile.Logger.Info(TotalDamageToBeTaken.ToString());
        return TotalDamageToBeTaken;
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power,
        decimal amount, Creature? applier,
        CardModel? cardSource) //calculate the amount of bleed that is set to be lost
    {
        if (power == this)
        {
            int TotalDamage = AmountOfBleedDamageToBeTaken(base.Owner);
            base.DynamicVars["BleedLost"].BaseValue = CalculateBleedLost(base.Amount);
        }
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (dealer != null && target != base.Owner && dealer == base.Owner && props.IsPoweredAttack() && target != null)
        {
            await CreatureCmd.Damage(choiceContext, base.Owner, base.Amount, ValueProp.Unpowered | ValueProp.SkipHurtAnim, null);
            if (base.Owner.IsAlive)
            {
                await PowerCmd.ModifyAmount(choiceContext, this, -CalculateBleedLost(base.Amount), null, null);
            }
            else
            {
                await Cmd.CustomScaledWait(0.1f, 0.25f);
            }
        }

    }
}