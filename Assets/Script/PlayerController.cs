using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private GameManager gameManagerScript;
    private BasicRocket basicRocketScript;
    private MarketNPC marketNPCScript;
    public GameObject basicRocketPrefab;
    
    private float speed = 15.0f;
    private float horizontalInput;
    Vector2 lookDirection = new Vector2(1, 0); //Las cosas a la cara!!!
    private Vector2 backDirection; //Las cosas a la espalda!!
    public bool dontMove = false; //Arrebata el control del personaje al jugador
    

    //LANZAMIENTO
    private float stayBack = 2.0f;
    public bool kiteoBool = false; //Aleja al jugador e inicia el arranque del rocket
    private bool isRocketPlanted = false;
    private float kiteoTimer = 2.0f; //Cuenta atrás del lanzamiento
    private float kiteoTimerMax = 2.0f; //Referencia que restaura kiteoTimer tras ser usado
    

    //HANGAR ON / OFF
    private GameObject solarTileset;
    private GameObject solarTriggerOn;
    private GameObject solarTriggerOff;
    private bool isInteriorOn = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        kiteoTimerMax = 2.0f;
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        marketNPCScript = GameObject.Find("Trader").GetComponent<MarketNPC>();
        solarTileset = GameObject.Find("Solar");
        solarTriggerOn = GameObject.Find("TriggerHangarOn");
        solarTriggerOff = GameObject.Find("TriggerHangarOff");
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {          
            StartTalk();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isRocketPlanted == true)
        {
            PrenderLaMecha();
        }

        if (kiteoBool == true)
        {
            CountDown();
        }
    }

    private void FixedUpdate()
    {
        Movilidad();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameManagerScript.basicRocket >= 1 && collision.GetComponent<Collider2D>().tag == "Lanzadera" && Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 rocketPosition = new Vector2(transform.position.x, 0);
            Instantiate(basicRocketPrefab, rocketPosition, basicRocketPrefab.transform.rotation);
            gameManagerScript.BasicRocketUpdate(-1);
            StartCoroutine(Fixing());
        }
    }

    IEnumerator Fixing()
    {
        yield return new WaitForSeconds(0.5f);
        isRocketPlanted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
       

        //HANGAR
        if (collision.gameObject == solarTriggerOn)
        {
            isInteriorOn = true;
        }

        if (collision.gameObject == solarTriggerOff)
        {
            isInteriorOn = false;
        }
        HangarVisible();
        //HANGAR
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
        transform.Translate(backDirection * stayBack * Time.deltaTime);
        basicRocketScript = GameObject.Find("BasicRocket(Clone)").GetComponent<BasicRocket>();
        kiteoTimer -= Time.deltaTime;
        if (kiteoTimer <= 0)
        {
            basicRocketScript.rocketRb.constraints = RigidbodyConstraints2D.None;
            basicRocketScript.rocketCollider2D.isTrigger = false;
            basicRocketScript.RocketLaunch();
            kiteoBool = false;
            dontMove = false;
            kiteoTimer = kiteoTimerMax;
        }
    }

    void Movilidad()
    {
        if (dontMove == false)
        {
            //Vector2 position = playerRb.position;
            horizontalInput = Input.GetAxis("Horizontal");
            //transform.Translate(Vector2.right * horizontalInput * speed * Time.deltaTime);
            //playerRb.MovePosition(position);
            playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);
        }
        
        if (!Mathf.Approximately(horizontalInput, 0.0f))
        {
            lookDirection = new Vector2(horizontalInput, 0f).normalized;
        }
        backDirection = -lookDirection.normalized;
    }

    void StartTalk()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerRb.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            //Conversación con el trader
           if (marketNPCScript != null)
            {
                marketNPCScript.Dialog();
            }
        }
    }
    
    void HangarVisible()
    {
        if (isInteriorOn == true)
        {
            solarTileset.SetActive(false);
        }
        else
            solarTileset.SetActive(true);
    }
}
