using Celeste_WinForms.Properties;
using System.ComponentModel;
using System.Diagnostics;
using XInputDotNetPure;
using ButtonState = XInputDotNetPure.ButtonState;
using SharpDX.DirectInput;

namespace Celeste_WinForms;

public partial class MainWindow : Form
{
    // Globální promìnné
    /// Texty programu
    List<string> texts = Resources.texts.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

    /// Hráè
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

    bool dashInput, ctrlReleased = true;
    bool dashed;
    bool dashedNonVertical;

    int playerLeftOffset, playerRightOffset;

    /// Interface
    string playingOn;
    Point cursor;
    Point previousCursor;
    string settingsOpenedFrom = "mainmenu";
    string controlsOpenedFrom = "mainmenu";
    int langIndex = 0;

    /// Level
    int currentLevel = 1;

    Terrain[] terrainArray;

    /// Zvuky
    public int soundOutputType = 0;
    float volume = 0.7f;
    bool grabbedOn = false;
    bool touchedGround = true;

    /// Kamera
    int cameraMovementSpeed, cameraMovementSpeedTarget;
    int playerCenterY;

    // Herní ovladaèe
    /// Sjednocené
    bool cA, cB, cX, cY;
    bool cLeft, cRight, cUp, cDown;
    bool cTrigger;
    bool cOptions;

    /// Xbox
    private GamePadState previousXboxState;
    GamePadState xboxState;
    bool xboxA, xboxB, xboxX, xboxY;
    bool xboxLeft, xboxRight, xboxUp, xboxDown;
    bool xboxTrigger;
    bool xboxOptions;

    /// PlayStation
    Joystick joystick;
    bool psControllerConnected;
    bool[]? previousButtons;
    bool previousButtonsCleared = true;
    bool psA, psB, psX, psY;
    bool psLeft, psRight, psUp, psDown;
    bool psTrigger;
    bool psOptions;

    bool developerKeys;   // NumPad0 stisknuta

    /// Tagy <summary>
    /// "collision" - na objekt se vztahují kolize
    /// "spring" - pružina
    /// jump-through - proskoèitelná platforma
    /// </summary>

    public MainWindow()
    {
        InitializeComponent();

        LoadTexts(0);
        Level1();

        FindControllers();

        menuMainContainer.Enabled = true; menuMainContainer.Visible = true;
        PlayingOnInfo("keyboard");
    }

    #region Herní ovladaèe

    private void timerControllers_Tick(object sender, EventArgs e)
    {
        // Reset inputù
        xboxA = false; xboxB = false; xboxX = false; xboxY = false;
        xboxLeft = false; xboxRight = false; xboxUp = false; xboxDown = false;
        xboxTrigger = false; xboxOptions = false;
        psA = false; psB = false; psX = false; psY = false;
        psLeft = false; psRight = false; psUp = false; psDown = false;
        psTrigger = false; psOptions = false;

        // Xbox
        GamePadState xboxState = GamePad.GetState(PlayerIndex.One);
        if (xboxState.IsConnected)
        {
            if (inputEnabled)
            {
                xboxA = xboxState.Buttons.A == ButtonState.Pressed;
                xboxB = xboxState.Buttons.B == ButtonState.Pressed;
                xboxX = xboxState.Buttons.X == ButtonState.Pressed;
                xboxY = xboxState.Buttons.Y == ButtonState.Pressed;
                xboxLeft =
                    xboxState.ThumbSticks.Left.X < -0.4 ||
                    xboxState.DPad.Left == ButtonState.Pressed;
                xboxRight =
                    xboxState.ThumbSticks.Left.X > 0.4 ||
                    xboxState.DPad.Right == ButtonState.Pressed;
                xboxUp =
                    xboxState.ThumbSticks.Left.Y > 0.4 ||
                    xboxState.DPad.Up == ButtonState.Pressed;
                xboxDown =
                    xboxState.ThumbSticks.Left.Y < -0.4 ||
                    xboxState.DPad.Down == ButtonState.Pressed;
                xboxTrigger =
                    xboxState.Triggers.Left > 0 ||
                    xboxState.Triggers.Right > 0 ||
                    xboxState.Buttons.LeftShoulder == ButtonState.Pressed ||
                    xboxState.Buttons.RightShoulder == ButtonState.Pressed;
            }

            xboxOptions = xboxState.Buttons.Start == ButtonState.Pressed &&
                previousXboxState.Buttons.Start == ButtonState.Released;

            #region Hraje na Xbox, pokud se dotknul

            if (xboxState.Buttons.A != previousXboxState.Buttons.A ||
                xboxState.Buttons.B != previousXboxState.Buttons.B ||
                xboxState.Buttons.X != previousXboxState.Buttons.X ||
                xboxState.Buttons.Y != previousXboxState.Buttons.Y ||
                xboxState.Buttons.Start != previousXboxState.Buttons.Start ||
                xboxState.Buttons.Back != previousXboxState.Buttons.Back ||
                xboxState.Buttons.Guide != previousXboxState.Buttons.Guide ||
                xboxState.Buttons.RightStick != previousXboxState.Buttons.RightStick ||
                xboxState.Buttons.LeftShoulder != previousXboxState.Buttons.LeftShoulder ||
                xboxState.Buttons.RightShoulder != previousXboxState.Buttons.RightShoulder ||
                xboxState.DPad.Left != previousXboxState.DPad.Left ||
                xboxState.DPad.Right != previousXboxState.DPad.Right ||
                xboxState.DPad.Up != previousXboxState.DPad.Up ||
                xboxState.DPad.Down != previousXboxState.DPad.Down ||
                xboxState.ThumbSticks.Left.X >= 0.1 ||
                xboxState.ThumbSticks.Left.X <= -0.1 ||
                xboxState.ThumbSticks.Left.Y >= 0.1 ||
                xboxState.ThumbSticks.Left.Y <= -0.1 ||
                xboxState.ThumbSticks.Right.X >= 0.1 ||
                xboxState.ThumbSticks.Right.X <= -0.1 ||
                xboxState.ThumbSticks.Right.Y >= 0.1 ||
                xboxState.ThumbSticks.Right.Y <= -0.1 ||
                xboxState.Triggers.Left != 0 ||
                xboxState.Triggers.Right != 0
                )
            {
                PlayingOnInfo("xbox");
            }

            #endregion

            previousXboxState = xboxState;
        }
        else
        {
            previousXboxState = GamePad.GetState(PlayerIndex.One);
        }

        // PlayStation
        try
        {
            if (psControllerConnected)
            {
                joystick.Poll();

                var state = joystick.GetCurrentState();
                var buttons = state.Buttons;

                if (previousButtonsCleared)
                {
                    previousButtons = buttons;
                    previousButtonsCleared = false;
                }

                if (inputEnabled)
                {
                    psA = buttons[1];
                    psB = buttons[2];
                    psX = buttons[0];
                    psY = buttons[3];
                    psLeft =
                        (state.X / 32768.0f) - 1 < -0.4 ||
                        state.PointOfViewControllers[0] == 22500 || // Down-Left
                        state.PointOfViewControllers[0] == 27000 || // Left
                        state.PointOfViewControllers[0] == 31500;   // Up-Left
                    psRight =
                        (state.X / 32768.0f) - 1 > 0.4 ||
                        state.PointOfViewControllers[0] == 4500 ||  // Up-Right
                        state.PointOfViewControllers[0] == 9000 ||  // Right
                        state.PointOfViewControllers[0] == 13500;   // Down-Right
                    psUp =
                        (state.Y / 32768.0f) - 1 < -0.4 ||
                        state.PointOfViewControllers[0] == 0 ||     // Up
                        state.PointOfViewControllers[0] == 4500 ||  // Up-Right
                        state.PointOfViewControllers[0] == 31500;   // Up-Left
                    psDown =
                        (state.Y / 32768.0f) - 1 > 0.4 ||
                        state.PointOfViewControllers[0] == 13500 || // Down-Right
                        state.PointOfViewControllers[0] == 18000 || // Down
                        state.PointOfViewControllers[0] == 22500;   // Down-Left
                    psTrigger =
                        buttons[4] ||
                        buttons[5];
                }

                psOptions = buttons[9] && !previousButtons[9];

                #region Hraje na PS, pokud se dotknul

                for (int i = 0; i <= 13; i++)
                {
                    if (buttons[i] != previousButtons[i])
                        PlayingOnInfo("playstation");
                }

                if ((state.X / 32768.0f) - 1 >= 0.1 ||
                    (state.X / 32768.0f) - 1 <= -0.1 ||
                    (state.Y / 32768.0f) - 1 >= 0.1 ||
                    (state.Y / 32768.0f) - 1 <= -0.1)
                {
                    PlayingOnInfo("playstation");
                }

                #endregion

                previousButtons = buttons;
            }
        }
        catch (Exception)
        {
            previousButtons = new bool[0];
            previousButtonsCleared = true;
            psControllerConnected = false;
            return;
        }

        // Sjednocení vstupù PS a Xbox
        cA = xboxA || psA;
        cB = xboxB || psB;
        cX = xboxX || psX;
        cY = xboxY || psY;
        cLeft = xboxLeft || psLeft;
        cRight = xboxRight || psRight;
        cUp = xboxUp || psUp;
        cDown = xboxDown || psDown;
        cTrigger = xboxTrigger || psTrigger;

        if (xboxOptions || psOptions)
        {
            Escape("controller");
        }

        // Kurzor
        previousCursor = cursor;
        cursor = PointToClient(Cursor.Position);

        if (cursor != previousCursor)
        {
            PlayingOnInfo("keyboard");
        }
    }

    #endregion Herní ovladaèe

    private void timer1_Tick(object sender, EventArgs e)
    {

        // Movement

        // Základní hodnoty
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

        updateCamera();

        // Pohyb
        /// Inputy
        if (leftInput || cLeft)
        {
            left = true;
            facing = "Left";
            lastStraightFacing = "Left";
        }
        else
            left = false;

        if (rightInput || cRight)
        {
            right = true;
            facing = "Right";
            lastStraightFacing = "Right";
        }
        else
            right = false;

        if (upInput || cUp)
            facing += "Up";
        else if (downInput || cDown)
            facing += "Down";

        if (facing == "")
            facing = lastStraightFacing;

        // Interakce s bloky
        foreach (PictureBox block in gameScreen.Controls.OfType<PictureBox>().Where(block => block.Tag != null))
        {
            if (!block.Tag.ToString().Contains("jump-through"))
            {
                // Postranní kolize
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

                // Pokud je hráè blízko k bloku, pøiblíží se pouze o rozdíl mezi hranou hráèe a bloku (proti bugùm)
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
                if (((onBlockLeft && (leftInput || cLeft)) || (onBlockRight && (rightInput || cRight))) && force < 0)
                {
                    slide = true;
                    midAir = false;
                }

                // Grab aktivace
                if ((grabInput || cTrigger) && (onBlockLeft || onBlockRight))
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

        // Hráè drží mezerník
        if ((jumpInput || (cA || cY)) && !midAir && !jumpCooldown && !grabAfterJumpCooldown)
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

        // Grab - Výskok na horní hranì bloku (<25px spodek hráèe - vršek bloku)
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
        if ((dashInput || (cB || cX)) && !dashed)
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

            if (!((leftInput || cLeft) || (rightInput || cRight) || (upInput || cUp) || (downInput || cDown)))
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
            // Pokud je hráè blízko k bloku, pøiblíží se pouze o rozdíl mezi hranou hráèe a bloku (proti bugùm)
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
                // Vrchní kolize
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

                // Pokud je hráè blízko k bloku, pøiblíží se pouze o rozdíl mezi hranou hráèe a bloku (proti bugùm)
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

                // Spodní kolize
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
                        movementSpeed += movementSpeed < movementSpeedTarget ? 1 : movementSpeedTarget - movementSpeed;
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
                        movementSpeed += movementSpeed > movementSpeedTarget ? -1 : movementSpeedTarget - movementSpeed;
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


        // Vývojáøské statistiky [F3]
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
            $"\r\nGameScreen.Top: {gameScreen.Top}" +
            $"\r\nPlayingOn: {playingOn}";

        jumpInput = false;
        dashInput = false;

        if (!onBlockDown)
            touchedGround = false;

        //// Reset hry, pokud se dostane hráè mimo hrací plochu
        //if (player.Bottom < gameScreen.Top || player.Left > gameScreen.Right || player.Top > gameScreen.Bottom + gameScreen.Height - 864 || player.Right < gameScreen.Left)
        //{
        //    menuEscapeContinue(true);
        //}
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

        // Fix na hranì obrazovky proti viditelnému zaseknutí
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

    // Zamìøí kameru nahoru
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

    //// Cooldown 30ms na výskok po výskoku
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

    //// Cooldown 280ms na Grab po výskoku z Grabu
    private void timerGrabCooldown_Tick(object sender, EventArgs e)
    {
        grabAfterJumpCooldown = false;
        timerGrabAfterJumpCooldown.Enabled = false;
    }

    //// Vypnutí gravitace na 100ms po Dashi (pokud nedashnul vertikálnì)
    private void timerDashedNonVertical_Tick(object sender, EventArgs e)
    {
        dashedNonVertical = false;
        timerDashedNonVertical.Enabled = false;
    }

    #endregion Cooldowny

    #region Vstupy

    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        PlayingOnInfo("keyboard");

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
                Escape("keyboard");
                break;

            case Keys.NumPad0:
                developerKeys = true;
                break;
        }

        // Funkce pro testování
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

    private void Escape(string escInputType)
    {
        if (!menuMainContainer.Enabled)
        {
            if (menuEscapeContainer.Enabled)   // Pokud je na obrazovce pauza
            {
                menuEscapeBtContinue.PerformClick();
            }
            else if (menuSettingsContainer.Enabled) // Pokud je v nastavení
            {
                menuSettingsBtBack.PerformClick();
                if (escInputType == "controller")
                    menuEscapeBtContinue.PerformClick();
            }
            else if (menuControlsContainer.Enabled)   // Pokud je na obrazovce ovládání
            {
                menuControlsBtBack.PerformClick();
                if (escInputType == "controller")
                    menuEscapeBtContinue.PerformClick();
            }
            else   // Pokud je ve høe
            {
                menuEscapeContainer.Enabled = true; menuEscapeContainer.Visible = true;
                gameScreen.Enabled = false; gameScreen.Visible = false;
                timer1.Enabled = false;
                inputEnabled = false;
            }
        }

        Focus();
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

    private void menuSettingsTrackR1_Scroll(object sender, EventArgs e)
    {
        volume = (float)(menuSettingsTrackR1.Value * 0.05);
        menuSettingsLbVolumeR1.Text = Math.Floor(volume * 100).ToString();
    }

    private void buttonClicked(object sender, EventArgs e)
    {
        Control clickedControl = sender as Control;

        switch (clickedControl.Name)
        {
            case "menuMainBtPlay":    // Zapnutí hry ze Start menu
                FindControllers();

                movementSpeed = 0; force = 0;
                spawnLevel(1);

                menuMainContainer.Enabled = false; menuMainContainer.Visible = false;
                gameScreen.Enabled = true; gameScreen.Visible = true;

                timer1.Enabled = true;
                inputEnabled = true;
                break;

            case "menuMainBtSettings":
                menuMainContainer.Enabled = false; menuMainContainer.Visible = false;
                menuSettingsContainer.Enabled = true; menuSettingsContainer.Visible = true;

                settingsOpenedFrom = "mainmenu";
                break;

            case "menuMainBtControls":
                menuMainContainer.Enabled = false; menuMainContainer.Visible = false;
                menuControlsContainer.Enabled = true; menuControlsContainer.Visible = true;

                controlsOpenedFrom = "mainmenu";
                break;

            case "menuMainBtClose":    // Vypnutí hry ze Start menu
                Close();
                break;

            case "menuEscapeBtContinue":    // Pokraèování ve høe z Escape menu
                menuEscapeContinue(false);
                break;

            case "menuEscapeBtScreenReset":    // Reset obrazovky z Escape menu
                menuEscapeContinue(true);
                break;

            case "menuEscapeBtSettings":
                menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;
                menuSettingsContainer.Enabled = true; menuSettingsContainer.Visible = true;

                settingsOpenedFrom = "pause";
                break;

            case "menuEscapeBtControls":    // Zobrazení ovládání v Escape menu
                menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;
                menuControlsContainer.Enabled = true; menuControlsContainer.Visible = true;

                controlsOpenedFrom = "pause";
                break;

            case "menuControlsBtBack":    // Odchod do Escape menu ze zobrazení ovládání
                menuControlsContainer.Enabled = false; menuControlsContainer.Visible = false;
                if (controlsOpenedFrom == "mainmenu")
                {
                    menuMainContainer.Enabled = true; menuMainContainer.Visible = true;
                }
                else if (controlsOpenedFrom == "pause")
                {
                    menuEscapeContainer.Enabled = true; menuEscapeContainer.Visible = true;
                }
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

            case "menuSettingsBtBack":  // Odchod z Nastavení
                menuSettingsContainer.Enabled = false; menuSettingsContainer.Visible = false;
                if (settingsOpenedFrom == "mainmenu")
                {
                    menuMainContainer.Visible = true; menuMainContainer.Enabled = true;
                }
                else if (settingsOpenedFrom == "pause")
                {
                    menuEscapeContainer.Visible = true; menuEscapeContainer.Enabled = true;
                }
                break;

            case "menuSettingsLbR2ControlL":   // Volba jazyka (zpìt)
                if (!(langIndex <= 0))
                    LoadTexts(-1);
                break;

            case "menuSettingsLbR2ControlR":   // Volba jazyka (další)
                if (!(langIndex >= languages.Count() - 1))
                    LoadTexts(1);
                break;

            case "menuSettingsLbR3ControlL":   // Volba vstupu (zpìt)
                if (!(soundOutputType <= 0))
                {
                    soundOutputType--;
                    LoadTexts(0);
                }
                break;

            case "menuSettingsLbR3ControlR":   // Volba vstupu (další)
                if (!(soundOutputType >= soundOutputTypeList.Count() - 1))
                {
                    soundOutputType++;
                    LoadTexts(0);
                }
                break;
        }

        Focus();
    }

    private void menuEscapeContinue(bool restart)
    {
        FindControllers();

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

    private void FindControllers()
    {
        var input = new DirectInput();
        var joystickGuid = Guid.Empty;

        foreach (var deviceInstance in input.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AllDevices))
        {
            if (!(deviceInstance.Type == DeviceType.Gamepad))   // Nepøidá Xbox
                joystickGuid = deviceInstance.InstanceGuid;
        }

        if (joystickGuid == Guid.Empty)
            psControllerConnected = false;
        else
        {
            joystick = new Joystick(input, joystickGuid);
            joystick.Properties.BufferSize = 128;
            joystick.Acquire();

            psControllerConnected = true;
        }
    }

    private void PlayingOnInfo(string device)
    {
        playingOn = device;

        switch (device)
        {
            case "keyboard":
                controlsContainer.RowStyles[0].Height = 100;
                controlsContainer.RowStyles[1].Height = 0;
                controlsContainer.RowStyles[2].Height = 0;
                break;

            case "xbox":
                controlsContainer.RowStyles[0].Height = 0;
                controlsContainer.RowStyles[1].Height = 100;
                controlsContainer.RowStyles[2].Height = 0;
                break;

            case "playstation":
                controlsContainer.RowStyles[0].Height = 0;
                controlsContainer.RowStyles[1].Height = 0;
                controlsContainer.RowStyles[2].Height = 100;
                break;
        }
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

    #region Texty

    List<string> languages = new List<string>()
    {
        "Èesky",
        "English"
    };

    List<string> soundOutputTypeList = new List<string>()
    {
        "Zvuky i hudba\tSounds and music",
        "Pouze zvuky\tOnly sounds",
        "Vypnuto\tOff"
    };

    private void LoadTexts(int shift)
    {
        langIndex += shift;
        menuSettingsLbR2Language.Text = languages[langIndex];

        foreach (Control text in menuSettingsLbR2Container.Controls)
            text.ForeColor = Color.FromArgb(68, 101, 147);

        if (langIndex <= 0)
            menuSettingsLbR2ControlL.ForeColor = Color.FromArgb(130, 160, 200);

        if (langIndex >= languages.Count() - 1)
            menuSettingsLbR2ControlR.ForeColor = Color.FromArgb(130, 160, 200);


        // Hlavní menu
        menuMainBtPlay.Text = texts[2].Split('\t')[langIndex];
        menuMainBtSettings.Text = texts[3].Split('\t')[langIndex];
        menuMainBtControls.Text = texts[8].Split('\t')[langIndex];
        menuMainBtClose.Text = texts[4].Split('\t')[langIndex];
        mainLbInfo.Text = texts[11].Split('\t')[langIndex];

        // Nastavení
        menuSettingsLbTitle.Text = texts[13].Split('\t')[langIndex];
        menuSettingsLbL1.Text = texts[14].Split('\t')[langIndex];
        menuSettingsLbL2.Text = texts[15].Split('\t')[langIndex];
        menuSettingsLbL3.Text = texts[16].Split('\t')[langIndex];

        menuSettingsLbR3Input.Text = soundOutputTypeList[soundOutputType].Split('\t')[langIndex];

        foreach (Control text in menuSettingsLbR3Container.Controls)
            text.ForeColor = Color.FromArgb(68, 101, 147);

        if (soundOutputType <= 0)
            menuSettingsLbR3ControlL.ForeColor = Color.FromArgb(130, 160, 200);

        if (soundOutputType >= soundOutputTypeList.Count() - 1)
            menuSettingsLbR3ControlR.ForeColor = Color.FromArgb(130, 160, 200);


        menuSettingsBtBack.Text = texts[5].Split('\t')[langIndex];

        // Ovládání
        menuControlsLbTitle.Text = texts[20].Split('\t')[langIndex];
        menuControlsLbL1.Text = texts[21].Split('\t')[langIndex];
        menuControlsLbL2.Text = texts[22].Split('\t')[langIndex];
        menuControlsLbL3.Text = texts[23].Split('\t')[langIndex];
        menuControlsLbL4.Text = texts[24].Split('\t')[langIndex];
        menuControlsLbL5.Text = texts[25].Split('\t')[langIndex];
        lbKeyboard1.Text = texts[26].Split('\t')[langIndex];
        lbKeyboard2.Text = texts[27].Split('\t')[langIndex];
        lbKeyboard3.Text = texts[28].Split('\t')[langIndex];
        lbKeyboard4.Text = texts[29].Split('\t')[langIndex];
        lbKeyboard5.Text = texts[30].Split('\t')[langIndex];
        lbKeyboard6.Text = texts[31].Split('\t')[langIndex];
        menuControlsBtBack.Text = texts[5].Split('\t')[langIndex];

        // Pauza
        menuEscapeLbTitle.Text = texts[33].Split('\t')[langIndex];
        menuEscapeBtContinue.Text = texts[6].Split('\t')[langIndex];
        menuEscapeBtScreenReset.Text = texts[7].Split('\t')[langIndex];
        menuEscapeBtSettings.Text = texts[3].Split('\t')[langIndex];
        menuEscapeBtControls.Text = texts[8].Split('\t')[langIndex];
        menuEscapeBtMainMenu.Text = texts[9].Split('\t')[langIndex];
    }

    #endregion Texty
}