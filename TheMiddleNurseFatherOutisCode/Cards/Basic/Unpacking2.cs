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
    0, CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
 protected override bool IsPlayable => TheMiddleNursefatherOutisCmd.CanUnpack(new BlockingPlayerChoiceContext(),base.Owner.Creature);

    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DamageVar(4m, ValueProp.Move),
        new DynamicVar("BleedPower", 3m),
        new CardsVar(1)
    ]);

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain,
        CardKeyword.Exhaust
       //TheMiddleNurseFatherOutisKeywords.Fervour
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BleedPower>(),
    ];

    

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!Owner.HasPower<FirstSealRemovedPower>())
        {
            return;
        }
        if (TheMiddleNursefatherOutisCmd.CanUnpack(choiceContext,Owner.Creature) && !(Owner.Creature.HasPower<LaevateinnPower>()))
        {
            MainFile.Logger.Info("autoplaying card");
            await CardCmd.AutoPlay(choiceContext,this,null); 
        } 
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (!Owner.HasPower<FirstSealRemovedPower>())
        {
            return;
        }
        
        if (TheMiddleNursefatherOutisCmd.CanUnpack(choiceContext,Owner.Creature) && !(Owner.Creature.HasPower<LaevateinnPower>()))
        {
            MainFile.Logger.Info("autoplaying card");
            await CardCmd.AutoPlay(choiceContext,this,null); 
        } 
    }


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    { 
        
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await PowerCmd.Apply<BleedPower>(choiceContext, play.Target, DynamicVars["BleedPower"].BaseValue,
            Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);

        if (play.Card.Owner.Creature.Player?.Character is Character.TheMiddleNurseFatherOutis)
        {
            await Owner.Creature.ChangeSwordSeal(choiceContext);
            CardModel Card = Owner?.Creature?.CombatState?.CreateCard<Unpacking3>(Owner.Creature.Player);
            await CardPileCmd.AddGeneratedCardToCombat(Card, PileType.Hand, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
        DynamicVars["BleedPower"].UpgradeValueBy(1m);
    }
}

