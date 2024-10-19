using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using static Lumberjack.Player;

namespace Lumberjack
{
    public enum eBarkSide
    {
        Left,
        Right,
        Center
    }

    class Tree
    {
        Texture2D barkTexture;
        Texture2D branchLeftTexture;
        Texture2D branchRightTexture;

        // Position of the Tree relative to the upper left side of the screen  
        public Vector2 Position;

        // Minimum number of barks to be generated
        private int numBarks = 15;

        // Bark Height
        private int barkHeight = 75;

        // List of Barks
        public Queue<Bark> barksList;

        // Level instance
        Level level;

        public void Initialize(Texture2D texture, Texture2D branchLeft, Texture2D branchRight, Vector2 position)
        {
            barkTexture = texture;
            branchLeftTexture = branchLeft;
            branchRightTexture = branchRight;
            Position = position;

            // Load the level.
            string levelPath = string.Format("Content/Levels/1.txt", 1);
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                level = new Level(fileStream);

            barksList = new Queue<Bark>();
            GenerateBarks();

            EventManager.RestartGame.AddListener(Reset);
        }

        private void Reset()
        {
            // Load the level.
            string levelPath = string.Format("Content/Levels/1.txt", 1);
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                level = new Level(fileStream);

            barksList = new Queue<Bark>();
            GenerateBarks();
        }

        public void Update()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var bark in barksList)
            {
                bark.Draw(spriteBatch);
            }
        }

        private void GenerateBarks()
        {
            if(barksList.Count <= 0)
            {
               for(int i = 0; i < numBarks; i++)
                {
                    CreateBark(i);
                }
            }
        }

        public void DestroyBark(ePlayerSide playerSide)
        {
            if(barksList.Count > 0)
            {
                var bark = barksList.Dequeue();
                bark.Active = false;
                int i = 0;
                foreach (var temp in barksList)
                {
                    var position = Position;
                    position.Y = position.Y - barkHeight * i;
                    temp.Position = position;
                    i++;
                }
                while(barksList.Count < numBarks)
                {
                    CreateBark(i);
                    i++;
                }
            }
        }
    
        private void CreateBark(int index)
        {
            var barkSide = level.LoadNextBark();
            var newBark = new Bark();
            var position = Position;
            position.Y = position.Y - barkHeight * index;
            newBark.BarkSide = barkSide;
            newBark.Initialize(GetBarkTexture(barkSide), position);
            barksList.Enqueue(newBark);
        }

        private Texture2D GetBarkTexture(eBarkSide barkSide)
        {
            Texture2D texture = barkTexture;
            switch (barkSide)
            {
                case eBarkSide.Left:
                    texture = branchLeftTexture; break;
                case eBarkSide.Right:
                    texture = branchRightTexture; break;
                case eBarkSide.Center:
                    texture = barkTexture; break;

            }
            return texture;
        }
    }
}
