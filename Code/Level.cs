using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Reflection.Metadata;

namespace Lumberjack
{
    class Level
    {

        ContentManager content;

        private Loader loader;

        int levelIndex = 0;
        int levelWidth = 0;
        int barkIndex = 0;

        List<string> lines;

        public Level(Stream fileStream)
        {
            lines = new List<string>();

            loader = new Loader(fileStream);

            LoadTreeData(fileStream);
        }

        private void LoadTreeData(Stream fileStream)
        {
            // Load the level
            lines = loader.ReadLinesFromTextFile();
            levelWidth = (lines.Count > 0) ? lines[0].Length : 0;
        }

        public eBarkSide LoadNextBark()
        {
            if(barkIndex >= levelWidth)
                barkIndex = 0;

            eBarkSide barkSide = eBarkSide.Center;

            if (barkIndex < levelWidth)
            {
                switch (lines[levelIndex][barkIndex])
                {
                    case '.':
                        barkSide = eBarkSide.Center; break;
                    case '[':
                        barkSide = eBarkSide.Left; break;
                    case ']':
                        barkSide = eBarkSide.Right; break;
                }
            }
            barkIndex++;
            return barkSide;
        }
    }
}
