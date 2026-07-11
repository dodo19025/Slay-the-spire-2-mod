using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheMiddleNurseFatherOutis;

public class PowerupAnimCheck() : CustomSingletonModel(HookType.Combat)
{
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Power &&  cardPlay.Card.Owner.Creature.Player?.Character is TheMiddleNurseFatherOutisCode.Character.TheMiddleNurseFatherOutis)
        {
            await cardPlay.Card.Owner.Creature.PlayAnimation("cast", 0.3f);
        }
    }
}