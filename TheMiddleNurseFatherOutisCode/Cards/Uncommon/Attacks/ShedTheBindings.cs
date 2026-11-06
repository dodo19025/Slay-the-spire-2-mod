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
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class ShedTheBindings() : TheMiddleNurseFatherOutisCard(
    3, CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    

    

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(9m, ValueProp.Move),
        new DynamicVar("GrudgeGained", 3m),
        new EnergyVar(1)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Swing
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<GrudgePower>(),
        HoverTipFactory.FromPower<RisingFeverPower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this,play.Target).Execute(choiceContext);
        await GrudgeResource.GainGrudge(DynamicVars["GrudgeGained"].IntValue,Owner.Creature.Player!);
        await TheMiddleNursefatherOutisCmd.ChangeFeverAmount(choiceContext, Owner.Creature, -1);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner || cardPlay.Card.Type != CardType.Attack)
        {
            return;
        }
        if (cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Kick) ||
            cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Punch))
        {
            EnergyCost.AddThisTurn(1);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars["GrudgeGained"].UpgradeValueBy(2m);
    }
}