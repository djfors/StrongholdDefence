using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Stronghold : MonoBehaviour
{
	public static int hp;
	public HealthBar healthBar;
	
	void Start()
    {
		hp = 1500;
		healthBar.SetMaxHealth(hp);
    }
	
	public void MakeDamage(int damage)
	{
		hp -= damage;
		if (hp < 0)
			hp = 0;
		healthBar.SetHealth(hp);
	}
}
