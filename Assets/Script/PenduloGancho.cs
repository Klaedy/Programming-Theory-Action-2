using UnityEngine;

public class PenduloGancho : MonoBehaviour
{
    public float speed = 0.1f; // Velocidad de rotación del péndulo
    private float range = 10f; // Rango de rotación del péndulo, desde -10 hasta 10
    private float direction = 1f; // Dirección de rotación actual del péndulo, por defecto hacia la derecha
    public float currentAngle = 0f; // Ángulo actual de rotación del péndulo

    // Update is called once per frame
    void Update()
    {
        // Calcula el ángulo al que se moverá el péndulo en la siguiente iteración
        // usando la velocidad, el tiempo desde la última iteración y la dirección actual.
        // Luego, convierte el resultado de radianes a grados.
        currentAngle += direction * speed * Time.deltaTime * Mathf.Rad2Deg;

        // Si el ángulo actual de rotación supera el rango de rotación, ajusta el ángulo
        // y cambia la dirección de rotación para completar el ciclo completo de rotación.
        if (currentAngle > range || currentAngle < -range)
        {
            direction *= -1f; // Cambia la dirección multiplicando por -1
            currentAngle = Mathf.Clamp(currentAngle, -range, range); // Ajusta el valor de ángulo para que esté dentro del rango
        }

        // Establece la nueva rotación del péndulo en el eje Z usando el ángulo actual de rotación
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
