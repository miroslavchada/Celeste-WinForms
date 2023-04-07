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
            menuMainTableLP = new TableLayoutPanel();
            menuMainLbTitle = new Label();
            menuMainLbSubtitle = new Label();
            menuMainBtPlay = new Button();
            menuMainBtSettings = new Button();
            menuMainBtClose = new Button();
            menuMainLbAuthor = new Label();
            mainLbInfo = new Label();
            menuEscapeContainer = new Panel();
            menuEscapeTableLP = new TableLayoutPanel();
            menuEscapeLbTitle = new Label();
            menuEscapeBtContinue = new Button();
            menuEscapeBtScreenReset = new Button();
            menuEscapeBtSettings = new Button();
            menuEscapeBtControls = new Button();
            menuEscapeBtMainMenu = new Button();
            menuControlsContainer = new Panel();
            menuControlsTableLP = new TableLayoutPanel();
            menuControlsLbTitle = new Label();
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
            menuControlsBtEscapeMenu = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            timerJumpHeadBumpCooldown = new System.Windows.Forms.Timer(components);
            timerGrabAfterJumpCooldown = new System.Windows.Forms.Timer(components);
            timerJumpCooldown = new System.Windows.Forms.Timer(components);
            timerDashedNonVertical = new System.Windows.Forms.Timer(components);
            menuSettingsContainer = new Panel();
            menuSettignsTableLP = new TableLayoutPanel();
            menuSettingsLbTitle = new Label();
            menuSettingsTableLPSettings = new TableLayoutPanel();
            menuSettingsLbL1 = new Label();
            menuSettingsLbR1Container = new TableLayoutPanel();
            menuSettingsTrackR1 = new TrackBar();
            menuSettingsLbVolumeR1 = new Label();
            menuSettingsLbL2 = new Label();
            menuSettingsLbR2Container = new TableLayoutPanel();
            menuSettingsLbR2Language = new Label();
            menuSettingsLbR2ControlL = new Label();
            menuSettingsLbR2ControlR = new Label();
            menuSettingsLbL3 = new Label();
            menuSettingsLbR3Container = new TableLayoutPanel();
            menuSettingsLbR3Input = new Label();
            menuSettingsLbR3ControlL = new Label();
            menuSettingsLbR3ControlR = new Label();
            menuSettingsBtBack = new Button();
            gameScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)player).BeginInit();
            menuMainContainer.SuspendLayout();
            menuMainTableLP.SuspendLayout();
            menuEscapeContainer.SuspendLayout();
            menuEscapeTableLP.SuspendLayout();
            menuControlsContainer.SuspendLayout();
            menuControlsTableLP.SuspendLayout();
            menuControlsTableLPControls.SuspendLayout();
            menuSettingsContainer.SuspendLayout();
            menuSettignsTableLP.SuspendLayout();
            menuSettingsTableLPSettings.SuspendLayout();
            menuSettingsLbR1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)menuSettingsTrackR1).BeginInit();
            menuSettingsLbR2Container.SuspendLayout();
            menuSettingsLbR3Container.SuspendLayout();
            SuspendLayout();
            // 
            // gameScreen
            // 
            gameScreen.BackColor = Color.FromArgb(15, 21, 39);
            gameScreen.BackgroundImageLayout = ImageLayout.Stretch;
            gameScreen.Controls.Add(player);
            gameScreen.Enabled = false;
            gameScreen.Location = new Point(0, 900);
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
            menuMainContainer.Controls.Add(menuMainTableLP);
            menuMainContainer.Enabled = false;
            menuMainContainer.Location = new Point(0, 0);
            menuMainContainer.Name = "menuMainContainer";
            menuMainContainer.Size = new Size(1536, 864);
            menuMainContainer.TabIndex = 8;
            menuMainContainer.Visible = false;
            // 
            // menuMainTableLP
            // 
            menuMainTableLP.BackColor = Color.Transparent;
            menuMainTableLP.ColumnCount = 1;
            menuMainTableLP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            menuMainTableLP.Controls.Add(menuMainLbTitle, 0, 0);
            menuMainTableLP.Controls.Add(menuMainLbSubtitle, 0, 1);
            menuMainTableLP.Controls.Add(menuMainBtPlay, 0, 3);
            menuMainTableLP.Controls.Add(menuMainBtSettings, 0, 4);
            menuMainTableLP.Controls.Add(menuMainBtClose, 0, 5);
            menuMainTableLP.Controls.Add(menuMainLbAuthor, 0, 6);
            menuMainTableLP.Controls.Add(mainLbInfo, 0, 7);
            menuMainTableLP.Dock = DockStyle.Fill;
            menuMainTableLP.Location = new Point(0, 0);
            menuMainTableLP.Name = "menuMainTableLP";
            menuMainTableLP.RowCount = 8;
            menuMainTableLP.RowStyles.Add(new RowStyle(SizeType.Absolute, 236F));
            menuMainTableLP.RowStyles.Add(new RowStyle());
            menuMainTableLP.RowStyles.Add(new RowStyle(SizeType.Absolute, 182F));
            menuMainTableLP.RowStyles.Add(new RowStyle());
            menuMainTableLP.RowStyles.Add(new RowStyle());
            menuMainTableLP.RowStyles.Add(new RowStyle());
            menuMainTableLP.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            menuMainTableLP.RowStyles.Add(new RowStyle());
            menuMainTableLP.Size = new Size(1536, 864);
            menuMainTableLP.TabIndex = 0;
            // 
            // menuMainLbTitle
            // 
            menuMainLbTitle.Anchor = AnchorStyles.Bottom;
            menuMainLbTitle.AutoSize = true;
            menuMainLbTitle.Font = new Font("Segoe UI", 62F, FontStyle.Bold, GraphicsUnit.Point);
            menuMainLbTitle.ForeColor = Color.FromArgb(68, 101, 147);
            menuMainLbTitle.Location = new Point(493, 72);
            menuMainLbTitle.Name = "menuMainLbTitle";
            menuMainLbTitle.Size = new Size(550, 164);
            menuMainLbTitle.TabIndex = 0;
            menuMainLbTitle.Text = "CELESTE";
            // 
            // menuMainLbSubtitle
            // 
            menuMainLbSubtitle.Anchor = AnchorStyles.Top;
            menuMainLbSubtitle.AutoSize = true;
            menuMainLbSubtitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            menuMainLbSubtitle.ForeColor = Color.FromArgb(68, 101, 147);
            menuMainLbSubtitle.Location = new Point(481, 236);
            menuMainLbSubtitle.Name = "menuMainLbSubtitle";
            menuMainLbSubtitle.Size = new Size(573, 54);
            menuMainLbSubtitle.TabIndex = 4;
            menuMainLbSubtitle.Text = "Fan-made WinForms Remake";
            // 
            // menuMainBtPlay
            // 
            menuMainBtPlay.Anchor = AnchorStyles.None;
            menuMainBtPlay.BackColor = Color.White;
            menuMainBtPlay.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            menuMainBtPlay.ForeColor = Color.FromArgb(68, 101, 147);
            menuMainBtPlay.Location = new Point(543, 475);
            menuMainBtPlay.Name = "menuMainBtPlay";
            menuMainBtPlay.Size = new Size(450, 70);
            menuMainBtPlay.TabIndex = 1;
            menuMainBtPlay.Text = "HRÁT";
            menuMainBtPlay.UseVisualStyleBackColor = false;
            menuMainBtPlay.Click += buttonClicked;
            // 
            // menuMainBtSettings
            // 
            menuMainBtSettings.Anchor = AnchorStyles.Top;
            menuMainBtSettings.BackColor = Color.White;
            menuMainBtSettings.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            menuMainBtSettings.ForeColor = Color.FromArgb(68, 101, 147);
            menuMainBtSettings.Location = new Point(543, 551);
            menuMainBtSettings.Name = "menuMainBtSettings";
            menuMainBtSettings.Size = new Size(450, 70);
            menuMainBtSettings.TabIndex = 2;
            menuMainBtSettings.Text = "NASTAVENÍ";
            menuMainBtSettings.UseVisualStyleBackColor = false;
            menuMainBtSettings.Click += buttonClicked;
            // 
            // menuMainBtClose
            // 
            menuMainBtClose.Anchor = AnchorStyles.Top;
            menuMainBtClose.BackColor = Color.White;
            menuMainBtClose.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            menuMainBtClose.ForeColor = Color.FromArgb(193, 54, 54);
            menuMainBtClose.Location = new Point(543, 627);
            menuMainBtClose.Name = "menuMainBtClose";
            menuMainBtClose.Size = new Size(450, 70);
            menuMainBtClose.TabIndex = 3;
            menuMainBtClose.Text = "ZAVŘÍT";
            menuMainBtClose.UseVisualStyleBackColor = false;
            menuMainBtClose.Click += buttonClicked;
            // 
            // menuMainLbAuthor
            // 
            menuMainLbAuthor.Anchor = AnchorStyles.Bottom;
            menuMainLbAuthor.AutoSize = true;
            menuMainLbAuthor.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            menuMainLbAuthor.ForeColor = Color.FromArgb(68, 101, 147);
            menuMainLbAuthor.Location = new Point(615, 781);
            menuMainLbAuthor.Name = "menuMainLbAuthor";
            menuMainLbAuthor.Size = new Size(305, 41);
            menuMainLbAuthor.TabIndex = 5;
            menuMainLbAuthor.Text = "Miroslav Chada 2023";
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
            menuEscapeTableLP.Controls.Add(menuEscapeBtSettings, 0, 4);
            menuEscapeTableLP.Controls.Add(menuEscapeBtControls, 0, 5);
            menuEscapeTableLP.Controls.Add(menuEscapeBtMainMenu, 0, 6);
            menuEscapeTableLP.Dock = DockStyle.Fill;
            menuEscapeTableLP.Location = new Point(0, 0);
            menuEscapeTableLP.Name = "menuEscapeTableLP";
            menuEscapeTableLP.RowCount = 8;
            menuEscapeTableLP.RowStyles.Add(new RowStyle(SizeType.Absolute, 162F));
            menuEscapeTableLP.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            menuEscapeTableLP.RowStyles.Add(new RowStyle());
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
            menuEscapeBtContinue.Location = new Point(597, 405);
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
            menuEscapeBtScreenReset.Location = new Point(597, 478);
            menuEscapeBtScreenReset.Name = "menuEscapeBtScreenReset";
            menuEscapeBtScreenReset.Size = new Size(342, 67);
            menuEscapeBtScreenReset.TabIndex = 5;
            menuEscapeBtScreenReset.Text = "RESET OBRAZOVKY";
            menuEscapeBtScreenReset.UseVisualStyleBackColor = false;
            menuEscapeBtScreenReset.Click += buttonClicked;
            // 
            // menuEscapeBtSettings
            // 
            menuEscapeBtSettings.Anchor = AnchorStyles.None;
            menuEscapeBtSettings.BackColor = Color.White;
            menuEscapeBtSettings.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            menuEscapeBtSettings.ForeColor = Color.FromArgb(68, 101, 147);
            menuEscapeBtSettings.Location = new Point(597, 551);
            menuEscapeBtSettings.Name = "menuEscapeBtSettings";
            menuEscapeBtSettings.Size = new Size(342, 67);
            menuEscapeBtSettings.TabIndex = 6;
            menuEscapeBtSettings.Text = "NASTAVENÍ";
            menuEscapeBtSettings.UseVisualStyleBackColor = false;
            menuEscapeBtSettings.Click += buttonClicked;
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
            menuEscapeBtMainMenu.BackColor = Color.White;
            menuEscapeBtMainMenu.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            menuEscapeBtMainMenu.ForeColor = Color.FromArgb(193, 54, 54);
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
            menuControlsTableLP.Controls.Add(menuControlsTableLPControls, 0, 1);
            menuControlsTableLP.Controls.Add(menuControlsBtEscapeMenu, 0, 2);
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
            // menuControlsTableLPControls
            // 
            menuControlsTableLPControls.ColumnCount = 2;
            menuControlsTableLPControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            menuControlsTableLPControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            menuControlsTableLPControls.Controls.Add(menuControlsLbL1, 0, 0);
            menuControlsTableLPControls.Controls.Add(menuControlsLbL2, 0, 1);
            menuControlsTableLPControls.Controls.Add(menuControlsLbL3, 0, 2);
            menuControlsTableLPControls.Controls.Add(menuControlsLbL4, 0, 3);
            menuControlsTableLPControls.Controls.Add(menuControlsLbL5, 0, 4);
            menuControlsTableLPControls.Controls.Add(menuControlsLbR1, 1, 0);
            menuControlsTableLPControls.Controls.Add(menuControlsLbR2, 1, 1);
            menuControlsTableLPControls.Controls.Add(menuControlsLbR3, 1, 2);
            menuControlsTableLPControls.Controls.Add(menuControlsLbR4, 1, 3);
            menuControlsTableLPControls.Controls.Add(menuControlsLbR5, 1, 4);
            menuControlsTableLPControls.Dock = DockStyle.Fill;
            menuControlsTableLPControls.Location = new Point(3, 203);
            menuControlsTableLPControls.Margin = new Padding(3, 43, 3, 3);
            menuControlsTableLPControls.Name = "menuControlsTableLPControls";
            menuControlsTableLPControls.RowCount = 6;
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            menuControlsTableLPControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            menuControlsTableLPControls.Size = new Size(1530, 484);
            menuControlsTableLPControls.TabIndex = 2;
            // 
            // menuControlsLbL1
            // 
            menuControlsLbL1.Anchor = AnchorStyles.Right;
            menuControlsLbL1.AutoSize = true;
            menuControlsLbL1.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuControlsLbL1.ForeColor = Color.FromArgb(68, 101, 147);
            menuControlsLbL1.Location = new Point(360, 16);
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
            menuControlsLbL2.Location = new Point(408, 96);
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
            menuControlsLbL3.Location = new Point(566, 176);
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
            menuControlsLbL4.Location = new Point(563, 256);
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
            menuControlsLbL5.Location = new Point(539, 336);
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
            menuControlsLbR1.Location = new Point(865, 16);
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
            menuControlsLbR2.Location = new Point(865, 96);
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
            menuControlsLbR3.Location = new Point(865, 176);
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
            menuControlsLbR4.Location = new Point(865, 256);
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
            menuControlsLbR5.Location = new Point(865, 336);
            menuControlsLbR5.Margin = new Padding(100, 0, 0, 0);
            menuControlsLbR5.Name = "menuControlsLbR5";
            menuControlsLbR5.Size = new Size(96, 48);
            menuControlsLbR5.TabIndex = 13;
            menuControlsLbR5.Text = "Shift";
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
            // menuSettingsContainer
            // 
            menuSettingsContainer.BackColor = Color.FromArgb(200, 215, 235);
            menuSettingsContainer.Controls.Add(menuSettignsTableLP);
            menuSettingsContainer.Enabled = false;
            menuSettingsContainer.Location = new Point(0, 0);
            menuSettingsContainer.Name = "menuSettingsContainer";
            menuSettingsContainer.Size = new Size(1536, 864);
            menuSettingsContainer.TabIndex = 2;
            menuSettingsContainer.Visible = false;
            // 
            // menuSettignsTableLP
            // 
            menuSettignsTableLP.ColumnCount = 1;
            menuSettignsTableLP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            menuSettignsTableLP.Controls.Add(menuSettingsLbTitle, 0, 0);
            menuSettignsTableLP.Controls.Add(menuSettingsTableLPSettings, 0, 1);
            menuSettignsTableLP.Controls.Add(menuSettingsBtBack, 0, 2);
            menuSettignsTableLP.Dock = DockStyle.Fill;
            menuSettignsTableLP.Location = new Point(0, 0);
            menuSettignsTableLP.Name = "menuSettignsTableLP";
            menuSettignsTableLP.RowCount = 3;
            menuSettignsTableLP.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
            menuSettignsTableLP.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            menuSettignsTableLP.RowStyles.Add(new RowStyle(SizeType.Absolute, 174F));
            menuSettignsTableLP.Size = new Size(1536, 864);
            menuSettignsTableLP.TabIndex = 0;
            // 
            // menuSettingsLbTitle
            // 
            menuSettingsLbTitle.Anchor = AnchorStyles.Bottom;
            menuSettingsLbTitle.AutoSize = true;
            menuSettingsLbTitle.Font = new Font("Segoe UI", 28F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsLbTitle.ForeColor = Color.FromArgb(68, 101, 147);
            menuSettingsLbTitle.Location = new Point(598, 86);
            menuSettingsLbTitle.Name = "menuSettingsLbTitle";
            menuSettingsLbTitle.Size = new Size(340, 74);
            menuSettingsLbTitle.TabIndex = 0;
            menuSettingsLbTitle.Text = "NASTAVENÍ";
            // 
            // menuSettingsTableLPSettings
            // 
            menuSettingsTableLPSettings.ColumnCount = 2;
            menuSettingsTableLPSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            menuSettingsTableLPSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            menuSettingsTableLPSettings.Controls.Add(menuSettingsLbL1, 0, 0);
            menuSettingsTableLPSettings.Controls.Add(menuSettingsLbR1Container, 1, 0);
            menuSettingsTableLPSettings.Controls.Add(menuSettingsLbL2, 0, 1);
            menuSettingsTableLPSettings.Controls.Add(menuSettingsLbR2Container, 1, 1);
            menuSettingsTableLPSettings.Controls.Add(menuSettingsLbL3, 0, 2);
            menuSettingsTableLPSettings.Controls.Add(menuSettingsLbR3Container, 1, 2);
            menuSettingsTableLPSettings.Dock = DockStyle.Fill;
            menuSettingsTableLPSettings.Location = new Point(3, 203);
            menuSettingsTableLPSettings.Margin = new Padding(3, 43, 3, 3);
            menuSettingsTableLPSettings.Name = "menuSettingsTableLPSettings";
            menuSettingsTableLPSettings.RowCount = 4;
            menuSettingsTableLPSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            menuSettingsTableLPSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            menuSettingsTableLPSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            menuSettingsTableLPSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            menuSettingsTableLPSettings.Size = new Size(1530, 484);
            menuSettingsTableLPSettings.TabIndex = 6;
            // 
            // menuSettingsLbL1
            // 
            menuSettingsLbL1.Anchor = AnchorStyles.Right;
            menuSettingsLbL1.AutoSize = true;
            menuSettingsLbL1.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsLbL1.ForeColor = Color.FromArgb(68, 101, 147);
            menuSettingsLbL1.Location = new Point(502, 16);
            menuSettingsLbL1.Margin = new Padding(0, 0, 100, 0);
            menuSettingsLbL1.Name = "menuSettingsLbL1";
            menuSettingsLbL1.Size = new Size(163, 48);
            menuSettingsLbL1.TabIndex = 5;
            menuSettingsLbL1.Text = "Hlasitost";
            // 
            // menuSettingsLbR1Container
            // 
            menuSettingsLbR1Container.ColumnCount = 2;
            menuSettingsLbR1Container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            menuSettingsLbR1Container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            menuSettingsLbR1Container.Controls.Add(menuSettingsTrackR1, 0, 1);
            menuSettingsLbR1Container.Controls.Add(menuSettingsLbVolumeR1, 1, 1);
            menuSettingsLbR1Container.Dock = DockStyle.Fill;
            menuSettingsLbR1Container.Location = new Point(765, 0);
            menuSettingsLbR1Container.Margin = new Padding(0);
            menuSettingsLbR1Container.Name = "menuSettingsLbR1Container";
            menuSettingsLbR1Container.RowCount = 3;
            menuSettingsLbR1Container.RowStyles.Add(new RowStyle(SizeType.Percent, 22.2222214F));
            menuSettingsLbR1Container.RowStyles.Add(new RowStyle(SizeType.Percent, 55.5555573F));
            menuSettingsLbR1Container.RowStyles.Add(new RowStyle(SizeType.Percent, 22.2222214F));
            menuSettingsLbR1Container.Size = new Size(765, 80);
            menuSettingsLbR1Container.TabIndex = 6;
            // 
            // menuSettingsTrackR1
            // 
            menuSettingsTrackR1.Dock = DockStyle.Fill;
            menuSettingsTrackR1.LargeChange = 1;
            menuSettingsTrackR1.Location = new Point(100, 20);
            menuSettingsTrackR1.Margin = new Padding(100, 3, 0, 3);
            menuSettingsTrackR1.Maximum = 20;
            menuSettingsTrackR1.Name = "menuSettingsTrackR1";
            menuSettingsTrackR1.Size = new Size(359, 38);
            menuSettingsTrackR1.TabIndex = 4;
            menuSettingsTrackR1.Value = 14;
            menuSettingsTrackR1.Scroll += menuSettingsTrackR1_Scroll;
            // 
            // menuSettingsLbVolumeR1
            // 
            menuSettingsLbVolumeR1.Anchor = AnchorStyles.Left;
            menuSettingsLbVolumeR1.AutoSize = true;
            menuSettingsLbVolumeR1.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsLbVolumeR1.ForeColor = Color.FromArgb(68, 101, 147);
            menuSettingsLbVolumeR1.Location = new Point(459, 17);
            menuSettingsLbVolumeR1.Margin = new Padding(0);
            menuSettingsLbVolumeR1.Name = "menuSettingsLbVolumeR1";
            menuSettingsLbVolumeR1.Size = new Size(55, 44);
            menuSettingsLbVolumeR1.TabIndex = 6;
            menuSettingsLbVolumeR1.Text = "70";
            // 
            // menuSettingsLbL2
            // 
            menuSettingsLbL2.Anchor = AnchorStyles.Right;
            menuSettingsLbL2.AutoSize = true;
            menuSettingsLbL2.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsLbL2.ForeColor = Color.FromArgb(68, 101, 147);
            menuSettingsLbL2.Location = new Point(558, 96);
            menuSettingsLbL2.Margin = new Padding(0, 0, 100, 0);
            menuSettingsLbL2.Name = "menuSettingsLbL2";
            menuSettingsLbL2.Size = new Size(107, 48);
            menuSettingsLbL2.TabIndex = 1;
            menuSettingsLbL2.Text = "Jazyk";
            // 
            // menuSettingsLbR2Container
            // 
            menuSettingsLbR2Container.ColumnCount = 4;
            menuSettingsLbR2Container.ColumnStyles.Add(new ColumnStyle());
            menuSettingsLbR2Container.ColumnStyles.Add(new ColumnStyle());
            menuSettingsLbR2Container.ColumnStyles.Add(new ColumnStyle());
            menuSettingsLbR2Container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            menuSettingsLbR2Container.Controls.Add(menuSettingsLbR2Language, 0, 0);
            menuSettingsLbR2Container.Controls.Add(menuSettingsLbR2ControlL, 0, 0);
            menuSettingsLbR2Container.Controls.Add(menuSettingsLbR2ControlR, 2, 0);
            menuSettingsLbR2Container.Dock = DockStyle.Fill;
            menuSettingsLbR2Container.Location = new Point(765, 80);
            menuSettingsLbR2Container.Margin = new Padding(0);
            menuSettingsLbR2Container.Name = "menuSettingsLbR2Container";
            menuSettingsLbR2Container.RowCount = 1;
            menuSettingsLbR2Container.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            menuSettingsLbR2Container.Size = new Size(765, 80);
            menuSettingsLbR2Container.TabIndex = 2;
            // 
            // menuSettingsLbR2Language
            // 
            menuSettingsLbR2Language.Anchor = AnchorStyles.Right;
            menuSettingsLbR2Language.AutoSize = true;
            menuSettingsLbR2Language.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsLbR2Language.ForeColor = Color.FromArgb(68, 101, 147);
            menuSettingsLbR2Language.Location = new Point(162, 16);
            menuSettingsLbR2Language.Margin = new Padding(0);
            menuSettingsLbR2Language.Name = "menuSettingsLbR2Language";
            menuSettingsLbR2Language.Size = new Size(114, 48);
            menuSettingsLbR2Language.TabIndex = 1;
            menuSettingsLbR2Language.Text = "Česky";
            // 
            // menuSettingsLbR2ControlL
            // 
            menuSettingsLbR2ControlL.Anchor = AnchorStyles.Right;
            menuSettingsLbR2ControlL.AutoSize = true;
            menuSettingsLbR2ControlL.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsLbR2ControlL.ForeColor = Color.FromArgb(130, 160, 200);
            menuSettingsLbR2ControlL.Location = new Point(100, 11);
            menuSettingsLbR2ControlL.Margin = new Padding(100, 0, 0, 0);
            menuSettingsLbR2ControlL.Name = "menuSettingsLbR2ControlL";
            menuSettingsLbR2ControlL.Padding = new Padding(5);
            menuSettingsLbR2ControlL.Size = new Size(62, 58);
            menuSettingsLbR2ControlL.TabIndex = 0;
            menuSettingsLbR2ControlL.Text = "◀";
            menuSettingsLbR2ControlL.Click += buttonClicked;
            // 
            // menuSettingsLbR2ControlR
            // 
            menuSettingsLbR2ControlR.Anchor = AnchorStyles.Right;
            menuSettingsLbR2ControlR.AutoSize = true;
            menuSettingsLbR2ControlR.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsLbR2ControlR.ForeColor = Color.FromArgb(68, 101, 147);
            menuSettingsLbR2ControlR.Location = new Point(276, 11);
            menuSettingsLbR2ControlR.Margin = new Padding(0);
            menuSettingsLbR2ControlR.Name = "menuSettingsLbR2ControlR";
            menuSettingsLbR2ControlR.Padding = new Padding(5);
            menuSettingsLbR2ControlR.Size = new Size(62, 58);
            menuSettingsLbR2ControlR.TabIndex = 2;
            menuSettingsLbR2ControlR.Text = "▶";
            menuSettingsLbR2ControlR.Click += buttonClicked;
            // 
            // menuSettingsLbL3
            // 
            menuSettingsLbL3.Anchor = AnchorStyles.Right;
            menuSettingsLbL3.AutoSize = true;
            menuSettingsLbL3.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsLbL3.ForeColor = Color.FromArgb(68, 101, 147);
            menuSettingsLbL3.Location = new Point(387, 176);
            menuSettingsLbL3.Margin = new Padding(0, 0, 100, 0);
            menuSettingsLbL3.Name = "menuSettingsLbL3";
            menuSettingsLbL3.Size = new Size(278, 48);
            menuSettingsLbL3.TabIndex = 3;
            menuSettingsLbL3.Text = "Vstupní zařízení";
            // 
            // menuSettingsLbR3Container
            // 
            menuSettingsLbR3Container.ColumnCount = 4;
            menuSettingsLbR3Container.ColumnStyles.Add(new ColumnStyle());
            menuSettingsLbR3Container.ColumnStyles.Add(new ColumnStyle());
            menuSettingsLbR3Container.ColumnStyles.Add(new ColumnStyle());
            menuSettingsLbR3Container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            menuSettingsLbR3Container.Controls.Add(menuSettingsLbR3Input, 0, 0);
            menuSettingsLbR3Container.Controls.Add(menuSettingsLbR3ControlL, 0, 0);
            menuSettingsLbR3Container.Controls.Add(menuSettingsLbR3ControlR, 2, 0);
            menuSettingsLbR3Container.Dock = DockStyle.Fill;
            menuSettingsLbR3Container.Location = new Point(765, 160);
            menuSettingsLbR3Container.Margin = new Padding(0);
            menuSettingsLbR3Container.Name = "menuSettingsLbR3Container";
            menuSettingsLbR3Container.RowCount = 1;
            menuSettingsLbR3Container.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            menuSettingsLbR3Container.Size = new Size(765, 80);
            menuSettingsLbR3Container.TabIndex = 7;
            // 
            // menuSettingsLbR3Input
            // 
            menuSettingsLbR3Input.Anchor = AnchorStyles.Right;
            menuSettingsLbR3Input.AutoSize = true;
            menuSettingsLbR3Input.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsLbR3Input.ForeColor = Color.FromArgb(68, 101, 147);
            menuSettingsLbR3Input.Location = new Point(162, 16);
            menuSettingsLbR3Input.Margin = new Padding(0);
            menuSettingsLbR3Input.Name = "menuSettingsLbR3Input";
            menuSettingsLbR3Input.Size = new Size(189, 48);
            menuSettingsLbR3Input.TabIndex = 1;
            menuSettingsLbR3Input.Text = "Klávesnice";
            // 
            // menuSettingsLbR3ControlL
            // 
            menuSettingsLbR3ControlL.Anchor = AnchorStyles.Right;
            menuSettingsLbR3ControlL.AutoSize = true;
            menuSettingsLbR3ControlL.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsLbR3ControlL.ForeColor = Color.FromArgb(130, 160, 200);
            menuSettingsLbR3ControlL.Location = new Point(100, 11);
            menuSettingsLbR3ControlL.Margin = new Padding(100, 0, 0, 0);
            menuSettingsLbR3ControlL.Name = "menuSettingsLbR3ControlL";
            menuSettingsLbR3ControlL.Padding = new Padding(5);
            menuSettingsLbR3ControlL.Size = new Size(62, 58);
            menuSettingsLbR3ControlL.TabIndex = 0;
            menuSettingsLbR3ControlL.Text = "◀";
            menuSettingsLbR3ControlL.Click += buttonClicked;
            // 
            // menuSettingsLbR3ControlR
            // 
            menuSettingsLbR3ControlR.Anchor = AnchorStyles.Right;
            menuSettingsLbR3ControlR.AutoSize = true;
            menuSettingsLbR3ControlR.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsLbR3ControlR.ForeColor = Color.FromArgb(68, 101, 147);
            menuSettingsLbR3ControlR.Location = new Point(351, 11);
            menuSettingsLbR3ControlR.Margin = new Padding(0);
            menuSettingsLbR3ControlR.Name = "menuSettingsLbR3ControlR";
            menuSettingsLbR3ControlR.Padding = new Padding(5);
            menuSettingsLbR3ControlR.Size = new Size(62, 58);
            menuSettingsLbR3ControlR.TabIndex = 2;
            menuSettingsLbR3ControlR.Text = "▶";
            menuSettingsLbR3ControlR.Click += buttonClicked;
            // 
            // menuSettingsBtBack
            // 
            menuSettingsBtBack.Anchor = AnchorStyles.Top;
            menuSettingsBtBack.BackColor = Color.White;
            menuSettingsBtBack.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            menuSettingsBtBack.ForeColor = Color.FromArgb(68, 101, 147);
            menuSettingsBtBack.Location = new Point(597, 693);
            menuSettingsBtBack.Name = "menuSettingsBtBack";
            menuSettingsBtBack.Size = new Size(342, 67);
            menuSettingsBtBack.TabIndex = 5;
            menuSettingsBtBack.Text = "ZPĚT";
            menuSettingsBtBack.UseVisualStyleBackColor = false;
            menuSettingsBtBack.Click += buttonClicked;
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
            Controls.Add(menuSettingsContainer);
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
            menuMainTableLP.ResumeLayout(false);
            menuMainTableLP.PerformLayout();
            menuEscapeContainer.ResumeLayout(false);
            menuEscapeTableLP.ResumeLayout(false);
            menuEscapeTableLP.PerformLayout();
            menuControlsContainer.ResumeLayout(false);
            menuControlsTableLP.ResumeLayout(false);
            menuControlsTableLP.PerformLayout();
            menuControlsTableLPControls.ResumeLayout(false);
            menuControlsTableLPControls.PerformLayout();
            menuSettingsContainer.ResumeLayout(false);
            menuSettignsTableLP.ResumeLayout(false);
            menuSettignsTableLP.PerformLayout();
            menuSettingsTableLPSettings.ResumeLayout(false);
            menuSettingsTableLPSettings.PerformLayout();
            menuSettingsLbR1Container.ResumeLayout(false);
            menuSettingsLbR1Container.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)menuSettingsTrackR1).EndInit();
            menuSettingsLbR2Container.ResumeLayout(false);
            menuSettingsLbR2Container.PerformLayout();
            menuSettingsLbR3Container.ResumeLayout(false);
            menuSettingsLbR3Container.PerformLayout();
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
        private TableLayoutPanel menuMainTableLP;
        private Label menuMainLbTitle;
        private Button menuMainBtPlay;
        private Button menuMainBtSettings;
        private Button menuMainBtClose;
        private Panel menuEscapeContainer;
        private Label menuEscapeLbTitle;
        private TableLayoutPanel menuEscapeTableLP;
        private Button menuEscapeBtContinue;
        private Button menuEscapeBtMainMenu;
        private Label menuMainLbSubtitle;
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
        private Label menuMainLbAuthor;
        private Label menuControlsLbR5;
        private Label menuControlsLbL5;
        private Panel menuSettingsContainer;
        private TableLayoutPanel menuSettignsTableLP;
        private Label menuSettingsLbTitle;
        private Button menuSettingsBtBack;
        private Button menuEscapeBtSettings;
        private TableLayoutPanel menuSettingsTableLPSettings;
        private Label menuSettingsLbL2;
        private TableLayoutPanel menuSettingsLbR2Container;
        private Label menuSettingsLbR2ControlR;
        private Label menuSettingsLbR2Language;
        private Label menuSettingsLbR2ControlL;
        private Label menuSettingsLbL3;
        private Label menuSettingsLbL1;
        private TableLayoutPanel menuSettingsLbR1Container;
        private TrackBar menuSettingsTrackR1;
        private Label menuSettingsLbVolumeR1;
        private TableLayoutPanel menuSettingsLbR3Container;
        private Label menuSettingsLbR3Input;
        private Label menuSettingsLbR3ControlL;
        private Label menuSettingsLbR3ControlR;
    }
}