using UnityEngine;
using System.Collections;

//part is serializable like WeaponDefinition
[System.Serializable]
public class Part
{
	//these three fields need to be defined in the inspector
	public string name; 			//name of part
	public float health; 			//amount of health for part
	public string[] protectedBy; 	//the other parts that protect this

	//these two fields are set automatically in start()
	public GameObject go;			//the object for the part
	public Material mat;			//material to show damage
}

public class Enemy_4 : Enemy {

	//enemy 4 will start offscreen and the pick a random point on the screen to move to. Once arrive, it will pick again and move again. Continuing until defeated.
	public Vector3[] points;

	public float timeStart; //birth time for enemy
	public float duration = 4; //duration of movement

	public Part[]	parts;

	// Use this for initialization
	void Start () 
	{
		points = new Vector3[2];
		//initial position chosen by Main.SpawnEnemy();
		points[0] = pos;
		points[1] = pos;

		InitMovement();

		//cache gameobject & material of each part in parts
		Transform t;
		foreach (Part part in parts)
		{
			t = transform.Find(part.name);
			if (t != null)
			{
				part.go = t.gameObject;
				part.mat = part.go.renderer.material;
			}
		}
	}

	void InitMovement()
	{
		//pick a new point to move to on screen
		Vector3 p1 = Vector3.zero;
		float esp = Main.S.enemySpawnPadding;

		Bounds cBounds = Utils.camBounds;

		p1.x =  Random.Range(cBounds.min.x + esp, cBounds.max.x - esp);
		p1.y = Random.Range(cBounds.min.y + esp, cBounds.max.y - esp);

		points[0] = points[1]; //shift points[1] to points[0]
		points[1] = p1;

		//reset the time
		timeStart = Time.time;
	}

	public override void Move() 
	{
		float u = (Time.time - timeStart)/duration;
		if (u >= 1)
		{
			InitMovement();
			u=0;
		}

		u = 1 - Mathf.Pow(1-u,2); //apply ease out easing to u

		pos = (1-u)*points[0] + u * points[1];
	}

	public override void OnCollisionEnter(Collision coll)
	{

		GameObject other = coll.gameObject;
		switch (other.tag)
		{
		case "ProjectileHero":
			Projectile p = other.GetComponent<Projectile>();
			//enemies dont take damage unless theyre on screen
			bounds.center = transform.position + boundsCenterOffset;
			if (bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen) != Vector3.zero)
			{
				Destroy (other);
				break;
			}

			//the collision coll has contacts[], an array of contactpoints. Because there was a collision, we're guaranteed that there is at least a contacts[0],
			//and contactpoints have a reference to thisCollider, which will be the collider for the part of the enemy_4 that was hit.
			GameObject goHit = coll.contacts[0].thisCollider.gameObject;
			Part partHit = FindPart(goHit);
			if (partHit == null)
			{
				goHit = coll.contacts[0].otherCollider.gameObject;
				partHit = FindPart(goHit);
			}

			//check whether part was still protected
			if (partHit.protectedBy != null)
			{
				foreach (string s in partHit.protectedBy)
				{
					//if one of the protecting parts hasn't been destroyed.
					if (!Destroyed(s))
					{
						//dont destroy this yet
						Destroy (other);
						return; //return before doing damage
					}
				}
			}

			//it's not protected, take hit
			//get damage amount from proectile.type and main.W_DEFS
			partHit.health -= Main.W_DEFS[p.type].damageOnHit;
			//show damage on part
			ShowLocalizedDamage(partHit.mat);
			if (partHit.health <= 0)
			{
				//disable part only
				partHit.go.SetActive(false);
			}
			//was whole enemy destroyed?
			bool allDestroyed = true; //assume yes..
			foreach (Part part in parts)
			{
				if (!Destroyed(part))
				{
					allDestroyed = false;
					break;
				}
			}
			if (allDestroyed)
			{
				//send message to main
				Main.S.ShipDestroyed(this);
				//destroy enemy
				Destroy (this.gameObject);
			}

			Destroy (other);
			break;
		}
	}

	//==================== Parts Finding Functions =============\\

	Part FindPart(string n)
	{
		foreach (Part part in parts)
		{
			if (part.name == n)
			{
				return (part);
			}
		}
		return (null);
	}

	Part FindPart(GameObject go)
	{
		foreach (Part part in parts)
		{
			if (part.go == go)
			{
				return (part);
			}
		}
		return (null);
	}

	//=================== Part Destroyed Functions ===============\\

	bool Destroyed(GameObject go)
	{
		return (Destroyed(FindPart(go)));
	}

	bool Destroyed(string s)
	{
		return (Destroyed(FindPart(s)));
	}

	bool Destroyed(Part part)
	{
		if (part == null)
		{
			return (true);
		}

		return (part.health <= 0);
	}

	//change color of just the hit part
	void ShowLocalizedDamage(Material m)
	{
		m.color = Color.red;
		remainingDamageFrames = showDamageForFrames;
	}
}