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
    Task AfterGrudgeGained(Player player, int amountgained);
}

/// <summary>
/// Runs right before you gain any grudge resource
/// </summary>
public interface IBeforeGrudgeGained
{
    /// <summary>
    /// Runs right before you gain any grudge resource
    /// </summary>
    Task BeforeGrudgeGained(Player player, int amountgained);
} 
/// <summary>
/// Runs after Payback Power does damage to an enemy 
/// </summary>
public interface IAfterPaybackDone
{
    /// <summary>
    /// Runs after Payback Power does damage to an enemy 
    /// </summary>
    Task AfterPaybackDone(PlayerChoiceContext choiceContext,Player player, int amountpayback, Creature target);
}



