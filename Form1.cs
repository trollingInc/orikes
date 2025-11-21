using System.Collections.Generic;
using System.Media;
using System.Reflection;

namespace orikes
{
    public partial class Form1 : Form
    {
        Player player;
        Boss1 boss1;

        // Parry sound
        protected static SoundPlayer ParrySound = new SoundPlayer(@"../../../sfx/ParrySound.wav");

        // Starts the game.
        void Play(object sender, EventArgs e)
        {
            player = new Player(playerParryTimer, playerParryCdTimer, playerAtkCdTimer, playerHpBar, mainCharSprite, GameOver);
            boss1 = new Boss1(EnemyCombo, EnemyInitiateAttack, EnemyParryTimer, boss1HpBar, boss1Sprite, GameOver);
            gameBoss1.Visible = true;
            gameBoss1.Enabled = true;
            GameOverMessage.Visible = false;
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(player.HandlePlayerAction);
            mainCharSprite.Image = player.animations["idle"];
            boss1Sprite.Image = boss1.animations["idle"];
        }

        // Ends the game. Returns to start screen with a result for the game as well as a play again button.
        public void GameOver(bool winner)
        {
            gameBoss1.Visible = false;
            gameBoss1.Enabled = false;

            // remove all leftover timers
            EnemyCombo.Enabled = false;
            EnemyInitiateAttack.Enabled = false;
            EnemyParryTimer.Enabled = false;
            playerAnimChangerTimer.Enabled = false;
            playerAtkCdTimer.Enabled = false;
            playerParryCdTimer.Enabled = false;
            playerParryTimer.Enabled = false;
            // ----

            player = null;
            boss1 = null;
            if (winner)
            {
                GameOverMessage.Text = "YOU WON!";
                GameOverMessage.ForeColor = Color.Green;
            }
            else
            {
                GameOverMessage.Text = "YOU LOST!";
                GameOverMessage.ForeColor = Color.Red;
            }
            GameOverMessage.Visible = true;
            PlayBtn.Text = "Play again";
        }
        public Form1()
        {
            InitializeComponent();
        }

        // Every entity (the player and enemy) need to have these functions
        interface IEntity
        {
            void TryToTakeDamage(int damage);
            void DealDamage(int damage, Entity target);
        }

        // Every entity will have an hp, animations and a Game over call back function it executes when it dies.
        abstract class Entity : IEntity
        {
            public int hp;
            public Dictionary<string, Image> animations;
            public Action<bool> GameOverCallBack;
            abstract public void TryToTakeDamage(int damage);
            abstract public void DealDamage(int damage, Entity target);
        }

        // The player class has all functionallity related to you, the player.
        // It includes animations, attacks, parry and of course input handling.
        private class Player : Entity
        {
            static int maxPlayerHp = 130;
            public int damage = 1;
            public bool isParrying;
            public bool canParry;
            public bool isAttacking;
            // milliseconds
            int parryDuration = 300;
            int parryCd = 300;

            // Forms UI stuff
            System.Windows.Forms.Timer parryTimer;
            System.Windows.Forms.Timer parryCdTimer;
            System.Windows.Forms.Timer attackCdTimer;
            ProgressBar UIhpBar;
            PictureBox sprite;
            // -------------------

            // tuple is (milliseconds delay, animation name)
            public (int, string)[] attacks;
            // index of 'attacks' array
            public int attackProgress;

            // constructor gets the needed timers and UI elements and also sets some default values.
            public Player(System.Windows.Forms.Timer parryTimer, System.Windows.Forms.Timer parryCdTimer, System.Windows.Forms.Timer attackCdTimer, ProgressBar UIhpBar, PictureBox sprite, Action<bool> gameOverCallback)
            {
                hp = maxPlayerHp;
                isParrying = false;
                isAttacking = false;
                canParry = true;

                this.GameOverCallBack = gameOverCallback;

                this.sprite = sprite;

                this.parryTimer = parryTimer;
                this.parryTimer.Interval = parryDuration;

                this.parryCdTimer = parryCdTimer;
                this.parryCdTimer.Interval = parryCd;

                this.attackCdTimer = attackCdTimer;
                this.UIhpBar = UIhpBar;
                this.UIhpBar.Maximum = maxPlayerHp;
                UIhpBar.Value = hp;

                animations = new Dictionary<string, Image>();
                animations.Add("idle", Properties.Resources.mc_idle);
                animations.Add("parrying", Properties.Resources.mc_parry);
                animations.Add("attackFrame1", Properties.Resources.mc_atkFrame1);
                animations.Add("attackFrame2", Properties.Resources.mc_atkFrame2);
                animations.Add("attackFrameEnd", Properties.Resources.mc_atkFrameEnd);

                attacks =
                [
                    (250, "attackFrame1"),
                    (120, "attackFrame2"),
                    (380, "attackFrameEnd")
                ];
            }

            // Function to take damage. It's called "TryTo..." because if the player is parrying, they will not take damage.
            // If health points (hp) falls below 0, the game ends and you lose.
            public override void TryToTakeDamage(int damage)
            {
                // if player has timed the parry correctly, don't take damage;
                if (isParrying)
                {
                    ParrySound.Stop();
                    ParrySound.Play();
                    canParry = true;
                    isParrying = false;
                    parryTimer.Enabled = false;
                    parryCdTimer.Enabled = false;
                    sprite.Image = animations["idle"];
                    // maybe play sound here
                    return;
                }

                hp -= damage;
                if (hp <= 0)
                {
                    UIhpBar.Value = 0;
                    GameOverCallBack(false);
                }
                else
                {
                    UIhpBar.Value = hp;
                }
            }

            // Deal damage to the target entity, in this case just the one enemy there is.
            public override void DealDamage(int damage, Entity target)
            {
                target.TryToTakeDamage(damage);
            }

            // Handles inputs for attack and parry. Attacking is done with the 'A' key and parrying is done with the 'B' key.
            public void HandlePlayerAction(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.A && !isAttacking)
                {
                    isAttacking = true;
                    canParry = false;
                    isParrying = false;
                    parryTimer.Enabled = false;
                    parryCdTimer.Enabled = false;
                    attackCdTimer.Interval = attacks[0].Item1;
                    attackProgress = 1;
                    sprite.Image = animations[attacks[0].Item2];
                    attackCdTimer.Enabled = true;
                }
                if (e.KeyCode == Keys.S && canParry)
                {
                    isParrying = true;
                    canParry = false;
                    sprite.Image = animations["parrying"];
                    parryTimer.Enabled = true;
                }
            }
        }

        // This timer is responsible for deciding when the enemy will attack as well as which attack it will use.
        private void EnemyInitiateAttack_Tick(object sender, EventArgs e)
        {
            if (boss1 == null) { return; }
            boss1.isAttacking = true;
            boss1.attackToUse = boss1.attackToUseGenerator.Next(0, 2);
            boss1.attackProgress = 1;
            boss1Sprite.Image = boss1.animations[boss1.attacks[boss1.attackToUse, 0].Item2];
            EnemyCombo.Interval = boss1.attacks[boss1.attackToUse, 0].Item1;
            EnemyCombo.Enabled = true;
            EnemyInitiateAttack.Enabled = false;
        }

        // This timer is responsible for the enemies attack combo pattern. This includes changing animations at the correct times and also dealing damage to the player.
        private void EnemyCombo_Tick(object sender, EventArgs e)
        {
            if (boss1 == null) { return; }
            // heavy attack
            if (boss1.attackToUse == 0)
            {
                if (boss1.attackProgress == boss1.attacks.GetLength(1) - 1)
                {
                    boss1.DealDamage(boss1.attacksDamage[boss1.attackToUse], player);
                }
            }
            else
            {
                // fast attack. different logic as this attack hits twice
                if (boss1.attackProgress == boss1.attacks.GetLength(1) - 1 || boss1.attackProgress == boss1.attacks.GetLength(1) - 2)
                {
                    boss1.DealDamage(boss1.attacksDamage[boss1.attackToUse], player);
                }
            }
            if (player == null || player.hp <= 0) { return; }

            if (boss1.attackProgress >= boss1.attacks.GetLength(1))
            {
                boss1.isAttacking = false;
                boss1Sprite.Image = boss1.animations["idle"];
                EnemyInitiateAttack.Interval = boss1.nextAttackInterval.Next(boss1.atkIntervalMin, boss1.atkIntervalMax);
                EnemyInitiateAttack.Enabled = true;
                EnemyCombo.Enabled = false;
                return;
            }
            boss1Sprite.Image = boss1.animations[boss1.attacks[boss1.attackToUse, boss1.attackProgress].Item2];
            EnemyCombo.Interval = boss1.attacks[boss1.attackToUse, boss1.attackProgress].Item1;
            boss1.attackProgress++;
        }

        // This timer is responsible stopping the parry mechanic on the player. Basically, you need to time the parry with the enemy's attack, no just spam the parry button.
        private void PlayerParryTimer_Tick(object sender, EventArgs e)
        {
            if (player == null) { return; }
            player.isParrying = false;
            mainCharSprite.Image = player.animations["idle"];
            playerParryCdTimer.Enabled = true;
            playerParryTimer.Enabled = false;
        }

        // This removes the parry cooldown. Again, don't spam the parry button.
        private void playerParryCdTimer_Tick(object sender, EventArgs e)
        {
            if (player == null) { return; }
            player.canParry = true;
            playerParryCdTimer.Enabled = false;
        }

        // This is responsible for the player's attack. Includes animations and dealing damage as well as disabling the parry. Be careful of when you attack.
        private void playerAtkCdTimer_Tick(object sender, EventArgs e)
        {
            if (player == null) { return; }
            if (player.attackProgress == player.attacks.Length - 1)
            {
                player.DealDamage(player.damage, boss1);
            }
            else if (player.attackProgress >= player.attacks.Length)
            {
                player.canParry = true;
                player.isAttacking = false;
                mainCharSprite.Image = player.animations["idle"];
                playerAtkCdTimer.Enabled = false;
                return;
            }
            if (player == null || boss1.hp <= 0) { return; }
            mainCharSprite.Image = player.animations[player.attacks[player.attackProgress].Item2];
            playerAtkCdTimer.Interval = player.attacks[player.attackProgress].Item1;
            player.attackProgress++;
        }

        // Boss class is responsible for the enemy
        private class Boss1 : Entity
        {
            // if you want to decrease the hp/difficulty, use this variable. It will cause the boss to spawn with less hp, though his hp bar will not resize to match this value
            static int maxBoss1Hp = 20;

            // miliseconds where the boss attacks once at least every 2.7 seconds or but could hit after 1,65 second at fastest
            public int atkIntervalMax = 2700;
            public int atkIntervalMin = 1650;
            // Every attack is after a random duration to keep the game more dynamic
            public Random nextAttackInterval;
            // If the boss is not attacking there's a 25% that he will parry the attack and counter attack
            Random shouldParry;
            // Randomly choose one out of 2 possible attacks. They come from the "attacks" array.
            public Random attackToUseGenerator;
            public int attackToUse;

            public bool isAttacking;

            System.Windows.Forms.Timer atkComboTimer;
            System.Windows.Forms.Timer atkInitTimer;
            System.Windows.Forms.Timer counterAtkTimer;
            ProgressBar UIhpBar;
            PictureBox sprite;

            public int attackProgress;
            public (int, string)[,] attacks;
            // Counter attack the boss does after parrying the player.
            public (int, string)[] counterAttack;
            public int counterAttackDamage = 45;
            public bool isCounterAttacking = false;

            // How much damage each attack does. index 0 is heavy attack, index 1 is for both strikes in the fast attack (so 20 would mean a total of 40 damage)
            public int[] attacksDamage;

            // The code itself is different, but the functionality is basically the same as the Player's constructor
            public Boss1(System.Windows.Forms.Timer atkComboTimer, System.Windows.Forms.Timer atkInitTimer, System.Windows.Forms.Timer counterAtkTimer, ProgressBar UIhpBar, PictureBox sprite, Action<bool> gameOverCallback)
            {
                this.GameOverCallBack = gameOverCallback;

                this.sprite = sprite;
                this.atkComboTimer = atkComboTimer;
                this.atkInitTimer = atkInitTimer;
                this.counterAtkTimer = counterAtkTimer;
                this.UIhpBar = UIhpBar;
                hp = maxBoss1Hp;
                this.UIhpBar.Value = hp;

                nextAttackInterval = new Random();
                shouldParry = new Random();
                attackToUseGenerator = new Random();

                isAttacking = false;

                animations = new Dictionary<string, Image>();
                animations.Add("idle", Properties.Resources.boss1_idle);
                animations.Add("parrying", Properties.Resources.Boss1_parry);
                animations.Add("counterAtk", Properties.Resources.boss1_parryCounterAtk);
                animations.Add("heavyAtkFrame1", Properties.Resources.boss1_heavyDelayAtk_frame1);
                animations.Add("heavyAtkFrame2", Properties.Resources.boss1_heavyDelayAtk_frame2);
                animations.Add("heavyAtkFrameEnd", Properties.Resources.boss1_heavyDelayAtkFrameEnd);
                animations.Add("fastAtkFrame1", Properties.Resources.boss1_doubleSwingAtk_frame1);
                animations.Add("fastAtkFrame2", Properties.Resources.boss1_doubleSwingAtk_frame2);
                animations.Add("fastAtkFrameEnd", Properties.Resources.boss1_doubleSwingAtk_frameEnd);

                attacks = new (int, string)[,]
                {
                    { (1100, "heavyAtkFrame1"), (90, "heavyAtkFrame2"), (400, "heavyAtkFrameEnd") },
                    { (320, "fastAtkFrame1"), (160, "fastAtkFrame2"), (160, "fastAtkFrameEnd") }
                };

                counterAttack = [(600, "parrying"), (400, "counterAtk")];

                attacksDamage = [64, 20];

                this.atkInitTimer.Interval = nextAttackInterval.Next(atkIntervalMin, atkIntervalMax);
                this.atkInitTimer.Enabled = true;
            }
            // Same as player
            public override void DealDamage(int damage, Entity target)
            {
                target.TryToTakeDamage(damage);
            }

            // Same as player except parrying is done on random occaisions. 25% chance to parry. If the enemy parries, it will also counter attack.
            public override void TryToTakeDamage(int damage)
            {
                if (!isAttacking && shouldParry.Next(1, 5) == 1)
                {
                    atkInitTimer.Enabled = false;
                    ParrySound.Stop();
                    ParrySound.Play();
                    sprite.Image = animations["parrying"];
                    counterAtkTimer.Interval = counterAttack[0].Item1;
                    isCounterAttacking = false;
                    counterAtkTimer.Enabled = true;
                    return;
                }

                hp -= damage;
                if (hp <= 0)
                {
                    UIhpBar.Value = 0;
                    GameOverCallBack(true);
                }
                else
                {
                    UIhpBar.Value = hp;
                }
            }
        }

        // This is responsible for the parry counter attack mentioned above. Changes animations and also deals damage.
        private void EnemyParryTimer_Tick(object sender, EventArgs e)
        {
            if (boss1 == null) { return; }
            if (!boss1.isCounterAttacking)
            {
                boss1.isCounterAttacking = true;
                EnemyParryTimer.Interval = boss1.counterAttack[1].Item1;
                boss1Sprite.Image = boss1.animations["counterAtk"];
                boss1.DealDamage(boss1.counterAttackDamage, player);
            }
            else
            {
                boss1Sprite.Image = boss1.animations["idle"];
                EnemyInitiateAttack.Interval = (boss1.nextAttackInterval.Next(boss1.atkIntervalMin, boss1.atkIntervalMin));
                EnemyInitiateAttack.Enabled = true;
                EnemyParryTimer.Enabled = false;
            }
        }
    }
}
