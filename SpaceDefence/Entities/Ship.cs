using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceDefence.Collision;

namespace SpaceDefence;

public class Ship : MovingObject
{
    private const float DEAD_ZONE = 0.0f;
    private const float TOP_SPEED = 500;
    private const float ACCELERATION = 400;
    private const float ROTATION_SPEED = MathHelper.Pi;

    private readonly Vector2 CORR = new Vector2(1, -1);

    public BaseTurret Turret;

    private Texture2D ship_body;
    private Texture2D laser_turret;
    private float buffTimer = 0;
    private float buffDuration = 10f;
    private RectangleCollider _rectangleCollider;
    private Point target;

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
        Turret = new DoubleTurret(this);
    }

    public override void Load(ContentManager content)
    {
        // Ship sprites from: https://zintoki.itch.io/space-breaker
        ship_body = content.Load<Texture2D>("ship_body");
        _rectangleCollider.shape.Size = ship_body.Bounds.Size;
        _rectangleCollider.shape.Location -= new Point(ship_body.Width / 2, ship_body.Height / 2);
        Turret.Load(content);
        base.Load(content);
    }

    public override void HandleInput(InputManager inputManager)
    {
        base.HandleInput(inputManager);

        Turret.HandleInput(inputManager);

        if (inputManager.IsKeyDown(Keys.W))
            gasPedal = 1;
        else if (inputManager.CurrentGamePadState.Triggers.Left > 0)
            gasPedal = inputManager.CurrentGamePadState.Triggers.Left;
        else
            gasPedal = 0;

        Vector2 leftStick = inputManager.CurrentGamePadState.ThumbSticks.Left;
        if (inputManager.CurrentGamePadState.ThumbSticks.Left.Length() > DEAD_ZONE)
        {
            var thumbVec = leftStick;
            rotationAim = (thumbVec * CORR).Angle() - MathHelper.PiOver2;
        }
        else if (inputManager.IsKeyDown(Keys.A))
            rotationAim = rotation - MathHelper.PiOver4;
        else if (inputManager.IsKeyDown(Keys.D))
            rotationAim = rotation + MathHelper.PiOver4;
        else if (inputManager.IsKeyDown(Keys.S)) // S angles against the current velocity
            rotationAim = Velocity.Angle() + MathHelper.PiOver2;
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Turret.Update(gameTime);
        // Update the Buff timer
        buffTimer -= deltaTime;

        rotation = rotation + MathHelper.WrapAngle(rotationAim - rotation) * deltaTime * ROTATION_SPEED;
        Turret.Rotation += MathHelper.WrapAngle(rotationAim - rotation) * deltaTime * ROTATION_SPEED;
        Velocity +=
            new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation))
            * ACCELERATION
            * gasPedal
            * deltaTime;
        if (Velocity.Length() > TOP_SPEED)
            Velocity = Velocity.Normalized() * TOP_SPEED;
        this._rectangleCollider.shape.Offset(Vector2.One * Velocity * deltaTime);

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Rectangle shipLocation = ship_body.Bounds;
        shipLocation.Location = _rectangleCollider.shape.Center;
        spriteBatch.Draw(
            ship_body,
            shipLocation.Location.ToVector2(),
            null,
            Color.White,
            rotation + MathHelper.PiOver2,
            ship_body.Bounds.Size.ToVector2() / 2f,
            Vector2.One,
            SpriteEffects.None,
            0
        );

        Turret.Draw(gameTime, spriteBatch);
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
        _rectangleCollider.shape.Location = GameManager
            .GetGameManager()
            .RandomScreenLocation()
            .ToPoint();
        Velocity = Vector2.Zero;
    }
}
