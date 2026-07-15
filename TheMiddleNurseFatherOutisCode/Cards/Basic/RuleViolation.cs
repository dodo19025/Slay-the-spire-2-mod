using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Localization;
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
        new SeetheVar(3m),
        new DynamicVar("TattosGained", 1m)
    ];

protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<PaybackPower>(),
        HoverTipFactory.FromPower<MiddleStyleTattoos>()

    ];
    
    
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int paybackamount = Owner.Creature.ValidPaybackAmount(base.DynamicVars["Seethe"].BaseValue);
        await Owner.Creature.GainPayback(choiceContext , this.DynamicVars["Seethe"].BaseValue,base.Owner.Creature, this);
        if (paybackamount >= base.DynamicVars["Seethe"].BaseValue)
        {
            await PowerCmd.Apply<MiddleStyleTattoos>(choiceContext,base.Owner.Creature,base.DynamicVars["TattosGained"].BaseValue,base.Owner.Creature,this);
        }
        //await Owner.Creature.AdditionalPayback(choiceContext ,AdditionalPayback,Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["Seethe"].UpgradeValueBy(2m);
    }
   // new CalculationBaseVar(3m),
   // new CalculationExtraVar(1m),
    //new CalculatedVar("CalculatedPaybackAmount").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, target) => 
    //card.Owner.Creature.GetPowerAmount<StrengthPower>()))
}