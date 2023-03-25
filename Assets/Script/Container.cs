using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private Rigidbody2D containerRb;
    private StoryManager storyManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        containerRb = GetComponent<Rigidbody2D>();
        storyManagerScript = GameObject.Find("StoryManager").GetComponent<StoryManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("AntiRocket"))
        {
            // Aquí puedes agregar el código que deseas ejecutar cuando hay una colisión con el objeto AntiRocket.
            storyManagerScript.smokeBool = true;
            Destroy(gameObject);
        }
    }
}
