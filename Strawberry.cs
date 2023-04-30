namespace Celeste_WinForms;

internal class Strawberry
{
    public bool tracking = false;

    public PictureBox pb;

    public Strawberry(int posX, int posY, Panel panel)
    {
        pb = new PictureBox
        {
            Left = posX,
            Top = posY,
            Width = 50,
            Height = 70,
            BackColor = Color.Red,
            //Image = strawberry,
            SizeMode = PictureBoxSizeMode.StretchImage
        };

        panel.Controls.Add(pb);
        pb.BringToFront();
    }

    public void TrackTarget(PictureBox target)
    {

    }
}
