using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lumberjack
{
    class Egg : Collidable
    {
        private static Egg instance;
        public static Egg Instance
        {
            get  {
                if (instance == null)
                {
                    instance = new Egg();
                }
                return instance;
            }
        }

        private float velocity = 5;

        public Texture2D EggTexture;
        public Vector2 Position;
        public bool Active = false;
        private bool gameOver = false;
        public void Initialize(Texture2D texture)
        {
            EggTexture = texture;

            boundingSphere = new BoundingSphere(new Vector3(Position.X, Position.Y, 5), 55);

            EventManager.PlayerDeath.AddListener(GameOver);
            EventManager.RestartGame.AddListener(ResetGame);
        }
        public void DropEgg(Vector2 position)
        {
            if(!gameOver)
            {
                Position = position;
                Active = true;
            }
        }
        public void Update(GameTime gameTime) 
        {
            if (Active)
            {
                Position.Y += velocity;
            }
            boundingSphere.Center = new Vector3(Position.X, Position.Y, 5);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(Active)
            {
                spriteBatch.Draw(EggTexture, Position, Color.White);
            }
        }
        private void ResetGame()
        {
            gameOver = false;
            DeactivateEgg();
        }
        private void GameOver() => gameOver = true;
        public void DeactivateEgg() => Active = false;
    }
}
