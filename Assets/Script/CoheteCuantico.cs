using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CoheteCuantico : CoheteSimple
{
    public float upForce = 400.0f;
    public float directionalForce = 20.0f;
    public float teleport = 5.0f;
    private bool rbAssigned = false;
    private Rigidbody2D rocketRb;
    public Animator coheteCuanticoAnimator;
    public ParticleSystem fireParticleBase1;
    public ParticleSystem fireParticleBase2;
    public ParticleSystem fireParticleLeft;
    public ParticleSystem fireParticleRight;
    public ParticleSystem smokeParticle;
    public ParticleSystem teleportParticle;
    public GameObject lightParticleBase1;
    public GameObject lightParticleBase2;
    public GameObject lightParticleLeft;
    public GameObject lightParticleRight;
    public bool lightParticlesOff;
    public bool isMurcielagosOn = false;
    public GameObject smokePlatform;
    public GameObject explosionPrefab;
    public AudioClip turboClip;
    public AudioClip explosionClip;
    public AudioClip teleportClip;
    public AudioClip birdsClip;
    private AudioSource audioSourceClip;
    private AudioSource audioSourceClipMurcielagos;
    public AudioClip errorButtonClip;
    private AudioSource audioSourceBase;
    private bool audioSourceBasePressed = false;
    private AudioSource audioSourceLeft;
    private bool audioSourceLeftPressed = false;
    private AudioSource audioSourceRight;
    private bool audioSourceRightPressed = false;
    private AudioSource audioSourceStart;
    private AudioSource audioSourceTeleport;
    private bool audioSourceStartOn;
    private bool audioSourceDirectionalAssigned = false;
    private Vector2 whereAmI;
    private bool alreadyPhoned = false;    
    //Sustituye las lanzaderas por antiRockets en la funci�n NoMoreRockets
    private bool collidersChanged = false;
    private Collider2D lanzadera1;
    private Collider2D antiRocket1;
    private Collider2D lanzadera2;
    private Collider2D antiRocket2;

    // NO PUEDE HABER START


    // Update is called once per frame
    public void Update()
    {
        Lanzamiento();
        whereAmI = transform.position;

        if (!lightParticlesOff)
            LightStartOff();
    }


    public override void Lanzamiento()
    {
        base.Lanzamiento();
        if (!audioSourceDirectionalAssigned)
            AudioSourceAsign();

        if (!alreadyPhoned)
            AlreadyPhoned();


        speed = 500.0f;


        if (isLaunched == true)
        {
            StartCoroutine(SmokingTime());
            if (!audioSourceStartOn)
            {
                audioSourceStart.Play();
                audioSourceStartOn = true;
            }

            if (smokePlatform != null)
                Destroy(smokePlatform);

            if (!rbAssigned)
                AsignarRigidBody2D();

            if (!collidersChanged)
                NoMoreRockets(); //Sustituye los Colliders de las lanzaderas por AntiRockets

            myCinemachine.Follow = transform;
            if (Input.GetKey(KeyCode.W))
            {
                rocketRb.AddForce(transform.up * upForce, ForceMode2D.Force);
                fireParticleBase1.Play();
                fireParticleBase2.Play();
                lightParticleBase1.SetActive(true);
                lightParticleBase2.SetActive(true);
                if (!audioSourceBasePressed)
                {
                    audioSourceBase.Play();
                    audioSourceBasePressed = true;
                }

            }
            else
            {
                fireParticleBase1.Stop();
                fireParticleBase2.Stop();
                lightParticleBase1.SetActive(false);
                lightParticleBase2.SetActive(false);
                audioSourceBase.Stop();
                audioSourceBasePressed = false;
            }

            if (Input.GetKey(KeyCode.A))
            {
                rocketRb.AddTorque(-directionalForce * Time.fixedDeltaTime);
                fireParticleLeft.Play();
                lightParticleLeft.SetActive(true);
                if (!audioSourceLeftPressed)
                {
                    audioSourceLeft.Play();
                    audioSourceLeftPressed = true;
                }

            }
            else
            {
                fireParticleLeft.Stop();
                lightParticleLeft.SetActive(false);
                audioSourceLeft.Stop();
                audioSourceLeftPressed = false;
            }

            if (Input.GetKey(KeyCode.D))
            {
                rocketRb.AddTorque(directionalForce * Time.fixedDeltaTime);
                fireParticleRight.Play();
                lightParticleRight.SetActive(true);
                if (!audioSourceRightPressed)
                {
                    audioSourceRight.Play();
                    audioSourceRightPressed = true;
                }
            }
            else
            {
                fireParticleRight.Stop();
                lightParticleRight.SetActive(false);
                audioSourceRight.Stop();
                audioSourceRightPressed = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                coheteCuanticoAnimator.SetTrigger("isTeleporting");
                Vector2 newPosition = new Vector2(whereAmI.x, whereAmI.y + teleport);
                rocketRb.MovePosition(newPosition);
                transform.eulerAngles = Vector3.zero;
                audioSourceTeleport.Play();
                teleportParticle.Play();               
            }

            Vector2 currentDirection = transform.up; // Obtenemos la direcci�n actual del cohete hacia arriba
            float dot = Vector2.Dot(currentDirection, Vector2.up); // Calculamos el producto escalar entre la direcci�n actual y el vector arriba
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "triggerMurcielagosStart")
        {
            if (!isMurcielagosOn)
            {
                audioSourceClipMurcielagos.PlayOneShot(birdsClip);
                StoryManager storyManagerScript = FindObjectOfType<StoryManager>();
                storyManagerScript.murcielagosController = true;
                StartCoroutine(AudioSourceClipMurcielagosRoutine());
                isMurcielagosOn = true;
            }
            
        }                  
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("AntiRocket"))
        {
            //TROZO DE C�DIGO INDESTRUCTIBLE para cualquier clase
            GameManager gameManagerScript = FindObjectOfType<GameManager>();
            gameManagerScript.PermitControlPolimorfico();

            ZoomEffect zoomEffectScript = FindObjectOfType<ZoomEffect>();
            zoomEffectScript.ZoomIn();
            isLaunched = false;
            MoreRockets();
            AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);
            playerControllerScript.bringBackTheControlDudeCuantico = true;
            playerControllerScript.CuanticoBack();//Inicia Coroutina de 4segundos y resta -1CC lo que inicia proceso de LOOT
            Instantiate(explosionPrefab, whereAmI, explosionPrefab.transform.rotation);
            Destroy(gameObject);
            //TROZO DE C�DIGO INDESTRUCTIBLE para cualquier clase           
        }

        if (collision.collider.CompareTag("Player"))
        {
            ZoomEffect zoomEffectScript = FindObjectOfType<ZoomEffect>();
            zoomEffectScript.ZoomIn();
            isLaunched = false;
            AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);
            MoreRockets();
            Instantiate(explosionPrefab, whereAmI, explosionPrefab.transform.rotation);
            playerControllerScript.bringBackTheControlDudeCuantico = true;
            playerControllerScript.CuanticoBack();//Inicia Coroutina de 4segundos y resta -1CC lo que inicia proceso de LOOT
            Destroy(gameObject);
        }

        if (collision.gameObject.name == "PulsadorFinal")
        {
            ZoomEffect zoomEffectScript = FindObjectOfType<ZoomEffect>();
            zoomEffectScript.ZoomIn();
            AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);
            audioSourceClip.PlayOneShot(errorButtonClip);
            Instantiate(explosionPrefab, whereAmI, explosionPrefab.transform.rotation);
            StoryManager storyManagerScript = FindObjectOfType<StoryManager>();
            storyManagerScript.pulsadorPulsed = true;
            Destroy(gameObject);
        }
    }

    public void AsignarRigidBody2D()
    {
        rocketRb = GetComponent<Rigidbody2D>();
        rbAssigned = true;
    }

    public void AudioSourceAsign()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSourceBase = audioSources[0];
        audioSourceBase.clip = turboClip;
        audioSourceLeft = audioSources[1];
        audioSourceLeft.clip = turboClip;
        audioSourceLeft.panStereo = -1;
        audioSourceRight = audioSources[2];
        audioSourceRight.clip = turboClip;
        audioSourceRight.panStereo = 1;
        audioSourceDirectionalAssigned = true;
        audioSourceStart = audioSources[3];
        audioSourceStart.clip = turboClip;
        audioSourceTeleport = audioSources[4];
        audioSourceTeleport.clip = teleportClip;
        audioSourceClip = audioSources[5];
        audioSourceClipMurcielagos = audioSources[6];
    }

    public IEnumerator SmokingTime()
    {
        yield return new WaitForSeconds(1);
        smokeParticle.Stop();
        audioSourceStart.Stop();
    }

    public void AlreadyPhoned()
    {
        playerControllerScript.coheteCuanticoPhoned = true;
        alreadyPhoned = true;
    }

    public void NoMoreRockets()
    {
        //Sustituye Lanzaderas por AntiRocket tras el lanzamiento
        lanzadera1 = GameObject.Find("Lanzadera1-Collider 22222").GetComponent<Collider2D>();
        antiRocket1 = GameObject.Find("Lanzadera1-AntiRocket").GetComponent<Collider2D>();
        lanzadera2 = GameObject.Find("Lanzadera2-SueloAzotea").GetComponent<Collider2D>();
        antiRocket2 = GameObject.Find("Lanzadera2-AntiRocket").GetComponent<Collider2D>();
        lanzadera1.enabled = false;
        antiRocket1.enabled = true;
        lanzadera2.enabled = false;
        antiRocket2.enabled = true;
        collidersChanged = true;
    }

    public void MoreRockets()
    {
        //Devuelve los colliders a su estado natural para el pr�ximo lanzamiento tras su colisi�n
        lanzadera1.enabled = true;
        antiRocket1.enabled = false;
        lanzadera2.enabled = true;
        antiRocket2.enabled = false;
    }

    public void LightStartOff()
    {
        lightParticleBase1.SetActive(false);
        lightParticleBase2.SetActive(false);
        lightParticleLeft.SetActive(false);
        lightParticleRight.SetActive(false);
        lightParticlesOff = true;
    }

    public IEnumerator AudioSourceClipMurcielagosRoutine()
    {
        yield return new WaitForSeconds(1);
        audioSourceClipMurcielagos.volume = 0.8f;
        yield return new WaitForSeconds(1);
        audioSourceClipMurcielagos.volume = 0.6f;
        yield return new WaitForSeconds(1);
        audioSourceClipMurcielagos.volume = 0.4f;
        yield return new WaitForSeconds(1);
        audioSourceClipMurcielagos.volume = 0f;
    }
}
