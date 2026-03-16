using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

public class BaseTurret : GameObject
{
    public Ship Base { get; protected set; }
    public Vector2 RelativePosition { get; protected set; } = new(0, 0);
    public Texture2D Texture { get; protected set; }

    public float Rotation { get; set; }
    public float AimRotation { get; set; }

    public virtual float RotationSpeed {get; protected set; } = MathHelper.Pi * 1.5f;
    public virtual float CoolDown { get; protected set; } = 0.2f;

    public float CoolDownLeft = 0;

    protected readonly Vector2 CORR = new Vector2(1, -1);

    public BaseTurret(Ship ship)
    {
        Base = ship;
    }

    protected virtual void Shoot()
    {
        Vector2 turretExit =
            Base.GetPosition().Center.ToVector2()
            + RelativePosition.Rotated(Rotation)
            + (new Vector2(0, Texture.Height) / 2f).Rotated(Rotation);
        GameManager
            .GetGameManager()
            .AddGameObject(new Bullet(turretExit, (-Vector2.UnitY).Rotated(Rotation), 1000, Base.Velocity));
    }

    public override void Load(ContentManager content)
    {
        // Ship sprites from: https://zintoki.itch.io/space-breaker
        base.Load(content);
        Texture = content.Load<Texture2D>("base_turret");
    }

    public override void HandleInput(InputManager inputManager)
    {
        base.HandleInput(inputManager);
        Vector2 rightStick = inputManager.CurrentGamePadState.ThumbSticks.Right;
        if (rightStick.Length() > 0)
            AimRotation = rightStick.Angle();
        else
            AimRotation = (
                inputManager.GetMouseScreenPosition() - Base.GetPosition().Center.ToVector2()
            ).Angle();

        if (inputManager.LeftMousePress() || inputManager.RightTriggerPress())
        {
            if (CoolDownLeft < 0)
            {
                CoolDownLeft = CoolDown;
                Shoot();
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Rotation += MathHelper.WrapAngle(AimRotation - Rotation) * deltaTime * RotationSpeed;

        CoolDownLeft -= deltaTime;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
        spriteBatch.Draw(
            Texture,
            RelativePosition + Base.GetPosition().Center.ToVector2(),
            (Rectangle?)null,
            Color.White,
            Rotation,
            Texture.Bounds.Size.ToVector2() / 2f,
            Vector2.One,
            SpriteEffects.None,
            0
        );
    }
}
