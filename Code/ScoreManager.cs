using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumberjack
{
    class ScoreManager
    {
        private static ScoreManager instance;
        private int _score = 0;
        private int cutTreeScore = 1;
        private int catchEggScore = 5;

        public static ScoreManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScoreManager();
                }
                return instance;
            }
        }

        public int Score { get { return _score; } }

        public ScoreManager()
        {
            _score = 0;
            EventManager.RestartGame.AddListener(ResetScore);
        }

        public void ResetScore()
        {
            HighScoreManager.Instance.UpdateHighScore(_score);
            _score = 0;
        }
        public void CutTree() => AddScore(cutTreeScore);
        public void CatchEgg() => AddScore(catchEggScore);
        private void AddScore(int score)
        {
            _score += score;
        }
    }
}
