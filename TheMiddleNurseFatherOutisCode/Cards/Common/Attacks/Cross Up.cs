using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class CrossUp() : TheMiddleNurseFatherOutisCard(
    1, CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)

{


    protected override bool ShouldGlowGoldInternal => FirstAttackPlayed();

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Kick
    ];
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new DamageVar(4m, ValueProp.Move),
            new EnergyVar(1),
            new CardsVar(1),
        ];
    
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this,play.Target).Execute(choiceContext);
        if (FirstAttackPlayed())
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    private bool FirstAttackPlayed()
    {
        int NumberOfCardsPlayed = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) =>
            e.HappenedThisTurn(this.CombatState) && e.CardPlay.Card.Type == CardType.Attack &&
            e.CardPlay.Card.Owner == Owner);
        if (NumberOfCardsPlayed <= 0)
        {
            return true;
        }
        return false;
    }
}