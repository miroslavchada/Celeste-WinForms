namespace Celeste_WinForms;

class Terrain
{
    public int elevatorMovementSpeed = 5;
    public int animIndex = 0;
    int phase = 0;
    int animDelay = 50;
    public bool moving;

    public bool resetForce;

    int fromX;
    int fromY;
    int toX;
    int toY;
    double multiplierX;
    double multiplierY;

    double elevatorXdouble;
    double elevatorYdouble;

    public bool onBlockLeftExclusive;
    public bool onBlockRightExclusive;
    public bool onBlockDown;
    public int movementSpeed;

    public PictureBox pb;

    public Terrain(int posX, int posY, int width, int height, int _toX, int _toY, string tag, Color color, bool hasTexture, Image texture, Panel panel)
    {
        pb = new PictureBox
        {
            Left = posX,
            Top = posY,
            Width = width,
            Height = height,
            Tag = tag,
            BackColor = color,
            Image = hasTexture ? texture : null,
            SizeMode = PictureBoxSizeMode.StretchImage
        };

        elevatorXdouble = posX;
        elevatorYdouble = posY;

        fromX = posX;
        fromY = posY;

        toX = _toX;
        toY = _toY;

        int deltaX = toX - fromX;
        int deltaY = toY - fromY;
        double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

        multiplierX = Math.Abs(deltaX / distance);
        multiplierY = Math.Abs(deltaY / distance);

        panel.Controls.Add(pb);
        pb.BringToFront();
    }

    public void ElevatorAnimation(PictureBox player, bool grabbed, int playerLeftOffset, int playerRightOffset, int movementSpeed)
    {
        resetForce = false;

        // Foward
        if (animIndex > animDelay && phase == 0)
        {
            if (!(elevatorMovementSpeed * multiplierX > Math.Abs(toX - pb.Left) ||
                elevatorMovementSpeed * multiplierY > Math.Abs(toY - pb.Top)))    // If it's not close to the target yet
            {
                elevatorXdouble += elevatorMovementSpeed * multiplierX * (pb.Left < toX ? 1 : -1);
                elevatorYdouble += elevatorMovementSpeed * multiplierY * (pb.Top < toY ? 1 : -1);

                CheckCollision(elevatorXdouble, elevatorYdouble, player, playerLeftOffset, playerRightOffset, grabbed, movementSpeed);

                if (player.Bounds.IntersectsWith(pb.Bounds) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed))
                {
                    player.Left += (int)(elevatorXdouble - pb.Left);

                    if (onBlockDown)
                        player.Top = (int)pb.Top - player.Height + 1;
                    else
                        player.Top += (int)(elevatorYdouble - pb.Top);
                }
            }
            else
            {
                if (player.Bounds.IntersectsWith(pb.Bounds) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed))
                {
                    player.Left += (int)(elevatorXdouble - pb.Left);
                    player.Top += (int)(elevatorYdouble - pb.Top);
                }

                pb.Left = toX;
                pb.Top = toY;
                elevatorMovementSpeed = 0;
                animIndex = 0;
                phase = 1;
            }

            if (Math.Abs(elevatorMovementSpeed) < 40 && animIndex % 2 == 0)
            {
                elevatorMovementSpeed++;
            }
        }

        // Backwards
        if (animIndex > animDelay && phase == 1)
        {
            elevatorMovementSpeed = 3;

            if (!(elevatorMovementSpeed * multiplierX > Math.Abs(fromX - pb.Left) ||
                elevatorMovementSpeed * multiplierY > Math.Abs(fromY - pb.Top)))    // If it's not close to the target yet
            {
                elevatorXdouble += elevatorMovementSpeed * multiplierX * (pb.Left < fromX ? 1 : -1);
                elevatorYdouble += elevatorMovementSpeed * multiplierY * (pb.Top < fromY ? 1 : -1);

                CheckCollision(elevatorXdouble, elevatorYdouble, player, playerLeftOffset, playerRightOffset, grabbed, movementSpeed);

                if (player.Bounds.IntersectsWith(pb.Bounds) || (player.Bottom <= pb.Top && player.Bottom > pb.Top - 2) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed))
                {
                    player.Top += (int)(elevatorYdouble - pb.Top + ((player.Bottom <= pb.Top && player.Bottom > pb.Top - 2) ? pb.Top - player.Bottom + 1 : 0));

                    if (player.Bounds.IntersectsWith(pb.Bounds))
                    {
                        player.Left += (int)(elevatorXdouble - pb.Left - (1 / (multiplierX * 11 / 8)));
                    }

                    if (onBlockLeftExclusive || onBlockRightExclusive)
                    {
                        player.Left += (int)Math.Floor(elevatorXdouble - pb.Left);
                    }
                }
            }
            else
            {
                if (player.Bounds.IntersectsWith(pb.Bounds) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed))
                {
                    player.Left += (int)(elevatorXdouble - pb.Left);
                    player.Top += (int)(elevatorYdouble - pb.Top);
                }

                pb.Left = fromX;
                pb.Top = fromY;
                elevatorMovementSpeed = 0;
                animIndex = 0;
                phase = 0;
                moving = false;
            }
        }

        pb.Left = (int)elevatorXdouble;
        pb.Top = (int)elevatorYdouble;

        #region Pokud v bloku, teleport na blok

        if (playerLeftOffset + elevatorMovementSpeed < pb.Right && player.Right > pb.Left + player.Width / 2 &&
        player.Bottom > pb.Top + 1 && player.Top < pb.Bottom)
        {
            player.Top = (int)Math.Ceiling(elevatorYdouble) - player.Height;
            resetForce = true;
        }

        if (playerRightOffset - elevatorMovementSpeed > pb.Left && player.Left < pb.Right - player.Width / 2 &&
            player.Bottom > pb.Top + 1 && player.Top < pb.Bottom)
        {
            player.Top = (int)Math.Ceiling(elevatorYdouble) - player.Height;
            resetForce = true;
        }

        #endregion

        animIndex++;
    }

    // Collision when travelling with a player
    private void CheckCollision(double vytahXdouble, double vytahYdouble, PictureBox player, int playerLeftOffset, int playerRightOffset, bool grabbed, int movementSpeed)
    {
        if ((player.Bottom > pb.Top + 2) && (player.Top < pb.Bottom) && !grabbed)
        {
            int playerWidthOffset = playerLeftOffset - playerRightOffset;

            // Left side of the block
            if (playerRightOffset > (int)vytahXdouble && player.Left < (int)vytahXdouble)
            {
                // If the player does not move faster than the elevator
                if (!(movementSpeed < -elevatorMovementSpeed))
                {
                    player.Left = (int)vytahXdouble - player.Width;
                }
            }

            // Right side of the block
            if (playerLeftOffset < ((int)vytahXdouble + pb.Width) && player.Right > ((int)vytahXdouble + pb.Width))
            {
                // If the player does not move faster than the elevator
                if (!(movementSpeed > elevatorMovementSpeed))
                {
                    player.Left = ((int)vytahXdouble + pb.Width) + ((player.Width + playerWidthOffset) / 2);
                }
            }
        }
    }
}