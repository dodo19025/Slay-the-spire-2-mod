using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Extensions;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class BadAssAssKicking()
    : TheMiddleNurseFatherOutisCard(1,
        CardType.Attack, CardRarity.Common,
        TargetType.AnyEnemy)
{
    
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { TheMiddleNurseFatherOutisTags.Kick };

    
        
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<LaevateinnPower>()
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => (
    [
        TheMiddleNurseFatherOutisKeywords.Punch
    ]);
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(7m,ValueProp.Move),
        new DynamicVar("BleedPower", 1m),
        new DynamicVar("HitAmount",2m),
        new DynamicVar("AdditionalHitAmount",2m)
    ];
    
    //
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int hitAmount = (int)DynamicVars["HitAmount"].BaseValue;
        if (Owner.Creature.HasPower<LaevateinnPower>())
        {
            hitAmount +=  (int)DynamicVars["AdditionalHitAmount"].BaseValue;
        }
        for (int i = 0; i < hitAmount; i++)
        {
            await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
            await TheMiddleNursefatherOutisCmd.CardApplyBleed(choiceContext,Owner.Creature,DynamicVars["BleedPower"].BaseValue,play.Target,this);
        }
    }
    

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["BleedPower"].UpgradeValueBy(1m);
    }
}