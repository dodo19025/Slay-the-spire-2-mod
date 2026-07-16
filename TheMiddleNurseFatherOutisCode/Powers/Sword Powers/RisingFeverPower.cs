using System.Diagnostics;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class RisingFeverPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[3]
    {
        new DynamicVar("RisingFeverReapply", 2m), 
        new DynamicVar("StrengthGain", 3m),
        new DynamicVar("CardsLeft",2m)
    });

    public int Stage = 0;
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power == this)
        {
            if (Owner.Player?.Character is Character.TheMiddleNurseFatherOutis)
            {
                int FeverAmountCorrection = 0;
                CanvasItem visualzeroseal = (Owner.GetCreatureNode()!.Body.GetNode("0Sealanimations") as CanvasItem)!;
                CanvasItem visualoneseal = (Owner.GetCreatureNode()!.Body.GetNode("1Sealanimations") as CanvasItem)!;
                CanvasItem visualtwoseal = (Owner.GetCreatureNode()!.Body.GetNode("2Sealaniamtions") as CanvasItem)!;
                CanvasItem visualthreeseal = (Owner.GetCreatureNode()!.Body.GetNode("3Sealanimations") as CanvasItem)!;
                
                int FeverAmount = base.Owner.GetPowerAmount<RisingFeverPower>();
                if (FeverAmount <= 0 && applier.IsPlayer && Stage < 3)
                {
                    if (FeverAmount <= 0) //fix for if it's negative
                    {
                        FeverAmountCorrection += (FeverAmount * -1); //catch for when i forget to make the check in the card (dumbass)
                        if (base.Owner.HasPower<LaevateinnPower>())
                        {
                            await PowerCmd.Remove(this);
                            return;
                        }
                    }
                    Stage += 1;
                    if (Stage == 1)
                    {
                        Modsounds.unpacking0.Play();
                        visualzeroseal.Visible = false;
                        visualoneseal.Visible = true;
                        await CreatureCmd.TriggerAnim(base.Owner, "unpacking", 0.4f);
                        await PowerCmd.Apply<RisingFeverPower>(choiceContext, base.Owner,
                            base.DynamicVars["RisingFeverReapply"].BaseValue + FeverAmountCorrection, base.Owner,
                            null); //reapplies rising fever to 2
                        await PowerCmd.Apply<SealedSwordPower>(choiceContext, base.Owner, -1m, base.Owner, null);
                        await PowerCmd.Apply<FirstSealRemovedPower>(choiceContext, base.Owner, 1m, base.Owner, null);
                    }

                    if (Stage == 2)
                    {
                        Modsounds.unpacking2.Play();
                        visualoneseal.Visible = false;
                        visualtwoseal.Visible = true;
                        await Owner.PlayAnimation("unpacking", 2f);
                        await Cmd.CustomScaledWait(0.5f, 1f);
                        await CreatureCmd.TriggerAnim(base.Owner, "sunglasses", 2f);
                        await PowerCmd.Apply<RisingFeverPower>(choiceContext, base.Owner,
                            base.DynamicVars["RisingFeverReapply"].BaseValue + FeverAmountCorrection, base.Owner,
                            null); //reapplies rising fever to 2
                        await PowerCmd.Apply<FirstSealRemovedPower>(choiceContext, base.Owner, -1m, base.Owner, null);
                        await PowerCmd.Apply<SecondSealRemovedPower>(choiceContext, base.Owner, 1m, base.Owner, null);
                    }

                    if (Stage == 3)
                    {
                        Modsounds.unpacking1.Play();
                        visualtwoseal.Visible = false;
                        visualthreeseal.Visible = true;
                        await CreatureCmd.TriggerAnim(base.Owner, "unpacking", 2f);
                        await PowerCmd.Apply<SecondSealRemovedPower>(choiceContext, base.Owner, -1m, base.Owner, null);
                        await PowerCmd.Apply<LaevateinnPower>(choiceContext, base.Owner, 1m, base.Owner, null);
                    }
                }
            }
        }
    }

    public const string _cardsLeftKey = "CardsLeft";

    public class Data
    {
        public int FeverCardsPlayed;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != base.Owner.Player || cardPlay.Card.Owner.HasPower<LaevateinnPower>())
        {
            return;
        }
        if(cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Fervour))
        {
            base.DynamicVars["CardsLeft"].BaseValue--;

        }
        if (base.DynamicVars["CardsLeft"].BaseValue <= 0)
        {
            await Cmd.Wait(0.25f);
            await PowerCmd.Decrement(this);
            base.DynamicVars["CardsLeft"].BaseValue = 2m;
        }
    }
}