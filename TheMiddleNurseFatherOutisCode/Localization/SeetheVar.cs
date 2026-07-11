using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Localization;

public class SeetheVar : DynamicVar
{
    public const string Key = "Seethe";

    public SeetheVar(decimal SeetheCount) : base(Key, SeetheCount)
    {
        this.WithTooltip();
    }
}