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

public class RightintheGut() : TheMiddleNurseFatherOutisCard(
    1, CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Punch
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<GrudgePower>(),
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(9m, ValueProp.Move), 
        new DynamicVar("StrengthPower", 2m),
        new DynamicVar("GrudgeConsume", 2m),
        new EnergyVar(1)
    ];
    bool played = false;
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        played = true;

        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        if (GrudgeResource.GetGrudge(Owner) >= DynamicVars["GrudgeConsume"].BaseValue)
        {
            await GrudgeResource.LoseGrudge(DynamicVars["GrudgeConsume"].IntValue, Owner);
            await CommonActions.ApplySelf<RightInTheGutPower>(this, DynamicVars["StrengthPower"].BaseValue);
        }
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card == this && cardPlay.Card.Owner == Owner)
        {
            played = true;
        }
        return Task.CompletedTask;
    }

    public override Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        MainFile.Logger.Info("Detected that a card is drawn");
        if (played == false || card.Owner.Creature != Owner.Creature)
        {
            MainFile.Logger.Info("Detected that a card is drawn but not played this yet");
            return Task.CompletedTask;
        }
        MainFile.Logger.Info("Detected that a card is drawn");

        if (card != null && card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Kick))
        {
            MainFile.Logger.Info("Detected that a card is drawn but this was and it was a kick");
            card.EnergyCost.AddThisTurn(-DynamicVars.Energy.IntValue);
            played = false;
        }
        return Task.CompletedTask;
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}