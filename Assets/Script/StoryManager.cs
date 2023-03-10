using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private GameManager gameManagerScript;
    public TextMeshProUGUI installPropulsorText;
    public TextMeshProUGUI installAzoteaText;
    public bool storyStarted = false;
    public bool gotCraneo = false;   

    //TRIGGERS
    private GameObject triggerBasura;
    public GameObject lanzadera;
    public GameObject triggerPideLlaves;
    public bool instalable = false; //Crea un delay entre la caja de texto Mondongo2 y la instalación del cohete
    public bool instalable2 = false; //lo mismo para Cohete-Juguete
    public float instalableDelay = 0;

    //MONDONGOS
    public GameObject mondongo1;
    public GameObject mondongo2;
    public GameObject mondongo3;
    private bool existMondongo2;


    //TRADER
    private GameObject traderTalk2;

    //TRINO
    public bool alreadyExplotedCoheteJuguete = false;
    public GameObject trinoTalk2;
    public GameObject trinoTalk4;

    //FUNCIONALIDADES
    private Coroutine textoIntermitenteCoroutine;
    private Coroutine textoIntermitenteCoroutine2;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        traderTalk2 = GameObject.Find("NPCs/Trader/TraderTalk2");
        triggerBasura = GameObject.Find("Triggers/TriggerBasura");       
    }

    // Update is called once per frame
    void Update()
    {
       if (storyStarted == true)
        {
            StoryStarted();
        }

       if (alreadyExplotedCoheteJuguete == true)
        {
            TrinoTalk2();
        }

       if (gameManagerScript.llaveAzoteaCount == 3)
        {
            TrinoTalk4();
            gameManagerScript.Got1KeyAzotea();
        }
    }

    public void TriggerBasuraON()
    {
        triggerBasura.SetActive(true);
    }
    
    public void GotCraneo()
    {
        gotCraneo = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
    }

    IEnumerator TextoIntermitente()
    {
        //ELIMINAR//float tiempoEntreCiclos = 2.5f; // Intervalo de tiempo en segundos entre ciclos de la intermitencia
        float tiempoEncendido = 2f; // Tiempo en segundos que el objeto está encendido
        float tiempoApagado = 0.5f; // Tiempo en segundos que el objeto está apagado

        while (true)
        {
            installPropulsorText.gameObject.SetActive(true);
            yield return new WaitForSeconds(tiempoEncendido);
            installPropulsorText.gameObject.SetActive(false);
            yield return new WaitForSeconds(tiempoApagado);
        }
    }

    IEnumerator TextoIntermitente2()
    {
        float tiempoEncendido2 = 2f;
        float tiempoApagado2 = 0.5f;
        while (true)
        {
            installAzoteaText.gameObject.SetActive(true);
            yield return new WaitForSeconds(tiempoEncendido2);
            installAzoteaText.gameObject.SetActive(false);
            yield return new WaitForSeconds(tiempoApagado2);
        }
    }

    IEnumerator LanzaderaON()
    {
        yield return new WaitForSeconds(1);
        instalable = true;
        StopCoroutine(LanzaderaON());
    }

    public void Mondongo2()
    {
        mondongo2.SetActive(true);       
    }

    public void Mondongo3()
    {
        mondongo3.SetActive(true);
    }

    public void TrinoTalk2()
    {
        trinoTalk2.SetActive(true);
        alreadyExplotedCoheteJuguete = false;
    }

    public void InstalarCohete()
    {
        textoIntermitenteCoroutine = StartCoroutine(TextoIntermitente());
        StartCoroutine(LanzaderaON());
    }

    public void InstalarCohete2()
    {
        textoIntermitenteCoroutine2 = StartCoroutine(TextoIntermitente2());
    }

    public void DetenerTextoIntermitente()
    {
        if (textoIntermitenteCoroutine != null)
        {
            StopCoroutine(textoIntermitenteCoroutine);
            textoIntermitenteCoroutine = null;
            installPropulsorText.gameObject.SetActive(false);
        }
    }

    public void DetenerTextoIntermitente2()
    {
        if (textoIntermitenteCoroutine2 != null)
        {
            StopCoroutine(textoIntermitenteCoroutine2);
            playerControllerScript.isFirstTimeAzoteaPublic++;
            textoIntermitenteCoroutine2 = null;
            installAzoteaText.gameObject.SetActive(false);
        }
    }

    public void DelayLanzadera()
    {
        StartCoroutine(DelayForLanzadera());
    }
    IEnumerator DelayForLanzadera()
    {
        yield return new WaitForSeconds(0.5f);
        lanzadera.SetActive(true);
        instalable2 = true;
    }
    public void StoryStarted()
    {
        mondongo1.SetActive(true);
        playerControllerScript.PermitControl();
        traderTalk2.SetActive(true);
        storyStarted = false;
    }

    public void TrinoTalk4()
    {
        trinoTalk4.SetActive(true);
    }
  
}
