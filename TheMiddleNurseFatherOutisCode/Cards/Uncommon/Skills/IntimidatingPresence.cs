using BaseLib.Extensions;
using BaseLib.Utils;
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
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class IntimidatingPresence() : TheMiddleNurseFatherOutisCard(
    2, CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("WeakPower", 1m),
        new DynamicVar("PaybackPowerIncrease", 50m),
        new DynamicVar("PaybackToBlockConversion", 25m)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var PresentagePayback = DynamicVars["PaybackPowerIncrease"].BaseValue / 100;
        var PresentageBlock = DynamicVars["PaybackToBlockConversion"].BaseValue / 100;
        if (Owner.Creature.HasPower<PaybackPower>())
        {
            await CommonActions.ApplySelf<PaybackPower>(this,
                (PresentagePayback * Owner.Creature.GetPowerAmount<PaybackPower>()));
            PowerModel? PaybackPower = Owner.Creature.GetPower<PaybackPower>();
            ((BoolVar)PaybackPower.DynamicVars["LosePayback"]).BoolVal = false;
            await CreatureCmd.GainBlock(Owner.Creature, (PresentageBlock * Owner.Creature.GetPowerAmount<PaybackPower>()), ValueProp.Move,play);
        }
        await CommonActions.Apply<WeakPower>(play.Target, this, DynamicVars["WeakPower"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PaybackPowerIncrease"].UpgradeValueBy(25m);
    }
}