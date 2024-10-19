using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Lumberjack
{
    class Bark
    {
        // State of the bark  
        public bool Active;

        // Texture representing the bark  
        public Texture2D BarkTexture;

        public eBarkSide BarkSide;

        // Position of the Bark relative to the upper left side of the screen  
        public Vector2 Position;

        // Get the width of the Texture  
        public int Width
        { get { return BarkTexture.Width; } }
        // Get the height of the Texture 
        public int Height
        { get { return BarkTexture.Height; } }

        public void Initialize(Texture2D texture, Vector2 position)
        {
            Position = position;
            Active = true;

            BarkTexture = texture;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Active)
            {
                Vector2 drawPosition;
                drawPosition.X = Position.X - Width / 2; drawPosition.Y = Position.Y - Height / 2;
                spriteBatch.Draw(BarkTexture, drawPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
