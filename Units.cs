using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    public LayerMask enemyLayer;
	public HealthBar healthBar;
	public AudioClip clip;

    private bool attackMode, isAlive;
    private float radius;
    private Vector3 castlePoint, goTo, myPosition;
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator anim;
	private Quaternion bodyRotation;
	private Collider col;
	private int hp, attackDamage;
	
	public int HP{get; set;}
	public float Radius{get; set;}
	public int AttackDamage{get; set;}
	public bool IsAlive{get; set;} = true;
	public bool AttackMode{get; set;} = false;
	public Collider Col{get; set;}
	public Quaternion BodyRotation{get; set;}
	public Vector3 GoTo{get; set;}
	public Vector3 MyPosition{get; set;}
	public UnityEngine.AI.NavMeshAgent Agent{get; set;}
	
	void Start()
	{
		castlePoint = GameObject.Find("Area/Buildings/Stronghold").transform.position;
		Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
		if (healthBar) 
			healthBar.SetMaxHealth(HP);
	}
	
	void LateUpdate()
    {
		
		if (HP <= 0 && IsAlive) 
        {
			Death();			
        }
		
		if (IsAlive)
		{
			Move();
		}
		else
		{	// костыль, чтобы тело не двигалось, не скользило. фиксирует позицию до дестроя.
			transform.position = GoTo;
			transform.rotation = BodyRotation;
		}
    }
	
	public virtual void Death()
	{
		GoTo = MyPosition;
		BodyRotation = transform.rotation;
		
		IsAlive = false;
		transform.tag = "Finish";
		Destroy(GetComponent<Collider>());
		Destroy(agent);
		anim.SetTrigger("isDead");
		Destroy(gameObject, 2f);
		
		GameObject sound = new GameObject("sound");
		sound.transform.position = MyPosition;
		AudioSource audioSource = sound.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.volume = 0.02f;
		audioSource.pitch = Random.Range(0.9f, 1.1f);
		audioSource.Play(0);
		
		Destroy(sound, 2f);
	}
	
	public virtual void Move()
	{
		MyPosition = transform.position;
		FindTarget();

		//Debug.DrawLine(MyPosition, GoTo, Color.yellow);
		transform.LookAt(GoTo);
		anim.SetBool("Attack", AttackMode);
		Agent.SetDestination(GoTo);
	}
	
	public void FindTarget()
	{
		Collider[] colliders = Physics.OverlapSphere(MyPosition, Radius, enemyLayer);

		if (colliders.Length > 0)  // если в радиусе защитники (пехота, башни, замок), пойдем к ним
		{
			float distance, min = Radius; 
			
			foreach(Collider collider in colliders)
			{
				distance = Vector3.Distance(collider.ClosestPoint(MyPosition), MyPosition);
				if (distance < min) 
				{
					min = distance;
					GoTo = collider.transform.position;   // ближайший противник
					Col = collider;
				}
			}
			
			if (min < 3.0f)
			{
				AttackMode = true; // анимация запускает функцию Атаки на определенный фрейм.
			}
			else
			{
				AttackMode = false;
				Col = null;
			}
		} 
		else    // иначе идём к замку
		{
			AttackMode = false;
			Col = null;
			GoTo = castlePoint;
		}
	}
	
	public void Attack()
	{
		if (Col != null)
		{
			Col.gameObject.SendMessage("MakeDamage",AttackDamage,SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public virtual void MakeDamage(int damage)
	{
		HP -= damage;
		if (HP < 0)
			HP = 0;
		healthBar.SetHealth(HP);
	}
}
