using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
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
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BurnPower>(),
        HoverTipFactory.FromPower<BleedPower>()
    ];
    
    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (dealer != base.Owner)
        {
            return 0m;
        }
        if (target == base.Owner)
        {
            return 0m;
        }
        if (!props.IsPoweredAttack())
        {
            return 0m;
        }
        if (target != null && target.HasPower<BleedPower>())
        {
            // ReSharper disable once PossibleLossOfFraction
            MainFile.Logger.Info($"Additional damage per 4 bleed--> { (Math.Round((decimal)base.DynamicVars["AdditionalDamagePerBleed"].BaseValue)*(target.GetPowerAmount<BleedPower>() / 4))}");
            return (Math.Round((decimal)(base.DynamicVars["AdditionalDamagePerBleed"].BaseValue) * (target.GetPowerAmount<BleedPower>() / base.DynamicVars["BleedThreshold"].BaseValue)));
        }
        return 0m;
    }
}