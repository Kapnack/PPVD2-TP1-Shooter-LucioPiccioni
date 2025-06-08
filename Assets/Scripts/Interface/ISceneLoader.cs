using UnityEngine.SceneManagement;

public interface ISceneLoader
{
    public bool IsSceneLoaded(SceneRef sceneRef);

    /// <summary>
    /// Loads the Scene and wait until it finish loading.
    /// After that it saves it in a list so it can be unloaded later with the other scenes on the list.
    /// </summary>
    /// <param name="sceneRef"></param>
    /// <param name="mode"></param>
    public void LoadScene(SceneRef sceneRef, LoadSceneMode mode = LoadSceneMode.Additive);

    /// <summary>
    /// Loads the Scenes and waits until they finish loading.
    /// After that it saves them in a list so they can be unloaded later.
    /// </summary>
    /// <param name="sceneRef"></param>
    /// <param name="mode"></param>
    public void LoadSceneAsync(SceneRef[] sceneRef, LoadSceneMode mode = LoadSceneMode.Additive);

    /// <summary>
    /// Unloads all active scene saved in the list inside the clase.
    /// </summary>
    public void UnloadAll();

    /// <summary>
    /// Unloads all active scene saved in the list inside the clase.
    /// Exept the one you declare as exeptions
    /// </summary>
    /// <param name="exeption"></param>
    public void UnloadAll(SceneRef exeption = null);

    /// <summary>
    /// Unloads all active scene saved in the list inside the clase.
    /// Exept the ones you declare as exeptions
    /// </summary>
    /// <param name="exeptions"></param>
    public void UnloadAll(SceneRef[] exeptions = null);
}
