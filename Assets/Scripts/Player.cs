using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float ShootDelay;
    public AudioClip ShootSound;
    public float Speed;
    public float TouchRadius;
    public int SwipeLength;
    public int SwipeSpeed;
    public GUIText GuiENP;
    public int ENP;
    public float ENPEnabledTime;
    public AudioClip ENPEnabledSound;
    public GUIText GuiRapidFire;
    public float RapidFireShootDelay;
    public int RapidFire;
    public float RapidFireEnabledTime;
    public AudioClip RapidFireEnabledSound;
    public AudioClip PowerUpDisabledSound;
    public AudioClip HitSound;
    public HealthBar healthBar;
    public GameObject Explosion;
    public GameObject PlayerBullet;
    public int MoveControl;
    public Pause PauseButton;

    private float spriteHalfWidth;
    private float spriteHalfHeight;
    private float screenLeftEdge;
    private float screenRightEdge;
    private float shootDelay;
    private bool hasShot;
    private bool RapidFireIsEnabled;
    private bool ENPIsEnabled;
    private Vector2 swipeLength;
    private Vector2 startSwipePos;
    private int level;

    public void Start()
    {
        //Set Difficulty
        level = GameObject.FindObjectOfType<GameManager>().Level;
        ENP /= level;
        RapidFire /= level;

        GuiENP.text = "ENP: " + ENP;
        GuiRapidFire.text = "RF: " + RapidFire;

        spriteHalfWidth = GetComponent<SpriteRenderer>().bounds.size.x * 0.5f;
        spriteHalfHeight = GetComponent<SpriteRenderer>().bounds.size.y * 0.5f;
        screenLeftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        screenRightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        hasShot = false;
        SetShootDelay();
        shootDelay = ShootDelay;
        MoveControl = PlayerPrefs.GetInt("MoveControl", 0);
    }

    public void Update()
    {
        TouchHandler();
        CageMovement();

		KeyBoardInput();
    }

	void KeyBoardInput ()
	{
		if (Input.GetKey (KeyCode.W)) 
		{
			StartCoroutine(EnableRapidFire());
		}
		if(Input.GetKey(KeyCode.S))
		{
			StartCoroutine(EnableENP());
		}
		if (Input.GetKey (KeyCode.Space)) 
		{
			StartCoroutine(Shoot());
		}
		if (Input.GetKey (KeyCode.A)) 
		{
			rigidbody2D.AddForce(new Vector2(-1 * 50, 0));
		}
		if (Input.GetKey (KeyCode.D)) 
		{
			rigidbody2D.AddForce(new Vector2(1 * 50, 0));
		}
	}

    public void FixedUpdate()
    {
        //Accelerometer Movement
        if (MoveControl == 1 || MoveControl == 2)
        {
            AccelerometerMove();
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "EnemyBullet")
        {
            EnemyBullet enemyBullet = coll.GetComponent<EnemyBullet>();
            if (healthBar.UpdateHealth(-enemyBullet.Damage))
            {
                coll.SendMessage("destroy", false);
                destroy();
            }
            else
            {
                audio.PlayOneShot(HitSound);
                StartCoroutine(Colorize());
                coll.SendMessage("destroy", false);
            }
        }
    }

    #region Helpers

    private void SetShootDelay()
    {
        switch (level)
        {
            case 1:
                {
                    ShootDelay = 0.36f;
                    break;
                }
            case 2:
                {
                    ShootDelay = 0.33f;
                    break;
                }
            case 3:
                {
                    ShootDelay = 0.3f;
                    break;
                }
        }
    }

    public void ChangeShootDelay(float value)
    {
        ShootDelay = value;
        shootDelay = ShootDelay;
    }

    private void TouchHandler()
    {
        //Touch Input
        foreach (var touch in Input.touches)
        {
            //Touch Movement or both
            if (MoveControl == 0 || MoveControl == 2)
            {
                Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
                //get bitmask of Touch layer index 8
                int layerMask = 1 << 8;
                RaycastHit2D hit = Physics2D.CircleCast(touchWorldPos, TouchRadius, Vector2.up, Mathf.Infinity, layerMask);

                if (hit.collider == collider2D)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        touchWorldPos.y = transform.position.y;
                        transform.position = touchWorldPos;
                    }
                }

            }

            //Touch Shooting
            if (touch.phase == TouchPhase.Began)
            {
                startSwipePos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                swipeLength.y = Vector2.Distance(new Vector2(0, startSwipePos.y), new Vector2(0, touch.position.y));
            }
            else if (touch.phase == TouchPhase.Stationary)
            {

                StartCoroutine(Shoot());
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                StartCoroutine(Shoot());
                swipeLength = Vector2.zero;
            }

            //Touch Swipe
            if (swipeLength.y > SwipeLength)
            {
                //Swipe up
                if (touch.deltaPosition.y > SwipeSpeed)
                {
                    StartCoroutine(EnableRapidFire());
                }

                //Swipe Down
                if (touch.deltaPosition.y < -SwipeSpeed)
                {
                    StartCoroutine(EnableENP());
                }
            }
        }
    }

    private void AccelerometerMove()
    {
        rigidbody2D.AddForce(new Vector2(Input.acceleration.x * Speed, 0));
    }

    private void CageMovement()
    {
        if (transform.position.x < screenLeftEdge + spriteHalfWidth)
        {
            transform.position = new Vector3(screenLeftEdge + spriteHalfWidth, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > screenRightEdge - spriteHalfWidth)
        {
            transform.position = new Vector3(screenRightEdge - spriteHalfWidth, transform.position.y, transform.position.z);
        }
    }

    private IEnumerator Shoot()
    {
        if (!hasShot)
        {
            hasShot = true;
            Instantiate(PlayerBullet,
            new Vector3(transform.position.x, transform.position.y + (spriteHalfHeight * 0.8f), transform.position.z),
            Quaternion.AngleAxis(180, Vector3.forward));
            audio.PlayOneShot(ShootSound);
            yield return new WaitForSeconds(shootDelay);
            hasShot = false;
        }
    }

    public void UpdateENP(int value)
    {
        ENP += value;
        GuiENP.text = "ENP: " + ENP;
    }

    public void UpdateRF(int value)
    {
        RapidFire += value;
        GuiRapidFire.text = "RF: " + RapidFire;
    }

    private IEnumerator EnableRapidFire()
    {
        if (!RapidFireIsEnabled && RapidFire > 0)
        {
            RapidFireIsEnabled = true;
            shootDelay = RapidFireShootDelay;
            UpdateRF(-1);
            audio.PlayOneShot(RapidFireEnabledSound);
            yield return new WaitForSeconds(RapidFireEnabledTime);
            shootDelay = ShootDelay;
            RapidFireIsEnabled = false;
            audio.PlayOneShot(PowerUpDisabledSound);
        }
    }

    private IEnumerator EnableENP()
    {
        if (!ENPIsEnabled && ENP > 0)
        {
            ENPIsEnabled = true;
            UpdateENP(-1);
            audio.PlayOneShot(ENPEnabledSound);
            FindObjectOfType<GameManager>().EnpIsEnabled = true;
            yield return new WaitForSeconds(ENPEnabledTime);
            FindObjectOfType<GameManager>().DisableEnp();
            ENPIsEnabled = false;
            audio.PlayOneShot(PowerUpDisabledSound);
        }
    }

    private IEnumerator Colorize()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
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

