using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Hooks;


public interface IAfterGrudgeGained
{
    Task AfterGrudgeGained(Player player, int amountgained);
}

public interface IBeforeGrudgeGained
{
    Task BeforeGrudgeGained(Player player, int amountgained);
}

public interface IAfterPaybackDone
{
    Task AfterPaybackDone(PlayerChoiceContext choiceContext,Player player, int amountpayback, Creature target);
}



