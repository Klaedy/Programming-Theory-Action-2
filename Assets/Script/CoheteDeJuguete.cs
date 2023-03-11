using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoheteDeJuguete : CoheteSimple
{
    public bool alreadyExplotedCoheteJuguete = false;   

    // NO PUEDE HABER START


    // Update is called once per frame
    public void Update()
    {
        Lanzamiento();
    }



    public override void Lanzamiento()
    {
        base.Lanzamiento();
        speed = 950.0f;  
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
            zoomEffectScript.ZoomIn();

            Destroy(gameObject);
        }
    }
}
