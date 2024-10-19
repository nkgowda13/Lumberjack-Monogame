using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Lumberjack
{
    class Timer
    {
        Texture2D timerTexture;
        Texture2D bgTexture;
        Vector2 timerPosition = new Vector2(150, 100); // Example position
        float countdownDuration = 60; // Total duration of the countdown in seconds
        float countdownTimer = 60; // Timer to keep track of countdown

        public float TimerValue { get => countdownTimer; }

        bool isGameOver = false;

        public Timer(Texture2D timerTexture, Texture2D bgTexture)
        {
            this.timerTexture = timerTexture;
            this.bgTexture = bgTexture;

            EventManager.PlayerDeath.AddListener(StopTimer);
            EventManager.RestartGame.AddListener(ResetTimer);
        }

        public void Update(GameTime gameTime)
        {
            if(!isGameOver)
            {
                // Update countdown timer
                float decrementAmount = 5.0f;
                countdownTimer -= decrementAmount * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Clamp countdown timer to prevent negative values
                countdownTimer = Math.Min(countdownTimer, 60);
                countdownTimer = Math.Max(countdownTimer, 0);
            }
        }

        public void AddTime(float time) => countdownTimer += time;

        public void Draw(SpriteBatch spriteBatch)
        {
            float scaleFactor = countdownTimer / countdownDuration;

            // Draw the timer sprite with scaled width
            spriteBatch.Draw(bgTexture,
                             timerPosition,
                             null,
                             Color.White,
                             0f, // Rotation angle (no rotation)
                             Vector2.Zero, // Origin (top-left corner)
                             new Vector2(1, 1), // Scale factor (scale width only)
                             SpriteEffects.None,
                             0f);
            // Draw the timer sprite with scaled width
            spriteBatch.Draw(timerTexture,
                             new Vector2(timerPosition.X + 5, timerPosition.Y + 5),
                             null,
                             Color.White,
                             0f, // Rotation angle (no rotation)
                             Vector2.Zero, // Origin (top-left corner)
                             new Vector2(scaleFactor, 1), // Scale factor (scale width only)
                             SpriteEffects.None,
                             0f);

        }
        private void StopTimer() => isGameOver = true;
        private void ResetTimer()
        {
            isGameOver = false;
            countdownDuration = 60;
            countdownTimer = 60; 
        }
    }
}
