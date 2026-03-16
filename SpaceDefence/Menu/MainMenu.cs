using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

public class MainMenu : GameObject
{
    List<Button> buttons = [];

    public override void Load(ContentManager content)
    {
        base.Load(content);
        // TODO: get screen size live?

        // PROBLEM: this crashes
        // var screen = GameManager.GetGameManager().Camera.Viewport.Size.ToVector2();
        var screen = new Vector2(1920, 1080);
        buttons = [
            new Button(() => { GameManager.GetGameManager().State = GameState.Playing;}, screen / 2 + Vector2.UnitY * screen.Y * 0.1f, "Play"),
            new Button(() => {GameManager.GetGameManager().Game.Exit();}, screen / 2 + Vector2.UnitY * screen.Y * 0.1f, "Quit")
        ];
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
    }
}
