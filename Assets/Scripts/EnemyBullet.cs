using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
	public float Speed;
	public int Damage;
	public GameObject Explosion;

    private float screenBottomEdge;

    public void Start()
    {
        int level = GameObject.FindObjectOfType<GameManager>().Level;
        Damage *=level;
        Speed *= level;
        screenBottomEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
    }

    public void Update()
    {
        if (transform.position.y < screenBottomEdge)
        {
            destroy(false);
        }
    }

    public void FixedUpdate()
    {
        Move();
    }

    #region Helpers
    public void destroy(bool explode)
	{
        if (explode)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}

	private void Move()
	{
        rigidbody2D.AddForce(-Vector2.up * Speed);
    }
    #endregion
}

