using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartSimulation : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
