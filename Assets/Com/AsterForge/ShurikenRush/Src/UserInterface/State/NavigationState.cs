namespace Com.AsterForge.ShurikenRush.UserInterface.State
{
    public enum GameStateType
    {
        MainMenu,
        Loading,
        LevelSelector,
        Settings,
        Ingame,
        PauseMenu,
        Victory,
        Defeat,
    }
    
    public static class GameState
    {
        public static GameStateType CurrentState = GameStateType.MainMenu;
    }
}