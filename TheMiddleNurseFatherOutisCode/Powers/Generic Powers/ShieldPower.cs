using System.Drawing;
using BaseLib.Hooks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using Color = Godot.Color;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class ShieldPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    private readonly Color shieldColor = new Color("#1f9eed");
    
    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(Amount, shieldColor, HealthBarForecastDirection.FromLeft)];
    }

    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        decimal damageTaken = amount;
        if (target != Owner)
        {
            return amount;
        }

        if (amount >= Amount)
        {
            Flash();
            MainFile.Logger.Info("Detected that the damage given is larger than shield amounts");
            damageTaken = amount - Amount;
            MainFile.Logger.Info($"Amount of damage to be taken: {damageTaken}\nAmount of damage done: {amount}\nAmount of shield you have {Amount}");
            Amount = 0;
            MainFile.Logger.Info($"Remaining amount of shield: {Amount}");

        }
        if (amount < Amount)
        {
            Flash();
            MainFile.Logger.Info("Detected that the damage given is smaller than shield amounts");
            damageTaken = 0;
            MainFile.Logger.Info($"Amount of damage to be taken: {damageTaken}\nAmount of damage done: {amount}\nAmount of shield you have {Amount}");
            Amount -= (int)amount;
            MainFile.Logger.Info($"Remaining amount of shield: {Amount}");

        }
        
        if (Amount <= 0)
        {
            PowerCmd.Remove(this);
        }
        
        //for playing the hurt animations
        
        if (Owner.Player?.Character is Character.TheMiddleNurseFatherOutis)
        {
             Owner.PlayAnimation("hurt", 0.2f);

        }
        else if (!(base.Owner.Player?.Character is Character.TheMiddleNurseFatherOutis))
        {
             CreatureCmd.TriggerAnim(Owner, "Hurt", 0.2f);
        }
        
        return damageTaken;
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power == this && Owner != null && power.Owner == Owner)
        {
            if (power.Amount <= 0)
            {
                await PowerCmd.Remove(this);
            }
        }
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.HasPower<ShieldPower>())
        {
            await PowerCmd.Remove(this);
        }
    }//removing the power at the turn end of the player
}