using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRocket : MonoBehaviour
{
    private PlayerController playerScript;
    private StoryManager storyManagerScript;
    public Rigidbody2D rocketRb;
    public Collider2D rocketCollider2D;
    protected float speed = 500.0f;
    private bool isFlying = false;
    // Start is called before the first frame update
    void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        storyManagerScript = GameObject.Find("StoryManager").GetComponent<StoryManager>();
        rocketRb = GetComponent<Rigidbody2D>();
        rocketCollider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void RocketLaunch()
    {   
        rocketRb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        isFlying = true;
    }

    //El misil explotará al estrellarse contra el suelo tras no conseguir despegar
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Lanzadera" && isFlying == true)
        {
            Debug.Log("booom");
            Destroy(gameObject);           
            storyManagerScript.storyStarted = true;
            storyManagerScript.instalable = false;
        }
    }

    //RocketPosition() no se está usando. Su función es devolver las coordenadas del rocket
    //public Vector2 RocketPosition()
    //{
    //Vector2 rocketPosition = new Vector2(transform.position.x, transform.position.y);
    //return rocketPosition;
    //}
}
