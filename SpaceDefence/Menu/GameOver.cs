using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence;

public class GameOverMenu : GameObject
{
    private SpriteFont font;
    private float FONT_SCALE = 4;

    // private Camera Camera = GameManager.GetGameManager().Camera;

    public override void HandleInput(InputManager inputManager)
    {
        base.HandleInput(inputManager);
        if (inputManager.IsKeyPress(Keys.Space) || inputManager.IsButtonPress(Buttons.A))
            GameManager.GetGameManager().State = GameState.Playing;
        else if (inputManager.IsKeyPress(Keys.Escape))
            GameManager.GetGameManager().Game.Exit();
    }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        font = content.Load<SpriteFont>("PixelFont");
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Camera Camera = GameManager.GetGameManager().Camera;
        Vector2 ScreenSize = Camera.Viewport.Size.ToVector2();
        Rectangle ScreenSpaceRect = new Rectangle(
            Point.Zero,
            Camera.Viewport.Size
        );

        GameManager
            .GetGameManager()
            .DrawRectangle(ScreenSpaceRect, new Color(0, 0, 0, 200), spriteBatch);
        string outputA = "GAME  OVER";
        Vector2 sizeA = font.MeasureString(outputA) * FONT_SCALE;
        Vector2 posA = new Vector2(sizeA.X * -0.5f, -sizeA.Y * 2) + ScreenSize / 2;
        spriteBatch.DrawString(
            font,
            outputA,
            posA,
            Color.White,
            0,
            Vector2.Zero,
            Vector2.One * FONT_SCALE,
            SpriteEffects.None,
            0
        );
        float subtitleScale = FONT_SCALE * .7f;
        string outputB = "Press  spacebar  or (A)  to  continue.";
        Vector2 sizeB = font.MeasureString(outputB) * subtitleScale;
        Vector2 posB = new Vector2(sizeB.X * -0.5f, sizeB.Y * 2) + ScreenSize / 2;
        spriteBatch.DrawString(
            font,
            outputB,
            posB,
            Color.White,
            0,
            Vector2.Zero,
            Vector2.One * subtitleScale,
            SpriteEffects.None,
            0
        );
    }
}
