using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class BadAssAssKicking()
    : TheMiddleNurseFatherOutisCard(1,
        CardType.Attack, CardRarity.Common,
        TargetType.AnyEnemy)
{
    
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { TheMiddleNurseFatherOutisTags.Kick };

    
        
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<PaybackPower>()
    ];

    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(3m,ValueProp.Move),
        new RepeatVar(1),
        new CalculationBaseVar(0),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedHits").WithMultiplier((CardModel card, Creature? _) => 1 + PaybackPower.PaybackAcitvated[base.Owner.PlayerCombatState]),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this,play.Target).Execute(choiceContext);
        Owner.Creature.AdditionalPayback(choiceContext,base.DynamicVars["PaybackAmount"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}