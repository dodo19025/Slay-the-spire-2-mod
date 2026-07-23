using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis;


public static class TheMiddleNurseFatherOutisKeywords
{
    [CustomEnum,KeywordProperties(AutoKeywordPosition.After)] public static CardKeyword Fervour;

    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Kick;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Punch;


}

//handling how the keywords will work using hooks

public class ComboHandler() : CustomSingletonModel(HookType.Combat)
{
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Kick))
        {
            int grudge = GrudgeResource.GetGrudge(cardPlay.Player);
            if (grudge >= 1)
            {
                GrudgeResource.LoseGrudge(1,cardPlay.Player);
                await PowerCmd.Apply<PaybackPower>(new BlockingPlayerChoiceContext(), cardPlay.Card.Owner.Creature, 2m,
                    cardPlay.Card.Owner.Creature, cardPlay.Card);
            }
        }

        if (cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Punch))
        {
            int grudge = GrudgeResource.GetGrudge(cardPlay.Player);
            if (grudge >= 1)
            {
                GrudgeResource.LoseGrudge(1,cardPlay.Player);
                CardModel? cardModel = PileType.Draw.GetPile(cardPlay.Card.Owner).Cards
                    .Where((CardModel c) => c.Type == CardType.Skill && c.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Kick))
                    .ToList().StableShuffle(cardPlay.Card.Owner.RunState.Rng.Shuffle).FirstOrDefault(); //i am literally confuused but okay
                if (cardModel == null) //shuffle manually
                {
                    cardModel = PileType.Draw.GetPile(cardPlay.Card.Owner).Cards
                        .Where((CardModel c) =>
                            c.Type == CardType.Skill && c.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Kick)).ToList()
                        .StableShuffle(cardPlay.Card.Owner.RunState.Rng.Shuffle).FirstOrDefault(); //does a stable shuffle, whatever that means
                }
                if (cardModel != null)
                {
                    await CardPileCmd.Add(cardModel, PileType.Hand);
                }
            }
        }
    }
}