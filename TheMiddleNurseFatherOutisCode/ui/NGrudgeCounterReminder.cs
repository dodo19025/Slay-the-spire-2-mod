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
        _counter = ResourceLoader.Load<PackedScene>(TheMiddleNurseFatherOutisResources.GrudgeCounterScene)
            .Instantiate<NGrudgeCounter>(); //creates the actual scene for the counter once activated
        var font = PreloadManager.Cache.GetAsset<Font>(TheMiddleNurseFatherOutisResources.MegaLabelFont);
        _counter.ApplyFont(font, minsize: 32, maxsize: 32); //applies a specific font once loaded
        _counter.Modulate = new Color(1, 1, 1, 0);
        AddChild(_counter);
    }

    public void Initialize(Player player)
    {
        _player = player;
        UpdateVisiblity();
    }

    public override void _EnterTree()
    {
        GrudgeResource.GrudgeChanged += OnGrudgeChanged;
        CombatManager.Instance.StateTracker.CombatStateChanged += OnCombatStateChanged;
    }

    public override void _ExitTree()
    {
        GrudgeResource.GrudgeChanged -= OnGrudgeChanged;
        CombatManager.Instance.StateTracker.CombatStateChanged -= OnCombatStateChanged;
    }

    private void OnGrudgeChanged(PlayerCombatState combatState, int oldvar, int newvar)
    {
        if (_player == null || combatState != _player.PlayerCombatState) return;
        UpdateVisiblity();
    }

    private void OnCombatStateChanged(CombatState _) => UpdateVisiblity();

    private void UpdateVisiblity()
    {
        var shouldShow = _player != null && GrudgeResource.CanSpendGrudge(_player) &&
                         _player.Creature.CombatState?.CurrentSide == CombatSide.Player;
        if (shouldShow)
            _counter.SetCount(GrudgeResource.GetGrudge(_player!));

        _fadeTween?.Kill();
        _fadeTween = CreateTween();
        _fadeTween.TweenProperty(_counter, "modulate:a", shouldShow ? 1f : 0f, 0.2f)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Sine);
    }
}