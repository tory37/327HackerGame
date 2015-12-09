using UnityEngine;
using System.Collections;

/// <summary>
/// This script should be placed on the main camera (with an audio listener)
/// Will select a random song, then play it. When that song ends, a new one 
/// will begin to play.
/// </summary>
public class GameMusicManager : MonoBehaviour {

    [SerializeField]
    private AudioClip[] songs;

    private AudioSource songSource;

    int numberOfSongs, previousSong;

	// Use this for initialization
	void Start () {
        numberOfSongs = songs.Length;
        songSource = gameObject.GetComponent<AudioSource>();
        previousSong = getRandomSong();
        songSource.clip = songs[previousSong];
        songSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
	    if(!songSource.isPlaying)
        {
            startRandomSong();
        }
	}

    private int getRandomSong()
    {
        return Random.Range(0, numberOfSongs);
    }

    private void startRandomSong()
    {
        int nextSong;

        //We do not want to repeat songs, because that'd be boring.
        if (numberOfSongs != 1)
        {
            do
            {
                nextSong = getRandomSong();
            } while (nextSong == previousSong);
        }
        else
        {
            //so it doesn't break if there is only one song
            nextSong = getRandomSong();
        }

        //play the song
        songSource.clip = songs[nextSong];
        songSource.Play();
        previousSong = nextSong;        
    }
}
