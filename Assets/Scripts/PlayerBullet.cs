using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour
{
	public float Speed;
	public int Damage;

    private float screenTopEdge;
    //private int level;
    
    public void Start()
    {
        //level = GameObject.FindObjectOfType<GameManager>().Level;
        screenTopEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
    }

    public void FixedUpdate()
    {
        Move();
    }

    public void Update()
    {
        if(transform.position.y > screenTopEdge)
        {
            //if (level != 1)
            //{
            //    GameObject.FindObjectOfType<GameManager>().ComboTimer.StopTimer();
            //}
            destroy();
        }
    }

    #region Helpers
    private void Move()
	{
        rigidbody2D.AddForce(Vector2.up * Speed, ForceMode2D.Impulse);
	}

	public void destroy()
	{
        Destroy(gameObject);
    }
    #endregion
}

