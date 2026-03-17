using SpaceDefence.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

internal class Supply : GameObject
{
    public RectangleCollider RectangleCollider {get; private set;}
    private Texture2D _texture;
    private float playerClearance = 100;

    public Supply()
    {

    }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        _texture = content.Load<Texture2D>("Entities/Crate");
        RectangleCollider = new RectangleCollider(_texture.Bounds);

        SetCollider(RectangleCollider);
        RandomMove();
    }

    public override void OnCollision(GameObject other)
    {
        RandomMove();
        if (other is Ship)
            GameManager.GetGameManager().Player.Buff();
        base.OnCollision(other);
    }

    public void RandomMove()
    {
        GameManager gm = GameManager.GetGameManager();
        RectangleCollider.shape.Location = (gm.RandomScreenLocation() - RectangleCollider.shape.Size.ToVector2() / 2).ToPoint();

        Vector2 centerOfPlayer = gm.Player.GetPosition().Center.ToVector2();
        while ((RectangleCollider.shape.Center.ToVector2() - centerOfPlayer).Length() < playerClearance)
            RectangleCollider.shape.Location = gm.RandomScreenLocation().ToPoint();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, RectangleCollider.shape, Color.White);
        base.Draw(gameTime, spriteBatch);
    }
}
