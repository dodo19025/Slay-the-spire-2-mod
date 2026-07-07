using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]
public class MiddleStrike()
    : TheMiddleNurseFatherOutisCard(1,
        CardType.Attack, CardRarity.Basic,
        TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<BleedPower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
    }



    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}