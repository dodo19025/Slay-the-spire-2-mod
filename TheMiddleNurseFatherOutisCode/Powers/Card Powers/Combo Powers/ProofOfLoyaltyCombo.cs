using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers.Combo_Powers;


public class ProofOfLoyaltyCombo()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("GrudgeGain", 4m),
        new DynamicVar("HPLost", 2m),
        new DynamicVar("StrengthPower", 1m)
    ];
    private bool Nextcard = false;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Nextcard == false)
        {
            Nextcard = true;
            return;
        }
        if (cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Kick) && cardPlay.Card.Owner == Owner.Player)
        {
            await GrudgeResource.GainGrudge(DynamicVars["GrudgeGain"].IntValue,Owner.Player);
        }
        else if (cardPlay.Card.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Swing) && cardPlay.Card.Owner == Owner.Player)
        {
            await CreatureCmd.Damage(choiceContext, Owner, DynamicVars["HPLost"].BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, Owner);
            await PowerCmd.Apply<VengeanceTattoo>(choiceContext, Owner, DynamicVars["StrengthPower"].BaseValue, Owner, null);
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, DynamicVars["StrengthPower"].BaseValue, Owner, null);
        }
        await PowerCmd.Remove(this);
    }
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.HasPower<ProofOfLoyaltyCombo>())
        {
            await PowerCmd.Remove(this);
        }
    }
}