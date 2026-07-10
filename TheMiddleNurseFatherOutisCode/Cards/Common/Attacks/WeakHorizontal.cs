using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class WeakHorizontal()
    : TheMiddleNurseFatherOutisCard(2,
        CardType.Attack, CardRarity.Common,
        TargetType.AllEnemies)
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
        new DamageVar(10m,ValueProp.Move),
        new DynamicVar("BurnApply", 7m)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).TargetingAllOpponents(base.CombatState).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
         base.DynamicVars.Damage.UpgradeValueBy(4m);
         base.DynamicVars["BurnApply"].UpgradeValueBy(5m);
    }
}