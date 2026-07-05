using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class FirstSealRemovedPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    private const int _baseCardsLeft = 2; //change to 5 when final

    private bool firsttimegained = true;
    
    private const string _cardsLeftKey = "CardsLeft";

    public override int DisplayAmount => base.DynamicVars["CardsLeft"].IntValue;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("CardsLeft", 2m)]; //change to 5 when final
    

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == base.Owner.Player && cardPlay.Card.Type == CardType.Attack)
        {
            if (!firsttimegained)
            {
                base.DynamicVars["CardsLeft"].BaseValue--;
            }

            firsttimegained = false;
            InvokeDisplayAmountChanged();
            if (base.DynamicVars["CardsLeft"].BaseValue <= 0)
            {
                Flash();
                await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner.Player);
                base.DynamicVars["CardsLeft"].BaseValue = _baseCardsLeft;
                InvokeDisplayAmountChanged();
            }
        }
    }
}