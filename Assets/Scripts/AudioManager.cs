using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	public AudioSFX AudioSFX;

	public AudioPlayer AudioPlayer;

	void Awake(){
		if (instance == null) {
			instance = this;
		}
	}

	public void PlayCoinPickupSound(GameObject obj){
		AudioSource.PlayClipAtPoint(AudioSFX.coinp, obj.transform.position);
	}

	public void PlayJumpSound(GameObject obj){
		AudioSource.PlayClipAtPoint(AudioPlayer.jump, obj.transform.position);
	}
	
	
	

	
}