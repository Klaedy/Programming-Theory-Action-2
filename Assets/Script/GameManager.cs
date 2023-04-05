using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private AudioManager audioManagerScript;
    public int basicRocket;
    public int directionalRocket;
    public int coheteJuguete;
    public int mondongoCount;
    public int llaveAzoteaCount;
    public int coheteCuantico;
    public int chipCount;

    //MENÚ DE PAUSA
    public bool isPaused = false;
    public GameObject canvasPause;
    public GameObject canvasOpciones;

    //SEMAFORO
    public GameObject greenLight;
    public GameObject yellowLight;
    public GameObject redLight;

    //CANVAS CHIPS
    private GameObject chipsCanvas;
    public TextMeshProUGUI chipsText;
    public int chipLeft;


    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        audioManagerScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        chipsCanvas = GameObject.Find("ChipsCanvas");       
        chipCount = 0;
        chipLeft = 9;
        basicRocket = 0;
        coheteJuguete = 0;
        mondongoCount = 0;
        llaveAzoteaCount = 0;
        directionalRocket = 0;
        coheteCuantico = 0; //PONLO A 0 CABRÓN!!!!!!!!!!

        StartCoroutine(Semaforo());
    }

    // Update is called once per frame
    void Update()
    {
        //REINICIA ESCENA
        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
            //SceneRestart();
        //}

        //MENÚ DE PAUSA
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PauseGame(); //Detiene El tiempo;
        }
        PausedGame(); //Activa y desactiva el canvas;
        //

    }

    public void ChipsLeftUpdate (int _chipToRest)
    {
        chipLeft -= _chipToRest;
        chipsText.text = "Faltan " + chipLeft;
    }

    public void YouAreFullOfChips()
    {

    }
    void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    void PausedGame()
    {
        if (isPaused == true)
            canvasPause.SetActive(true);

        if (isPaused == false)
        {
            canvasPause.SetActive(false);
            canvasOpciones.SetActive(false);
        }           
    }

    public void FadeOut()
    {
        audioManagerScript.FadeOutOne();
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

    public void ChipUpdate(int _chipToAdd)
    {
        chipCount += _chipToAdd;
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

    public void DirectionalRocketNEVERMORE()
    {
        directionalRocket = 0;
    }

    public void PermitControlPolimorfico()
    {
        playerControllerScript.PermitControl();
    }

    public IEnumerator Semaforo()
    {
        float greenTime = 10.0f;
        float yellowTime = 3.0f;
        float redTime = 7.0f;
        while (true)
        {
            greenLight.SetActive(true);
            redLight.SetActive(false);
            yield return new WaitForSeconds(greenTime);
            yellowLight.SetActive(true);
            greenLight.SetActive(false);
            yield return new WaitForSeconds(yellowTime);
            redLight.SetActive(true);
            yellowLight.SetActive(false);
            yield return new WaitForSeconds(redTime);
        }
    }
}
