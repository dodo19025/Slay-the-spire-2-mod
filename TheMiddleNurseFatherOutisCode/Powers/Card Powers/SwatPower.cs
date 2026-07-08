using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;



public class SwatPower() : CustomTemporaryPowerModelWrapper<Swat,StrengthPower>
{
    private CustomTemporaryPowerModel _customTemporaryPowerModelImplementation;

    public override PowerType Type =>
        PowerType.Debuff;

    public override string CustomBigIconPath => "res://TheMiddleNurseFatherOutis/images/powers/big/swat_power.png";
    public override string CustomPackedIconPath => "res://TheMiddleNurseFatherOutis/images/powers/swat_power.png";

    public override LocString Description => new LocString("powers","THEMIDDLENURSEFATHEROUTIS-SWAT_POWER.description");
    public override LocString Title => new LocString("powers","THEMIDDLENURSEFATHEROUTIS-SWAT_POWER.title");

    protected override string SmartDescriptionLocKey => "THEMIDDLENURSEFATHEROUTIS-SWAT_POWER.smartDescription";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<StrengthPower>()
    ];


    protected override bool InvertInternalPowerAmount => true;
}