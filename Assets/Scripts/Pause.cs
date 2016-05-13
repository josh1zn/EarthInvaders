using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{
    public GUISkin GuiSkin;

    private bool enablePause;
    private bool paused;
    private float buttonWidth;
    private float buttonHeight;
    private int control;
    private string[] controls;
    private Rect rect;

	//Initialization
	public void Start()
	{
        enablePause = true;
        ResizeControls();       
        controls = new string[] { "Touch", "Accelerometer", "Both" };
        control = PlayerPrefs.GetInt("MoveControl", 0);
	}

	//Called once per frame
	public void Update()
	{
        if (enablePause)
        {
            foreach (Touch t in Input.touches)
            {
                Vector3 touchWorldPos = Camera.main.ScreenToWorldPoint(t.position);
                if (Physics2D.OverlapCircle(touchWorldPos, 0.7f, 1 << 8) == collider2D && t.phase == TouchPhase.Ended)
                {
                    PauseGame();
                }
            }
        }
	}

    public void OnGUI()
    {
        if (paused)
        {
            ResizeControls();
            GUI.skin = GuiSkin;
            GUILayout.BeginArea(rect);
            if (GUILayout.Button("Resume", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                ChangeControl();
                ResumeGame();
            }
            GUILayout.Label("", "Divider");
            GUILayout.Space(buttonHeight);
            control = GUILayout.SelectionGrid(control, controls, 3, "Toggle");
            GUILayout.Space(buttonHeight);
            GUILayout.Label("", "Divider");
            if (GUILayout.Button("Main Menu", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                Application.LoadLevel("MainMenu");
            }
            GUILayout.EndArea();
        }
    }

    #region Helpers

    public void EnablePause()
    {
        enablePause = true;
    }

    public void DisablePause()
    {
        enablePause = false;
    }

    public void PauseGame()
    {
        paused = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public bool GameIsPaused()
    {
        return paused;
    }

    private void ResizeControls()
    {
        buttonWidth = Screen.width * 0.8f;
        buttonHeight = Screen.height * 0.1f;
        rect.width = Screen.width * 0.8f;
        rect.height = (Screen.height * 0.8f);
        rect.x = Screen.width * 0.1f; //(1 - 0.8) / 2 for left space
        rect.y = Screen.height * 0.2f; //(1 - 0.8) / 2 for Top space but add double padding top
    }

    private void ChangeControl()
    {
        if (control == 0)
        {
            Settings.ControlTouch();
        }
        else if (control == 1)
        {
            Settings.ControlAccelerometer();
        }
        else
        {
            Settings.ControlAccelerometerAndTouch();
        }
        Settings.SaveSettings();
        GameObject.FindObjectOfType<Player>().MoveControl = control;
    }
    #endregion
}