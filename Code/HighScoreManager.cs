using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumberjack
{
    class HighScoreManager
    {
        private int _highScore;
        private static HighScoreManager instance;

        public int HighScore { get { return _highScore; } }
        public static HighScoreManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HighScoreManager();
                }
                return instance;
            }
        }

        private string filePath = "highscores.xml";

        HighScoreManager()
        {
            _highScore = XMLParse.DeserializeFromXml<int>(filePath);
        }

        public void UpdateHighScore(int highScore)
        {
            if (highScore > HighScore)
            {
                XMLParse.SerializeToXml(filePath, highScore);
                _highScore = highScore;
            }
        }
    }
}
