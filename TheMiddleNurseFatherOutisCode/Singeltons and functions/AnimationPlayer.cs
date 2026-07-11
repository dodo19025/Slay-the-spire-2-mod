using System.Buffers;
using System.Dynamic;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheMiddleNurseFatherOutis;

public class AttackAnimationChange(): CustomSingletonModel(HookType.Combat)
{


    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (dealer.IsEnemy)
        {
            return;
        }

        if (cardSource == null)
        {
            return;
        }

        if (!(cardSource.Owner.Creature.Player?.Character is TheMiddleNurseFatherOutisCode.Character
                .TheMiddleNurseFatherOutis))
        {
            return;
        }
        if (cardSource.Type == CardType.Attack && cardSource.Owner.Creature.Player?.Character is TheMiddleNurseFatherOutisCode.Character.TheMiddleNurseFatherOutis)
        {
            CanvasItem visualthreeseal = (cardSource.Owner.Creature.GetCreatureNode()!.Body.GetNode("3Sealanimations") as CanvasItem)!;
            var node = NCombatRoom.Instance?.GetCreatureNode(dealer);
            if (node?.Visuals == null)
            {
                return;
            }
            var Statemachine = node.Visuals.GetNodeOrNull<AnimationTree>("AnimationTree").Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
            
            
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
                Statemachine.Travel("legattack");
                //await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "legattack", 0.05f);
            }
        }
    }
    
}