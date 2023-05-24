using Celeste_WinForms.Properties;
using SharpDX.DirectInput;
using System.Diagnostics;
using XInputDotNetPure;
using ButtonState = XInputDotNetPure.ButtonState;

namespace Celeste_WinForms;

public partial class MainWindow : Form {
    // Global variables
    /// Player
    bool inputEnabled = false;

    bool left, right;
    bool leftInput, rightInput;
    bool upInput, downInput;

    bool jump, jumpInput, spaceReleased = true;
    bool slide;
    bool grab, grabInput;
    bool grabFirstTimeSupport;  // Pomoc pøi trackování pøichycení

    string facing = "", lastStraightFacing = "Right", lastStraightFacingBefore;
    bool midAir;
    bool jumpCooldown = false;
    bool grabAfterJumpCooldown = false;
    public static int movementSpeed;
    int movementSpeedTarget = 0;
    int movementSpeedMax = 6;
    public static int force;

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
    bool dashed, dashedBefore;
    bool dashedNonVertical;

    int playerLeftOffset, playerRightOffset;

    enum PlayerAnim { Climb, Dangling, Dash, Fall, Idle, Jump, Walk }
    PlayerAnim playerAnimNow;
    PlayerAnim playerAnimBefore;
    PlayerAnim playerAnimQueue;

    /// Falling block
    bool rodeOnFallingBlock;

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
    int spawnLocation = 0;  // For level5 (multiple spawn locations)
    // For level6 - end
    Stopwatch swEnd = new Stopwatch(); // Stopwatch for end animation
    bool endScreen = false;
    bool grabbedOn = false;

    static Terrain[] terrainArray;
    static Strawberry[] strawberryArray;

    /// Sound
    public enum SoundTypes { Dash, Death, FallingblockShake, Grab, GrabLetgo, Jump, JumpWall, Land, Spring, StrawberryGet, StrawberryTouch, ZipmoverTouch, ZipmoverImpact, ZipmoverReturn, ZipmoverReset }
    public static SoundTypes? soundQueue;
    public int soundOutputType = 0;
    bool grabbedBefore; // If was player grabbed last tick - for playing sound only once
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
    bool clickedController;

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

    public MainWindow() {
        InitializeComponent();

        ConfigFileUpdate(0, "load");
        SettingsUpdate();
        Level1();

        FindControllers();

        menuMainContainer.Enabled = true; menuMainContainer.Visible = true;
        PlayingOnInfo("keyboard");
    }

    #region Gaming controllers

    private void timerControllers_Tick(object sender, EventArgs e) {
        // Input reset
        xboxA = false; xboxB = false; xboxX = false; xboxY = false;
        xboxLeft = false; xboxRight = false; xboxUp = false; xboxDown = false;
        xboxTrigger = false; xboxOptions = false;

        psA = false; psB = false; psX = false; psY = false;
        psLeft = false; psRight = false; psUp = false; psDown = false;
        psTrigger = false; psOptions = false;

        // Xbox
        GamePadState xboxState = GamePad.GetState(PlayerIndex.One);
        if (xboxState.IsConnected) {
            if (inputEnabled) {
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
                xboxState.DPad.Down != previousXboxState.DPad.Down) {
                PlayingOnInfo("xbox");
                clickedController = true;
            }

            if (xboxState.ThumbSticks.Left.X >= 0.1 ||
                xboxState.ThumbSticks.Left.X <= -0.1 ||
                xboxState.ThumbSticks.Left.Y >= 0.1 ||
                xboxState.ThumbSticks.Left.Y <= -0.1 ||
                xboxState.ThumbSticks.Right.X >= 0.1 ||
                xboxState.ThumbSticks.Right.X <= -0.1 ||
                xboxState.ThumbSticks.Right.Y >= 0.1 ||
                xboxState.ThumbSticks.Right.Y <= -0.1 ||
                xboxState.Triggers.Left != 0 ||
                xboxState.Triggers.Right != 0) {
                PlayingOnInfo("xbox");
            }

            #endregion

            previousXboxState = xboxState;
        } else {
            previousXboxState = GamePad.GetState(PlayerIndex.One);
        }

        // PlayStation
        try {
            if (psControllerConnected) {
                joystick.Poll();

                var state = joystick.GetCurrentState();
                var buttons = state.Buttons;

                if (previousButtonsCleared) {
                    previousButtons = buttons;
                    previousButtonsCleared = false;
                }

                if (inputEnabled) {
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

                for (int i = 0; i <= 13; i++) {
                    if (buttons[i] != previousButtons[i]) {
                        PlayingOnInfo("playstation");
                        clickedController = true;
                    }
                }

                if ((state.X / 32768.0f) - 1 >= 0.1 ||
                    (state.X / 32768.0f) - 1 <= -0.1 ||
                    (state.Y / 32768.0f) - 1 >= 0.1 ||
                    (state.Y / 32768.0f) - 1 <= -0.1) {
                    PlayingOnInfo("playstation");
                }

                #endregion

                previousButtons = buttons;
            }
        } catch (Exception) {
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

        if (cOptions) {
            Escape("controller");
        }

        // Mouse cursor
        previousCursor = cursor;
        cursor = PointToClient(Cursor.Position);

        if (cursor != previousCursor) {
            // Input is "keyboard" if mouse is moved
            PlayingOnInfo("keyboard");
        }
    }

    #endregion Herní ovladaèe

    private void timer1_Tick(object sender, EventArgs e) {
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

        playerAnimBefore = playerAnimNow;
        dashedBefore = dashed;
        lastStraightFacingBefore = lastStraightFacing;


        #region Movement

        foreach (Terrain item in terrainArray) {
            item.onBlockLeftExclusive = false; item.onBlockRightExclusive = false;
            item.onBlockDown = false;
        }

        updateCamera();

        // Movement
        /// Inputs
        if (leftInput || cLeft) {
            left = true;
            facing = "Left";
            lastStraightFacing = "Left";
        } else
            left = false;

        if (rightInput || cRight) {
            right = true;
            facing = "Right";
            lastStraightFacing = "Right";
        } else
            right = false;

        if (upInput || cUp)
            facing += "Up";
        else if (downInput || cDown)
            facing += "Down";

        if (facing == "")
            facing = lastStraightFacing;

        // Interacton with blocks
        foreach (Terrain block in terrainArray.Where(block => block.pb.Tag != null)) {
            if (!block.pb.Tag.ToString().Contains("jump-through")) {
                // Side collisions
                if (block.pb.Tag.ToString().Contains("collision") && player.Bounds.IntersectsWith(block.pb.Bounds)) {
                    if (playerLeftOffset < block.pb.Right && player.Right > block.pb.Left + player.Width / 2 &&
                        player.Bottom > block.pb.Top + 1 && player.Top < block.pb.Bottom) {
                        left = false;
                        movementSpeed = 0;
                    }

                    if (playerRightOffset > block.pb.Left && player.Left < block.pb.Right - player.Width / 2 &&
                        player.Bottom > block.pb.Top + 1 && player.Top < block.pb.Bottom) {
                        right = false;
                        movementSpeed = 0;
                    }
                }

                // If the player is close to the block, it will only get closer by the difference between the edge of the player and the block (against bugs)
                if (block.pb.Tag.ToString().Contains("collision")) {
                    if (playerLeftOffset - block.pb.Right < (Math.Abs(movementSpeed) < movementSpeedMax ? movementSpeedMax : Math.Abs(movementSpeed)) && playerLeftOffset - block.pb.Right >= 0 &&
                        player.Bottom > block.pb.Top + 1 && player.Top < block.pb.Bottom) {
                        closeToBlockLeft = true;
                        closeToBlockLeftDist = playerLeftOffset - block.pb.Right;

                        if (playerLeftOffset - block.pb.Right == 0 &&
                            player.Top + player.Height / 2 < block.pb.Bottom) {
                            onBlockLeft = true;
                            block.onBlockLeftExclusive = true;
                            midAir = false;
                            playerBlockHeightDiff = player.Bottom - block.pb.Top;
                        }
                    }

                    if (block.pb.Left - playerRightOffset < (movementSpeed < movementSpeedMax ? movementSpeedMax : movementSpeed) && block.pb.Left - playerRightOffset >= 0 &&
                        player.Bottom > block.pb.Top + 1 && player.Top < block.pb.Bottom) {
                        closeToBlockRight = true;
                        closeToBlockRightDist = block.pb.Left - playerRightOffset;

                        if (block.pb.Left - playerRightOffset == 0 &&
                            player.Top + player.Height / 2 < block.pb.Bottom) {
                            onBlockRight = true;
                            block.onBlockRightExclusive = true;
                            midAir = false;
                            playerBlockHeightDiff = player.Bottom - block.pb.Top;
                        }
                    }
                }

                // Slide activation
                if (((onBlockLeft && (leftInput || cLeft)) || (onBlockRight && (rightInput || cRight))) && force < 0) {
                    slide = true;
                    midAir = false;
                }

                // Grab activation
                if ((grabInput || cTrigger) && (onBlockLeft || onBlockRight)) {
                    grab = true;
                    soundMaterial = block.pb.Tag.ToString();
                    midAir = false;

                    // If grabbed, move 1 pixel up (against bugs)
                    if (!grabFirstTimeSupport) {
                        player.Top -= 1;
                        grabFirstTimeSupport = true;
                    }

                    lastGrabbedOn = onBlockLeft ? "Left" : onBlockRight ? "Right" : "";
                }
            }

            // midAir
            if (block.pb.Top - player.Bottom == -1 &&
                playerLeftOffset < block.pb.Right && playerRightOffset > block.pb.Left) {
                midAir = false;
            }
        }

        // User holds Space/Jump
        if ((jumpInput || (cA || cY)) && !midAir && !jumpCooldown && !grabAfterJumpCooldown) {
            jump = true;
            force = 15;
            PlayerAnimationChanged(PlayerAnim.Jump);

            if (((onBlockLeft || onBlockRight) && !onBlockDown) ||  // Touching block mid-air
                slide)  // Sliding on block
            {
                movementSpeed = onBlockLeft ? movementSpeedMax * 2 : onBlockRight ? -movementSpeedMax * 2 : 0;

                force = 11;

                PlaySound(SoundTypes.JumpWall);
            } else if (!grab)
                PlaySound(SoundTypes.Jump);

            // Grabbed on block
            if (grab) {
                if (onBlockLeft && facing.Contains("Right") || onBlockRight && facing.Contains("Left"))
                    force = 13;
                else
                    force = 11;

                int grabJumpPower = 3;

                movementSpeed += facing.Contains("Left") ? -movementSpeedMax * (grabJumpPower / 2) : facing.Contains("Right") ? movementSpeedMax * (grabJumpPower / 2) : 0;

                grab = false;
                grabAfterJumpCooldown = true;
                timerGrabAfterJumpCooldown.Enabled = true;

                PlaySound(SoundTypes.JumpWall);
            }

            jumpCooldown = true;
            timerJumpCooldown.Enabled = true;
        }

        // Grab - Jump on top of block (<25px: bottom of player - top of block)
        if (grab && !grabAfterJumpCooldown) {
            if (playerBlockHeightDiff < 25) {
                force = Math.Abs(playerBlockHeightDiff / 2);
                climbed = true;

                grab = false;
                grabAfterJumpCooldown = true;
                timerGrabAfterJumpCooldown.Enabled = true;
            }
        }
        if (climbed && !(onBlockLeft || onBlockRight)) {
            movementSpeed = lastGrabbedOn == "Left" ? -12 : lastGrabbedOn == "Right" ? 12 : 0;
            climbed = false;
            lastGrabbedOn = "";
        }

        // Dash ability
        if ((dashInput || (cB || cX)) && !dashed) {
            int dashPower = 4;

            switch (facing) {
                case "Right":
                    if (!onBlockRight)
                        movementSpeed = (dashPower * movementSpeedMax);
                    force = 0;

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "RightUp":
                    if (!onBlockRight)
                        movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * dashPower * movementSpeedMax);
                    force = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * dashPower * movementSpeedMax);

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "Up":
                    movementSpeed = 0;
                    force = dashPower * 5;
                    break;

                case "LeftUp":
                    if (!onBlockLeft)
                        movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * -dashPower * movementSpeedMax);
                    force = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * dashPower * movementSpeedMax);

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "Left":
                    if (!onBlockLeft)
                        movementSpeed = -dashPower * movementSpeedMax;
                    force = 0;

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "LeftDown":
                    if (!onBlockLeft)
                        movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * -dashPower * movementSpeedMax);
                    if (!onBlockDown)
                        force = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * -dashPower * movementSpeedMax);

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;

                case "Down":
                    movementSpeed = 0;
                    if (!onBlockDown)
                        force = -dashPower * 5;
                    break;

                case "RightDown":
                    if (!onBlockRight)
                        movementSpeed = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * dashPower * movementSpeedMax);
                    if (!onBlockDown)
                        force = Convert.ToInt32((double)(Math.Sqrt(2) / (double)2) * -dashPower * movementSpeedMax);

                    dashedNonVertical = true;
                    timerDashedNonVertical.Enabled = true;
                    break;
            }

            PlaySound(SoundTypes.Dash);
            timerDashCooldown.Enabled = true;
            PlayerAnimationChanged(PlayerAnim.Dash);
            dashed = true;
        }

        // Grab ability
        if (grab && !grabAfterJumpCooldown && Math.Abs(movementSpeed) < movementSpeedMax) {
            if (!grabbedBefore)
                PlaySound(SoundTypes.Grab);
            grabbedBefore = true;

            force = 0;
            movementSpeedTarget = 0;

            if (!((leftInput || cLeft) || (rightInput || cRight) || (upInput || cUp) || (downInput || cDown)))
                facing = onBlockLeft ? "Left" : onBlockRight ? "Right" : lastStraightFacing;

            if (facing == "Up" && !onBlockUp) {
                if (!onBlockUp)
                    force = movementSpeedMax / 5 * 4;
                else
                    force = closeToBlockUpDist;
            }

            if (facing == "Down" && !onBlockDown) {
                if (!closeToBlockDown)
                    force = -movementSpeedMax / 5 * 4;
                else
                    force = -closeToBlockDownDist;
            }

            player.Top -= force;
        } else  // Gravity
            {
            if (closeToBlockDown) {
                player.Top += closeToBlockDownDist - 1;
                force = 0;
            } else if (closeToBlockUp) {
                player.Top -= closeToBlockUpDist;
            }

            player.Top -= force;

            if (force > (slide ? -2 : -25) && !dashedNonVertical & !closeToBlockDown) {
                force -= 1;
            }

            grabbedBefore = false;
        }


        closeToBlockDown = false;
        onBlockDown = false;
        onBlockUp = false;

        foreach (PictureBox block in gameScreen.Controls.OfType<PictureBox>().Where(block => block.Tag != null)) {
            // If the player is close to the block, it only gets closer by the difference between the edge of the player and the block (against bugs)
            // From block top
            if (block.Tag.ToString().Contains("collision")) {
                if (block.Top + 1 - player.Bottom <= -force + 1 &&
                    playerLeftOffset <= block.Right && playerRightOffset >= block.Left &&
                    player.Bottom < block.Top) {
                    closeToBlockDown = true;
                    closeToBlockDownDist = block.Top - player.Bottom + 1;
                }
            }

            // Interaction with blocks
            if (block.Tag.ToString().Contains("collision") && player.Bounds.IntersectsWith(block.Bounds) &&
                playerLeftOffset < block.Right &&
                playerRightOffset > block.Left) {
                // Bottom collision (top of block)
                if (player.Bottom == block.Top + 1 && player.Top < block.Top) {
                    player.Top = block.Top - player.Height + 1;
                    force = 0;
                    jump = false;

                    onBlockDown = true;
                    grabFirstTimeSupport = false;

                    foreach (Terrain item in terrainArray) {
                        item.onBlockDown = true;
                    }

                    closeToBlockDown = false;
                    if (!timerDashCooldown.Enabled)
                        dashed = false;

                    if (!block.Tag.ToString().Contains("spring")) {
                        if (!touchedGround) {
                            soundMaterial = block.Tag.ToString();
                            PlaySound(SoundTypes.Land);
                        }
                        touchedGround = true;
                    }
                }

                // If the player is close to the block, it only gets closer by the difference between the edge of the player and the block (against bugs)
                // From block bottom
                if (block.Tag.ToString().Contains("collision") && !block.Tag.ToString().Contains("jump-through")) {
                    if (player.Top - block.Bottom <= force + 1 &&
                        playerLeftOffset <= block.Right && playerRightOffset >= block.Left &&
                        player.Bottom < block.Top) {
                        closeToBlockUp = true;
                        closeToBlockUpDist = player.Top - block.Bottom;

                        if (player.Top - block.Bottom == 0) {
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
                playerRightOffset - 1 > block.Left) {
                force = 22;
                jump = true;
                dashed = false;

                PlaySound(SoundTypes.Spring);
                PlayerAnimationChanged(playerAnimQueue);
            }
        }

        // Sideways movement
        if (left ^ right && !slide && !grab) {
            if (left) {
                movementSpeedTarget = -movementSpeedMax;

                if (closeToBlockLeft) {
                    movementSpeed = 0;
                    player.Left -= closeToBlockLeftDist;
                } else if (movementSpeed != movementSpeedTarget) {
                    movementSpeed += movementSpeed < movementSpeedTarget ? 1 : movementSpeed > movementSpeedTarget ? -1 : 0;

                    if (!midAir)
                        movementSpeed += movementSpeed < movementSpeedTarget ? 1 : movementSpeedTarget - movementSpeed;
                }
            }

            if (right) {
                movementSpeedTarget = movementSpeedMax;

                if (closeToBlockRight) {
                    movementSpeed = 0;
                    player.Left += closeToBlockRightDist;
                } else if (movementSpeed != movementSpeedTarget) {
                    movementSpeed += movementSpeed < movementSpeedTarget ? 1 : movementSpeed > movementSpeedTarget ? -1 : 0;

                    if (!midAir)
                        movementSpeed += movementSpeed > movementSpeedTarget ? -1 : movementSpeedTarget - movementSpeed;
                }
            }
        } else {
            if (movementSpeed != 0) {
                movementSpeedTarget = 0;

                if (movementSpeed < -closeToBlockLeftDist && closeToBlockLeft) {
                    movementSpeed = 0;
                    player.Left -= closeToBlockLeftDist;
                } else if (movementSpeed > closeToBlockRightDist && closeToBlockRight) {
                    movementSpeed = 0;
                    player.Left += closeToBlockRightDist;
                } else
                    movementSpeed += movementSpeed < movementSpeedTarget ? 1 : movementSpeed > movementSpeedTarget ? -1 : 0;
            }
        }
        player.Left += movementSpeed;

        #endregion Movement

        #region Level teleports

        foreach (Terrain teleport in terrainArray.Where(teleport => teleport.pb.Tag.ToString().Contains("level"))) {

            if (player.Bounds.IntersectsWith(teleport.pb.Bounds)) {
                string levelNumber = "";

                foreach (char letter in teleport.pb.Tag.ToString()) {
                    if (char.IsDigit(letter))
                        levelNumber += letter;
                }

                SpawnLevel(Convert.ToInt32(levelNumber));
            }
        }

        // Level end
        foreach (Terrain end in terrainArray.Where(end => end.pb.Tag.ToString() == "end")) {

            if (player.Bounds.IntersectsWith(end.pb.Bounds)) {
                timerEndAnim.Enabled = true;
            }
        }

        #endregion

        #region Elevators

        foreach (Terrain elevator in terrainArray.Where(elevator => elevator.pb.Tag.ToString().Contains("elevator"))) {
            if ((player.Bounds.IntersectsWith(elevator.pb.Bounds) || (elevator.onBlockLeftExclusive || elevator.onBlockRightExclusive)) && !elevator.moving)
                elevator.moving = true;

            if (elevator.moving) {
                elevator.elevatorTexturePhase = 1;

                elevator.ElevatorAnimation(player, grabbedOn, playerLeftOffset, playerRightOffset, movementSpeed);

                if (elevator.resetForce)
                    force = 0;

                if (elevator.playerKill) {
                    elevator.playerKill = false;
                    PlaySound(SoundTypes.Death);
                    SpawnLevel(currentLevel);
                }
            }


            // Change elevator texture
            switch (elevator.elevatorTexturePhase) {
                case 0:
                    switch (elevator.pb.Width) {
                        case 120:
                            elevator.pb.BackgroundImage = Resources.elevator3_red;
                            break;

                        case 160:
                            elevator.pb.BackgroundImage = Resources.elevator4_red;
                            break;

                        case 200:
                            elevator.pb.BackgroundImage = Resources.elevator5_red;
                            break;
                    }
                    break;

                case 1:
                    switch (elevator.pb.Width) {
                        case 120:
                            elevator.pb.BackgroundImage = Resources.elevator3_green;
                            break;

                        case 160:
                            elevator.pb.BackgroundImage = Resources.elevator4_green;
                            break;

                        case 200:
                            elevator.pb.BackgroundImage = Resources.elevator5_green;
                            break;
                    }
                    break;

                case 2:
                    switch (elevator.pb.Width) {
                        case 120:
                            elevator.pb.BackgroundImage = Resources.elevator3_yellow;
                            break;

                        case 160:
                            elevator.pb.BackgroundImage = Resources.elevator4_yellow;
                            break;

                        case 200:
                            elevator.pb.BackgroundImage = Resources.elevator5_yellow;
                            break;
                    }
                    break;
            }
        }

        #endregion

        #region Falling blocks

        foreach (Terrain fallingBlock in terrainArray.Where(fallingBlock => fallingBlock.pb.Tag.ToString().Contains("falling"))) {
            if ((player.Bounds.IntersectsWith(fallingBlock.pb.Bounds) || (fallingBlock.onBlockLeftExclusive || fallingBlock.onBlockRightExclusive)) && !fallingBlock.falling && !fallingBlock.fallen)
                fallingBlock.falling = true;

            if (fallingBlock.falling) {
                if (player.Bounds.IntersectsWith(fallingBlock.pb.Bounds) || (fallingBlock.onBlockLeftExclusive || fallingBlock.onBlockRightExclusive)) {
                    rodeOnFallingBlock = true;

                    fallingBlock.onFallingBlock = true;
                } else {
                    fallingBlock.onFallingBlock = false;
                }

                foreach (Terrain terrain in terrainArray.Where(terrain => terrain.pb.Tag.ToString().Contains("collision"))) {
                    if (fallingBlock.pb.Bounds.IntersectsWith(terrain.pb.Bounds) &&
                        terrain != fallingBlock) {
                        fallingBlock.fallingOnGround = true;
                        fallingBlock.fallingGroundedPos = terrain.pb.Top - fallingBlock.pb.Height;
                    }
                }

                fallingBlock.FallingAnimation(player);

                // Player jumps off when in motion
                if (!((player.Bounds.IntersectsWith(fallingBlock.pb.Bounds) || (fallingBlock.onBlockLeftExclusive || fallingBlock.onBlockRightExclusive))) && fallingBlock.moving && rodeOnFallingBlock) {
                    force -= fallingBlock.fallingForce;
                    rodeOnFallingBlock = false;
                }
            }
        }

        #endregion

        #region Strawberries

        foreach (Strawberry strawberry in strawberryArray) {
            if (player.Bounds.IntersectsWith(strawberry.pb.Bounds))
                strawberry.tracking = true;

            if (strawberry.tracking) {
                strawberry.TrackTarget(player);

                // If player is on ground, start CollectingTime timer
                if (onBlockDown && !strawberry.timerCollectingTime.Enabled) {
                    strawberry.timerCollectingTime.Enabled = true;

                } else if (!onBlockDown && strawberry.timerCollectingTime.Enabled) {
                    strawberry.timerCollectingTime.Enabled = false;
                }
            }
        }

        #endregion

        #region Spikes

        foreach (Terrain spike in terrainArray.Where(spike => spike.pb.Tag.ToString().Contains("spike"))) {
            if (player.Bounds.IntersectsWith(spike.pb.Bounds) || spike.onBlockLeftExclusive || spike.onBlockRightExclusive) {
                PlaySound(SoundTypes.Death);
                SpawnLevel(currentLevel);
            }
        }

        #endregion

        #region Player texture

        if (onBlockDown) {
            if (Math.Abs(movementSpeed) > 0) {
                playerAnimNow = PlayerAnim.Walk;
            } else {
                playerAnimNow = PlayerAnim.Idle;
            }
        }

        if (midAir) {
            playerAnimNow = PlayerAnim.Fall;
        }

        if (grab) {
            if (Math.Abs(force) > 0) {
                playerAnimNow = PlayerAnim.Climb;
            } else {
                playerAnimNow = PlayerAnim.Dangling;
            }
        }

        // If state changed, change texture
        if (playerAnimBefore != playerAnimNow ||
            (dashed != dashedBefore && dashed) ||
            lastStraightFacingBefore != lastStraightFacing) {
            PlayerAnimationChanged(playerAnimNow);
        }

        #endregion

        #region Sound queue from other classes

        if (soundQueue != null) {
            PlaySound((SoundTypes)soundQueue);
            soundQueue = null;
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
        clickedController = false;

        if (!onBlockDown)
            touchedGround = false;

        // Reset the game if the player gets off the screen
        if (player.Bottom < gameScreen.Top || player.Left > gameScreen.Right || player.Top > gameScreen.Bottom + gameScreen.Height - 864 || player.Right < gameScreen.Left) {
            PlaySound(SoundTypes.Death);
            SpawnLevel(currentLevel);
        }
    }

    #region Player textures Update

    // Player textures
    private void PlayerAnimationChanged(PlayerAnim animation) {

        if (animation == PlayerAnim.Dash) {
            timerOneTimePlayerAnimation.Enabled = false;
            timerOneTimePlayerAnimation.Enabled = true;

            if (lastStraightFacing == "Left")
                player.Image = Resources.player_dash_dl;
            else if (lastStraightFacing == "Right")
                player.Image = Resources.player_dash_dr;
        }

        if (animation == PlayerAnim.Jump) {
            timerOneTimePlayerAnimation.Enabled = false;
            timerOneTimePlayerAnimation.Enabled = true;

            if (lastStraightFacing == "Left")
                player.Image = Resources.player_jump_nl;
            else if (lastStraightFacing == "Right")
                player.Image = Resources.player_jump_nr;
        }

        if (!timerOneTimePlayerAnimation.Enabled) {
            if (!dashed) {
                switch (animation) {
                    case PlayerAnim.Climb:
                        if (lastGrabbedOn == "Left")
                            player.Image = Resources.player_climb_nl;
                        else if (lastGrabbedOn == "Right")
                            player.Image = Resources.player_climb_nr;
                        break;
                    case PlayerAnim.Dangling:
                        if (lastGrabbedOn == "Left")
                            player.Image = Resources.player_dangling_nl;
                        else if (lastGrabbedOn == "Right")
                            player.Image = Resources.player_dangling_nr;
                        break;
                    case PlayerAnim.Fall:
                        if (lastStraightFacing == "Left")
                            player.Image = Resources.player_fall_nl;
                        else if (lastStraightFacing == "Right")
                            player.Image = Resources.player_fall_nr;
                        break;
                    case PlayerAnim.Idle:
                        if (lastStraightFacing == "Left")
                            player.Image = Resources.player_idle_nl;
                        else if (lastStraightFacing == "Right")
                            player.Image = Resources.player_idle_nr;
                        break;
                    case PlayerAnim.Walk:
                        if (lastStraightFacing == "Left")
                            player.Image = Resources.player_walk_nl;
                        else if (lastStraightFacing == "Right")
                            player.Image = Resources.player_walk_nr;
                        break;
                }
            } else {
                switch (animation) {
                    case PlayerAnim.Climb:
                        if (lastGrabbedOn == "Left")
                            player.Image = Resources.player_climb_dl;
                        else if (lastGrabbedOn == "Right")
                            player.Image = Resources.player_climb_dr;
                        break;
                    case PlayerAnim.Dangling:
                        if (lastGrabbedOn == "Left")
                            player.Image = Resources.player_dangling_dl;
                        else if (lastGrabbedOn == "Right")
                            player.Image = Resources.player_dangling_dr;
                        break;
                    case PlayerAnim.Fall:
                        if (lastStraightFacing == "Left")
                            player.Image = Resources.player_fall_dl;
                        else if (lastStraightFacing == "Right")
                            player.Image = Resources.player_fall_dr;
                        break;
                    case PlayerAnim.Idle:
                        if (lastStraightFacing == "Left")
                            player.Image = Resources.player_idle_dl;
                        else if (lastStraightFacing == "Right")
                            player.Image = Resources.player_idle_dr;
                        break;
                    case PlayerAnim.Walk:
                        if (lastStraightFacing == "Left")
                            player.Image = Resources.player_walk_dl;
                        else if (lastStraightFacing == "Right")
                            player.Image = Resources.player_walk_dr;
                        break;
                }
            }
        } else {
            playerAnimQueue = animation;
        }
    }

    #endregion Player textures

    private void timerOneTimePlayerAnimation_Tick(object sender, EventArgs e) {
        timerOneTimePlayerAnimation.Enabled = false;
        PlayerAnimationChanged(playerAnimQueue);
    }

    // Elevator
    public static void ElevatorJumpedOff(int elevatorMovementSpeed, double multiplierX, double multiplierY) {
        movementSpeed += (int)(elevatorMovementSpeed * multiplierX * 2);
        force += (int)(elevatorMovementSpeed * multiplierY * 2);
    }

    // End animation
    private void timerEndAnim_Tick(object sender, EventArgs e) {
        if (!swEnd.IsRunning) {
            swEnd.Start();
        }

        TimeSpan ts = swEnd.Elapsed;

        if (ts.TotalMilliseconds >= 1500 && ts.TotalMilliseconds < 2000) {
            inputEnabled = false;
        }

        if (ts.TotalMilliseconds >= 2000 && ts.TotalMilliseconds < 5000) {
            jumpInput = true;
        }

        if (ts.TotalMilliseconds >= 5000 && ts.TotalMilliseconds < 7000) {
            timer1.Enabled = false;
            tableLPEndscreen.Visible = true;
            lbEndContinue.Visible = false;
            tableLPEndscreen.BringToFront();
            KeyDown -= MainWindow_KeyDown;
        }

        if (ts.TotalMilliseconds >= 7000) {
            KeyDown += endingMainWindow_KeyDown;

            if (clickedController) {
                clickedController = false;
                endingMainMenu();
            }

            lbEndContinue.Visible = true;
        }
    }

    private void timerDashCooldown_Tick(object sender, EventArgs e) {
        timerDashCooldown.Enabled = false;
    }

    #region Camera

    private void updateCamera() {
        cameraMovementSpeedTarget = (432 - playerCenterY) - gameScreen.Top;

        if (gameScreen.Top >= 0 && playerCenterY < 432) {
            cameraMovementSpeedTarget = 0;
            cameraMovementSpeed = 0;
        } else if (gameScreen.Bottom <= 864 && gameScreen.Height - playerCenterY < 432) {
            cameraMovementSpeedTarget = 0;
            cameraMovementSpeed = 0;
        }

        if (cameraMovementSpeed != cameraMovementSpeedTarget) {
            if (cameraMovementSpeed < cameraMovementSpeedTarget) {
                cameraMovementSpeed += cameraMovementSpeedTarget / 10 + (force > 10 ? (force - 10) : 0) - cameraMovementSpeed;
            } else if (cameraMovementSpeed > cameraMovementSpeedTarget) {
                cameraMovementSpeed -= cameraMovementSpeed - cameraMovementSpeedTarget / 10 + (force < -10 ? (-force - 10) : 0);
            }
        }

        // Fix on the edge of the screen against spawn delays
        if (cameraMovementSpeedTarget > 0 && gameScreen.Top >= 0 - cameraMovementSpeed && playerCenterY < 432) {
            gameScreen.Top = 0;
        } else if (cameraMovementSpeedTarget < 0 && gameScreen.Bottom <= 864 - cameraMovementSpeed && gameScreen.Height - playerCenterY < 432) {
            gameScreen.Top = 864 - gameScreen.Height;
        } else {
            gameScreen.Top += cameraMovementSpeed;
        }
    }

    // Focuses the camera on the selected location
    private void CameraFocus(string focus) {
        switch (focus) {
            case "Player": gameScreen.Top = 432 - player.Top - player.Height / 2; break;
            case "Top": gameScreen.Top = 0; break;
            case "Bottom": gameScreen.Top = 864 - gameScreen.Height; break;
        }
    }

    #endregion Camera

    #region Cooldowns

    //// Jump cooldown after jumping - 30ms
    private void timerJumpCooldown_Tick(object sender, EventArgs e) {
        jumpInput = false;
        jumpCooldown = false;
        timerJumpCooldown.Enabled = false;
    }

    //// Jump cooldown if player hits his head on the bottom of the block - 300ms
    private void timerJumpHeadBumpCooldown_Tick(object sender, EventArgs e) {
        jumpCooldown = false;
        timerJumpHeadBumpCooldown.Enabled = false;
    }

    //// Grab cooldown after jumping from Grab - 280ms
    private void timerGrabCooldown_Tick(object sender, EventArgs e) {
        grabAfterJumpCooldown = false;
        timerGrabAfterJumpCooldown.Enabled = false;
    }

    //// Turning off gravity after Dash (if didn't dashed vertical) - 100ms
    private void timerDashedNonVertical_Tick(object sender, EventArgs e) {
        dashedNonVertical = false;
        timerDashedNonVertical.Enabled = false;
    }

    #endregion Cooldowns

    #region Inputs

    private void MainWindow_KeyDown(object sender, KeyEventArgs e) {
        PlayingOnInfo("keyboard");

        if (inputEnabled) {
            switch (e.KeyCode) {
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
                    if (spaceReleased) {
                        jumpInput = true;
                        spaceReleased = false;
                    }
                    break;

                case Keys.ControlKey:
                    if (ctrlReleased) {
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

        switch (e.KeyCode) {
            case Keys.Escape:
                Escape("keyboard");
                break;

            case Keys.NumPad0:
                developerKeys = true;
                break;
        }

        // Testing functions
        if (developerKeys) {
            switch (e.KeyCode) {
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

                case Keys.NumPad4:
                    SpawnLevel(4);
                    break;

                case Keys.NumPad5:
                    spawnLocation = 0;
                    SpawnLevel(5);
                    break;

                case Keys.NumPad6:
                    SpawnLevel(6);
                    break;

                case Keys.NumPad9:
                    SpawnLevel(9);
                    break;
            }
        }
    }

    private void Escape(string escInputType) {
        if (!menuMainContainer.Enabled) {
            if (menuEscapeContainer.Enabled)   // If on pause screen
            {
                menuEscapeBtContinue.PerformClick();
            } else if (menuSettingsContainer.Enabled) // If in settings
            {
                menuSettingsBtBack.PerformClick();
                if (escInputType == "controller")
                    menuEscapeBtContinue.PerformClick();
            } else if (menuControlsContainer.Enabled)   // If in controlls
            {
                menuControlsBtBack.PerformClick();
                if (escInputType == "controller")
                    menuEscapeBtContinue.PerformClick();
            } else   // If in game
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

    private void MainWindow_KeyUp(object sender, KeyEventArgs e) {
        switch (e.KeyCode) {
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
                PlaySound(SoundTypes.GrabLetgo);
                break;

            case Keys.NumPad0:
                developerKeys = false;
                break;
        }
    }

    private void endingMainWindow_KeyDown(object sender, KeyEventArgs e) {
        endingMainMenu();
    }

    private void endingMainMenu() {
        swEnd.Reset();
        tableLPEndscreen.Visible = false;
        timerEndAnim.Enabled = false;
        timer1.Enabled = true;
        KeyDown -= endingMainWindow_KeyDown;
        KeyDown += MainWindow_KeyDown;
        Escape(playingOn);
        menuEscapeBtMainMenu.PerformClick();
    }

    private void menuSettingsTrackR1_Scroll(object sender, EventArgs e) {
        SoundManager.volume = (float)(menuSettingsTrackR1.Value * 0.05);
        menuSettingsLbVolumeR1.Text = Math.Floor(SoundManager.volume * 100).ToString();

        ConfigFileUpdate(1, menuSettingsLbVolumeR1.Text);
    }

    private void buttonClicked(object sender, EventArgs e) {
        Control clickedControl = sender as Control;

        switch (clickedControl.Name) {
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
                if (controlsOpenedFrom == "mainmenu") {
                    menuMainContainer.Enabled = true; menuMainContainer.Visible = true;
                } else if (controlsOpenedFrom == "pause") {
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
                } else if (settingsOpenedFrom == "pause") // from Escape menu
                  {
                    menuEscapeContainer.Visible = true; menuEscapeContainer.Enabled = true;
                }
                break;

            case "menuSettingsLbR2ControlL":   // Language selection (back)
                if (!(langIndex <= 0)) {
                    langIndex--;
                    SettingsUpdate();
                }
                break;

            case "menuSettingsLbR2ControlR":   // Language selection (foward)
                if (!(langIndex >= languages.Count() - 1)) {
                    langIndex++;
                    SettingsUpdate();
                }
                break;

            case "menuSettingsLbR3ControlL":   // Sound selection (back)
                if (!(soundOutputType <= 0)) {
                    soundOutputType--;
                    SettingsUpdate();
                }
                break;

            case "menuSettingsLbR3ControlR":   // Sound selection (foward)
                if (!(soundOutputType >= soundOutputTypeList.Count() - 1)) {
                    soundOutputType++;
                    SettingsUpdate();
                }
                break;

            case "menuSettingsLbR4ControlL":   // Text scale selection (back)
                if (!(textScaleIndex <= 0)) {
                    textScaleIndex--;
                    SettingsUpdate();
                    AdjustFontSize(textScaleIndex);
                }
                break;

            case "menuSettingsLbR4ControlR":   // Text scale selection (foward)
                if (!(textScaleIndex >= fontSizeList.Count() - 1)) {
                    textScaleIndex++;
                    SettingsUpdate();
                    AdjustFontSize(textScaleIndex);
                }
                break;
        }

        Focus();
    }

    private void menuEscapeContinue(bool restart) {
        FindControllers();

        if (restart) {
            movementSpeed = 0; force = 0;
            SpawnLevel(currentLevel);
        }

        menuControlsContainer.Enabled = false; menuControlsContainer.Visible = false;
        menuEscapeContainer.Enabled = false; menuEscapeContainer.Visible = false;
        gameScreen.Enabled = true; gameScreen.Visible = true;
        timer1.Enabled = true;
        inputEnabled = true;

    }

    private void FindControllers() {
        var input = new DirectInput();
        var joystickGuid = Guid.Empty;

        foreach (DeviceInstance deviceInstance in input.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AllDevices)) {
            if (!(deviceInstance.Type == DeviceType.Gamepad))   // Doesn't add Xbox
                joystickGuid = deviceInstance.InstanceGuid;
        }

        if (joystickGuid == Guid.Empty)
            psControllerConnected = false;
        else {
            joystick = new Joystick(input, joystickGuid);
            joystick.Properties.BufferSize = 128;
            joystick.Acquire();

            psControllerConnected = true;
        }
    }

    private void PlayingOnInfo(string device) {
        playingOn = device;

        switch (device) {
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

    private void Level1() {
        gameScreen.Height = 864;

        Terrain pictureBox1 = new(68, 112, 40, 280, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox6 = new(28, 352, 40, 360, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox7 = new(68, 672, 80, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox8 = new(148, 672, 40, 40, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox9 = new(188, 672, 40, 120, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox10 = new(188, 792, 280, 40, 0, 0, "collision dirt", Color.FromArgb(197, 146, 93), null, gameScreen);
        Terrain pictureBox11 = new(428, 592, 40, 200, 0, 0, "collision dirt", Color.FromArgb(197, 146, 93), null, gameScreen);
        Terrain pictureBox12 = new(468, 592, 40, 40, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox13 = new(508, 592, 40, 120, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox14 = new(508, 712, 120, 40, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox15 = new(628, 712, 160, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox16 = new(748, 392, 40, 320, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox17 = new(748, 392, 40, 160, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox18 = new(788, 392, 80, 40, 0, 0, "collision metal", Color.FromArgb(103, 119, 135), null, gameScreen);
        Terrain pictureBox20 = new(868, 392, 40, 240, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox19 = new(868, 392, 40, 120, 0, 0, "collision metal", Color.FromArgb(93, 108, 124), null, gameScreen);
        Terrain pictureBox21 = new(868, 632, 160, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox22 = new(1028, 632, 40, 80, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox23 = new(1028, 712, 40, 40, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox24 = new(1028, 752, 160, 40, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox25 = new(1148, 792, 40, 80, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox26 = new(228, 762, 200, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox27 = new(548, 682, 200, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox28 = new(1068, 722, 120, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox29 = new(908, 602, 160, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox30 = new(868, -8, 40, 200, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox31 = new(908, 32, 120, 40, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox33 = new(1028, 32, 240, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox34 = new(748, 72, 120, 40, 0, 0, "collision dirt", Color.FromArgb(197, 146, 93), null, gameScreen);
        Terrain pictureBox35 = new(708, 72, 40, 80, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox36 = new(668, 112, 40, 40, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox37 = new(628, 112, 40, 120, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox38 = new(588, 192, 40, 40, 0, 0, "collision dirt", Color.FromArgb(197, 146, 93), null, gameScreen);
        Terrain pictureBox39 = new(548, 112, 40, 120, 0, 0, "collision dirt", Color.FromArgb(197, 146, 93), null, gameScreen);
        Terrain pictureBox40 = new(348, 32, 200, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox41 = new(548, 32, 40, 80, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox42 = new(268, 72, 120, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox43 = new(228, 32, 80, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox44 = new(188, -8, 40, 320, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox45 = new(228, 152, 80, 40, 0, 0, "collision metal", Color.FromArgb(103, 119, 135), null, gameScreen);
        Terrain pictureBox46 = new(348, 112, 40, 200, 0, 0, "collision metal", Color.FromArgb(93, 108, 124), null, gameScreen);
        Terrain pictureBox47 = new(228, 272, 120, 40, 0, 0, "collision metal", Color.FromArgb(103, 119, 135), null, gameScreen);
        Terrain pictureBox48 = new(68, 72, 120, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox49 = new(1308, 352, 80, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox50 = new(1268, 272, 40, 120, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox51 = new(1308, 272, 120, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox53 = new(1428, 112, 40, 200, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox52 = new(1468, -8, 40, 120, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox54 = new(1388, 352, 40, 120, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox55 = new(1428, 432, 40, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox56 = new(1468, 432, 40, 120, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox57 = new(1468, 552, 80, 40, 0, 0, "collision dirt", Color.FromArgb(197, 146, 93), null, gameScreen);
        Terrain pictureBox58 = new(1508, 592, 40, 120, 0, 0, "collision dirt", Color.FromArgb(197, 146, 93), null, gameScreen);
        Terrain pictureBox59 = new(1358, 392, 30, 80, 0, 0, "collision spikes", null, Resources.spike_right, gameScreen);
        Terrain pictureBox60 = new(1468, 112, 40, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox61 = new(788, 472, 80, 40, 0, 0, "collision metal", Color.FromArgb(103, 119, 135), null, gameScreen);
        Terrain pictureBox62 = new(828, 512, 40, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox63 = new(788, 552, 80, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox2 = new(1388, 162, 40, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain teleport1 = new(1268, -8, 200, 1, 0, 0, "level2", Color.FromArgb(21, 23, 45), null, gameScreen);
        Terrain pictureBox3 = new(468, 632, 40, 160, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox4 = new(548, 752, 120, 40, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox72 = new(668, 752, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox5 = new(788, 592, 40, 120, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox32 = new(908, 672, 120, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox64 = new(988, 712, 40, 80, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox65 = new(1068, 792, 40, 40, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox66 = new(1108, 792, 40, 80, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox67 = new(228, 832, 200, 40, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox68 = new(108, 712, 80, 40, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox69 = new(148, 752, 40, 40, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox70 = new(68, 712, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox71 = new(28, 152, 40, 200, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox73 = new(28, 112, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox74 = new(108, 32, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox75 = new(228, -8, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox76 = new(388, -8, 160, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox77 = new(308, 32, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox78 = new(588, 32, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox79 = new(588, 72, 40, 120, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox80 = new(668, 72, 40, 40, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox81 = new(748, 32, 120, 40, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox82 = new(908, -8, 120, 40, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox83 = new(1308, 312, 120, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox84 = new(1468, 152, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox85 = new(1468, 232, 40, 80, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox86 = new(1428, 392, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox87 = new(1508, 512, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox88 = new(-12, 332, 40, 80, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox89 = new(-12, 412, 40, 260, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox90 = new(1028, -8, 200, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox91 = new(1228, -8, 40, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);

        terrainArray = new Terrain[] { pictureBox91, pictureBox90, pictureBox89, pictureBox88, pictureBox87, pictureBox86, pictureBox85, pictureBox84, pictureBox83, pictureBox82, pictureBox81, pictureBox80, pictureBox79, pictureBox78, pictureBox77, pictureBox76, pictureBox75, pictureBox74, pictureBox73, pictureBox71, pictureBox70, pictureBox69, pictureBox68, pictureBox67, pictureBox66, pictureBox65, pictureBox64, pictureBox32, pictureBox5, pictureBox72, pictureBox4, pictureBox3, teleport1, pictureBox2, pictureBox63, pictureBox62, pictureBox61, pictureBox60, pictureBox59, pictureBox58, pictureBox57, pictureBox56, pictureBox55, pictureBox54, pictureBox52, pictureBox53, pictureBox51, pictureBox50, pictureBox49, pictureBox48, pictureBox47, pictureBox46, pictureBox45, pictureBox44, pictureBox43, pictureBox42, pictureBox41, pictureBox40, pictureBox39, pictureBox38, pictureBox37, pictureBox36, pictureBox35, pictureBox34, pictureBox33, pictureBox31, pictureBox30, pictureBox29, pictureBox28, pictureBox27, pictureBox26, pictureBox25, pictureBox24, pictureBox23, pictureBox22, pictureBox21, pictureBox19, pictureBox20, pictureBox18, pictureBox17, pictureBox16, pictureBox15, pictureBox14, pictureBox13, pictureBox12, pictureBox11, pictureBox10, pictureBox9, pictureBox8, pictureBox7, pictureBox6, pictureBox1 };
        strawberryArray = new Strawberry[] { };

        player.Left = 110;
        player.Top = 605;

        CameraFocus("Bottom");
    }

    private void Level2() {
        gameScreen.Height = 864;

        Terrain pictureBox6 = new(28, 392, 40, 480, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox16 = new(788, 512, 40, 120, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox17 = new(548, 672, 40, 200, 0, 0, "collision metal", Color.FromArgb(103, 119, 135), null, gameScreen);
        Terrain pictureBox20 = new(948, 512, 40, 80, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox19 = new(588, 672, 40, 200, 0, 0, "collision metal", Color.FromArgb(110, 127, 143), null, gameScreen);
        Terrain pictureBox21 = new(828, 512, 120, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox29 = new(788, 482, 200, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox52 = new(1428, -8, 40, 320, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox3 = new(68, 752, 160, 15, 0, 0, "collision wood", Color.FromArgb(161, 96, 67), null, gameScreen);
        Terrain pictureBox4 = new(228, 752, 40, 120, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox5 = new(268, 752, 200, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox7 = new(468, 752, 80, 40, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox8 = new(558, 652, 60, 20, 0, 0, "collision spring", Color.FromArgb(157, 88, 68), null, gameScreen);
        Terrain pictureBox1 = new(68, 232, 40, 200, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox2 = new(108, 232, 240, 40, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox9 = new(148, 152, 200, 40, 0, 0, "collision metal", Color.FromArgb(103, 119, 135), null, gameScreen);
        Terrain pictureBox11 = new(108, 112, 40, 120, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox12 = new(828, 592, 40, 280, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox13 = new(908, 592, 40, 200, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox14 = new(948, 752, 80, 40, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox15 = new(988, 792, 160, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox18 = new(1108, 672, 40, 120, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox22 = new(1148, 672, 40, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox23 = new(1188, 672, 60, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox25 = new(1288, 572, 180, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox26 = new(1428, 312, 40, 260, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox10 = new(108, 72, 160, 40, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox27 = new(228, 32, 160, 40, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox28 = new(348, 72, 40, 200, 0, 0, "collision metal", Color.FromArgb(93, 108, 124), null, gameScreen);
        Terrain pictureBox30 = new(388, 32, 40, 160, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox31 = new(428, 32, 160, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox32 = new(588, -8, 40, 240, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox33 = new(628, 32, 80, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox34 = new(668, 72, 160, 40, 0, 0, "collision stone", Color.FromArgb(162, 181, 191), null, gameScreen);
        Terrain pictureBox35 = new(788, 112, 40, 80, 0, 0, "collision stone", Color.FromArgb(162, 181, 191), null, gameScreen);
        Terrain pictureBox36 = new(788, 192, 80, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox37 = new(828, 232, 40, 80, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox38 = new(908, 232, 40, 80, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox39 = new(868, 272, 40, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox40 = new(948, 72, 40, 160, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox41 = new(988, 72, 120, 40, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox42 = new(1068, 112, 80, 40, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox43 = new(1148, 112, 40, 120, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox44 = new(1228, 192, 40, 160, 0, 0, "collision stone", Color.FromArgb(162, 181, 191), null, gameScreen);
        Terrain pictureBox45 = new(1188, 192, 40, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox46 = new(1268, -8, 40, 440, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain teleport1 = new(1308, -8, 120, 1, 0, 0, "level3", Color.FromArgb(21, 23, 45), null, gameScreen);
        Terrain pictureBox90 = new(-12, 432, 40, 440, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox47 = new(28, 272, 40, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox48 = new(628, -8, 40, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox49 = new(708, 32, 80, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox50 = new(1028, 32, 40, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox51 = new(1188, 152, 80, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox53 = new(1228, 32, 40, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox54 = new(1468, -8, 40, 80, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox55 = new(1468, 152, 40, 200, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox56 = new(1468, 352, 40, 220, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox57 = new(1288, 612, 140, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox59 = new(1148, 712, 40, 80, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox60 = new(1188, 712, 80, 40, 0, 0, "collision stone", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox62 = new(868, 672, 40, 120, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox63 = new(828, 552, 120, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox64 = new(868, 592, 40, 80, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox65 = new(308, 792, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox66 = new(388, 792, 40, 80, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox67 = new(268, 792, 40, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox68 = new(948, 722, 80, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox69 = new(1028, 762, 80, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox70 = new(68, 112, 40, 80, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox71 = new(468, -8, 120, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox72 = new(287, -8, 80, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox73 = new(828, 112, 40, 80, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox74 = new(908, 112, 40, 120, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox75 = new(868, 232, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox76 = new(1028, 832, 80, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox77 = new(948, 792, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox61 = new(868, 792, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox78 = new(1188, 112, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox79 = new(1228, -8, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox80 = new(28, 352, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox81 = new(-12, 672, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox58 = new(1248, 572, 40, 140, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Strawberry strawberry1 = new(479, 219, gameScreen);

        terrainArray = new Terrain[] { pictureBox58, pictureBox81, pictureBox80, pictureBox79, pictureBox78, pictureBox61, pictureBox77, pictureBox76, pictureBox75, pictureBox74, pictureBox73, pictureBox72, pictureBox71, pictureBox70, pictureBox69, pictureBox68, pictureBox67, pictureBox66, pictureBox65, pictureBox64, pictureBox63, pictureBox62, pictureBox60, pictureBox59, pictureBox57, pictureBox56, pictureBox55, pictureBox54, pictureBox53, pictureBox51, pictureBox50, pictureBox49, pictureBox48, pictureBox47, pictureBox90, teleport1, pictureBox46, pictureBox45, pictureBox44, pictureBox43, pictureBox42, pictureBox41, pictureBox40, pictureBox39, pictureBox38, pictureBox37, pictureBox36, pictureBox35, pictureBox34, pictureBox33, pictureBox32, pictureBox31, pictureBox30, pictureBox28, pictureBox27, pictureBox10, pictureBox26, pictureBox25, pictureBox23, pictureBox22, pictureBox18, pictureBox15, pictureBox14, pictureBox13, pictureBox12, pictureBox11, pictureBox9, pictureBox2, pictureBox1, pictureBox8, pictureBox7, pictureBox5, pictureBox4, pictureBox3, pictureBox52, pictureBox29, pictureBox21, pictureBox19, pictureBox20, pictureBox17, pictureBox16, pictureBox6 };
        strawberryArray = new Strawberry[] { strawberry1 };

        player.Left = 136;
        player.Top = 684;

        CameraFocus("Bottom");
    }

    private void Level3() {
        gameScreen.Height = 864;

        Terrain pictureBox16 = new(708, 592, 40, 120, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox20 = new(668, 592, 40, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox21 = new(668, 472, 40, 80, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox29 = new(1308, 722, 160, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox52 = new(1428, -8, 40, 280, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox3 = new(108, 792, 120, 15, 0, 0, "collision wood", Color.FromArgb(161, 96, 67), null, gameScreen);
        Terrain pictureBox4 = new(228, 792, 40, 80, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox1 = new(68, 112, 40, 120, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox12 = new(748, 512, 40, 240, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox13 = new(1108, 632, 40, 80, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox14 = new(1068, 632, 40, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox15 = new(1148, 672, 120, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox26 = new(1468, 232, 40, 240, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox34 = new(628, 72, 40, 80, 0, 0, "collision stone", Color.FromArgb(162, 181, 191), null, gameScreen);
        Terrain pictureBox41 = new(668, 72, 320, 40, 0, 0, "collision stone", Color.FromArgb(162, 181, 191), null, gameScreen);
        Terrain pictureBox46 = new(1228, -8, 40, 160, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox47 = new(68, 712, 40, 160, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox6 = new(28, 232, 40, 520, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox5 = new(268, 792, 40, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox48 = new(308, 792, 40, 80, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox2 = new(108, 112, 120, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox7 = new(228, 32, 40, 120, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox9 = new(268, 32, 360, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox10 = new(588, 72, 40, 80, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox11 = new(1468, -8, 40, 240, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox17 = new(1508, 432, 40, 280, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox19 = new(1468, 712, 80, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox25 = new(1308, 752, 200, 40, 0, 0, "collision stone", Color.FromArgb(162, 181, 191), null, gameScreen);
        Terrain pictureBox27 = new(1268, 672, 40, 120, 0, 0, "collision stone", Color.FromArgb(162, 181, 191), null, gameScreen);
        Terrain pictureBox18 = new(1028, 632, 40, 160, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox22 = new(948, 752, 80, 40, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox23 = new(908, 752, 40, 120, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox24 = new(1468, 682, 40, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox28 = new(908, 722, 120, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox30 = new(1028, 602, 120, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox31 = new(1148, 642, 160, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox32 = new(1188, 32, 40, 80, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox33 = new(628, 432, 40, 200, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox35 = new(668, 552, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox36 = new(708, 512, 40, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox8 = new(428, 352, 40, 120, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox37 = new(348, 272, 120, 80, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox38 = new(1028, 32, 160, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox39 = new(988, 32, 40, 160, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox40 = new(1228, 392, 40, 160, 0, 0, "collision ice", Color.FromArgb(79, 135, 235), null, gameScreen);
        Terrain pictureBox42 = new(1188, 312, 40, 200, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox43 = new(1268, 312, 40, 120, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox44 = new(1228, 312, 40, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox45 = new(1228, 352, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox49 = new(348, 312, 80, 40, 0, 0, "", Color.FromArgb(79, 135, 235), null, gameScreen);
        Terrain pictureBox58 = new(1508, 272, 40, 160, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox76 = new(308, -8, 240, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox50 = new(668, 32, 40, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox51 = new(748, 32, 200, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox53 = new(1028, -8, 200, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox54 = new(108, 72, 120, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox55 = new(28, 192, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox56 = new(-12, 272, 40, 400, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox57 = new(28, 752, 40, 120, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox59 = new(1068, 672, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox60 = new(908, 32, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox61 = new(788, 32, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox62 = new(468, -8, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox63 = new(335, -8, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox64 = new(1148, 712, 120, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox65 = new(1308, 792, 160, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox66 = new(948, 792, 80, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox67 = new(268, 832, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox68 = new(-12, 552, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox69 = new(1268, -8, 160, 1, 0, 0, "level4", Color.FromArgb(21, 23, 45), null, gameScreen);
        Strawberry strawberry1 = new(1364, 552, gameScreen);

        terrainArray = new Terrain[] { pictureBox69, pictureBox68, pictureBox67, pictureBox66, pictureBox65, pictureBox64, pictureBox63, pictureBox62, pictureBox61, pictureBox60, pictureBox59, pictureBox57, pictureBox56, pictureBox55, pictureBox54, pictureBox53, pictureBox51, pictureBox50, pictureBox76, pictureBox58, pictureBox49, pictureBox45, pictureBox44, pictureBox43, pictureBox42, pictureBox40, pictureBox39, pictureBox38, pictureBox37, pictureBox8, pictureBox36, pictureBox35, pictureBox33, pictureBox32, pictureBox31, pictureBox30, pictureBox28, pictureBox24, pictureBox23, pictureBox22, pictureBox18, pictureBox27, pictureBox25, pictureBox19, pictureBox17, pictureBox11, pictureBox10, pictureBox9, pictureBox7, pictureBox2, pictureBox48, pictureBox5, pictureBox6, pictureBox47, pictureBox46, pictureBox41, pictureBox34, pictureBox26, pictureBox15, pictureBox14, pictureBox13, pictureBox12, pictureBox1, pictureBox4, pictureBox3, pictureBox52, pictureBox29, pictureBox21, pictureBox20, pictureBox16 };
        strawberryArray = new Strawberry[] { strawberry1 };

        player.Left = 148;
        player.Top = 725;

        CameraFocus("Bottom");
    }

    private void Level4() {
        gameScreen.Height = 864;

        Terrain pictureBox52 = new(748, -8, 40, 280, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox3 = new(148, 752, 120, 15, 0, 0, "collision wood", Color.FromArgb(161, 96, 67), null, gameScreen);
        Terrain pictureBox26 = new(1468, 152, 40, 360, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox46 = new(948, -8, 40, 120, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox47 = new(108, 672, 40, 200, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox11 = new(1508, 192, 40, 280, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox17 = new(1508, 472, 40, 400, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox30 = new(108, 512, 30, 160, 0, 0, "collision spikes", null, Resources.spike_left, gameScreen);
        Terrain pictureBox33 = new(388, 352, 40, 280, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox8 = new(348, 352, 40, 40, 0, 0, "collision metal", Color.FromArgb(103, 119, 135), null, gameScreen);
        Terrain pictureBox4 = new(268, 752, 40, 120, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox5 = new(68, 472, 40, 240, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox6 = new(108, 192, 40, 320, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox1 = new(108, 152, 120, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox2 = new(188, 112, 80, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox7 = new(228, 72, 240, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox9 = new(708, 152, 40, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox10 = new(668, 32, 40, 160, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox32 = new(428, 32, 240, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox34 = new(988, 72, 80, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox38 = new(1028, 112, 80, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox39 = new(1068, 152, 40, 160, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox40 = new(358, 472, 30, 160, 0, 0, "collision spikes", null, Resources.spike_right, gameScreen);
        Terrain pictureBox37 = new(428, 552, 80, 40, 0, 0, "collision metal", Color.FromArgb(103, 119, 135), null, gameScreen);
        Terrain pictureBox41 = new(428, 522, 80, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox42 = new(348, 392, 40, 80, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox43 = new(428, 592, 40, 40, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox44 = new(428, 352, 80, 15, 0, 0, "collision wood jump-through", Color.FromArgb(161, 96, 67), null, gameScreen);
        Terrain pictureBox12 = new(1108, 232, 40, 240, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox16 = new(1148, 232, 40, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox20 = new(1188, 112, 40, 160, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox21 = new(1188, 72, 160, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox35 = new(1308, 32, 120, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox36 = new(1388, 72, 40, 120, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox45 = new(1428, 152, 40, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox13 = new(1428, 72, 40, 80, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox14 = new(68, 712, 40, 160, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox15 = new(28, 512, 40, 160, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox18 = new(68, 192, 40, 280, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox19 = new(148, 112, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox22 = new(268, 32, 160, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox23 = new(468, -8, 200, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox24 = new(708, -8, 40, 160, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox25 = new(988, -8, 40, 80, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox27 = new(1108, 152, 40, 80, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox28 = new(1148, 112, 40, 120, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox29 = new(1228, 32, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox31 = new(1348, -8, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox48 = new(308, 752, 80, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox49 = new(388, 752, 40, 120, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox50 = new(308, 792, 80, 80, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox51 = new(588, 392, 120, 80, 908, 352, "collision elevator metal", Color.FromArgb(192, 192, 192), null, gameScreen);
        Terrain pictureBox54 = new(1038, 152, 30, 160, 0, 0, "collision spikes", null, Resources.spike_right, gameScreen);
        Terrain pictureBox69 = new(788, -8, 160, 1, 0, 0, "level5", Color.FromArgb(21, 23, 45), null, gameScreen);

        terrainArray = new Terrain[] { pictureBox69, pictureBox54, pictureBox51, pictureBox50, pictureBox49, pictureBox48, pictureBox31, pictureBox29, pictureBox28, pictureBox27, pictureBox25, pictureBox24, pictureBox23, pictureBox22, pictureBox19, pictureBox18, pictureBox15, pictureBox14, pictureBox13, pictureBox45, pictureBox36, pictureBox35, pictureBox21, pictureBox20, pictureBox16, pictureBox12, pictureBox44, pictureBox43, pictureBox42, pictureBox41, pictureBox37, pictureBox40, pictureBox39, pictureBox38, pictureBox34, pictureBox32, pictureBox10, pictureBox9, pictureBox7, pictureBox2, pictureBox1, pictureBox6, pictureBox5, pictureBox4, pictureBox8, pictureBox33, pictureBox30, pictureBox17, pictureBox11, pictureBox47, pictureBox46, pictureBox26, pictureBox3, pictureBox52 };
        strawberryArray = new Strawberry[] { };

        player.Left = 194;
        player.Top = 685;

        CameraFocus("Bottom");

        spawnLocation = 0;
    }

    private void Level5() {
        gameScreen.Height = 1428;

        Terrain pictureBox113 = new(1085, 867, 100, 1, 0, 0, "collision spikes", Color.FromArgb(21, 23, 45), null, gameScreen);
        Terrain pictureBox16 = new(548, 272, 40, 280, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox20 = new(948, 272, 40, 120, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox21 = new(708, 472, 40, 120, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox3 = new(148, 1272, 120, 15, 0, 0, "collision wood", Color.FromArgb(161, 96, 67), null, gameScreen);
        Terrain pictureBox4 = new(68, 1272, 40, 160, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox1 = new(68, 112, 40, 80, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox12 = new(508, 232, 40, 80, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox6 = new(1068, 592, 40, 280, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox5 = new(-12, 1272, 80, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox48 = new(108, 1272, 40, 80, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox2 = new(-12, 152, 80, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox27 = new(468, 152, 40, 120, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox28 = new(788, 1122, 80, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox33 = new(908, 392, 40, 280, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox35 = new(588, 472, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox36 = new(908, 352, 40, 40, 0, 0, "", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox37 = new(708, 112, 40, 200, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox42 = new(748, 272, 200, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox49 = new(268, 32, 120, 40, 0, 0, "", Color.FromArgb(79, 135, 235), null, gameScreen);
        Terrain pictureBox47 = new(68, 1272, 80, 40, 0, 0, "", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox50 = new(-12, 1352, 80, 80, 0, 0, "", Color.FromArgb(40, 43, 46), null, gameScreen);
        Terrain pictureBox51 = new(808, 472, 100, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox53 = new(708, 432, 140, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox54 = new(428, 152, 40, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox55 = new(388, 72, 40, 120, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox56 = new(308, 72, 80, 40, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox13 = new(588, 512, 80, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox14 = new(1108, 712, 160, 40, 0, 0, "collision metal", Color.FromArgb(93, 108, 124), null, gameScreen);
        Terrain pictureBox15 = new(108, 32, 40, 120, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox17 = new(148, 32, 120, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox18 = new(228, 72, 80, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox7 = new(628, 552, 80, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox9 = new(748, 112, 80, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox10 = new(788, 72, 160, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox11 = new(908, 32, 80, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox19 = new(988, 32, 80, 40, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox22 = new(1068, 32, 40, 100, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox23 = new(1108, 72, 40, 60, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox24 = new(1148, 72, 160, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox25 = new(1308, 72, 80, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox26 = new(1348, 32, 200, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox29 = new(1388, 392, 160, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox30 = new(1348, 392, 40, 120, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox31 = new(1388, 472, 40, 40, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox32 = new(1428, 472, 40, 240, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox34 = new(1388, 672, 40, 120, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox38 = new(1228, 752, 160, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox39 = new(1108, 592, 40, 40, 0, 0, "collision metal", Color.FromArgb(103, 119, 135), null, gameScreen);
        Terrain pictureBox40 = new(1108, 832, 160, 40, 0, 0, "collision metal", Color.FromArgb(103, 119, 135), null, gameScreen);
        Terrain pictureBox41 = new(1228, 792, 40, 40, 0, 0, "", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox43 = new(1188, 872, 40, 280, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox44 = new(1188, 872, 40, 80, 0, 0, "", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox45 = new(1228, 1112, 40, 40, 0, 0, "collision stone", Color.FromArgb(142, 161, 171), null, gameScreen);
        Terrain pictureBox46 = new(1268, 1112, 40, 120, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox52 = new(1308, 1192, 80, 40, 0, 0, "collision stone", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox57 = new(1348, 1232, 40, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox58 = new(1388, 1232, 40, 120, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox59 = new(1428, 1312, 40, 120, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox60 = new(988, 1112, 200, 80, 988, 872, "collision elevator metal", Color.FromArgb(192, 192, 192), null, gameScreen);
        Terrain pictureBox62 = new(388, 1192, 160, 80, 708, 1192, "collision elevator metal", Color.FromArgb(192, 192, 192), null, gameScreen);
        Terrain pictureBox64 = new(788, 1152, 80, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox65 = new(868, 1152, 30, 40, 0, 0, "collision spikes", null, Resources.spike_left, gameScreen);
        Terrain pictureBox66 = new(758, 1152, 30, 40, 0, 0, "collision spikes", null, Resources.spike_right, gameScreen);
        Terrain pictureBox67 = new(468, 952, 40, 80, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox68 = new(438, 952, 30, 80, 0, 0, "collision spikes", null, Resources.spike_right, gameScreen);
        Terrain pictureBox69 = new(508, 952, 30, 80, 0, 0, "collision spikes", null, Resources.spike_left, gameScreen);
        Terrain pictureBox70 = new(468, 922, 40, 30, 0, 0, "collision spikes", null, Resources.spike_down, gameScreen);
        Terrain pictureBox71 = new(468, 1032, 40, 30, 0, 0, "collision spikes", null, Resources.spike_up, gameScreen);
        Terrain pictureBox72 = new(737, 392, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox73 = new(848, 432, 60, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox74 = new(748, 312, 200, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox75 = new(878, 352, 30, 80, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox76 = new(668, 152, 40, 120, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox77 = new(737, 72, 50, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox78 = new(828, 32, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox79 = new(668, 472, 40, 80, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox80 = new(588, 312, 40, 160, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox81 = new(948, -8, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox82 = new(1308, 32, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox83 = new(1348, -8, 200, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox84 = new(1468, 1352, 40, 80, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox85 = new(1428, 1272, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox86 = new(1228, 872, 40, 80, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox87 = new(1268, 792, 120, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox88 = new(148, -8, 120, 40, 0, 0, "", Color.FromArgb(79, 135, 235), null, gameScreen);
        Terrain pictureBox89 = new(1468, 632, 40, 40, 0, 0, "", Color.FromArgb(84, 94, 99), null, gameScreen);
        Terrain pictureBox90 = new(1468, 512, 40, 120, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox91 = new(1388, 432, 160, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox92 = new(1428, 712, 40, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox93 = new(1228, 952, 40, 160, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox94 = new(1308, 1152, 40, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox95 = new(1148, 32, 160, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox96 = new(-12, 112, 80, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox97 = new(69, 72, 40, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox98 = new(428, 112, 40, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox99 = new(508, 192, 40, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox100 = new(-12, 1312, 80, 40, 0, 0, "", Color.FromArgb(32, 36, 38), null, gameScreen);
        Terrain pictureBox8 = new(1148, 592, 40, 240, 0, 0, "collision metal", Color.FromArgb(103, 119, 135), null, gameScreen);
        Terrain pictureBox101 = new(588, 832, 80, 120, 0, 0, "collision falling ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox102 = new(287, 666, 80, 120, 0, 0, "collision falling ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox103 = new(68, 352, 40, 160, 0, 0, "collision metal", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox104 = new(28, 472, 40, 120, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox105 = new(28, 352, 40, 120, 0, 0, "collision metal", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox106 = new(-12, 552, 40, 80, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox107 = new(-12, 392, 40, 160, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox108 = new(-12, 352, 40, 40, 0, 0, "collision metal", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox109 = new(28, 352, 80, 40, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox110 = new(108, 472, 80, 15, 0, 0, "collision wood", Color.FromArgb(161, 96, 67), null, gameScreen);
        Terrain pictureBox111 = new(1548, 72, 1, 320, 0, 0, "level6", Color.FromArgb(21, 23, 45), null, gameScreen);
        Terrain pictureBox112 = new(-12, 192, 1, 160, 0, 0, "level9", Color.FromArgb(21, 23, 45), null, gameScreen);

        terrainArray = new Terrain[] { pictureBox113, pictureBox112, pictureBox111, pictureBox110, pictureBox109, pictureBox108, pictureBox107, pictureBox106, pictureBox105, pictureBox104, pictureBox103, pictureBox102, pictureBox101, pictureBox8, pictureBox100, pictureBox99, pictureBox98, pictureBox97, pictureBox96, pictureBox95, pictureBox94, pictureBox93, pictureBox92, pictureBox91, pictureBox90, pictureBox89, pictureBox88, pictureBox87, pictureBox86, pictureBox85, pictureBox84, pictureBox83, pictureBox82, pictureBox81, pictureBox80, pictureBox79, pictureBox78, pictureBox77, pictureBox76, pictureBox75, pictureBox74, pictureBox73, pictureBox72, pictureBox71, pictureBox70, pictureBox69, pictureBox68, pictureBox67, pictureBox66, pictureBox65, pictureBox64, pictureBox62, pictureBox60, pictureBox59, pictureBox58, pictureBox57, pictureBox52, pictureBox46, pictureBox45, pictureBox44, pictureBox43, pictureBox41, pictureBox40, pictureBox39, pictureBox38, pictureBox34, pictureBox32, pictureBox31, pictureBox30, pictureBox29, pictureBox26, pictureBox25, pictureBox24, pictureBox23, pictureBox22, pictureBox19, pictureBox11, pictureBox10, pictureBox9, pictureBox7, pictureBox18, pictureBox17, pictureBox15, pictureBox14, pictureBox13, pictureBox56, pictureBox55, pictureBox54, pictureBox53, pictureBox51, pictureBox50, pictureBox47, pictureBox49, pictureBox42, pictureBox37, pictureBox36, pictureBox35, pictureBox33, pictureBox28, pictureBox27, pictureBox2, pictureBox48, pictureBox5, pictureBox6, pictureBox12, pictureBox1, pictureBox4, pictureBox3, pictureBox21, pictureBox20, pictureBox16 };
        strawberryArray = new Strawberry[] { };

        switch (spawnLocation) {
            case 0:
                player.Left = 58;
                player.Top = 1205;
                CameraFocus("Bottom");
                break;

            case 1:
                player.Left = 28;
                player.Top = 285;
                CameraFocus("Top");
                break;

            case 2:
                player.Left = 1457;
                player.Top = 325;
                CameraFocus("Top");
                break;
        }
    }

    private void Level6() {
        gameScreen.Height = 864;

        Terrain endArea = new(587, -8, 800, 720, 0, 0, "end", Color.FromArgb(21, 23, 45), null, gameScreen);
        Terrain pictureBox48 = new(-12, 712, 120, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox1 = new(68, 752, 520, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox2 = new(548, 712, 80, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox3 = new(588, 672, 80, 40, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox4 = new(628, 592, 40, 80, 0, 0, "collision ice", Color.FromArgb(90, 149, 255), null, gameScreen);
        Terrain pictureBox5 = new(668, 592, 200, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox6 = new(828, 632, 160, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox7 = new(948, 672, 120, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox8 = new(1028, 712, 80, 40, 0, 0, "collision ice", Color.FromArgb(110, 162, 255), null, gameScreen);
        Terrain pictureBox23 = new(1108, 712, 240, 40, 0, 0, "collision dirt", Color.FromArgb(217, 160, 102), null, gameScreen);
        Terrain pictureBox32 = new(668, 632, 160, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox64 = new(1068, 752, 320, 40, 0, 0, "", Color.FromArgb(63, 45, 27), null, gameScreen);
        Terrain pictureBox9 = new(1348, 712, 200, 40, 0, 0, "collision ice", Color.FromArgb(99, 155, 255), null, gameScreen);
        Terrain pictureBox10 = new(868, 672, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox11 = new(988, 712, 40, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox12 = new(1388, 752, 160, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox13 = new(108, 792, 120, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox14 = new(-12, 752, 80, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox15 = new(668, 392, 160, 200, 0, 0, "", Color.FromArgb(150, 168, 178), null, gameScreen);
        Terrain pictureBox16 = new(688, 362, 120, 30, 0, 0, "", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox17 = new(698, 445, 100, 10, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox18 = new(721, 414, 10, 10, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox19 = new(762, 414, 10, 10, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox20 = new(737, 409, 20, 20, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox21 = new(742, 414, 10, 10, 0, 0, "", Color.FromArgb(169, 169, 169), null, gameScreen);
        Terrain pictureBox22 = new(698, 490, 100, 10, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox24 = new(709, 460, 10, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox25 = new(694, 470, 20, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox26 = new(699, 480, 40, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox27 = new(745, 480, 10, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox28 = new(761, 480, 10, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox29 = new(777, 480, 20, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox30 = new(720, 470, 40, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox31 = new(766, 470, 10, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox33 = new(782, 470, 20, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox34 = new(725, 460, 20, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox35 = new(751, 460, 20, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox36 = new(776, 460, 10, 5, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox38 = new(737, 514, 20, 20, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox37 = new(742, 519, 10, 10, 0, 0, "", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox39 = new(742, 540, 10, 10, 0, 0, "", Color.FromArgb(112, 128, 144), null, gameScreen);
        Terrain pictureBox40 = new(698, 332, 10, 30, 0, 0, "", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox41 = new(782, 332, 10, 30, 0, 0, "", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox42 = new(742, 282, 10, 80, 0, 0, "", Color.FromArgb(155, 173, 183), null, gameScreen);
        Terrain pictureBox43 = new(737, 289, 20, 60, 0, 0, "", Color.FromArgb(145, 163, 173), null, gameScreen);
        Terrain pictureBox44 = new(658, 572, 180, 20, 0, 0, "", Color.FromArgb(135, 153, 163), null, gameScreen);
        Terrain pictureBox45 = new(228, 792, 200, 40, 0, 0, "", Color.FromArgb(89, 145, 245), null, gameScreen);
        Terrain pictureBox46 = new(428, 792, 120, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox47 = new(202, 832, 250, 40, 0, 0, "", Color.FromArgb(69, 125, 225), null, gameScreen);
        Terrain pictureBox111 = new(-12, -8, 1, 720, 0, 0, "level5", Color.FromArgb(21, 23, 45), null, gameScreen);

        terrainArray = new Terrain[] { pictureBox111, pictureBox47, pictureBox46, pictureBox45, pictureBox44, pictureBox43, pictureBox42, pictureBox41, pictureBox40, pictureBox39, pictureBox37, pictureBox38, pictureBox36, pictureBox35, pictureBox34, pictureBox33, pictureBox31, pictureBox30, pictureBox29, pictureBox28, pictureBox27, pictureBox26, pictureBox25, pictureBox24, pictureBox22, pictureBox21, pictureBox20, pictureBox19, pictureBox18, pictureBox17, pictureBox16, pictureBox15, pictureBox14, pictureBox13, pictureBox12, pictureBox11, pictureBox10, pictureBox9, pictureBox64, pictureBox32, pictureBox23, pictureBox8, pictureBox7, pictureBox6, pictureBox5, pictureBox4, pictureBox3, pictureBox2, pictureBox1, pictureBox48, endArea };
        strawberryArray = new Strawberry[] { };

        player.Left = 42;
        player.Top = 645;

        CameraFocus("Bottom");

        spawnLocation = 2;
    }

    private void Level9() {

    }

    private void SpawnLevel(int level) {
        // Destroy old level
        foreach (Terrain terrain in terrainArray)
            DestroyAll(terrain.pb, gameScreen);

        foreach (Strawberry strawberry in strawberryArray) {
            strawberry.Dispose();
            DestroyAll(strawberry.pb, gameScreen);
        }

        force = 0;
        movementSpeed = 0;

        // Spawn new level
        switch (level) {
            case 1: Level1(); break;
            case 2: Level2(); break;
            case 3: Level3(); break;
            case 4: Level4(); break;
            case 5: Level5(); break;
            case 6: Level6(); break;
            case 9: Level9(); break;
        }

        player.BringToFront();
        lbDeveloperStats.BringToFront();

        currentLevel = level;
    }

    private void DestroyAll(PictureBox pb, Panel panel) {
        pb.Bounds = Rectangle.Empty;
        pb.Parent?.Controls.Remove(pb);
        pb.Dispose();
    }

    #endregion Level design

    #region Sound design

    string soundMaterial = "";

    SoundManager sndDash = new("dash");

    SoundManager sndDeath = new("death");

    SoundManager sndFallingBlockShake = new("fallingblock_ice_shake");

    SoundManager sndGrabDirt = new("grab_dirt");
    SoundManager sndGrabIce = new("grab_ice");
    SoundManager sndGrabMetal = new("grab_metal");
    SoundManager sndGrabWood = new("grab_wood");

    SoundManager sndGrabLetgo = new("grab_letgo");

    SoundManager sndJump = new("jump");

    SoundManager sndJumpWall = new("jump_wall");

    SoundManager sndLandDirt = new("land_dirt");
    SoundManager sndLandIce = new("land_ice");
    SoundManager sndLandMetal = new("land_metal");
    SoundManager sndLandWood = new("land_wood");

    SoundManager sndSpring = new("spring");

    SoundManager sndStrawberryGet = new("strawberry_red_get_1000");
    SoundManager sndStrawberryTouch = new("strawberry_touch");

    SoundManager sndZipmoverTouch = new("zipmover_touch");
    SoundManager sndZipmoverImpact = new("zipmover_impact");
    SoundManager sndZipmoverReturn = new("zipmover_return");
    SoundManager sndZipmoverReset = new("zipmover_reset");

    public void PlaySound(SoundTypes sound) {
        if (soundMaterial.Contains("dirt") || soundMaterial.Contains("stone"))
            soundMaterial = "dirt";
        else if (soundMaterial.Contains("ice"))
            soundMaterial = "ice";
        else if (soundMaterial.Contains("metal"))
            soundMaterial = "metal";
        else if (soundMaterial.Contains("wood"))
            soundMaterial = "wood";
        else
            soundMaterial = "dirt";

        switch (sound) {
            case SoundTypes.Dash:
                sndDash.PlaySound();
                break;

            case SoundTypes.Death:
                sndDeath.PlaySound();
                break;

            case SoundTypes.FallingblockShake:
                sndFallingBlockShake.PlaySound();
                break;

            case SoundTypes.Grab:
                switch (soundMaterial) {
                    case "dirt":
                        sndGrabDirt.PlaySound();
                        break;
                    case "ice":
                        sndGrabIce.PlaySound();
                        break;
                    case "metal":
                        sndGrabMetal.PlaySound();
                        break;
                    case "wood":
                        sndGrabWood.PlaySound();
                        break;
                }
                break;

            case SoundTypes.GrabLetgo:
                sndGrabLetgo.PlaySound();
                break;

            case SoundTypes.Jump:
                sndJump.PlaySound();
                break;

            case SoundTypes.JumpWall:
                sndJumpWall.PlaySound();
                break;

            case SoundTypes.Land:
                switch (soundMaterial) {
                    case "dirt":
                        sndLandDirt.PlaySound();
                        break;
                    case "ice":
                        sndLandIce.PlaySound();
                        break;
                    case "metal":
                        sndLandMetal.PlaySound();
                        break;
                    case "wood":
                        sndLandWood.PlaySound();
                        break;
                }
                break;

            case SoundTypes.Spring:
                sndSpring.PlaySound();
                break;

            case SoundTypes.StrawberryGet:
                sndStrawberryGet.PlaySound();
                break;

            case SoundTypes.StrawberryTouch:
                sndStrawberryTouch.PlaySound();
                break;

            case SoundTypes.ZipmoverTouch:
                sndZipmoverTouch.PlaySound();
                break;

            case SoundTypes.ZipmoverImpact:
                sndZipmoverImpact.PlaySound();
                break;

            case SoundTypes.ZipmoverReturn:
                sndZipmoverReturn.StopSound();
                sndZipmoverReturn.PlaySound();
                break;

            case SoundTypes.ZipmoverReset:
                sndZipmoverReturn.StopSound();
                sndZipmoverReset.PlaySound();
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
        "Zapnuto\tOn",
        "Vypnuto\tOff"
    };

    List<string> fontSizeList = new List<string>()
    {
        "0,8x\t0.8x",
        "1x\t1x"
    };

    private void SettingsUpdate() {
        // Start menu
        menuMainBtPlay.Text = Resources.strPlay.Split(';')[langIndex].ToUpper();
        menuMainBtSettings.Text = Resources.strSettings.Split(';')[langIndex].ToUpper();
        menuMainBtControls.Text = Resources.strControls.Split(';')[langIndex].ToUpper();
        menuMainBtClose.Text = Resources.strClose.Split(';')[langIndex].ToUpper();
        mainLbInfo.Text = Resources.strProjectInfo.Split(';')[langIndex];

        // Settings
        menuSettingsLbTitle.Text = Resources.strSettings.Split(';')[langIndex].ToUpper();
        menuSettingsLbL1.Text = Resources.strVolume.Split(';')[langIndex];

        /// Language
        menuSettingsLbL2.Text = Resources.strLanguage.Split(';')[langIndex];

        menuSettingsLbR2Language.Text = languages[langIndex];

        foreach (Control text in menuSettingsLbR2Container.Controls)
            text.ForeColor = Color.FromArgb(68, 101, 147);

        if (langIndex <= 0)
            menuSettingsLbR2ControlL.ForeColor = Color.FromArgb(130, 160, 200);

        if (langIndex >= languages.Count() - 1)
            menuSettingsLbR2ControlR.ForeColor = Color.FromArgb(130, 160, 200);

        ConfigFileUpdate(2, menuSettingsLbR2Language.Text);


        /// Sound selection
        menuSettingsLbL3.Text = Resources.strSounds.Split(';')[langIndex];

        menuSettingsLbR3Input.Text = soundOutputTypeList[soundOutputType].Split('\t')[langIndex];

        foreach (Control text in menuSettingsLbR3Container.Controls)
            text.ForeColor = Color.FromArgb(68, 101, 147);

        switch (soundOutputType) {
            case 0:
                menuSettingsLbR3ControlL.ForeColor = Color.FromArgb(130, 160, 200);
                SoundManager.bannedSound = false;

                ConfigFileUpdate(3, "On");
                break;

            case 1:
                SoundManager.bannedSound = true;
                menuSettingsLbR3ControlR.ForeColor = Color.FromArgb(130, 160, 200);

                ConfigFileUpdate(3, "Off");
                break;
        }


        /// Font scale selection
        menuSettingsLbL4.Text = Resources.strTextSize.Split(';')[langIndex];

        menuSettingsLbR4FontSize.Text = fontSizeList[textScaleIndex].Split('\t')[langIndex];

        foreach (Control text in menuSettingsLbR4Container.Controls)
            text.ForeColor = Color.FromArgb(68, 101, 147);

        if (textScaleIndex <= 0)
            menuSettingsLbR4ControlL.ForeColor = Color.FromArgb(130, 160, 200);

        if (textScaleIndex >= fontSizeList.Count() - 1)
            menuSettingsLbR4ControlR.ForeColor = Color.FromArgb(130, 160, 200);


        menuSettingsBtBack.Text = Resources.strBack.Split(';')[langIndex].ToUpper();

        // Controlls
        menuControlsLbTitle.Text = Resources.strControls.Split(';')[langIndex].ToUpper();
        menuControlsLbL1.Text = Resources.strLeftRight.Split(';')[langIndex];
        menuControlsLbL2.Text = Resources.strUpDown.Split(';')[langIndex];
        menuControlsLbL3.Text = Resources.strJump.Split(';')[langIndex];
        menuControlsLbL4.Text = Resources.strDash.Split(';')[langIndex];
        menuControlsLbL5.Text = Resources.strGrab.Split(';')[langIndex];
        menuControlsLbL6.Text = Resources.strPause.Split(';')[langIndex];
        lbKeyboard1.Text = Resources.strAD.Split(';')[langIndex];
        lbKeyboard2.Text = Resources.strWS.Split(';')[langIndex];
        lbKeyboard3.Text = Resources.strSpace.Split(';')[langIndex];
        lbKeyboard4.Text = Resources.strAlt.Split(';')[langIndex];
        lbKeyboard5.Text = Resources.strShift.Split(';')[langIndex];
        lbKeyboard6.Text = Resources.strEsc.Split(';')[langIndex];
        menuControlsBtBack.Text = Resources.strBack.Split(';')[langIndex].ToUpper();

        // Escape menu
        menuEscapeLbTitle.Text = Resources.strPaused.Split(';')[langIndex].ToUpper();
        menuEscapeBtContinue.Text = Resources.strContinue.Split(';')[langIndex].ToUpper();
        menuEscapeBtScreenReset.Text = Resources.strScreenReset.Split(';')[langIndex].ToUpper();
        menuEscapeBtSettings.Text = Resources.strSettings.Split(';')[langIndex].ToUpper();
        menuEscapeBtControls.Text = Resources.strControls.Split(';')[langIndex].ToUpper();
        menuEscapeBtMainMenu.Text = Resources.strMainMenu.Split(';')[langIndex].ToUpper();

        // End
        lbEndContinue.Text = Resources.strEndContinue.Split(';')[langIndex];
    }

    private void AdjustFontSize(int index) {
        Label[] titles = new Label[] { menuSettingsLbTitle, menuControlsLbTitle, menuEscapeLbTitle };
        Label[] menuText = new Label[] { menuSettingsLbL1, menuSettingsLbL2, menuSettingsLbL3, menuSettingsLbL4, menuSettingsLbVolumeR1, menuSettingsLbR2ControlL, menuSettingsLbR2Language, menuSettingsLbR2ControlR, menuSettingsLbR3ControlL, menuSettingsLbR3Input, menuSettingsLbR3ControlR, menuSettingsLbR4ControlL, menuSettingsLbR4FontSize, menuSettingsLbR4ControlR, menuControlsLbL1, menuControlsLbL2, menuControlsLbL3, menuControlsLbL4, menuControlsLbL5, menuControlsLbL6, lbKeyboard1, lbKeyboard2, lbKeyboard3, lbKeyboard4, lbKeyboard5, lbKeyboard6 };
        Label[] text = new Label[] { menuMainLbAuthor, lbEndContinue };
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

    private void ConfigFileUpdate(int record, string data) {
        string[] config = new string[0];

        try {
            config = File.ReadAllLines("config.txt");
        } catch (Exception) {
            DialogResult dialogResult = MessageBox.Show("Nebylo možné uložit zmìny", "Chyba", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            if (dialogResult == DialogResult.Retry) {
                ConfigFileUpdate(record, data);
            } else {
                ConfigFileUpdate(0, "load");
            }
        }

        // 1 - Volume change
        // 2 - Language change
        // 3 - Sound type (music & sounds, sounds, off)
        // 4 - Text scale


        if (record != 0) {
            config[record] = data;

            try {
                File.WriteAllLines("config.txt", config);
            } catch (Exception) {
                DialogResult dialogResult = MessageBox.Show("Nebylo možné uložit zmìny", "Chyba", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.Retry) {
                    ConfigFileUpdate(record, data);
                } else {
                    ConfigFileUpdate(0, "load");
                }
            }
        } else if (data == "load") {
            AdjustFontSize(0);

            // Load volume
            SoundManager.volume = (float)(Convert.ToInt32(config[1]) * 0.01);
            menuSettingsLbVolumeR1.Text = Math.Floor(SoundManager.volume * 100).ToString();
            menuSettingsTrackR1.Value = (int)(SoundManager.volume * 20);

            // Load language
            switch (config[2]) {
                case "Èesky":
                    langIndex = 0;
                    break;

                case "English":
                    langIndex = 1;
                    break;
            }

            // Load output type
            switch (config[3]) {
                case "On":
                    soundOutputType = 0;
                    break;

                case "Off":
                    soundOutputType = 1;
                    break;
            }

            // Load text scale
            textScaleIndex = Convert.ToInt32(config[4]);
            AdjustFontSize(textScaleIndex);
        }
    }

    // Protect timers from pause (if paused, timers will reset on continue)
    private void TimerHandler(string action) {
        switch (action) {
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