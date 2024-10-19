using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Lumberjack
{
    class RunningGameState : IGameState
    {
        // Used to display the static Images   
        Texture2D foreground;
        Texture2D background;
        Texture2D gameOverScreen;

        // Represent the Timer class
        Timer timer;

        //Represents the player  
        Player player;

        // Represents the Bird
        Bird bird;

        // Represents the Tree class
        Tree tree;

        SpriteFont font;

        // Collision Manager for checking Collisions
        CollisionManager collisionManager;

        // Score Manager class for mainting score
        ScoreManager scoreManager;

        //Command Manager for keyBinds
        CommandManager commandManager;

        bool gameOver = false;

        private int birdSpawn = 1;
        private int birdSpawnTimer = 5;

        //Time bonus for each bark cut
        private float timeAdd = 1;

        public RunningGameState(Texture2D foreground, Texture2D background, Texture2D gameOverTexture, Player player, 
            Bird bird, Tree tree, Texture2D timerTexture, Texture2D timerBgTexture, SpriteFont font)
        {
            this.foreground = foreground;
            this.background = background;
            this.player = player;
            this.bird = bird;
            this.tree = tree;
            this.scoreManager = ScoreManager.Instance;
            this.commandManager = new CommandManager();
            this.collisionManager = new CollisionManager();
            this.timer = new Timer(timerTexture, timerBgTexture);
            this.font = font;
            gameOverScreen = gameOverTexture;

            Initialize();

            EventManager.RestartGame.AddListener(ResetGame);
        }

        public void Initialize()
        {
            SubscribeToEvents();

            InitializeBindings();

            InitializeCollidableObjects();
        }

        private void InitializeBindings()
        {
            commandManager.AddKeyboardBinding(Keys.Escape, ExitGame);
            commandManager.AddKeyboardBinding(Keys.Left, MoveLeft);
            commandManager.AddKeyboardBinding(Keys.Right, MoveRight);
            commandManager.AddKeyboardBinding(Keys.R, RestartGame);
        }

        public void Update(GameTime gameTime)
        {
            // Update the collision manager
            collisionManager.Update();

            // Update the command manager
            commandManager.Update();

            if (timer.TimerValue <= 0)
                EventManager.PlayerDeath.Execute();

            timer.Update(gameTime);

            player.Update(gameTime);

            bird.Update(gameTime);

            if (gameTime.TotalGameTime.TotalSeconds > birdSpawn)
            {
                BirdPowerUp();
                birdSpawn += birdSpawnTimer;
            }

            Egg.Instance.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the Main Background Texture  
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            // Draw the Egg
            Egg.Instance.Draw(spriteBatch);

            //Draw the forground Texture
            spriteBatch.Draw(foreground, Vector2.Zero, Color.White);

            // Draw the Tree
            tree.Draw(spriteBatch);

            // Draw the Bird
            bird.Draw(spriteBatch);

            // Draw the Player  
            if (player.Active)
            {
                // Player Collision Box
                var playerRectangle = new Rectangle((int)player.Position.X - 60, (int)player.Position.Y - 50, 120, 120);
                //_spriteBatch.Draw(pixelTexture, playerRectangle, Color.Red);
                player.Draw(spriteBatch);
            }

            // Draw the Timer
            timer.Draw(spriteBatch);

            // Draw Gameover
            var scorePos = new Vector2(250, 200);
            if (gameOver)
            {
                spriteBatch.Draw(gameOverScreen, new Vector2(100, 250), Color.White);
                scorePos = new Vector2(248, 420);
            }

            // Draw the score  
            spriteBatch.DrawString(font, Convert.ToString(ScoreManager.Instance.Score),
               scorePos, Color.White);
        }

        private void BirdPowerUp()
        {
            var pos = Vector2.Zero;
            pos.X = -50;
            var playerPos = player.Position;
            if (player.PlayerSide == ePlayerSide.LEFT)
            {
                playerPos.X -= 50;
            }
            bird.MoveBird(pos, playerPos);
        }
        private void InitializeCollidableObjects()
        {
            collisionManager.AddCollidable(player);
            collisionManager.AddCollidable(Egg.Instance);
        }

        private void CheckCollisionsWithTree(ePlayerSide playerSide)
        {

            if ((tree.barksList.Peek().BarkSide == eBarkSide.Left && playerSide == ePlayerSide.LEFT) ||
                (tree.barksList.Peek().BarkSide == eBarkSide.Right && playerSide == ePlayerSide.RIGHT))
            {
                EventManager.PlayerDeath.Execute();
            }
        }

        private void GameOver() => gameOver = true;
        private void ResetGame() => gameOver = false;
        
        #region Game Actions
        public void MoveLeft(eButtonState buttonState, Vector2 amount)
        {
            if (player.Active && buttonState == eButtonState.DOWN)
            {
                tree.DestroyBark(ePlayerSide.LEFT);
                player.MoveLeft();
                CheckCollisionsWithTree(ePlayerSide.LEFT);
                if (player.Active)
                {
                    EventManager.CutBark.Execute();
                    timer.AddTime(timeAdd);
                }
            }
        }
        public void MoveRight(eButtonState buttonState, Vector2 amount)
        {
            if (player.Active && buttonState == eButtonState.DOWN)
            {
                tree.DestroyBark(ePlayerSide.RIGHT);
                player.MoveRight();
                CheckCollisionsWithTree(ePlayerSide.RIGHT);
                if (player.Active)
                {
                    EventManager.CutBark.Execute();
                    timer.AddTime(timeAdd);
                }
            }
        }
        public void RestartGame(eButtonState buttonState, Vector2 amount)
        {
            if(gameOver && buttonState == eButtonState.DOWN)
                EventManager.RestartGame.Execute();
        }
        public void ExitGame(eButtonState buttonState, Vector2 amout) => EventManager.ExitGame.Execute();
        #endregion

        private void SubscribeToEvents()
        {
            // Subscribe to Player Death Event
            EventManager.PlayerDeath.AddListener(player.DeactivatePlayer);
            EventManager.PlayerDeath.AddListener(GameOver);

            // Subscribe to Collect Egg Event
            EventManager.CollectEgg.AddListener(Egg.Instance.DeactivateEgg);
            EventManager.CollectEgg.AddListener(scoreManager.CatchEgg);

            // Subscribe to Cut Tree Event
            EventManager.CutBark.AddListener(scoreManager.CutTree);
        }
    }
}
