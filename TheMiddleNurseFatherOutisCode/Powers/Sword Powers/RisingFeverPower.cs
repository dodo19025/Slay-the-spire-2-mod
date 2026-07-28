using System.Diagnostics;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class RisingFeverPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DynamicVar("FeverAmount", 0m),
        new BoolVar("CanUnpack", false)
    ]);
    
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromCard<Unpacking>()
    ];

    public override int DisplayAmount => DynamicVars["FeverAmount"].IntValue;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (!((BoolVar)DynamicVars["CanUnpack"]).BoolVal)
        {
            MainFile.Logger.Info("Detected that unpack is false, setting to true");
            ((BoolVar)DynamicVars["CanUnpack"]).BoolVal = true;
        }
        
        foreach (CardModel? cardModel in Owner.Player.PlayerCombatState!.Hand.Cards)
        {
            //to catch if the player ever has the unpacking cards and hasn't used them
            if (cardModel is Unpacking or Unpacking2 or Unpacking3)
            {
                return;
            }
        }
        
        
        PowerModel? swordStage = TheMiddleNursefatherOutisCmd.getSwordStage(player.Creature);
        
        if (swordStage == null || swordStage is LaevateinnPower || !(TheMiddleNursefatherOutisCmd.CanUnpack(choiceContext, player.Creature)))
        {
            return;
        }

        switch (swordStage)
        {
            case SealedSwordPower:
                CardModel? unpacking = Owner?.CombatState?.CreateCard<Unpacking>(Owner.Player);
                await CardPileCmd.AddGeneratedCardToCombat(unpacking, PileType.Hand, Owner.Player);
                return;
            case FirstSealRemovedPower:
                CardModel? unpacking2 = Owner?.CombatState?.CreateCard<Unpacking2>(Owner.Player);
                await CardPileCmd.AddGeneratedCardToCombat(unpacking2, PileType.Hand, Owner.Player);
                return;
            case SecondSealRemovedPower:
                CardModel? unpacking3 = Owner?.CombatState?.CreateCard<Unpacking3>(Owner.Player);
                await CardPileCmd.AddGeneratedCardToCombat(unpacking3, PileType.Hand, Owner.Player);
                return;
        }
        
    }

    //reason this is cardplayed late is to check for powers and such
    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }

        PowerModel? swordStage = TheMiddleNursefatherOutisCmd.getSwordStage(cardPlay.Card.Owner.Creature);
        
        if (swordStage == null || swordStage is LaevateinnPower || !(TheMiddleNursefatherOutisCmd.CanUnpack(choiceContext, cardPlay.Card.Owner.Creature)))
        {
            //MainFile.Logger.Info("Detected that the player can't unpack from 'rising fever' function");
            return;
        }
        //MainFile.Logger.Info("Detected that the player can unpack from 'rising fever' function");
        foreach (CardModel? cardModel in cardPlay.Card.Owner.PlayerCombatState!.Hand.Cards)
        {
            //to catch if the player ever has the unpacking cards and hasn't used them
            if (cardModel is Unpacking or Unpacking2 or Unpacking3)
            {
                return;
            }
        }
        switch (swordStage)
        {
            case SealedSwordPower:
                CardModel? unpacking = Owner?.CombatState?.CreateCard<Unpacking>(Owner.Player);
                await CardPileCmd.AddGeneratedCardToCombat(unpacking, PileType.Hand, Owner.Player);
                return;
            case FirstSealRemovedPower:
                CardModel? unpacking2 = Owner?.CombatState?.CreateCard<Unpacking2>(Owner.Player);
                await CardPileCmd.AddGeneratedCardToCombat(unpacking2, PileType.Hand, Owner.Player);
                return;
            case SecondSealRemovedPower:
                CardModel? unpacking3 = Owner?.CombatState?.CreateCard<Unpacking3>(Owner.Player);
                await CardPileCmd.AddGeneratedCardToCombat(unpacking3, PileType.Hand, Owner.Player);
                return;
        }
    }
}