using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence;

public class MainMenu : GameObject
{
    private const float FONT_SCALE = 4;

    private List<Text> textItems = [];
    private List<Button> buttons = [];
    private SpriteFont font;

    public MainMenu()
    {
        var screen = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        buttons = [
            new Button(() => {GameManager.GetGameManager().State = GameState.Playing;}, screen / 2 + Vector2.UnitY * screen.Y * 0.05f, "(A)  Play"),
            new Button(() => {GameManager.GetGameManager().Game.Exit();}, screen / 2 + Vector2.UnitY * screen.Y * 0.1f, "(-)  Quit")
        ];
        textItems = [
            new Text("MAIN  MENU", new Vector2(.5f, .4f)),
        ];

    }
    public override void Load(ContentManager content)
    {
        foreach(Button button in buttons)
            button.Load(content);
        foreach(Text text in textItems)
            text.Load(content);
        font = content.Load<SpriteFont>("PixelFont");
    }

    public override void HandleInput(InputManager inputManager)
        {
        if (inputManager.IsButtonPress(Buttons.A))
            GameManager.GetGameManager().State = GameState.Playing;
        else if (inputManager.IsKeyPress(Keys.Escape) || inputManager.IsButtonPress(Buttons.Back))
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


        foreach(Button button in buttons)
            button.Draw(gameTime, spriteBatch);
        foreach(Text text in textItems)
            text.Draw(gameTime, spriteBatch);
    }
}
