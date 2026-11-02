using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


public class RightPlexus()
    : TheMiddleNurseFatherOutisCard(2,
        CardType.Attack, CardRarity.Common,
        TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Punch
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BleedPower>(),
        HoverTipFactory.FromPower<GrudgePower>()
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DamageVar(12m, ValueProp.Move),
        new DynamicVar("GrudgeGained", 3m),
        new DynamicVar("BleedPower", 2m),
        new EnergyVar(1)
    ]);
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Owner.Creature.LastCard(play);
        MainFile.Logger.Info($"Last card played: {play}");

    }

    protected override void OnUpgrade()
    {

    }


}