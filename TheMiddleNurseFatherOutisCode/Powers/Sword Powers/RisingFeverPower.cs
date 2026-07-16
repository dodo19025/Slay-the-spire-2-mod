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
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[3]
    {
        new DynamicVar("RisingFeverReapply", 2m), 
        new DynamicVar("StrengthGain", 3m),
        new DynamicVar("CardsLeft",2m)
    });

 

    public const string _cardsLeftKey = "CardsLeft";

    public class Data
    {
        public int FeverCardsPlayed;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != base.Owner.Player || cardPlay.Card.Owner.HasPower<LaevateinnPower>())
        {
            return;
        }
        if(cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Fervour))
        {
            base.DynamicVars["CardsLeft"].BaseValue--;

        }
        if (base.DynamicVars["CardsLeft"].BaseValue <= 0)
        {
            await Cmd.Wait(0.25f);
            base.DynamicVars["CardsLeft"].BaseValue = 2m;
            await PowerCmd.Decrement(this);
        }
    }
}