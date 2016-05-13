using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int Level;
    public int Score;
    public int WinScore;
    public AudioClip WinSound;
    public float SpawnDelayMin;
    public float DelayChangeAmt;
    public int ScoreDivider;
    public float SpawnDelayMax;
    public GUIText GuiScore;
    public GUIText GuiInfo;
    public bool CanSpawn;
    public GameObject EnemyFive;
    public GameObject EnemyTwo;
    public GameObject EnemyThree;
    public GameObject EnemyFour;
    public GameObject EnemyOne;
    public GUIText GuiCombo;
    public GUIText GuiScoreAward;
    public Timer ComboTimer;
    public Bonus Bonus;
    public Pause PauseButton;
    public bool EnpIsEnabled;
    public float ENPEnemyMoveSpeedPercent;

    public int PlayerHealthAwardMultiple;
    public GUIText PlayerHealthAwardText;
    public int EarthHealthAwardMultiple;
    public GUIText EarthHealthAwardText;
    public int RFAwardMultiple;
    public GUIText RFAwardText;
    public int ENPAwardMultiple;
    public GUIText ENPAwardText;

    private int Combo;
    private bool enemySpawned;
    private float leftScreenEdge;
    private float rightScreenEdge;
    private float topScreenEdge;
    private Vector3 lastComboPosition;
    private int scoreDividedAmount;
    private bool PlayerWon;

    private float enemy1SpawnRate;
    private float enemy2SpawnRate;
    private float enemy3SpawnRate;
    private float enemy4SpawnRate;

    public void Start()
    {
        Level = PlayerPrefs.GetInt("Difficulty", 1);
        enemy1SpawnRate = PlayerPrefs.GetFloat("EnemyOneSpawnProbability", 0.45f);
        enemy2SpawnRate = PlayerPrefs.GetFloat("EnemyTwoSpawnProbability", 0.65f);
        enemy3SpawnRate = PlayerPrefs.GetFloat("EnemyThreeSpawnProbability", 0.8f);
        enemy4SpawnRate = PlayerPrefs.GetFloat("EnemyFourSpawnProbability", 0.93f);
        leftScreenEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        rightScreenEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        topScreenEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;

        GuiScore.text = "Score: " + Score;
        scoreDividedAmount = 1;
        PlayerWon = false;
        SetSpawnDelay();
    }

    public void Update()
    {
        if (CanSpawn)
        {
            StartCoroutine(SpawnEnemy());
        }

        HandleENP();
        ShowComboScoreAwarded();
        UpdateSpawnDelay();
        HandleBackButton();
    }

    #region Helpers
    public void UpdatePlayerScore(int val, Vector3 position)
    {
        GuiScoreAward.text = "+" + val;
        Instantiate(GuiScoreAward, Camera.main.WorldToViewportPoint(position), Quaternion.identity);
        Score += val;
        GuiScore.text = "Score: " + Score;
        if (!PlayerWon)
        {
            CheckWin();
        }
    }

    private void SetSpawnDelay()
    {
        switch (Level)
        {
            case 1:
                {
                    SpawnDelayMin = 1f;
                    SpawnDelayMax = 6;
                    break;
                }
            case 2:
                {
                    SpawnDelayMin = 1f;
                    SpawnDelayMax = 4;
                    break;
                }
            case 3:
                {
                    SpawnDelayMin = 1f;
                    SpawnDelayMax = 3;
                    break;
                }
        }
    }

    private void HandleBackButton()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (PauseButton.GameIsPaused())
            {
                PauseButton.ResumeGame();
            }
            else
            {
                PauseButton.PauseGame();
            }
        }
    }

    private void HandleENP()
    {
        if (EnpIsEnabled)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length > 0)
            {
                foreach (var enemy in enemies)
                {
                    Enemy en = enemy.GetComponent<Enemy>();
                    if (!en.EnpIsEnabled())
                    {
                        en.EnableEnp(ENPEnemyMoveSpeedPercent);
                    }
                }
            }
        }
    }

    public void DisableEnp()
    {
        EnpIsEnabled = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            foreach (var enemy in enemies)
            {
                Enemy en = enemy.GetComponent<Enemy>();
                en.ResetEnp();
            }
        }
    }

    private void CheckWin()
    {
        if (Bonus.AwardWin() && Score >= WinScore)
        {
            CanSpawn = false;
            PauseButton.DisablePause();
            PlayerWon = true;
            DisplayInfo("You Win! :)", 5, true, WinSound);
        }
    }

    public void DisplayInfo(string message, float seconds, bool goToMainMenu, AudioClip sound = null)
    {
        StartCoroutine(ShowInfo(message, seconds, goToMainMenu, sound));
    }

    public IEnumerator ShowInfo(string message, float seconds, bool goToMainMenu, AudioClip sound = null)
    {
        GuiInfo.text = message;
        GuiInfo.enabled = true;
        if (sound != null)
        {
            audio.PlayOneShot(sound);
        }
        yield return new WaitForSeconds(seconds);
        GuiInfo.enabled = false;
        if (goToMainMenu)
        {
            Application.LoadLevel("MainMenu");
        }
    }

    public void UpdateCombo(Vector3 position)
    {
        if (!ComboTimer.TimerStarted)
        {
            ComboTimer.ResetTimer();
            ComboTimer.StartTimer();
            Combo = 1;
        }
        else
        {
            Combo++;
            ComboTimer.ResetTimer();

            //Store last combo position for showing score awarded for combo
            lastComboPosition = position;
            ShowComboText();

            AwardBonusForCombo();
        }
    }

    private void AwardBonusForCombo()
    {
        if (Combo % PlayerHealthAwardMultiple == 0)
        {
            PlayerHealthAwardText.text = "+" + 5;
            Instantiate(PlayerHealthAwardText, Camera.main.WorldToViewportPoint(lastComboPosition), Quaternion.identity);
            Bonus.AwardPlayerHealth(5);
        }
        if (Combo % EarthHealthAwardMultiple == 0)
        {
            EarthHealthAwardText.text = "+" + 50;
            Instantiate(EarthHealthAwardText, Camera.main.WorldToViewportPoint(lastComboPosition), Quaternion.identity);
            Bonus.AwardEarthHealth(50);
        }
        if (Combo % RFAwardMultiple == 0)
        {
            RFAwardText.text = "+1";
            Instantiate(RFAwardText, Camera.main.WorldToViewportPoint(lastComboPosition), Quaternion.identity);
            Bonus.AwardRapidFire();
        }
        if (Combo % ENPAwardMultiple == 0)
        {
            ENPAwardText.text = "+1";
            Instantiate(ENPAwardText, Camera.main.WorldToViewportPoint(lastComboPosition), Quaternion.identity);
            Bonus.AwardENP();
        }
    }

    public void ShowComboText()
    {
        GuiCombo.text = "X" + Combo;
        Instantiate(GuiCombo, Camera.main.WorldToViewportPoint(lastComboPosition), Quaternion.identity);
    }

    public void ShowComboScoreAwarded()
    {
        //if combo count has stopped
        if (Combo > 1 && ComboTimer.GetTimeLeft() == 0)
        {
            //show score awarded for combo attained
            UpdatePlayerScore(Combo * 10, lastComboPosition);
            Combo = 0;
        }
    }

    private IEnumerator SpawnEnemy()
    {
        if (!enemySpawned)
        {
            float randomVal = Random.value;
            Vector3 spawnPosition = new Vector3(Random.Range(leftScreenEdge, rightScreenEdge), topScreenEdge * 1.2f, 0);

            if (randomVal < enemy1SpawnRate)
            {
                AdjustSpawnPosition(EnemyOne, ref spawnPosition);
                Instantiate(EnemyOne, spawnPosition, Quaternion.AngleAxis(180, Vector3.forward));

            }
            else if (randomVal < enemy2SpawnRate)
            {
                AdjustSpawnPosition(EnemyTwo, ref spawnPosition);
                Instantiate(EnemyTwo, spawnPosition, Quaternion.AngleAxis(180, Vector3.forward));
            }
            else if (randomVal < enemy3SpawnRate)
            {
                AdjustSpawnPosition(EnemyThree, ref spawnPosition);
                Instantiate(EnemyThree, spawnPosition, Quaternion.AngleAxis(180, Vector3.forward));
            }
            else if (randomVal < enemy4SpawnRate)
            {
                AdjustSpawnPosition(EnemyFour, ref spawnPosition);
                Instantiate(EnemyFour, spawnPosition, Quaternion.AngleAxis(180, Vector3.forward));
            }
            else
            {
                AdjustSpawnPosition(EnemyFive, ref spawnPosition);
                Instantiate(EnemyFive, spawnPosition, Quaternion.AngleAxis(180, Vector3.forward));
            }

            enemySpawned = true;
            yield return new WaitForSeconds(Random.Range(SpawnDelayMin, SpawnDelayMax));
            enemySpawned = false;
        }
    }

    private void UpdateSpawnDelay()
    {
        if (Score < WinScore * 0.66f && Score > WinScore / ScoreDivider * scoreDividedAmount)
        {
            SpawnDelayMax -= SpawnDelayMax * DelayChangeAmt;
            scoreDividedAmount++;
        }
        if (Score > WinScore * 0.75)
        {
            FindObjectOfType<Player>().ChangeShootDelay(0.3f);
        }
    }

    private void AdjustSpawnPosition(GameObject enemy, ref Vector3 spawnPosition)
    {
        float spriteHalfWidth = enemy.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f;
        if (spawnPosition.x - spriteHalfWidth < leftScreenEdge)
        {
            spawnPosition.x = leftScreenEdge + spriteHalfWidth;
        }
        else if (spawnPosition.x + spriteHalfWidth > rightScreenEdge)
        {
            spawnPosition.x = rightScreenEdge - spriteHalfWidth;
        }
    }
    #endregion
}

