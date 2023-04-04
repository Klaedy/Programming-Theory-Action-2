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
    private bool isHugoAlive = true;
    private bool rocketsFailing = false;
    public GameObject creditText;
    private Animator playerAnimator;
    private SpriteRenderer starBackgroundSprite;
    public float alpha = 0f;
    private float transitionTime = 8f;
    public bool isSpaceVisible = false;

    void Start()
    {
        starBackgroundSprite = GameObject.Find("StarBackground").GetComponent<SpriteRenderer>();
        starBackgroundSprite.color = new Color(0, 0, 0, alpha);
        playerAnimator = GetComponent<Animator>();      
        legLeft.Play();
        legRight.Play();
    }

    void Update()
    {
        if (isHugoAlive == true)
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


            //Rutina de eventos para finalizar

            if (creditText.gameObject.GetComponent<RectTransform>().anchoredPosition.y >= -270 && rocketsFailing == false)
            {
                StartCoroutine(PropulsoresOff());
            }
        }   
        
        if (isSpaceVisible == true)
        {
            if (alpha <= 2)
            {
                alpha += Time.deltaTime / transitionTime;
                starBackgroundSprite.color = new Color(255, 255, 255, alpha);
            }
        }
    }

    public IEnumerator PropulsoresOff()
    {
        rocketsFailing = true;
        isSpaceVisible = true;
        legLeft.Stop();
        yield return new WaitForSeconds(1);
        legLeft.Play();
        yield return new WaitForSeconds(0.5f);
        legRight.Stop();
        yield return new WaitForSeconds(1);
        legLeft.Stop();
        yield return new WaitForSeconds(1);
        legRight.Play();
        yield return new WaitForSeconds(1);
        legRight.Stop();
        yield return new WaitForSeconds(0.5f);
        legRight.Play();
        yield return new WaitForSeconds(0.5f);
        legLeft.Play();
        yield return new WaitForSeconds(1);
        legLeft.Stop();
        legRight.Stop();
        yield return new WaitForSeconds(0.5f);       
        legLeft.Play();
        legRight.Play();
        yield return new WaitForSeconds(0.5f);
        legLeft.Stop();
        legRight.Stop();
        yield return new WaitForSeconds(3);
        playerAnimator.SetBool("isHugoDeath", true);
        isHugoAlive = false;
    }
}
