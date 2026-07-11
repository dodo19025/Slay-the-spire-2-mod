using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Localization;

public class PaybackVar : DynamicVar
{
    
    public const string Key = "Payback";

    public PaybackVar(decimal PaybackNum) : base(Key, PaybackNum)
    {
        this.WithTooltip();
    }
}