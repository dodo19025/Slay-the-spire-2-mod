using BaseLib.Extensions;
using BaseLib.Utils;
using HarmonyLib;
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


public class DontBreakNow() : TheMiddleNurseFatherOutisCard(
    2, CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Swing
    ];
    
    
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<LaevateinnPower>();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(15m,ValueProp.Move),
        new DynamicVar("PowerPower",1m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)

    ];
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this,play.Target).Execute(choiceContext);
        int power_apply = TheMiddleNursefatherOutisCmd.getSwordStagePower(Owner.Creature);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, power_apply, Owner.Creature, this);
        if (TheMiddleNursefatherOutisCmd.getSwordStage(Owner.Creature) is LaevateinnPower)
        {
            await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, power_apply, Owner.Creature, this);
            await CardCmd.Exhaust(choiceContext, this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);

    }
}