namespace Celeste_WinForms
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.gameScreen = new System.Windows.Forms.Panel();
            this.player = new System.Windows.Forms.PictureBox();
            this.blockBorderLeft = new System.Windows.Forms.PictureBox();
            this.lbDeveloperStats = new System.Windows.Forms.Label();
            this.lbDeveloperSounds = new System.Windows.Forms.Label();
            this.menuStartContainer = new System.Windows.Forms.Panel();
            this.startTableLP = new System.Windows.Forms.TableLayoutPanel();
            this.startLbTitle = new System.Windows.Forms.Label();
            this.startLbSubtitle = new System.Windows.Forms.Label();
            this.startBtPlay = new System.Windows.Forms.Button();
            this.startBtSettings = new System.Windows.Forms.Button();
            this.startBtClose = new System.Windows.Forms.Button();
            this.menuEscapeContainer = new System.Windows.Forms.Panel();
            this.menuEscapeTableLP = new System.Windows.Forms.TableLayoutPanel();
            this.menuEscapeLbPause = new System.Windows.Forms.Label();
            this.menuEscapeBtContinue = new System.Windows.Forms.Button();
            this.menuEscapeBtResetScreen = new System.Windows.Forms.Button();
            this.menuEscapeBtControls = new System.Windows.Forms.Button();
            this.menuEscapeBtStartMenu = new System.Windows.Forms.Button();
            this.menuControlsContainer = new System.Windows.Forms.Panel();
            this.menuControlsTableLP = new System.Windows.Forms.TableLayoutPanel();
            this.menuControlsLbTitle = new System.Windows.Forms.Label();
            this.menuControlsBtEscapeMenu = new System.Windows.Forms.Button();
            this.menuControlsTableLPControls = new System.Windows.Forms.TableLayoutPanel();
            this.menuControlsLbL1 = new System.Windows.Forms.Label();
            this.menuControlsLbL2 = new System.Windows.Forms.Label();
            this.menuControlsLbL3 = new System.Windows.Forms.Label();
            this.menuControlsLbL4 = new System.Windows.Forms.Label();
            this.menuControlsLbL5 = new System.Windows.Forms.Label();
            this.menuControlsLbL6 = new System.Windows.Forms.Label();
            this.menuControlsLbL7 = new System.Windows.Forms.Label();
            this.menuControlsLbR1 = new System.Windows.Forms.Label();
            this.menuControlsLbR2 = new System.Windows.Forms.Label();
            this.menuControlsLbR3 = new System.Windows.Forms.Label();
            this.menuControlsLbR4 = new System.Windows.Forms.Label();
            this.menuControlsLbR5 = new System.Windows.Forms.Label();
            this.menuControlsLbR6 = new System.Windows.Forms.Label();
            this.menuControlsLbR7 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timerJumpHeadBumpCooldown = new System.Windows.Forms.Timer(this.components);
            this.timerGrabAfterJumpCooldown = new System.Windows.Forms.Timer(this.components);
            this.timerJumpCooldown = new System.Windows.Forms.Timer(this.components);
            this.timerDashedNonVertical = new System.Windows.Forms.Timer(this.components);
            this.gameScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.player)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockBorderLeft)).BeginInit();
            this.menuStartContainer.SuspendLayout();
            this.startTableLP.SuspendLayout();
            this.menuEscapeContainer.SuspendLayout();
            this.menuEscapeTableLP.SuspendLayout();
            this.menuControlsContainer.SuspendLayout();
            this.menuControlsTableLP.SuspendLayout();
            this.menuControlsTableLPControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameScreen
            // 
            this.gameScreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(137)))), ((int)(((byte)(158)))));
            this.gameScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gameScreen.Controls.Add(this.player);
            this.gameScreen.Controls.Add(this.blockBorderLeft);
            this.gameScreen.Enabled = false;
            this.gameScreen.Location = new System.Drawing.Point(0, 0);
            this.gameScreen.Margin = new System.Windows.Forms.Padding(2);
            this.gameScreen.Name = "gameScreen";
            this.gameScreen.Size = new System.Drawing.Size(1536, 864);
            this.gameScreen.TabIndex = 0;
            this.gameScreen.Visible = false;
            this.gameScreen.Paint += new System.Windows.Forms.PaintEventHandler(this.gameScreen_Paint);
            // 
            // player
            // 
            this.player.BackColor = System.Drawing.Color.Transparent;
            this.player.BackgroundImage = global::Celeste_WinForms.Properties.Resources.mario_;
            this.player.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.player.Location = new System.Drawing.Point(9, 788);
            this.player.Margin = new System.Windows.Forms.Padding(0);
            this.player.Name = "player";
            this.player.Size = new System.Drawing.Size(51, 67);
            this.player.TabIndex = 0;
            this.player.TabStop = false;
            // 
            // blockBorderLeft
            // 
            this.blockBorderLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.blockBorderLeft.Location = new System.Drawing.Point(-19, 0);
            this.blockBorderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.blockBorderLeft.Name = "blockBorderLeft";
            this.blockBorderLeft.Size = new System.Drawing.Size(10, 864);
            this.blockBorderLeft.TabIndex = 7;
            this.blockBorderLeft.TabStop = false;
            this.blockBorderLeft.Tag = "object collision";
            // 
            // lbDeveloperStats
            // 
            this.lbDeveloperStats.AutoSize = true;
            this.lbDeveloperStats.BackColor = System.Drawing.Color.Black;
            this.lbDeveloperStats.Font = new System.Drawing.Font("Cascadia Code", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbDeveloperStats.ForeColor = System.Drawing.Color.Yellow;
            this.lbDeveloperStats.Location = new System.Drawing.Point(15, 10);
            this.lbDeveloperStats.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbDeveloperStats.Name = "lbDeveloperStats";
            this.lbDeveloperStats.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.lbDeveloperStats.Size = new System.Drawing.Size(102, 32);
            this.lbDeveloperStats.TabIndex = 0;
            this.lbDeveloperStats.Text = "F3 STATS";
            // 
            // lbDeveloperSounds
            // 
            this.lbDeveloperSounds.AutoSize = true;
            this.lbDeveloperSounds.BackColor = System.Drawing.Color.Black;
            this.lbDeveloperSounds.Font = new System.Drawing.Font("Cascadia Code", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbDeveloperSounds.ForeColor = System.Drawing.Color.Yellow;
            this.lbDeveloperSounds.Location = new System.Drawing.Point(1100, 10);
            this.lbDeveloperSounds.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbDeveloperSounds.Name = "lbDeveloperSounds";
            this.lbDeveloperSounds.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.lbDeveloperSounds.Size = new System.Drawing.Size(113, 32);
            this.lbDeveloperSounds.TabIndex = 8;
            this.lbDeveloperSounds.Text = "F3 SOUNDS";
            // 
            // menuStartContainer
            // 
            this.menuStartContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(137)))), ((int)(((byte)(158)))));
            this.menuStartContainer.Controls.Add(this.startTableLP);
            this.menuStartContainer.Enabled = false;
            this.menuStartContainer.Location = new System.Drawing.Point(0, 0);
            this.menuStartContainer.Name = "menuStartContainer";
            this.menuStartContainer.Size = new System.Drawing.Size(1536, 864);
            this.menuStartContainer.TabIndex = 8;
            this.menuStartContainer.Visible = false;
            // 
            // startTableLP
            // 
            this.startTableLP.ColumnCount = 1;
            this.startTableLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.startTableLP.Controls.Add(this.startLbTitle, 0, 0);
            this.startTableLP.Controls.Add(this.startLbSubtitle, 0, 1);
            this.startTableLP.Controls.Add(this.startBtPlay, 0, 3);
            this.startTableLP.Controls.Add(this.startBtSettings, 0, 4);
            this.startTableLP.Controls.Add(this.startBtClose, 0, 5);
            this.startTableLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startTableLP.Location = new System.Drawing.Point(0, 0);
            this.startTableLP.Name = "startTableLP";
            this.startTableLP.RowCount = 7;
            this.startTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 236F));
            this.startTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.startTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 231F));
            this.startTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.startTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.startTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.startTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.startTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.startTableLP.Size = new System.Drawing.Size(1536, 864);
            this.startTableLP.TabIndex = 0;
            // 
            // startLbTitle
            // 
            this.startLbTitle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.startLbTitle.AutoSize = true;
            this.startLbTitle.Font = new System.Drawing.Font("Segoe UI", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.startLbTitle.ForeColor = System.Drawing.Color.White;
            this.startLbTitle.Location = new System.Drawing.Point(448, 45);
            this.startLbTitle.Name = "startLbTitle";
            this.startLbTitle.Size = new System.Drawing.Size(640, 191);
            this.startLbTitle.TabIndex = 0;
            this.startLbTitle.Text = "CELESTE";
            // 
            // startLbSubtitle
            // 
            this.startLbSubtitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.startLbSubtitle.AutoSize = true;
            this.startLbSubtitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.startLbSubtitle.ForeColor = System.Drawing.Color.White;
            this.startLbSubtitle.Location = new System.Drawing.Point(471, 236);
            this.startLbSubtitle.Name = "startLbSubtitle";
            this.startLbSubtitle.Size = new System.Drawing.Size(594, 65);
            this.startLbSubtitle.TabIndex = 4;
            this.startLbSubtitle.Text = "Fan-made Forms Remake";
            // 
            // startBtPlay
            // 
            this.startBtPlay.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.startBtPlay.BackColor = System.Drawing.Color.White;
            this.startBtPlay.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.startBtPlay.Location = new System.Drawing.Point(543, 535);
            this.startBtPlay.Name = "startBtPlay";
            this.startBtPlay.Size = new System.Drawing.Size(450, 70);
            this.startBtPlay.TabIndex = 1;
            this.startBtPlay.Text = "HRÁT";
            this.startBtPlay.UseVisualStyleBackColor = false;
            this.startBtPlay.Click += new System.EventHandler(this.menuStartBtPlay_Click);
            // 
            // startBtSettings
            // 
            this.startBtSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.startBtSettings.BackColor = System.Drawing.Color.White;
            this.startBtSettings.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.startBtSettings.Location = new System.Drawing.Point(543, 611);
            this.startBtSettings.Name = "startBtSettings";
            this.startBtSettings.Size = new System.Drawing.Size(450, 70);
            this.startBtSettings.TabIndex = 2;
            this.startBtSettings.Text = "NASTAVENÍ";
            this.startBtSettings.UseVisualStyleBackColor = false;
            // 
            // startBtClose
            // 
            this.startBtClose.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.startBtClose.BackColor = System.Drawing.Color.White;
            this.startBtClose.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.startBtClose.Location = new System.Drawing.Point(543, 687);
            this.startBtClose.Name = "startBtClose";
            this.startBtClose.Size = new System.Drawing.Size(450, 70);
            this.startBtClose.TabIndex = 3;
            this.startBtClose.Text = "ZAVŘÍT";
            this.startBtClose.UseVisualStyleBackColor = false;
            this.startBtClose.Click += new System.EventHandler(this.menuStartBtClose_Click);
            // 
            // menuEscapeContainer
            // 
            this.menuEscapeContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(137)))), ((int)(((byte)(158)))));
            this.menuEscapeContainer.Controls.Add(this.menuEscapeTableLP);
            this.menuEscapeContainer.Enabled = false;
            this.menuEscapeContainer.Location = new System.Drawing.Point(0, 0);
            this.menuEscapeContainer.Name = "menuEscapeContainer";
            this.menuEscapeContainer.Size = new System.Drawing.Size(1536, 864);
            this.menuEscapeContainer.TabIndex = 9;
            this.menuEscapeContainer.Visible = false;
            // 
            // menuEscapeTableLP
            // 
            this.menuEscapeTableLP.ColumnCount = 1;
            this.menuEscapeTableLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.menuEscapeTableLP.Controls.Add(this.menuEscapeLbPause, 0, 0);
            this.menuEscapeTableLP.Controls.Add(this.menuEscapeBtContinue, 0, 2);
            this.menuEscapeTableLP.Controls.Add(this.menuEscapeBtResetScreen, 0, 3);
            this.menuEscapeTableLP.Controls.Add(this.menuEscapeBtControls, 0, 4);
            this.menuEscapeTableLP.Controls.Add(this.menuEscapeBtStartMenu, 0, 5);
            this.menuEscapeTableLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuEscapeTableLP.Location = new System.Drawing.Point(0, 0);
            this.menuEscapeTableLP.Name = "menuEscapeTableLP";
            this.menuEscapeTableLP.RowCount = 7;
            this.menuEscapeTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 162F));
            this.menuEscapeTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.menuEscapeTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.menuEscapeTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.menuEscapeTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.menuEscapeTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.menuEscapeTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.menuEscapeTableLP.Size = new System.Drawing.Size(1536, 864);
            this.menuEscapeTableLP.TabIndex = 2;
            // 
            // menuEscapeLbPause
            // 
            this.menuEscapeLbPause.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.menuEscapeLbPause.AutoSize = true;
            this.menuEscapeLbPause.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuEscapeLbPause.ForeColor = System.Drawing.Color.White;
            this.menuEscapeLbPause.Location = new System.Drawing.Point(661, 88);
            this.menuEscapeLbPause.Name = "menuEscapeLbPause";
            this.menuEscapeLbPause.Size = new System.Drawing.Size(214, 74);
            this.menuEscapeLbPause.TabIndex = 0;
            this.menuEscapeLbPause.Text = "PAUZA";
            // 
            // menuEscapeBtContinue
            // 
            this.menuEscapeBtContinue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.menuEscapeBtContinue.BackColor = System.Drawing.Color.White;
            this.menuEscapeBtContinue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuEscapeBtContinue.Location = new System.Drawing.Point(597, 478);
            this.menuEscapeBtContinue.Name = "menuEscapeBtContinue";
            this.menuEscapeBtContinue.Size = new System.Drawing.Size(342, 67);
            this.menuEscapeBtContinue.TabIndex = 1;
            this.menuEscapeBtContinue.Text = "POKRAČOVAT";
            this.menuEscapeBtContinue.UseVisualStyleBackColor = false;
            this.menuEscapeBtContinue.Click += new System.EventHandler(this.menuEscapeBtContinue_Click);
            // 
            // menuEscapeBtResetScreen
            // 
            this.menuEscapeBtResetScreen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.menuEscapeBtResetScreen.BackColor = System.Drawing.Color.White;
            this.menuEscapeBtResetScreen.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuEscapeBtResetScreen.Location = new System.Drawing.Point(597, 551);
            this.menuEscapeBtResetScreen.Name = "menuEscapeBtResetScreen";
            this.menuEscapeBtResetScreen.Size = new System.Drawing.Size(342, 67);
            this.menuEscapeBtResetScreen.TabIndex = 5;
            this.menuEscapeBtResetScreen.Text = "RESET OBRAZOVKY";
            this.menuEscapeBtResetScreen.UseVisualStyleBackColor = false;
            this.menuEscapeBtResetScreen.Click += new System.EventHandler(this.menuEscapeBtResetScreen_Click);
            // 
            // menuEscapeBtControls
            // 
            this.menuEscapeBtControls.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.menuEscapeBtControls.BackColor = System.Drawing.Color.White;
            this.menuEscapeBtControls.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuEscapeBtControls.Location = new System.Drawing.Point(597, 624);
            this.menuEscapeBtControls.Name = "menuEscapeBtControls";
            this.menuEscapeBtControls.Size = new System.Drawing.Size(342, 67);
            this.menuEscapeBtControls.TabIndex = 4;
            this.menuEscapeBtControls.Text = "OVLÁDÁNÍ";
            this.menuEscapeBtControls.UseVisualStyleBackColor = false;
            this.menuEscapeBtControls.Click += new System.EventHandler(this.menuEscapeBtControls_Click);
            // 
            // menuEscapeBtStartMenu
            // 
            this.menuEscapeBtStartMenu.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.menuEscapeBtStartMenu.BackColor = System.Drawing.Color.RosyBrown;
            this.menuEscapeBtStartMenu.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuEscapeBtStartMenu.ForeColor = System.Drawing.Color.White;
            this.menuEscapeBtStartMenu.Location = new System.Drawing.Point(597, 697);
            this.menuEscapeBtStartMenu.Name = "menuEscapeBtStartMenu";
            this.menuEscapeBtStartMenu.Size = new System.Drawing.Size(342, 67);
            this.menuEscapeBtStartMenu.TabIndex = 3;
            this.menuEscapeBtStartMenu.Text = "HLAVNÍ NABÍDKA";
            this.menuEscapeBtStartMenu.UseVisualStyleBackColor = false;
            this.menuEscapeBtStartMenu.Click += new System.EventHandler(this.menuEscapeBtStartMenu_Click);
            // 
            // menuControlsContainer
            // 
            this.menuControlsContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(137)))), ((int)(((byte)(158)))));
            this.menuControlsContainer.Controls.Add(this.menuControlsTableLP);
            this.menuControlsContainer.Enabled = false;
            this.menuControlsContainer.Location = new System.Drawing.Point(0, 0);
            this.menuControlsContainer.Name = "menuControlsContainer";
            this.menuControlsContainer.Size = new System.Drawing.Size(1536, 864);
            this.menuControlsContainer.TabIndex = 1;
            this.menuControlsContainer.Visible = false;
            // 
            // menuControlsTableLP
            // 
            this.menuControlsTableLP.ColumnCount = 1;
            this.menuControlsTableLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.menuControlsTableLP.Controls.Add(this.menuControlsLbTitle, 0, 0);
            this.menuControlsTableLP.Controls.Add(this.menuControlsBtEscapeMenu, 0, 2);
            this.menuControlsTableLP.Controls.Add(this.menuControlsTableLPControls, 0, 1);
            this.menuControlsTableLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuControlsTableLP.Location = new System.Drawing.Point(0, 0);
            this.menuControlsTableLP.Name = "menuControlsTableLP";
            this.menuControlsTableLP.RowCount = 3;
            this.menuControlsTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.menuControlsTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.menuControlsTableLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 174F));
            this.menuControlsTableLP.Size = new System.Drawing.Size(1536, 864);
            this.menuControlsTableLP.TabIndex = 0;
            // 
            // menuControlsLbTitle
            // 
            this.menuControlsLbTitle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.menuControlsLbTitle.AutoSize = true;
            this.menuControlsLbTitle.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbTitle.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbTitle.Location = new System.Drawing.Point(635, 86);
            this.menuControlsLbTitle.Name = "menuControlsLbTitle";
            this.menuControlsLbTitle.Size = new System.Drawing.Size(265, 74);
            this.menuControlsLbTitle.TabIndex = 0;
            this.menuControlsLbTitle.Text = "Ovládání";
            // 
            // menuControlsBtEscapeMenu
            // 
            this.menuControlsBtEscapeMenu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.menuControlsBtEscapeMenu.BackColor = System.Drawing.Color.White;
            this.menuControlsBtEscapeMenu.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsBtEscapeMenu.Location = new System.Drawing.Point(597, 693);
            this.menuControlsBtEscapeMenu.Name = "menuControlsBtEscapeMenu";
            this.menuControlsBtEscapeMenu.Size = new System.Drawing.Size(342, 67);
            this.menuControlsBtEscapeMenu.TabIndex = 5;
            this.menuControlsBtEscapeMenu.Text = "ZPĚT";
            this.menuControlsBtEscapeMenu.UseVisualStyleBackColor = false;
            this.menuControlsBtEscapeMenu.Click += new System.EventHandler(this.menuEscapeControlsBtEscapeMenu_Click);
            // 
            // menuControlsTableLPControls
            // 
            this.menuControlsTableLPControls.ColumnCount = 2;
            this.menuControlsTableLPControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.menuControlsTableLPControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbL1, 0, 1);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbL2, 0, 2);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbL3, 0, 3);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbL4, 0, 4);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbL5, 0, 5);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbL6, 0, 6);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbL7, 0, 7);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbR1, 1, 1);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbR2, 1, 2);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbR3, 1, 3);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbR4, 1, 4);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbR5, 1, 5);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbR6, 1, 6);
            this.menuControlsTableLPControls.Controls.Add(this.menuControlsLbR7, 1, 7);
            this.menuControlsTableLPControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuControlsTableLPControls.Location = new System.Drawing.Point(3, 163);
            this.menuControlsTableLPControls.Name = "menuControlsTableLPControls";
            this.menuControlsTableLPControls.RowCount = 9;
            this.menuControlsTableLPControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.menuControlsTableLPControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.menuControlsTableLPControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.menuControlsTableLPControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.menuControlsTableLPControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.menuControlsTableLPControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.menuControlsTableLPControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.menuControlsTableLPControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.menuControlsTableLPControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.menuControlsTableLPControls.Size = new System.Drawing.Size(1530, 524);
            this.menuControlsTableLPControls.TabIndex = 2;
            // 
            // menuControlsLbL1
            // 
            this.menuControlsLbL1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.menuControlsLbL1.AutoSize = true;
            this.menuControlsLbL1.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbL1.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbL1.Location = new System.Drawing.Point(533, 47);
            this.menuControlsLbL1.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
            this.menuControlsLbL1.Name = "menuControlsLbL1";
            this.menuControlsLbL1.Size = new System.Drawing.Size(132, 48);
            this.menuControlsLbL1.TabIndex = 0;
            this.menuControlsLbL1.Text = "Doleva";
            // 
            // menuControlsLbL2
            // 
            this.menuControlsLbL2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.menuControlsLbL2.AutoSize = true;
            this.menuControlsLbL2.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbL2.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbL2.Location = new System.Drawing.Point(507, 111);
            this.menuControlsLbL2.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
            this.menuControlsLbL2.Name = "menuControlsLbL2";
            this.menuControlsLbL2.Size = new System.Drawing.Size(158, 48);
            this.menuControlsLbL2.TabIndex = 1;
            this.menuControlsLbL2.Text = "Doprava";
            // 
            // menuControlsLbL3
            // 
            this.menuControlsLbL3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.menuControlsLbL3.AutoSize = true;
            this.menuControlsLbL3.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbL3.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbL3.Location = new System.Drawing.Point(521, 175);
            this.menuControlsLbL3.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
            this.menuControlsLbL3.Name = "menuControlsLbL3";
            this.menuControlsLbL3.Size = new System.Drawing.Size(144, 48);
            this.menuControlsLbL3.TabIndex = 2;
            this.menuControlsLbL3.Text = "Nahoru";
            // 
            // menuControlsLbL4
            // 
            this.menuControlsLbL4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.menuControlsLbL4.AutoSize = true;
            this.menuControlsLbL4.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbL4.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbL4.Location = new System.Drawing.Point(567, 239);
            this.menuControlsLbL4.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
            this.menuControlsLbL4.Name = "menuControlsLbL4";
            this.menuControlsLbL4.Size = new System.Drawing.Size(98, 48);
            this.menuControlsLbL4.TabIndex = 3;
            this.menuControlsLbL4.Text = "Dolu";
            // 
            // menuControlsLbL5
            // 
            this.menuControlsLbL5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.menuControlsLbL5.AutoSize = true;
            this.menuControlsLbL5.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbL5.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbL5.Location = new System.Drawing.Point(566, 303);
            this.menuControlsLbL5.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
            this.menuControlsLbL5.Name = "menuControlsLbL5";
            this.menuControlsLbL5.Size = new System.Drawing.Size(99, 48);
            this.menuControlsLbL5.TabIndex = 4;
            this.menuControlsLbL5.Text = "Skok";
            // 
            // menuControlsLbL6
            // 
            this.menuControlsLbL6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.menuControlsLbL6.AutoSize = true;
            this.menuControlsLbL6.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbL6.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbL6.Location = new System.Drawing.Point(563, 367);
            this.menuControlsLbL6.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
            this.menuControlsLbL6.Name = "menuControlsLbL6";
            this.menuControlsLbL6.Size = new System.Drawing.Size(102, 48);
            this.menuControlsLbL6.TabIndex = 5;
            this.menuControlsLbL6.Text = "Dash";
            // 
            // menuControlsLbL7
            // 
            this.menuControlsLbL7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.menuControlsLbL7.AutoSize = true;
            this.menuControlsLbL7.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbL7.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbL7.Location = new System.Drawing.Point(539, 431);
            this.menuControlsLbL7.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
            this.menuControlsLbL7.Name = "menuControlsLbL7";
            this.menuControlsLbL7.Size = new System.Drawing.Size(126, 48);
            this.menuControlsLbL7.TabIndex = 6;
            this.menuControlsLbL7.Text = "Držení";
            // 
            // menuControlsLbR1
            // 
            this.menuControlsLbR1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.menuControlsLbR1.AutoSize = true;
            this.menuControlsLbR1.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbR1.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbR1.Location = new System.Drawing.Point(865, 47);
            this.menuControlsLbR1.Margin = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.menuControlsLbR1.Name = "menuControlsLbR1";
            this.menuControlsLbR1.Size = new System.Drawing.Size(44, 48);
            this.menuControlsLbR1.TabIndex = 7;
            this.menuControlsLbR1.Text = "A";
            // 
            // menuControlsLbR2
            // 
            this.menuControlsLbR2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.menuControlsLbR2.AutoSize = true;
            this.menuControlsLbR2.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbR2.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbR2.Location = new System.Drawing.Point(865, 111);
            this.menuControlsLbR2.Margin = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.menuControlsLbR2.Name = "menuControlsLbR2";
            this.menuControlsLbR2.Size = new System.Drawing.Size(46, 48);
            this.menuControlsLbR2.TabIndex = 8;
            this.menuControlsLbR2.Text = "D";
            // 
            // menuControlsLbR3
            // 
            this.menuControlsLbR3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.menuControlsLbR3.AutoSize = true;
            this.menuControlsLbR3.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbR3.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbR3.Location = new System.Drawing.Point(865, 175);
            this.menuControlsLbR3.Margin = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.menuControlsLbR3.Name = "menuControlsLbR3";
            this.menuControlsLbR3.Size = new System.Drawing.Size(55, 48);
            this.menuControlsLbR3.TabIndex = 9;
            this.menuControlsLbR3.Text = "W";
            // 
            // menuControlsLbR4
            // 
            this.menuControlsLbR4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.menuControlsLbR4.AutoSize = true;
            this.menuControlsLbR4.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbR4.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbR4.Location = new System.Drawing.Point(865, 239);
            this.menuControlsLbR4.Margin = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.menuControlsLbR4.Name = "menuControlsLbR4";
            this.menuControlsLbR4.Size = new System.Drawing.Size(40, 48);
            this.menuControlsLbR4.TabIndex = 10;
            this.menuControlsLbR4.Text = "S";
            // 
            // menuControlsLbR5
            // 
            this.menuControlsLbR5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.menuControlsLbR5.AutoSize = true;
            this.menuControlsLbR5.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbR5.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbR5.Location = new System.Drawing.Point(865, 303);
            this.menuControlsLbR5.Margin = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.menuControlsLbR5.Name = "menuControlsLbR5";
            this.menuControlsLbR5.Size = new System.Drawing.Size(117, 48);
            this.menuControlsLbR5.TabIndex = 11;
            this.menuControlsLbR5.Text = "Space";
            // 
            // menuControlsLbR6
            // 
            this.menuControlsLbR6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.menuControlsLbR6.AutoSize = true;
            this.menuControlsLbR6.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbR6.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbR6.Location = new System.Drawing.Point(865, 367);
            this.menuControlsLbR6.Margin = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.menuControlsLbR6.Name = "menuControlsLbR6";
            this.menuControlsLbR6.Size = new System.Drawing.Size(66, 48);
            this.menuControlsLbR6.TabIndex = 12;
            this.menuControlsLbR6.Text = "Alt";
            // 
            // menuControlsLbR7
            // 
            this.menuControlsLbR7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.menuControlsLbR7.AutoSize = true;
            this.menuControlsLbR7.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuControlsLbR7.ForeColor = System.Drawing.Color.White;
            this.menuControlsLbR7.Location = new System.Drawing.Point(865, 431);
            this.menuControlsLbR7.Margin = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.menuControlsLbR7.Name = "menuControlsLbR7";
            this.menuControlsLbR7.Size = new System.Drawing.Size(96, 48);
            this.menuControlsLbR7.TabIndex = 13;
            this.menuControlsLbR7.Text = "Shift";
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerJumpHeadBumpCooldown
            // 
            this.timerJumpHeadBumpCooldown.Interval = 300;
            this.timerJumpHeadBumpCooldown.Tick += new System.EventHandler(this.timerJumpHeadBumpCooldown_Tick);
            // 
            // timerGrabAfterJumpCooldown
            // 
            this.timerGrabAfterJumpCooldown.Interval = 280;
            this.timerGrabAfterJumpCooldown.Tick += new System.EventHandler(this.timerGrabCooldown_Tick);
            // 
            // timerJumpCooldown
            // 
            this.timerJumpCooldown.Interval = 30;
            this.timerJumpCooldown.Tick += new System.EventHandler(this.timerJumpCooldown_Tick);
            // 
            // timerDashedNonVertical
            // 
            this.timerDashedNonVertical.Tick += new System.EventHandler(this.timerDashedNonVertical_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(137)))), ((int)(((byte)(158)))));
            this.ClientSize = new System.Drawing.Size(1536, 864);
            this.Controls.Add(this.lbDeveloperSounds);
            this.Controls.Add(this.lbDeveloperStats);
            this.Controls.Add(this.gameScreen);
            this.Controls.Add(this.menuStartContainer);
            this.Controls.Add(this.menuControlsContainer);
            this.Controls.Add(this.menuEscapeContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Celeste: Forms Edition";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyUp);
            this.gameScreen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.player)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockBorderLeft)).EndInit();
            this.menuStartContainer.ResumeLayout(false);
            this.startTableLP.ResumeLayout(false);
            this.startTableLP.PerformLayout();
            this.menuEscapeContainer.ResumeLayout(false);
            this.menuEscapeTableLP.ResumeLayout(false);
            this.menuEscapeTableLP.PerformLayout();
            this.menuControlsContainer.ResumeLayout(false);
            this.menuControlsTableLP.ResumeLayout(false);
            this.menuControlsTableLP.PerformLayout();
            this.menuControlsTableLPControls.ResumeLayout(false);
            this.menuControlsTableLPControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel gameScreen;
        private PictureBox player;
        private System.Windows.Forms.Timer timer1;
        private Label lbDeveloperStats;
        private System.Windows.Forms.Timer timerJumpHeadBumpCooldown;
        private PictureBox blockBorderLeft;
        private Panel menuStartContainer;
        private TableLayoutPanel startTableLP;
        private Label startLbTitle;
        private Button startBtPlay;
        private Button startBtSettings;
        private Button startBtClose;
        private Panel menuEscapeContainer;
        private Label menuEscapeLbPause;
        private TableLayoutPanel menuEscapeTableLP;
        private Button menuEscapeBtContinue;
        private Button menuEscapeBtStartMenu;
        private Label startLbSubtitle;
        private Button menuEscapeBtControls;
        private Panel menuControlsContainer;
        private TableLayoutPanel menuControlsTableLP;
        private Label menuControlsLbTitle;
        private TableLayoutPanel menuControlsTableLPControls;
        private Button menuControlsBtEscapeMenu;
        private Label menuControlsLbL1;
        private Label menuControlsLbL2;
        private Label menuControlsLbL3;
        private Label menuControlsLbL4;
        private Label menuControlsLbL5;
        private Label menuControlsLbL6;
        private Label menuControlsLbL7;
        private Label menuControlsLbR1;
        private Label menuControlsLbR2;
        private Label menuControlsLbR3;
        private Label menuControlsLbR4;
        private Label menuControlsLbR5;
        private Label menuControlsLbR6;
        private Label menuControlsLbR7;
        private System.Windows.Forms.Timer timerGrabAfterJumpCooldown;
        private System.Windows.Forms.Timer timerJumpCooldown;
        private System.Windows.Forms.Timer timerDashedNonVertical;
        private Button menuEscapeBtResetScreen;
        private Label lbDeveloperSounds;
    }
}