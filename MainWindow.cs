using Celeste_WinForms.Properties;
using XInputDotNetPure;
using ButtonState = XInputDotNetPure.ButtonState;
using SharpDX.DirectInput;

namespace Celeste_WinForms;

public partial class MainWindow : Form
{
    // Global variables
    /// List of texts
    List<string> texts = Resources.texts.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

    /// Player
    bool inputEnabled = false;

    bool left, right;
    bool leftInput, rightInput;
    bool upInput, downInput;

    bool jump, jumpInput, spaceReleased = true;
    bool slide;
    bool grab, grabInput;
    bool grabFirstTimeSupport;  // Pomoc pøi trackování pøichycení

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

    /// Výtah
    bool rodeOnElevator;

    /// Interface
    string playingOn;
    Point cursor;
    Point previousCursor;
    string settingsOpenedFrom = "mainmenu";
    string controlsOpenedFrom = "mainmenu";
    int langIndex = 0;
    int textScaleIndex = 0;

    /// Level
    int currentLevel = 1;

    Terrain[] terrainArray;
    Strawberry[] strawberryArray;

    /// Sound
    public int soundOutputType = 0;
    float volume = 0.7f;
    bool grabbedOn = false;
    bool touchedGround = true;

    /// Camera
    int cameraMovementSpeed, cameraMovementSpeedTarget;
    int playerCenterY;

    // Gaming controllers
    /// Unified inputs
    bool cA, cB, cX, cY;
    bool cLeft, cRight, cUp, cDown;
    bool cTrigger;
    bool cOptions;

    /// Xbox controller
    private GamePadState previousXboxState;
    GamePadState xboxState;
    bool xboxA, xboxB, xboxX, xboxY;
    bool xboxLeft, xboxRight, xboxUp, xboxDown;
    bool xboxTrigger;
    bool xboxOptions;

    /// PlayStation controller
    Joystick joystick;
    bool psControllerConnected;
    bool[]? previousButtons;
    bool previousButtonsCleared = true;
    bool psA, psB, psX, psY;
    bool psLeft, psRight, psUp, psDown;
    bool psTrigger;
    bool psOptions;

    bool developerKeys;   // NumPad0 pressed

    // Protect timers from pause (if paused, timers will reset on continue)
    bool tTimerJumpCooldown;
    bool tTimerJumpHeadBumpCooldown;
    bool tTimerGrabAfterJumpCooldown;

    public MainWindow()
    {
        InitializeComponent();

        ConfigFileUpdate(0, "load");
        SettingsUpdate();
        Level1();

        FindControllers();

        menuMainContainer.Enabled = true; menuMainContainer.Visible = true;
        PlayingOnInfo("keyboard");
    }

    #region Gaming controllers

    private void timerControllers_Tick(object sender, EventArgs e)
    {
        // Input reset
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

            #region Input is "xbox" if controller is touched

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

                #region Input is "xbox" if controller is touched

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

        // Unifying Xbox and PlayStation inputs
        cA = xboxA || psA;
        cB = xboxB || psB;
        cX = xboxX || psX;
        cY = xboxY || psY;
        cLeft = xboxLeft || psLeft;
        cRight = xboxRight || psRight;
        cUp = xboxUp || psUp;
        cDown = xboxDown || psDown;
        cTrigger = xboxTrigger || psTrigger;
        cOptions = xboxOptions || psOptions;

        if (cOptions)
        {
            Escape("controller");
        }

        // Mouse cursor
        previousCursor = cursor;
        cursor = PointToClient(Cursor.Position);

        if (cursor != previousCursor)
        {
            // Input is "keyboard" if mouse is moved
            PlayingOnInfo("keyboard");
        }
    }

    #endregion Herní ovladaèe

    private void timer1_Tick(object sender, EventArgs e)
    {
        // Default values
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


        #region Movement

        foreach (Terrain item in terrainArray)
        {
            item.onBlockLeftExclusive = false; item.onBlockRightExclusive = false;
            item.onBlockDown = false;
        }

        updateCamera();

        // Movement
        /// Inputs
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

        // Interacton with blocks
        foreach (Terrain block in terrainArray.Where(block => block.pb.Tag != null))
        {
            if (!block.pb.Tag.ToString().Contains("jump-through"))
            {
                // Side collisions
                if (block.pb.Tag.ToString().Contains("collision") && player.Bounds.IntersectsWith(block.pb.Bounds))
                {
                    if (playerLeftOffset < block.pb.Right && player.Right > block.pb.Left + player.Width / 2 &&
                        player.Bottom > block.pb.Top + 1 && player.Top < block.pb.Bottom)
                    {
                        left = false;
                        movementSpeed = 0;
                    }

                    if (playerRightOffset > block.pb.Left && player.Left < block.pb.Right - player.Width / 2 &&
                        player.Bottom > block.pb.Top + 1 && player.Top < block.pb.Bottom)
                    {
                        right = false;
                        movementSpeed = 0;
                    }
                }

                // If the player is close to the block, it will only get closer by the difference between the edge of the player and the block (against bugs)
                if (block.pb.Tag.ToString().Contains("collision"))
                {
                    if (playerLeftOffset - block.pb.Right < (Math.Abs(movementSpeed) < movementSpeedMax ? movementSpeedMax : Math.Abs(movementSpeed)) && playerLeftOffset - block.pb.Right >= 0 &&
                        player.Bottom > block.pb.Top + 1 && player.Top < block.pb.Bottom)
                    {
                        closeToBlockLeft = true;
                        closeToBlockLeftDist = playerLeftOffset - block.pb.Right;

                        if (playerLeftOffset - block.pb.Right == 0 &&
                            player.Top + player.Height / 2 < block.pb.Bottom)
                        {
                            onBlockLeft = true;
                            block.onBlockLeftExclusive = true;
                            midAir = false;
                            playerBlockHeightDiff = player.Bottom - block.pb.Top;
                        }
                    }

                    if (block.pb.Left - playerRightOffset < (movementSpeed < movementSpeedMax ? movementSpeedMax : movementSpeed) && block.pb.Left - playerRightOffset >= 0 &&
                        player.Bottom > block.pb.Top + 1 && player.Top < block.pb.Bottom)
                    {
                        closeToBlockRight = true;
                        closeToBlockRightDist = block.pb.Left - playerRightOffset;

                        if (block.pb.Left - playerRightOffset == 0 &&
                            player.Top + player.Height / 2 < block.pb.Bottom)
                        {
                            onBlockRight = true;
                            block.onBlockRightExclusive = true;
                            midAir = false;
                            playerBlockHeightDiff = player.Bottom - block.pb.Top;
                        }
                    }
                }

                // Slide activation
                if (((onBlockLeft && (leftInput || cLeft)) || (onBlockRight && (rightInput || cRight))) && force < 0)
                {
                    slide = true;
                    midAir = false;
                }

                // Grab activation
                if ((grabInput || cTrigger) && (onBlockLeft || onBlockRight))
                {
                    grab = true;
                    midAir = false;

                    // If grabbed, move 1 pixel up (against bugs)
                    if (!grabFirstTimeSupport)
                    {
                        player.Top -= 1;
                        grabFirstTimeSupport = true;
                    }

                    lastGrabbedOn = onBlockLeft ? "Left" : onBlockRight ? "Right" : "";
                }
            }

            // midAir
            if (block.pb.Top - player.Bottom == -1 &&
                playerLeftOffset < block.pb.Right && playerRightOffset > block.pb.Left)
            {
                midAir = false;
            }
        }

        // User holds Space/Jump
        if ((jumpInput || (cA || cY)) && !midAir && !jumpCooldown && !grabAfterJumpCooldown)
        {
            jump = true;
            force = 15;
            PlaySound("jumped");

            if (((onBlockLeft || onBlockRight) && !onBlockDown) ||  // Touching block mid-air
                slide)  // Sliding on block
            {
                movementSpeed = onBlockLeft ? movementSpeedMax * 2 : onBlockRight ? -movementSpeedMax * 2 : 0;

                force = 11;
            }

            // Grabbed on block
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

        // Grab - Jump on top of block (<25px: bottom of player - top of block)
        if (grab && !grabAfterJumpCooldown)
        {
            if (playerBlockHeightDiff < 25)
            {
                force = Math.Abs(playerBlockHeightDiff / 2);
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

        // Dash ability
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

        // Grab ability
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
        else  // Gravity
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
            // If the player is close to the block, it only gets closer by the difference between the edge of the player and the block (against bugs)
            // From block top
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

            // Interaction with blocks
            if (block.Tag.ToString().Contains("collision") && player.Bounds.IntersectsWith(block.Bounds) &&
                playerLeftOffset < block.Right &&
                playerRightOffset > block.Left)
            {
                // Bottom collision (top of block)
                if (player.Bottom == block.Top + 1 && player.Top < block.Top)
                {
                    player.Top = block.Top - player.Height + 1;
                    force = 0;
                    jump = false;

                    onBlockDown = true;
                    grabFirstTimeSupport = false;

                    foreach (Terrain item in terrainArray)
                    {
                        item.onBlockDown = true;
                    }

                    closeToBlockDown = false;
                    dashed = false;

                    if (!block.Tag.ToString().Contains("spring"))
                    {
                        if (!touchedGround)
                            PlaySound("landed");
                        touchedGround = true;
                    }
                }

                // If the player is close to the block, it only gets closer by the difference between the edge of the player and the block (against bugs)
                // From block bottom
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

                // Top collision (bottom of block)
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

        // Sideways movement
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

        #endregion Movement

        #region Elevators

        foreach (Terrain elevator in terrainArray.Where(elevator => elevator.pb.Tag.ToString().Contains("elevator")))
        {
            if ((player.Bounds.IntersectsWith(elevator.pb.Bounds) || (elevator.onBlockLeftExclusive || elevator.onBlockRightExclusive)) && !elevator.moving)
                elevator.moving = true;

            elevator.pb.BackColor = Color.Black;

            if (elevator.moving)
            {
                elevator.pb.BackColor = Color.White;

                elevator.ElevatorAnimation(player, grabbedOn, playerLeftOffset, playerRightOffset, movementSpeed);

                if (player.Bounds.IntersectsWith(elevator.pb.Bounds) || (elevator.onBlockLeftExclusive || elevator.onBlockRightExclusive))
                    rodeOnElevator = true;

                if (!((player.Bounds.IntersectsWith(elevator.pb.Bounds) || (elevator.onBlockLeftExclusive || elevator.onBlockRightExclusive))) && elevator.moving && rodeOnElevator)  // Pokud seskoèí za jízdy
                {
                    movementSpeed += (int)(elevator.elevatorMovementSpeed * elevator.multiplierX);
                    force += (int)(elevator.elevatorMovementSpeed * elevator.multiplierY);
                    rodeOnElevator = false;
                }

                if (elevator.resetForce)
                    force = 0;

                if (elevator.playerKill)
                {
                    elevator.playerKill = false;
                    SpawnLevel(currentLevel);
                }
            }
        }

        #endregion

        #region Strawberries

        foreach (Strawberry strawberry in strawberryArray)
        {
            if (player.Bounds.IntersectsWith(strawberry.pb.Bounds))
                strawberry.tracking = true;

            if (strawberry.tracking)
            {
                strawberry.TrackTarget(player);

                // If player is on ground, start CollectingTime timer
                if (onBlockDown && !strawberry.collectingTime.Enabled)
                {
                    strawberry.collectingTime.Enabled = true;

                }
                else if (!onBlockDown && strawberry.collectingTime.Enabled)
                {
                    strawberry.collectingTime.Enabled = false;
                }
            }
        }

        #endregion

        // Developer stats [F3]
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

        // Reset the game if the player gets off the screen
        if (player.Bottom < gameScreen.Top || player.Left > gameScreen.Right || player.Top > gameScreen.Bottom + gameScreen.Height - 864 || player.Right < gameScreen.Left)
        {
            menuEscapeContinue(true);
        }
    }

    #region Camera

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

        // Fix on the edge of the screen against spawn delays
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

    // Focuses the camera on the selected location
    private void CameraFocus(string focus)
    {
        switch (focus)
        {
            case "Player": gameScreen.Top = 432 - player.Top - player.Height / 2; break;
            case "Top": gameScreen.Top = 0; break;
            case "Bottom": gameScreen.Top = 864 - gameScreen.Height; break;
        }
    }

    #endregion Camera

    #region Cooldowns

    //// Jump cooldown after jumping - 30ms
    private void timerJumpCooldown_Tick(object sender, EventArgs e)
    {
        jumpInput = false;
        jumpCooldown = false;
        timerJumpCooldown.Enabled = false;
    }

    //// Jump cooldown if player hits his head on the bottom of the block - 300ms
    private void timerJumpHeadBumpCooldown_Tick(object sender, EventArgs e)
    {
        jumpCooldown = false;
        timerJumpHeadBumpCooldown.Enabled = false;
    }

    //// Grab cooldown after jumping from Grab - 280ms
    private void timerGrabCooldown_Tick(object sender, EventArgs e)
    {
        grabAfterJumpCooldown = false;
        timerGrabAfterJumpCooldown.Enabled = false;
    }

    //// Turning off gravity after Dash (if didn't dashed vertical) - 100ms
    private void timerDashedNonVertical_Tick(object sender, EventArgs e)
    {
        dashedNonVertical = false;
        timerDashedNonVertical.Enabled = false;
    }

    #endregion Cooldowns

    #region Inputs

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

        // Testing functions
        if (developerKeys)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:   // Developer stats
                    lbDeveloperStats.Visible = !lbDeveloperStats.Visible;
                    break;

                case Keys.NumPad1:
                    SpawnLevel(1);
                    break;

                case Keys.NumPad2:
                    SpawnLevel(2);
                    break;

                case Keys.NumPad3:
                    SpawnLevel(3);
                    break;
            }
        }
    }

    private void Escape(string escInputType)
    {
        if (!menuMainContainer.Enabled)
        {
            if (menuEscapeContainer.Enabled)   // If on pause screen
            {
                menuEscapeBtContinue.PerformClick();
            }
            else if (menuSettingsContainer.Enabled) // If in settings
            {
                menuSettingsBtBack.PerformClick();
                if (escInputType == "controller")
                    menuEscapeBtContinue.PerformClick();
            }
            else if (menuControlsContainer.Enabled)   // If in controlls
            {
                menuControlsBtBack.PerformClick();
                if (escInputType == "controller")
                    menuEscapeBtContinue.PerformClick();
            }
            else   // If in game
            {
                menuEscapeContainer.Enabled = true; menuEscapeContainer.Visible = true;
                gameScreen.Enabled = false; gameScreen.Visible = false;
                timer1.Enabled = false;
                inputEnabled = false;

                TimerHandler("Pause");
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

        ConfigFileUpdate(1, menuSettingsLbVolumeR1.Text);
    }

    private void buttonClicked(object sender, EventArgs e)
    {
        Control clickedControl = sender as Control;

        switch (clickedControl.Name)
        {
            case "menuMainBtPlay":    // Starting the game from Start menu
                FindControllers();

                movementSpeed = 0; force = 0;
                SpawnLevel(1);

                menuMainContainer.Enabled = false; menuMainContainer.Visible = false;
                gameScreen.Enabled = true; gameScreen.Visible = true;

                timer1.Enabled = true;
                inputEnabled = true;

                TimerHandler("Play");
                break;

            case "menuMainBtSettings":  // Settings from Start menu
                menuMainContainer.Enabled = false; menuMainContainer.Visible = false;
                menuSettingsContainer.Enabled = true; menuSettingsContainer.Visible = true;

                settingsOpenedFrom = "mainmenu";
                break;

            case "menuMainBtControls":  // Controlls from Start menu
                menuMainContainer.Enabled = false; menuMainContainer.Visible = false;
                menuControlsContainer.Enabled = true; menuControlsContainer.Visible = true;

                controlsOpenedFrom = "mainmenu";
                break;

            case "menuMainBtClose":    // Closing the game from Start menu
                Close();
                break;

            case "menuEscapeBtContinue":    // Continue from Escape menu
                menuEscapeContinue(false);

                TimerHandler("Play");
                break;

            case "menuEscapeBtScreenReset":    // Screen reset from Escape menu
                menuEscapeContinue(true);

                TimerHandler("Reset");
                break;

            case "menuEscapeBtSettings":    // Settings from Escape menu
                menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;
                menuSettingsContainer.Enabled = true; menuSettingsContainer.Visible = true;

                settingsOpenedFrom = "pause";
                break;

            case "menuEscapeBtControls":    // Controlls from Escape menu
                menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;
                menuControlsContainer.Enabled = true; menuControlsContainer.Visible = true;

                controlsOpenedFrom = "pause";
                break;

            case "menuControlsBtBack":    // Escape menu from Controlls
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

            case "menuEscapeBtMainMenu":    // Start menu from Escape menu
                menuControlsContainer.Enabled = false; menuControlsContainer.Visible = false;
                menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;

                timer1.Enabled = false;
                inputEnabled = false;

                gameScreen.Enabled = false; gameScreen.Visible = false;
                menuMainContainer.Enabled = true;
                menuMainContainer.Visible = true;
                break;

            case "menuSettingsBtBack":  // Leaving settings
                menuSettingsContainer.Enabled = false; menuSettingsContainer.Visible = false;
                if (settingsOpenedFrom == "mainmenu")   // from Main menu
                {
                    menuMainContainer.Visible = true; menuMainContainer.Enabled = true;
                }
                else if (settingsOpenedFrom == "pause") // from Escape menu
                {
                    menuEscapeContainer.Visible = true; menuEscapeContainer.Enabled = true;
                }
                break;

            case "menuSettingsLbR2ControlL":   // Language selection (back)
                if (!(langIndex <= 0))
                {
                    langIndex--;
                    SettingsUpdate();
                }
                break;

            case "menuSettingsLbR2ControlR":   // Language selection (foward)
                if (!(langIndex >= languages.Count() - 1))
                {
                    langIndex++;
                    SettingsUpdate();
                }
                break;

            case "menuSettingsLbR3ControlL":   // Sound selection (back)
                if (!(soundOutputType <= 0))
                {
                    soundOutputType--;
                    SettingsUpdate();
                }
                break;

            case "menuSettingsLbR3ControlR":   // Sound selection (foward)
                if (!(soundOutputType >= soundOutputTypeList.Count() - 1))
                {
                    soundOutputType++;
                    SettingsUpdate();
                }
                break;

            case "menuSettingsLbR4ControlL":   // Text scale selection (back)
                if (!(textScaleIndex <= 0))
                {
                    textScaleIndex--;
                    SettingsUpdate();
                    AdjustFontSize(textScaleIndex);
                }
                break;

            case "menuSettingsLbR4ControlR":   // Text scale selection (foward)
                if (!(textScaleIndex >= fontSizeList.Count() - 1))
                {
                    textScaleIndex++;
                    SettingsUpdate();
                    AdjustFontSize(textScaleIndex);
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
            SpawnLevel(currentLevel);
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
            if (!(deviceInstance.Type == DeviceType.Gamepad))   // Doesn't add Xbox
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

    #endregion Inputs

    #region Level design

    private void Level1()
    {
        gameScreen.Height = 864;

        Terrain pictureBox1 = new(0, 768, 1339, 96, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox2 = new(337, 717, 77, 51, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox3 = new(645, 684, 222, 84, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox4 = new(942, 609, 190, 84, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox5 = new(281, 314, 66, 113, 0, 0, "collision", Color.FromArgb(115, 149, 218), false, Resources.blank, gameScreen);
        Terrain pictureBox6 = new(355, 697, 51, 20, 0, 0, "collision spring", Color.FromArgb(154, 205, 50), false, Resources.blank, gameScreen);
        Terrain pictureBox7 = new(67, 549, 66, 113, 0, 0, "collision", Color.FromArgb(115, 149, 218), false, Resources.blank, gameScreen);
        Terrain pictureBox8 = new(77, 133, 66, 113, 0, 0, "collision", Color.FromArgb(115, 149, 218), false, Resources.blank, gameScreen);
        Terrain pictureBox9 = new(507, 423, 222, 62, 0, 0, "collision", Color.FromArgb(115, 149, 218), false, Resources.blank, gameScreen);
        Terrain pictureBox10 = new(1256, 549, 259, 56, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox11 = new(942, 279, 222, 84, 0, 0, "collision", Color.FromArgb(115, 149, 218), false, Resources.blank, gameScreen);
        Terrain pictureBox12 = new(470, 111, 397, 55, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);

        terrainArray = new Terrain[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, };
        strawberryArray = new Strawberry[] { };

        player.Left = 186;
        player.Top = 701;

        CameraFocus("Bottom");
    }

    private void Level2()
    {
        gameScreen.Height = 1627;

        Terrain pictureBox31 = new(1352, 448, 47, 28, 0, 0, "collision spring", Color.FromArgb(154, 205, 50), false, Resources.blank, gameScreen);
        Terrain pictureBox30 = new(1349, 684, 50, 67, 0, 0, "collision", Color.FromArgb(128, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox29 = new(1225, 883, 297, 28, 0, 0, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox28 = new(16, 1, 1209, 17, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox27 = new(1522, -1576, 16, 1627, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox23 = new(-1, -1576, 16, 1627, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox22 = new(913, 506, 114, 88, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox26 = new(1341, 476, 69, 63, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox25 = new(1181, 18, 44, 127, 0, 0, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox24 = new(1181, 394, 44, 517, 0, 0, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox21 = new(378, 748, 114, 88, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox20 = new(16, 394, 230, 33, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox19 = new(182, 428, 64, 53, 0, 0, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox18 = new(182, 598, 64, 62, 0, 0, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox17 = new(246, 911, 1276, 35, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox16 = new(182, 659, 64, 445, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox15 = new(16, 1264, 121, 28, 0, 0, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox14 = new(262, 1264, 121, 28, 0, 0, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox13 = new(488, 1264, 121, 28, 0, 0, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox12 = new(609, 1346, 50, 75, 0, 0, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox11 = new(1218, 1448, 50, 67, 0, 0, "collision", Color.FromArgb(128, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox10 = new(1071, 30, 338, 20, 0, 0, "collision", Color.FromArgb(255, 0, 0), false, Resources.blank, gameScreen);
        Terrain pictureBox9 = new(1409, 1519, 50, 108, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox8 = new(1098, 1323, 294, 43, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox7 = new(1021, 1519, 50, 108, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox6 = new(838, 1344, 50, 283, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox5 = new(838, 1134, 50, 210, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox4 = new(659, 1323, 179, 22, 0, 0, "collision jump-through", Color.FromArgb(65, 50, 31), false, Resources.blank, gameScreen);
        Terrain pictureBox3 = new(609, 1134, 50, 210, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox1 = new(16, 1293, 593, 52, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Terrain pictureBox2 = new(16, 1519, 476, 52, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);

        terrainArray = new Terrain[] { pictureBox31, pictureBox30, pictureBox29, pictureBox28, pictureBox27, pictureBox23, pictureBox22, pictureBox26, pictureBox25, pictureBox24, pictureBox21, pictureBox20, pictureBox19, pictureBox18, pictureBox17, pictureBox16, pictureBox15, pictureBox14, pictureBox13, pictureBox12, pictureBox11, pictureBox10, pictureBox9, pictureBox8, pictureBox7, pictureBox6, pictureBox5, pictureBox4, pictureBox3, pictureBox1, pictureBox2, };
        strawberryArray = new Strawberry[] { };

        player.Left = 86;
        player.Top = 1453;

        CameraFocus("Bottom");
    }

    private void Level3()
    {
        gameScreen.Height = 864;

        Terrain elevator1 = new(900, 600, 150, 180, 1200, 550, "collision elevator", Color.FromArgb(68, 101, 147), false, Resources.blank, gameScreen);
        Terrain pictureBox1 = new(0, 800, 1536, 64, 0, 0, "collision", Color.FromArgb(72, 55, 34), false, Resources.blank, gameScreen);
        Strawberry strawberry1 = new(450, 400, gameScreen);
        strawberry1.pb.Click += StrawberryClick;

        terrainArray = new Terrain[] { pictureBox1, elevator1 };
        strawberryArray = new Strawberry[] { strawberry1 };

        player.Left = 186;
        player.Top = 733;

        CameraFocus("Bottom");
    }

    private void StrawberryClick(object sender, EventArgs e)
    {
        foreach (Strawberry strawberry in strawberryArray)
        {
            strawberry.Collect();
        }
    }

    private void SpawnLevel(int level)
    {
        // Destroy old level
        foreach (Terrain terrain in terrainArray)
            DestroyAll(terrain.pb, gameScreen);

        foreach (Strawberry strawberry in strawberryArray)
            DestroyAll(strawberry.pb, gameScreen);

        // Spawn new level
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
                soundJumped.PlaySound(0, volume);
                break;

            case "landed":
                switch (lastMaterial)
                {
                    case "ice":
                        soundLandedIce.PlaySound(landVariant, volume);

                        if (landVariant >= 5)
                            landVariant = 1;
                        else landVariant++;
                        break;
                }
                break;

            case "spring":
                soundSpring.PlaySound(0, volume);
                break;

            case "grabOn":
                if (!grabbedOn)
                {
                    switch (lastMaterial)
                    {
                        case "ice":
                            soundGrabOnIce.PlaySound(grabOnVariant, volume);

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
                    soundGrabOff.PlaySound(0, volume);
                }
                grabbedOn = false;
                break;

            case "dash":
                soundDash.PlaySound(0, volume);
                break;
        }
    }

    #endregion Sound design

    #region Config

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

    List<string> fontSizeList = new List<string>()
    {
        "0,8x\t0.8x",
        "1x\t1x"
    };

    private void SettingsUpdate()
    {
        // Start menu
        menuMainBtPlay.Text = texts[2].Split('\t')[langIndex];
        menuMainBtSettings.Text = texts[3].Split('\t')[langIndex];
        menuMainBtControls.Text = texts[8].Split('\t')[langIndex];
        menuMainBtClose.Text = texts[4].Split('\t')[langIndex];
        mainLbInfo.Text = texts[11].Split('\t')[langIndex];

        // Settings
        menuSettingsLbTitle.Text = texts[13].Split('\t')[langIndex];
        menuSettingsLbL1.Text = texts[14].Split('\t')[langIndex];

        /// Language
        menuSettingsLbL2.Text = texts[15].Split('\t')[langIndex];

        menuSettingsLbR2Language.Text = languages[langIndex];

        foreach (Control text in menuSettingsLbR2Container.Controls)
            text.ForeColor = Color.FromArgb(68, 101, 147);

        if (langIndex <= 0)
            menuSettingsLbR2ControlL.ForeColor = Color.FromArgb(130, 160, 200);

        if (langIndex >= languages.Count() - 1)
            menuSettingsLbR2ControlR.ForeColor = Color.FromArgb(130, 160, 200);

        ConfigFileUpdate(2, menuSettingsLbR2Language.Text);


        /// Sound selection
        menuSettingsLbL3.Text = texts[16].Split('\t')[langIndex];

        menuSettingsLbR3Input.Text = soundOutputTypeList[soundOutputType].Split('\t')[langIndex];

        foreach (Control text in menuSettingsLbR3Container.Controls)
            text.ForeColor = Color.FromArgb(68, 101, 147);

        switch (soundOutputType)
        {
            case 0:
                menuSettingsLbR3ControlL.ForeColor = Color.FromArgb(130, 160, 200);
                SoundManager.bannedSound = false;
                SoundManager.bannedMusic = false;

                ConfigFileUpdate(3, "Music");
                break;

            case 1:
                SoundManager.bannedSound = false;
                SoundManager.bannedMusic = true;

                ConfigFileUpdate(3, "Sound");
                break;

            case 2:
                menuSettingsLbR3ControlR.ForeColor = Color.FromArgb(130, 160, 200);
                SoundManager.bannedSound = true;
                SoundManager.bannedMusic = true;

                ConfigFileUpdate(3, "Off");
                break;
        }


        /// Font scale selection
        menuSettingsLbL4.Text = texts[17].Split('\t')[langIndex];

        menuSettingsLbR4FontSize.Text = fontSizeList[textScaleIndex].Split('\t')[langIndex];

        foreach (Control text in menuSettingsLbR4Container.Controls)
            text.ForeColor = Color.FromArgb(68, 101, 147);

        if (textScaleIndex <= 0)
            menuSettingsLbR4ControlL.ForeColor = Color.FromArgb(130, 160, 200);

        if (textScaleIndex >= fontSizeList.Count() - 1)
            menuSettingsLbR4ControlR.ForeColor = Color.FromArgb(130, 160, 200);


        menuSettingsBtBack.Text = texts[5].Split('\t')[langIndex];

        // Controlls
        menuControlsLbTitle.Text = texts[19].Split('\t')[langIndex];
        menuControlsLbL1.Text = texts[20].Split('\t')[langIndex];
        menuControlsLbL2.Text = texts[21].Split('\t')[langIndex];
        menuControlsLbL3.Text = texts[22].Split('\t')[langIndex];
        menuControlsLbL4.Text = texts[23].Split('\t')[langIndex];
        menuControlsLbL5.Text = texts[24].Split('\t')[langIndex];
        menuControlsLbL6.Text = texts[25].Split('\t')[langIndex];
        lbKeyboard1.Text = texts[26].Split('\t')[langIndex];
        lbKeyboard2.Text = texts[27].Split('\t')[langIndex];
        lbKeyboard3.Text = texts[28].Split('\t')[langIndex];
        lbKeyboard4.Text = texts[29].Split('\t')[langIndex];
        lbKeyboard5.Text = texts[30].Split('\t')[langIndex];
        lbKeyboard6.Text = texts[31].Split('\t')[langIndex];
        menuControlsBtBack.Text = texts[5].Split('\t')[langIndex];

        // Escape menu
        menuEscapeLbTitle.Text = texts[33].Split('\t')[langIndex];
        menuEscapeBtContinue.Text = texts[6].Split('\t')[langIndex];
        menuEscapeBtScreenReset.Text = texts[7].Split('\t')[langIndex];
        menuEscapeBtSettings.Text = texts[3].Split('\t')[langIndex];
        menuEscapeBtControls.Text = texts[8].Split('\t')[langIndex];
        menuEscapeBtMainMenu.Text = texts[9].Split('\t')[langIndex];
    }

    private void AdjustFontSize(int index)
    {
        menuMainLbTitle.Font = new Font("Segoe UI", (index == 0 ? 62 : 84), FontStyle.Bold);
        menuMainLbSubtitle.Font = new Font("Segoe UI", (index == 0 ? 20 : 26), FontStyle.Bold);

        Label[] titles = new Label[] { menuSettingsLbTitle, menuControlsLbTitle, menuEscapeLbTitle };
        Label[] menuText = new Label[] { menuSettingsLbL1, menuSettingsLbL2, menuSettingsLbL3, menuSettingsLbL4, menuSettingsLbVolumeR1, menuSettingsLbR2ControlL, menuSettingsLbR2Language, menuSettingsLbR2ControlR, menuSettingsLbR3ControlL, menuSettingsLbR3Input, menuSettingsLbR3ControlR, menuSettingsLbR4ControlL, menuSettingsLbR4FontSize, menuSettingsLbR4ControlR, menuControlsLbL1, menuControlsLbL2, menuControlsLbL3, menuControlsLbL4, menuControlsLbL5, menuControlsLbL6, lbKeyboard1, lbKeyboard2, lbKeyboard3, lbKeyboard4, lbKeyboard5, lbKeyboard6 };
        Label[] text = new Label[] { menuMainLbAuthor };
        Label[] smallText = new Label[] { mainLbInfo };
        Button[] buttons = new Button[] { menuMainBtPlay, menuMainBtSettings, menuMainBtControls, menuMainBtClose, menuSettingsBtBack, menuControlsBtBack, menuEscapeBtContinue, menuEscapeBtScreenReset, menuEscapeBtSettings, menuEscapeBtControls, menuEscapeBtMainMenu };

        foreach (Label item in titles)
            item.Font = new Font("Segoe UI", (index == 0 ? 28 : 36), FontStyle.Bold);

        foreach (Label item in menuText)
            item.Font = new Font("Segoe UI Semibold", (index == 0 ? 18 : 24));

        foreach (Label item in text)
            item.Font = new Font("Segoe UI Semibold", (index == 0 ? 15 : 18));

        foreach (Label item in smallText)
            item.Font = new Font("Segoe UI", (index == 0 ? 11 : 12), FontStyle.Regular);

        foreach (Button item in buttons)
            item.Font = new Font("Segoe UI", (index == 0 ? 14 : 18), FontStyle.Bold);

        ConfigFileUpdate(4, index.ToString());
    }

    private void ConfigFileUpdate(int record, string data)
    {
        string[] config = new string[0];

        try
        {
            config = File.ReadAllLines("config.txt");
        }
        catch (Exception)
        {
            DialogResult dialogResult = MessageBox.Show("Nebylo možné uložit zmìny", "Chyba", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            if (dialogResult == DialogResult.Retry)
            {
                ConfigFileUpdate(record, data);
            }
            else
            {
                ConfigFileUpdate(0, "load");
            }
        }

        // 1 - Volume change
        // 2 - Language change
        // 3 - Sound type (music & sounds, sounds, off)
        // 4 - Text scale


        if (record != 0)
        {
            config[record] = data;

            try
            {
                File.WriteAllLines("config.txt", config);
            }
            catch (Exception)
            {
                DialogResult dialogResult = MessageBox.Show("Nebylo možné uložit zmìny", "Chyba", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.Retry)
                {
                    ConfigFileUpdate(record, data);
                }
                else
                {
                    ConfigFileUpdate(0, "load");
                }
            }
        }
        else if (data == "load")
        {
            AdjustFontSize(0);

            // Load volume
            volume = (float)(Convert.ToInt32(config[1]) * 0.01);
            menuSettingsLbVolumeR1.Text = Math.Floor(volume * 100).ToString();
            menuSettingsTrackR1.Value = (int)(volume * 20);

            // Load language
            switch (config[2])
            {
                case "Èesky":
                    langIndex = 0;
                    break;

                case "English":
                    langIndex = 1;
                    break;
            }

            // Load output type
            switch (config[3])
            {
                case "Off":
                    soundOutputType = 0;
                    break;

                case "Sound":
                    soundOutputType = 1;
                    break;

                case "Music":
                    soundOutputType = 2;
                    break;
            }

            // Load text scale
            textScaleIndex = Convert.ToInt32(config[4]);
            AdjustFontSize(textScaleIndex);
        }
    }

    // Protect timers from pause (if paused, timers will reset on continue)
    private void TimerHandler(string action)
    {
        switch (action)
        {
            case "Pause":
                if (timerJumpCooldown.Enabled) tTimerJumpCooldown = true;
                if (timerJumpHeadBumpCooldown.Enabled) tTimerJumpHeadBumpCooldown = true;
                if (timerGrabAfterJumpCooldown.Enabled) tTimerGrabAfterJumpCooldown = true;

                timerJumpCooldown.Stop();
                timerJumpHeadBumpCooldown.Stop();
                timerGrabAfterJumpCooldown.Stop();
                break;

            case "Play":
                if (tTimerJumpCooldown) timerJumpCooldown.Start();
                if (tTimerJumpHeadBumpCooldown) timerJumpHeadBumpCooldown.Start();
                if (tTimerGrabAfterJumpCooldown) timerGrabAfterJumpCooldown.Start();

                tTimerJumpCooldown = false;
                tTimerJumpHeadBumpCooldown = false;
                tTimerGrabAfterJumpCooldown = false;
                break;

            case "Reset":
                tTimerJumpCooldown = false;
                tTimerJumpHeadBumpCooldown = false;
                tTimerGrabAfterJumpCooldown = false;
                break;
        }
    }

    #endregion Config
}