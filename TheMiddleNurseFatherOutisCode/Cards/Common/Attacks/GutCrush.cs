using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class GutCrush() : TheMiddleNurseFatherOutisCard(
    3, CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    
    protected override bool ShouldGlowGoldInternal => base.CombatState?.HittableEnemies.Any((Creature e) => e.HasPower<BleedPower>()) ?? false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(24m,ValueProp.Move),
    ];

        
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BleedPower>()
    ];

    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this,play.Target).Execute(choiceContext);
        if (play.Target.IsAlive && play.Target.HasPower<BleedPower>())
        {
            int _enemyBleed = play.Target.GetPowerAmount<BleedPower>();
            await CreatureCmd.Damage(choiceContext, play.Target, (decimal)_enemyBleed, ValueProp.Unpowered | ValueProp.SkipHurtAnim, null);
            await TheMiddleNursefatherOutisCmd.ActivateBleed(choiceContext, play.Target, _enemyBleed);
            
        }
    }
    
    public decimal CalculateBleedLost(decimal BleedAmount) //bleedamount is for the amount of bleed that we currently have
    {
        if (BleedAmount <= 1)
        {
            return  1m;
        }

        return ((decimal)Math.Round((decimal)(BleedAmount * (1m / 2m))));

    }

    protected override void OnUpgrade()
    {

    }
}