using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public abstract class SecondSealRemovedPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    private const int _baseBurnApplyLeft = 3;

    private const string _burnApplyLeftKey = "BurnApplyLeft";
    
    public override int DisplayAmount => base.DynamicVars["BurnApplyLeft"].IntValue;
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[2]
    {
        new DynamicVar("BurnApplyLeft", 3m),
        new DynamicVar("BurnApplication", 2m)
    });
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BurnPower>(),//
    ];

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power is BurnPower && applier == base.Owner && !(amount <= 0))
        {
            base.DynamicVars["BurnApplyLeft"].BaseValue--;
            InvokeDisplayAmountChanged();

            if (base.DynamicVars["BurnApplyLeft"].BaseValue <= 0)
            {
                Flash();
                var ownerCombatState = base.Owner.CombatState;
                if (ownerCombatState != null)
                {
                    await PowerCmd.Apply<BurnPower>(choiceContext, ownerCombatState.HittableEnemies,
                        base.DynamicVars["BurnApplication"].BaseValue, base.Owner, null);
                }
                
                base.DynamicVars["BurnApplyLeft"].BaseValue = 3m;
                InvokeDisplayAmountChanged();
            }

        }
    }
}