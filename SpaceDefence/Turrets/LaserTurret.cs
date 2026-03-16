using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

public class LaserTurret : BaseTurret
{
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
            + Rotation * Texture.Height / 2f;
        GameManager
            .GetGameManager()
            .AddGameObject(
                new Laser(new LinePieceCollider(turretExit, turretExit + base.Rotation), 700)
            );
    }

    public override void Load(ContentManager content)
    {
        // Ship sprites from: https://zintoki.itch.io/space-breaker
        base.Texture = content.Load<Texture2D>("laser_turret");
        // NOT LOADING BASE CLASS TEXTURES SINCE THAT'D OVERRIDE OUR BEAUTIFUL TURRET
    }
}
