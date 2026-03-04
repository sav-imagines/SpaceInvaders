using Microsoft.Xna.Framework;

namespace SpaceDefence;

public class Camera
{
    public const float MAX_SPEED = 400;
    public Rectangle Viewport { get; set; }
    private GameManager Game { get; set; }

    public Camera(GameManager game, Rectangle viewport)
    {
        Game = game;
        Viewport = viewport;
    }

    public Matrix GetScreenSpaceMatrix(Rectangle viewport)
    {
        // Via https://community.khronos.org/t/2d-graphics-with-perspective-projection/36811/10
        return new Matrix(
            ((viewport.Right - viewport.Left) / viewport.Width), 0, 0, viewport.Left,
            0, ((viewport.Bottom - viewport.Top) / viewport.Height), 0, viewport.Top,
            0, 0, 1, 0,
            0, 0, 0, 1
        );
    }

    public void CenterCameraToWorldPosition(Vector2 coordinates) {
        Viewport = new Rectangle((coordinates - (Viewport.Size.ToVector2() / 2)).ToPoint(), Viewport.Size);
    }
}
