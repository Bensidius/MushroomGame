using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource musicPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        // If there is only 1 music player in the game, play the music
        if (FindObjectsOfType<MusicPlayer>().Length == 1)
        {
            musicPlayer = GetComponent<AudioSource>();
            musicPlayer.Play();
            DontDestroyOnLoad(gameObject); // Make sure the gameobject is not destroyed when changing scenes
        }

        else
        {
            Destroy(gameObject);
        }
    }

    
}
