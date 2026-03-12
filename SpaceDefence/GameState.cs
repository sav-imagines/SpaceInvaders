public enum GameState
{
    Playing,
    Paused,
    Gameover,
    Mainmenu,
}

public static class GameStateMethods
{
    public static bool IsPlaying(this GameState state)
    {
        return state == GameState.Playing;
    }
}
