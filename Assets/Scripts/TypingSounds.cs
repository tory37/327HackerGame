using UnityEngine;
using System.Collections;

public class TypingSounds : MonoBehaviour {

	// Use this for initialization
    private AudioSource keyboardSoundPlayer;
    [SerializeField]
    private AudioClip [] keyboardSounds;
	void Start () {
        keyboardSoundPlayer = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Return))
        {
            keyboardSoundPlayer.clip = keyboardSounds[0];
            keyboardSoundPlayer.pitch = Random.Range(0.9f, 1.1f);
            keyboardSoundPlayer.volume = Random.Range(0.75f,1f);
            keyboardSoundPlayer.Play();
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            keyboardSoundPlayer.clip = keyboardSounds[1];
            keyboardSoundPlayer.pitch = 1;
            keyboardSoundPlayer.volume = 1;
            keyboardSoundPlayer.Play();
        }
	}
}
