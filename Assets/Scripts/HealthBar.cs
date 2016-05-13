using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public int Health;
    public bool HealthBarVisible;
    public Transform HealthTransformFG;
    public SpriteRenderer HealthRendererFG;
    public SpriteRenderer HealthRendererBG;

    private float health;

    public void Start()
    {
        health = Health;
    }

    //returns true if health is 0 else false
    public bool UpdateHealth(int val)
    {
        health += val;
        if (health < 0)
        {
            health = 0;
        }
        else if (health > Health)
        {
            health = Health;
        }

        if (HealthTransformFG != null)
        {
            HealthTransformFG.localScale = new Vector3(health / Health - 0.01f,
                                                              HealthTransformFG.localScale.y,
                                                              HealthTransformFG.localScale.z);
            StartCoroutine(ShowHealthBar());
            if (health == 0)
            {
                destroy();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return true;
    }

    private IEnumerator ShowHealthBar()
    {
        if (!HealthBarVisible)
        {
            HealthBarVisible = true;
            HealthRendererFG.enabled = true;
            HealthRendererBG.enabled = true;
            yield return new WaitForSeconds(2);
            if (HealthRendererBG)
            {
                HealthRendererFG.enabled = false;
                HealthRendererBG.enabled = false;
            }
            HealthBarVisible = false;
        }
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    private void destroy()
    {
        Destroy(HealthRendererBG.gameObject);
        Destroy(HealthRendererFG.gameObject);
    }
}

