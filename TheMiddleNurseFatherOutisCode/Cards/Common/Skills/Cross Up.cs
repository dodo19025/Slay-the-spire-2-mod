using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class CrossUp() : TheMiddleNurseFatherOutisCard(
    1, CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)

{
    
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<LaevateinnPower>();

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new BlockVar(5m, ValueProp.Move),
            new DynamicVar("BurnApply", 7m),
            new DynamicVar("AdditionalBurn",5m)
        ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BurnPower>(),
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var additionalburn = 0m;
        if (Owner.Creature.HasPower<LaevateinnPower>())
        {
            additionalburn = base.DynamicVars["AdditionalBurn"].BaseValue;
        }
        await CreatureCmd.GainBlock(base.Owner.Creature, DynamicVars.Block, play);
        await PowerCmd.Apply<BurnPower>(choiceContext, play.Target, base.DynamicVars["BurnApply"].BaseValue + additionalburn,
            base.Owner.Creature, this);

    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(2m);
        base.DynamicVars["BurnApply"].UpgradeValueBy(2m);
    }
}