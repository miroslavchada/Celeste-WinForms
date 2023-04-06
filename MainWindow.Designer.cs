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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            gameScreen = new Panel();
            player = new PictureBox();
            lbDeveloperStats = new Label();
            menuMainContainer = new Panel();
            mainTableLP = new TableLayoutPanel();
            mainLbTitle = new Label();
            mainLbSubtitle = new Label();
            mainBtPlay = new Button();
            mainBtSettings = new Button();
            mainBtClose = new Button();
            mainLbAuthor = new Label();
            mainLbInfo = new Label();
            menuEscapeContainer = new Panel();
            menuEscapeTableLP = new TableLayoutPanel();
            menuEscapeLbTitle = new Label();
            menuEscapeBtContinue = new Button();
            menuEscapeBtScreenReset = new Button();
            menuEscapeBtControls = new Button();
            menuEscapeBtMainMenu = new Button();
            menuControlsContainer = new Panel();
            menuControlsTableLP = new TableLayoutPanel();
            menuControlsLbTitle = new Label();
            menuControlsBtEscapeMenu = new Button();
            menuControlsTableLPControls = new TableLayoutPanel();
            menuControlsLbL1 = new Label();
            menuControlsLbL2 = new Label();
            menuControlsLbL3 = new Label();
            menuControlsLbL4 = new Label();
            menuControlsLbL5 = new Label();
            menuControlsLbR1 = new Label();
            menuControlsLbR2 = new Label();
            menuControlsLbR3 = new Label();
            menuControlsLbR4 = new Label();
            menuControlsLbR5 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            timerJumpHeadBumpCooldown = new System.Windows.Forms.Timer(components);
            timerGrabAfterJumpCooldown = new System.Windows.Forms.Timer(components);
            timerJumpCooldown = new System.Windows.Forms.Timer(components);
            timerDashedNonVertical = new System.Windows.Forms.Timer(components);
            gameScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)player).BeginInit();
            menuMainContainer.SuspendLayout();
            mainTableLP.SuspendLayout();
            menuEscapeContainer.SuspendLayout();
            menuEscapeTableLP.SuspendLayout();
            menuControlsContainer.SuspendLayout();
            menuControlsTableLP.SuspendLayout();
            menuControlsTableLPControls.SuspendLayout();
            SuspendLayout();
            // 
            // gameScreen
            // 
            gameScreen.BackColor = Color.FromArgb(15, 21, 39);
            gameScreen.BackgroundImageLayout = ImageLayout.Stretch;
            gameScreen.Controls.Add(player);
            gameScreen.Enabled = false;
            gameScreen.Location = new Point(0, 0);
            gameScreen.Margin = new Padding(2);
            gameScreen.Name = "gameScreen";
            gameScreen.Size = new Size(1536, 864);
            gameScreen.TabIndex = 0;
            gameScreen.Visible = false;
            // 
            // player
            // 
            player.BackColor = Color.Transparent;
            player.BackgroundImage = Properties.Resources.mario_;
            player.BackgroundImageLayout = ImageLayout.None;
            player.Location = new Point(9, 788);
            player.Margin = new Padding(0);
            player.Name = "player";
            player.Size = new Size(51, 67);
            player.TabIndex = 0;
            player.TabStop = false;
            // 
            // lbDeveloperStats
            // 
            lbDeveloperStats.AutoSize = true;
            lbDeveloperStats.BackColor = Color.Black;
            lbDeveloperStats.Enabled = false;
            lbDeveloperStats.Font = new Font("Cascadia Code", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lbDeveloperStats.ForeColor = Color.Yellow;
            lbDeveloperStats.Location = new Point(15, 10);
            lbDeveloperStats.Margin = new Padding(2, 0, 2, 0);
            lbDeveloperStats.Name = "lbDeveloperStats";
            lbDeveloperStats.Padding = new Padding(2, 4, 2, 4);
            lbDeveloperStats.Size = new Size(102, 32);
            lbDeveloperStats.TabIndex = 0;
            lbDeveloperStats.Text = "F3 STATS";
            lbDeveloperStats.Visible = false;
            // 
            // menuMainContainer
            // 
            menuMainContainer.BackColor = Color.FromArgb(200, 215, 235);
            menuMainContainer.Controls.Add(mainTableLP);
            menuMainContainer.Enabled = false;
            menuMainContainer.Location = new Point(0, 0);
            menuMainContainer.Name = "menuMainContainer";
            menuMainContainer.Size = new Size(1536, 864);
            menuMainContainer.TabIndex = 8;
            menuMainContainer.Visible = false;
            // 
            // mainTableLP
            // 
            mainTableLP.BackColor = Color.Transparent;
            mainTableLP.ColumnCount = 1;
            mainTableLP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainTableLP.Controls.Add(mainLbTitle, 0, 0);
            mainTableLP.Controls.Add(mainLbSubtitle, 0, 1);
            mainTableLP.Controls.Add(mainBtPlay, 0, 3);
            mainTableLP.Controls.Add(mainBtSettings, 0, 4);
            mainTableLP.Controls.Add(mainBtClose, 0, 5);
            mainTableLP.Controls.Add(mainLbAuthor, 0, 6);
            mainTableLP.Controls.Add(mainLbInfo, 0, 7);
            mainTableLP.Dock = DockStyle.Fill;
            mainTableLP.Location = new Point(0, 0);
            mainTableLP.Name = "mainTableLP";
            mainTableLP.RowCount = 8;
            mainTableLP.RowStyles.Add(new RowStyle(SizeType.Absolute, 236F));
            mainTableLP.RowStyles.Add(new RowStyle());
            mainTableLP.RowStyles.Add(new RowStyle(SizeType.Absolute, 182F));
            mainTableLP.RowStyles.Add(new RowStyle());
            mainTableLP.RowStyles.Add(new RowStyle());
            mainTableLP.RowStyles.Add(new RowStyle());
            mainTableLP.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainTableLP.RowStyles.Add(new RowStyle());
            mainTableLP.Size = new Size(1536, 864);
            mainTableLP.TabIndex = 0;
            // 
            // mainLbTitle
            // 
            mainLbTitle.Anchor = AnchorStyles.Bottom;
            mainLbTitle.AutoSize = true;
            mainLbTitle.Font = new Font("Segoe UI", 72F, FontStyle.Bold, GraphicsUnit.Point);
            mainLbTitle.ForeColor = Color.FromArgb(68, 101, 147);
            mainLbTitle.Location = new Point(448, 45);
            mainLbTitle.Name = "mainLbTitle";
            mainLbTitle.Size = new Size(640, 191);
            mainLbTitle.TabIndex = 0;
            mainLbTitle.Text = "CELESTE";
            // 
            // mainLbSubtitle
            // 
            mainLbSubtitle.Anchor = AnchorStyles.Top;
            mainLbSubtitle.AutoSize = true;
            mainLbSubtitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
            mainLbSubtitle.ForeColor = Color.FromArgb(68, 101, 147);
            mainLbSubtitle.Location = new Point(425, 236);
            mainLbSubtitle.Name = "mainLbSubtitle";
            mainLbSubtitle.Size = new Size(685, 65);
            mainLbSubtitle.TabIndex = 4;
            mainLbSubtitle.Text = "Fan-made WinForms Remake";
            // 
            // mainBtPlay
            // 
            mainBtPlay.Anchor = AnchorStyles.None;
            mainBtPlay.BackColor = Color.White;
            mainBtPlay.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            mainBtPlay.ForeColor = Color.FromArgb(68, 101, 147);
            mainBtPlay.Location = new Point(543, 486);
            mainBtPlay.Name = "mainBtPlay";
            mainBtPlay.Size = new Size(450, 70);
            mainBtPlay.TabIndex = 1;
            mainBtPlay.Text = "HRÁT";
            mainBtPlay.UseVisualStyleBackColor = false;
            mainBtPlay.Click += buttonClicked;
            // 
            // mainBtSettings
            // 
            mainBtSettings.Anchor = AnchorStyles.Top;
            mainBtSettings.BackColor = Color.White;
            mainBtSettings.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            mainBtSettings.ForeColor = Color.FromArgb(68, 101, 147);
            mainBtSettings.Location = new Point(543, 562);
            mainBtSettings.Name = "mainBtSettings";
            mainBtSettings.Size = new Size(450, 70);
            mainBtSettings.TabIndex = 2;
            mainBtSettings.Text = "NASTAVENÍ";
            mainBtSettings.UseVisualStyleBackColor = false;
            // 
            // mainBtClose
            // 
            mainBtClose.Anchor = AnchorStyles.Top;
            mainBtClose.BackColor = Color.White;
            mainBtClose.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            mainBtClose.ForeColor = Color.FromArgb(193, 54, 54);
            mainBtClose.Location = new Point(543, 638);
            mainBtClose.Name = "mainBtClose";
            mainBtClose.Size = new Size(450, 70);
            mainBtClose.TabIndex = 3;
            mainBtClose.Text = "ZAVŘÍT";
            mainBtClose.UseVisualStyleBackColor = false;
            mainBtClose.Click += buttonClicked;
            // 
            // mainLbAuthor
            // 
            mainLbAuthor.Anchor = AnchorStyles.Bottom;
            mainLbAuthor.AutoSize = true;
            mainLbAuthor.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            mainLbAuthor.ForeColor = Color.FromArgb(68, 101, 147);
            mainLbAuthor.Location = new Point(615, 781);
            mainLbAuthor.Name = "mainLbAuthor";
            mainLbAuthor.Size = new Size(305, 41);
            mainLbAuthor.TabIndex = 5;
            mainLbAuthor.Text = "Miroslav Chada 2023";
            // 
            // mainLbInfo
            // 
            mainLbInfo.Anchor = AnchorStyles.None;
            mainLbInfo.AutoSize = true;
            mainLbInfo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            mainLbInfo.ForeColor = Color.FromArgb(68, 101, 147);
            mainLbInfo.Location = new Point(321, 825);
            mainLbInfo.Margin = new Padding(3, 3, 3, 9);
            mainLbInfo.Name = "mainLbInfo";
            mainLbInfo.Size = new Size(893, 30);
            mainLbInfo.TabIndex = 6;
            mainLbInfo.Text = "Hra vznikla v rámci ročníkové práce ve druhém ročníku na Střední průmyslové škole Ostrov";
            mainLbInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // menuEscapeContainer
            // 
            menuEscapeContainer.BackColor = Color.FromArgb(200, 215, 235);
            menuEscapeContainer.Controls.Add(menuEscapeTableLP);
            menuEscapeContainer.Enabled = false;
            menuEscapeContainer.Location = new Point(0, 0);
            menuEscapeContainer.Name = "menuEscapeContainer";
            menuEscapeContainer.Size = new Size(1536, 864);
            menuEscapeContainer.TabIndex = 9;
            menuEscapeContainer.Visible = false;
            // 
            // menuEscapeTableLP
            // 
            menuEscapeTableLP.ColumnCount = 1;
            menuEscapeTableLP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            menuEscapeTableLP.Controls.Add(menuEscapeLbTitle, 0, 0);
            menuEscapeTableLP.Controls.Add(menuEscapeBtContinue, 0, 2);
            menuEscapeTableLP.Controls.Add(menuEscapeBtScreenReset, 0, 3);
            menuEscapeTableLP.Controls.Add(menuEscapeBtControls, 0, 4);
            menuEscapeTableLP.Controls.Add(menuEscapeBtMainMenu, 0, 5);
            menuEscapeTableLP.Dock = DockStyle.Fill;
            menuEscapeTableLP.Location = new Point(0, 0);
            menuEscapeTableLP.Name = "menuEscapeTableLP";
            menuEscapeTableLP.RowCount = 7;
            menuEscapeTableLP.RowStyles.Add(new RowStyle(SizeType.Absolute, 162F));
            menuEscapeTableLP.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            menuEscapeTableLP.RowStyles.Add(new RowStyle());
            menuEscapeTableLP.RowStyles.Add(new RowStyle());
            menuEscapeTableLP.RowStyles.Add(new RowStyle());
            menuEscapeTableLP.RowStyles.Add(new RowStyle());
            menuEscapeTableLP.RowStyles.Add(new RowStyle(SizeType.Absolute, 97F));
            menuEscapeTableLP.Size = new Size(1536, 864);
            menuEscapeTableLP.TabIndex = 2;
            // 
            // menuEscapeLbTitle
            // 
            menuEscapeLbTitle.Anchor = AnchorStyles.Bottom;
            menuEscapeLbTitle.AutoSize = true;
            menuEscapeLbTitle.Font = new Font("Segoe UI", 28F, FontStyle.Bold, GraphicsUnit.Point);
            menuEscapeLbTitle.ForeColor = Color.FromArgb(68, 101, 147);
            menuEscapeLbTitle.Location = new Point(661, 88);
            menuEscapeLbTitle.Name = "menuEscapeLbTitle";
            menuEscapeLbTitle.Size = new Size(214, 74);
            menuEscapeLbTitle.TabIndex = 0;
            menuEscapeLbTitle.Text = "PAUZA";
            // 
            // menuEscapeBtContinue
            // 
            menuEscapeBtContinue.Anchor = AnchorStyles.None;
            menuEscapeBtContinue.BackColor = Color.White;
            menuEscapeBtContinue.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            menuEscapeBtContinue.ForeColor = Color.FromArgb(68, 101, 147);
            menuEscapeBtContinue.Location = new Point(597, 478);
            menuEscapeBtContinue.Name = "menuEscapeBtContinue";
            menuEscapeBtContinue.Size = new Size(342, 67);
            menuEscapeBtContinue.TabIndex = 1;
            menuEscapeBtContinue.Text = "POKRAČOVAT";
            menuEscapeBtContinue.UseVisualStyleBackColor = false;
            menuEscapeBtContinue.Click += buttonClicked;
            // 
            // menuEscapeBtScreenReset
            // 
            menuEscapeBtScreenReset.Anchor = AnchorStyles.None;
            menuEscapeBtScreenReset.BackColor = Color.White;
            menuEscapeBtScreenReset.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            menuEscapeBtScreenReset.ForeColor = Color.FromArgb(68, 101, 147);
            menuEscapeBtScreenReset.Location = new Point(597, 551);
            menuEscapeBtScreenReset.Name = "menuEscapeBtScreenReset";
            menuEscapeBtScreenReset.Size = new Size(342, 67);
            menuEscapeBtScreenReset.TabIndex = 5;
            menuEscapeBtScreenReset.Text = "RESET OBRAZOVKY";
            menuEscapeBtScreenReset.UseVisualStyleBackColor = false;
            menuEscapeBtScreenReset.Click += buttonClicked;
            // 
            // menuEscapeBtControls
            // 
            menuEscapeBtControls.Anchor = AnchorStyles.None;
            menuEscapeBtControls.BackColor = Color.White;
            menuEscapeBtControls.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            menuEscapeBtControls.ForeColor = Color.FromArgb(68, 101, 147);
            menuEscapeBtControls.Location = new Point(597, 624);
            menuEscapeBtControls.Name = "menuEscapeBtControls";
            menuEscapeBtControls.Size = new Size(342, 67);
            menuEscapeBtControls.TabIndex = 4;
            menuEscapeBtControls.Text = "OVLÁDÁNÍ";
            menuEscapeBtControls.UseVisualStyleBackColor = false;
            menuEscapeBtControls.Click += buttonClicked;
            // 
            // menuEscapeBtMainMenu
            // 
            menuEscapeBtMainMenu.Anchor = AnchorStyles.None;
            menuEscapeBtMainMenu.BackColor = Color.RosyBrown;
            menuEscapeBtMainMenu.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            menuEscapeBtMainMenu.ForeColor = Color.White;
            menuEscapeBtMainMenu.Location = new Point(597, 697);
            menuEscapeBtMainMenu.Name = "menuEscapeBtMainMenu";
            menuEscapeBtMainMenu.Size = new Size(342, 67);
            menuEscapeBtMainMenu.TabIndex = 3;
            menuEscapeBtMainMenu.Text = "HLAVNÍ NABÍDKA";
            menuEscapeBtMainMenu.UseVisualStyleBackColor = false;
            menuEscapeBtMainMenu.Click += buttonClicked;
            // 
            // menuControlsContainer
            // 
            menuControlsContainer.BackColor = Color.FromArgb(200, 215, 235);
            menuControlsContainer.Controls.Add(menuControlsTableLP);
            menuControlsContainer.Enabled = false;
            menuControlsContainer.Location = new Point(0, 0);
            menuControlsContainer.Name = "menuControlsContainer";
            menuControlsContainer.Size = new Size(1536, 864);
            menuControlsContainer.TabIndex = 1;
            menuControlsContainer.Visible = false;
            // 
            // menuControlsTableLP
            // 
            menuControlsTableLP.ColumnCount = 1;
            menuControlsTableLP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            menuControlsTableLP.Controls.Add(menuControlsLbTitle, 0, 0);
            menuControlsTableLP.Controls.Add(menuControlsBtEscapeMenu, 0, 2);
            menuControlsTableLP.Controls.Add(menuControlsTableLPControls, 0, 1);
            menuControlsTableLP.Dock = DockStyle.Fill;
            menuControlsTableLP.Location = new Point(0, 0);
            menuControlsTableLP.Name = "menuControlsTableLP";
            menuControlsTableLP.RowCount = 3;
            menuControlsTableLP.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
            menuControlsTableLP.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            menuControlsTableLP.RowStyles.Add(new RowStyle(SizeType.Absolute, 174F));
            menuControlsTableLP.Size = new Size(1536, 864);
            menuControlsTableLP.TabIndex = 0;
            // 
            // menuControlsLbTitle
            // 
            menuControlsLbTitle.Anchor = AnchorStyles.Bottom;
            menuControlsLbTitle.AutoSize = true;
            menuControlsLbTitle.Font = new Font("Segoe UI", 28F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbTitle.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbTitle.Location = new Point(608, 86);
            menuControlsLbTitle.Name = "menuControlsLbTitle";
            menuControlsLbTitle.Size = new Size(320, 74);
            menuControlsLbTitle.TabIndex = 0;
            menuControlsLbTitle.Text = "OVLÁDÁNÍ";
            // 
            // menuControlsBtEscapeMenu
            // 
            menuControlsBtEscapeMenu.Anchor = AnchorStyles.Top;
            menuControlsBtEscapeMenu.BackColor = Color.White;
            menuControlsBtEscapeMenu.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsBtEscapeMenu.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsBtEscapeMenu.Location = new Point(597, 693);
            menuControlsBtEscapeMenu.Name = "menuControlsBtEscapeMenu";
            menuControlsBtEscapeMenu.Size = new Size(342, 67);
            menuControlsBtEscapeMenu.TabIndex = 5;
            menuControlsBtEscapeMenu.Text = "ZPĚT";
            menuControlsBtEscapeMenu.UseVisualStyleBackColor = false;
            menuControlsBtEscapeMenu.Click += buttonClicked;
            // 
            // menuControlsTableLPControls
            // 
            menuControlsTableLPControls.ColumnCount = 2;
            menuControlsTableLPControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            menuControlsTableLPControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            menuControlsTableLPControls.Controls.Add(menuControlsLbL1, 0, 1);
            menuControlsTableLPControls.Controls.Add(menuControlsLbL2, 0, 2);
            menuControlsTableLPControls.Controls.Add(menuControlsLbL3, 0, 3);
            menuControlsTableLPControls.Controls.Add(menuControlsLbL4, 0, 4);
            menuControlsTableLPControls.Controls.Add(menuControlsLbL5, 0, 5);
            menuControlsTableLPControls.Controls.Add(menuControlsLbR1, 1, 1);
            menuControlsTableLPControls.Controls.Add(menuControlsLbR2, 1, 2);
            menuControlsTableLPControls.Controls.Add(menuControlsLbR3, 1, 3);
            menuControlsTableLPControls.Controls.Add(menuControlsLbR4, 1, 4);
            menuControlsTableLPControls.Controls.Add(menuControlsLbR5, 1, 5);
            menuControlsTableLPControls.Dock = DockStyle.Fill;
            menuControlsTableLPControls.Location = new Point(3, 163);
            menuControlsTableLPControls.Name = "menuControlsTableLPControls";
            menuControlsTableLPControls.RowCount = 7;
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 61F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            menuControlsTableLPControls.Size = new Size(1530, 524);
            menuControlsTableLPControls.TabIndex = 2;
            // 
            // menuControlsLbL1
            // 
            menuControlsLbL1.Anchor = AnchorStyles.Right;
            menuControlsLbL1.AutoSize = true;
            menuControlsLbL1.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbL1.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbL1.Location = new Point(360, 77);
            menuControlsLbL1.Margin = new Padding(0, 0, 100, 0);
            menuControlsLbL1.Name = "menuControlsLbL1";
            menuControlsLbL1.Size = new Size(305, 48);
            menuControlsLbL1.TabIndex = 0;
            menuControlsLbL1.Text = "Doleva / Doprava";
            // 
            // menuControlsLbL2
            // 
            menuControlsLbL2.Anchor = AnchorStyles.Right;
            menuControlsLbL2.AutoSize = true;
            menuControlsLbL2.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbL2.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbL2.Location = new Point(408, 157);
            menuControlsLbL2.Margin = new Padding(0, 0, 100, 0);
            menuControlsLbL2.Name = "menuControlsLbL2";
            menuControlsLbL2.Size = new Size(257, 48);
            menuControlsLbL2.TabIndex = 2;
            menuControlsLbL2.Text = "Nahoru / Dolu";
            // 
            // menuControlsLbL3
            // 
            menuControlsLbL3.Anchor = AnchorStyles.Right;
            menuControlsLbL3.AutoSize = true;
            menuControlsLbL3.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbL3.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbL3.Location = new Point(566, 237);
            menuControlsLbL3.Margin = new Padding(0, 0, 100, 0);
            menuControlsLbL3.Name = "menuControlsLbL3";
            menuControlsLbL3.Size = new Size(99, 48);
            menuControlsLbL3.TabIndex = 4;
            menuControlsLbL3.Text = "Skok";
            // 
            // menuControlsLbL4
            // 
            menuControlsLbL4.Anchor = AnchorStyles.Right;
            menuControlsLbL4.AutoSize = true;
            menuControlsLbL4.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbL4.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbL4.Location = new Point(563, 317);
            menuControlsLbL4.Margin = new Padding(0, 0, 100, 0);
            menuControlsLbL4.Name = "menuControlsLbL4";
            menuControlsLbL4.Size = new Size(102, 48);
            menuControlsLbL4.TabIndex = 5;
            menuControlsLbL4.Text = "Dash";
            // 
            // menuControlsLbL5
            // 
            menuControlsLbL5.Anchor = AnchorStyles.Right;
            menuControlsLbL5.AutoSize = true;
            menuControlsLbL5.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbL5.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbL5.Location = new Point(539, 397);
            menuControlsLbL5.Margin = new Padding(0, 0, 100, 0);
            menuControlsLbL5.Name = "menuControlsLbL5";
            menuControlsLbL5.Size = new Size(126, 48);
            menuControlsLbL5.TabIndex = 6;
            menuControlsLbL5.Text = "Držení";
            // 
            // menuControlsLbR1
            // 
            menuControlsLbR1.Anchor = AnchorStyles.Left;
            menuControlsLbR1.AutoSize = true;
            menuControlsLbR1.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbR1.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbR1.Location = new Point(865, 77);
            menuControlsLbR1.Margin = new Padding(100, 0, 0, 0);
            menuControlsLbR1.Name = "menuControlsLbR1";
            menuControlsLbR1.Size = new Size(105, 48);
            menuControlsLbR1.TabIndex = 7;
            menuControlsLbR1.Text = "A / D";
            // 
            // menuControlsLbR2
            // 
            menuControlsLbR2.Anchor = AnchorStyles.Left;
            menuControlsLbR2.AutoSize = true;
            menuControlsLbR2.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbR2.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbR2.Location = new Point(865, 157);
            menuControlsLbR2.Margin = new Padding(100, 0, 0, 0);
            menuControlsLbR2.Name = "menuControlsLbR2";
            menuControlsLbR2.Size = new Size(110, 48);
            menuControlsLbR2.TabIndex = 8;
            menuControlsLbR2.Text = "W / S";
            // 
            // menuControlsLbR3
            // 
            menuControlsLbR3.Anchor = AnchorStyles.Left;
            menuControlsLbR3.AutoSize = true;
            menuControlsLbR3.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbR3.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbR3.Location = new Point(865, 237);
            menuControlsLbR3.Margin = new Padding(100, 0, 0, 0);
            menuControlsLbR3.Name = "menuControlsLbR3";
            menuControlsLbR3.Size = new Size(170, 48);
            menuControlsLbR3.TabIndex = 11;
            menuControlsLbR3.Text = "Mezerník";
            // 
            // menuControlsLbR4
            // 
            menuControlsLbR4.Anchor = AnchorStyles.Left;
            menuControlsLbR4.AutoSize = true;
            menuControlsLbR4.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbR4.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbR4.Location = new Point(865, 317);
            menuControlsLbR4.Margin = new Padding(100, 0, 0, 0);
            menuControlsLbR4.Name = "menuControlsLbR4";
            menuControlsLbR4.Size = new Size(66, 48);
            menuControlsLbR4.TabIndex = 12;
            menuControlsLbR4.Text = "Alt";
            // 
            // menuControlsLbR5
            // 
            menuControlsLbR5.Anchor = AnchorStyles.Left;
            menuControlsLbR5.AutoSize = true;
            menuControlsLbR5.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbR5.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbR5.Location = new Point(865, 397);
            menuControlsLbR5.Margin = new Padding(100, 0, 0, 0);
            menuControlsLbR5.Name = "menuControlsLbR5";
            menuControlsLbR5.Size = new Size(96, 48);
            menuControlsLbR5.TabIndex = 13;
            menuControlsLbR5.Text = "Shift";
            // 
            // timer1
            // 
            timer1.Interval = 1;
            timer1.Tick += timer1_Tick;
            // 
            // timerJumpHeadBumpCooldown
            // 
            timerJumpHeadBumpCooldown.Interval = 300;
            timerJumpHeadBumpCooldown.Tick += timerJumpHeadBumpCooldown_Tick;
            // 
            // timerGrabAfterJumpCooldown
            // 
            timerGrabAfterJumpCooldown.Interval = 280;
            timerGrabAfterJumpCooldown.Tick += timerGrabCooldown_Tick;
            // 
            // timerJumpCooldown
            // 
            timerJumpCooldown.Interval = 30;
            timerJumpCooldown.Tick += timerJumpCooldown_Tick;
            // 
            // timerDashedNonVertical
            // 
            timerDashedNonVertical.Tick += timerDashedNonVertical_Tick;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.FromArgb(15, 21, 39);
            ClientSize = new Size(1536, 864);
            Controls.Add(lbDeveloperStats);
            Controls.Add(gameScreen);
            Controls.Add(menuMainContainer);
            Controls.Add(menuControlsContainer);
            Controls.Add(menuEscapeContainer);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Celeste: Forms Edition";
            TransparencyKey = Color.FromArgb(64, 64, 0);
            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
            gameScreen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)player).EndInit();
            menuMainContainer.ResumeLayout(false);
            mainTableLP.ResumeLayout(false);
            mainTableLP.PerformLayout();
            menuEscapeContainer.ResumeLayout(false);
            menuEscapeTableLP.ResumeLayout(false);
            menuEscapeTableLP.PerformLayout();
            menuControlsContainer.ResumeLayout(false);
            menuControlsTableLP.ResumeLayout(false);
            menuControlsTableLP.PerformLayout();
            menuControlsTableLPControls.ResumeLayout(false);
            menuControlsTableLPControls.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel gameScreen;
        private PictureBox player;
        private System.Windows.Forms.Timer timer1;
        private Label lbDeveloperStats;
        private System.Windows.Forms.Timer timerJumpHeadBumpCooldown;
        private Panel menuMainContainer;
        private TableLayoutPanel mainTableLP;
        private Label mainLbTitle;
        private Button mainBtPlay;
        private Button mainBtSettings;
        private Button mainBtClose;
        private Panel menuEscapeContainer;
        private Label menuEscapeLbTitle;
        private TableLayoutPanel menuEscapeTableLP;
        private Button menuEscapeBtContinue;
        private Button menuEscapeBtMainMenu;
        private Label mainLbSubtitle;
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
        private Label menuControlsLbR1;
        private Label menuControlsLbR2;
        private Label menuControlsLbR3;
        private Label menuControlsLbR4;
        private System.Windows.Forms.Timer timerGrabAfterJumpCooldown;
        private System.Windows.Forms.Timer timerJumpCooldown;
        private System.Windows.Forms.Timer timerDashedNonVertical;
        private Button menuEscapeBtScreenReset;
        private Label mainLbInfo;
        private Label mainLbAuthor;
        private Label menuControlsLbR5;
        private Label menuControlsLbL5;
    }
}