using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumberjack
{
    class BirdFleeState : State
    {
        public BirdFleeState()
        {
            Name = "Flee";
        }
        public override void Enter(object owner)
        {
            Bird bird = owner as Bird;
            bird.Rotation = MathHelper.ToRadians(-30);
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            Bird bird = owner as Bird;
            bird.Velocity = 5;
            bird.Position.X += bird.Velocity * (float)Math.Cos(bird.Rotation);
            bird.Position.Y += bird.Velocity * (float)Math.Sin(bird.Rotation);
            if (bird.Position.Y < -100)
                bird.Active = false;
        }

        public override void Exit(object owner)
        {
            Bird bird = owner as Bird;
            bird.Active = false;
        }
    }
}
