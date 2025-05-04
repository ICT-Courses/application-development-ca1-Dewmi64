using System.Windows.Forms;
using System;

public static class UIStyler
{
    public static void RoundButton(Button btn, int radius)
    {
        btn.Region = System.Drawing.Region.FromHrgn(
            CreateRoundRectRgn(0, 0, btn.Width, btn.Height, radius, radius));
    }

    public static void RoundRichTextBox(RichTextBox rtb, int radius)
    {
        rtb.Region = System.Drawing.Region.FromHrgn(
            CreateRoundRectRgn(0, 0, rtb.Width, rtb.Height, radius, radius));
    }

    public static void TextBox(RichTextBox rtb, int radius)
    {
        rtb.Region = System.Drawing.Region.FromHrgn(
            CreateRoundRectRgn(0, 0, rtb.Width, rtb.Height, radius, radius));
    }

    [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    private static extern IntPtr CreateRoundRectRgn(
        int nLeftRect, int nTopRect,
        int nRightRect, int nBottomRect,
        int nWidthEllipse, int nHeightEllipse);
}
