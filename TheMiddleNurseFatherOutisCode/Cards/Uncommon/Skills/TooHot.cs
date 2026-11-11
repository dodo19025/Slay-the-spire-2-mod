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
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Localization;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]


public class TooHot() : TheMiddleNurseFatherOutisCard(
    1, CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new SustainVar(3m),
        new PowerVar<BurnPower>(2m),
        new DynamicVar("VitalityPower",4m),
        new CardsVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<VitalityPower>(),
        HoverTipFactory.FromPower<BurnPower>(),
        HoverTipFactory.FromCard<Burn>(),
    ];



    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardModel card = CombatState.CreateCard<Burn>(Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner));
        await Cmd.Wait(0.5f);
        int Burnapply = TheMiddleNursefatherOutisCmd.getSwordStagePower(Owner.Creature);
        foreach (Creature HittableEnemy in CombatState.HittableEnemies)
        {
            CommonActions.Apply<BurnPower>(HittableEnemy,this,(1 + Burnapply) * DynamicVars.Power<BurnPower>().BaseValue);
        }
        CommonActions.Apply<TooHotPower>(Owner.Creature,this,DynamicVars["VitalityPower"].BaseValue);
        CommonActions.Apply<SustainPower>(Owner.Creature,this,DynamicVars["Sustain"].BaseValue);


    }

    protected override void OnUpgrade()
    {
        DynamicVars["BurnPower"].UpgradeValueBy(1m);
    }
}