using UnityEngine;
using UnityEngine.UI;

public class OpcionesMenu : MonoBehaviour
{
    public Slider volumenSlider;

    private void Start()
    {
        // Establece el valor inicial del slider al volumen actual del AudioListener
        volumenSlider.value = AudioListener.volume;
    }

    public void OnVolumenChanged(float volumen)
    {
        // Actualiza el volumen del AudioListener con el valor del slider
        AudioListener.volume = volumen;
    }
}