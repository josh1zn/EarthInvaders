using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour
{
	public Player Player;
    public Earth Earth;

	public void AwardENP()
	{
        Player.UpdateENP(1);
	}

	public void AwardRapidFire()
	{
        Player.UpdateRF(1);
	}

	public void AwardPlayerHealth(int val)
	{
        Player.healthBar.UpdateHealth(val);
	}

	public void AwardEarthHealth(int val)
	{
        Earth.healthBar.UpdateHealth(val);
	}

    public bool AwardWin()
    {
        return Earth.healthBar.GetCurrentHealth() == Earth.healthBar.Health;
    }
}

