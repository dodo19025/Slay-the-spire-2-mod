using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Hooks;

/// <summary>
/// Runs right after you gain any grudge resource
/// </summary>
public interface IAfterGrudgeGained
{
    /// <summary>
    /// Runs right after you gain any grudge resource
    /// </summary>
    /// <param name="player"> The player that gained grudge</param>
    /// <param name="amountGained"> The amount of grudge that was gained</param>
    /// <returns></returns>
    Task AfterGrudgeGained(Player player, int amountGained);
}

/// <summary>
/// Runs right before you gain any grudge resource
/// </summary>
public interface IBeforeGrudgeGained
{
    /// <summary>
    /// Runs right before you gain any grudge resource
    /// </summary>
    /// <param name="player"> The player that gained grudge</param>
    /// <param name="amountGained"> The amount of grudge that was gained</param>
    /// <returns></returns>
    Task BeforeGrudgeGained(Player player, int amountGained);
} 
/// <summary>
/// Runs after Payback Power does damage to an enemy 
/// </summary>
public interface IAfterPaybackDone
{
    /// <summary>
    /// Runs after Payback Power does damage to an enemy 
    /// </summary>
    /// <param name="choiceContext"> The player's choice that signaled this event</param>
    /// <param name="player"> The player that activated payback</param>
    /// <param name="amountPayback"> The amount of payback that the player had after activating payback</param>
    /// <param name="target"> The target taking payback damage</param>
    /// <returns></returns>
    Task AfterPaybackDone(PlayerChoiceContext choiceContext,Player player, int amountPayback, Creature target);
}



