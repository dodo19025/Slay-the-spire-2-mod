using System.Diagnostics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;


public class RisingFeverPower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>()
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[2]
    {
        new DynamicVar("RisingFeverReapply", 2m),
        new DynamicVar("StrengthGain", 1m)
    });

    public int Stage = 0;
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        int FeverAmount = base.Owner.GetPowerAmount<RisingFeverPower>();
        if (FeverAmount <= 0 && applier.IsPlayer && Stage < 3)
        {
           await PowerCmd.Apply<RisingFeverPower>(choiceContext, base.Owner,
                base.DynamicVars["RisingFeverReapply"].BaseValue, base.Owner, null);
           await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner,
               base.DynamicVars["StrengthGain"].BaseValue, base.Owner, null);
           Stage += 1;
           if (Stage == 1)
           {
               Modsounds.unpacking0.Play();
               await CreatureCmd.TriggerAnim(base.Owner, "unpacking", 0.2f);
               await PowerCmd.Apply<SealedSwordPower>(choiceContext, base.Owner, -1m,base.Owner, null );
               await PowerCmd.Apply<FirstSealRemovedPower>(choiceContext, base.Owner, 1m,base.Owner, null );
           }

           if (Stage == 2)
           {
               await PowerCmd.Apply<FirstSealRemovedPower>(choiceContext, base.Owner, -1m,base.Owner, null );
               await PowerCmd.Apply<SecondSealRemovedPower>(choiceContext, base.Owner, 1m,base.Owner, null );
           }
        }

                
            
               
        

    }
}