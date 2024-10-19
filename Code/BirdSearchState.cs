using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumberjack
{
    class BirdSearchState : State
    {
        public BirdSearchState() 
        {
            Name = "Search";
        }
        public override void Enter(object owner)
        {
            Bird bird = owner as Bird;
            bird.Active = true;
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            Bird bird = owner as Bird;
            bird.Position.X += bird.Velocity * (float)Math.Cos(bird.Rotation);
            bird.Position.Y += bird.Velocity * (float)Math.Sin(bird.Rotation);
        }

        public override void Exit(object owner)
        {
            Bird bird = owner as Bird;
            Egg.Instance.DropEgg(bird.Position);
        }
    }
}
