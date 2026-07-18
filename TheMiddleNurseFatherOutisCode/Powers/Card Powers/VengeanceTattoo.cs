using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;


public class VengeanceTattoo()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DynamicVar("MaxAmount", 4m),
        new DynamicVar("DamageReduction",1m),
    ]); //Made for values to be easily changable

}