using UnityEngine;
using System.Collections;
/*
public class WeaponBehind : Weapon {

	public GameObject go;

	public override void Fire()
	{
		if (!gameObject.activeInHierarchy) return;
		if (Time.time - lastShot < def.delayBetweenShots) return;

		
		Projectile p;


		switch (type)
		{
		case WeaponType.blaster:
			p = MakeProjectile(false);
			p.SetType(Hero.S.weapons[0].type);
			p.rigidbody.velocity = Vector3.down * def.velocity;

			break;
			
		case WeaponType.spread:
			p = MakeProjectile(false);
			p.SetType(Hero.S.weapons[0].type);
			p.rigidbody.velocity = Vector3.down * def.velocity;
			p = MakeProjectile(false);
			p.SetType(Hero.S.weapons[0].type);
			p.rigidbody.velocity = new Vector3(-.2f, -0.9f, 0) * def.velocity;
			p = MakeProjectile(false);
			p.SetType(Hero.S.weapons[0].type);
			p.rigidbody.velocity = new Vector3(.2f, -0.9f, 0) * def.velocity;
			break;

		case WeaponType.phaser:
			p = MakeProjectile(false);
			//Vector3 tempPos = pos;
			p.rigidbody.velocity = -p.transform.position;
			p.rigidbody.velocity = -p.transform.position;
			p = MakeProjectile(false);
			break;

		case WeaponType.autoTurret:
			p = MakeProjectile(false);
			p.rigidbody.velocity = go.transform.position;
			break;
		}
	}
}
*/