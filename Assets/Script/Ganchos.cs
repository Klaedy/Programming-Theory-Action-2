using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Ganchos : MonoBehaviour
{
    private StoryManager storyManagerScript;
    private Rigidbody2D containerRb;
    public GameObject[] backgroundLength;
    public GameObject[] realSheetLength;
    public GameObject cameraFocusHere;
    public Animator[] ganchoAnimator;
    public Animator[] gancho2Animator;
    private CinemachineVirtualCamera myCinemachine;
    private AudioSource audioSourceClip;
    private AudioSource audioSourceClip2;
    public AudioClip metalSlash;
    public AudioClip metalSlash2;
    private ZoomEffect zoomEffectScript;
    private Collider2D containerCollider2D;
    public float speed = 2.0f;

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        storyManagerScript = GameObject.Find("StoryManager").GetComponent<StoryManager>();
        containerRb = GameObject.Find("Container").GetComponent<Rigidbody2D>();
        containerCollider2D = GameObject.Find("Container").GetComponent<Collider2D>();
        myCinemachine = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        zoomEffectScript = GameObject.Find("CameraManager").GetComponent<ZoomEffect>();
        audioSourceClip = cameraFocusHere.GetComponent<AudioSource>();
        audioSourceClip2 = cameraFocusHere.GetComponent<AudioSource>();
        audioSourceClip.clip = metalSlash;
        audioSourceClip2.clip = metalSlash2;
    }

    // Update is called once per frame
    void Update()
    {
        //PlayRotation();
        if (storyManagerScript.ganchosMoving == true)
        {
            myCinemachine.Follow = cameraFocusHere.transform;
            StartCoroutine(GanchosScene());
            storyManagerScript.ganchosMoving = false;
        }
    }
 
    IEnumerator GanchosScene()
    {
        storyManagerScript.ganchosMoving = false;
        zoomEffectScript.ZoomOut();
        yield return new WaitForSeconds(3);
        ganchoAnimator[3].SetTrigger("ganchoActivated");
        audioSourceClip.Play();       
        yield return new WaitForSeconds(2);
        ganchoAnimator[1].SetTrigger("ganchoActivated");        
        audioSourceClip2.Play();
        yield return new WaitForSeconds(1);
        ganchoAnimator[0].SetTrigger("ganchoActivated");        
        audioSourceClip.Play();
        yield return new WaitForSeconds(3);
        ganchoAnimator[2].SetTrigger("ganchoActivated");       
        audioSourceClip.Play();
        yield return new WaitForSeconds(1);
        gancho2Animator[0].SetTrigger("gancho2Activated");
        audioSourceClip.Play();
        yield return new WaitForSeconds(1);
        gancho2Animator[1].SetTrigger("gancho2Activated");
        audioSourceClip.Play();
        yield return new WaitForSeconds(1);
        gancho2Animator[2].SetTrigger("gancho2Activated");
        audioSourceClip.Play();
        yield return new WaitForSeconds(1);
        gancho2Animator[3].SetTrigger("gancho2Activated");
        audioSourceClip.Play();
        yield return new WaitForSeconds(0.5f);
        containerRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        containerCollider2D.isTrigger = false;
        storyManagerScript.traderNPC.SetActive(false);      
    }
   
}
