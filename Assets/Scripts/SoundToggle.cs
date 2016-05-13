using UnityEngine;
using System.Collections;

public class SoundToggle : MonoBehaviour
{
    public Sprite MuteButton;
    public Sprite UnMuteButton;

    private bool SoundOn;

	//Initialization
	public void Start()
	{
        if (PlayerPrefs.GetInt("SoundOn", 1) == 1)
        {
            UnMuteSound();
        }
        else if (PlayerPrefs.GetInt("SoundOn", 1) == 0)
        {
            MuteSound();
        }
	}

	//Called once per frame
	public void Update()
	{
        foreach (Touch t in Input.touches)
        {
            Vector3 touchWorldPos = Camera.main.ScreenToWorldPoint(t.position);
            if (Physics2D.OverlapCircle(touchWorldPos, 0.7f, 1 << 8) == collider2D && t.phase == TouchPhase.Ended)
            {
                if(SoundOn)
                {
                    MuteSound();
                }
                else
                {
                    UnMuteSound();
                }
            }
        }
	}

    private void UnMuteSound()
    {
        AudioListener.volume = 1;
        Settings.SoundOn();
        Settings.SaveSettings();
        GetComponent<SpriteRenderer>().sprite = MuteButton;
        SoundOn = true;
    }

    private void MuteSound()
    {
        AudioListener.volume = 0;
        Settings.SoundOff();
        Settings.SaveSettings();
        GetComponent<SpriteRenderer>().sprite = UnMuteButton;
        SoundOn = false;
    }
}