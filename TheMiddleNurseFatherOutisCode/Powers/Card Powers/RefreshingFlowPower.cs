using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;



public class RefreshingFlowPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromCard<Wound>(),
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new EnergyVar(1),
    ];

    bool punchdone = false;
    bool kickdone = false;
    bool swingdone = false;
    bool gotenergy = false;
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }

        if (cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Punch) && punchdone == false)
        {
            punchdone = true;
            gotenergy = true;
        }
        else if (cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Kick) && kickdone == false)
        {
            kickdone = true;
            gotenergy = true;
        }
        else if (cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Swing) && swingdone == false)
        {
            swingdone = true;
            gotenergy = true;
        }

        if (gotenergy)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner.Player);
            Amount--;
            
            gotenergy = false;
            CardModel card = CombatState.CreateCard<Burn>(Owner.Player);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Discard, Owner.Player));
        }
    }
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.HasPower<RefreshingFlowPower>())
        {
            await PowerCmd.Remove(this);
        }
    }
}