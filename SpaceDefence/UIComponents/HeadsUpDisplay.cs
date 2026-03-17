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
    private Texture2D enemyArrowTexture;
    private Texture2D supplyArrowTexture;
    public List<GameObject> gameObjects {get; set;}

    public override void Load(ContentManager content)
        {
            base.Load(content);
            enemyArrowTexture = content.Load<Texture2D>("UI/Arrow_Alien");
            supplyArrowTexture = content.Load<Texture2D>("UI/Arrow_Crate");
        }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (gameObjects == null)
            return;
        Camera camera = GameManager.GetGameManager().Camera;
        Vector2 center = camera.Viewport.Size.ToVector2() / 2;
        Vector2 playerLocation = GameManager.GetGameManager().Player.GetPosition().Center.ToVector2();


        // for (float i = 0; i < 360; i++) {
        //     float angle = (i / MathHelper.Pi);
        //         spriteBatch.Draw(arrowTexture, center + Vector2.UnitX.Rotated(angle) * ARROW_DIST, null, Color.White, angle, arrowTexture.Bounds.Center.ToVector2(), Vector2.One, SpriteEffects.None, 0);
        // }
        foreach (GameObject gameObject in gameObjects) {
            if (gameObject is Alien alien && !camera.IsOnScreen(alien._circleCollider)) {
                Vector2 direction = (alien._circleCollider.Center - playerLocation).Normalized();
                spriteBatch.Draw(enemyArrowTexture, center + direction * ARROW_DIST, null, Color.White, direction.Angle(), enemyArrowTexture.Bounds.Center.ToVector2(), Vector2.One, SpriteEffects.None, 0);
            }
            else if (gameObject is Supply supply && !camera.IsOnScreen(supply.RectangleCollider)) {
                Vector2 direction = (supply.RectangleCollider.shape.Center.ToVector2() - playerLocation).Normalized();
                spriteBatch.Draw(supplyArrowTexture, center + direction * ARROW_DIST, null, Color.White, direction.Angle(), enemyArrowTexture.Bounds.Center.ToVector2(), Vector2.One, SpriteEffects.None, 0);
            }
        }
    }
}
