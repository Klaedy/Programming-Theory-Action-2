using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float volume = 1f;
    public static AudioManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = volume;
    }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        AudioListener.volume = volume;
    }
}
