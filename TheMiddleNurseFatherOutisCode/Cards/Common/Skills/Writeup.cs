using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class Writeup() : TheMiddleNurseFatherOutisCard(
    1, CardType.Skill, CardRarity.Common,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("VunNextTurnPower", 1m),
        new DynamicVar("EnergyNextTurnPower", 1m),
    ];


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext,base.Owner.Creature,base.DynamicVars["EnergyNextTurnPower"].BaseValue,base.Owner.Creature, this);
        
        foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<VulnerableNextTurn>(choiceContext, hittableEnemy, base.DynamicVars["VunNextTurnPower"].BaseValue, base.Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["EnergyNextTurnPower"].UpgradeValueBy(1);
    }
}