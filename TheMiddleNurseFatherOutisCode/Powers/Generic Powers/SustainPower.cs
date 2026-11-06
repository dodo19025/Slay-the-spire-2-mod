using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class SustainPower() : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power,
        decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power.Owner != Owner)
        {
            return;
        }

        int numOfSutains = 0;
        foreach (PowerModel powerModel in Owner.Powers)
        {
            if (powerModel is SustainPower)
            {
                numOfSutains++;
            }
        }
        if (power is SustainPower && amount <= Amount && numOfSutains >= 2)//find if the newly added instance of sustain is smaller than this one, then remove this current one if it is
        {
            await PowerCmd.Remove(this);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner))
        {
            if (Owner.HasPower<SustainPower>() && Owner.Block > 0)
            {
                var VitalityGained = (Math.Round((decimal)Owner.Block / Amount));
                if (VitalityGained <= 0)
                {
                    return;
                }
                await PowerCmd.Apply<ShieldPower>(choiceContext, Owner, VitalityGained, Owner, null);
                await CreatureCmd.LoseBlock(choiceContext, Owner, Owner.Block, Owner);
                await PowerCmd.Remove(this);

            }
        }
    }
}