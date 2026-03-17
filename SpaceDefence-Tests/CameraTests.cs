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
        Assert.AreEqual(position, camera.ToScreenSpace(camera.ToWorldSpace(position)));

        camera = new Camera(GameManager.GetGameManager(), new Rectangle(128, 128, 128, 128));
        Assert.AreEqual(position, camera.ToScreenSpace(camera.ToWorldSpace(position)));
    }

    [TestMethod]
    public void TestIsInBounds()
    {
        Camera camera = new Camera(GameManager.GetGameManager(), new Rectangle(0, 0, 128, 128));
        Vector2 position = camera.Viewport.Center.ToVector2();
        Assert.IsTrue(camera.Viewport.Contains(position));
        camera.Viewport = new Rectangle(
            new Point(camera.Viewport.Width * 3, 0),
            camera.Viewport.Size
        );
        Assert.IsFalse(camera.Viewport.Contains(position));

        camera = new Camera(GameManager.GetGameManager(), new Rectangle(20, 20, 128, 128));
        position = camera.Viewport.Center.ToVector2();
        Assert.IsTrue(camera.Viewport.Contains(position));
        Assert.IsFalse(camera.Viewport.Contains(new Vector2(0, 0)));
        Assert.IsTrue(camera.Viewport.Contains(new Vector2(21, 21)));

        // simulate a moved camera, taking a position relative to it, and see if it's in bounds now
        camera = new Camera(
            GameManager.GetGameManager(),
            new Rectangle(new Point(500, 500), camera.Viewport.Size)
        );

        // take a position relative to camera and attempt to transform into world space
        Vector2 newPosition = camera.ToWorldSpace(camera.Viewport.Size.ToVector2() / 2);

        // check that using 'ToWorldSpace' transforms it along with the camera
        Console.WriteLine(newPosition);
        Assert.IsTrue(camera.IsOnScreen(new CircleCollider(newPosition, 0)));
    }

    [TestMethod]
    public void NoDisplacementAtCenter()
    {
        Camera camera = new(GameManager.GetGameManager(), new Rectangle(0, 0, 128, 128));
        Vector2 position = camera.Viewport.Center.ToVector2();
        Assert.AreEqual(camera.ToWorldSpace(position), position);
        Assert.AreEqual(camera.ToScreenSpace(position), position);
    }

    [TestMethod]
    public void ScreenSpaceAndWorldSpaceAreOpposites()
    {
        Camera camera = new(GameManager.GetGameManager(), new Rectangle(10, 10, 128, 128));
        Vector2 position = new(10, 10);
        Assert.AreEqual(camera.ToScreenSpace(position), new(0, 0));
        Assert.AreEqual(camera.ToWorldSpace(position), new(20, 20));
    }
}
