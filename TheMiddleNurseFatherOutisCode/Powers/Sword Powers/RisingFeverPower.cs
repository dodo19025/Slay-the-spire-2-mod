using System.Diagnostics;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class RisingFeverPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DynamicVar("FeverAmount", 0m),
        new BoolVar("CanUnpack", false)
    ]);

    public override int DisplayAmount => base.DynamicVars["FeverAmount"].IntValue;

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (!((BoolVar)DynamicVars["CanUnpack"]).BoolVal)
        {
            MainFile.Logger.Info("Detected that unpack is false, setting to true");
            ((BoolVar)DynamicVars["CanUnpack"]).BoolVal = true;
        }
        return Task.CompletedTask;
    }
}