using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Security.Cryptography;
using static System.Formats.Asn1.AsnWriter;

namespace Lumberjack
{
    public enum GameState
    {
        HomeScreen,
        InGame
    }

    public class LumberjackGame : Game
    {
        #region Fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameState _currentGameState;

        // The font used to display UI elements  
        SpriteFont font;

        // Used to display the static Images   
        Texture2D foreground;
        Texture2D background;
        Texture2D homeScreenBackground;
        Texture2D controlsScreenBackground;
        Texture2D gameOverScreenBackground;
        Texture2D timer;
        Texture2D timerBg;

        //Represents the player  
        Player player;

        Texture2D playerSprite;

        // Represents the Bird
        Bird bird;

        // Represents the Tree class
        Tree tree;

        IGameState homeScreenState;
        IGameState runningGameState;

        #endregion

        public LumberjackGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 504;
            _graphics.PreferredBackBufferHeight = 896;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ShowHomeScreen();

            // Initialize the player class
            player = new Player();

            // Initialize the tree class
            tree = new Tree();

            // Initialize the Bird class
            bird = new Bird();

            base.Initialize();

            EventManager.StartGame.AddListener(StartGame);
            EventManager.ExitGame.AddListener(Exit);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            homeScreenState = new HomeScreenState(homeScreenBackground, playerSprite, controlsScreenBackground, font);
            runningGameState = new RunningGameState(foreground, background, gameOverScreenBackground, player, bird, tree, timer, timerBg, font);
        }

        protected override void LoadContent()
        {

            // Load the score font   
            font = Content.Load<SpriteFont>("Graphics\\gameFont");

            // Load the timer textures
            timer = Content.Load<Texture2D>("Graphics\\LoadingSprite");
            timerBg = Content.Load<Texture2D>("Graphics\\LoadingBgSprite");

            // Load the background resources
            background = Content.Load<Texture2D>("Graphics/background");
            foreground = Content.Load<Texture2D>("Graphics/foreground");
            homeScreenBackground = Content.Load<Texture2D>("Graphics/HomeScreenBackground");
            controlsScreenBackground = Content.Load<Texture2D>("Graphics/ControlsScreen");
            gameOverScreenBackground = Content.Load<Texture2D>("Graphics/GameOverScreen");

            // Load the tree 
            Vector2 treePosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
            GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            tree.Initialize(Content.Load<Texture2D>("Graphics\\bark"), Content.Load<Texture2D>("Graphics\\BranchLeft"),
                Content.Load<Texture2D>("Graphics\\BranchRight"), treePosition);


            // Load the player resources   
            Animation playerAnimCuttingLeft = new Animation();
            Animation playerAnimeIdleLeft = new Animation();
            Animation playerAnimCuttingRight = new Animation();
            Animation playerAnimIdleRight = new Animation();
            Texture2D playerTexture;
            // Left Side Player Cutting Anim
            playerTexture = Content.Load<Texture2D>("Graphics\\Woodcutter_attack2");
            playerAnimCuttingLeft.Initialize(playerTexture, Vector2.Zero, 48, 48, 6, 30, Color.White, 2.5f, false);
            // Left Side Player Idle Anim
            playerTexture = Content.Load<Texture2D>("Graphics\\Woodcutter_Left");
            playerAnimeIdleLeft.Initialize(playerTexture, Vector2.Zero, 48, 48, 1, 1, Color.White, 2.5f, true);
            // Right Side Player Cutting Anim
            playerTexture = Content.Load<Texture2D>("Graphics\\Woodcutter_attack1");
            playerAnimCuttingRight.Initialize(playerTexture, Vector2.Zero, 48, 48, 6, 30, Color.White, 2.5f, false);
            // Right Side Player Idle Anim
            playerTexture = Content.Load<Texture2D>("Graphics\\Woodcutter_Right");
            playerAnimIdleRight.Initialize(playerTexture, Vector2.Zero, 48, 48, 1, 1, Color.White, 2.5f, true);

            playerSprite = Content.Load<Texture2D>("Graphics\\Woodcutter_Cursor");

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
            GraphicsDevice.Viewport.TitleSafeArea.Y + 2 * GraphicsDevice.Viewport.TitleSafeArea.Height / 3 );

            player.Initialize(playerAnimCuttingLeft, playerAnimeIdleLeft, playerAnimCuttingRight, playerAnimIdleRight, playerPosition);

            // Load the bird
            Texture2D birdTexture;
            Animation birdAnim = new Animation();
            birdTexture = Content.Load<Texture2D>("Graphics\\Bird");
            birdAnim.Initialize(birdTexture, Vector2.Zero, 32, 32, 6, 70, Color.White, 1.5f, true);
            var birdPos = Vector2.Zero;
            bird.Initialize(birdAnim, birdPos);

            // Load the Egg
            Texture2D eggTexture;
            eggTexture = Content.Load<Texture2D>("Graphics\\Egg");
            Egg.Instance.Initialize(eggTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (_currentGameState == GameState.HomeScreen)
                homeScreenState.Update(gameTime);
            else if (_currentGameState == GameState.InGame)
                runningGameState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start drawing  
            _spriteBatch.Begin();

            if (_currentGameState == GameState.HomeScreen)
            {
                homeScreenState.Draw(_spriteBatch);
            }
            else if(_currentGameState == GameState.InGame)
            {
                runningGameState.Draw(_spriteBatch);
            }

            // Stop drawing  
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void StartGame()
        {
            _currentGameState = GameState.InGame;
        }

        private void ShowHomeScreen()
        {
            _currentGameState = GameState.HomeScreen;
        }
    }
}