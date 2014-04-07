using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TileBasedSmoothPlatformer
{
    internal class Player
    {
        public Vector2 position, velocity = Vector2.Zero;
        private Rectangle hitbox;
        private Controller controller;
        public bool grounded = false;
        private KeyboardState state, prevState;

        private int accel = 1;
        private int maxSpeed = 10;
        private int speed = 0;
        private int jumpPower = 15;
        private int slopeWalk = 16;

        private int LeftOrRight = 0;
        private int marginY = 3;
        public bool jump;

        private bool changeToGrounded = true;

        public Player(Controller controller, Vector2 position)
        {
            this.position = position;
            hitbox = new Rectangle((int)position.X, (int)position.Y, Controller.tileSize, Controller.tileSize);
            this.controller = controller;
            state = Keyboard.GetState();
        }

        public void Update()
        {
            changeToGrounded = true;
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
            prevState = state;
            state = Keyboard.GetState();

            Vector2 moveDistance;
            bool prevGrounded = grounded;

            if (state.IsKeyDown(Keys.W) && prevState.IsKeyUp(Keys.W) && grounded)
            {
                velocity.Y = -jumpPower - 0.01f;
                jump = true;
            }

            Move(velocity, out moveDistance);

            if (moveDistance.Y == 0)
            {
                if (changeToGrounded)
                {
                    if (velocity.Y > 0)
                    {
                        SetGrounded();
                    }
                }
                velocity.Y = 0.1f;
            }
            else
            {
                if (changeToGrounded)
                {
                    grounded = false;
                    velocity.Y++;
                } 
                if (prevGrounded)
                {
                    if (SnapDown(velocity))
                    {
                        if (changeToGrounded)
                        {
                            SetGrounded();
                        }
                    }
                }
            }

            if (state.IsKeyDown(Keys.A))
            {
                if (speed > 0) speed = 0;
                if (speed > -maxSpeed) speed -= accel;
                Move(new Vector2(speed, 0), out moveDistance);
                LeftOrRight = -1;
            }
            if (state.IsKeyDown(Keys.D))
            {
                if (speed < 0) speed = 0;
                if (speed < maxSpeed) speed += accel;
                Move(new Vector2(speed, 0), out moveDistance);
                LeftOrRight = 1;
            }
            if (state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.D))
            {
                speed = 0;
            }
        }

        public void SetGrounded()
        {
            grounded = true;
            jump = false;
            changeToGrounded = false;
        }

        public void Move(Vector2 movement, out Vector2 moveDistance)
        {
            moveDistance = Vector2.Zero;
            if (movement.X != 0)
            {
                float forwardX;
                if (movement.X < 0)
                {
                    forwardX = hitbox.Left;
                }
                else
                {
                    forwardX = hitbox.Right;
                }

                int tempMovement = 0;
                movement.X = (int)Math.Abs(movement.X) - 1;
                bool isClear;
                while (tempMovement < (int)movement.X)
                {
                    isClear = true;
                    for (int i = 4; i < hitbox.Height; i++)
                    {
                        Wall tempWall;
                        Slope tempSlope;

                        if (controller.IsObstacle(new Vector2(forwardX + ((tempMovement + 1) * LeftOrRight), position.Y + i), out tempWall, out tempSlope))
                        {
                            if (tempWall != null)
                            {
                                if (hitbox.Bottom - slopeWalk <= tempWall.position.Y && !controller.IsObstacle(new Vector2(forwardX + ((tempMovement + 1) * LeftOrRight), tempWall.position.Y - Controller.tileSize)) && Math.Abs(speed) - tempMovement >= marginY + 1)
                                //stairs walking
                                {
                                    position.Y = tempWall.position.Y - Controller.tileSize;
                                }
                                else
                                {
                                    isClear = false;
                                }
                            }
                            else if (tempSlope != null)
                            {
                                float posY = tempSlope.topLine.GetPointAtPosX(forwardX + ((tempMovement + 1) * LeftOrRight)).Y;
                                if (hitbox.Bottom - slopeWalk <= posY && !controller.IsObstacle(new Vector2(forwardX + ((tempMovement + 1) * LeftOrRight), posY - Controller.tileSize)) && Math.Abs(speed) - tempMovement >= marginY + 1)
                                //slope walking
                                {
                                    position.Y = (int)posY - Controller.tileSize;
                                }
                                else
                                {
                                    isClear = false;
                                }
                            }
                            else
                            {
                                isClear = false;
                            }
                        }
                    }
                    if (isClear)
                    {
                        tempMovement++;
                    }
                    else
                    {
                        break;
                    }
                }
                position.X += tempMovement * (LeftOrRight);
                moveDistance.X = tempMovement;
            }
            if (movement.Y != 0)
            {
                float forwardY;
                int TopOrBottom;
                if (movement.Y < 0)
                {
                    forwardY = hitbox.Top;
                    TopOrBottom = -1;
                }
                else
                {
                    forwardY = hitbox.Bottom;
                    TopOrBottom = 1;
                }

                int tempMovement = 0;
                movement.Y = Math.Abs(movement.Y);
                while (tempMovement < movement.Y)
                {
                    Slope tempSlope;
                    Wall tempWall;
                    bool isClear = true;
                    for (int i = marginY; i < hitbox.Width - marginY; i++)
                    {
                        if (controller.IsObstacle(new Vector2(position.X + i, forwardY + (tempMovement * TopOrBottom)), out tempWall, out tempSlope))
                        {
                            if (tempSlope != null)
                            {
                                isClear = false;
                                if (velocity.Y > 0)
                                {
                                    SetGrounded();
                                }
                            }
                            else if (tempWall != null && i >= marginY && i <= hitbox.Width - marginY)
                            {
                                isClear = false;
                            }
                        }
                    }

                    if (isClear)
                    {
                        tempMovement++;
                    }
                    else
                    {
                        break;
                    }
                }
                position.Y += tempMovement * (TopOrBottom);
                moveDistance.Y = tempMovement;
            }
        }

        public bool SnapDown(Vector2 vel) // walking down slopes
        {
            Wall tempWall = Wall.Nothing;
            Slope tempSlope = Slope.Nothing;
            bool found = false;

            float posX = position.X;

            for (int i = 0; i < slopeWalk; i++)
            {
                if (found)
                {
                    break;
                }
                for (int ix = marginY; ix < hitbox.Width - marginY; ix++)
                {
                    if (controller.IsObstacle(new Vector2(position.X + ix, position.Y + Controller.tileSize + i), out tempWall, out tempSlope))
                    {
                        if (i == 0)
                        {
                            return true;
                        }
                        found = true;
                        posX = position.X + ix;
                        break;
                    }
                }
            }

            if (found)
            {
                if (tempWall != null)
                {
                    position.Y = tempWall.position.Y - Controller.tileSize;
                }
                else if (tempSlope != null)
                {
                    float posY = tempSlope.topLine.GetPointAtPosX(posX).Y;
                    position.Y = posY - Controller.tileSize;
                }
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentManager.tPlayer, position, Color.White);
        }
    }
}