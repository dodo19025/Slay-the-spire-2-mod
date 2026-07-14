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
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Localization;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class Stomping() : TheMiddleNurseFatherOutisCard(
    1, CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[5]
    {
        new DamageVar(3m, ValueProp.Move),
        new DynamicVar("Exclamation", 1m),
        new DynamicVar("BleedPower",1m),
        new DynamicVar("HitAmount",2m),
        new DynamicVar("BleedThreshold",5m)
    });



    protected override bool ShouldGlowGoldInternal => HasEnoughBleed();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BleedPower>()
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        for (int i = 0; i < base.DynamicVars["HitAmount"].BaseValue; i++)
        {
            await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
            await PowerCmd.Apply<BleedPower>(choiceContext, play.Target,base.DynamicVars["BleedPower"].BaseValue,base.Owner.Creature,this);
        }

        int TargetBleed = play.Target.GetPowerAmount<BleedPower>();
        if (TargetBleed >= base.DynamicVars["BleedThreshold"].BaseValue)
        {
            decimal AmountOfHitsIncreased = Math.Round(base.DynamicVars["BleedThreshold"].BaseValue / TargetBleed);
            base.DynamicVars["HitAmount"].BaseValue++;
        }
    }

    public bool HasEnoughBleed()
    {
        foreach (Creature HittableEnemy in base.CombatState.HittableEnemies)
        {
            if (HittableEnemy.HasPower<BleedPower>())
            {
                int bleednum = HittableEnemy.GetPowerAmount<BleedPower>();
                if (bleednum >= base.DynamicVars["BleedThreshold"].BaseValue)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected override void OnUpgrade()
    {
        //base.DynamicVars.Damage.UpgradeValueBy(1m);
        base.DynamicVars["BleedPower"].UpgradeValueBy(1m);
    }
}