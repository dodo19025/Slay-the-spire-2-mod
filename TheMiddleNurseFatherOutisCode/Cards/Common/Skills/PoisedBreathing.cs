using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Localization;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

    
public class PoisedBreathing()
    : TheMiddleNurseFatherOutisCard(1,
        CardType.Skill, CardRarity.Common,
        TargetType.Self)
{
    

    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new SeetheVar(6m),
        new DynamicVar("PaybackThreshold", 4m),
        new EnergyVar(2)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<PaybackPower>()
    ];

    protected override bool ShouldGlowGoldInternal =>
        (Owner.Creature.Block >= base.DynamicVars["PaybackThreshold"].BaseValue);

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (Owner.Creature.Block >= base.DynamicVars["PaybackThreshold"].BaseValue)
        {
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
        }
        await Owner.Creature.GainPayback(choiceContext,base.DynamicVars["Seethe"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Energy.UpgradeValueBy(1m);
    }
}