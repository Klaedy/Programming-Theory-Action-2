using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEnd : MonoBehaviour
{
    public float transitionTime = 5f;
    public float alpha = 0f;
    private SpriteRenderer spriteR;
    public bool isFinishing = false;
    // Start is called before the first frame update
    void Start()
    {

        spriteR = GetComponent<SpriteRenderer>();
        spriteR.color = new Color(0, 0, 0, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinishing == true)
        {
            Finishing();           
        }
            
    }

    public void Finishing()
    {
        alpha += Time.deltaTime / transitionTime;
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alpha);
        if (alpha >= 2f)
        {
            isFinishing = false;
        }
    }

    public void TurnFinishOn()
    {
        isFinishing = true;
    }
}
