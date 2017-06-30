using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	public AudioClip[] menuTracks;
	public AudioClip[] dungeonTracks;
	public AudioSource source;

	private bool inDungeon = false;// Not in dungeon on start

	void Start () {
		// If our source is designated playOnAwake, play music as soon as we can
		if (!source.playOnAwake) {
			if (inDungeon) {
				source.clip = dungeonTracks [Random.Range (0, dungeonTracks.Length)];
				source.Play ();
			} else {
				source.clip = menuTracks [Random.Range (0, menuTracks.Length)];
				source.Play ();
			}
		}
	}
		
	void Update () {
		// If the song is over or we need to switch from menu to dungeon or vice versa
		if (!source.isPlaying) {
			// Set our current mode locally
			inDungeon = GameControl.control.inDungeon;

			// Play appropriate music
			if (inDungeon) {
				source.clip = dungeonTracks [Random.Range (0, dungeonTracks.Length)];
				source.Play ();
			} else {
				source.clip = menuTracks [Random.Range (0, menuTracks.Length)];
				source.Play ();
			}
		}
		if (GameControl.control != null && inDungeon != GameControl.control.inDungeon) {
			// Set our current mode locally
			inDungeon = GameControl.control.inDungeon;

			// Play appropriate music
			if (inDungeon) {
				source.clip = dungeonTracks [Random.Range (0, dungeonTracks.Length)];
				source.Play ();
			} else {
				source.clip = menuTracks [Random.Range (0, menuTracks.Length)];
				source.Play ();
			}
		}
}
}
