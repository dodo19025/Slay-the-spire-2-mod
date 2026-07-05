using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class RuleViolation()
    : TheMiddleNurseFatherOutisCard(0,
        CardType.Skill, CardRarity.Basic,
        TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new("PaybackAmount", 3)
        ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<PaybackPower>(),
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int AdditionalPayback = 0;
        if (Owner.Creature.HasPower<StrengthPower>())
        {
            AdditionalPayback += Owner.Creature.GetPowerAmount<StrengthPower>();
        }
        Owner.Creature.GainPayback(choiceContext ,base.DynamicVars["PaybackAmount"].BaseValue,Owner.Creature, this);
        Owner.Creature.AdditionalPayback(choiceContext ,AdditionalPayback,Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PaybackAmount"].UpgradeValueBy(2);
    }
}