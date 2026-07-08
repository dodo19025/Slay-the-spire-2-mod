using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;


public class SwatPower() : CustomTemporaryPowerModelWrapper<Swat,StrengthPower>
{
    private CustomTemporaryPowerModel _customTemporaryPowerModelImplementation;

    public override PowerType Type =>
        PowerType.Debuff;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<StrengthPower>()
    ];


    protected override bool InvertInternalPowerAmount => true;
}