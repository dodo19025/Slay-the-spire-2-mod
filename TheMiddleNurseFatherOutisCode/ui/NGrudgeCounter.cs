using Godot;
using MegaCrit.Sts2.addons.mega_text;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.ui;

public partial class NGrudgeCounter : Control
{
	private TheMiddleNurseFatherOutisMegaLabel? _countLabel;

	private TheMiddleNurseFatherOutisMegaLabel? CountLabel => _countLabel ??= GetNode<TheMiddleNurseFatherOutisMegaLabel>("%Count");

	public void SetCount(int count)
	{
		CountLabel.Text = count.ToString();
	}

	public void ApplyFont(Font font, int minsize, int maxsize)
	{
		CountLabel.AddThemeFontOverride(ThemeConstants.Label.Font,font);
		CountLabel.MinFontSize = minsize;
		CountLabel.MaxFontSize = maxsize;
	}
}
