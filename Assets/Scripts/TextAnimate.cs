using UnityEngine;
using System.Collections;

public class TextAnimate : MonoBehaviour
{
    public float AliveTime;
    public Vector2 Direction;

    public void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        rigidbody2D.AddForce(Direction);
        yield return new WaitForSeconds(AliveTime);
        Destroy(gameObject);
    }
}

