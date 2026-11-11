using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class Rule_Violation()
    : TheMiddleNurseFatherOutisCard(1,
        CardType.Skill, CardRarity.Basic,
        TargetType.Self)
{

    
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<PaybackPower>(),
        HoverTipFactory.FromPower<GrudgePower>(),
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("GrudgeConsume",2m),
        new PowerVar<PaybackPower>(4m),
        new DynamicVar("GrudgeGained",3m),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int grudge = GrudgeResource.GetGrudge(Owner.Creature.Player!);
        if (grudge >= DynamicVars["GrudgeConsume"].BaseValue)
        {
            await GrudgeResource.LoseGrudge((int)DynamicVars["GrudgeConsume"].BaseValue, Owner.Creature.Player!);
            await PowerCmd.Apply<PaybackPower>(choiceContext, Owner.Creature,DynamicVars.Power<PaybackPower>().BaseValue,Owner.Creature,this);
        }
        await GrudgeResource.GainGrudge((int)DynamicVars["GrudgeGained"].BaseValue, Owner.Creature.Player!);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PaybackPower"].UpgradeValueBy(2m);
        DynamicVars["GrudgeGained"].UpgradeValueBy(2m);

    }
}