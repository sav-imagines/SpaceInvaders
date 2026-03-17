using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceDefence.Collision;

namespace SpaceDefence;

public class Button : Clickable {
    private SpriteFont font;
    private float FONT_SCALE = 4;

    public string Text {get; private set;}
    public Action ClickHandler {get; private set;}
    public RectangleCollider Position {get; private set;}

    public Button(Action clickHandler, Vector2 centerPosition, string text) {
        ClickHandler = clickHandler;
        Text = text;
        Position = new RectangleCollider(new Rectangle(centerPosition.ToPoint(), Point.Zero));
    }

    public override void HandleInput(InputManager inputManager)
        {
            if (inputManager.LeftMousePress() && Position.Contains(inputManager.GetMouseScreenPosition()))
                OnClick();
        }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        font = content.Load<SpriteFont>("Other/PixelFont");
        Vector2 textSize = font.MeasureString(Text) * FONT_SCALE;
        Position = new RectangleCollider(new Rectangle((Position.shape.Location.ToVector2() - textSize / 2).ToPoint(), textSize.ToPoint()));
    }

    public override void OnClick()
    {
        ClickHandler();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Camera Camera = GameManager.GetGameManager().Camera;
        base.Draw(gameTime, spriteBatch);
        spriteBatch.DrawString(font, Text, Position.shape.Location.ToVector2(), Color.White, 0, Vector2.Zero, Vector2.One * FONT_SCALE, SpriteEffects.None, 0);
    }
}
