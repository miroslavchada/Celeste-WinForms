using Celeste_WinForms.Properties;
using System.Diagnostics;

namespace Celeste_WinForms;

internal class Strawberry
{
    public bool tracking = false;
    public System.Windows.Forms.Timer collectingTime;

    private System.Windows.Forms.Timer idleAnimationTimer;
    private System.Windows.Forms.Timer collectAnimationTimer;

    // Idle animation
    int idleAnimIndex = 0;

    // Collecting animation
    private void WhitenImage(Image img, float tintAmount)
    {
        // Get bitmap of strawberry
        Bitmap bmp = new Bitmap(img);

        // tintAmount - the amount of tint to apply (between 0 and 1)
        // Apply a partial white tint to the bitmap
        Color tint = Color.FromArgb(128, 255, 255, 255);    // the tint color (semi-transparent white)
        for (int x = 0; x < bmp.Width; x++)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                Color pixelColor = bmp.GetPixel(x, y);
                Color blendedColor = Color.FromArgb(
                    pixelColor.A,
                    (int)(pixelColor.R * (1 - tintAmount) + tint.R * tintAmount),
                    (int)(pixelColor.G * (1 - tintAmount) + tint.G * tintAmount),
                    (int)(pixelColor.B * (1 - tintAmount) + tint.B * tintAmount)
                );
                bmp.SetPixel(x, y, blendedColor);
            }
        }

        pb.Image = bmp;
    }

    double strawberryScale = 0.8;

    public PictureBox pb;

    public Strawberry(int posX, int posY, Panel panel)
    {
        pb = new PictureBox
        {
            Left = posX,
            Top = posY,
            Width = (int)(60 * strawberryScale),
            Height = (int)(78 * strawberryScale),
            BackColor = Color.Transparent,
            Image = Resources.Strawberry_idle,
            SizeMode = PictureBoxSizeMode.StretchImage
        };

        #region Idle

        // Idle animation tick
        idleAnimationTimer = new System.Windows.Forms.Timer
        {
            Interval = 150,
            Enabled = true
        };
        idleAnimationTimer.Tick += IdleAnimate;

        #endregion Idle


        #region Collecting

        // Delay between landing and collecting strawberry
        collectingTime = new System.Windows.Forms.Timer
        {
            Interval = 300,
            Enabled = false
        };
        collectingTime.Tick += TimerCollect;

        // Collecting animation tick
        collectAnimationTimer = new System.Windows.Forms.Timer
        {
            Interval = 30,
            Enabled = false
        };
        collectAnimationTimer.Tick += CollectAnimate;

        #endregion Collecting

        panel.Controls.Add(pb);
        pb.BringToFront();
    }

    public void TrackTarget(PictureBox target)
    {
        // Turn off idle animation
        if (idleAnimationTimer.Enabled)
            idleAnimationTimer.Enabled = false;
    }

    public void IdleAnimation() // Animation for staying in place
    {
        int moveByDistance = 7;

        switch (idleAnimIndex)
        {
            case 0 or 1:
                pb.Top += moveByDistance;
                break;

            case 4 or 5 or 6 or 7:
                pb.Top -= moveByDistance;
                break;

            case 10:
                pb.Top += moveByDistance;
                break;

            case 11:
                pb.Top += moveByDistance;
                idleAnimIndex = 0;
                break;
        }

        idleAnimIndex++;
    }

    public void Collect()   // Collecting animation
    {
        // Start animation
        if (!collectAnimationTimer.Enabled)
        {
            collectingTime.Enabled = false;
            collectAnimationTimer.Enabled = true;
        }

        // Flattening
        if (pb.Height > 0)
        {
            pb.Height -= 10;
            pb.Top += 5;

            if (pb.Height < 50)
            {
                WhitenImage(pb.Image, 0.5f);
            }
        }
        else
        {
            collectAnimationTimer.Enabled = false;
            pb.Dispose();
        }

    }

    #region Event handlers for timers

    // Event handler for idleAnimationTimer.Tick
    private void IdleAnimate(object sender, EventArgs e)
    {
        IdleAnimation();
    }

    // Event handler for collectingTime.Tick
    private void TimerCollect(object sender, EventArgs e)
    {
        collectingTime.Enabled = false;
        Collect();
    }

    // Event handler for collectAnimationTimer.Tick
    private void CollectAnimate(object sender, EventArgs e)
    {
        Collect();
    }

    #endregion Event handlers for timers
}
