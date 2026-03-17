using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence;

public class MainMenu : GameObject
{
    private const float FONT_SCALE = 4;

    private List<Button> buttons = [];
    private SpriteFont font;

    public override void Load(ContentManager content)
    {
        base.Load(content);
        // TODO: get screen size live?

        // PROBLEM: this crashes
        // var screen = GameManager.GetGameManager().Camera.Viewport.Size.ToVector2();
        var screen = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        buttons = [
            new Button(() => { GameManager.GetGameManager().State = GameState.Playing;}, screen / 2 + Vector2.UnitY * screen.Y * 0.05f, "Play"),
            new Button(() => {GameManager.GetGameManager().Game.Exit();}, screen / 2 + Vector2.UnitY * screen.Y * 0.1f, "Quit")
        ];
        foreach(Button button in buttons)
            button.Load(content);
        font = content.Load<SpriteFont>("PixelFont");
    }

    public override void HandleInput(InputManager inputManager)
        {
        if (inputManager.IsKeyPress(Keys.Space) || inputManager.IsButtonPress(Buttons.A))
            GameManager.GetGameManager().State = GameState.Playing;
        else if (inputManager.IsKeyPress(Keys.Escape))
            GameManager.GetGameManager().Game.Exit();

        foreach(Button button in buttons)
            button.HandleInput(inputManager);
        }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        foreach(Button button in buttons)
            button.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Camera Camera = GameManager.GetGameManager().Camera;
        Vector2 ScreenSize = Camera.Viewport.Size.ToVector2();

        string outputA = "MAIN  MENU";
        Vector2 sizeA = font.MeasureString(outputA) * FONT_SCALE * 1.5f;
        Vector2 posA = new Vector2(sizeA.X * -0.5f, -sizeA.Y * 1) + ScreenSize / 2;
        spriteBatch.DrawString(
            font,
            outputA,
            posA,
            Color.White,
            0,
            Vector2.Zero,
            Vector2.One * FONT_SCALE * 1.5f,
            SpriteEffects.None,
            0
        );
        foreach(Button button in buttons)
            button.Draw(gameTime, spriteBatch);
    }
}
