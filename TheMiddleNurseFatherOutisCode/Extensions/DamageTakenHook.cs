using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Extensions;

public class DamageTakenHook() : CustomSingletonModel(HookType.Combat)
{
    public static readonly SavedSpireField<PlayerCombatState, int> PaybackAcitvated = new(() => 0,"Payback_Acitvated");

    public static readonly SpireField<PlayerCombatState, bool> TookDamageLastTurn =
        new SpireField<PlayerCombatState, bool>(() => false);


}