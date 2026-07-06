using System.Drawing;
using BaseLib.Hooks;
using BaseLib.Patches.UI;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using Color = Godot.Color;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class BurnPower() : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public Color burncolor = new Color("#f7681b");
    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(base.Amount, burncolor, HealthBarForecastDirection.FromRight)];
    }
    
    private bool IsBurnLethal() //compares this creature's current health with the amount of burn it has
    {
        if (base.Amount <= 0)
        {
            return false; //jsut a sfety measure or whatever
        }

        return base.Amount >= base.Owner.CurrentHp; //compares the burn amount with health, if it's larger than or equal then returns true
        //otherwise return false
    }
    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(base.Owner))
        {
            return;
        }
        await CreatureCmd.Damage(choiceContext, base.Owner,base.Amount,ValueProp.Unpowered,null,null,null);
        if (base.Owner.IsAlive)
        {
            await PowerCmd.Decrement(this);
        }
        else
        {
            await Cmd.CustomScaledWait(0.1f, 0.25f);
        }
    }
    
}