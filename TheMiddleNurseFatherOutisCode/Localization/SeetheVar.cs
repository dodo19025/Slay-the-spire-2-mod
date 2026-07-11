using BaseLib.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Localization;

public class SeetheVar : DynamicVar
{
    public const string Key = "Seethe";
    
    public SeetheVar(Decimal SeetheCount) : base(Key, SeetheCount)
    {
        this.WithTooltip();
    }
}