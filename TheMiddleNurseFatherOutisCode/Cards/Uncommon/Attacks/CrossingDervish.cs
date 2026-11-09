using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class CrossingDervish()
    : TheMiddleNurseFatherOutisCard(1,
        CardType.Attack, CardRarity.Uncommon,
        TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Swing
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BleedPower>(),
        HoverTipFactory.FromPower<VengeanceTattoo>()
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new ExtraDamageVar(3m),
        new CalculationBaseVar(3m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel c, Creature? _) =>
        {
            if (c.Owner.Creature.GetPowerAmount<VengeanceTattoo>() <= 0)
            {
                return 1m;
            }
            return 1 + c.Owner.Creature.GetPowerAmount<VengeanceTattoo>();
        }),
        new CalculationExtraVar(2m),
        new CalculatedVar("CalculatedBleed").WithMultiplier((CardModel c, Creature? _) => c.Owner.Creature.GetPowerAmount<VengeanceTattoo>()),
    ];
    
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var damage = ((CalculatedDamageVar)DynamicVars["CalculatedDamage"]).Calculate(Owner.Creature);
        await CommonActions.CardAttack(this, play.Target, damage)
            .Execute(choiceContext);
        await TheMiddleNursefatherOutisCmd.CardApplyBleed(choiceContext, Owner.Creature,
            ((CalculatedVar)DynamicVars["CalculatedBleed"]).Calculate(Owner.Creature), play.Target, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
        DynamicVars.CalculationExtra.UpgradeValueBy(1m);
    }
}