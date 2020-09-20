using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void Start()
    {
        Debug.Log("START");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Log()
    {
        Debug.Log("CLICK");
    }
}
