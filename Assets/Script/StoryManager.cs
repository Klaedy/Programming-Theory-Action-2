using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class StoryManager : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private GameObject playerGameObject;
    private AudioSource audioSource;
    public AudioClip teleportClip;
    private GameManager gameManagerScript;
    public TextMeshProUGUI installPropulsorText;
    public TextMeshProUGUI installAzoteaText;
    public TextMeshProUGUI installAzoteaText3;
    private CinemachineVirtualCamera myCinemachine;
    private ZoomEffect zoomEffectScript;
    public bool storyStarted = false;
    public bool gotCraneo = false;

    //TRIGGERS
    private GameObject triggerBasura;
    public GameObject lanzadera;
    public GameObject triggerPideLlaves;
    public GameObject lootDirectionalRocket; //Puedo lootear un directional Rocket de la estantería
    public GameObject noLootDirectionalRocket; //No puedo lootearlo
    public GameObject noMoreLootDirectionalRocket; //No puedo lootearlo NUNCA MÁS
    public GameObject lootCC; //Cohete Cuantico Looteable;
    public GameObject pulsadorGreen;
    public GameObject pulsadorRed;
    public GameObject triggerMurcielagos; //evita que se reproduzca audio de pájaros más de una vez.
    public Animator pulsadorContainer;
    public bool alreadyLooteableCC = false; //
    public bool canILoot = false;
    public bool instalable = false; //Crea un delay entre la caja de texto Mondongo2 y la instalación del cohete
    public bool instalable2 = false; //lo mismo para Cohete-Juguete
    public bool pulsadorPulsed = false; //activa la caída del container cuando CC se estrella contra pulsador
    public float instalableDelay = 0;
    public bool tengoAccesoCuantico = false; //En el momento que puedo lanzar cohetes cuanticos, dejo de poder lootear direccionales;

    //MONDONGOS
    public GameObject mondongo1;
    public GameObject mondongo2;
    public GameObject mondongo3;
    private bool existMondongo2;


    //TRADER
    private GameObject traderTalk2;

    //DEALER
    public GameObject dealerTalk5;

    //TRINO
    public bool alreadyExplotedCoheteJuguete = false;
    public GameObject trinoTalk2;
    public GameObject trinoTalk4;
    public AudioClip doorBoltClip;
    public GameObject cameraFocusDoor;
    public GameObject muroEscalera;

    //FUNCIONALIDADES
    private Coroutine textoIntermitenteCoroutine;
    private Coroutine textoIntermitenteCoroutine2;
    private Coroutine textoIntermitenteCoroutine3;

    //CONTAINER
    public AudioClip buttonOn;
    public bool ganchosMoving = false;
    public ParticleSystem murcielagosParticle;
    public bool murcielagosController = false;
    public bool murcielagosController2 = false;

    //SMOKE & START END
    public bool smokeBool = false; //Es activado desde el script de Ganchos justo tras estrellarse el container
    public ParticleSystem smokeParticle;
    public AudioClip bigExplosion;
    private AudioSource audioSourceBigExplosion;
    public GameObject traderNPC;
    public GameObject containerFinal;
    public GameObject dealerNPC;
    public GameObject dealerFinalNPC;

    // Start is called before the first frame update
    void Start()
    {       
        playerGameObject = GameObject.Find("Player");
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = doorBoltClip;
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        traderTalk2 = GameObject.Find("NPCs/Trader/TraderTalk2");
        triggerBasura = GameObject.Find("Triggers/TriggerBasura");
        audioSourceBigExplosion = GetComponent<AudioSource>();
        myCinemachine = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        zoomEffectScript = GameObject.Find("CameraManager").GetComponent<ZoomEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.chipCount == 9) //PONLO A 9 CABRÓN!!!!
        {
            dealerTalk5.SetActive(true);
        }

        if (storyStarted == true)
        {
            StartCoroutine(PauseAfterExplosion());
            storyStarted = false;
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

        if (smokeBool == true)
            SmokeContainer();

        //Poder o no lootear Directional Rocket de la estantería
        if (gameManagerScript.directionalRocket == 0 && canILoot == true && tengoAccesoCuantico == false)
            CanLoot();

        if (gameManagerScript.directionalRocket == 1 && canILoot == true && tengoAccesoCuantico == false)
            CantLoot();

        if (alreadyLooteableCC == true && tengoAccesoCuantico == true)
            CanLootCC();

        //Relativo a la escena del Container
        if (pulsadorPulsed == true)
            Pulsador();

        if(murcielagosController == true)
        {
            if (!murcielagosController2)
            {
                murcielagosParticle.Play();
                Destroy(triggerMurcielagos);
                murcielagosController2 = true;
            }
        }
    }

    public void SmokeContainer()
    {
        audioSourceBigExplosion.volume = 0.4f;
        audioSourceBigExplosion.clip = bigExplosion;
        audioSourceBigExplosion.Play();
        smokeParticle.Play();
        containerFinal.SetActive(true);
        dealerNPC.SetActive(false);
        dealerFinalNPC.SetActive(true);
        StartCoroutine(FinalContainer());
        smokeBool = false;
    }
    public void Pulsador()
    {
        pulsadorContainer.SetBool("pulsadorPulsed", pulsadorPulsed);
        StartCoroutine(PulsadorTurnRed());
        pulsadorPulsed = false; //Línea final obligatoria para evitar que se siga reproduciendo
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

    IEnumerator TextoIntermitente3()
    {
        float tiempoEncendido3 = 2f;
        float tiempoApagado3 = 0.5f;
        while (true)
        {
            installAzoteaText3.gameObject.SetActive(true);
            yield return new WaitForSeconds(tiempoEncendido3);
            installAzoteaText3.gameObject.SetActive(false);
            yield return new WaitForSeconds(tiempoApagado3);
        }
    }

    IEnumerator LanzaderaON()
    {
        yield return new WaitForSeconds(1);
        instalable = true;
        StopCoroutine(LanzaderaON());
    }

    IEnumerator FinalContainer()
    {
        yield return new WaitForSeconds(8);
        zoomEffectScript.ZoomIn();
        myCinemachine.Follow = playerGameObject.transform;
        yield return new WaitForSeconds(1);
        playerControllerScript.PermitControl();
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

    public void InstalarCohete3()
    {
        textoIntermitenteCoroutine3 = StartCoroutine(TextoIntermitente3());
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

    public void DetenerTextoIntermitente3()
    {
        if (textoIntermitenteCoroutine3 != null)
        {
            StopCoroutine(textoIntermitenteCoroutine3);
            textoIntermitenteCoroutine3 = null;
            installAzoteaText3.gameObject.SetActive(false);
        }
    }

    public void DetenerTextoIntermitente2()
    {
        if (textoIntermitenteCoroutine2 != null)
        {
            Debug.Log("debería parar el texto");
            StopCoroutine(textoIntermitenteCoroutine2);
            playerControllerScript.isFirstTimeAzoteaPublic++;
            installAzoteaText.gameObject.SetActive(false);
            textoIntermitenteCoroutine2 = null;           
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

    public IEnumerator PauseAfterExplosion()
    {
        yield return new WaitForSeconds(1);
        StoryStarted();
    }

    public void TriggerDirectionalLootOn()
    {
        canILoot = true;
    }
    public void CanLoot()
    {
        lootDirectionalRocket.SetActive(true);
        noLootDirectionalRocket.SetActive(false);
    }

    public void CantLoot()
    {
        lootDirectionalRocket.SetActive(false);
        noLootDirectionalRocket.SetActive(true);
    }

    public void CanLootCC()
    {
        if(gameManagerScript.coheteCuantico == 0)
        {
            lootCC.SetActive(true);
            audioSource.PlayOneShot(teleportClip);
            alreadyLooteableCC = false;
        }
    }

    public void GiveMeTheChipsLittleKiddo()
    {
        gameManagerScript.ChipUpdate(-15);
    }

    public void AccesoCuanticoTrue()
    {
        tengoAccesoCuantico = true;
        lootDirectionalRocket.SetActive(false);
        noLootDirectionalRocket.SetActive(false);
        noMoreLootDirectionalRocket.SetActive(true);
    }

    public void TrinoOpenDoor()
    {
        
    }

    public void CameraGoToDoor()
    {
        myCinemachine.Follow = cameraFocusDoor.transform;
        StartCoroutine(CameraBackToPlayerRoutine());
    }

    public IEnumerator CameraBackToPlayerRoutine()
    {
        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(doorBoltClip);
        muroEscalera.SetActive(false);
        yield return new WaitForSeconds(1);
        myCinemachine.Follow = playerGameObject.transform;
        playerControllerScript.PermitControl();
    }

    public IEnumerator PulsadorTurnRed()
    {
        yield return new WaitForSeconds(1);
        pulsadorGreen.SetActive(false);
        pulsadorRed.SetActive(true);
        audioSource.PlayOneShot(buttonOn);       
        pulsadorPulsed = false;
        StartCoroutine(PulsadorTurnRedDelay());
    }

    public IEnumerator PulsadorTurnRedDelay()
    {
        yield return new WaitForSeconds(2);
        ganchosMoving = true;
    }
        
        
}
