using System;
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
        // Viewport.Offset(100, 100);
    }

    public Matrix GetScreenSpaceMatrix()
    {
        // return Matrix.Invert(Matrix.Identity);
        // return Matrix.CreateOrthographicOffCenter(Viewport, 0, 1);
        // DOES NOT WORK AFTER MOVING

        // Console.WriteLine(
        //     $"Location: {Viewport.Location.ToVector2()}, Size: {Viewport.Size.ToVector2()}, Matrix: {Viewport.Location.ToVector2() / Viewport.Size.ToVector2()}"
        // );
        return Matrix.CreateTranslation(Viewport.Left, Viewport.Top, 0);
        // return Matrix.CreateOrthographicOffCenter(0, Viewport.Width, Viewport.Height, 0, -1, 1);
        // Via https://community.khronos.org/t/2d-graphics-with-perspective-projection/36811/10
        // return new Matrix(
        //     1, 0, (float)Viewport.Left / (float)Viewport.Width, 0,
        //     0, 1, (float)Viewport.Top / (float)Viewport.Height, 0,
        //     0, 0, 1, 0,
        //     0, 0, 0, 1
        // );
    }

    public void CenterCameraToWorldPosition(Vector2 coordinates)
    {
        Viewport = new Rectangle(
            ((Viewport.Size.ToVector2() / 2) - coordinates).ToPoint(),
            Viewport.Size
        );
        // Viewport = new Rectangle((coordinates - (Viewport.Size.ToVector2() / 2)).ToPoint(), Viewport.Size);
    }

    public Vector2 ToWorldSpace(Vector2 vec)
    {
        return Vector2.Transform(vec, Matrix.Invert(GetScreenSpaceMatrix()));
    }

    public Vector2 ToScreenSpace(Vector2 vec)
    {
        return Vector2.Transform(vec, GetScreenSpaceMatrix());
    }

    public bool IsOnScreen(Vector2 worldSpacePos)
    {
        return Viewport.Contains(ToScreenSpace(worldSpacePos));
    }
}
