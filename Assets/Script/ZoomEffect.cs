using UnityEngine;
using Cinemachine;
using System.Collections;

public class ZoomEffect : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomAmount = 2f;
    public float zoomDuration = 1f;
  

    private float originalSize;

    private void Start()
    {
        originalSize = virtualCamera.m_Lens.OrthographicSize;
    }

    public void ZoomOut()
    {
        StartCoroutine(ZoomCoroutine(originalSize + zoomAmount));
    }

    public void ZoomIn()
    {
        StartCoroutine(ZoomCoroutine(originalSize));
    }

    private IEnumerator ZoomCoroutine(float targetSize)
    {
        float timer = 0f;
        float startSize = virtualCamera.m_Lens.OrthographicSize;

        while (timer < zoomDuration)
        {
            float t = timer / zoomDuration;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);
            timer += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetSize;
    }
}