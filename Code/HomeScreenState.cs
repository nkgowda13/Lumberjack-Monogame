using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Lumberjack
{
    class HomeScreenState : IGameState
    {
        public enum ScreenState
        {
            HomeScreen,
            ControlsScreen
        }

        private ScreenState _currentScreen;

        // Used to display the static Images   
        Texture2D background;
        Texture2D controlsBackground;

        Texture2D player;

        SpriteFont font;

        List<Vector2> menuPositions = new List<Vector2>();
        private int menuPos;

        CommandManager commandManager;

        HighScoreManager highScoreManager;
        string highScore;

        public HomeScreenState(Texture2D background, Texture2D player, Texture2D controlsBackground, SpriteFont font) 
        { 
            _currentScreen = ScreenState.HomeScreen;
            this.background = background;
            this.controlsBackground = controlsBackground;
            this.player = player;
            this.font = font;
            this.commandManager = new CommandManager();
            highScoreManager = HighScoreManager.Instance;
            highScore = Convert.ToString( highScoreManager.HighScore);

            Initialize();

            menuPositions.Add(new Vector2(270, 480));
            menuPositions.Add(new Vector2(270, 600));

            menuPos = 0;
        }

        public void Initialize()
        {
            SubscribeToEvents();

            InitializeBindings();
        }

        private void InitializeBindings()
        {
            commandManager.AddKeyboardBinding(Keys.Escape, ExitGame);
            commandManager.AddKeyboardBinding(Keys.Up, MoveUp);
            commandManager.AddKeyboardBinding(Keys.Down, MoveDown);
            commandManager.AddKeyboardBinding(Keys.Enter, SelectMenu);
            commandManager.AddKeyboardBinding(Keys.B, ShowHome);
        }

        public void Update(GameTime gameTime)
        {
            commandManager.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(_currentScreen == ScreenState.HomeScreen)
            {
                var sourceRect = new Rectangle(0, 0, player.Width, player.Height);
                //Draw the Main Background Texture  
                spriteBatch.Draw(background, Vector2.Zero, Color.White);
                spriteBatch.Draw(player, menuPositions[menuPos], sourceRect, Color.White, 0, Vector2.Zero, 2 * Vector2.One, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, highScore, new Vector2(440, 13), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
            }
            else if(_currentScreen == ScreenState.ControlsScreen)
            {
                spriteBatch.Draw(controlsBackground, Vector2.Zero, Color.White);
            }
        }

        private void SubscribeToEvents()
        {
            
        }

        private void PlayGame()
        {
            EventManager.StartGame.Execute();
        }
        private void ShowControls()
        {
            _currentScreen = ScreenState.ControlsScreen;
        }
        private void ShowHome(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && _currentScreen != ScreenState.HomeScreen)
            {
                _currentScreen = ScreenState.HomeScreen;
            }
        }
        private void SelectMenu(eButtonState buttonState, Vector2 amount)
        {
            if(buttonState == eButtonState.DOWN && _currentScreen == ScreenState.HomeScreen)
            {
                switch (menuPos)
                {
                    case 0:
                        PlayGame(); break;
                    case 1:
                        ShowControls(); break;
                }
            }
        }
        private void MoveUp(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && _currentScreen == ScreenState.HomeScreen)
            {
                if (menuPos > 0)
                    menuPos--;
            }
        }
        private void MoveDown(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && _currentScreen == ScreenState.HomeScreen)
            {
                if(menuPos < 1)
                    menuPos++;
            }
        }
        public void ExitGame(eButtonState buttonState, Vector2 amout) => EventManager.ExitGame.Execute();
    }
}
