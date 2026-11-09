using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
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


public class PromisedSuffering()
    : TheMiddleNurseFatherOutisCard(2,
        CardType.Attack, CardRarity.Uncommon,
        TargetType.AnyEnemy)


{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Kick
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<GrudgePower>(),
        HoverTipFactory.FromPower<PaybackPower>()
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(14m, ValueProp.Move),
        new DynamicVar("PaybackGain",5m),
        new DynamicVar("GrudgeConsume", 3m)
    ];

    private bool ReturnCard;
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await CommonActions.ApplySelf<PaybackPower>(this, DynamicVars["PaybackGain"].BaseValue);
        if (GrudgeResource.GetGrudge(Owner) <= DynamicVars["GrudgeConsume"].BaseValue)
        {
            GrudgeResource.LoseGrudge(DynamicVars["GrudgeConsume"].IntValue,Owner);
            ReturnCard = true;
        }
        
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Pile.Type == PileType.Discard && ReturnCard)
        {
            ReturnCard = false;
            CardPileCmd.Add(this, PileType.Hand);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PaybackGain"].UpgradeValueBy(3m);
        DynamicVars["GrudgeConsume"].UpgradeValueBy(-1m);
    }
}