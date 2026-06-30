using BaseLib.Abstracts;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Extensions;
using Godot;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;

public class TheMiddleNurseFatherOutisPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => TheMiddleNurseFatherOutis.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/outis_text_energy.png".ImagePath();
}