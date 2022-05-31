using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Units
{
	public Orc()
	{
		HP = 100;
		AttackDamage = 50;
		Radius = 30f;
	}

	public override void Death()
	{
		GameController.moneyBank += 300;
		base.Death();
	}


}