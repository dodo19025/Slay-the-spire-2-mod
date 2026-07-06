using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public abstract class VulnerableNextTurn()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    private const string _VulnerableNextTurnKey = "VulnerableNextTurn";
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("VulnerableNextTurn", 1m)];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        await PowerCmd.Apply<VulnerablePower>(choiceContext, base.Owner, base.DynamicVars["VulnerableNextTurn"].BaseValue, base.Owner, null);
        await PowerCmd.Decrement(this);
    }
}