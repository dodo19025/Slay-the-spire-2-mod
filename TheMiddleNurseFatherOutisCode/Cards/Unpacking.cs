using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;


public class Unpacking() : TheMiddleNurseFatherOutisCard(
    1, CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    private const string _Power = "Power";
    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[3]
    {
        new DamageVar(4m, ValueProp.Move),
        new DynamicVar("Power", 1m),
        new DynamicVar("FeverLost", 1m)
    });

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<RisingFeverPower>(),
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await PowerCmd.Apply<VulnerableNextTurn>(choiceContext, play.Target, base.DynamicVars["Power"].BaseValue,
            base.Owner.Creature, this);
        await PowerCmd.Apply<RisingFeverPower>(choiceContext, base.Owner.Creature, -base.DynamicVars["FeverLost"].BaseValue,base.Owner.Creature,this );
        Modsounds.Swordvfx.Play();
        
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}