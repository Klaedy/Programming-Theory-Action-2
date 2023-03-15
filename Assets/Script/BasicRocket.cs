using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRocket : MonoBehaviour
{
    private PlayerController playerScript;
    private StoryManager storyManagerScript;
    public Rigidbody2D rocketRb;
    public Collider2D rocketCollider2D;
    private ParticleSystem smokeParticle;
    private ParticleSystem fireParticle;
    public GameObject explosionPrefab;
    private bool isFireBurning = false;
    private bool isTurboBasicRocketSoundOn = false;
    protected float speed = 500.0f;
    private bool isFlying = false;
    public AudioClip turboClip;
    public AudioClip explosionClip;
    private AudioSource audioSource;
    private Collider2D triggerShake;
    private Vector2 whereAmI;

    // Start is called before the first frame update
    void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        storyManagerScript = GameObject.Find("StoryManager").GetComponent<StoryManager>();
        smokeParticle = GameObject.Find("SmokeParticle").GetComponent<ParticleSystem>();
        rocketRb = GetComponent<Rigidbody2D>();
        rocketCollider2D = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = turboClip;
        triggerShake = GameObject.Find("Triggers/BasicRocketShake").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        whereAmI = transform.position;
       if (playerScript.startParticle == true)
        {           
            ParticleSystemSmokePlay();
            playerScript.startParticle = false;
        }

       if (isFireBurning == true && isFlying == true)
        {
            fireParticle = GameObject.Find("FireParticle").GetComponent<ParticleSystem>();
            
            StartCoroutine(Temblor());
            ParticleSystemFirePlay();
            isFireBurning = false;
            isTurboBasicRocketSoundOn = true;
        }

       if (isTurboBasicRocketSoundOn == true)
        {
            audioSource.Play();
            isTurboBasicRocketSoundOn = false;
        }
    }

    public void RocketLaunch()
    {   
        rocketRb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        isFireBurning = true;
        isFlying = true;
    }

    public void ParticleSystemSmokePlay()
    {
        smokeParticle.Play();
    }

    public void ParticleSystemFirePlay()
    {
        fireParticle.Play();
    }

    IEnumerator Temblor()
    {
        yield return new WaitForSeconds(0.5f);
        triggerShake.enabled = true;
    }



    //El misil explotará al estrellarse contra el suelo tras no conseguir despegar
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Lanzadera" && isFlying == true)
        {
            AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);
            audioSource.Stop();
            triggerShake.enabled = false;
            Destroy(gameObject);
            storyManagerScript.storyStarted = true;
            storyManagerScript.instalable = false;
            Instantiate(explosionPrefab, whereAmI, explosionPrefab.transform.rotation);
        }
    }

    //RocketPosition() no se está usando. Su función es devolver las coordenadas del rocket
    //public Vector2 RocketPosition()
    //{
    //Vector2 rocketPosition = new Vector2(transform.position.x, transform.position.y);
    //return rocketPosition;
    //}
}
