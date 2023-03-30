using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEnd : MonoBehaviour
{
    public float transitionTime = 5f;
    public float alpha = 0f;
    private SpriteRenderer spriteR;
    public bool isFinishing = false;
    private bool goingCredits = false;
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

        if (isFinishing == false && goingCredits == true)
        {
            StartCoroutine(ThisIsTheEnd());
        }
            
    }

    public void Finishing()
    {
        alpha += Time.deltaTime / transitionTime;
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alpha);
        if (alpha >= 2f)
        {
            isFinishing = false;
            goingCredits = true;
        }
    }

    public void TurnFinishOn()
    {
        isFinishing = true;
    }

    public IEnumerator ThisIsTheEnd()
    {
        goingCredits = false;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(2);
    }
}
