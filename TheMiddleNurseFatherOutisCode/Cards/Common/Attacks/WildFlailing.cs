using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class WildFlailing() : TheMiddleNurseFatherOutisCard(
    1, CardType.Attack, CardRarity.Common,
    TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DamageVar(2m, ValueProp.Move),
        new DynamicVar("HitAmount", 4m),
        new DynamicVar("AdditionalHitAmount", 2m),
        new DynamicVar("GrudgeConsume", 3m),
    ]);

    public override IEnumerable<CardKeyword> CanonicalKeywords => (
    [
        TheMiddleNurseFatherOutisKeywords.Punch
    ]);
    
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int hitAmount = (int)DynamicVars["HitAmount"].BaseValue;
        if (GrudgeResource.GetGrudge(Owner.Creature.Player!) >= DynamicVars["GrudgeConsume"].BaseValue)
        {
            await GrudgeResource.LoseGrudge((int)DynamicVars["GrudgeConsume"].BaseValue, Owner.Creature.Player!);
            hitAmount += (int)DynamicVars["AdditionalHitAmount"].BaseValue;
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this,play)
            .TargetingRandomOpponents(CombatState) //has to be done this way since we don't pass a target by default
            .WithHitCount(hitAmount)
            .Execute(choiceContext);
    }
    

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);

    }
}