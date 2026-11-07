using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
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
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DynamicVar("GrudgeConsume", 2m),
        new DynamicVar("TotalDamage", TotalDamageCalc()),
        new CalculationBaseVar(7m),
        new ExtraDamageVar(3m),
    ]);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<GrudgePower>()
    ];

    protected override bool ShouldGlowGoldInternal =>  GrudgeResource.GetGrudge(Owner.Creature.Player!) >= DynamicVars["GrudgeConsume"].BaseValue;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int grudge = GrudgeResource.GetGrudge(Owner.Creature.Player!);
        var damage = (TotalDamageCalc());
        if (grudge >= DynamicVars["GrudgeConsume"].BaseValue)
        {
            await GrudgeResource.LoseGrudge(DynamicVars["GrudgeConsume"].IntValue, Owner);
        }
        await DamageCmd.Attack(damage).FromCard(this,play).TargetingAllOpponents(CombatState!).Execute(choiceContext);

    }

    public decimal TotalDamageCalc()
    {
        if (!IsInCombat)
        {
            return 7m;
        }
        int grudge = GrudgeResource.GetGrudge(Owner.Creature.Player!);
        var total = DynamicVars.CalculationBase.BaseValue;
        if (grudge >= DynamicVars["GrudgeConsume"].BaseValue)
        {
            total =  DynamicVars.CalculationBase.BaseValue + DynamicVars.ExtraDamage.BaseValue;
        }

        return total;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(2m);
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}