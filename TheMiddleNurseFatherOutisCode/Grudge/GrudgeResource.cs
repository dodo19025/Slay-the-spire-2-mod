using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Hooks;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;

public static partial class GrudgeResource
{
	public static readonly SpireField<PlayerCombatState, int> PlayerGrudge = new(() => 0);
	
	public static event Action<PlayerCombatState, int, int>? GrudgeChanged;
		//GrudgeChanged?.Invoke(player.PlayerCombatState,Grudges,PlayerGrudge[player.PlayerCombatState]);


	public static int GetGrudge(Player player) =>
		player.PlayerCombatState != null ? PlayerGrudge[player.PlayerCombatState] : 0; //this should do what it says? aka ??

	public static bool CanSpendGrudge(Player player)
	{
		if (player.PlayerCombatState == null) return false;
		if (GetGrudge(player) <= 0) return false; //basically if the player does not have grudge to begin with they can't spend it
		return true; //the player can spend Grudge if they have anything higher than 0
	}

	public static async Task GainGrudge(int amount, Player player)
	{
		if(player.PlayerCombatState == null || player.Creature.CombatState == null) return;
		
		var Grudges = PlayerGrudge[player.PlayerCombatState];
		PlayerGrudge[player.PlayerCombatState] = (int)Math.Max(amount + Grudges, 0);
		foreach (var model in player.Creature.CombatState.IterateHookListeners().ToList())
		{
			if (model is IAfterGrudgeGained listener)
			{
				await listener.AfterGrudgeGained(player, amount);
			}
		}

		
	}

	internal static void LoseGrudge(int amount, Player player)
	{
		if (player.PlayerCombatState == null) return;
		var OldVar = PlayerGrudge[player.PlayerCombatState];
		var newVar = Math.Max(0, OldVar - amount); //check if basically you reach 0... oh that is alot simpler than i thought
		if (newVar == OldVar) return;
		PlayerGrudge[player.PlayerCombatState] = newVar; //basically changes the number to the new one now after losing it
		GrudgeChanged?.Invoke(player.PlayerCombatState,OldVar,newVar);
	}

	public class GrudgeCounterController() : CustomSingletonModel(HookType.Combat)
	{
		public static readonly AddedNode<NEnergyCounter, NGrudgeCounter> GrudgeCounterNode = new(parent =>
		{
			var counter = PreloadManager.Cache.GetScene("res://TheMiddleNurseFatherOutis/scenes/GrudgeCounter.tscn")
				.Instantiate<NGrudgeCounter>();
			
			
			parent.AddChildSafely(counter);
			counter.SetAnchorsPreset(Control.LayoutPreset.Center);
			counter.Position = new Vector2(110,-50);
			counter.Size = new Vector2(94, 94);
			counter.ZIndex = parent.ZIndex - 1;

			return counter;

		});
	}
	
	

	

}
