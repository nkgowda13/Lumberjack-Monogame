using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection.Metadata;
using System.Runtime;

namespace Lumberjack
{
    public enum ePlayerSide
    {
        LEFT,
        RIGHT
    }

    class Player : Collidable
    {
        #region Fields

        // Animation representing the player  
        public Animation PlayerAnimCuttingLeft;
        public Animation PlayerAnimIdleLeft;
        public Animation PlayerAnimCuttingRight;
        public Animation PlayerAnimIdleRight;
        public Animation CurrentAnimation;

        // Position of the Player relative to the upper left side of the screen  
        public Vector2 Position;

        // State of the player  
        public bool Active { get => _active; }
        private bool _active = true;

        // Flag to indicate if the player is currently cutting
        bool isCutting;

        // Player distance from the tree
        private float distFromTree = 100;

        private float posLeft = 100;
        private float posRight = 100;

        // Get Player Side
        public ePlayerSide PlayerSide
        { get { return playerSide; } }

        // Player side
        private ePlayerSide playerSide;

        #endregion

        public void Initialize(Animation animationCuttingLeft, Animation animIdleLeft,
            Animation animationCuttingRight, Animation animIdleRight, Vector2 position)
        {
            PlayerAnimCuttingLeft = animationCuttingLeft;
            PlayerAnimIdleLeft = animIdleLeft;
            PlayerAnimCuttingRight = animationCuttingRight;
            PlayerAnimIdleRight = animIdleRight;

            CurrentAnimation = animIdleLeft;
            isCutting = false;

            // Set the starting position of the player around the middle of the screen and to the back
            playerSide = ePlayerSide.LEFT;
            position.X -= distFromTree / 2 + 30;
            position.Y -= 25;
            Position = position;
            posLeft = Position.X;
            posRight = Position.X * 2;

            // Set the player to be active  
            _active = true;
            boundingSphere = new BoundingSphere(new Vector3(Position.X, Position.Y, 5), 45);
            UpdateBoundingSpherePosition();

            EventManager.RestartGame.AddListener(Reset);
        }

        private void Reset()
        {
            CurrentAnimation = PlayerAnimIdleLeft;
            isCutting = false;

            // Set the starting position of the player
            playerSide = ePlayerSide.LEFT;
            var playerPos = Position;
            playerPos.X = posLeft;
            Position = playerPos;
            boundingSphere.Center = new Vector3(playerPos.X, playerPos.Y, 5);

            // Set the player to be active  
            _active = true;
        }

        public void Update(GameTime gameTime)
        {
            CurrentAnimation.Position = Position;

            CurrentAnimation.Update(gameTime);

            // Update cutting animation
            if (isCutting && !CurrentAnimation.Active)
            {
                // Cutting Animation is completed
                isCutting = false;

                if (playerSide == ePlayerSide.LEFT)
                    CurrentAnimation = PlayerAnimIdleLeft;
                else
                    CurrentAnimation = PlayerAnimIdleRight;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentAnimation.Draw(spriteBatch);
        }

        private void UpdateBoundingSpherePosition()
        {
            var playerPos = Position;
            if (playerSide == ePlayerSide.LEFT)
                playerPos.X = posLeft;
            else
                playerPos.X = posRight;
            boundingSphere.Center = new Vector3(playerPos.X, playerPos.Y, 5);
        }
        public override void OnCollision(Collidable obj)
        {
            Egg.Instance.Position = Vector2.Zero;
            EventManager.CollectEgg.Execute();
        }
        public void MoveLeft()
        {
            if (playerSide == ePlayerSide.RIGHT)
            {
                playerSide = ePlayerSide.LEFT;

                var playerPos = Position;
                playerPos.X = posLeft;
                Position = playerPos;
                UpdateBoundingSpherePosition();
            }
            PlayerAnimCuttingLeft.Reset();
            CurrentAnimation = PlayerAnimCuttingLeft;
            isCutting = true;
        }

        public void MoveRight()
        {
            if (playerSide == ePlayerSide.LEFT)
            {
                playerSide = ePlayerSide.RIGHT;

                var playerPos = Position;
                playerPos.X = posRight;
                Position = playerPos;
                UpdateBoundingSpherePosition();
            }
            PlayerAnimCuttingRight.Reset();
            CurrentAnimation = PlayerAnimCuttingRight;
            isCutting = true;
        }

        public void DeactivatePlayer() => _active = false;
    }
}