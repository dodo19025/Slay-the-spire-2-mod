using BaseLib.Utils;
using Godot;
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
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Powers;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;


public class Unpacking2() : TheMiddleNurseFatherOutisCard(
    0, CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    private const string _Power = "Power";
    private const string _FeverLost = "FeverLost";
    
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<LaevateinnPower>();

    protected override bool IsPlayable => GetUserFever();

    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[3]
    {
        new DamageVar(10m, ValueProp.Move),
        new DynamicVar("Power", 1m),
        new DynamicVar("FeverLost", 1m),
    });

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain,
        CardKeyword.Exhaust
       //TheMiddleNurseFatherOutisKeywords.Fervour
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<RisingFeverPower>(),
        HoverTipFactory.FromPower<VulnerableNextTurn>(),
        HoverTipFactory.FromPower<VulnerablePower>()
    ];


    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power is RisingFeverPower && power.Owner == base.Owner.Creature && !(base.Owner.Creature.HasPower<LaevateinnPower>()))
        {
            MainFile.Logger.Info("Decected that rising fever has change");
            if (power.Amount <= 0)
            {
                MainFile.Logger.Info("autoplaying card");
                await CardCmd.AutoPlay(choiceContext,this,null);
            }
        }
    }
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    { 
        
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await PowerCmd.Apply<VulnerableNextTurn>(choiceContext, play.Target, base.DynamicVars["Power"].BaseValue,
            base.Owner.Creature, this);

        if (play.Card.Owner.Creature.Player?.Character is Character.TheMiddleNurseFatherOutis)
        { 
            CanvasItem visualzeroseal = (play.Card.Owner.Creature.GetCreatureNode()!.Body.GetNode("0Sealanimations") as CanvasItem)!;
            CanvasItem visualoneseal = (play.Card.Owner.Creature.GetCreatureNode()!.Body.GetNode("1Sealanimations") as CanvasItem)!;
            CanvasItem visualtwoseal = (play.Card.Owner.Creature.GetCreatureNode()!.Body.GetNode("2Sealaniamtions") as CanvasItem)!;
            CanvasItem visualthreeseal = (play.Card.Owner.Creature.GetCreatureNode()!.Body.GetNode("3Sealanimations") as CanvasItem)!;
            Modsounds.unpacking1.Play();
            visualoneseal.Visible = false;
            visualtwoseal.Visible = true;;
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "unpacking", 0.4f);
            await PowerCmd.Apply<RisingFeverPower>(choiceContext, base.Owner.Creature, 2m, base.Owner.Creature, this); //reapplies rising fever to 2
            await PowerCmd.Apply<FirstSealRemovedPower>(choiceContext, base.Owner.Creature, -1m, base.Owner.Creature, this);
            await PowerCmd.Apply<SecondSealRemovedPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
            await Cmd.CustomScaledWait(0.5f, 1f);
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "sunglasses", 2f);
            CardModel Card = base.Owner.Creature.CombatState.CreateCard<Unpacking3>(base.Owner.Creature.Player);
            CardPileCmd.AddGeneratedCardToCombat(Card, PileType.Hand, base.Owner);
            
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }

    public bool GetUserFever()
    {
        if (base.Owner.Creature.HasPower<RisingFeverPower>())
        {
            return false;
        }
        return true;
    }
}

