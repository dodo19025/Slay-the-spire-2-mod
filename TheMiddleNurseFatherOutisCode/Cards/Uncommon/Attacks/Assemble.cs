using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;

[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class Assemble() : TheMiddleNurseFatherOutisCard(
    1, CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Kick
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<GrudgePower>(),
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(6m, ValueProp.Move),
        new CardsVar(1),
        new DynamicVar("GrudgeConsume", 3m)
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        if (GrudgeResource.GetGrudge(Owner) >= DynamicVars["GrudgeConsume"].BaseValue)
        {
            await GrudgeResource.LoseGrudge(DynamicVars["GrudgeConsume"].IntValue,Owner);
            var card =  await CommonActions.SelectSingleCard(this, SelectionScreenPrompt,choiceContext,PileType.Draw, filter:(CardModel c ) => c.Type == CardType.Skill);
            if (card != null)
            {
                await CardPileCmd.Add(card, PileType.Hand);
            }
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);

    }
}