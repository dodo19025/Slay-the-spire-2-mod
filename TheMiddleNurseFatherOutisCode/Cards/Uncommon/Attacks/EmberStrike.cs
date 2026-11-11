using BaseLib.Extensions;
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


public class EmberStrike()
    : TheMiddleNurseFatherOutisCard(2,
        CardType.Attack, CardRarity.Uncommon,
        TargetType.AllEnemies)
{
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Swing
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BurnPower>(),
    ];


    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(10m,ValueProp.Move),
        new PowerVar<BurnPower>(2m),
        new EnergyVar(1)

    ];
    
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this,play).TargetingAllOpponents(CombatState!).Execute(choiceContext);
        var burnMulti = TheMiddleNursefatherOutisCmd.getSwordStagePower(Owner.Creature);
        foreach (Creature hittableEnemy in CombatState!.HittableEnemies)
        {
            await PowerCmd.Apply<BurnPower>(choiceContext, hittableEnemy, DynamicVars.Power<BurnPower>().BaseValue * burnMulti, Owner.Creature, this);
        }

        var totalBurn = 0;
        foreach (Creature hittableEnemy in CombatState!.HittableEnemies)
        {
            if (!hittableEnemy.HasPower<BurnPower>())
            {
                return;
            }
            if (hittableEnemy.GetPowerAmount<BurnPower>() >= 10)
            {
                await PowerCmd.ModifyAmount(choiceContext, hittableEnemy.GetPower<BurnPower>()!, -10m, hittableEnemy, this);
                totalBurn += 10;
            }
            else
            {
                totalBurn += hittableEnemy.GetPowerAmount<BurnPower>();
                await PowerCmd.Remove<BurnPower>(hittableEnemy);
            }

            var totalEnergy = Math.Round(totalBurn / 10m);
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue * totalEnergy, Owner);
        }
    }

    protected override void OnUpgrade()
    {
         DynamicVars.Damage.UpgradeValueBy(3m);
         DynamicVars["BurnPower"].UpgradeValueBy(1m);
    }
}