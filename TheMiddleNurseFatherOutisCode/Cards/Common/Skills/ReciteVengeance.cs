using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class ReciteVengeance()
    : TheMiddleNurseFatherOutisCard(1,
        CardType.Skill, CardRarity.Common,
        TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[3]
    {
        new BlockVar(4m, ValueProp.Move),
        new DynamicVar("RisingFeverLose", 1m),
        new DynamicVar("IncreasedBlock", 3m)
    });
    
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


    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromPower<RisingFeverPower>()
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        if (base.Owner.Creature.HasPower<RisingFeverPower>())
        {
            await PowerCmd.Apply<RisingFeverPower>(choiceContext, base.Owner.Creature, -base.DynamicVars["RisingFeverLose"].BaseValue,base.Owner.Creature,this);
        }

        foreach (Creature HittableEnemy in base.CombatState.HittableEnemies)
        {
            if (HittableEnemy.Monster.IntendsToAttack)
            {
                await CreatureCmd.GainBlock(Owner.Creature, base.DynamicVars["IncreasedBlock"].BaseValue, ValueProp.Move, play);

            }
        }

    }

    public bool AnyEnemyIntendsAttack()
    {
        if (base.CombatState == null)
        {
            return false;
        }
        return base.CombatState.HittableEnemies.Any((Creature e) => e.Monster?.IntendsToAttack ?? false);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["IncreasedBlock"].UpgradeValueBy(1m);

    }
}