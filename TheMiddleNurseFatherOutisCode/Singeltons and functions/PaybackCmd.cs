using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Achievements;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis;

public static class PaybackCmd
{
    public static async Task GainPayback(this Creature creature, PlayerChoiceContext choiceContext,Decimal amount, Creature? applier = null, CardModel? cardSource = null)
    {
        int CurrentBlock = creature.Block;
        
        if (CurrentBlock > 0) //if player has block
        {
            if (amount >= CurrentBlock)
            {
                amount = CurrentBlock; //if the amount of payback gained is less than block, then current block becomes the amount gained
            }
            
            await CreatureCmd.LoseBlock(creature, amount); //consumes block equal to payback
            await PowerCmd.Apply<PaybackPower>(choiceContext, creature, amount, creature, null); // Gains equal amounts payback power
        }
        
    }

    public static bool GainedPayback(this Creature creature, decimal PaybackAmount)
    {
        if (PaybackAmount > creature.Block)
        {
            return false;
        }

        return true;
    }

    public static async Task AdditionalPayback(this Creature creature, PlayerChoiceContext choiceContext,
        Decimal amount, Creature? applier = null, CardModel? cardSource = null)
    {
        await PowerCmd.Apply<PaybackPower>(choiceContext, creature, amount, creature, null); // unconditional paybackgain
    }

}