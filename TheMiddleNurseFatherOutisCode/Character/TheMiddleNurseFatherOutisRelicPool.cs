using BaseLib.Abstracts;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Extensions;
using Godot;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;

public class TheMiddleNurseFatherOutisRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => TheMiddleNurseFatherOutis.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}