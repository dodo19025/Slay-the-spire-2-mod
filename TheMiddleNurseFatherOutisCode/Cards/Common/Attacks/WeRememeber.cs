using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Extensions;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;
[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class WeRememeber() : TheMiddleNurseFatherOutisCard(
    1, CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{

    protected override bool ShouldGlowGoldInternal =>
        DamageTakenHook.ActivatedPaybackLastTurn[Owner.Creature.Player!.PlayerCombatState!];

    protected override IEnumerable<DynamicVar> CanonicalVars => (
    [
        new DamageVar(5m, ValueProp.Move)
    ]);
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Punch
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromCard<Wound>(),
    ];


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int hitnum = 1;
        
        if (DamageTakenHook.ActivatedPaybackLastTurn[Owner.Creature.Player!.PlayerCombatState!])
        {
            hitnum++;
        }
        await CommonActions.CardAttack(this,play.Target)
            .WithHitCount(hitnum)
            .Execute(choiceContext); 
        
        if (DamageTakenHook.ActivatedPaybackLastTurn[Owner.Creature.Player!.PlayerCombatState!])
        {
            CardModel card = base.CombatState.CreateCard<Wound>(base.Owner);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Discard, base.Owner));
            await Cmd.Wait(0.5f);
            
        }

    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);

    }
}