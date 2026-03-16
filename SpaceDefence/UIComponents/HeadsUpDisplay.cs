using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

public class HeadsUpDisplay : GameObject {
    public readonly float ARROW_DIST = GraphicsAdapter
        .DefaultAdapter
        .CurrentDisplayMode
        .Width / 30;
    private Texture2D arrowTexture;
    public List<GameObject> gameObjects {get; set;}

    public override void Load(ContentManager content)
        {
            base.Load(content);
            arrowTexture = content.Load<Texture2D>("Arrow");
        }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (gameObjects == null)
            return;
        Camera camera = GameManager.GetGameManager().Camera;
        Vector2 center = camera.Viewport.Size.ToVector2() / 2;
        Vector2 playerLocation = GameManager.GetGameManager().Player.GetPosition().Center.ToVector2();

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // for (float i = 0; i < 360; i++) {
        //     float angle = (i / MathHelper.Pi);
        //         spriteBatch.Draw(arrowTexture, center + Vector2.UnitX.Rotated(angle) * ARROW_DIST, null, Color.White, angle, arrowTexture.Bounds.Center.ToVector2(), Vector2.One, SpriteEffects.None, 0);
        // }
        foreach (GameObject gameObject in gameObjects) {
            if (gameObject is Alien alien) {
                Vector2 direction = (alien._circleCollider.Center - playerLocation).Normalized();
                spriteBatch.Draw(arrowTexture, center + direction * ARROW_DIST, null, Color.White, direction.Angle(), arrowTexture.Bounds.Center.ToVector2(), Vector2.One, SpriteEffects.None, 0);
            }
        }
        spriteBatch.End();
    }
}
