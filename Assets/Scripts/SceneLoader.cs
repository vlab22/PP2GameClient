using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;

    [ContextMenu("Load Scene From Scene Name")]
    public void LoadSceneFromSceneName()
    {
        LoadScene(sceneName);
    }
    
    public void LoadScene(string pSceneName)
    {
        SceneManager.LoadScene(pSceneName);
    }
    
}
