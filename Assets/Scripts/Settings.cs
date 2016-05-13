using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour
{
	public static void Easy()
	{
        PlayerPrefs.SetInt("Difficulty", 1);
        PlayerPrefs.SetFloat("EnemyOneSpawnProbability", 0.45f);
        PlayerPrefs.SetFloat("EnemyTwoSpawnProbability", 0.65f);
        PlayerPrefs.SetFloat("EnemyThreeSpawnProbability", 0.8f);
        PlayerPrefs.SetFloat("EnemyFourSpawnProbability", 0.93f);
	}

	public static void Medium()
	{
        PlayerPrefs.SetInt("Difficulty", 2);
        PlayerPrefs.SetFloat("EnemyOneSpawnProbability", 0.30f);
        PlayerPrefs.SetFloat("EnemyTwoSpawnProbability", 0.55f);
        PlayerPrefs.SetFloat("EnemyThreeSpawnProbability", 0.70f);
        PlayerPrefs.SetFloat("EnemyFourSpawnProbability", 0.85f);
	}

	public static void Hard()
	{
        PlayerPrefs.SetInt("Difficulty", 3);
        PlayerPrefs.SetFloat("EnemyOneSpawnProbability", 0.20f);
        PlayerPrefs.SetFloat("EnemyTwoSpawnProbability", 0.40f);
        PlayerPrefs.SetFloat("EnemyThreeSpawnProbability", 0.60f);
        PlayerPrefs.SetFloat("EnemyFourSpawnProbability", 0.80f);
    }

	public static void ControlTouch()
	{
        PlayerPrefs.SetInt("MoveControl", 0);
	}

	public static void ControlAccelerometer()
	{
        PlayerPrefs.SetInt("MoveControl", 1);
	}
    public static void ControlAccelerometerAndTouch()
    {
        PlayerPrefs.SetInt("MoveControl", 2);
    }

    public static void SoundOff()
    {
        PlayerPrefs.SetInt("SoundOn", 0);
    }

    public static void SoundOn()
    {
        PlayerPrefs.SetInt("SoundOn", 1);
    }

    public static void SaveSettings()
    {
        PlayerPrefs.Save();
    }
}

