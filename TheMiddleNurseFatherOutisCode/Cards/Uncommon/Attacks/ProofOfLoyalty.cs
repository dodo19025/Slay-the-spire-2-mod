using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers.Combo_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class ProofOfLoyalty()
    : TheMiddleNurseFatherOutisCard(1,
        CardType.Attack, CardRarity.Uncommon,
        TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(10m, ValueProp.Move),
        new DynamicVar("GrudgeGain", 4m),
        new DynamicVar("HPLost", 2m),
        new DynamicVar("StrengthPower", 1m)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<GrudgePower>(),
        HoverTipFactory.FromPower<VengeanceTattoo>(),
        HoverTipFactory.FromPower<StrengthPower>()
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Punch
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this,play.Target).Execute(choiceContext);
        await PowerCmd.Apply<ProofOfLoyaltyCombo>(choiceContext, Owner.Creature, 1m , Owner.Creature, this);

    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}