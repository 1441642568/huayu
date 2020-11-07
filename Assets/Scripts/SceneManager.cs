using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    public void OnStartGame(int sceneIndex)
    {
        Application.LoadLevel(sceneIndex);
    }
}
