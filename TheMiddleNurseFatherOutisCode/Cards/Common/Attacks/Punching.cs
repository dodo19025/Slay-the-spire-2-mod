using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class Punching() : TheMiddleNurseFatherOutisCard(
    1, CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[3]
    {
        new DamageVar(7m, ValueProp.Move),
        new DynamicVar("Exclamation", 1m),
        new CardsVar(1)
    });
    
    //really no need for extra hover tips here
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        CardModel? cardModel = PileType.Draw.GetPile(base.Owner).Cards
            .Where((CardModel c) => c.Type == CardType.Attack && c.Tags.Contains(TheMiddleNurseFatherOutisTags.Kick))
            .ToList().StableShuffle(base.Owner.RunState.Rng.Shuffle).FirstOrDefault(); //i am literally confuused but okay
        if (cardModel == null) //shuffle manually
        {
            cardModel = PileType.Draw.GetPile(base.Owner).Cards
                .Where((CardModel c) =>
                    c.Type == CardType.Attack && c.Tags.Contains(TheMiddleNurseFatherOutisTags.Kick)).ToList()
                .StableShuffle(base.Owner.RunState.Rng.Shuffle).FirstOrDefault(); //does a stable shuffle, whatever that means
        }
        if (cardModel != null)
        {
            await CardPileCmd.Add(cardModel, PileType.Hand);
        }
    }
    

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);

    }
}