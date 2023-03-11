using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DirectionalRocket : CoheteSimple
{
    private float upForce = 700.0f;
    private float directionalForce = 50.0f;
    private Rigidbody2D rocketRb;
    // NO PUEDE HABER START


    // Update is called once per frame
    public void FixedUpdate()
    {
        Lanzamiento();      
    }



    public override void Lanzamiento()
    {
        base.Lanzamiento();
        speed = 500.0f;
        
        if (isLaunched == true)
        {
            AsignarRigidBody2D();
            myCinemachine.Follow = transform;          
            if (Input.GetKey(KeyCode.W))
            {
                rocketRb.AddForce(transform.up * upForce, ForceMode2D.Force);
            }

            if (Input.GetKey(KeyCode.A))
            {
                rocketRb.AddTorque(directionalForce * Time.fixedDeltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rocketRb.AddTorque(-directionalForce * Time.fixedDeltaTime);
            }

            Vector2 currentDirection = transform.up; // Obtenemos la dirección actual del cohete hacia arriba
            float dot = Vector2.Dot(currentDirection, Vector2.up); // Calculamos el producto escalar entre la dirección actual y el vector arriba
        }      
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("AntiRocket"))
        {
            //TROZO DE CÓDIGO INDESTRUCTIBLE para cualquier clase
            GameManager gameManagerScript = FindObjectOfType<GameManager>();
            gameManagerScript.PermitControlPolimorfico();          
            ZoomEffect zoomEffectScript = FindObjectOfType<ZoomEffect>();
            zoomEffectScript.ZoomIn();
            Destroy(gameObject);
            //TROZO DE CÓDIGO INDESTRUCTIBLE para cualquier clase           
        }
    }

    public void AsignarRigidBody2D()
    {
        rocketRb = GetComponent<Rigidbody2D>();
    }
}
