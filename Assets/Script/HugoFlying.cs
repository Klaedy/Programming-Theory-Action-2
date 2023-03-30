using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugoFlying : MonoBehaviour
{
    public float speed = 2.0f;
    public float floatingSpeed = 0.01f;
    public float amplitude = 0.05f;
    public bool midLane = false;
    private Vector2 startingPosition;
    public ParticleSystem legLeft;
    public ParticleSystem legRight;
    private float upBoundry = 19.0f;
    private float downBoundry = 15.0f;
    private float leftBoundry = 1002.0f;
    private float rightBoundry = 1006.0f;
    private bool getStartingPosition = false;

    void Start()
    {
        legLeft.Play();
        legRight.Play();
    }

    void Update()
    {
        if (transform.position.y <= 17 && midLane == false)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else midLane = true;

        if (midLane == true)
        {
            if (!getStartingPosition)
            {
                startingPosition = transform.position;
                getStartingPosition = true;
            }

            float xOffset = amplitude * Mathf.Sin(Time.time * floatingSpeed * Mathf.PI * 1f);
            float yOffset = amplitude * Mathf.Cos(Time.time * floatingSpeed * Mathf.PI * 1f);
            Vector2 newPosition = startingPosition + new Vector2(xOffset, yOffset);

            if (newPosition.x < leftBoundry || newPosition.x > rightBoundry)
            {
                // Invert the x direction
                speed = -speed;
            }

            if (newPosition.y < downBoundry || newPosition.y > upBoundry)
            {
                amplitude = -amplitude;
            }

            // Smoothly move to the new position
            transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * 5f);
        }
    }
}
