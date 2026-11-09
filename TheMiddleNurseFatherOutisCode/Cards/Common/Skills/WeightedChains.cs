using BaseLib.Utils;
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
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;
[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class WeightedChains() : TheMiddleNurseFatherOutisCard(
    1, CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DynamicVar("BleedPower", 1m),
        new DynamicVar("SelfBleedPower", 2m),
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BleedPower>(),
        HoverTipFactory.FromPower<BurnPower>()
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<BleedPower>(choiceContext, Owner.Creature, base.DynamicVars["SelfBleedPower"].BaseValue,base.Owner.Creature,this);
        await PowerCmd.Apply<BleedNextAttackPower>(choiceContext, Owner.Creature, base.DynamicVars["BleedPower"].BaseValue,base.Owner.Creature,this);

    }
    
    protected override void OnUpgrade()
    { 
        DynamicVars["SelfBleedPower"].UpgradeValueBy(1m);
    }
}