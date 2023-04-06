using Celeste_WinForms.Properties;

namespace Celeste_WinForms;

public partial class MainWindow : Form
{
    // Glob�ln� prom�nn�
    /// Texty programu
    List<string> texts = Resources.texts.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

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
    int movementSpeedMax = 6;
    int force;

    bool closeToBlockLeft, closeToBlockRight;
    int closeToBlockLeftDist, closeToBlockRightDist;
    bool onBlockLeft, onBlockRight;
    bool closeToBlockDown, closeToBlockUp;
    int closeToBlockDownDist, closeToBlockUpDist;
    bool onBlockDown, onBlockUp;
    int playerBlockHeightDiff;
    bool climbed;
    string lastGrabbedOn = "";

    // O�et�en� bugu dashe
    bool dashCloseToBlockRight, dashCloseToBlockDown, dashCloseToBlockLeft, dashCloseToBlockUp;
    bool dashCloseToBlockRightDownRight, dashCloseToBlockRightDownDown, dashCloseToBlockLeftDownLeft, dashCloseToBlockLeftDownDown, dashCloseToBlockLeftUpLeft, dashCloseToBlockLeftUpUp, dashCloseToBlockRightUpRight, dashCloseToBlockRightUpUp;
    int dashCloseToBlockRightDist, dashCloseToBlockDownDist, dashCloseToBlockLeftDist, dashCloseToBlockUpDist;
    int dashCloseToBlockRightDownRightDist, dashCloseToBlockRightDownDownDist, dashCloseToBlockLeftDownLeftDist, dashCloseToBlockLeftDownDownDist, dashCloseToBlockLeftUpLeftDist, dashCloseToBlockLeftUpUpDist, dashCloseToBlockRightUpRightDist, dashCloseToBlockRightUpUpDist;

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


    bool developerKeys;   // NumPad0 stisknuta

    /// Tagy <summary>
    /// "collision" - na objekt se vztahuj� kolize
    /// "spring" - pru�ina
    /// jump-through - prosko�iteln� platforma
    /// </summary>

    public MainWindow()
    {
        InitializeComponent();

        LoadTexts("en");
        Level1();

        menuMainContainer.Enabled = true; menuMainContainer.Visible = true;
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
        closeToBlockLeftDist = 0;
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
            if (!block.Tag.ToString().Contains("jump-through"))
            {
                // Postrann� kolize
                if (block.Tag.ToString().Contains("collision") && player.Bounds.IntersectsWith(block.Bounds))
                {
                    if (playerLeftOffset < block.Right && player.Right > block.Left + player.Width / 2 &&
                        player.Bottom > block.Top + 1 && player.Top < block.Bottom)
                    {
                        left = false;
                        movementSpeed = 0;
                    }

                    if (playerRightOffset > block.Left && player.Left < block.Right - player.Width / 2 &&
                        player.Bottom > block.Top + 1 && player.Top < block.Bottom)
                    {
                        right = false;
                        movementSpeed = 0;
                    }
                }

                // Pokud je hr�� bl�zko k bloku, p�ibl�� se pouze o rozd�l mezi hranou hr��e a bloku (proti bug�m)
                if (block.Tag.ToString().Contains("collision"))
                {
                    if (playerLeftOffset - block.Right < (Math.Abs(movementSpeed) < movementSpeedMax ? movementSpeedMax : Math.Abs(movementSpeed)) && playerLeftOffset - block.Right >= 0 &&
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

                    if (block.Left - playerRightOffset < (movementSpeed < movementSpeedMax ? movementSpeedMax : movementSpeed) && block.Left - playerRightOffset >= 0 &&
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

            // midAir
            if (block.Top - player.Bottom == -1 &&
                playerLeftOffset < block.Right && playerRightOffset > block.Left)
            {
                midAir = false;
            }
        }

        // Hr�� dr�� mezern�k
        if (jumpInput && !midAir && !jumpCooldown && !grabAfterJumpCooldown)
        {
            jump = true;
            force = 15;
            PlaySound("jumped");

            if ((onBlockLeft || onBlockRight) && !onBlockDown)
            {
                movementSpeed = onBlockLeft ? movementSpeedMax * 2 : onBlockRight ? -movementSpeedMax * 2 : 0;

                force = 11;
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
                    if (!onBlockRight)
                        movementSpeed = (4 * movementSpeedMax);
                    force = 0;

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "RightUp":
                    if (!onBlockRight)
                        movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * 4 * movementSpeedMax);
                    force = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * 4 * movementSpeedMax);

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "Up":
                    movementSpeed = 0;
                    force = 22;
                    break;

                case "LeftUp":
                    if (!onBlockLeft)
                        movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * -4 * movementSpeedMax);
                    force = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * 4 * movementSpeedMax);

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "Left":
                    if (!onBlockLeft)
                        movementSpeed = -4 * movementSpeedMax;
                    force = 0;

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "LeftDown":
                    if (!onBlockLeft)
                        movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * -4 * movementSpeedMax);
                    if (!onBlockDown)
                        force = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * -4 * movementSpeedMax);

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "Down":
                    movementSpeed = 0;
                    if (!onBlockDown)
                        force = -22;
                    break;

                case "RightDown":
                    if (!onBlockRight)
                        movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * 4 * movementSpeedMax);
                    if (!onBlockDown)
                        force = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * -4 * movementSpeedMax);

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;
            }

            PlaySound("dash");
            dashed = true;
        }

        // Funkce Grab
        if (grab && !grabAfterJumpCooldown && Math.Abs(movementSpeed) < movementSpeedMax)
        {
            PlaySound("grabOn");

            force = 0;
            movementSpeedTarget = 0;

            if (!(leftInput || rightInput || upInput || downInput))
                facing = onBlockLeft ? "Left" : onBlockRight ? "Right" : lastStraightFacing;

            if (facing == "Up" && !onBlockUp)
            {
                if (!onBlockUp)
                    force = movementSpeedMax / 5 * 4;
                else
                    force = closeToBlockUpDist;
            }

            if (facing == "Down" && !onBlockDown)
            {
                if (!closeToBlockDown)
                    force = -movementSpeedMax / 5 * 4;
                else
                    force = -closeToBlockDownDist;
            }

            player.Top -= force;
        }
        else  // Gravitace
        {
            if (closeToBlockDown)
            {
                player.Top += closeToBlockDownDist - 1;
                force = 0;
            }
            else if (closeToBlockUp)
            {
                player.Top -= closeToBlockUpDist;
            }

            player.Top -= force;

            if (force > (slide ? -2 : -25) && !dashedNonVertical & !closeToBlockDown)
            {
                force -= 1;
            }
        }

        closeToBlockDown = false;
        onBlockDown = false;
        onBlockUp = false;

        foreach (PictureBox block in gameScreen.Controls.OfType<PictureBox>().Where(block => block.Tag != null))
        {
            // Pokud je hr�� bl�zko k bloku, p�ibl�� se pouze o rozd�l mezi hranou hr��e a bloku (proti bug�m)
            if (block.Tag.ToString().Contains("collision"))
            {
                if (block.Top + 1 - player.Bottom <= -force + 1 &&
                    playerLeftOffset <= block.Right && playerRightOffset >= block.Left &&
                    player.Bottom < block.Top)
                {
                    closeToBlockDown = true;
                    closeToBlockDownDist = block.Top - player.Bottom + 1;
                }
            }

            // Interakce s bloky
            if (block.Tag.ToString().Contains("collision") && player.Bounds.IntersectsWith(block.Bounds) &&
                playerLeftOffset < block.Right &&
                playerRightOffset > block.Left)
            {
                // Vrchn� kolize
                if (player.Bottom == block.Top + 1 && player.Top < block.Top) /// Zeshora
                {
                    player.Top = block.Top - player.Height + 1;
                    force = 0;
                    jump = false;

                    onBlockDown = true;
                    closeToBlockDown = false;
                    dashed = false;

                    if (!block.Tag.ToString().Contains("spring"))
                    {
                        if (!touchedGround)
                            PlaySound("landed");
                        touchedGround = true;
                    }
                }

                // Pokud je hr�� bl�zko k bloku, p�ibl�� se pouze o rozd�l mezi hranou hr��e a bloku (proti bug�m)
                if (block.Tag.ToString().Contains("collision") && !block.Tag.ToString().Contains("jump-through"))
                {
                    if (player.Top - block.Bottom <= force + 1 &&
                        playerLeftOffset <= block.Right && playerRightOffset >= block.Left &&
                        player.Bottom < block.Top)
                    {
                        closeToBlockUp = true;
                        closeToBlockUpDist = player.Top - block.Bottom;

                        if (player.Top - block.Bottom == 0)
                        {
                            onBlockUp = true;
                        }
                    }
                }

                // Spodn� kolize
                if ((player.Top < block.Bottom && player.Bottom > block.Bottom) && !block.Tag.ToString().Contains("jump-through")) /// Zespodu
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

                PlaySound("spring");
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
            $"\r\nCameraMovementSpeed: {cameraMovementSpeed}" +
            $"\r\nCameraMovementSpeedTarget: {cameraMovementSpeedTarget}" +
            $"\r\nMovementSpeed: {movementSpeed}" +
            $"\r\nMovementSpeedTarget: {movementSpeedTarget}" +
            $"\r\nMovementSpeedMax: {movementSpeedMax}" +
            $"\r\nForce: {force}" +
            $"\r\nFacing: {facing}" +
            $"\r\nJump: {jump}" +
            $"\r\nJumpInput: {jumpInput}" +
            $"\r\nMidAir: {midAir}" +
            $"\r\nJumpCooldown: {jumpCooldown}" +
            $"\r\nCloseToBlock: {(closeToBlockLeft ? "Left" : closeToBlockRight ? "Right" : "none")}" +
            $"\r\nOnBlock: {(onBlockLeft ? "Left" : onBlockRight ? "Right" : "none")}" +
            $"\r\nCloseToBlockDown: {closeToBlockDown} ({closeToBlockDownDist})" +
            $"\r\nOnBlockDown: {onBlockDown}" +
            $"\r\nPlayerBlockHeightDiff: {playerBlockHeightDiff}" +
            $"\r\nGrabInput: {grabInput}" +
            $"\r\nGrab: {grab}" +
            $"\r\nGrabAfterJumpCooldown: {grabAfterJumpCooldown}" +
            $"\r\nLastGrabbedOn: {lastGrabbedOn}" +
            $"\r\nDashInput: {dashInput}" +
            $"\r\nDashed: {dashed}" +
            $"\r\nGameScreen.Top: {gameScreen.Top}";

        jumpInput = false;
        dashInput = false;

        if (!onBlockDown)
            touchedGround = false;

        // Reset hry, pokud se dostane hr�� mimo hrac� plochu
        if (player.Bottom < gameScreen.Top || player.Left > gameScreen.Right || player.Top > gameScreen.Bottom + gameScreen.Height - 864 || player.Right < gameScreen.Left)
        {
            menuEscapeContinue(true);
        }
    }

    #region Kamera

    private void updateCamera()
    {
        cameraMovementSpeedTarget = (432 - playerCenterY) - gameScreen.Top;

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

        // Fix na hran� obrazovky proti viditeln�mu zaseknut�
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

    // Zam��� kameru nahoru
    private void CameraFocus(string focus)
    {
        switch (focus)
        {
            case "Player": gameScreen.Top = 432 - player.Top - player.Height / 2; break;
            case "Top": gameScreen.Top = 0; break;
            case "Bottom": gameScreen.Top = 864 - gameScreen.Height; break;
        }
    }

    #endregion Kamera

    #region Cooldowny

    //// Cooldown 30ms na v�skok po v�skoku
    private void timerJumpCooldown_Tick(object sender, EventArgs e)
    {
        jumpInput = false;
        jumpCooldown = false;
        timerJumpCooldown.Enabled = false;
    }

    //// Cooldown 300ms na skok pokud se bouchne hlavou o spodek bloku
    private void timerJumpHeadBumpCooldown_Tick(object sender, EventArgs e)
    {
        jumpCooldown = false;
        timerJumpHeadBumpCooldown.Enabled = false;
    }

    //// Cooldown 280ms na Grab po v�skoku z Grabu
    private void timerGrabCooldown_Tick(object sender, EventArgs e)
    {
        grabAfterJumpCooldown = false;
        timerGrabAfterJumpCooldown.Enabled = false;
    }

    //// Vypnut� gravitace na 100ms po Dashi (pokud nedashnul vertik�ln�)
    private void timerDashedNonVertical_Tick(object sender, EventArgs e)
    {
        dashedNonVertical = false;
        timerDashedNonVertical.Enabled = false;
    }

    #endregion Cooldowny

    #region Vstupy

    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (inputEnabled)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    upInput = true;
                    break;

                case Keys.S:
                    downInput = true;
                    break;

                case Keys.A:
                    leftInput = true;
                    break;

                case Keys.D:
                    rightInput = true;
                    break;

                case Keys.Space:
                    if (spaceReleased)
                    {
                        jumpInput = true;
                        spaceReleased = false;
                    }
                    break;

                case Keys.ControlKey:
                    if (ctrlReleased)
                    {
                        dashInput = true;
                        ctrlReleased = false;
                    }
                    break;

                case Keys.ShiftKey:
                    grabInput = true;
                    break;

                case Keys.F3:
                    lbDeveloperStats.Visible = !lbDeveloperStats.Visible;
                    break;
            }
        }

        switch (e.KeyCode)
        {
            case Keys.Escape:
                if (!menuMainContainer.Enabled)
                {
                    if (menuEscapeContainer.Enabled)   // Pokud je na obrazovce pauza
                    {
                        menuEscapeBtContinue.PerformClick();
                    }
                    else if (menuControlsContainer.Enabled)   // Pokud je na obrazovce ovl�d�n�
                    {
                        menuControlsBtEscapeMenu.PerformClick();
                    }
                    else   // Pokud je ve h�e
                    {
                        menuEscapeContainer.Enabled = true; menuEscapeContainer.Visible = true;
                        gameScreen.Enabled = false; gameScreen.Visible = false;
                        timer1.Enabled = false;
                        inputEnabled = false;
                    }
                }
                break;

            case Keys.NumPad0:
                developerKeys = true;
                break;
        }

        // Funkce pro testov�n�
        if (developerKeys)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:   // Developer stats
                    lbDeveloperStats.Visible = !lbDeveloperStats.Visible;
                    break;

                case Keys.NumPad1:
                    spawnLevel(1);
                    break;

                case Keys.NumPad2:
                    spawnLevel(2);
                    break;

                case Keys.NumPad3:
                    spawnLevel(3);
                    break;
            }
        }
    }

    private void MainWindow_KeyUp(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.W:
                upInput = false;
                break;

            case Keys.S:
                downInput = false;
                break;

            case Keys.A:
                leftInput = false;
                break;

            case Keys.D:
                rightInput = false;
                break;

            case Keys.Space:
                spaceReleased = true;
                break;

            case Keys.ControlKey:
                ctrlReleased = true;
                break;

            case Keys.ShiftKey:
                grabInput = false;
                PlaySound("grabOff");
                break;

            case Keys.NumPad0:
                developerKeys = false;
                break;
        }
    }

    private void buttonClicked(object sender, EventArgs e)
    {
        Button clickedButton = sender as Button;

        switch (clickedButton.Name)
        {
            case "mainBtPlay":    // Zapnut� hry ze Start menu
                movementSpeed = 0; force = 0;
                spawnLevel(1);

                menuMainContainer.Enabled = false;
                menuMainContainer.Visible = false;
                gameScreen.Enabled = true;
                gameScreen.Visible = true;
                timer1.Enabled = true;
                inputEnabled = true;
                break;

            case "mainBtClose":    // Vypnut� hry ze Start menu
                Close();
                break;

            case "menuEscapeBtContinue":    // Pokra�ov�n� ve h�e z Escape menu
                menuEscapeContinue(false);
                break;

            case "menuEscapeBtScreenReset":    // Reset obrazovky z Escape menu
                menuEscapeContinue(true);
                break;

            case "menuEscapeBtControls":    // Zobrazen� ovl�d�n� v Escape menu
                menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;
                menuControlsContainer.Enabled = true; menuControlsContainer.Visible = true;
                break;

            case "menuControlsBtEscapeMenu":    // Odchod do Escape menu ze zobrazen� ovl�d�n�
                menuControlsContainer.Enabled = false; menuControlsContainer.Visible = false;
                menuEscapeContainer.Enabled = true; menuEscapeContainer.Visible = true;
                break;

            case "menuEscapeBtMainMenu":    // Odchod do Start menu z Escape menu
                menuControlsContainer.Enabled = false; menuControlsContainer.Visible = false;
                menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;

                timer1.Enabled = false;
                inputEnabled = false;

                gameScreen.Enabled = false; gameScreen.Visible = false;
                menuMainContainer.Enabled = true;
                menuMainContainer.Visible = true;
                break;
        }

        Focus();
    }

    private void menuEscapeContinue(bool restart)
    {
        if (restart)
        {
            movementSpeed = 0; force = 0;
            spawnLevel(currentLevel);
        }

        menuControlsContainer.Enabled = false; menuControlsContainer.Visible = false;
        menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;
        gameScreen.Enabled = true; gameScreen.Visible = true;
        timer1.Enabled = true;
        inputEnabled = true;
    }

    #endregion Vstupy

    #region Level design

    private void Level1()
    {
        gameScreen.Height = 864;

        Terrain pictureBox1 = new(0, 768, 1339, 96, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox2 = new(337, 717, 77, 51, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox3 = new(645, 684, 222, 84, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox4 = new(942, 609, 190, 84, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox5 = new(281, 314, 66, 113, "collision", Color.FromArgb(115, 149, 218), false, Resources.blank, gameScreen);
        Terrain pictureBox6 = new(355, 697, 51, 20, "collision spring", Color.FromArgb(154, 205, 50), false, Resources.blank, gameScreen);
        Terrain pictureBox7 = new(67, 549, 66, 113, "collision", Color.FromArgb(115, 149, 218), false, Resources.blank, gameScreen);
        Terrain pictureBox8 = new(77, 133, 66, 113, "collision", Color.FromArgb(115, 149, 218), false, Resources.blank, gameScreen);
        Terrain pictureBox9 = new(507, 423, 222, 62, "collision", Color.FromArgb(115, 149, 218), false, Resources.blank, gameScreen);
        Terrain pictureBox10 = new(1256, 549, 259, 56, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox11 = new(942, 279, 222, 84, "collision", Color.FromArgb(115, 149, 218), false, Resources.blank, gameScreen);
        Terrain pictureBox12 = new(470, 111, 397, 55, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);

        terrainArray = new Terrain[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, };

        player.Left = 186;
        player.Top = 701;

        CameraFocus("Bottom");
    }

    private void Level2()
    {
        gameScreen.Height = 1627;

        Terrain pictureBox31 = new(1352, 448, 47, 28, "collision spring", Color.FromArgb(154, 205, 50), false, Resources.blank, gameScreen);
        Terrain pictureBox30 = new(1349, 684, 50, 67, "collision", Color.FromArgb(128, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox29 = new(1225, 883, 297, 28, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox28 = new(16, 1, 1209, 17, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox27 = new(1522, -1576, 16, 1627, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox23 = new(-1, -1576, 16, 1627, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox22 = new(913, 506, 114, 88, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox26 = new(1341, 476, 69, 63, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox25 = new(1181, 18, 44, 127, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox24 = new(1181, 394, 44, 517, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox21 = new(378, 748, 114, 88, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox20 = new(16, 394, 230, 33, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox19 = new(182, 428, 64, 53, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox18 = new(182, 598, 64, 62, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox17 = new(246, 911, 1276, 35, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox16 = new(182, 659, 64, 445, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox15 = new(16, 1264, 121, 28, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox14 = new(262, 1264, 121, 28, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox13 = new(488, 1264, 121, 28, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox12 = new(609, 1346, 50, 75, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox11 = new(1218, 1448, 50, 67, "collision", Color.FromArgb(128, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox10 = new(1071, 30, 338, 20, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox9 = new(1409, 1519, 50, 108, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox8 = new(1098, 1323, 294, 43, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox7 = new(1021, 1519, 50, 108, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox6 = new(838, 1344, 50, 283, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox5 = new(838, 1134, 50, 210, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox4 = new(659, 1323, 179, 22, "collision jump-through", Color.FromArgb(65, 50, 31), false, Resources.blank, gameScreen);
        Terrain pictureBox3 = new(609, 1134, 50, 210, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox1 = new(16, 1293, 593, 52, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox2 = new(16, 1519, 476, 52, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);

        terrainArray = new Terrain[] { pictureBox31, pictureBox30, pictureBox29, pictureBox28, pictureBox27, pictureBox23, pictureBox22, pictureBox26, pictureBox25, pictureBox24, pictureBox21, pictureBox20, pictureBox19, pictureBox18, pictureBox17, pictureBox16, pictureBox15, pictureBox14, pictureBox13, pictureBox12, pictureBox11, pictureBox10, pictureBox9, pictureBox8, pictureBox7, pictureBox6, pictureBox5, pictureBox4, pictureBox3, pictureBox1, pictureBox2, };

        player.Left = 86;
        player.Top = 1453;

        CameraFocus("Bottom");
    }

    private void Level3()
    {
        gameScreen.Height = 864;

        Terrain pictureBox1 = new(0, 768, 1339, 96, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);

        terrainArray = new Terrain[] { pictureBox1 };

        player.Left = 186;
        player.Top = 701;

        CameraFocus("Bottom");
    }

    private void spawnLevel(int level)
    {
        foreach (Terrain terrain in terrainArray)
        {
            DestroyAll(terrain.pb, gameScreen);
        }

        switch (level)
        {
            case 1: Level1(); break;
            case 2: Level2(); break;
            case 3: Level3(); break;
        }

        currentLevel = level;
    }

    private void DestroyAll(PictureBox pb, Panel panel)
    {
        pb.Bounds = Rectangle.Empty;
        panel.Controls.Remove(pb);
        pb.Dispose();
    }

    #endregion Level design

    #region Sound design

    string lastMaterial = "ice";

    SoundManager soundJumped = new("jump", false);
    SoundManager soundLandedIce = new("land_ice", true);
    public int landVariant = 1;
    SoundManager soundSpring = new("spring", false);
    SoundManager soundGrabOnIce = new("grab_ice", true);
    public int grabOnVariant = 1;
    SoundManager soundGrabOff = new("grab_letgo", false);
    SoundManager soundDash = new("dash", false);

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "jumped":
                soundJumped.PlaySound(0);
                break;

            case "landed":
                switch (lastMaterial)
                {
                    case "ice":
                        soundLandedIce.PlaySound(landVariant);

                        if (landVariant >= 5)
                            landVariant = 1;
                        else landVariant++;
                        break;
                }
                break;

            case "spring":
                soundSpring.PlaySound(0);
                break;

            case "grabOn":
                if (!grabbedOn)
                {
                    switch (lastMaterial)
                    {
                        case "ice":
                            soundGrabOnIce.PlaySound(grabOnVariant);

                            if (grabOnVariant >= 5)
                                grabOnVariant = 1;
                            else grabOnVariant++;
                            break;
                    }
                }
                grabbedOn = true;
                break;

            case "grabOff":
                if (grabbedOn)
                {
                    soundGrabOff.PlaySound(0);
                }
                grabbedOn = false;
                break;

            case "dash":
                soundDash.PlaySound(0);
                break;
        }
    }

    #endregion Sound design

    #region Texts

    private void LoadTexts(string language)
    {
        int langIndex = 0;
        switch (language)
        {
            case "cs":
                langIndex = 0;
                break;

            case "en":
                langIndex = 1;
                break;
        }

        // Hlavn� menu
        mainBtPlay.Text = texts[2].Split('\t')[langIndex];
        mainBtSettings.Text = texts[3].Split('\t')[langIndex];
        mainBtClose.Text = texts[4].Split('\t')[langIndex];
        mainLbInfo.Text = texts[11].Split('\t')[langIndex];

        // Ovl�d�n�
        menuControlsLbTitle.Text = texts[13].Split('\t')[langIndex];
        menuControlsLbL1.Text = texts[14].Split('\t')[langIndex];
        menuControlsLbL2.Text = texts[15].Split('\t')[langIndex];
        menuControlsLbL3.Text = texts[16].Split('\t')[langIndex];
        menuControlsLbL4.Text = texts[17].Split('\t')[langIndex];
        menuControlsLbL5.Text = texts[18].Split('\t')[langIndex];
        menuControlsLbR1.Text = texts[19].Split('\t')[langIndex];
        menuControlsLbR2.Text = texts[20].Split('\t')[langIndex];
        menuControlsLbR3.Text = texts[21].Split('\t')[langIndex];
        menuControlsLbR4.Text = texts[22].Split('\t')[langIndex];
        menuControlsLbR5.Text = texts[23].Split('\t')[langIndex];
        menuControlsBtEscapeMenu.Text = texts[5].Split('\t')[langIndex];

        // Pauza
        menuEscapeLbTitle.Text = texts[25].Split('\t')[langIndex];
        menuEscapeBtContinue.Text = texts[6].Split('\t')[langIndex];
        menuEscapeBtScreenReset.Text = texts[7].Split('\t')[langIndex];
        menuEscapeBtControls.Text = texts[8].Split('\t')[langIndex];
        menuEscapeBtMainMenu.Text = texts[9].Split('\t')[langIndex];
    }

    #endregion Texts
}