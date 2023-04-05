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
        //scriptLine = 0;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EntregaPropulsor()
    {
        gameManagerScript.BasicRocketUpdate(1);
        
    }
}
