using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheMiddleNurseFatherOutis;

public class attack_animation_change(): CustomSingletonModel(HookType.Combat)
{
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Attack)
        {
            if (cardPlay.Card.Keywords.Contains(FervourKeyWord.Fervour))
            {
                await CreatureCmd.TriggerAnim(cardPlay.Card.Owner.Creature, "legattack", 0.05f);
            }
            else if(!cardPlay.Card.Keywords.Contains(FervourKeyWord.Fervour))
            {
                await CreatureCmd.TriggerAnim(cardPlay.Card.Owner.Creature, "legattack", 0.05f);
            }
        }
    }
}