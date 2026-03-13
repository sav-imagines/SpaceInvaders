using Microsoft.Xna.Framework;

using SpaceDefence;

namespace SpaceDefence_Tests;

[TestClass]
public sealed class CameraTests
{
    [TestMethod]
    public void TestToWorldSpace()
    {
        Vector2 position = new(0, 0);
        Camera camera = new(GameManager.GetGameManager(), new Rectangle(0, 0, 128, 128));
        Assert.AreEqual(position, camera.ToWorldSpace(camera.ToScreenSpace(position)));

        camera = new Camera(GameManager.GetGameManager(), new Rectangle(128, 128, 128, 128));
        Assert.AreEqual(position, camera.ToWorldSpace(camera.ToScreenSpace(position)));
    }

    [TestMethod]
    public void TestIsInBounds()
    {
        Camera camera = new Camera(GameManager.GetGameManager(), new Rectangle(0, 0, 128, 128));
        Vector2 position = camera.Viewport.Center.ToVector2();
        Assert.IsTrue(camera.Viewport.Contains(position));
        camera.Viewport = new Rectangle(new Point(camera.Viewport.Width * 3, 0), camera.Viewport.Size);
        Assert.IsFalse(camera.Viewport.Contains(position));
    }

    [TestMethod]
    public void NoDisplacementAtCenter()
    {
        Camera camera = new(GameManager.GetGameManager(), new Rectangle(0, 0, 128, 128));
        Vector2 position = camera.Viewport.Center.ToVector2();
        Assert.AreEqual(camera.ToScreenSpace(position), position);
        Assert.AreEqual(camera.ToWorldSpace(position), position);
    }

    [TestMethod]
    public void ScreenSpaceAndWorldSpaceAreOpposites()
    {
        Camera camera = new(GameManager.GetGameManager(), new Rectangle(10, 10, 128, 128));
        Vector2 position = new(10, 10);
        Assert.AreEqual(camera.ToWorldSpace(position), new(0, 0));
        Assert.AreEqual(camera.ToScreenSpace(position), new(20, 20));
    }
}
