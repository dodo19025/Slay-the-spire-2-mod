using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards.Common;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers.Card_Powers;


public class BideTimePower()
    : TheMiddleNurseFatherOutisPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player == Owner.Player)
        {
            await CardPileCmd.ShuffleIfNecessary(choiceContext, Owner.Player);
            await CardPileCmd.Add(await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner.Player), Owner.Player, new CardSelectorPrefs(SelectionScreenPrompt, Amount), filter: (CardModel c) => c.Type == CardType.Attack ), PileType.Hand);
            await PowerCmd.Remove(this);
        }
    }
    
}