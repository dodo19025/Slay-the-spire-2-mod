using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

  
[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class StandGround()
    : TheMiddleNurseFatherOutisCard(0,
        CardType.Skill, CardRarity.Uncommon,
        TargetType.Self)
{
    private decimal _totalreduceblock = 0;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("ReducedBlockGain", 2m),
        new DynamicVar("TotalBlock", 8m)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        decimal _totalblock = base.DynamicVars["TotalBlock"].BaseValue - _totalreduceblock;
        await CreatureCmd.GainBlock(base.Owner.Creature, _totalblock,ValueProp.Move, play);
        _totalreduceblock += base.DynamicVars["ReducedBlockGain"].BaseValue;
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["TotalBlock"].UpgradeValueBy(2m);
    }
}