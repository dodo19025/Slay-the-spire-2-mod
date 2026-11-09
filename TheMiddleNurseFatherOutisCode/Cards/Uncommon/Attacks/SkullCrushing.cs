using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class SkullCrushing() : TheMiddleNurseFatherOutisCard(
    1, CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Kick
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<GrudgePower>(),
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(7m, ValueProp.Move), 
        new DynamicVar("PaybackGain", 4m),
        new DynamicVar("ExtraPayback", 2m),
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await CommonActions.ApplySelf<PaybackPower>(this, DynamicVars["PaybackGain"].BaseValue);
        await CommonActions.Apply<SkullCrushingPower>(play.Target,this,DynamicVars["ExtraPayback"].BaseValue);
    }
    

    protected override void OnUpgrade()
    {
        DynamicVars["PaybackGain"].UpgradeValueBy(1m);
        DynamicVars["ExtraPayback"].UpgradeValueBy(1m);
    }
}