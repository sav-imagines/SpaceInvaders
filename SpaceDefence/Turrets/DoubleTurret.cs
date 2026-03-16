using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

public class DoubleTurret : BaseTurret
{
    private bool LastTurret = false;

    public DoubleTurret(Ship ship)
        : base(ship)
    {
        base.CoolDown = .08f;
    }

    protected override void Shoot()
    {
        float exitSide = LastTurret ? -1f : 1f;
        Vector2 turretExit =
            Base.GetPosition().Center.ToVector2()
            + RelativePosition * Rotation
            + new Vector2(Texture.Width / 9 * exitSide, Texture.Height / -2).Rotated(
                Rotation.Angle()
            );
        GameManager
            .GetGameManager()
            .AddGameObject(new Bullet(turretExit, Rotation.Normalized(), 1000, Base.Velocity));
        LastTurret = !LastTurret;
    }

    public override void HandleInput(InputManager inputManager)
    {
        base.HandleInput(inputManager);
        Vector2 rightStick = inputManager.CurrentGamePadState.ThumbSticks.Right;
        if (rightStick.Length() > 0)
            Rotation = rightStick.Normalized() * CORR;
        else
            Rotation = (
                inputManager.GetMouseScreenPosition() - Base.GetPosition().Center.ToVector2()
            ).Normalized();

        if (inputManager.LeftMouseDown() || inputManager.CurrentGamePadState.Triggers.Right > 0)
        {
            if (CoolDownLeft < 0)
            {
                CoolDownLeft = CoolDown;
                Shoot();
            }
        }
    }

    public override void Load(ContentManager content)
    {
        // Ship sprites from: https://zintoki.itch.io/space-breaker
        base.Texture = content.Load<Texture2D>("double_turret");
        // NOT LOADING BASE CLASS TEXTURES SINCE THAT'D OVERRIDE OUR BEAUTIFUL TURRET
    }
}
