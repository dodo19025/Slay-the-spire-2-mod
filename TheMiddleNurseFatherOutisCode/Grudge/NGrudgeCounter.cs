using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
[GlobalClass]
public partial class NGrudgeCounter : Control
{
	private Label? _label;
	private NEnergyCounter? _energyCounter;
	private Player? _player;

	private int _grudgeCount = 0;

	public override void _Ready()
	{
		this.MouseFilter = MouseFilterEnum.Ignore;
		
		_label = GetNodeOrNull<Label>("%Count");
		
		if (GetParent() is NEnergyCounter energyCounter)
		{
			_energyCounter = energyCounter;
			_player = energyCounter._player;
		}

		this.Visible = false;
		RefreshVisiblity();
		if (this.Visible == true)
		{
			UpdateGrudge();
		}
			
	}
	

	public override void _Process(double delta)
	{
		if(_player == null) return;
		RefreshVisiblity();
		if(this.Visible == false) return;
		UpdateGrudge();
	}

	private void RefreshVisiblity()
	{

		if (_player == null || _player.PlayerCombatState == null)
		{
			this.Visible = false;
		}
		else
		{
			int Grudge = GrudgeResource.GetGrudge(_player);
			this.Visible = this.Visible || _player.Character is Character.TheMiddleNurseFatherOutis || Grudge > 0;
		}
	}

	private void UpdateGrudge()
	{
		if (_player == null || _player.PlayerCombatState == null)
		{
			return;
		}

		if (_label == null)
		{
			return;
		}
		int Grudge =  GrudgeResource.GetGrudge(_player);
		_grudgeCount = Grudge;
		_label.Text = _grudgeCount.ToString();
	}
}
