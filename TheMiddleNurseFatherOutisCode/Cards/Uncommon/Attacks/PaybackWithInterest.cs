using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

public class PaybackWithInterest()
    : TheMiddleNurseFatherOutisCard(1,
        CardType.Attack, CardRarity.Uncommon,
        TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        
    }

    protected override void OnUpgrade()
    {

    }
}