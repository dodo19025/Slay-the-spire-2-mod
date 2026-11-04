using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Hooks;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class Retaliation() : TheMiddleNurseFatherOutisCard(
    2, CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<LaevateinnPower>();
    
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BurnPower>(),
        HoverTipFactory.FromPower<BleedPower>(),
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new BlockVar(9, ValueProp.Move),
        new DynamicVar("RetaliatePower",25)
    ];


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<RetaliatePower>(choiceContext, Owner.Creature, base.DynamicVars["RetaliatePower"].BaseValue,
            base.Owner.Creature, this);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);

    }
    

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
        //base.DynamicVars["BurnApply"].UpgradeValueBy(3m);
        //base.DynamicVars["SelfBurn"].UpgradeValueBy(-2m);

    }
}