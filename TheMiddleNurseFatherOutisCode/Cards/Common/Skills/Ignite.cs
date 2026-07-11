using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
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


public class Ignite() : TheMiddleNurseFatherOutisCard(
    1, CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<LaevateinnPower>();

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Fervour
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BurnPower>(),
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("BurnApply", 8m),
    ];


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<BurnPower>(choiceContext, play.Target, base.DynamicVars["BurnApply"].BaseValue,
            base.Owner.Creature, this);
        if (Owner.Creature.HasPower<LaevateinnPower>())
        {
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "swordattack", 0.05f); //animation for sword since this is classified as a skill
            await CreatureCmd.Damage(choiceContext, play.Target, base.DynamicVars["BurnApply"].BaseValue, ValueProp.Move,
                base.Owner.Creature, this, play);
            
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BurnApply"].UpgradeValueBy(4m);

    }
}