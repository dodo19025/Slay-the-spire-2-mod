using System.Buffers;
using System.Dynamic;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;

namespace TheMiddleNurseFatherOutis;

public class AttackAnimationChange(): CustomSingletonModel(HookType.Combat)
{

    

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (cardSource == null || !props.IsPoweredAttack() || dealer.IsEnemy)
        {
            return;
        }

        if (!(cardSource.Owner.Creature.Player?.Character is TheMiddleNurseFatherOutisCode.Character
                .TheMiddleNurseFatherOutis))
        {
            return;
        }
        
        if (cardSource.Type == CardType.Attack && cardSource.Owner.Creature.Player?.Character is TheMiddleNurseFatherOutisCode.Character.TheMiddleNurseFatherOutis)
        {
            CanvasItem visualthreeseal = (cardSource.Owner.Creature.GetCreatureNode()!.Body.GetNode("3Sealanimations") as CanvasItem)!;
            if (cardSource is Unpacking || cardSource is Unpacking2 || cardSource is Unpacking3)
            {
                await dealer.PlayAnimation("swordattack",0.2f);
                return;
            }
            if (cardSource.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Fervour))
            {

                if (visualthreeseal.Visible)
                {
                     await dealer.PlayAnimation("swordlattack",0.2f);
                     return;
                } 
                if (!visualthreeseal.Visible)
                {
                     await dealer.PlayAnimation("swordattack",0.2f);
                     return;
                }

            }
            else
            {
                 await dealer.PlayAnimation("legattack",0.2f);
                 return;
            }
        }
        

    }
    
}