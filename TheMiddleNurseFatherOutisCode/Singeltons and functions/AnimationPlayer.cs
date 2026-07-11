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
            if (cardSource.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Fervour))
            {
                if (visualthreeseal.Visible)
                {
                    await PlayAnimation(dealer, "swordlattack");
                }
                else if (!visualthreeseal.Visible)
                {
                    await PlayAnimation(dealer, "swordattack");
                }

            }
            else
            {
                await PlayAnimation(dealer, "legattack");
            }
        }
    }

    public static async Task PlayAnimation(Creature creature, string AnimationName)
    {
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
                _ => AnimationName
            };
            if (Statemachine.HasConnections(GodotTrigger))
            {
                Statemachine.Start(GodotTrigger);
                await Cmd.CustomScaledWait(0.1f, 0.2f);
            }
        }
    }
    
}