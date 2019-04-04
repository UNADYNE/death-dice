using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class US_AutoLevelLoader : MonoBehaviour
{

    public float loadDelay;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene(1);
        }
    }

}
