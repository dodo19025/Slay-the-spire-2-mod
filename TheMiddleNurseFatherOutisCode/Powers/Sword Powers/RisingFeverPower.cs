using System.Diagnostics;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public abstract class RisingFeverPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[2]
    {
        new DynamicVar("RisingFeverReapply", 2m), 
        new DynamicVar("StrengthGain", 3m)
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
                        FeverAmountCorrection += (FeverAmount * -1);
                    }
                    Stage += 1;
                    if (Stage == 1)
                    {
                        Modsounds.unpacking0.Play();
                        visualzeroseal.Visible = false;
                        visualoneseal.Visible = true;
                        await Cmd.CustomScaledWait(0.1f, 0.25f);
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
                        await Cmd.CustomScaledWait(0.1f, 0.25f);
                        await CreatureCmd.TriggerAnim(base.Owner, "unpacking", 2f);
                        await Cmd.CustomScaledWait(0.1f, 0.25f);
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
                        await Cmd.CustomScaledWait(0.1f, 0.25f);
                        await CreatureCmd.TriggerAnim(base.Owner, "unpacking", 2f);
                        await PowerCmd.Apply<SecondSealRemovedPower>(choiceContext, base.Owner, -1m, base.Owner, null);
                        await PowerCmd.Apply<LaevateinnPower>(choiceContext, base.Owner, 1m, base.Owner, null);
                        await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner,
                            base.DynamicVars["StrengthGain"].BaseValue, base.Owner, null);
                    }
                }
            }
        }
    }
}