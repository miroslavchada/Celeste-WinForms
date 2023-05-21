using Celeste_WinForms.Properties;

namespace Celeste_WinForms;

class Terrain {
    public bool resetForce;
    public bool playerKill;

    // For falling block
    public bool falling;
    public bool fallen;
    public int fallingForce = 0;
    int fallingForceMax = 16;

    public bool fallingOnGround;
    public int fallingGroundedPos;    // Y coords of falling block' bottom

    public bool onFallingBlock;

    int fallAnimIndex = 0;
    int fallAnimDelay = 40;

    // For elevator
    int fromX;
    int fromY;
    int toX;
    int toY;
    public double multiplierX;
    public double multiplierY;

    double elevatorXdouble;
    double elevatorYdouble;

    public int elevatorAnimIndex = 0;
    int elevatorAnimDelay = 50;
    public int elevatorMovementSpeed = 5;
    int elevatorPhase = 0;
    public int elevatorTexturePhase = 0;    // 0 - red, 1 - green, 2 - yellow
    public bool moving;

    public bool onBlockLeftExclusive;
    public bool onBlockRightExclusive;
    public bool onBlockDown;
    bool rodeOnElevator;
    public int movementSpeed;

    public PictureBox pb;

    public Terrain(int posX, int posY, int width, int height, int _toX, int _toY, string tag, Color? color, Image? texture, Panel panel) {
        pb = new PictureBox {
            Left = posX,
            Top = posY,
            Width = width,
            Height = height,
            Tag = tag,
            BackColor = color != null ? (Color)color : Color.Transparent,
            BackgroundImage = texture,
            BackgroundImageLayout = tag.Contains("spikes") ? ImageLayout.Tile : ImageLayout.Stretch
        };

        // For elevator
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

    #region Falling block

    public void FallingAnimation(PictureBox player) {
        // Falling
        if (fallAnimIndex > fallAnimDelay && falling) {
            if (fallingForce < fallingForceMax)
                fallingForce++;

            if (fallingOnGround) {
                falling = false;
                fallen = true;
                pb.Top = fallingGroundedPos;

                if (onFallingBlock)
                    player.Top = fallingGroundedPos - player.Height;
            } else {
                pb.Top += fallingForce;

                if (onFallingBlock)
                    player.Top += fallingForce;
            }
        }

        fallAnimIndex++;
    }

    #endregion Falling block

    #region Elevator

    public void ElevatorAnimation(PictureBox player, bool grabbed, int playerLeftOffset, int playerRightOffset, int movementSpeed) {
        elevatorAnimDelay = 20;
        resetForce = false;

        // Foward
        if (elevatorAnimIndex > elevatorAnimDelay && elevatorPhase == 0) {
            if (!(elevatorMovementSpeed * multiplierX > Math.Abs(toX - pb.Left) ||
                elevatorMovementSpeed * multiplierY > Math.Abs(toY - pb.Top)))    // If it's not close to the target yet
            {
                elevatorXdouble += elevatorMovementSpeed * multiplierX * (pb.Left < toX ? 1 : -1);
                elevatorYdouble += elevatorMovementSpeed * multiplierY * (pb.Top < toY ? 1 : -1);

                CheckCollision(elevatorXdouble, elevatorYdouble, player, playerLeftOffset, playerRightOffset, grabbed, movementSpeed);

                if (player.Bounds.IntersectsWith(pb.Bounds) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed)) {
                    player.Left += (int)(elevatorXdouble - pb.Left);

                    if (onBlockDown)
                        player.Top = (int)pb.Top - player.Height + 1;
                    else
                        player.Top += (int)(elevatorYdouble - pb.Top);
                }
            } else {
                if (player.Bounds.IntersectsWith(pb.Bounds) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed)) {
                    player.Left += (int)(elevatorXdouble - pb.Left);
                    player.Top += (int)(elevatorYdouble - pb.Top);
                }

                pb.Left = toX;
                pb.Top = toY;
                elevatorMovementSpeed = 0;
                elevatorAnimIndex = 0;
                elevatorPhase = 1;
            }

            if (Math.Abs(elevatorMovementSpeed) < 40 && elevatorAnimIndex % 2 == 0) {
                elevatorMovementSpeed++;
            }
        }

        // Backwards
        if (elevatorAnimIndex > elevatorAnimDelay + 40 && elevatorPhase == 1) {
            elevatorMovementSpeed = 3;
            elevatorTexturePhase = 2;

            if (!(elevatorMovementSpeed * multiplierX > Math.Abs(fromX - pb.Left) ||
                elevatorMovementSpeed * multiplierY > Math.Abs(fromY - pb.Top)))    // If it's not close to the target yet
            {
                elevatorXdouble += elevatorMovementSpeed * multiplierX * (pb.Left < fromX ? 1 : -1);
                elevatorYdouble += elevatorMovementSpeed * multiplierY * (pb.Top < fromY ? 1 : -1);

                CheckCollision(elevatorXdouble, elevatorYdouble, player, playerLeftOffset, playerRightOffset, grabbed, movementSpeed);

                if (player.Bounds.IntersectsWith(pb.Bounds) || (player.Bottom <= pb.Top && player.Bottom > pb.Top - 2) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed)) {
                    player.Top += (int)(elevatorYdouble - player.Bottom + 1);

                    if (player.Bounds.IntersectsWith(pb.Bounds)) {
                        player.Left += (int)(elevatorXdouble - pb.Left - (multiplierX > 0.05 ? (1 / (multiplierX * 11 / 8)) : 0));
                    }

                    if (onBlockLeftExclusive || onBlockRightExclusive) {
                        player.Left += (int)Math.Floor(elevatorXdouble - pb.Left);
                    }
                }
            } else {
                if (player.Bounds.IntersectsWith(pb.Bounds) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed)) {
                    player.Left += (int)(elevatorXdouble - pb.Left);
                    player.Top += (int)(elevatorYdouble - pb.Top);
                }

                pb.Left = fromX;
                pb.Top = fromY;
                elevatorMovementSpeed = 0;
                elevatorAnimIndex = 0;
                elevatorPhase = 0;
                elevatorTexturePhase = 0;
                moving = false;
            }
        }

        pb.Left = (int)elevatorXdouble;
        pb.Top = (int)elevatorYdouble;

        #region If in block, teleport on block

        if (playerLeftOffset + elevatorMovementSpeed < pb.Right && player.Right > pb.Left + player.Width / 2 &&
            player.Bottom > pb.Top + 1 && player.Top < pb.Bottom) {
            // Kill player if he is under the block
            if (pb.Bottom - player.Top < 20) {
                playerKill = true;
            } else {
                player.Top = (int)Math.Ceiling(elevatorYdouble) - player.Height;
                resetForce = true;
            }
        }

        if (playerRightOffset - elevatorMovementSpeed > pb.Left && player.Left < pb.Right - player.Width / 2 &&
            player.Bottom > pb.Top + 1 && player.Top < pb.Bottom) {
            // Kill player if he is under the block
            if (pb.Bottom - player.Top < 20) {
                playerKill = true;
            } else {
                player.Top = (int)Math.Ceiling(elevatorYdouble) - player.Height;
                resetForce = true;
            }
        }

        #endregion

        CheckJumpedOff(player);

        elevatorAnimIndex++;
    }

    // Collision with player when travelling
    private void CheckCollision(double vytahXdouble, double vytahYdouble, PictureBox player, int playerLeftOffset, int playerRightOffset, bool grabbed, int movementSpeed) {
        if ((player.Bottom > pb.Top + 2) && (player.Top < vytahYdouble + pb.Height) && !grabbed) {
            int playerWidthOffset = playerLeftOffset - playerRightOffset;

            // Left side of the block
            if (playerRightOffset > (int)vytahXdouble && player.Left < (int)vytahXdouble) {
                // If the player does not move faster than the elevator
                if (!(movementSpeed < -elevatorMovementSpeed)) {
                    player.Left = (int)vytahXdouble - player.Width;
                }
            }

            // Right side of the block
            if (playerLeftOffset < ((int)vytahXdouble + pb.Width) && player.Right > ((int)vytahXdouble + pb.Width)) {
                // If the player does not move faster than the elevator
                if (!(movementSpeed > elevatorMovementSpeed)) {
                    player.Left = ((int)vytahXdouble + pb.Width) + ((player.Width + playerWidthOffset) / 2);
                }
            }
        }
    }

    private void CheckJumpedOff(PictureBox player) {
        if (player.Bounds.IntersectsWith(pb.Bounds) || (onBlockLeftExclusive || onBlockRightExclusive))
            rodeOnElevator = true;

        // Player jumps off when in motion
        if (!(player.Bounds.IntersectsWith(pb.Bounds) || (onBlockLeftExclusive || onBlockRightExclusive)) && moving && rodeOnElevator) {
            rodeOnElevator = false;

            MainWindow.ElevatorJumpedOff(movementSpeed, multiplierX, multiplierY);
        }
    }

    #endregion Elevator
}