using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public float Speed;
    public bool CanShoot;
	public float ShootDelayMin;
	public float ShootDelayMax;
    public GameObject Explosion;
    public HealthBar healthBar;
	public int Score;
	public int Damage;
    public AudioClip ShootSound;
    public AudioClip HitSound;
    public EnemyBullet EnemyBullet;

    private float speed;
    private bool hasShot;
    private bool ENPEnabled;
    private float screenBottomEdge;
    private float screenTopEdge;
    private float shootPositionY;

    public void Start()
    {
        //Set Difficulty
        int level = GameObject.FindObjectOfType<GameManager>().Level;
        Speed *= level;
        Damage *= level;
        ShootDelayMin = ShootDelayMin / level;
        ShootDelayMax = ShootDelayMax / level;

        screenBottomEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        screenTopEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        shootPositionY = GetComponent<SpriteRenderer>().bounds.size.y * 0.4f;
        speed = Speed;
        ENPEnabled = false;
    }

    public void Update()
    {
        if(!CanShoot && !ENPEnabled && transform.position.y < screenTopEdge)
        {
            CanShoot = true;
        }

        if (transform.position.y < screenBottomEdge)
        {
            destroy(false);
        }
    }

    public void FixedUpdate()
    {
        Move();
        if (CanShoot)
        {
            StartCoroutine("ShootE");
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "PlayerBullet")
        {
            PlayerBullet playerBullet = coll.GetComponent<PlayerBullet>();
            if (healthBar.UpdateHealth(-playerBullet.Damage))
            {
                Camera.main.GetComponent<GameManager>().UpdatePlayerScore(Score, transform.position);
                Camera.main.GetComponent<GameManager>().UpdateCombo(transform.position);

                //destroy player bullet
                coll.SendMessage("destroy", false);

                //destroy this enemy
                destroy(true);
            }
            else
            {
                Camera.main.GetComponent<GameManager>().UpdateCombo(transform.position);
                coll.SendMessage("destroy", false);
                audio.PlayOneShot(HitSound);
                StartCoroutine(Colorize());
            }
        }
    }

    #region Helpers
	private void Move()
	{
        rigidbody2D.AddForce(-Vector2.up * speed);
	}

	private IEnumerator ShootE()
	{
        if (!hasShot && !ENPEnabled)
        {
            hasShot = true;
            yield return new WaitForSeconds(Random.Range(ShootDelayMin, ShootDelayMax));
            Instantiate(EnemyBullet, new Vector3(transform.position.x, transform.position.y - shootPositionY, transform.position.z), Quaternion.identity);
            audio.PlayOneShot(ShootSound);
            hasShot = false;
        }
	}

    public void EnableEnp(float percent)
    {
        speed *= percent;
        ENPEnabled = true;
        CanShoot = false;
        StopCoroutine("ShootE");
    }

    public void ResetEnp()
    {
        speed = Speed;
        ENPEnabled = false;
        CanShoot = true;
    }

    public bool EnpIsEnabled()
    {
        return ENPEnabled;
    }

	private IEnumerator Colorize()
	{
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void destroy(bool explode)
    {
        if (explode)
        {
            StopCoroutine("ShootE");
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
}

