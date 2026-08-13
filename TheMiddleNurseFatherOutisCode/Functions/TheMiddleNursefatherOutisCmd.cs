using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Achievements;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Relics;

namespace TheMiddleNurseFatherOutis;

public static class TheMiddleNursefatherOutisCmd
{
    
    /// <summary>
    /// Used for generating Payback with the "seethe" mechanic, which consumes block to gain equal payback
    /// </summary>
    /// <param name="creature"> Creatures that gains payback</param>
    /// <param name="choiceContext"> Players choicecontext</param>
    /// <param name="amount"> Amount of payback you are trying to gain</param>
    /// <param name="cardSource">Card that gavepayback from this</param>
    public static async Task GainPayback(this Creature creature, PlayerChoiceContext choiceContext,Decimal amount, CardModel? cardSource = null)
    {
        int CurrentBlock = creature.Block;
        
        if (CurrentBlock > 0) //if player has block
        {
            if (amount >= CurrentBlock)
            {
                amount = CurrentBlock; //if the amount of payback gained is less than block, then current block becomes the amount gained
            }
            
            await CreatureCmd.LoseBlock(choiceContext,creature,amount,creature); //consumes block equal to payback
            await PowerCmd.Apply<PaybackPower>(choiceContext, creature, amount, creature, null); // Gains equal amounts payback power
        }
        
    }

    /// <summary>
    /// Compares the payback amount to your block number and checks if you are Valid to gain all payback in the amount given
    /// </summary>
    /// <param name="creature"> Creature getting its payback compared</param>
    /// <param name="PaybackAmount"> Amount of payback it is set to compare to block</param>
    /// <returns></returns>
    public static bool GainedPayback(this Creature creature, decimal PaybackAmount)
    {
        if (PaybackAmount > creature.Block)
        {
            return false;
        }
        return true;
    }

    
    /// <summary>
    /// Returns how much payback you are set to gain depending on how much block you have (if you have more block you gain all the payback specified, otherwise you gain payback equal to your block or 0)
    /// </summary>
    /// <param name="creature"> Creature getting the payback compared to</param>
    /// <param name="PaybackAmount"> Amount of payback being checked</param>
    /// <returns></returns>
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

    /// <summary>
    /// Gives unconditional amount of payback, in hindsight this is uncencessary lool
    /// </summary>
    /// <param name="creature"> Creature gaining payback</param>
    /// <param name="choiceContext"> The player's choice that signaled this event</param>
    /// <param name="amount"> Amount of payback to be gained</param>
    /// <param name="cardSource"> The card source that would apply this payback amount</param>
    public static async Task AdditionalPayback(this Creature creature, PlayerChoiceContext choiceContext,
        Decimal amount, CardModel? cardSource = null)
    {
        await PowerCmd.Apply<PaybackPower>(choiceContext, creature, amount, creature, null); // unconditional paybackgain
    }
    
    
    /// <summary>
    /// Animation player handler for outis
    /// </summary>
    /// <param name="creature"> Gets the creature to play the animations for</param>
    /// <param name="AnimationName"> The animation name for the animation to be played</param>
    /// <param name="NormalSecondWait"> The delay that is played after the animation ends</param>
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
        var Statemachine = node?.Visuals.GetNodeOrNull<AnimationTree>("AnimationTree").Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
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

    
    /// <summary>
    /// Handler for changing the sword seals, playing animations and sounds and changing the rising fever numbers
    /// </summary>
    /// <param name="creature"> The creature that is having their sealed sword state changed</param>
    /// <param name="choiceContext"> The players choice that signaled this event</param>
    public static async Task ChangeSwordSeal(this Creature creature, PlayerChoiceContext choiceContext)
    {
        if (!(creature.Player?.Character is TheMiddleNurseFatherOutisCode.Character
                .TheMiddleNurseFatherOutis))
        {
            return;
        }
        
        CanvasItem visualzeroseal = (creature.GetCreatureNode()!.Body.GetNode("0Sealanimations") as CanvasItem)!;
        CanvasItem visualoneseal = (creature.GetCreatureNode()!.Body.GetNode("1Sealanimations") as CanvasItem)!;
        CanvasItem visualtwoseal = (creature.GetCreatureNode()!.Body.GetNode("2Sealaniamtions") as CanvasItem)!;
        CanvasItem visualthreeseal = (creature.GetCreatureNode()!.Body.GetNode("3Sealanimations") as CanvasItem)!;

        if (creature.HasPower<LaevateinnPower>())
        {
            return;
        }
        
        foreach (PowerModel power in creature.Powers)
        {
            switch (power)
            { 
                case SealedSwordPower: 
                    visualzeroseal.Visible = false;
                    visualoneseal.Visible = true;
                    Modsounds.unpacking0.Play();
                    await Cmd.CustomScaledWait(0.5f, 1f);
                    await CreatureCmd.TriggerAnim(creature, "unpacking", 0.4f);
                    await PowerCmd.Remove<SealedSwordPower>(creature);
                    await PowerCmd.Apply<FirstSealRemovedPower>(choiceContext, creature,1m,creature,null);
                    await ChangeFeverAmount(choiceContext, creature, 2,true);
                    return;
                    
                case FirstSealRemovedPower:
                    visualoneseal.Visible = false;
                    visualtwoseal.Visible = true;
                    Modsounds.unpacking1.Play();
                    await Cmd.CustomScaledWait(0.5f, 1f);
                    await CreatureCmd.TriggerAnim(creature, "unpacking", 0.4f);
                    await PowerCmd.Remove<FirstSealRemovedPower>(creature);
                    await PowerCmd.Apply<SecondSealRemovedPower>(choiceContext, creature,1m,creature,null);
                    await ChangeFeverAmount(choiceContext, creature, 2, true);
                    await Cmd.CustomScaledWait(0.5f, 1f);
                    await CreatureCmd.TriggerAnim(creature, "sunglasses", 2f);
                    return;
                    
                case SecondSealRemovedPower:
                    visualtwoseal.Visible = false;
                    visualthreeseal.Visible = true;
                    Modsounds.unpacking2.Play();
                    await Cmd.CustomScaledWait(0.5f, 1f);
                    await CreatureCmd.TriggerAnim(creature, "unpacking", 0.4f);
                    await PowerCmd.Remove<SecondSealRemovedPower>(creature);
                    await PowerCmd.Apply<LaevateinnPower>(choiceContext, creature,1m,creature,null);
                    if (creature.HasPower<RisingFeverPower>())
                    {
                        await PowerCmd.Remove<RisingFeverPower>(creature);
                    }
                    return;
            }
        }
    }
    
    
    /// <summary>
    /// Calculates the amount of bleed to be reduced after it is set to be activated
    /// </summary>
    /// <param name="BleedAmount">Target's bleed amount before it would get reduced</param>
    /// <returns></returns>
    public static decimal CalculateBleedLost(decimal BleedAmount) //bleedamount is for the amount of bleed that we currently have
    {
        if (BleedAmount <= 1)
        {
            return  1m;
        }

        return ((decimal)Math.Round((decimal)(BleedAmount * (1m / 2m))));

    }
    /// <summary>
    /// The function that gets called when activating bleed on the target to handle the calculations, damage and reduce it by the appropriate amounts
    /// </summary>
    /// <param name="choiceContext"> The player's choice that signaled this event</param>
    /// <param name="target"> The target that is set to take damage from bleed</param>
    /// <param name="BleedAmount"> The target's bleed amount before it gets reduced</param>
    public static async Task ActivateBleed(PlayerChoiceContext choiceContext, Creature? target, decimal BleedAmount)
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

    /// <summary>
    /// Handles applying bleed using outis' cards for the additional burn application using the swords, applies bleed as normal if at sealed sword 0 or no sword stage
    /// </summary>
    /// <param name="choiceContext"> The player's choice that signaled this event</param>
    /// <param name="applier"> The creature applying the bleed</param>
    /// <param name="bleedAmount"> The amount of bleed to be applied</param>
    /// <param name="target"> The creature that will have bleed applied to them</param>
    /// <param name="cardModel"> The card that applied bleed</param>

    public static async Task CardApplyBleed(PlayerChoiceContext choiceContext, Creature? applier, decimal bleedAmount ,Creature? target, CardModel cardModel)
    {
        decimal additionalBurnPerBleed = 0;
        PowerModel? swordPower = null;
        
        if (applier.HasPower<FirstSealRemovedPower>())
        {
            swordPower = applier.GetPower<FirstSealRemovedPower>();
            additionalBurnPerBleed = swordPower.DynamicVars["BurnApplicationValue"].BaseValue; 
        }

        if (applier.HasPower<SecondSealRemovedPower>() || applier.HasPower<LaevateinnPower>())
        {
            additionalBurnPerBleed = bleedAmount; 
        }
        
        await PowerCmd.Apply<BleedPower>(choiceContext, target, bleedAmount, applier, cardModel);
        
        if (additionalBurnPerBleed > 0)
        {
            await PowerCmd.Apply<BurnPower>(choiceContext, target, additionalBurnPerBleed, applier, cardModel);
        }
        
    }

    /// <summary>
    /// Changes the rising fever value and unpacks upon it is set to 0
    /// </summary>
    /// <param name="choiceContext"> The player's choice that signaled this event</param>
    /// <param name="creature"> The creature that is set to have its fever amount changed</param>
    /// <param name="FeverAmount"> The amount of fever that is modifying the current fever amount, the numbers are additive meaning a negative reduction just needs a negative number fed it</param>
    /// <param name="RestrictUnpackingThisTurn"> Dictates if unpacking is restricted for this turn. PS: unpacking through rising fever automatically sets this to false for this turn so any powers or skills that unrestrict this will have to manually change the rising fever boolean</param>
    /// <returns></returns>
    public static Task ChangeFeverAmount(PlayerChoiceContext choiceContext, Creature creature, decimal FeverAmount, bool RestrictUnpackingThisTurn)
    {
        if (!creature.HasPower<RisingFeverPower>()) return Task.CompletedTask;
        PowerModel? risingFever = creature.GetPower<RisingFeverPower>();
        risingFever.DynamicVars["FeverAmount"].BaseValue =
            Math.Max(0, risingFever.DynamicVars["FeverAmount"].BaseValue + FeverAmount);
        risingFever.Flash();
        risingFever.InvokeDisplayAmountChanged();
        if (RestrictUnpackingThisTurn)
        {
            ((BoolVar)risingFever.DynamicVars["CanUnpack"]).BoolVal = false;
        }
        return Task.CompletedTask;
    }
    
    
    /// <summary>
    /// Checks if the player is allowed to unpack based on if they have 0 rising fever
    /// </summary>
    /// <param name="choiceContext"> The player's choice which signaled this event</param>
    /// <param name="creature"> The creature that has rising fever</param>
    /// <returns></returns>
    public static bool CanUnpack(PlayerChoiceContext choiceContext, Creature creature)
    {
        if (creature.HasPower<RisingFeverPower>() && creature.IsPlayer && creature.IsAlive)
        {
            //MainFile.Logger.Info("Detected that the player can unpack from 'can unpack' function");
            PowerModel? power = creature.GetPower<RisingFeverPower>();
            if (power?.DynamicVars["FeverAmount"].BaseValue <= 0 && ((BoolVar)power.DynamicVars["CanUnpack"]).BoolVal)
            {
                return true;
            }
        }
        return false;
    }

/// <summary>
/// Returns the current sword stage the player is on
/// </summary>
/// <param name="creature"> The creature who has the sword</param>
/// <returns></returns>
    public static PowerModel? getSwordStage(Creature creature)
    {
        foreach (PowerModel power in creature.Powers)
        {
            switch (power)
            {
                case SealedSwordPower:
                    return power;
                case FirstSealRemovedPower:
                    return power;
                case SecondSealRemovedPower:
                    return power;
                case LaevateinnPower:
                    return power;

            }
        }
        return null;
    }
    
    /// <summary>
    /// Converts Grudge to tattoos based on the conversion rate that is set in the relic.PS: change how this works so it is friendly with cards instead of relying on a relic
    /// </summary>
    /// <param name="creature"> The creature that is gaining tattoos</param>
    /// <param name="choiceContext"> The players choice that fired this signal</param>
    /// <param name="GrudgeConsumed"> The amount of grudge to be consumed for this change</param>
    public static async Task ConvertGrudgeToTattoo(Creature? creature, PlayerChoiceContext choiceContext,
		decimal GrudgeConsumed)
    {
        decimal MaxTattoos = (decimal)HOSBookOfVengeance.MaxTattoos[creature?.Player!.PlayerCombatState!];
        decimal ConversionRate = (decimal)HOSBookOfVengeance.ConversionRate[creature?.Player!.PlayerCombatState!];
        decimal TotalConsumedGrudge = (decimal)HOSBookOfVengeance.RecordedConsumedGrudge[creature?.Player!.PlayerCombatState!];

        decimal CorrectedGrudge = 0;
        
		if(!creature!.HasPower<GrudgePower>()) return;
        //MainFile.Logger.Info($"$Recorded Grudge Amount Before Any Calculations --> {HOSBookOfVengeance.RecordedConsumedGrudge[creature?.Player!.PlayerCombatState!]}");
        // we need to get the total amount of grudge they ACTUALLY consumed
        decimal CurrentGrudge = creature!.GetPowerAmount<GrudgePower>();
        if (CurrentGrudge < GrudgeConsumed)
        {
            CorrectedGrudge = CurrentGrudge;
        }
        if (CurrentGrudge >= GrudgeConsumed)
        {
            CorrectedGrudge = GrudgeConsumed;

        }

        TotalConsumedGrudge += CorrectedGrudge;
        HOSBookOfVengeance.RecordedConsumedGrudge[creature?.Player!.PlayerCombatState!] += (int)TotalConsumedGrudge;
       //MainFile.Logger.Info($"$Recorded Grudge Amount Before Conversions --> {HOSBookOfVengeance.RecordedConsumedGrudge[creature?.Player!.PlayerCombatState!]}");
       
        if (creature!.GetPowerAmount<GrudgePower>() <= CorrectedGrudge)
        {
            await PowerCmd.Apply<GrudgePower>(choiceContext,creature, -creature.GetPowerAmount<GrudgePower>(),creature, null);
        }

        if (creature.GetPowerAmount<GrudgePower>() > CorrectedGrudge)
        {
            await PowerCmd.Apply<GrudgePower>(choiceContext,creature, -CorrectedGrudge,creature, null);

        }
        
		if (TotalConsumedGrudge >= ConversionRate)
		{
			decimal TattoosGained = TotalConsumedGrudge / ConversionRate; //get thenumber of tattoos you'll gain from the total amount of grudges you have
			decimal LeftOverGrudge = TotalConsumedGrudge % ConversionRate; //dunno why im making this ??? what
            HOSBookOfVengeance.RecordedConsumedGrudge[creature?.Player!.PlayerCombatState!] -=  (int)(TattoosGained * ConversionRate); //reduce your grudge by the amount that was actually consumed
            //MainFile.Logger.Info($"$Recorded Grudge Amount After Conversions --> {HOSBookOfVengeance.RecordedConsumedGrudge[creature?.Player!.PlayerCombatState!]}");
			if (creature!.HasPower<VengeanceTattoo>())
            { 
                
                if (creature.GetPowerAmount<VengeanceTattoo>() + TattoosGained >= MaxTattoos && creature.GetPowerAmount<VengeanceTattoo>() < MaxTattoos) //what this does is check if the creature's current tattoo count + the amount to be gained is higher than the max tattoos allowed
				{
                    
					decimal _amountCorrected = MaxTattoos - creature.GetPowerAmount<VengeanceTattoo>(); //since the amount of tattoos would overflow, you'd just set this to the max tattoos
					await PowerCmd.Apply<VengeanceTattoo>(choiceContext,creature, _amountCorrected,creature, null);

				}
				else if (creature.GetPowerAmount<VengeanceTattoo>() + TattoosGained < MaxTattoos &&
				         creature.GetPowerAmount<VengeanceTattoo>() < MaxTattoos) //this is if you're gaining an okay amount!!
				{
					await PowerCmd.Apply<VengeanceTattoo>(choiceContext,creature, TattoosGained,creature, null);

				}
			}
			else
			{
				await PowerCmd.Apply<VengeanceTattoo>(choiceContext,creature, TattoosGained,creature, null);

			}
		}
	}

}