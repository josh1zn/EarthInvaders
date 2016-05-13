using UnityEngine;
using System.Collections;
using System.Text;

public class MainMenu : MonoBehaviour
{
    public GUISkin GuiSkin;
    public float WidthP;
    public float HeightP;

    private float windowWidth;
    private float windowHeight;
    private float width;
    private float height;
    private float x;
    private float y;
    private bool showMainMenu;
    private bool showInstructions;
    private bool showSettings;
    private bool showCredits;

    private int control;
    private string[] controls;
    private int level;
    private string[] levels;
    private Vector2 scrollView;

    public void Start()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        control = PlayerPrefs.GetInt("MoveControl", 0);
        level = PlayerPrefs.GetInt("Difficulty", 1) - 1; 
        controls = new string[] { "Touch", "Accelerometer", "Both" };
        levels = new string[] { "Easy", "Medium", "Hard" };
        showMainMenu = true;
        scrollView = new Vector2(0, height);
    }

    //public void Update()
    //{
    //    if (showMainMenu)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Escape))
    //        {
    //            Application.Quit();
    //        }
    //    }
    //}

    public void OnGUI()
    {
        GUI.skin = GuiSkin;
        //Get Dimensions
        width = windowWidth * WidthP;
        height = windowHeight * HeightP; //for 5 buttons 10% of screen height (50% for buttons and 50% vertical spacers)
        windowWidth = Screen.width - (0.14f * Screen.width); //0.1: PadR, 0.04: PadL
        windowHeight = Screen.height - (0.25f * Screen.height); //0.15 PadT, 0.1 PadB
        x = Screen.width * 0.1f;
        y = Screen.height * 0.15f;
        if (showMainMenu)
        {
            ShowMainMenu();
        }
        else if (showInstructions)
        {
            ShowInstructions();
        }
        else if (showCredits)
        {
            ShowCredits();
        }
        else if(showSettings)
        {
            ShowSettings();
        }
    }

    #region Helpers
    private void ShowSettings()
    {
        GUI.BeginGroup(new Rect(x, y, windowWidth, windowHeight));
        GUILayout.BeginVertical();
        GUILayout.Label("Control Settings", GUILayout.Width(width));
        GUILayout.Label("", "Divider", GUILayout.Width(width));
        GUILayout.Space(height * 0.25f);
        control = GUILayout.SelectionGrid(control, controls, 3, "Toggle", GUILayout.Width(windowWidth));
        GUILayout.Space(height * 0.25f);
        GUILayout.Label("", "Divider", GUILayout.Width(width));
        GUILayout.Label("Difficulty Settings", GUILayout.Width(width));
        GUILayout.Label("", "Divider", GUILayout.Width(width));
        GUILayout.Space(height * 0.25f);
        level = GUILayout.SelectionGrid(level, levels, 3, "Toggle", GUILayout.Width(windowWidth));
        GUILayout.Space(height * 0.25f);
        GUILayout.Label("", "Divider", GUILayout.Width(width));
        GUILayout.Space(height * 0.25f);
        if (GUILayout.Button("Back", GUILayout.Width(width), GUILayout.Height(height)) || Input.GetKeyUp(KeyCode.Escape))
        {
            switch (control)
            {
                case 0:
                    {
                        Settings.ControlTouch();
                        break;
                    }
                case 1:
                    {
                        Settings.ControlAccelerometer();
                        break;
                    }
                case 2:
                    {
                        Settings.ControlAccelerometerAndTouch();
                        break;
                    }
            }

            switch (level)
            {
                case 0:
                    {
                        Settings.Easy();
                        break;
                    }
                case 1:
                    {
                        Settings.Medium();
                        break;
                    }
                case 2:
                    {
                        Settings.Hard();
                        break;
                    }
            }

            Settings.SaveSettings();

            showSettings = false;
            showMainMenu = true;
        }
        GUILayout.EndVertical();
        GUI.EndGroup();
    }

    private void ShowMainMenu()
    {
        GUILayout.BeginArea(new Rect(x, y, windowWidth, windowHeight));
        GUILayout.BeginVertical();
        GUILayout.Label("Earth Invaders", GUILayout.Width(width));
        if (GUILayout.Button("Play Game", GUILayout.Width(width), GUILayout.Height(height)))
        {
            Application.LoadLevel("SaveEarth");
        }
        if (GUILayout.Button("Instructions", GUILayout.Width(width), GUILayout.Height(height)))
        {
            showInstructions = true;
            showMainMenu = false;
        }
        if (GUILayout.Button("Settings", GUILayout.Width(width), GUILayout.Height(height)))
        {
            showSettings = true;
            showMainMenu = false;
        }
        if (GUILayout.Button("Credits", GUILayout.Width(width), GUILayout.Height(height)))
        {
            showCredits = true;
            showMainMenu = false;
        }
        if (GUILayout.Button("Exit", GUILayout.Width(width), GUILayout.Height(height)))
        {
            Application.Quit();
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    public void ShowInstructions()
    {
        GUILayout.BeginArea(new Rect(x, y, windowWidth, windowHeight));
        GUILayout.Label("Instructions", GUILayout.Width(width));
        scrollView = GUILayout.BeginScrollView(scrollView, GUILayout.Width(width));
        GUILayout.Box(GenerateInstructions());
        GUILayout.EndScrollView();
        if (GUILayout.Button("Main Menu", GUILayout.Width(width)) || Input.GetKeyUp(KeyCode.Escape))
        {
            showInstructions = false;
            showMainMenu = true;
        }
        GUILayout.EndArea();
    }

    public void ShowCredits()
    {
        GUILayout.BeginArea(new Rect(x, y, windowWidth, windowHeight));
        GUILayout.Label("Credits", GUILayout.Width(width));
        scrollView = GUILayout.BeginScrollView(scrollView, GUILayout.Width(width));
        GUILayout.Box(GenerateCredits());
        GUILayout.EndScrollView();
        if (GUILayout.Button("Main Menu", GUILayout.Width(width)) || Input.GetKeyUp(KeyCode.Escape))
        {
            showCredits = false;
            showMainMenu = true;
        }
        GUILayout.EndArea();
    }

    private string GenerateInstructions()
    {
        StringBuilder instructions = new StringBuilder();
        instructions.AppendLine("Save earth by aquiring over 50000 score while the earth's health is full. Aquire combos by destroying enemies quickly and without missing.");
        instructions.AppendLine("");
        instructions.AppendLine("<b>COMBO AWARDS</b>");
        instructions.AppendLine("Ship Health: X5");
        instructions.AppendLine("Earth Health: X10");
        instructions.AppendLine("RF: X15");
        instructions.AppendLine("ENP: X20");
        instructions.AppendLine("");
        instructions.AppendLine("<b>SHOOT</b>");
        instructions.AppendLine("Tap or touch hold anywhere on screen");
        instructions.AppendLine("");
        instructions.AppendLine("<b>CONTROL SHIP (Touch)</b>");
        instructions.AppendLine("Touch anywhere on or below your spaceship then");
        instructions.AppendLine("drag left or right to move left or right respectively.");
        instructions.AppendLine("");
        instructions.AppendLine("<b>CONTROL SHIP (Accelerometer)</b>");
        instructions.AppendLine("Tilt/rotate the phone left or right to move left or right respectively.");
        instructions.AppendLine("");
        instructions.AppendLine("<b>ACTIVATE BONUS</b>");
        instructions.AppendLine("ENP: Swipe Down");
        instructions.AppendLine("RF: Swipe Up");
        return instructions.ToString();
    }

    private string GenerateCredits()
    {
        StringBuilder credits = new StringBuilder();
        credits.AppendLine("<b>Game Developer</b>");
        credits.AppendLine("Joshua Dhanapalan");
        credits.AppendLine("");
        credits.AppendLine("<b>Art</b>");
        credits.AppendLine("chabull");
        credits.AppendLine("OpenGameArt.Org");
        credits.AppendLine("");
        credits.AppendLine("<b>Sound Effects</b>");
        credits.AppendLine("AstroMenace Artwork ver 1.2 Assets Copyright (c) 2006-2007 Michael Kurinnoy, Viewizard");
        credits.AppendLine("Michel Baradari: apollo-music.de");
        credits.AppendLine("Dravenx");
        credits.AppendLine("");
        credits.AppendLine("<b>Music</b>");
        credits.AppendLine("tgfcoder: twitter.com/tgfcoder");
        credits.AppendLine("DST: www.nosoapradio.us");
        credits.AppendLine("Alexandr Zhelanov: soundcloud.com/alexandr-zhelanov");
        credits.AppendLine("");
        return credits.ToString();
    }
    #endregion
}