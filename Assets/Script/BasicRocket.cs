using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRocket : MonoBehaviour
{
    public Rigidbody2D rocketRb;
    public Collider2D rocketCollider2D;
    protected float speed = 500.0f;
    // Start is called before the first frame update
    void Awake()
    {
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
    }

    //RocketPosition() no se está usando. Su función es devolver las coordenadas del rocket
    //public Vector2 RocketPosition()
    //{
        //Vector2 rocketPosition = new Vector2(transform.position.x, transform.position.y);
        //return rocketPosition;
    //}
}
