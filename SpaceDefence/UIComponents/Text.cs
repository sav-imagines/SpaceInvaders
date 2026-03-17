using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

public class Text : GameObject
{
    private const float FONT_SCALE = 4;

    private string Content;
    private static SpriteFont Font;
    private Vector2 Center;

    private float _size;

    public float Size {
        get {
            return Size * FONT_SCALE;
        } set {
            _size = value;
        }
    }


    public Text(string text, Vector2 center, float size = 1)
    {
        Content = text;
        Center = center;
        Size = size;
    }

    public override void Load(ContentManager content)
    {
        Font ??= content.Load<SpriteFont>("PixelFont");
    }


    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Vector2 screenSize = GameManager.GetGameManager().Camera.Viewport.Size.ToVector2();
        Vector2 sizeA = Font.MeasureString(Content) * FONT_SCALE;
        Vector2 posA = new Vector2(sizeA.X * -.5f, -sizeA.Y * -.5f) + screenSize * Center;
        GameManager.GetGameManager().DrawRectangle(new Rectangle(posA.ToPoint(), sizeA.ToPoint()), Color.Blue, spriteBatch);
        spriteBatch.DrawString(
            Font,
            Content,
            posA,
            Color.White,
            0,
            Vector2.Zero,
            Vector2.One * FONT_SCALE,
            SpriteEffects.None,
            0
            );
    }
}
