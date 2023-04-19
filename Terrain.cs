using System.Numerics;

namespace Celeste_WinForms
{
    class Terrain
    {
        public int vytahMovementSpeed = 5;
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

        double vytahXdouble;
        double vytahYdouble;

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

            vytahXdouble = posX;
            vytahYdouble = posY;

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

        public void VytahAnimation(PictureBox player, bool grabbed, int playerLeftOffset, int playerRightOffset, int movementSpeed)
        {
            resetForce = false;

            // Vpřed
            if (animIndex > animDelay && phase == 0)
            {
                if (!(vytahMovementSpeed * multiplierX > Math.Abs(toX - pb.Left) ||
                    vytahMovementSpeed * multiplierY > Math.Abs(toY - pb.Top)))    // Pokud ještě není blízko cíli
                {
                    vytahXdouble += vytahMovementSpeed * multiplierX * (pb.Left < toX ? 1 : -1);
                    vytahYdouble += vytahMovementSpeed * multiplierY * (pb.Top < toY ? 1 : -1);

                    CheckCollision(vytahXdouble, vytahYdouble, player, playerLeftOffset, playerRightOffset, grabbed, movementSpeed);

                    if (player.Bounds.IntersectsWith(pb.Bounds) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed))
                    {
                        player.Left += (int)(vytahXdouble - pb.Left);

                        if (onBlockDown)
                            player.Top = (int)pb.Top - player.Height + 1;
                        else
                            player.Top += (int)(vytahYdouble - pb.Top);
                    }
                }
                else
                {
                    if (player.Bounds.IntersectsWith(pb.Bounds) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed))
                    {
                        player.Left += (int)(vytahXdouble - pb.Left);
                        player.Top += (int)(vytahYdouble - pb.Top);
                    }

                    pb.Left = toX;
                    pb.Top = toY;
                    vytahMovementSpeed = 0;
                    animIndex = 0;
                    phase = 1;
                }

                if (Math.Abs(vytahMovementSpeed) < 40 && animIndex % 2 == 0)
                {
                    vytahMovementSpeed++;
                }
            }

            // Zpět
            if (animIndex > animDelay && phase == 1)
            {
                vytahMovementSpeed = 3;

                if (!(vytahMovementSpeed * multiplierX > Math.Abs(fromX - pb.Left) ||
                    vytahMovementSpeed * multiplierY > Math.Abs(fromY - pb.Top)))    // Pokud ještě není blízko cíli
                {
                    vytahXdouble += vytahMovementSpeed * multiplierX * (pb.Left < fromX ? 1 : -1);
                    vytahYdouble += vytahMovementSpeed * multiplierY * (pb.Top < fromY ? 1 : -1);

                    CheckCollision(vytahXdouble, vytahYdouble, player, playerLeftOffset, playerRightOffset, grabbed, movementSpeed);

                    if (player.Bounds.IntersectsWith(pb.Bounds) || (player.Bottom <= pb.Top && player.Bottom > pb.Top - 2) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed))
                    {
                        player.Top += (int)(vytahYdouble - pb.Top + ((player.Bottom <= pb.Top && player.Bottom > pb.Top - 2) ? pb.Top - player.Bottom + 1 : 0));

                        if (player.Bounds.IntersectsWith(pb.Bounds))
                        {
                            player.Left += (int)(vytahXdouble - pb.Left - (1 / (multiplierX * 11 / 8)));
                        }

                        if (onBlockLeftExclusive || onBlockRightExclusive)
                        {
                            player.Left += (int)Math.Floor(vytahXdouble - pb.Left);
                        }
                    }
                }
                else
                {
                    if (player.Bounds.IntersectsWith(pb.Bounds) || ((onBlockLeftExclusive || onBlockRightExclusive) && grabbed))
                    {
                        player.Left += (int)(vytahXdouble - pb.Left);
                        player.Top += (int)(vytahYdouble - pb.Top);
                    }

                    pb.Left = fromX;
                    pb.Top = fromY;
                    vytahMovementSpeed = 0;
                    animIndex = 0;
                    phase = 0;
                    moving = false;
                }
            }

            pb.Left = (int)vytahXdouble;
            pb.Top = (int)vytahYdouble;

            #region Pokud v bloku, teleport na blok

            if (playerLeftOffset + vytahMovementSpeed < pb.Right && player.Right > pb.Left + player.Width / 2 &&
            player.Bottom > pb.Top + 1 && player.Top < pb.Bottom)
            {
                player.Top = (int)Math.Ceiling(vytahYdouble) - player.Height;
                resetForce = true;
            }

            if (playerRightOffset - vytahMovementSpeed > pb.Left && player.Left < pb.Right - player.Width / 2 &&
                player.Bottom > pb.Top + 1 && player.Top < pb.Bottom)
            {
                player.Top = (int)Math.Ceiling(vytahYdouble) - player.Height;
                resetForce = true;
            }

            #endregion

            animIndex++;
        }

        // Kolize při pojezdu s hráčem
        private void CheckCollision(double vytahXdouble, double vytahYdouble, PictureBox player, int playerLeftOffset, int playerRightOffset, bool grabbed, int movementSpeed)
        {
            if ((player.Bottom > pb.Top + 2) && !grabbed)
            {
                int playerWidthOffset = playerLeftOffset - playerRightOffset;

                // Levá strana bloku
                if (playerRightOffset > (int)vytahXdouble && player.Left < (int)vytahXdouble)
                {
                    // Pokud se nepohybuje rychleji, než výtah
                    if (!(movementSpeed < -vytahMovementSpeed))
                    {
                        player.Left = (int)vytahXdouble - player.Width;
                    }
                }

                // Pravá strana bloku
                if (playerLeftOffset < ((int)vytahXdouble + pb.Width) && player.Right > ((int)vytahXdouble + pb.Width))
                {
                    // Pokud se nepohybuje rychleji, než výtah
                    if (!(movementSpeed > vytahMovementSpeed))
                    {
                        player.Left = ((int)vytahXdouble + pb.Width) + ((player.Width + playerWidthOffset) / 2);
                    }
                }
            }
        }
    }
}