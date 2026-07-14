using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Validation;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Extensions;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;



public class PaybackPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    public static readonly SavedSpireField<PlayerCombatState, int> PaybackAcitvated = new(() => 0,"Payback_Acitvated");
    

    private bool _didPayback = false;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target,
        DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (dealer == null || dealer == base.Owner || dealer.Side == base.Owner.Side || !props.IsPoweredAttack())
        {
            return;
        }
        
        if (target.IsPlayer && result.UnblockedDamage > 0)
        {
            if (base.Owner.Player?.Character is Character.TheMiddleNurseFatherOutis)
            {
                await Owner.PlayAnimation("attack", 0.2f);

            }
            else if (!(base.Owner.Player?.Character is Character.TheMiddleNurseFatherOutis))
            {
                await CreatureCmd.TriggerAnim(base.Owner, "Attack",0.2f);
            }
            await CreatureCmd.Damage(choiceContext, dealer, base.Amount, ValueProp.Unpowered,base.Owner, null, null);
            _didPayback = true;
            PlayerCombatState? playerCombatState = base.Owner.Player.PlayerCombatState;
            DamageTakenHook.PaybackAcitvated[playerCombatState] += 1;
            MainFile.Logger.Info($"Num of paybacks done: {DamageTakenHook.PaybackAcitvated[playerCombatState]}");
            MainFile.Logger.Info("Did Payback");
            
        }
    }
        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (_didPayback)
        {
            await PowerCmd.Remove(this);
            _didPayback = false;
        }
    }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        PlayerCombatState? playerCombatState = base.Owner.Player.PlayerCombatState;
        DamageTakenHook.PaybackAcitvated[playerCombatState] = 0;
        DamageTakenHook.TookDamageLastTurn[playerCombatState] = false;
    }
    

}

