using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;


public class Unpacking2() : TheMiddleNurseFatherOutisCard(
    2, CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
 protected override bool IsPlayable => TheMiddleNursefatherOutisCmd.CanUnpack(new BlockingPlayerChoiceContext(),Owner.Creature);

    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DamageVar(4m, ValueProp.Move),
        new DynamicVar("BleedPower", 3m),
        new CardsVar(1)
    ]);

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain,
        CardKeyword.Exhaust,
        TheMiddleNurseFatherOutisKeywords.Swing
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BleedPower>(),
    ];
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    { 
        
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await TheMiddleNursefatherOutisCmd.CardApplyBleed(choiceContext, Owner.Creature,
            DynamicVars["BleedPower"].BaseValue, play.Target, play.Card);

        if (play.Card.Owner.Creature.Player?.Character is Character.TheMiddleNurseFatherOutis)
        {
            await Owner.Creature.ChangeSwordSeal(choiceContext);
        }
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        EnergyCost.AddThisCombat(-1);
        return Task.CompletedTask;
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
        DynamicVars["BleedPower"].UpgradeValueBy(1m);
    }
}

