using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadScene : MonoBehaviour {

	public void LoadSceneCustom(string sScene)
    {
        SceneManager.LoadScene(sScene);
    }
}
