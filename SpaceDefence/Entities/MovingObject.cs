using Microsoft.Xna.Framework;

namespace SpaceDefence;

public abstract class MovingObject : GameObject {
    public Vector2 Velocity {get; set; }
}
