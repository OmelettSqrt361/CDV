using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHopper : MonoBehaviour
{

    public void SceneHop(int id)
    {
        SceneManager.LoadScene(id);
    }
}
