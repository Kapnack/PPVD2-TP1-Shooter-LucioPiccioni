using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneLoader
{
    public void LoadScene(int index, LoadSceneMode mode, bool Async = false);
    public void LoadScene(int[] scenes, LoadSceneMode mode, bool Async = false);
    public void LoadScene(string scene, LoadSceneMode mode, bool Async = false);
    public void LoadScene(string[] scenes, LoadSceneMode mode, bool Async = false);

    public void UnloadScene(int index);
    public void UnloadScene(int[] index);
    public void UnloadScene(string scene);
    public void UnloadScene(string[] scene);
}
