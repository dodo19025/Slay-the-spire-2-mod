using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;

public static partial class GrudgeResource
{
	private static readonly SpireField<PlayerCombatState, int> PlayerGrudge = new(() => 0);
	
	public static event Action<PlayerCombatState, int, int>? GrudgeChanged;


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
		
		//what is about to happen next is basically the process of how you gain stars one by one in the display
		for (var i = 0; i < amount; i++)
		{
			MainFile.Logger.Info($"Added {i} Grudge");
			var OldVar = PlayerGrudge[player.PlayerCombatState];
			PlayerGrudge[player.PlayerCombatState] = OldVar + i;
			GrudgeChanged?.Invoke(player.PlayerCombatState,OldVar,OldVar+1);

			foreach (var model in player.Creature.CombatState.IterateHookListeners().ToList())
			{
				//then come back ONCE you make hooks for gainign grudge
			}
		}
		MainFile.Logger.Info($"Added {amount} total Grudge");

		
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
			var counter = new NGrudgeCounter
			{
				Name = "GrudgeCounter",
				MouseFilter = Control.MouseFilterEnum.Ignore
			};

			counter.SetAnchorsPreset(Control.LayoutPreset.TopRight);
			counter.Position = Vector2.Right;
			counter.Size = new Vector2(200, 200);
			counter.ZIndex = 0;

			var virtualscene =
				ResourceLoader.Load<PackedScene>("res://TheMiddleNurseFatherOutis/scenes/GrudgeCounter.tscn");

			var visual = virtualscene.Instantiate<Control>();
			visual.Name = "%Icon";
			visual.MouseFilter = Control.MouseFilterEnum.Ignore;

			counter.AddChild(visual);

			return counter;

		});
	}
	
	

}
