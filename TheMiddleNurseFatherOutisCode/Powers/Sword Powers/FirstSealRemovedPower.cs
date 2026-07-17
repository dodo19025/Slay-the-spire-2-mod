using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class FirstSealRemovedPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("BurnApplicationValue", 1m),
        new DynamicVar("AdditionalDamagePerBleed", 1m),
        new DynamicVar("BleedThreshold",4m)
    ];

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    private class Data
    {
        public int TargetOldleed;

        public int TargetNewBleed;
    }
    public override async Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target,
        Creature? applier,
        CardModel? cardSource)
    {
        Data data = GetInternalData<Data>();
    }
    
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        Data data = GetInternalData<Data>();
        if (cardPlay.Card.Owner.Creature != base.Owner)
        {
            return Task.CompletedTask;
        }
        if (cardPlay.Target != null && cardPlay.Target.HasPower<BleedPower>())
        {
            data.TargetOldleed = cardPlay.Target.GetPowerAmount<BleedPower>();
            MainFile.Logger.Info($"$Target Old Bleed,detected target has bleed --> {data.TargetOldleed}");
            return Task.CompletedTask;
        } 
        if (cardPlay?.Target != null && !(cardPlay.Target.HasPower<BleedPower>()))
        {
            data.TargetOldleed = 0;
            MainFile.Logger.Info($"$Target Old Bleed --> {data.TargetOldleed}");
            return Task.CompletedTask;
        }
        return Task.CompletedTask;    
    }
    

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Data data = GetInternalData<Data>();
        data.TargetNewBleed = 0;
        if (cardPlay?.Target != null && cardPlay.Target.HasPower<BleedPower>())
        {
            data.TargetNewBleed = cardPlay.Target.GetPowerAmount<BleedPower>();
        }
        MainFile.Logger.Info($"$Target New Bleed --> {data.TargetNewBleed}");

        if (data.TargetNewBleed > data.TargetOldleed)
        {
            await PowerCmd.Apply<BurnPower>(choiceContext, cardPlay.Target,
                base.DynamicVars["BurnApplicationValue"].BaseValue, cardPlay.Card.Owner.Creature, cardPlay.Card);
        }
    }
}