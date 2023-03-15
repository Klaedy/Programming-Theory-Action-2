using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoheteDeJuguete : CoheteSimple
{
    public bool alreadyExplotedCoheteJuguete = false;
    public ParticleSystem fireParticle;
    public ParticleSystem smokeParticle;
    public bool alreadySmoking = false;
    public bool alreadyFiring = false;
    public AudioClip turboClip;
    public AudioClip explosionClip;
    public GameObject explosionPrefab;
    private AudioSource audioSource;
    private bool isTurboCoheteJugueteOn = false;
    private bool audioSourceCoheteJugueteAsigned = false;
    private Vector2 whereAmI;

    // NO PUEDE HABER START


    // Update is called once per frame
    public void Update()
    {
        whereAmI = transform.position;
        Lanzamiento();
    }



    public override void Lanzamiento()
    {
        base.Lanzamiento();
        if (!audioSourceCoheteJugueteAsigned)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = turboClip;
            audioSourceCoheteJugueteAsigned = true;
        }
        
        speed = 950.0f;
        if (alreadySmoking == false)
        {
            smokeParticle.Play();
            alreadySmoking = true;
        }

        if (isLaunched == true)
        {           
            if (alreadyFiring == false)
            {
                fireParticle.Play();
                smokeParticle.Stop();
                alreadyFiring = true;
                isLaunched = false;
                isTurboCoheteJugueteOn = true;
            }
            
            if (isTurboCoheteJugueteOn == true)
            {
                audioSource.Play();
                isTurboCoheteJugueteOn = false;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("AntiRocket"))
        {
            GameManager gameManagerScript = FindObjectOfType<GameManager>();
            gameManagerScript.PermitControlPolimorfico();
            StoryManager storyManagerScript = FindObjectOfType<StoryManager>();
            storyManagerScript.alreadyExplotedCoheteJuguete = true;
            ZoomEffect zoomEffectScript = FindObjectOfType<ZoomEffect>();
            AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);
            zoomEffectScript.ZoomIn();
            Instantiate(explosionPrefab, whereAmI, explosionPrefab.transform.rotation);
            audioSource.Stop();
            Destroy(gameObject);
        }
    }
}
