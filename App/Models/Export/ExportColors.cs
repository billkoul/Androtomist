using System.Drawing;

public class ExportColors
{
	public Color HeaderFont;
	public Color HeaderFill;
	public Color HeaderBorder;

	public Color DataEvenFill;
	public Color DataOddFill;
	public Color DataFont;
	public Color DataBorder;

	public Color BoxFont;
	public Color BoxFill;
	public Color BoxBorder;

	public ExportColors()
	{
		HeaderFont = Color.FromArgb(255, 255, 255);
		HeaderFill = Color.FromArgb(191, 4, 23);
		HeaderBorder = Color.FromArgb(218, 220, 221);

		DataEvenFill = Color.FromArgb(255, 255, 255);
		DataOddFill = Color.FromArgb(240, 245, 255);
		DataFont = Color.FromArgb(0, 0, 0);
		DataBorder = Color.FromArgb(218, 220, 221);

		BoxFont = Color.FromArgb(255, 255, 255);
		BoxFill = Color.FromArgb(191, 4, 23);
		BoxBorder = Color.FromArgb(218, 220, 221);
	}
}
