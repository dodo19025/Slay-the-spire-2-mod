using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheMiddleNurseFatherOutis;

public class attack_animation_change(): CustomSingletonModel(HookType.Combat)
{
    

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        CanvasItem visualthreeseal = (cardSource.Owner.Creature.GetCreatureNode()!.Body.GetNode("3Sealanimations") as CanvasItem)!;
        if (cardSource.Type == CardType.Attack && cardSource.Owner.Creature.Player?.Character is TheMiddleNurseFatherOutisCode.Character.TheMiddleNurseFatherOutis)
        {
            if (cardSource.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Fervour))
            {
                if (visualthreeseal.Visible)
                {
                    await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "swordlattack", 0.05f);
                }
                else if (!visualthreeseal.Visible)
                {
                    await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "swordattack", 0.05f);
                }

            }
            else
            {
                await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "legattack", 0.05f);
            }
        }
    }
}