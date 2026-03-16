using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

internal class Alien : GameObject
{
    private float TOP_SPEED = 300;
    public CircleCollider _circleCollider { get; private set; }
    private Texture2D _texture;
    private float playerClearance = 100;
    private float speed = 150;

    public Alien() { }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        _texture = content.Load<Texture2D>("Alien");
        _circleCollider = new CircleCollider(Vector2.Zero, _texture.Width / 2);
        SetCollider(_circleCollider);
        RandomMove();
    }

    public override void OnCollision(GameObject other)
    {
        if (other is Ship player)
            GameManager.GetGameManager().State = GameState.Gameover;
        RandomMove();
        speed = speed > TOP_SPEED ? TOP_SPEED : speed + 30;
        base.OnCollision(other);
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 playerLocation = GameManager
            .GetGameManager()
            .Player.GetPosition()
            .Center.ToVector2();
        _circleCollider.Center +=
            (playerLocation - _circleCollider.Center).Normalized() * speed * deltaTime;
    }

    public void RandomMove()
    {
        GameManager gm = GameManager.GetGameManager();
        _circleCollider.Center = gm.RandomScreenLocation();

        Vector2 centerOfPlayer = gm.Player.GetPosition().Center.ToVector2();
        while ((_circleCollider.Center - centerOfPlayer).Length() < playerClearance)
            _circleCollider.Center = gm.RandomScreenLocation();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _circleCollider.GetBoundingBox(), Color.White);
        base.Draw(gameTime, spriteBatch);
    }
}
