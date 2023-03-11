using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public abstract class CoheteSimple : MonoBehaviour
{
    protected GameObject playerObject;
    protected PlayerController playerControllerScript;
    protected ZoomEffect zoomEffectScript;
    protected Collider2D rocketCollider2D;
    protected CinemachineVirtualCamera myCinemachine;
    protected CinemachineCameraOffset cameraOffset;
    protected float speed = 200.0f;
    protected float countDown = 2.0f;
    protected float countDownMax = 2.0f;
    protected bool isLaunched = false;
    protected bool inContact = false;
    protected bool isSafe = false;
    Vector2 newLookDirection;   

    public void Start()
    {
        playerObject = GameObject.Find("Player");
        playerControllerScript = playerObject.GetComponent<PlayerController>();
        zoomEffectScript = GameObject.Find("CameraManager").GetComponent<ZoomEffect>();
        rocketCollider2D = GetComponent<Collider2D>();
        myCinemachine = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        cameraOffset = myCinemachine.GetComponent<CinemachineCameraOffset>();
        countDownMax = 2.0f;
        countDown = 2.0f;      
    }
 
    public interface IRigidbodyProvider
    {
        Rigidbody2D GetRigidbody();
    }
    public virtual void Lanzamiento()
    {
        //aquí va la funcionalidad
        Vector2 playerPosition = playerControllerScript.GetPlayerPosition();
        Vector2 lookDirection = playerControllerScript.GetLookDirection();       
        RaycastHit2D hit = Physics2D.Raycast(playerPosition + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("RealRocket"));
        RaycastHit2D hit2 = Physics2D.Raycast(playerPosition + Vector2.up * 2.0f, lookDirection, 1.5f, LayerMask.GetMask("RealRocket"));


        if (hit.collider != null || hit2.collider != null)
        {           
            inContact = true;          
        }
        else
        {
            inContact = false;
        }
        if (isLaunched == false && inContact == true && Input.GetKeyDown(KeyCode.Space) && playerControllerScript.safeDelay == true)
        {
            isSafe = true;              
        }
        if (isSafe == true)
        {
            zoomEffectScript.ZoomOut();//El ZoomIn de vuelta tienes que incorporarlo en el collision del rocket.
            playerControllerScript.StaySafe(); //Esta función arrebata el control al jugador y no lo devuelve. Cada cohete debe llevar ya reactivación en su explosión
            countDown -= Time.deltaTime;
            if (countDown <= 0)
            {
                Rigidbody2D rocketRb = GetComponent<Rigidbody2D>();
                rocketRb.constraints = RigidbodyConstraints2D.None;
                rocketCollider2D.isTrigger = false;
                rocketRb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
                isSafe = false;
                isLaunched = true;
                playerControllerScript.safeDelay = false;
                countDown = countDownMax;
            }
        }    
    }
}
