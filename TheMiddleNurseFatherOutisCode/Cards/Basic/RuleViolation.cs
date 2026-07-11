using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
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

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("PaybackAmount",3m)
    ];

protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<PaybackPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromPower<StrengthPower>()
    ];
    
    
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int AdditionalPayback = 0;
        if (Owner.Creature.HasPower<StrengthPower>())
        {
            if (Owner.Creature.GetPowerAmount<StrengthPower>() > 0)  //haha glad i caught that one! right guys...?
            {
                AdditionalPayback += Owner.Creature.GetPowerAmount<StrengthPower>();
            }
        }
        await Owner.Creature.GainPayback(choiceContext , base.DynamicVars["PaybackAmount"].BaseValue,base.Owner.Creature, this);
        //await Owner.Creature.AdditionalPayback(choiceContext ,AdditionalPayback,Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PaybackAmount"].UpgradeValueBy(2m);
    }
   // new CalculationBaseVar(3m),
   // new CalculationExtraVar(1m),
    //new CalculatedVar("CalculatedPaybackAmount").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, target) => 
    //card.Owner.Creature.GetPowerAmount<StrengthPower>()))
}