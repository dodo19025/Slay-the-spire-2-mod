using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers.Combo_Powers;


public class PlexusCombo()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new EnergyVar(1)
    ];
    

    private bool Nextcard = false;
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Nextcard == false)
        {
            Nextcard = true;
            return;
        }
        if (cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Swing) && cardPlay.Card.Owner == Owner.Player)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, cardPlay.Card.Owner);
        }
        await PowerCmd.Remove(this);
    }
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.HasPower<WriteSinsPower>())
        {
            await PowerCmd.Remove(this);
        }
    }
}