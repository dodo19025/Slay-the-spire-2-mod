using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
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
        CardType.Attack, CardRarity.Uncommon,
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
        new DamageVar(8m,ValueProp.Move),
        new DynamicVar("BurnApply", 7m),
        new DynamicVar("BurnApplyCount",1m)

    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this,play).TargetingAllOpponents(base.CombatState).Execute(choiceContext);
        
        int ApplyCount = (int)base.DynamicVars["BurnApplyCount"].BaseValue;
        if (Owner.Creature.HasPower<LaevateinnPower>())
        {
            ApplyCount++;
        }

        for (int i = 0; i < ApplyCount; i++)
        {
            foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<BurnPower>(choiceContext, hittableEnemy, base.DynamicVars["BurnApply"].BaseValue, base.Owner.Creature, this);
            }
        }

    }

    protected override void OnUpgrade()
    {
         base.DynamicVars.Damage.UpgradeValueBy(3m);
         base.DynamicVars["BurnApply"].UpgradeValueBy(2m);
    }
}