using System.Drawing;
using BaseLib.Hooks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using Color = Godot.Color;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class ShieldPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    private readonly Color shieldColor = new Color("#1f9eed");
    
    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(Amount, shieldColor, HealthBarForecastDirection.FromLeft)];
    }

    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        decimal damageTaken = 0;
        if (target != Owner)
        {
            return amount;
        }

        if (amount >= Amount)
        {
            Flash();
            damageTaken = amount - Amount;
            Amount = 0;
        }

        if (amount < Amount)
        {
            Flash();
            damageTaken = Amount - amount;
            Amount -= (int)amount;
        }
        return Math.Min(0, damageTaken);
    }
}