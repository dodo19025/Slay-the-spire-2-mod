using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Localization;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class IsItYou() : TheMiddleNurseFatherOutisCard(
    1, CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("GrudgeGained",3m),
        new DynamicVar("PaybackGained",4m)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<GrudgePower>(),
        HoverTipFactory.FromPower<PaybackPower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<GrudgePower>(choiceContext,base.Owner.Creature,base.DynamicVars["GrudgeGained"].BaseValue,base.Owner.Creature,this);
        await PowerCmd.Apply<PaybackPower>(choiceContext,base.Owner.Creature,base.DynamicVars["PaybackGained"].BaseValue,base.Owner.Creature,this);


    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["GrudgeGained"].UpgradeValueBy(1m);
        this.DynamicVars["PaybackGained"].UpgradeValueBy(3m);
    }
}