using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;



[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class GritYourTeeth() : TheMiddleNurseFatherOutisCard(
    1, CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new CardsVar(1),
        new BlockVar(6m,ValueProp.Move),
        new DynamicVar("HPLost", 4m)
    ];
    

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<VengeanceTattoo>(),
        HoverTipFactory.FromPower<StrengthPower>(),
    ];


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CommonActions.CardBlock(this, play);
        var Cards = await  CommonActions.SelectCards(this, SelectionScreenPrompt,
            choiceContext, PileType.Draw, DynamicVars.Cards.IntValue);
        if (Cards != null)
        {
            foreach (var card in Cards)
            {
                CardPileCmd.Add(card, PileType.Discard);
                if (card.Type == CardType.Curse || card.Type == CardType.Status)
                {
                    await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars["HPLost"].BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, play);
                    await CommonActions.ApplySelf<StrengthPower>(this, 1m);
                    await CommonActions.ApplySelf<VengeanceTattoo>(this, 1m);
                }
            }
        }

    }

    protected override void OnUpgrade()
    {
        DynamicVars["HPLost"].UpgradeValueBy(-1m);
        DynamicVars.Block.UpgradeValueBy(3m);
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}