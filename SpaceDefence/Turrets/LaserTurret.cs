using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

public class LaserTurret : BaseTurret
{
    public override float RotationSpeed {get; protected set; } = MathHelper.Pi * 1.5f;
    public LaserTurret(Ship ship)
        : base(ship)
    {
        base.CoolDown = .3f;
    }

    protected override void Shoot()
    {
        Vector2 turretExit =
            Base.GetPosition().Center.ToVector2()
            + RelativePosition * Rotation
            + (new Vector2(0, Texture.Height) / -2f).Rotated(Rotation);
        GameManager
            .GetGameManager()
            .AddGameObject(
                new Laser(new LinePieceCollider(turretExit, turretExit + (-Vector2.UnitY).Rotated(base.Rotation)), 700)
            );
    }

    // protected override void Shoot()
    // {
    //     float exitSide = LastTurret ? -1f : 1f;
    //     Vector2 turretExit =
    //         Base.GetPosition().Center.ToVector2()
    //         + RelativePosition * Rotation
    //         + new Vector2(Texture.Width / 9 * exitSide, Texture.Height / -2).Rotated(
    //             Rotation
    //         );
    //     GameManager
    //         .GetGameManager()
    //         .AddGameObject(new Bullet(turretExit, (-Vector2.UnitY).Rotated(Rotation), 1000, Base.Velocity));
    //     LastTurret = !LastTurret;
    // }

    public override void Load(ContentManager content)
    {
        // Ship sprites from: https://zintoki.itch.io/space-breaker
        base.Texture = content.Load<Texture2D>("Ship/Laser_Turret");
        // NOT LOADING BASE CLASS TEXTURES SINCE THAT'D OVERRIDE OUR BEAUTIFUL TURRET
    }
}
