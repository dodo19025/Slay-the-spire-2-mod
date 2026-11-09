using System.Globalization;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Hooks;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;


[Pool(typeof(TheMiddleNurseFatherOutisCardPool))]

public class Dusting() : TheMiddleNurseFatherOutisCard(
    0, CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy), IAfterGrudgeChange
{
    
    protected override bool HasEnergyCostX => true;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        TheMiddleNurseFatherOutisKeywords.Swing
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<GrudgePower>(),
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("TotalDamage",Getgrudge()),
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int numofhits = ResolveEnergyXValue();
        if (IsUpgraded)
        {
            numofhits += 1;
        }
        await CommonActions.CardAttack(this, play.Target,DynamicVars["TotalDamage"].BaseValue,ValueProp.Move,numofhits).Execute(choiceContext);
        await GrudgeResource.LoseGrudge(GrudgeResource.GetGrudge(Owner), Owner);
    }

    public Task AfterGrudgeChange(Player player, int amountChange)
    {
        DynamicVars["TotalDamage"].BaseValue = GrudgeResource.GetGrudge(Owner);
        return Task.CompletedTask;
    }
    
    public override Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        DynamicVars["TotalDamage"].BaseValue = GrudgeResource.GetGrudge(Owner);
        return Task.CompletedTask;
    }

    public int Getgrudge()
    {
        if (IsInCombat)
        {
            return GrudgeResource.GetGrudge(Owner);
        }
        return 0;
    }
    
    protected override void OnUpgrade()
    {

    }
}