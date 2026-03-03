using System;
using SpaceDefence.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence
{
    public class Ship : GameObject
    {
        private Texture2D ship_body;
        private Texture2D base_turret;
        private Texture2D laser_turret;
        private float buffTimer = 100;
        private float buffDuration = 10f;
        private RectangleCollider _rectangleCollider;
        private Point target;

        private Vector2 velocity;
        private float rotation;
        private float aimRotation;
        private float TOP_SPEED;
        private float acceleration;

        /// <summary>
        /// The player character
        /// </summary>
        /// <param name="Position">The ship's starting position</param>
        public Ship(Point Position)
        {
            _rectangleCollider = new RectangleCollider(new Rectangle(Position, Point.Zero));
            SetCollider(_rectangleCollider);
        }

        public override void Load(ContentManager content)
        {
            // Ship sprites from: https://zintoki.itch.io/space-breaker
            ship_body = content.Load<Texture2D>("ship_body");
            base_turret = content.Load<Texture2D>("base_turret");
            laser_turret = content.Load<Texture2D>("laser_turret");
            _rectangleCollider.shape.Size = ship_body.Bounds.Size;
            _rectangleCollider.shape.Location -= new Point(ship_body.Width / 2, ship_body.Height / 2);
            base.Load(content);
        }



        public override void HandleInput(InputManager inputManager)
        {
            base.HandleInput(inputManager);
            GamePadState controller = GamePad.GetState(PlayerIndex.One);
            Vector2 aimVector = controller.ThumbSticks.Left;
            target = (_rectangleCollider.shape.Center.ToVector2() + aimVector * 400).ToPoint();

            if (controller.Buttons.A == ButtonState.Pressed)
            {
                Vector2 turretExit = _rectangleCollider.shape.Center.ToVector2() + aimVector * base_turret.Height / 2f;
                if (buffTimer <= 0)
                    GameManager.GetGameManager().AddGameObject(new Bullet(turretExit, aimVector, 150));
                else
                    GameManager.GetGameManager().AddGameObject(new Laser(new LinePieceCollider(turretExit, target.ToVector2()), 400));
            }

            if (controller.Triggers.Right > 0)
                acceleration = controller.Triggers.Right;

            float aimAngle;
            if (inputManager.IsKeyDown(Keys.A))
                aimAngle = -(float)Math.PI * 0.5f;
            else if (inputManager.IsKeyDown(Keys.D))
                aimAngle = (float)Math.PI * 0.5f;
            else if (inputManager.IsKeyDown(Keys.S))
                aimAngle = -(float)Math.PI;
            else
                aimAngle = controller.ThumbSticks.Left.Angle();
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Update the Buff timer
            if (buffTimer > 0)
                buffTimer -= deltaTime;

            rotation += aimRotation * deltaTime;
            rotation = MathHelper.WrapAngle(rotation);

            velocity += acceleration * velocity;
            this._rectangleCollider.shape.Offset((Vector2.One * velocity * deltaTime).Rotated(rotation));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ship_body, _rectangleCollider.shape, null, Color.White); //, rotation, _rectangleCollider.shape.Size.ToVector2(), Vector2.One, SpriteEffects.None, 0);
            float aimAngle = GamePad.GetState(0).ThumbSticks.Left.Angle();
            if (buffTimer <= 0)
            {
                Rectangle turretLocation = base_turret.Bounds;
                turretLocation.Location = _rectangleCollider.shape.Center;
                spriteBatch.Draw(base_turret, turretLocation, null, Color.White, aimAngle, turretLocation.Size.ToVector2() / 2f, SpriteEffects.None, 0);
            }
            else
            {
                Rectangle turretLocation = laser_turret.Bounds;
                turretLocation.Location = _rectangleCollider.shape.Center;
                spriteBatch.Draw(laser_turret, turretLocation, null, Color.White, aimAngle, turretLocation.Size.ToVector2() / 2f, SpriteEffects.None, 0);
            }
            base.Draw(gameTime, spriteBatch);
        }


        public void Buff()
        {
            buffTimer = buffDuration;
        }

        public Rectangle GetPosition()
        {
            return _rectangleCollider.shape;
        }
    }
}
