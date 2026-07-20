using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Grudge;
[GlobalClass]
public partial class NGrudgeCounter : Control
{
	private Label? _label;
	private NEnergyCounter? _energyCounter;
	private Player? _player;
	private HoverTip _hoverTip;
	private int _grudgeCount = 0;

	public override void _Ready()
	{
		this.MouseFilter = MouseFilterEnum.Ignore;
		
		_label = GetNodeOrNull<Label>("%Count");
		LocString locString = new LocString("static_hover_tips","THEMIDDLENURSEFATHEROUTIS-GRUDGE.description");
		_hoverTip = new HoverTip(new LocString("static_hover_tips", "THEMIDDLENURSEFATHEROUTIS-GRUDGE.title"), locString);


		Connect(Control.SignalName.MouseEntered, Callable.From(OnHovered));
		Connect(Control.SignalName.MouseExited, Callable.From(OnUnhovered));

		
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

	
	//make it increase one by one
	
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


	private void OnHovered()
	{
		MainFile.Logger.Info("@@@");
		NHoverTipSet.CreateAndShow(this,_hoverTip)?.SetGlobalPosition(base.GlobalPosition + new Vector2(-34f, -300f));
	}
	
	

	private void OnUnhovered()
	{
		NHoverTipSet.Remove(this);
	}
}
