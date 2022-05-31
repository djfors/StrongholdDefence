using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour
{
	public Transform cannon, infantryman;
	
	private AudioSource audioSource;
	private Camera cam;
	private int cannonCost = 400, infantrymanCost = 100;

    void Start()
    {
        cam = Camera.main;
		audioSource = transform.GetComponent<AudioSource>();
		//audioSource.clip = clips[0];
    }

	
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			onClickSpawnUnits(cannon, cannonCost);
		}
		
		if (Input.GetMouseButtonDown(1)) 
		{
			onClickSpawnUnits(infantryman, infantrymanCost);
		}
    }
	
	public void onClickSpawnUnits(Transform unit, int cost)
	{
		if (GameController.moneyBank < cost)
		{
			print("Казна пуста, милорд!");
			return;
		}

		RaycastHit hit;
		
		if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
		{
			Collider[] hitColliders = Physics.OverlapSphere(hit.point, 3f);
			
			foreach (Collider collider in hitColliders)
			{
				if (collider.gameObject.layer == 3 || collider.gameObject.layer >= 6) // 6,7,8
				{
					//print("Тут строить нельзя!");
					audioSource.Play(0);
					return;
				}
			}
			// если не слились на деньгах и нет защитников/зданий, то строим
			GameController.moneyBank -= cost;
			Instantiate(unit, hit.point, Quaternion.identity);
		}
	}
}
