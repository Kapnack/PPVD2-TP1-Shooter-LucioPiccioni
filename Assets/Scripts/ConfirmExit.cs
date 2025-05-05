using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmExit : MonoBehaviour
{
    public void ConfirmedExit()
    {
#if UNITY_EDITOR || UNITY_WEBGL

        Debug.Log("Program Exit succesfully");
#else
    
        Application.Quit();

#endif

    }

    public void CanceledExit()
    {
        SceneManager.UnloadSceneAsync("ConfirmExit");
    }
}
