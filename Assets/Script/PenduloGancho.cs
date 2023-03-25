using UnityEngine;

public class PenduloGancho : MonoBehaviour
{
    public float speed = 0.1f; // Velocidad de rotaci�n del p�ndulo
    private float range = 10f; // Rango de rotaci�n del p�ndulo, desde -10 hasta 10
    private float direction = 1f; // Direcci�n de rotaci�n actual del p�ndulo, por defecto hacia la derecha
    public float currentAngle = 0f; // �ngulo actual de rotaci�n del p�ndulo

    // Update is called once per frame
    void Update()
    {
        // Calcula el �ngulo al que se mover� el p�ndulo en la siguiente iteraci�n
        // usando la velocidad, el tiempo desde la �ltima iteraci�n y la direcci�n actual.
        // Luego, convierte el resultado de radianes a grados.
        currentAngle += direction * speed * Time.deltaTime * Mathf.Rad2Deg;

        // Si el �ngulo actual de rotaci�n supera el rango de rotaci�n, ajusta el �ngulo
        // y cambia la direcci�n de rotaci�n para completar el ciclo completo de rotaci�n.
        if (currentAngle > range || currentAngle < -range)
        {
            direction *= -1f; // Cambia la direcci�n multiplicando por -1
            currentAngle = Mathf.Clamp(currentAngle, -range, range); // Ajusta el valor de �ngulo para que est� dentro del rango
        }

        // Establece la nueva rotaci�n del p�ndulo en el eje Z usando el �ngulo actual de rotaci�n
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
