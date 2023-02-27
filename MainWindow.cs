using Celeste_WinForms.Properties;

namespace Celeste_WinForms;

public partial class MainWindow : Form
{
    // Glob�ln� prom�nn�
    /// Hr��
    bool inputEnabled = false;

    bool left, right;
    bool leftInput, rightInput;
    bool upInput, downInput;

    bool jump, jumpInput, spaceReleased = true;
    bool slide;
    bool grab, grabInput;

    string facing = "", lastStraightFacing = "Right";
    bool midAir;
    bool jumpCooldown = false;
    bool grabAfterJumpCooldown = false;
    int movementSpeed;
    int movementSpeedTarget = 0;
    int movementSpeedMax;
    int movementSpeedMaxTarget = 6;
    int force;

    bool closeToBlockLeft, closeToBlockRight;
    int closeToBlockLeftDist, closeToBlockRightDist;
    bool onBlockLeft, onBlockRight;
    bool onBlockDown;
    int playerBlockHeightDiff;
    bool climbed;
    string lastGrabbedOn = "";

    bool dashInput, ctrlReleased = true;
    bool dashed;
    bool dashedNonVertical;

    int playerLeftOffset, playerRightOffset;

    /// Level
    int currentLevel = 1;

    Terrain[] terrainArray;

    /// Zvuky
    bool grabbedOn = false;
    bool touchedGround = true;

    /// Kamera
    int cameraMovementSpeed, cameraMovementSpeedTarget;
    int playerCenterY;

    /// Tagy <summary>
    /// "collision" - na objekt se vztahuj� kolize
    /// "spring" - pru�ina
    /// </summary>

    public MainWindow()
    {
        InitializeComponent();

        Level1();
        player.Top = 702;
        player.Left = 160;

        menuStartContainer.Enabled = true; menuStartContainer.Visible = true;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        // Movement

        // Z�kladn� hodnoty
        playerLeftOffset = player.Left + 0;
        playerRightOffset = player.Right - 0;
        playerCenterY = player.Top + player.Height / 2;
        facing = "";
        movementSpeedTarget = 0;
        midAir = true;
        closeToBlockLeft = false; closeToBlockRight = false;
        closeToBlockLeftDist = movementSpeed; closeToBlockRightDist = movementSpeed;
        onBlockLeft = false; onBlockRight = false;
        playerBlockHeightDiff = 0;
        slide = false;
        grab = false;

        // Kurzor
        Point cursor = this.PointToClient(Cursor.Position);

        updateCamera();

        // Pohyb
        /// Inputy
        if (leftInput)
        {
            left = true;
            facing = "Left";
            lastStraightFacing = "Left";
        }
        else
            left = false;

        if (rightInput)
        {
            right = true;
            facing = "Right";
            lastStraightFacing = "Right";
        }
        else
            right = false;

        if (upInput)
            facing += "Up";
        else if (downInput)
            facing += "Down";

        if (facing == "")
            facing = lastStraightFacing;

        // Interakce s bloky
        foreach (PictureBox block in gameScreen.Controls.OfType<PictureBox>().Where(block => block.Tag != null))
        {
            // Postrann� kolize
            if (block.Tag.ToString().Contains("collision") && player.Bounds.IntersectsWith(block.Bounds))
            {
                if (playerLeftOffset < block.Right && player.Right > block.Left + player.Width / 2 &&
                    player.Bottom > block.Top + 1 && player.Top < block.Bottom)
                {
                    left = false;
                }

                if (playerRightOffset > block.Left && player.Left < block.Right - player.Width / 2 &&
                    player.Bottom > block.Top + 1 && player.Top < block.Bottom)
                {
                    right = false;
                }
            }

            // Pokud je hr�� bl�zko k bloku, p�ibl�� se pouze o rozd�l mezi hranou hr��e a bloku (proti bug�m)
            if (block.Tag.ToString().Contains("collision"))
            {
                if (playerLeftOffset - block.Right < movementSpeedMax && playerLeftOffset - block.Right >= 0 &&
                    player.Bottom > block.Top + 1 && player.Top < block.Bottom)
                {
                    closeToBlockLeft = true;
                    closeToBlockLeftDist = playerLeftOffset - block.Right;

                    if (playerLeftOffset - block.Right == 0 &&
                        player.Top + player.Height / 2 < block.Bottom)
                    {
                        onBlockLeft = true;
                        midAir = false;
                        playerBlockHeightDiff = player.Bottom - block.Top;
                    }
                }

                if (block.Left - playerRightOffset < movementSpeedMax && block.Left - playerRightOffset >= 0 &&
                    player.Bottom > block.Top + 1 && player.Top < block.Bottom)
                {
                    closeToBlockRight = true;
                    closeToBlockRightDist = block.Left - playerRightOffset;

                    if (block.Left - playerRightOffset == 0 &&
                        player.Top + player.Height / 2 < block.Bottom)
                    {
                        onBlockRight = true;
                        midAir = false;
                        playerBlockHeightDiff = player.Bottom - block.Top;
                    }
                }

                // midAir
                if (block.Top - player.Bottom == -1 &&
                    playerLeftOffset < block.Right && playerRightOffset > block.Left)
                {
                    midAir = false;
                }
            }

            // Slide aktivace
            if (((onBlockLeft && leftInput) || (onBlockRight && rightInput)) && force < 0)
            {
                slide = true;
                midAir = false;
            }

            // Grab aktivace
            if (grabInput && (onBlockLeft || onBlockRight))
            {
                grab = true;
                midAir = false;

                lastGrabbedOn = onBlockLeft ? "Left" : onBlockRight ? "Right" : "";
            }
        }

        // Hr�� dr�� mezern�k
        if (jumpInput && !midAir && !jumpCooldown && !grabAfterJumpCooldown)
        {
            jump = true;
            force = 15;
            playSound("jumped");

            if (onBlockLeft || onBlockRight)
            {
                if (!onBlockDown)
                {
                    movementSpeed = onBlockLeft ? movementSpeedMax * 2 : onBlockRight ? -movementSpeedMax * 2 : 0;

                    force = 11;
                }
            }

            if (slide)
            {
                movementSpeed = onBlockLeft ? movementSpeedMax * 2 : onBlockRight ? -movementSpeedMax * 2 : 0;

                force = 11;
            }

            if (grab)
            {
                if (onBlockLeft && facing.Contains("Right") || onBlockRight && facing.Contains("Left"))
                    force = 13;
                else
                    force = 11;

                movementSpeed += facing.Contains("Left") ? -movementSpeedMax * 2 : facing.Contains("Right") ? movementSpeedMax * 2 : 0;

                grab = false;
                grabAfterJumpCooldown = true;
                timerGrabAfterJumpCooldown.Enabled = true;
            }

            jumpCooldown = true;
            timerJumpCooldown.Enabled = true;
        }

        onBlockDown = false;

        // Grab - V�skok na horn� hran� bloku (<25px spodek hr��e - vr�ek bloku)
        if (grab && !grabAfterJumpCooldown)
        {
            if (playerBlockHeightDiff < 25)
            {
                force = playerBlockHeightDiff / 2;
                climbed = true;

                grab = false;
                grabAfterJumpCooldown = true;
                timerGrabAfterJumpCooldown.Enabled = true;
            }
        }
        if (climbed && !(onBlockLeft || onBlockRight))
        {
            movementSpeed = lastGrabbedOn == "Left" ? -12 : lastGrabbedOn == "Right" ? 12 : 0;
            climbed = false;
            lastGrabbedOn = "";
        }

        // Funkce Dash
        if (dashInput && !dashed)
        {
            switch (facing)
            {
                case "Right":
                    movementSpeed = 4 * movementSpeedMax;
                    force = 0;

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "RightUp":
                    movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * 4 * movementSpeedMax);
                    force = movementSpeed;

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "Up":
                    movementSpeed = 0;
                    force = 22;
                    break;

                case "LeftUp":
                    movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * -4 * movementSpeedMax);
                    force = -movementSpeed;

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "Left":
                    movementSpeed = -4 * movementSpeedMax;
                    force = 0;

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "LeftDown":
                    movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * -4 * movementSpeedMax);
                    force = movementSpeed;

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "Down":
                    movementSpeed = 0;
                    force = -22;
                    break;

                case "RightDown":
                    movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * 4 * movementSpeedMax);
                    force = -movementSpeed;

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;
            }

            if (Math.Abs(movementSpeed) > movementSpeedMaxTarget)
                movementSpeedMax = Math.Abs(movementSpeed);

            playSound("dash");
            dashed = true;
        }

        // Funkce Grab
        if (grab && !grabAfterJumpCooldown && Math.Abs(movementSpeed) < movementSpeedMax)
        {
            playSound("grabOn");

            if (!(leftInput || rightInput || upInput || downInput))
                facing = onBlockLeft ? "Left" : onBlockRight ? "Right" : lastStraightFacing;

            if (facing == "Up")
                player.Top -= movementSpeedMax / 5 * 4;

            if (facing == "Down")
                player.Top += movementSpeedMax / 5 * 4;

            movementSpeedTarget = 0;
            force = 0;
        }
        else  // Gravitace
        {
            player.Top -= force;
            if (force > (slide ? -2 : -25) && !dashedNonVertical)
                force -= 1;
        }

        foreach (PictureBox block in gameScreen.Controls.OfType<PictureBox>().Where(block => block.Tag != null))
        {
            // Interakce s bloky
            if (block.Tag.ToString().Contains("collision") && player.Bounds.IntersectsWith(block.Bounds) &&
                playerLeftOffset < block.Right &&
                playerRightOffset > block.Left)
            {
                // Vrchn� kolize
                if (player.Bottom >= block.Top && player.Top < block.Top) /// Zeshora
                {
                    player.Top = block.Top - player.Height + 1;
                    force = 0;
                    jump = false;

                    onBlockDown = true;
                    dashed = false;

                    if (!block.Tag.ToString().Contains("spring"))
                    {
                        if (!touchedGround)
                            playSound("landed");
                        touchedGround = true;
                    }
                }

                // Spodn� kolize
                if (player.Top < block.Bottom && player.Bottom > block.Bottom) /// Zespodu
                {
                    if (force > 3)
                        force = -3;
                    else
                        force *= -1;

                    player.Top = block.Bottom;

                    jumpCooldown = true;
                    timerJumpHeadBumpCooldown.Enabled = true;
                }
            }

            if (block.Tag.ToString().Contains("spring") && player.Bounds.IntersectsWith(block.Bounds) &&
                playerLeftOffset < block.Right &&
                playerRightOffset - 1 > block.Left)
            {
                force = 30;
                jump = true;

                playSound("spring");
            }
        }

        // Pohyb do stran
        if (left ^ right && !slide && !grab)
        {
            if (left)
            {
                movementSpeedTarget = -movementSpeedMax;

                if (closeToBlockLeft)
                {
                    movementSpeed = 0;
                    player.Left -= closeToBlockLeftDist;
                }
                else if (movementSpeed != movementSpeedTarget)
                {
                    movementSpeed += movementSpeed < movementSpeedTarget ? 1 : movementSpeed > movementSpeedTarget ? -1 : 0;

                    if (!midAir)
                        movementSpeed = movementSpeedTarget;
                }
            }

            if (right)
            {
                movementSpeedTarget = movementSpeedMax;

                if (closeToBlockRight)
                {
                    movementSpeed = 0;
                    player.Left += closeToBlockRightDist;
                }
                else if (movementSpeed != movementSpeedTarget)
                {
                    movementSpeed += movementSpeed < movementSpeedTarget ? 1 : movementSpeed > movementSpeedTarget ? -1 : 0;

                    if (!midAir)
                        movementSpeed = movementSpeedTarget;
                }
            }
        }
        else
        {
            if (movementSpeed != 0)
            {
                movementSpeedTarget = 0;

                if (movementSpeed < -closeToBlockLeftDist && closeToBlockLeft)
                {
                    movementSpeed = 0;
                    player.Left -= closeToBlockLeftDist;
                }
                else if (movementSpeed > closeToBlockRightDist && closeToBlockRight)
                {
                    movementSpeed = 0;
                    player.Left += closeToBlockRightDist;
                }
                else
                    movementSpeed += movementSpeed < movementSpeedTarget ? 1 : movementSpeed > movementSpeedTarget ? -1 : 0;
            }
        }
        player.Left += movementSpeed;


        // V�voj��sk� statistiky [F3]
        lbDeveloperStats.Text =
            $"Cursor: [{cursor.X}; {cursor.Y}]" +
            $"\r\nPlayer: [{player.Left}; {player.Bottom}]" +
            $"\r\nMovementSpeed: {movementSpeed}" +
            $"\r\nMovementSpeedTarget: {movementSpeedTarget}" +
            $"\r\nForce: {force}" +
            $"\r\nFacing: {facing}" +
            $"\r\nJump: {jump}" +
            $"\r\nJumpInput: {jumpInput}" +
            $"\r\nMidAir: {midAir}" +
            $"\r\nJumpCooldown: {jumpCooldown}" +
            $"\r\nCloseToBlock: {(closeToBlockLeft ? "Left" : closeToBlockRight ? "Right" : "none")}" +
            $"\r\nOnBlock: {(onBlockLeft ? "Left" : onBlockRight ? "Right" : "none")}" +
            $"\r\nOnBlockDown: {onBlockDown}" +
            $"\r\nPlayerBlockHeightDiff: {playerBlockHeightDiff}" +
            $"\r\nGrabInput: {grabInput}" +
            $"\r\nGrab: {grab}" +
            $"\r\nGrabAfterJumpCooldown: {grabAfterJumpCooldown}" +
            $"\r\nLastGrabbedOn: {lastGrabbedOn}" +
            $"\r\nDashInput: {dashInput}" +
            $"\r\nDashed: {dashed}" +
            $"\r\nGameScreen.Top: {gameScreen.Top}" +
            $"\r\nCameraMovementSpeed: {cameraMovementSpeed}" +
            $"\r\nCameraMovementSpeedTarget: {cameraMovementSpeedTarget}";


        if (movementSpeedMax != movementSpeedMaxTarget)
        {
            if (movementSpeedMax > movementSpeedMaxTarget)
                movementSpeedMax--;
            else
                movementSpeedMax = movementSpeedMaxTarget;
        }

        jumpInput = false;
        dashInput = false;

        if (!onBlockDown)
            touchedGround = false;
    }

    private void updateCamera()
    {
        // Kamera
        cameraMovementSpeedTarget = (432 - playerCenterY) - (gameScreen.Top);

        if (gameScreen.Top >= 0 && playerCenterY < 432)
        {
            cameraMovementSpeedTarget = 0;
            cameraMovementSpeed = 0;
        }
        else if (gameScreen.Bottom <= 864 && gameScreen.Height - playerCenterY < 432)
        {
            cameraMovementSpeedTarget = 0;
            cameraMovementSpeed = 0;
        }

        if (cameraMovementSpeed != cameraMovementSpeedTarget)
        {
            if (cameraMovementSpeed < cameraMovementSpeedTarget)
            {
                cameraMovementSpeed += cameraMovementSpeedTarget / 10 + (force > 10 ? (force - 10) : 0) - cameraMovementSpeed;
            }
            else if (cameraMovementSpeed > cameraMovementSpeedTarget)
            {
                cameraMovementSpeed -= cameraMovementSpeed - cameraMovementSpeedTarget / 10 + (force < -10 ? (-force - 10) : 0);
            }
        }

        /// Fix na hran� obrazovky proti viditeln�mu zaseknut�
        if (cameraMovementSpeedTarget > 0 && gameScreen.Top >= 0 - cameraMovementSpeed && playerCenterY < 432)
        {
            gameScreen.Top = 0;
        }
        else if (cameraMovementSpeedTarget < 0 && gameScreen.Bottom <= 864 - cameraMovementSpeed && gameScreen.Height - playerCenterY < 432)
        {
            gameScreen.Top = 864 - gameScreen.Height;
        }
        else
        {
            gameScreen.Top += cameraMovementSpeed;
        }
    }

    // Pokud se bouchne hlavou o spodek bloku
    private void timerJumpHeadBumpCooldown_Tick(object sender, EventArgs e) /// 300ms
    {
        jumpCooldown = false;
        timerJumpHeadBumpCooldown.Enabled = false;
    }

    // Cooldown na Grab po v�skoku z Grabu
    private void timerGrabCooldown_Tick(object sender, EventArgs e)
    {
        grabAfterJumpCooldown = false;
        timerGrabAfterJumpCooldown.Enabled = false;
    }

    // Obecn� funkce formu

    // Reset obrazovky
    private void menuEscapeBtResetScreen_Click(object sender, EventArgs e)
    {

    }

    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (inputEnabled)
        {
            if (e.KeyCode == Keys.A)
                leftInput = true;

            if (e.KeyCode == Keys.D)
                rightInput = true;

            if (e.KeyCode == Keys.W)
                upInput = true;

            if (e.KeyCode == Keys.S)
                downInput = true;

            if (e.KeyCode == Keys.Space && spaceReleased)
            {
                jumpInput = true;
                spaceReleased = false;
            }

            if (e.KeyCode == Keys.ControlKey && ctrlReleased)
            {
                dashInput = true;
                ctrlReleased = false;
            }

            if (e.KeyCode == Keys.ShiftKey)
                grabInput = true;

            if (e.KeyCode == Keys.F3) // Developer stats
            {
                lbDeveloperStats.Visible = !lbDeveloperStats.Visible;
                lbDeveloperSounds.Visible = !lbDeveloperSounds.Visible;
            }
        }

        if (e.KeyCode == Keys.Escape && !menuStartContainer.Enabled)
        {
            if (menuEscapeContainer.Enabled || menuControlsContainer.Enabled)
            {
                menuEscapeContinue();
            }
            else
            {
                menuEscapeContainer.Enabled = true; menuEscapeContainer.Visible = true;
                gameScreen.Enabled = false; gameScreen.Visible = false;
                timer1.Enabled = false;
                inputEnabled = false;
            }
        }
    }

    private void MainWindow_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.A)
            leftInput = false;

        if (e.KeyCode == Keys.D)
            rightInput = false;

        if (e.KeyCode == Keys.W)
            upInput = false;

        if (e.KeyCode == Keys.S)
            downInput = false;

        if (e.KeyCode == Keys.Space)
            spaceReleased = true;

        if (e.KeyCode == Keys.ControlKey)
            ctrlReleased = true;

        if (e.KeyCode == Keys.ShiftKey)
        {
            grabInput = false;
            playSound("grabOff");
        }
    }

    private void timerDashedNonVertical_Tick(object sender, EventArgs e)
    {
        dashedNonVertical = false;
        timerDashedNonVertical.Enabled = false;
    }

    private void timerJumpCooldown_Tick(object sender, EventArgs e)
    {
        jumpInput = false;
        jumpCooldown = false;
        timerJumpCooldown.Enabled = false;
    }

    private void menuEscapeContinue()
    {
        menuControlsContainer.Enabled = false; menuControlsContainer.Visible = false;
        menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;
        gameScreen.Enabled = true; gameScreen.Visible = true;
        timer1.Enabled = true;
        inputEnabled = true;
        this.Focus();
    }

    private void menuStartBtPlay_Click(object sender, EventArgs e)
    {
        menuStartContainer.Enabled = false;
        menuStartContainer.Visible = false;
        gameScreen.Enabled = true;
        gameScreen.Visible = true;
        timer1.Enabled = true;
        inputEnabled = true;
        this.Focus();
    }

    private void menuStartBtClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void menuEscapeBtContinue_Click(object sender, EventArgs e)
    {
        menuEscapeContinue();
    }

    private void menuEscapeBtControls_Click(object sender, EventArgs e)
    {
        menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;
        menuControlsContainer.Enabled = true; menuControlsContainer.Visible = true;
        this.Focus();
    }

    private void menuEscapeBtStartMenu_Click(object sender, EventArgs e)
    {
        menuControlsContainer.Enabled = false; menuControlsContainer.Visible = false;
        menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;

        timer1.Enabled = false;
        inputEnabled = false;

        gameScreen.Enabled = false; gameScreen.Visible = false;
        menuStartContainer.Enabled = true;
        menuStartContainer.Visible = true;

        this.Focus();
    }

    private void menuEscapeControlsBtEscapeMenu_Click(object sender, EventArgs e)
    {
        menuControlsContainer.Enabled = false; menuControlsContainer.Visible = false;
        menuEscapeContainer.Enabled = true; menuEscapeContainer.Visible = true;

        this.Focus();
    }

    private void DestroyAll(PictureBox pb, Panel panel)
    {
        pb.Bounds = Rectangle.Empty;
        panel.Controls.Remove(pb);
        pb.Dispose();
    }

    // Level design

    private void Level1()
    {
        gameScreen.Height = 1677;

        Terrain pictureBox1 = new(0, 1581, 1382, 96, "collision", Color.FromArgb(28, 28, 28), true, Resources.bg_mario_h540, gameScreen);
        Terrain pictureBox2 = new(337, 1530, 77, 51, "collision", Color.FromArgb(28, 28, 28), false, Resources.blank, gameScreen);
        Terrain pictureBox3 = new(869, 9, 222, 1572, "collision", Color.FromArgb(28, 28, 28), false, Resources.blank, gameScreen);
        Terrain pictureBox4 = new(67, 1408, 66, 113, "collision", Color.FromArgb(28, 28, 28), false, Resources.blank, gameScreen);
        Terrain pictureBox5 = new(241, 1046, 66, 113, "collision", Color.FromArgb(28, 28, 28), false, Resources.blank, gameScreen);
        Terrain pictureBox6 = new(355, 1510, 51, 20, "collision spring", Color.FromArgb(154, 205, 50), false, Resources.blank, gameScreen);
        Terrain pictureBox7 = new(67, 549, 66, 113, "collision", Color.FromArgb(28, 28, 28), false, Resources.blank, gameScreen);
        Terrain pictureBox8 = new(135, 149, 66, 113, "collision", Color.FromArgb(28, 28, 28), false, Resources.blank, gameScreen);

        terrainArray = new Terrain[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, };

        player.Left = 171;
        player.Top = 1514;
    }

    // Pozad�
    private void gameScreen_Paint(object sender, PaintEventArgs e)
    {
        Graphics canvas = e.Graphics;

        // canvas.DrawImage(Resources.bg_mario_h540, new Rectangle(0, 0, gameScreen.Width, gameScreen.Height));
    }

    private void playSound(string sound)
    {
        switch (sound)
        {
            case "jumped":
                lbDeveloperSounds.Text = $"{sound}\r\n" + lbDeveloperSounds.Text;
                break;

            case "landed":
                lbDeveloperSounds.Text = $"{sound}\r\n" + lbDeveloperSounds.Text;
                break;

            case "spring":
                lbDeveloperSounds.Text = $"{sound}\r\n" + lbDeveloperSounds.Text;
                break;

            case "grabOn":
                if (!grabbedOn)
                {
                    lbDeveloperSounds.Text = $"{sound}\r\n" + lbDeveloperSounds.Text;
                }
                grabbedOn = true;
                break;

            case "grabOff":
                if (grabbedOn)
                {
                    lbDeveloperSounds.Text = $"{sound}\r\n" + lbDeveloperSounds.Text;
                }
                grabbedOn = false;
                break;

            case "dash":
                lbDeveloperSounds.Text = $"{sound}\r\n" + lbDeveloperSounds.Text;
                break;
        }
    }
}