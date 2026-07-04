using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheMiddleNurseFatherOutis;

public class attack_animation_change(): CustomSingletonModel(HookType.Combat)
{
    
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        CanvasItem visualthreeseal = (cardPlay.Card.Owner.Creature.GetCreatureNode()!.Body.GetNode("3Sealaniamtions") as CanvasItem)!;
        if (cardPlay.Card.Type == CardType.Attack && cardPlay.Card.Owner.Creature.Player?.Character is TheMiddleNurseFatherOutisCode.Character.TheMiddleNurseFatherOutis)
        {
            if (cardPlay.Card.Keywords.Contains(FervourKeyWord.Fervour))
            {
                if (visualthreeseal.Visible)
                {
                    CreatureCmd.TriggerAnim(cardPlay.Card.Owner.Creature, "swordlattack", 0.05f);
                }
                else if (!visualthreeseal.Visible)
                {
                    CreatureCmd.TriggerAnim(cardPlay.Card.Owner.Creature, "swordattack", 0.05f);
                }

            }
            else if(!cardPlay.Card.Keywords.Contains(FervourKeyWord.Fervour))
            {
                 CreatureCmd.TriggerAnim(cardPlay.Card.Owner.Creature, "legattack", 0.05f);
            }
        }
    }
}