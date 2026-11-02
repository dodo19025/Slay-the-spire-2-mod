using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;
[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]
public class Punting() : TheMiddleNurseFatherOutisCard(
    1, CardType.Attack, CardRarity.Common,
    TargetType.AllEnemies)
{
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Kick
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[3]
    {
        new DamageVar(11m, ValueProp.Move),
        new DynamicVar("DamageIncrease", 3m),
        new DynamicVar("GrudgeConsume", 2m)
    });
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromPower<GrudgePower>()
    ];


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        decimal damage = base.DynamicVars.Damage.BaseValue;
        int grudge = GrudgeResource.GetGrudge(Owner.Creature.Player!);
        if (grudge >= DynamicVars["GrudgeConsume"].BaseValue)
        {
            damage += DynamicVars["DamageIncrease"].BaseValue;
        }
        await DamageCmd.Attack(damage).FromCard(this,play).TargetingAllOpponents(base.CombatState).Execute(choiceContext);

    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["DamageIncrease"].UpgradeValueBy(1m);
    }
}