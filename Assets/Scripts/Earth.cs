using UnityEngine;
using System.Collections;

public class Earth : MonoBehaviour
{
    public float SpeedRot;
    public HealthBar healthBar;
    public GameObject Explosion;
    public Pause PauseButton;

    private float maxPosY;
    private float minPosY;

    public void Start()
    {
        maxPosY = transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y * 0.5f;
        minPosY = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
    }

    public void Update()
    {
        Spin();
    }

    public void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "EnemyBullet")
        {
            float randomPos = Random.Range(minPosY, maxPosY);
            if (coll.transform.position.y * 0.9f < randomPos)
            {
                EnemyBullet enemyBullet = coll.GetComponent<EnemyBullet>();

                if (healthBar.UpdateHealth(-enemyBullet.Damage))
                {
                    destroy();
                }
                coll.SendMessage("destroy", true);
            }
        }
        else if (coll.tag == "Enemy")
        {
            float randomPos = Random.Range(minPosY, maxPosY);
            if (coll.transform.position.y * 0.9f < randomPos)
            {
                Enemy enemy = coll.GetComponent<Enemy>();

                if (healthBar.UpdateHealth(-enemy.Damage))
                {
                    destroy();
                }
                coll.SendMessage("destroy", true);
            }
        }

    }

    #region Helpers

    private void Spin()
    {
        transform.Rotate(Vector3.forward * SpeedRot, Space.Self);
    }

    public void destroy()
    {
        Instantiate(Explosion, transform.position, transform.rotation);
        GameManager gameManager = Camera.main.GetComponent<GameManager>();
        gameManager.CanSpawn = false;
        PauseButton.DisablePause();
        gameManager.DisplayInfo("Game Over! :(", 5, true);
        Destroy(gameObject);
    }

    #endregion
}

