using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;

namespace Lumberjack
{
    class Bird
    {
        // Animation representing the bird  
        public Animation BirdFlyingAnim;

        // Bird Position
        public Vector2 Position;

        // Bird Velocity
        public float Velocity = 2;

        // Bird Rotation
        public float Rotation = MathHelper.ToRadians(30);

        // State of the bird  
        public bool Active;

        // Has the egg been dropped
        public bool IsEggDropped = false;

        // Egg Drop Position
        public Vector2 DropPos;

        FSM fsm;

        public void Initialize(Animation anim, Vector2 position)
        {
            fsm = new FSM(this);

            // Create the states 
            BirdSearchState search = new BirdSearchState();
            BirdFleeState flee = new BirdFleeState();

            // Add the created states to the FSM 
            fsm.AddState(search);
            fsm.AddState(flee);

            // Create the transitions between the states 
            search.AddTransition(new Transition(flee, () => IsEggDropped));

            // Set the starting state of the FSM 
            fsm.Initialise("Search");

            BirdFlyingAnim = anim;
            Position = position;
            Active = false;

            EventManager.RestartGame.AddListener(Reset);
        }

        private void Reset()
        {
            Active = false;
        }

        public void MoveBird(Vector2 position, Vector2 dropPos)
        {
            Position = position;
            DropPos = dropPos;
            Rotation = MathHelper.ToRadians(30);
            Velocity = 2;
            Active = true; 
            IsEggDropped = false;
            fsm.Initialise("Search");
        }

        public void Update(GameTime gameTime)
        {
            if(Active)
            {
                if (Position.X >= DropPos.X)
                    DropEgg();

                BirdFlyingAnim.Position = Position;

                BirdFlyingAnim.Update(gameTime);

                fsm.Update(gameTime);

                if(Position.Y <= -10)
                    Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Active)
                BirdFlyingAnim.Draw(spriteBatch, Rotation);
        }

        private void DropEgg()
        {
            IsEggDropped = true;
        }
    }
}
