using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //public void LoadByIndex(int sceneIndex)
    public void LoadSceneByName(string sceneName)
    {
        //SceneManager.LoadScene (sceneIndex);
        SceneManager.LoadScene(sceneName);
    }
}
