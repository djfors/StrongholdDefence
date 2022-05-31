using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private float speed = 25f;
	private int attackDamage = 1000;
    
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.layer == 3)
		{
			col.gameObject.SendMessage("MakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
		}
		Destroy(gameObject);
	}
}
