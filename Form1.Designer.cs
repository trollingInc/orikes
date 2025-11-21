namespace orikes
{
    partial class Form1
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
            EnemyInitiateAttack = new System.Windows.Forms.Timer(components);
            EnemyCombo = new System.Windows.Forms.Timer(components);
            StartMenu = new Panel();
            GameOverMessage = new Label();
            gameBoss1 = new Panel();
            boss1HpBar = new ProgressBar();
            playerHpBar = new ProgressBar();
            boss1Sprite = new PictureBox();
            mainCharSprite = new PictureBox();
            PlayBtn = new Button();
            playerParryTimer = new System.Windows.Forms.Timer(components);
            playerAtkCdTimer = new System.Windows.Forms.Timer(components);
            playerAnimChangerTimer = new System.Windows.Forms.Timer(components);
            playerParryCdTimer = new System.Windows.Forms.Timer(components);
            EnemyParryTimer = new System.Windows.Forms.Timer(components);
            StartMenu.SuspendLayout();
            gameBoss1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)boss1Sprite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mainCharSprite).BeginInit();
            SuspendLayout();
            // 
            // EnemyInitiateAttack
            // 
            EnemyInitiateAttack.Tick += EnemyInitiateAttack_Tick;
            // 
            // EnemyCombo
            // 
            EnemyCombo.Tick += EnemyCombo_Tick;
            // 
            // StartMenu
            // 
            StartMenu.Controls.Add(GameOverMessage);
            StartMenu.Controls.Add(gameBoss1);
            StartMenu.Controls.Add(PlayBtn);
            StartMenu.Location = new Point(0, 0);
            StartMenu.Name = "StartMenu";
            StartMenu.Size = new Size(1106, 674);
            StartMenu.TabIndex = 1;
            // 
            // GameOverMessage
            // 
            GameOverMessage.AutoSize = true;
            GameOverMessage.Font = new Font("Segoe UI", 40F);
            GameOverMessage.Location = new Point(350, 131);
            GameOverMessage.Name = "GameOverMessage";
            GameOverMessage.Size = new Size(0, 72);
            GameOverMessage.TabIndex = 2;
            GameOverMessage.Visible = false;
            // 
            // gameBoss1
            // 
            gameBoss1.Controls.Add(boss1HpBar);
            gameBoss1.Controls.Add(playerHpBar);
            gameBoss1.Controls.Add(boss1Sprite);
            gameBoss1.Controls.Add(mainCharSprite);
            gameBoss1.Enabled = false;
            gameBoss1.Location = new Point(0, 0);
            gameBoss1.Name = "gameBoss1";
            gameBoss1.Size = new Size(1106, 674);
            gameBoss1.TabIndex = 1;
            gameBoss1.Visible = false;
            // 
            // boss1HpBar
            // 
            boss1HpBar.BackColor = SystemColors.ControlDark;
            boss1HpBar.ForeColor = Color.FromArgb(192, 0, 0);
            boss1HpBar.Location = new Point(579, 70);
            boss1HpBar.Maximum = 20;
            boss1HpBar.Name = "boss1HpBar";
            boss1HpBar.Size = new Size(294, 30);
            boss1HpBar.Step = 1;
            boss1HpBar.Style = ProgressBarStyle.Continuous;
            boss1HpBar.TabIndex = 3;
            boss1HpBar.Value = 20;
            // 
            // playerHpBar
            // 
            playerHpBar.BackColor = Color.LightGray;
            playerHpBar.ForeColor = Color.Lime;
            playerHpBar.Location = new Point(502, 442);
            playerHpBar.Name = "playerHpBar";
            playerHpBar.Size = new Size(294, 30);
            playerHpBar.Step = 1;
            playerHpBar.Style = ProgressBarStyle.Continuous;
            playerHpBar.TabIndex = 2;
            playerHpBar.Value = 100;
            // 
            // boss1Sprite
            // 
            boss1Sprite.Image = Properties.Resources.boss1_idle;
            boss1Sprite.Location = new Point(292, 3);
            boss1Sprite.Name = "boss1Sprite";
            boss1Sprite.Size = new Size(260, 343);
            boss1Sprite.SizeMode = PictureBoxSizeMode.Zoom;
            boss1Sprite.TabIndex = 1;
            boss1Sprite.TabStop = false;
            // 
            // mainCharSprite
            // 
            mainCharSprite.Image = Properties.Resources.mc_idle;
            mainCharSprite.Location = new Point(124, 342);
            mainCharSprite.Name = "mainCharSprite";
            mainCharSprite.Size = new Size(372, 332);
            mainCharSprite.SizeMode = PictureBoxSizeMode.Zoom;
            mainCharSprite.TabIndex = 0;
            mainCharSprite.TabStop = false;
            // 
            // PlayBtn
            // 
            PlayBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PlayBtn.BackColor = SystemColors.Control;
            PlayBtn.BackgroundImageLayout = ImageLayout.None;
            PlayBtn.FlatAppearance.BorderColor = Color.Black;
            PlayBtn.FlatAppearance.MouseDownBackColor = Color.Gray;
            PlayBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
            PlayBtn.Font = new Font("Segoe UI", 22F);
            PlayBtn.Location = new Point(376, 245);
            PlayBtn.Name = "PlayBtn";
            PlayBtn.Size = new Size(356, 86);
            PlayBtn.TabIndex = 0;
            PlayBtn.Text = "Play";
            PlayBtn.UseVisualStyleBackColor = false;
            PlayBtn.Click += Play;
            // 
            // playerParryTimer
            // 
            playerParryTimer.Tick += PlayerParryTimer_Tick;
            // 
            // playerAtkCdTimer
            // 
            playerAtkCdTimer.Tick += playerAtkCdTimer_Tick;
            // 
            // playerParryCdTimer
            // 
            playerParryCdTimer.Tick += playerParryCdTimer_Tick;
            // 
            // EnemyParryTimer
            // 
            EnemyParryTimer.Tick += EnemyParryTimer_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1106, 673);
            Controls.Add(StartMenu);
            Name = "Form1";
            Text = "Form1";
            StartMenu.ResumeLayout(false);
            StartMenu.PerformLayout();
            gameBoss1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)boss1Sprite).EndInit();
            ((System.ComponentModel.ISupportInitialize)mainCharSprite).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer EnemyInitiateAttack;
        private System.Windows.Forms.Timer EnemyCombo;
        private Panel StartMenu;
        private Button PlayBtn;
        private Panel gameBoss1;
        private PictureBox mainCharSprite;
        private PictureBox boss1Sprite;
        private ProgressBar playerHpBar;
        private System.Windows.Forms.Timer playerParryTimer;
        private System.Windows.Forms.Timer playerAtkCdTimer;
        private System.Windows.Forms.Timer playerAnimChangerTimer;
        private System.Windows.Forms.Timer playerParryCdTimer;
        private ProgressBar boss1HpBar;
        private System.Windows.Forms.Timer EnemyParryTimer;
        private Label GameOverMessage;
    }
}
