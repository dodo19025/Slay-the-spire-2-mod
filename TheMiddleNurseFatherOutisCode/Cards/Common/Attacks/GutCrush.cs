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
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class GutCrush() : TheMiddleNurseFatherOutisCard(
    2, CardType.Attack, CardRarity.Common,
    TargetType.AllEnemies)
{
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(12m,ValueProp.Move),
        new DynamicVar("BleedPower", 4m),
        new DynamicVar("GrudgeConsume", 4m),


    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Punch
    ];
        
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BleedPower>(),
        HoverTipFactory.FromPower<GrudgePower>()

    ];

    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this,play).TargetingAllOpponents(base.CombatState).Execute(choiceContext);
        if (GrudgeResource.GetGrudge(Owner) >= DynamicVars["GrudgeConsume"].BaseValue)
        {
            await GrudgeResource.LoseGrudge(DynamicVars["GrudgeConsume"].IntValue,Owner);
            foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<BleedPower>(choiceContext, hittableEnemy, base.DynamicVars["BleedPower"].BaseValue, base.Owner.Creature, this);
            }
        }
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars["BleedPower"].UpgradeValueBy(2m);
    }
}