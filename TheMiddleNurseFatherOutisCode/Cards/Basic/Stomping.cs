using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Localization;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class Stomping() : TheMiddleNurseFatherOutisCard(
    0, CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DamageVar(3m, ValueProp.Move),
        new DynamicVar("Exclamation", 1m),
        new DynamicVar("AttackThreshold",3m),
        new DynamicVar("RisingFeverLost",1m),
    ]);



    protected override bool ShouldGlowGoldInternal => PlayedEnoughCards();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<RisingFeverPower>()
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        if (PlayedEnoughCards())
        {
            await TheMiddleNursefatherOutisCmd.ChangeFeverAmount(choiceContext, Owner.Creature, -1m, false);
        }
        else
        {
            //so if the player did not meet the conditions first
            int NumberOfCardsPlayed = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) =>
                e.HappenedThisTurn(this.CombatState) && e.CardPlay.Card.Type == CardType.Attack &&
                e.CardPlay.Card.Owner == Owner);
            int amountOfCardsLeftToPlay = (int)DynamicVars["AttackThreshold"].BaseValue - NumberOfCardsPlayed;
            await PowerCmd.Apply<StompingUnseal>(choiceContext, Owner.Creature, (decimal)amountOfCardsLeftToPlay,
                Owner.Creature, this);
        }
    }

    public bool PlayedEnoughCards()
    {
        int NumberOfCardsPlayed = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) =>
            e.HappenedThisTurn(this.CombatState) && e.CardPlay.Card.Type == CardType.Attack &&
            e.CardPlay.Card.Owner == Owner);
        if (NumberOfCardsPlayed + 1>= DynamicVars["AttackThreshold"].BaseValue)
        {
            return true;
        }
        return false;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
        
    }
}