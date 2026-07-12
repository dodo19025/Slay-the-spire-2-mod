using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


public class Concuss() : TheMiddleNurseFatherOutisCard(
    2, CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    private bool _foundDebuff = false;

    protected override bool ShouldGlowGoldInternal => AnyHasDebuff();
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("StrengthPowerDown", 3m),
        new DamageVar(16m,ValueProp.Move)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<StrengthPower>()
    ];
    
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this,play.Target).Execute(choiceContext);
        
        foreach (PowerModel power in play.Target.Powers)
        {
            if (power != null && power.Type == PowerType.Debuff)
            {
                _foundDebuff = true;
            }
        }
        if (_foundDebuff)
        {
            await PowerCmd.Apply<SwatPower>(choiceContext, play.Target, base.DynamicVars["StrengthPowerDown"].BaseValue,base.Owner.Creature,this);
        }

    }

    public bool AnyHasDebuff()
    {
        foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
        {
            foreach (PowerModel power in hittableEnemy.Powers)
            {
                if (power != null && power.Type == PowerType.Debuff)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(5m);
    }
}