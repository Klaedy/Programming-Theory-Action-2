using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController playerControllerScript;
    public int basicRocket;
    public int directionalRocket;
    public int coheteJuguete;
    public int mondongoCount;
    public int llaveAzoteaCount;
    public int coheteCuantico;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        basicRocket = 0;
        coheteJuguete = 0;
        mondongoCount = 0;
        llaveAzoteaCount = 0;
        directionalRocket = 1;
        coheteCuantico = 0; //PONLO A 0 CABRÓN!!!!!!!!!!
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

    public void CoheteJugueteUpdate(int _coheteJugueteToAdd)
    {
        coheteJuguete += _coheteJugueteToAdd;
    }

    public void MondongoUpdate(int _mondongoToAdd)
    {
        mondongoCount += _mondongoToAdd;
    }

    public void LlaveAzoteaUpdate(int _llaveAzoteaToAdd)
    {
        llaveAzoteaCount += _llaveAzoteaToAdd;
    }

    public void DirectionalRocketUpdate(int _directionalRocketToAdd)
    {
        directionalRocket += _directionalRocketToAdd;
    }

    public void Got1DirectionalRocket()
    {
        DirectionalRocketUpdate(1);
    }

    public void CoheteCuanticoUpdate(int _coheteCuanticoToAdd)
    {
        coheteCuantico += _coheteCuanticoToAdd;
    }

    public void Got1CoheteCuantico()
    {
        CoheteCuanticoUpdate(1);
    }

    public void Got1RocketJuguete()
    {
        CoheteJugueteUpdate(1);
    }

    public void Got1Mondongo()
    {
        MondongoUpdate(1);
    }

    public void Got1KeyAzotea()
    {
        LlaveAzoteaUpdate(1);
    }

    public void PermitControlPolimorfico()
    {
        playerControllerScript.PermitControl();
    }
}
