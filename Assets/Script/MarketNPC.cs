using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketNPC : MonoBehaviour
{
    private GameManager gameManagerScript;
    private int scriptLine;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        scriptLine = 0;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dialog()
    {
        if (scriptLine == 0)
        {
            Debug.Log("Te hace entrega del cohete");
            gameManagerScript.BasicRocketUpdate(1);
            scriptLine++;

            return;
        }

        if (scriptLine == 1)
        {
            Debug.Log("Esto es el segundo mensaje");
            scriptLine++;
            return;
        }

        if (scriptLine == 2)
        {
            Debug.Log("Esto es el tercer mensaje");
        }
    }
}
