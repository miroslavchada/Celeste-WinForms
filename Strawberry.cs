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
    int initialY;
    int idleAnimIndex = 0;

    // Tracking
    int allowedDistance = 20;

    double strawberryScale = 0.8;

    public PictureBox pb;

    public Strawberry(int posX, int posY, Panel panel)
    {
        pb = new PictureBox
        {
            Left = posX,
            Top = posY,
            Width = (int)(60 * strawberryScale),    // Default image resolution * scale
            Height = (int)(78 * strawberryScale),
            BackColor = Color.Transparent,
            Image = Resources.Strawberry_idle,
            SizeMode = PictureBoxSizeMode.StretchImage
        };

        #region Idle

        // Storing default vertical position (against animation drift)
        initialY = posY;

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
            Interval = 80,
            Enabled = false
        };
        collectAnimationTimer.Tick += CollectAnimate;

        #endregion Collecting

        panel.Controls.Add(pb);
        pb.BringToFront();
    }

    public void TrackTarget(PictureBox target)
    {
        bool awayLeft = target.Left - pb.Right > allowedDistance;    // If the strawberry is in the "safezone" from: target's left
        bool awayRight = pb.Left - target.Right > allowedDistance;   // target's right
        bool awayTop = target.Top - pb.Bottom > allowedDistance;     // target's top
        bool awayBottom = pb.Top - target.Bottom > allowedDistance;  // target's bottom

        int movementSpeedRatio = 20;

        if (awayLeft)   // Strawberry is on the left
        {
            pb.Left += (Math.Abs(target.Left - allowedDistance) - pb.Right) / movementSpeedRatio;
        }
        else if (awayRight)   // Strawberry is on the right
        {
            pb.Left += (Math.Abs(target.Right + allowedDistance) - pb.Left) / movementSpeedRatio;
        }

        if (awayTop)   // Strawberry is up
        {
            pb.Top += (Math.Abs(target.Top + allowedDistance) - pb.Bottom) / movementSpeedRatio;
        }
        else if (awayBottom)  // Strawberry is down
        {
            pb.Top += (Math.Abs(target.Bottom - allowedDistance) - pb.Top) / movementSpeedRatio;
        }

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
                pb.Top = initialY;
                idleAnimIndex = -1;
                break;
        }

        idleAnimIndex++;
    }

    public void Collect()   // Collecting animation
    {
        // Start animation
        if (!collectAnimationTimer.Enabled)
        {
            tracking = false;
            collectingTime.Enabled = false;
            collectAnimationTimer.Enabled = true;
        }

        // Flattening
        if (pb.Height > 0)
        {
            pb.Height -= 20;
            pb.Top += 10;
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
