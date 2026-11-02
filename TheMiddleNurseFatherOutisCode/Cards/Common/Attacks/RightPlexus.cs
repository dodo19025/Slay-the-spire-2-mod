using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers.Combo_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


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
    
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<LaevateinnPower>();

    
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
        CardModel lastcard = Owner.Creature.LastCard(play);
        if (lastcard != null &&lastcard.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Punch))
        {
           await GrudgeResource.GainGrudge(DynamicVars["GrudgeGained"].IntValue, Owner.Creature.Player);
        }
        await CommonActions.CardAttack(this,play.Target).Execute(choiceContext);
        if (TheMiddleNursefatherOutisCmd.getSwordStage(Owner.Creature) is LaevateinnPower)
        {
            await TheMiddleNursefatherOutisCmd.CardApplyBleed(choiceContext, Owner.Creature,
                DynamicVars["BleedPower"].BaseValue, play.Target, this);
        }
        await PowerCmd.Apply<PlexusCombo>(choiceContext, Owner.Creature, 1 , Owner.Creature, this);

    }

    protected override void OnUpgrade()
    {
        DynamicVars["BleedPower"].UpgradeValueBy(2);
        DynamicVars["GrudgeGained"].UpgradeValueBy(2);
    }


}