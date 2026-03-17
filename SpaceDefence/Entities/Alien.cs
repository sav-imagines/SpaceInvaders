using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

internal class Alien : GameObject
{
    public CircleCollider _circleCollider { get; private set; }
    private Texture2D _texture;
    private float playerClearance = 200;
    private float Speed;

    public Alien() {
        Speed = 150;
    }

    public Alien(float speed) {
        Speed = speed;
    }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        _texture = content.Load<Texture2D>("Entities/Alien");
        _circleCollider = new CircleCollider(Vector2.Zero, _texture.Width / 2);
        SetCollider(_circleCollider);
        RandomMove();
    }

    public override void OnCollision(GameObject other)
    {
        if (other is Ship player)
            GameManager.GetGameManager().State = GameState.Gameover;
        GameManager.GetGameManager().WaveFactory.AlienDied();
        GameManager.GetGameManager().RemoveGameObject(this);
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 playerLocation = GameManager
            .GetGameManager()
            .Player.GetPosition()
            .Center.ToVector2();
        _circleCollider.Center +=
            (playerLocation - _circleCollider.Center).Normalized() * Speed * deltaTime;
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
