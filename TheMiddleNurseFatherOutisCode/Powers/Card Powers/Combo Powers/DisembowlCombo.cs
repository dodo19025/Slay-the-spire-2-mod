using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers.Combo_Powers;


public class DisembowlCombo()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    
    

    private int Nextcard = 0;
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Nextcard == 0)
        {
            Nextcard++;
            return;
        }
        
        if (cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Punch) && cardPlay.Card.Owner == Owner.Player && Nextcard == 1)
        {
            await PlayerCmd.GainEnergy(Amount, cardPlay.Card.Owner);
            Nextcard++;
            return;
        }

        if (cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Swing) &&
            cardPlay.Card.Owner == Owner.Player && Nextcard == 2)
        {
            await CommonActions.ApplySelf<VengeanceTattoo>(null, 1m);
            await CommonActions.ApplySelf<StrengthPower>(null, 1m);
            Nextcard++;
        }

        if (Nextcard == 2)
        {
            return;
        }
        await PowerCmd.Remove(this);
    }
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.HasPower<DisembowlCombo>())
        {
            await PowerCmd.Remove(this);
        }
    }
}