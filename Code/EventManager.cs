using Microsoft.Xna.Framework;
using System;

namespace Lumberjack
{
    public delegate void GameAction(eButtonState buttonState, Vector2 amount);
    public class EventManager
    {
        public static Event PlayerDeath = new Event();
        public static Event CollectEgg = new Event();
        public static Event CutBark = new Event();
        public static Event StartGame = new Event();
        public static Event RestartGame = new Event();
        public static Event ExitGame = new Event();
    }          

}
