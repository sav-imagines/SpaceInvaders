using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence;

public class GameOverMenu : GameObject
{
    private List<Text> textItems = []; 
    private List<Button> buttons = [];

    public GameOverMenu()
    {
        var screen = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        buttons = [
            new Button(() => Continue(), screen / 2 + Vector2.UnitY * screen.Y * 0.05f, "(A)  Respawn"),
            new Button(() => ToMainMenu(), screen / 2 + Vector2.UnitY * screen.Y * 0.1f, "(-)  Main  menu")
        ];
        textItems = [
            new Text("GAME  OVER", new Vector2(.5f, .4f)),
        ];
    }

    public override void Load(ContentManager content)
    {
        foreach(Button button in buttons)
            button.Load(content);
        foreach(Text text in textItems)
            text.Load(content);
    }

    public override void HandleInput(InputManager inputManager)
        {
        if (inputManager.IsButtonPress(Buttons.A))
            Continue();
        else if (inputManager.IsKeyPress(Keys.Escape))
            ToMainMenu();

        foreach(Button button in buttons)
            button.HandleInput(inputManager);
        }

    public override void Update(GameTime gameTime)
    {
        foreach(Button button in buttons)
            button.Update(gameTime);
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
        foreach(Button button in buttons)
            button.Draw(gameTime, spriteBatch);
        foreach(Text text in textItems)
            text.Draw(gameTime, spriteBatch);
    }

    private void ToMainMenu()
    {
        GameManager.GetGameManager().GameOverReset();
        GameManager.GetGameManager().State = GameState.Mainmenu;
    }

    private void Continue() =>
        GameManager.GetGameManager().GameOverReset();
}
