using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DirectionalRocket : CoheteSimple
{
    public float upForce = 600.0f;
    public float directionalForce = 50.0f;
    private bool rbAssigned = false;
    private Rigidbody2D rocketRb;
    public ParticleSystem fireParticleBase1;
    public ParticleSystem fireParticleBase2;
    public ParticleSystem fireParticleLeft;
    public ParticleSystem fireParticleRight;
    public ParticleSystem smokeParticle;
    public GameObject lightParticleBase1;
    public GameObject lightParticleBase2;
    public GameObject lightParticleLeft;
    public GameObject lightParticleRight;
    public GameObject smokePlatform;
    public GameObject explosionPrefab;
    public GameObject chipPrefab;
    private SpriteRenderer directionalSprite;
    public AudioClip turboClip;
    public AudioClip explosionClip;
    private AudioSource audioSourceBase;
    private bool audioSourceBasePressed = false;
    private AudioSource audioSourceLeft;
    private bool audioSourceLeftPressed = false;
    private AudioSource audioSourceRight;
    private bool audioSourceRightPressed = false;
    private AudioSource audioSourceStart;
    private bool audioSourceStartOn;
    private bool audioSourceDirectionalAssigned = false;
    private Vector2 whereAmI;
    private bool alreadyPhoned = false;
    //Sustituye las lanzaderas por antiRockets en la función NoMoreRockets
    private bool collidersChanged = false;
    private Collider2D lanzadera1;
    private Collider2D antiRocket1;
    private Collider2D lanzadera2;
    private Collider2D antiRocket2;

    // NO PUEDE HABER START


    // Update is called once per frame
    public void FixedUpdate()
    {
        Lanzamiento();
        whereAmI = transform.position;
    }


    public override void Lanzamiento()
    {
        //POLYMORPHISM
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

            Vector2 currentDirection = transform.up; // Obtenemos la dirección actual del cohete hacia arriba
            float dot = Vector2.Dot(currentDirection, Vector2.up); // Calculamos el producto escalar entre la dirección actual y el vector arriba
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("AntiRocket"))
        {
            //TROZO DE CÓDIGO INDESTRUCTIBLE para cualquier clase
            GameManager gameManagerScript = FindObjectOfType<GameManager>();
            gameManagerScript.PermitControlPolimorfico();
            ZoomEffect zoomEffectScript = FindObjectOfType<ZoomEffect>();
            zoomEffectScript.ZoomIn();
            isLaunched = false;
            MoreRockets();
            AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);
            playerControllerScript.bringBackTheControlDude = true;
            Instantiate(explosionPrefab, whereAmI, explosionPrefab.transform.rotation);
            Destroy(gameObject);
            //TROZO DE CÓDIGO INDESTRUCTIBLE para cualquier clase           
        }

        if (collision.collider.CompareTag("Radar"))
        {
            isLaunched = false;
            AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);
            MoreRockets();
            zoomEffectScript.ZoomIn();
            GenerateChips(); //Instancia 3 chips
            Instantiate(explosionPrefab, whereAmI, explosionPrefab.transform.rotation);
            playerControllerScript.bringBackTheControlDude = true;
            Destroy(collision.gameObject);
            fireParticleBase1.Stop();
            fireParticleBase2.Stop();
            fireParticleLeft.Stop();
            fireParticleRight.Stop();
            GetComponent<SpriteRenderer>().enabled = false;                               
        }

        if (collision.collider.CompareTag("Player"))
        {
            isLaunched = false;
            AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);
            MoreRockets();
            Instantiate(explosionPrefab, whereAmI, explosionPrefab.transform.rotation);
            playerControllerScript.bringBackTheControlDude = true;
            Destroy(gameObject);
            zoomEffectScript.ZoomIn();
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
    }

    public IEnumerator SmokingTime()
    {
        yield return new WaitForSeconds(1);
        smokeParticle.Stop();
        audioSourceStart.Stop();
    }

    public void AlreadyPhoned()
    {
        playerControllerScript.directionalRocketPhoned = true;
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
        //Devuelve los colliders a su estado natural para el próximo lanzamiento tras su colisión
        lanzadera1.enabled = true;
        antiRocket1.enabled = false;
        lanzadera2.enabled = true;
        antiRocket2.enabled = false;
    }

    public void GenerateChips()
    {
        StartCoroutine(GenerateChipsRoutine());       
    }

    public IEnumerator GenerateChipsRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        Instantiate(chipPrefab, whereAmI, chipPrefab.transform.rotation);
        yield return new WaitForSeconds(0.3f);
        Instantiate(chipPrefab, whereAmI, chipPrefab.transform.rotation);
        yield return new WaitForSeconds(0.3f);
        Instantiate(chipPrefab, whereAmI, chipPrefab.transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
