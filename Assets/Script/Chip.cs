using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    private float forceUp = 10.0f;
    private float horizontalRange = 5.0f;
    private Rigidbody2D chipRb;
    public GameObject platform;
    private bool isGenerated = false;
    public float rotationSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        chipRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGenerated)
            ICanFly();
        
        Platform();
       
    }

    public void ICanFly()
    {
        float horizontalForce = Random.Range(horizontalRange, -horizontalRange);
        Vector2 chipDirection = new Vector2(horizontalForce, forceUp);
        chipRb.AddForce(chipDirection, ForceMode2D.Impulse);
        isGenerated = true;
    }

    public void Platform()
    {
        transform.Rotate(0f, rotationSpeed, 0f);
        platform.transform.localPosition = new Vector2(0f, -1f);
        platform.transform.Rotate(0f, 0f, 0f);
    }  
}
