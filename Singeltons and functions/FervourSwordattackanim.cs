using System.Buffers;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheMiddleNurseFatherOutis;

public class FervourSwordattackanim(): CustomSingletonModel(HookType.Combat)
{
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Keywords.Contains(FervourKeyWord.Fervour))
        {
            await CreatureCmd.TriggerAnim(cardPlay.Card.Owner.Creature, "swordattack", 0.05f);
        }
    }
}