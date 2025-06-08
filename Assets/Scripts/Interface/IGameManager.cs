public interface IGameManager
{
    public int CurrentLevelIndex { get; }

    public void HideCursor();

    public void ShowCursor();

    public void LoadMainMenu();

    public void LoadTutorial();

    public void LoadCurrentLevel();

    public void LevelCompleted();
}
