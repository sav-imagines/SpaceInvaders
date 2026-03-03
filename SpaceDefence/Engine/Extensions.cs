using System;
using Microsoft.Xna.Framework;

public static class Extensions
{
    public static float Angle(this Vector2 vec)
    {
        return (float)Math.Atan2(vec.Y, vec.X) + ((float)Math.PI / 2);
    }

    public static Vector2 Rotated(this Vector2 vec, float angle)
    {
        Vector2 newVec = new(vec.X, vec.Y);
        newVec.Rotate(angle);
        return newVec;
    }
}
