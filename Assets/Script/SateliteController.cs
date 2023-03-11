using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteController : MonoBehaviour
{
    private float speed;
    private float horizontalBoundry;
    private float rightBoundry;
    private float leftBoundry;
    private float startPosition;
    private float actualPosition;
    private float randomDirection;
    private bool movingRight = false;
    private bool movingLeft = false;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position.x;
        randomDirection = Random.value;
        speed = Random.Range(9, 15);
    }

    // Update is called once per frame
    void Update()
    {
        if (randomDirection < 0.5)
        {
            RightStart();
        }
        if (randomDirection >= 0.5)
        {
            LeftStart();
        }
            
    }

    public void RightStart()
    {
        horizontalBoundry = Random.Range(8, 15);
        rightBoundry = startPosition + horizontalBoundry;
        leftBoundry = startPosition - horizontalBoundry;
        Vector2 actualPosition = transform.position;

        if (movingLeft == false && movingRight == false)
        {
            movingRight = true;
            movingLeft = false;
        }
        
        if (actualPosition.x > rightBoundry)
        {
            movingRight = false;
            movingLeft = true;
        }
        if (actualPosition.x < leftBoundry)
        {
            movingRight = true;
            movingLeft = false;
        }

        if (movingRight == true && movingLeft == false)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        if (movingRight == false && movingLeft == true)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    public void LeftStart()
    {
        horizontalBoundry = Random.Range(8, 15);
        rightBoundry = startPosition + horizontalBoundry;
        leftBoundry = startPosition - horizontalBoundry;
        Vector2 actualPosition = transform.position;

        if (movingLeft == false && movingRight == false)
        {
            movingRight = false;
            movingLeft = true;
        }

        if (actualPosition.x > rightBoundry)
        {
            movingRight = false;
            movingLeft = true;
        }
        if (actualPosition.x < leftBoundry)
        {
            movingRight = true;
            movingLeft = false;
        }

        if (movingRight == true && movingLeft == false)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        if (movingRight == false && movingLeft == true)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }
}
