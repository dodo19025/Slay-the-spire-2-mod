using BaseLib.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Localization;

public class SustainVar : DynamicVar
{
    public const string Key = "Sustain";
    
    public SustainVar(Decimal SustainCount) : base(Key, SustainCount)
    {
        this.WithTooltip();
    }
    
}