using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private OpcionesMenu opcionesMenuScript;
    public static AudioManager Instance;
    private bool audioSourcesAsigned = false;
    private bool firstSongPlayed = false;
    private bool secondSongPlayed = false;
    private AudioSource[] audioSources;
    private AudioSource beachBumMusic;
    private AudioSource stargazingMusic;
    private AudioSource dataMusic;
    public AudioClip beachBumMusicClip;
    public AudioClip stargazingMusicClip;
    public AudioClip dataMusicClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        opcionesMenuScript = GetComponent<OpcionesMenu>();
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
        if (!audioSourcesAsigned)
        {
            beachBumMusic = audioSources[0];
            beachBumMusic.clip = beachBumMusicClip;
            stargazingMusic = audioSources[1];
            stargazingMusic.clip = stargazingMusicClip;
            dataMusic = audioSources[2];
            dataMusic.clip = dataMusicClip;
            audioSourcesAsigned = true;
        }

        if (!firstSongPlayed)
        {
            audioSources[0].Play();
            firstSongPlayed = true;
        }
    }

    public void FadeOutOne()
    {
        StartCoroutine(FadeOutOneRoutine());
    }

    public IEnumerator FadeOutOneRoutine()
    {      
        //beachBumMusic es la variable asignada al AudioSource
        for (float vol = beachBumMusic.volume; vol > 0; vol -= 0.1f)
        {
            beachBumMusic.volume = vol;
            yield return new WaitForSeconds(1);
        }
        //Una vez el for termine su bucle, nos aseguramos de que la canción esté en 0 y detenida
        beachBumMusic.volume = 0f;
        beachBumMusic.Stop();
        yield return new WaitForSeconds(10);
        if (!secondSongPlayed)
        {
            audioSources[1].Play();
            secondSongPlayed = true;
        }       
    }

    public void FadeOutTwo()
    {
        StartCoroutine(FadeOutTwoRoutine());
    }

    public IEnumerator FadeOutTwoRoutine()
    {
        for (float vol2 = stargazingMusic.volume; vol2 > 0; vol2 -= 0.1f)
        {
            stargazingMusic.volume = vol2;
            yield return new WaitForSeconds(1);
        }
        stargazingMusic.volume = 0f;
        stargazingMusic.Stop();
    }

    public void FinalSong()
    {
        dataMusic.Play();
    }
}

