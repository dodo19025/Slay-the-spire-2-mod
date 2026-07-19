using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Extensions;

public class DamageTakenHook() : CustomSingletonModel(HookType.Combat)
{
    public static readonly SpireField<PlayerCombatState, int> PaybackAcitvated = new(() => 0);
    public static readonly SpireField<PlayerCombatState, bool> TookDamageLastTurn =
        new SavedSpireField<PlayerCombatState, bool>(() => false, "TookDamageLastTurn");

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (dealer == null ||  dealer.Side == target.Side || !props.IsPoweredAttack() || dealer.IsPlayer || !target.IsPlayer)
        {
            return Task.CompletedTask;
        }

        if (target.IsPlayer && result.UnblockedDamage > 0)
        {
            TookDamageLastTurn[target.Player!.PlayerCombatState!] = true;
            MainFile.Logger.Info($"Damage Last turn Status: {DamageTakenHook.TookDamageLastTurn[target?.Player!.PlayerCombatState!]}");

        }
        return Task.CompletedTask;
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        foreach (var creature in participants)
        {
            if (creature.IsPlayer)
            {
                TookDamageLastTurn[creature?.Player!.PlayerCombatState!] = false;
                MainFile.Logger.Info($"Damage Last turn Status: {DamageTakenHook.TookDamageLastTurn[creature?.Player!.PlayerCombatState!]}");
            }
        }

        return Task.CompletedTask;
    }
    
}