using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

public class Button : Clickable {
    private SpriteFont font;
    private float FONT_SCALE = 4;

    public string Text {get; private set;}
    public Action ClickHandler {get; private set;}
    public Vector2 Position {get; private set;}

    public Button(Action clickHandler, Vector2 centerPosition, string text) {
        ClickHandler = clickHandler;
        Text = text;
        Position = centerPosition;
    }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        font = content.Load<SpriteFont>("PixelFont");
    }

    public override void OnClick()
    {
        ClickHandler();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Camera Camera = GameManager.GetGameManager().Camera;
        base.Draw(gameTime, spriteBatch);
        Vector2 textSize = font.MeasureString(Text) * FONT_SCALE;
        Vector2 textPos = Camera.ToScreenSpace(Position - textSize / 2);
        spriteBatch.DrawString( font, Text, textPos, Color.White, 0, Vector2.Zero, Vector2.One * FONT_SCALE, SpriteEffects.None, 0);
    }
}
