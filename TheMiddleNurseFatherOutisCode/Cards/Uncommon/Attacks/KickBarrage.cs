using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class KickBarrage()
    : TheMiddleNurseFatherOutisCard(1,
        CardType.Attack, CardRarity.Uncommon,
        TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(10m,ValueProp.Move),
        new RepeatVar(1),
        new CalculationBaseVar(0),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedHits").WithMultiplier((CardModel card, Creature? _) => 1 + CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.HappenedThisTurn(card.CombatState) && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Tags.Contains(TheMiddleNurseFatherOutisTags.Kick) && e.CardPlay.Card.Owner == card.Owner))
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .WithHitCount((int)((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(play.Target))
            .Targeting(play.Target)
            .FromCard(this, play)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}