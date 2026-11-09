using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class WatchYourBack()
    : TheMiddleNurseFatherOutisCard(2,
        CardType.Attack, CardRarity.Uncommon,
        TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(5m, ValueProp.Move),
        new DynamicVar("BleedPower", 2m),
    ];

    protected override bool ShouldGlowGoldInternal =>
        Owner.Creature.LastCard().Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Swing);

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Kick
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BleedPower>(),
    ];

    private bool swinglastcard;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        for (int i = 0; i < 2; i++)
        {
            await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
            await CommonActions.Apply<BleedPower>(play.Target, this, DynamicVars["BleedPower"].BaseValue);
        }

        if (Owner.Creature.LastCard().Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Swing))
        {
            swinglastcard = true;
        }
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null && swinglastcard)
        {
            await Cmd.Wait(0.5f);
            swinglastcard = false;
            await CardCmd.AutoPlay(choiceContext, this, cardPlay.Target);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
        
    }
}