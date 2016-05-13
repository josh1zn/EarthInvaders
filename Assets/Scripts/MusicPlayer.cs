using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] Tracks;

    private int trackNumber;

	//Initialization
	public void Start()
	{

	}

	//Called once per frame
	public void Update()
	{
        if (!audio.isPlaying && !AudioListener.pause)
        {
            trackNumber = Random.Range(0, Tracks.Length);
            audio.clip = Tracks[trackNumber];
            audio.Play();
        }
	}

    public void Pause()
    {
        audio.Pause();
    }

    public void Stop()
    {
        audio.Stop();
    }
}