using System.Collections.Generic;

namespace SpaceDefence;

public enum GameState
{
    Playing,
    Paused,
    Gameover,
    Mainmenu,
}

public static class GameStateMethods
{
    public static readonly Dictionary<GameState, GameObject> Screens = new Dictionary<
        GameState,
        GameObject
    >(
        collection: new List<KeyValuePair<GameState, GameObject>>
        {
            new KeyValuePair<GameState, GameObject>(GameState.Gameover, new GameOverMenu()),
            new KeyValuePair<GameState, GameObject>(GameState.Paused, new PausedMenu()),
            new KeyValuePair<GameState, GameObject>(GameState.Mainmenu, new MainMenu()),
        }
    );

    public static bool IsPlaying(this GameState state)
    {
        return state == GameState.Playing;
    }
}
