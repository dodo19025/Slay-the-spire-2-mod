using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class StrikeBack() : TheMiddleNurseFatherOutisCard(
    2, CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            if (base.CombatState == null)
            {
                return false;
            }
            return base.CombatState.HittableEnemies.Any((Creature e) => e.Monster?.IntendsToAttack ?? false);
        }
    }
    
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(10m, ValueProp.Move),
        new DynamicVar("AdditionalDamage", 10m)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        decimal _totalDamage = base.DynamicVars.Damage.BaseValue;
        if (play.Target.Monster.IntendsToAttack)
        {
            _totalDamage += base.DynamicVars["AdditionalDamage"].BaseValue;
        }
        await CreatureCmd.Damage(choiceContext, play.Target, _totalDamage, ValueProp.Move,base.Owner.Creature,this,play);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
        base.DynamicVars["AdditionalDamage"].UpgradeValueBy(4m);
    }
}