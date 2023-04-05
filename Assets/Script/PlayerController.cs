using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRb;
    private GameManager gameManagerScript;
    private StoryManager storyManagerScript;
    private AudioSource audioSource;
    public AudioClip chipClip;
    private BasicRocket basicRocketScript;
    private DirectionalRocket directionalRocketScript;
    private CoheteCuantico coheteCuanticoScript;
    private Animator animator;
    //Intercambio de controles con DIRECTIONAL ROCKET
    public bool directionalRocketPhoned = false; //Recibe un True desde el Script DirectionalRocket para obtener asignación de script. Retorna a estado False tras colision
    public bool directionalRocketAssigned = false; //Tapadera
    public bool bringBackTheControlDude = false; //Tras la colision de DirectionalRocket se le devuelve el control y la cámara al player. Leer indicaciones.
    //Intercambio de controles con COHETE CUANTICO
    public bool coheteCuanticoPhoned = false;
    public bool coheteCuanticoAssigned = false;
    public bool bringBackTheControlDudeCuantico = false;
    private MarketNPC marketNPCScript;
    public GameObject basicRocketPrefab;
    public GameObject coheteJuguetePrefab;
    public GameObject directionalRocketPrefab;
    public GameObject coheteCuanticoPrefab;
    public GameObject satelitesOn;
    private GameObject satelitesTrigger;
    private CinemachineVirtualCamera myCinemachine;

    private float speed = 5.0f;
    private float horizontalInput;
    private float verticalInput;
    Vector2 lookDirection = new Vector2(1, 0); //Las cosas a la cara!!!
    private Vector2 backDirection; //Las cosas a la espalda!!
    public Vector2 LookDirection { get { return lookDirection; } set { lookDirection = value; } }
    public bool dontMove = false; //Arrebata el control del personaje al jugador
    private GameObject escalerasTrigger;
    public bool addMoveY = false; //Controla si el jugador está o no en una escalera
    private Vector2 previousVelocity = Vector2.zero;

    //LANZAMIENTO
    private float stayBack = 2.0f;
    public bool kiteoBool = false; //Aleja al jugador e inicia el arranque del rocket   
    public bool startParticle = false; //Envía a los cohetes la señal de Play Particle System
    private bool isRocketPlanted = false;
    private float kiteoTimer = 2.0f; //Cuenta atrás del lanzamiento
    private float kiteoTimerMax = 2.0f; //Referencia que restaura kiteoTimer tras ser usado
    public bool safeDelay = false; //Previene activar el lanzamiento mediante missclick//puedo usarlo sin miedo ya que se desactiva automáticamente tras Lanzamiento()
    private int isFirstTimeAzoteaPrivate; //Creará texto intermitente en la azotea explicando cómo lanzar cohete//el trigger está en SatelitesTrigger
    public int isFirstTimeAzoteaPublic //ENCAPSULAMIENTO example
    {
        get { return isFirstTimeAzoteaPrivate; }
        set { isFirstTimeAzoteaPrivate = value; }
    }

    //HANGAR ON / OFF
    private GameObject solarTileset;
    private GameObject solarTriggerOn;
    private GameObject solarTriggerOff;

    private bool isInteriorOn = false;

    //CAMERA START
    private bool firstPlane = false;
    private bool secondPlane = false;
    private GameObject cameraStartObject;
    private GameObject canvasIntro;

    // Start is called before the first frame update
    void Start()
    {

        playerRb = GetComponent<Rigidbody2D>();
        kiteoTimerMax = 2.0f;
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        storyManagerScript = GameObject.Find("StoryManager").GetComponent<StoryManager>();
        marketNPCScript = GameObject.Find("Trader").GetComponent<MarketNPC>();
        audioSource = GetComponent<AudioSource>();
        solarTileset = GameObject.Find("Solar");
        solarTriggerOn = GameObject.Find("Colliders/TriggerHangarOn");
        solarTriggerOff = GameObject.Find("Colliders/TriggerHangarOff");
        escalerasTrigger = GameObject.Find("Triggers/EjeYEscalera");
        myCinemachine = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        satelitesTrigger = GameObject.Find("Triggers/SatelitesTrigger");
        Application.targetFrameRate = 60;
        isFirstTimeAzoteaPrivate = 0;
        animator = GetComponent<Animator>();
        cameraStartObject = GameObject.Find("CameraStart");
        canvasIntro = GameObject.Find("CanvasIntro");
        speed = 5.0f; //CAMBIALO A 5
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isRocketPlanted == true)
        {
            PrenderLaMecha();
        }

        if (kiteoBool == true)
        {
            CountDown();
        }

        if (!directionalRocketAssigned)
        {           
            if (directionalRocketPhoned == true)               
                DirectionalRocketAssigned(); //Tengo acceso al Script de DirectionalRocket. Cuando colisione, vuelve false que exista.                        
        }

        if (bringBackTheControlDude == true)
        {
            //Este es el lugar en el que todas las booleanas deben volver a su estado por defecto y se le devuelve al player el control y la cámara//
            BringMeBackEverythingDirectional();
        }

        if (!coheteCuanticoAssigned)
        {
            if (coheteCuanticoPhoned == true)
                CoheteCuanticoAssigned();
        }

        if (bringBackTheControlDudeCuantico == true)
        {
            BringMeBackEverythingCuantico();
        }

        if (firstPlane == false)
        {
            myCinemachine.Follow = cameraStartObject.transform;
            firstPlane = true;
        }

        if (firstPlane == true && Input.GetKeyDown(KeyCode.Space) && secondPlane == false)
        {
            canvasIntro.SetActive(false);
            myCinemachine.Follow = transform;
            secondPlane = true;
        }
    }

    private void FixedUpdate()
    {
        Movilidad();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameManagerScript.basicRocket >= 1 && collision.GetComponent<Collider2D>().tag == "Lanzadera" && Input.GetKeyDown(KeyCode.Space) && storyManagerScript.instalable == true)
        {
            Vector2 rocketPosition = new Vector2(transform.position.x, 0);
            Instantiate(basicRocketPrefab, rocketPosition, basicRocketPrefab.transform.rotation);
            startParticle = true;
            storyManagerScript.DetenerTextoIntermitente3();
            gameManagerScript.BasicRocketUpdate(-1);
            StartCoroutine(Fixing());
        }

        if (gameManagerScript.coheteJuguete >= 1 && collision.GetComponent<Collider2D>().tag == "Lanzadera" && Input.GetKeyDown(KeyCode.Space) && storyManagerScript.instalable2 == true)
        {
            Vector2 rocket2Position = new Vector2(transform.position.x, 0);
            Instantiate(coheteJuguetePrefab, rocket2Position, coheteJuguetePrefab.transform.rotation);

            gameManagerScript.CoheteJugueteUpdate(-1);
            StartCoroutine(Fixing2());
        }

        if (gameManagerScript.directionalRocket >= 1 && collision.GetComponent<Collider2D>().tag == "Lanzadera2" && Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 rocket3Position = new Vector2(transform.position.x, 20.4f);
            Instantiate(directionalRocketPrefab, rocket3Position, directionalRocketPrefab.transform.rotation);
            gameManagerScript.DirectionalRocketUpdate(-1);
            storyManagerScript.DetenerTextoIntermitente2();
            StartCoroutine(Fixing2());
        }

        if (gameManagerScript.coheteCuantico >= 1 && collision.GetComponent<Collider2D>().tag == "Lanzadera2" && Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 rocket3Position = new Vector2(transform.position.x, 20.4f);
            Instantiate(coheteCuanticoPrefab, rocket3Position, coheteCuanticoPrefab.transform.rotation);
            gameManagerScript.CoheteCuanticoUpdate(-1); //Aquí me pasé de listo->La contabilización de su destrucción en este tipo de cohete debe ir en su colisión.
            //storyManagerScript.DetenerTextoIntermitente2();
            StartCoroutine(Fixing2());
        }
    }

    IEnumerator Fixing()
    {
        yield return new WaitForSeconds(0.5f);
        isRocketPlanted = true;
    }

    IEnumerator Fixing2() //REUTILIZABLE//safeDelay automatizado//
    {
        yield return new WaitForSeconds(0.5f);
        safeDelay = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //MOVIMIENTO EJE Y
        if (collision.gameObject == escalerasTrigger)
        {
            addMoveY = true;
            previousVelocity = playerRb.velocity;
            playerRb.velocity = new Vector2(previousVelocity.x, 0f);
            playerRb.gravityScale = 0; // Desactiva la gravedad
            satelitesOn.SetActive(false);
        }
        //HANGAR
        if (collision.gameObject == solarTriggerOn)
        {
            isInteriorOn = true;
        }

        if (collision.gameObject == solarTriggerOff)
        {
            isInteriorOn = false;
        }

        if (collision.gameObject == satelitesTrigger && isFirstTimeAzoteaPrivate == 0)
        {           
            storyManagerScript.InstalarCohete2();
        }

        if (isFirstTimeAzoteaPrivate == 1)
        {
            //Debug.Log("FIRST TIME AZOTEA PRIVATE");
        }

            HangarVisible();
        //HANGAR

        //Mondongos//
        if (collision.CompareTag("Trigger") && gameManagerScript.mondongoCount == 0)
        {
            storyManagerScript.Mondongo2();
        }
        if (collision.CompareTag("Trigger") && gameManagerScript.mondongoCount == 1)
        {
            storyManagerScript.Mondongo3();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == escalerasTrigger)
        {
            addMoveY = false;
            playerRb.velocity = previousVelocity;
            playerRb.gravityScale = 1; // Desactiva la gravedad
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Chip"))
        {
            gameManagerScript.ChipUpdate(1);
            gameManagerScript.ChipsLeftUpdate(1);
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(chipClip);
        }
    }



    void PrenderLaMecha()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerRb.position + Vector2.up * 0.2f, lookDirection, 2.5f, LayerMask.GetMask("Rocket"));
        if (hit.collider != null)
        {
            kiteoBool = true;
            dontMove = true;
        }
    }

    void CountDown()
    {
        storyManagerScript.DetenerTextoIntermitente();
        transform.Translate(backDirection * stayBack * Time.deltaTime);
        basicRocketScript = GameObject.Find("BasicRocket(Clone)").GetComponent<BasicRocket>();
        kiteoTimer -= Time.deltaTime;
        if (kiteoTimer <= 0)
        {
            basicRocketScript.rocketRb.constraints = RigidbodyConstraints2D.None;
            basicRocketScript.rocketCollider2D.isTrigger = false;
            basicRocketScript.RocketLaunch();
            kiteoBool = false;
            //dontMove = false;
            kiteoTimer = kiteoTimerMax;
        }
    }

    void Movilidad()
    {
        if (dontMove == false && addMoveY == false)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);
            if (horizontalInput < -0.5f)
            {
                animator.SetFloat("Move X", -1);
                animator.SetFloat("Blend", 0);
            }
            if (horizontalInput > 0.5f)
            {
                animator.SetFloat("Move X", 1);
                animator.SetFloat("Blend", 0);
            }
            if (horizontalInput > -0.4f && horizontalInput < 0.4f)
            {
                animator.SetFloat("Move X", 0);
                animator.SetFloat("Blend", 0);
            }
        }
        else if (dontMove == false && addMoveY == true)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            playerRb.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);
            if (transform.position.y > 18)
            {
                Vector2 newPosition = transform.position;
                newPosition.y = 18;
                transform.position = newPosition;
            }
        }


        if (!Mathf.Approximately(horizontalInput, 0.0f))
        {
            lookDirection = new Vector2(horizontalInput, 0f).normalized;
        }
        backDirection = -lookDirection.normalized;
    }

    //void StartTalk()
    //{
    //  RaycastHit2D hit = Physics2D.Raycast(playerRb.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
    //if (hit.collider != null)
    //{
    //Conversación con el trader
    // if (marketNPCScript != null)
    //{
    //  marketNPCScript.Dialog();
    //}
    //}
    //}

    void HangarVisible()
    {
        if (isInteriorOn == true)
        {
            solarTileset.SetActive(false);
        }
        else
            solarTileset.SetActive(true);
    }

    public void CancelControl()
    {
        dontMove = true;
        playerRb.velocity = Vector2.zero;
    }

    public void PermitControl()
    {
        dontMove = false;
    }

    public void StaySafe()
    {
        dontMove = true;
        transform.Translate(backDirection * stayBack * Time.deltaTime);
    }

    //Con esta función puedo declarar hacia qué dirección mira el player en cualquier otro script
    public Vector2 GetPlayerPosition()
    {
        return playerRb.position;
    }

    public Vector2 GetLookDirection()
    {
        return LookDirection;
    }

    public void DirectionalRocketAssigned()
    {
        directionalRocketScript = GameObject.Find("CoheteDireccional(Clone)").GetComponent<DirectionalRocket>();
        directionalRocketAssigned = true;
    }
    public void BringMeBackEverythingDirectional()
    {       
        StartCoroutine(BringMeBackEveryThingDirectionalNumerator());
        directionalRocketAssigned = false;
        directionalRocketPhoned = false;
        bringBackTheControlDude = false;
    }
    public IEnumerator BringMeBackEveryThingDirectionalNumerator()
    {
        yield return new WaitForSeconds(2);
        myCinemachine.Follow = transform;       
        yield return new WaitForSeconds(1);
        PermitControl();
    }

    public IEnumerator BringMeBackEveryThingCuanticoNumerator()
    {
        yield return new WaitForSeconds(2);
        myCinemachine.Follow = transform;
        yield return new WaitForSeconds(1);
        PermitControl();
    }

    public void CuanticoBack()
    {
        StartCoroutine(CuanticoBackNumerator());
    }

    public IEnumerator CuanticoBackNumerator()
    {
        yield return new WaitForSeconds(4);       
        storyManagerScript.alreadyLooteableCC = true;
    }
        

    public void CoheteCuanticoAssigned()
    {
        coheteCuanticoScript = GameObject.Find("CoheteCuantico(Clone)").GetComponent<CoheteCuantico>();
        coheteCuanticoAssigned = true;
    }

    public void BringMeBackEverythingCuantico()
    {
        StartCoroutine(BringMeBackEveryThingCuanticoNumerator());
        coheteCuanticoAssigned = false;
        coheteCuanticoPhoned = false;
        bringBackTheControlDudeCuantico = false;
    }

    public void ChangeSpeed()
    {
        speed = 10f;
    }
}
