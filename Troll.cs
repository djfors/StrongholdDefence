using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Units
{
	public Detonator boomPrefab;
	
	public Troll()
	{
		HP = 1;
		AttackDamage = 500;
		Radius = 30f;
	}
	
	public override void MakeDamage(int damage)
	{
		GameController.moneyBank += 300;
		Death();
	}
	
	public override void Death()
	{
		Instantiate(boomPrefab, transform.position, Quaternion.identity);
			
		GameObject explosionSound = new GameObject("explosionSound");
		explosionSound.transform.position = transform.position;
		AudioSource audioSource = explosionSound.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.volume = 0.02f;
		audioSource.Play(0);
		Destroy(explosionSound, 2f);
		
		Destroy(gameObject);
	}
	
	public override void Move()
	{
		MyPosition = transform.position;
		FindTarget();
		transform.LookAt(GoTo);
		Agent.SetDestination(GoTo);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.layer == 6)	// DefendersLayer
		{
			Attack();
			Death();
		}
		
	}
}