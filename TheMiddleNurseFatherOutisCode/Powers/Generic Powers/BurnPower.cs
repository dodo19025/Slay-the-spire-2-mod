using System.Buffers;
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


public class BurnPower() 
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public Color Burncolor = new Color("#f7681b");
    public int DamageMinusBlock = 0;
    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        if (base.Amount > base.Owner.Block)
        {
            DamageMinusBlock = base.Amount - base.Owner.Block;
        }
        return [new HealthBarForecastSegment(DamageMinusBlock, Burncolor, HealthBarForecastDirection.FromRight)];
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power == this && base.Amount > 0)
        {
            if (base.Amount > base.Owner.Block)
            {
                DamageMinusBlock = base.Amount - base.Owner.Block;
            }
        }
    }

    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (creature.HasPower<BurnPower>()&& base.Amount > 0)
        {
            if (base.Amount > base.Owner.Block)
            {
                DamageMinusBlock = base.Amount - base.Owner.Block;
            }
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
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