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
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class ToweringSilhouette() : TheMiddleNurseFatherOutisCard(
    2, CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new BlockVar(6m, ValueProp.Move),
        new DynamicVar("MoreBlock", 7m),
        new DynamicVar("GrudgeConsume", 3m)

    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.Static(StaticHoverTip.Block),
    ];

    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        if (GrudgeResource.GetGrudge(Owner.Creature.Player!) >= DynamicVars["GrudgeConsume"].BaseValue)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars["MoreBlock"].BaseValue,ValueProp.Move, play);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(1m);
        DynamicVars["MoreBlock"].UpgradeValueBy(3m);

    }
}