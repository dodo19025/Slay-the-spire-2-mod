using System.Resources;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.ui;

public partial class NGrudgeCounterReminder : Control
{
    private NGrudgeCounter _counter = null!;
    private Player? _player;
    private Tween? _fadeTween;

    public override void _Ready()
    {
        MainFile.Logger.Info($"Node Ready");

        _counter = ResourceLoader.Load<PackedScene>(TheMiddleNurseFatherOutisResources.GrudgeCounterScene)
            .Instantiate<NGrudgeCounter>(); //creates the actual scene for the counter once activated
        AddChild(_counter);
    }

    public void Initialize(Player player)
    {
        _player = player;
        UpdateVisiblity();
    }

    public override void _EnterTree()
    {
        MainFile.Logger.Info($"Tree Entered");

        GrudgeResource.GrudgeChanged += OnGrudgeChanged;
        CombatManager.Instance.StateTracker.CombatStateChanged += OnCombatStateChanged;
    }

    public override void _ExitTree()
    {
        MainFile.Logger.Info($"Tree Left");

        GrudgeResource.GrudgeChanged -= OnGrudgeChanged;
        CombatManager.Instance.StateTracker.CombatStateChanged -= OnCombatStateChanged;
    }

    private void OnGrudgeChanged(PlayerCombatState combatState, int oldvar, int newvar)
    {
        MainFile.Logger.Info($"Checking if Grudge Changed");

        if (_player == null || combatState != _player.PlayerCombatState) return;
        MainFile.Logger.Info($"Checking if Grudge did");

        UpdateVisiblity();
    }

    private void OnCombatStateChanged(CombatState _) => UpdateVisiblity();

    private void UpdateVisiblity()
    {
        MainFile.Logger.Info($"Updated Visiblity0");

        var shouldShow = _player != null && GrudgeResource.CanSpendGrudge(_player) &&
                         _player.Creature.CombatState?.CurrentSide == CombatSide.Player;
        if (shouldShow)
            _counter.SetCount(GrudgeResource.GetGrudge(_player!));
        MainFile.Logger.Info($"Updated Visiblity");

    }
}