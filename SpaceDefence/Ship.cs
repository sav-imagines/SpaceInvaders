using SpaceDefence.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceDefence
{
    public class Ship : GameObject
    {
        private const float DEAD_ZONE = 0.0f;
        private const float TOP_SPEED = 500;
        private const float ACCELERATION = 400;
        private const float ROTATION_SPEED = MathHelper.Tau;

        private Texture2D ship_body;
        private Texture2D base_turret;
        private Texture2D laser_turret;
        private float buffTimer = 100;
        private float buffDuration = 10f;
        private RectangleCollider _rectangleCollider;
        private Point target;

        private Vector2 velocity;
        private float rotation;
        private Vector2 turretAim;
        private float rotationAim; // the angle you are steering towards

        private float gasPedal;


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
            if (controller.ThumbSticks.Right.Length() > DEAD_ZONE)
            {
                turretAim = controller.ThumbSticks.Right.Normalized();
                turretAim = new(turretAim.X, -turretAim.Y);
            }

            if (controller.Triggers.Right > DEAD_ZONE)
            {
                target = (_rectangleCollider.shape.Center.ToVector2() + turretAim * 400).ToPoint();
                Vector2 turretExit = _rectangleCollider.shape.Center.ToVector2() + turretAim * base_turret.Height / 2f;
                if (buffTimer <= 0)
                    GameManager.GetGameManager().AddGameObject(new Bullet(turretExit, turretAim, 1000));
                else
                    GameManager.GetGameManager().AddGameObject(new Laser(new LinePieceCollider(turretExit, target.ToVector2()), 400));
            }

            if (inputManager.IsKeyDown(Keys.W))
                gasPedal = 1;
            else
                gasPedal = controller.Triggers.Left;

            if (controller.ThumbSticks.Left.Length() > DEAD_ZONE)
            {
                var thumbVec = controller.ThumbSticks.Left;
                rotationAim = new Vector2(thumbVec.X, -thumbVec.Y).Angle() - MathHelper.PiOver2;
            }
            else if (inputManager.IsKeyDown(Keys.A))
                rotationAim = rotation - MathHelper.PiOver4;
            else if (inputManager.IsKeyDown(Keys.D))
                rotationAim = rotation + MathHelper.PiOver4;

        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Update the Buff timer
            if (buffTimer > 0)
                buffTimer -= deltaTime;

            rotation = rotation + (MathHelper.WrapAngle(rotationAim - rotation)) * deltaTime * ROTATION_SPEED;
            velocity += new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * ACCELERATION * gasPedal * deltaTime;
            if (velocity.Length() > TOP_SPEED)
                velocity = velocity.Normalized() * TOP_SPEED;
            this._rectangleCollider.shape.Offset((Vector2.One * velocity * deltaTime));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle shipLocation = ship_body.Bounds;
            shipLocation.Location = _rectangleCollider.shape.Center;
            spriteBatch.Draw(ship_body, shipLocation.Location.ToVector2(), null, Color.White, rotation + (float)Math.PI / 2, shipLocation.Size.ToVector2() / 2f, Vector2.One, SpriteEffects.None, 0);
            if (buffTimer <= 0)
            {
                Rectangle turretLocation = base_turret.Bounds;
                turretLocation.Location = _rectangleCollider.shape.Center;
                spriteBatch.Draw(base_turret, turretLocation, null, Color.White, turretAim.Angle(), turretLocation.Size.ToVector2() / 2f, SpriteEffects.None, 0);
            }
            else
            {
                Rectangle turretLocation = laser_turret.Bounds;
                turretLocation.Location = _rectangleCollider.shape.Center;
                spriteBatch.Draw(laser_turret, turretLocation, null, Color.White, turretAim.Angle(), turretLocation.Size.ToVector2() / 2f, SpriteEffects.None, 0);
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

        public void ResetPosition()
        {
            _rectangleCollider.shape.Location = GameManager.GetGameManager().RandomScreenLocation().ToPoint();
        }
    }
}
