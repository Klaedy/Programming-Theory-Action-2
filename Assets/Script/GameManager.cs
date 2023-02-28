using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int basicRocket;
    // Start is called before the first frame update
    void Start()
    {
        basicRocket = 0;
    }

    // Update is called once per frame
    void Update()
    {   
        
        
        //REINICIA ESCENA
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneRestart();
        }
    }

    void SceneRestart()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BasicRocketUpdate(int _basicRocketToAdd)
    {
        basicRocket += _basicRocketToAdd;
    }
}
