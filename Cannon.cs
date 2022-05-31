//using System.Collections;
using UnityEngine;

public class Cannon : Units
{
    public Transform cannonBall;
    public AudioSource audioSource;
	public AudioClip[] clips;

	private float min, distance, reloadTime, fireDelay = 2.0f;
	private Transform floor, tower, muzzle, gun, enemy;

	public Cannon()
	{
		HP = 250;
		AttackDamage = 1000;
		Radius = 30f;
	}
	
	void Start()
    {
        reloadTime = fireDelay;
        gun = transform.GetChild(0);
		muzzle = gun.GetChild(0);
		floor = transform.GetChild(1);
		tower = transform.GetChild(2);
		healthBar.SetMaxHealth(HP);
		
		audioSource = transform.GetComponent<AudioSource>();
		audioSource.clip = clips[0];
		audioSource.volume = 0.6f;
    }
	
	public override void Death()
	{
		IsAlive = false;
		reloadTime = 50f; // чтоб больше не стреляла
		transform.tag = "Finish";
		transform.GetComponent<Collider>().enabled = false;
		
		audioSource.clip = clips[1];	// breaking sound
		audioSource.volume = 0.99f;
		audioSource.Play(0);

		Destroy(gameObject, 3f);
		Destroy(gun.GetChild(1).gameObject);
		Destroy(tower.gameObject, 3f);
		Destroy(floor.gameObject, 3f);
		Destroy(muzzle.gameObject, 3f);
		Destroy(gun.gameObject, 3f);
		Destroy(transform.GetChild(3).gameObject, 3f);

		floor.GetComponent<Rigidbody>().isKinematic = false;
		muzzle.GetComponent<Rigidbody>().isKinematic = false;
		gun.GetComponent<Rigidbody>().isKinematic = false;
		tower.GetComponent<Rigidbody>().isKinematic = false;
		
		gun.DetachChildren();
		transform.DetachChildren();
		tower.GetComponent<Rigidbody>().AddExplosionForce(11.0f, transform.position, 5f, 1f, ForceMode.VelocityChange);
	}
	
	public override void Move()
	{
		if (reloadTime > 0)
			reloadTime -= Time.deltaTime;
		
		MyPosition = transform.position;

		if (enemy == null) {
			Collider[] colliders = Physics.OverlapSphere(MyPosition, Radius, enemyLayer);
			
			if (colliders.Length > 0) 
			{
				min = Radius;
			
				foreach(Collider collider in colliders)
				{
					distance = Vector3.Distance(collider.transform.position, MyPosition);
					if (distance < min) 
					{
						min = distance;
						enemy = collider.transform;   // ближайший противник
					}
				}
			}
		}
		
		if (enemy) {
			gun.LookAt(new Vector3(enemy.position.x, gun.position.y, enemy.position.z));

			if (Vector3.Distance(transform.position, enemy.position) > Radius)
				enemy = null;
			else if (reloadTime <= 0)
				Fire();
		}
	}
	
	void Fire() 
    {
        Transform shot = Instantiate(cannonBall, muzzle.position, Quaternion.identity);
        shot.LookAt(enemy);
		audioSource.Play(0);

        reloadTime = fireDelay;
    }
}