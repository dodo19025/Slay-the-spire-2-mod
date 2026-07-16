using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Achievements;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis;

public static class TheMiddleNursefatherOutisCmd
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

    public static int ValidPaybackAmount(this Creature creature, decimal PaybackAmount)
    {
        if (creature.Block >= PaybackAmount && creature.Block > 0)
        {
            return (int)PaybackAmount;
        }
        else if (creature.Block < PaybackAmount && creature.Block > 0)
        {
            return (int)creature.Block;
        }
        return 0;
    }

    public static async Task AdditionalPayback(this Creature creature, PlayerChoiceContext choiceContext,
        Decimal amount, Creature? applier = null, CardModel? cardSource = null)
    {
        await PowerCmd.Apply<PaybackPower>(choiceContext, creature, amount, creature, null); // unconditional paybackgain
    }
    
    public static async Task PlayAnimation(this Creature creature, string AnimationName, float NormalSecondWait)
    {
        if (!(creature.Player?.Character is TheMiddleNurseFatherOutisCode.Character.TheMiddleNurseFatherOutis))
        {
            return;
        }
        var node = NCombatRoom.Instance?.GetCreatureNode(creature);
        if (node?.Visuals == null)
        {
            return;
        }
        var Statemachine = node.Visuals.GetNodeOrNull<AnimationTree>("AnimationTree").Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
        if (Statemachine != null)
        {
            
            
            string GodotTrigger = AnimationName.ToLowerInvariant() switch
            {
                "hit" => "hurt",
                "idle" => "idle",
                "dead" => "die",
                "cast" => "cast",
                "block" => "block",
                "attack" => "legattack",
                "swordattack" => "swordattack",
                "swordlattack" => "swordlattack",
                "unpacking" => "unpacking",
                _ => AnimationName
            };
            Statemachine.Start(GodotTrigger);
            float FastSecondWait = NormalSecondWait/2;
            await Cmd.CustomScaledWait(FastSecondWait, NormalSecondWait);
        }
    }
    
    public static decimal CalculateBleedLost(decimal BleedAmount) //bleedamount is for the amount of bleed that we currently have
    {
        if (BleedAmount <= 1)
        {
            return  1m;
        }

        return ((decimal)Math.Round((decimal)(BleedAmount * (1m / 2m))));

    }
    
    public static async Task ActivateBleed(PlayerChoiceContext choiceContext, Creature target, decimal BleedAmount)
    {
        await CreatureCmd.Damage(choiceContext, target, BleedAmount, ValueProp.Unpowered | ValueProp.SkipHurtAnim, null);
        if (target.IsAlive)
        {
            await PowerCmd.ModifyAmount(choiceContext, target.GetPower<BleedPower>(), -CalculateBleedLost(BleedAmount), null, null);
        }
        else
        {
            await Cmd.CustomScaledWait(0.1f, 0.25f);
        }
    } 

}