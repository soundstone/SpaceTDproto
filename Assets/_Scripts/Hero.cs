using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour {

	#region variables
	static public Hero S;

	public float gameResetDelay = 2f;

	//ship status information
	private float _shieldLevel = 4;

	//Weaapon Fields
	public WeaponTest[] 	weapons;

	//Inventory Fields
	//public List<LootableItem> inventoryList = new List<LootableItem>();

	//fields control movement of ship
	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;



	public bool __________________;

	public Bounds bounds;

	//delegate for weapons
	public delegate void WeaponFireDelegate();
	public WeaponFireDelegate fireDelegate;

	#endregion

	void Awake()
	{
		S = this;
		bounds = Utils.CombineBoundsOfChildren(this.gameObject);
	}

	void Start()
	{
		//reset the weapons to start _Hero with 1 blaster
		ClearWeapons();
		weapons[0].SetType(WeaponType.blaster);
	}

	void Update()
	{
		//pull in information from the input class
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");

		//change transform position based on axes
		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;

		bounds.center = transform.position;

		//keep the ship constrained to the screen bounds
		Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
		if (off != Vector3.zero)
		{
			pos -= off;
			transform.position = pos;
		}

		//rotate ship to make it feel more dynamic
		transform.rotation = Quaternion.Euler(yAxis*pitchMult, xAxis*rollMult, 0);


		if ( Input.GetAxis("Fire1") == 1 && fireDelegate != null)
		{
			fireDelegate();
		}
		
	}

	//holds reference to the last triggering gameobject
	public GameObject lastTriggerGo = null;

	void OnTriggerEnter(Collider other)
	{
		//find the tag of other.gameObject or its parent gameobjects
		GameObject go = Utils.FindTaggedParent(other.gameObject);
		//if there is a parent with tag
		if (go != null)
		{
			//make sure it's not the same triggering go as last time
			if (go == lastTriggerGo)
				return;

			lastTriggerGo = go;

			if (go.tag == "Enemy")
			{
				//decrease shield
				shieldLevel--;

				//destroy enemy
				Destroy (go);
			}
			else if (go.tag == "PowerUp")
			{
				//if shield was triggered by powerup
				AbsorbPowerUp(go);
			}
			else 
			{
				print ("Triggered: "+go.name);
			}

		}
		else 
		{
			print ("Triggered: "+other.gameObject.name);
		}
	}

	public void AbsorbPowerUp(GameObject go)
	{
		PowerUp pu = go.GetComponent<PowerUp>();
		switch (pu.type)
		{
		case WeaponType.shield:
			shieldLevel++;
			break;

		default:
			if (pu.type == weapons[0].type)
			{
				WeaponTest w = GetEmptyWeaponSlot();
				if (w != null)
				{
					//set it to pu.type
					w.SetType(pu.type);
				}
			}
			else 
			{
				ClearWeapons();
				weapons[0].SetType(pu.type);
			}
			break;
		}
		pu.AbsorbedBy(this.gameObject);
	}

	public Vector3 pos
	{
		get {return (transform.position);}
		set { transform.position = value; }
	}

	WeaponTest GetEmptyWeaponSlot()
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons[i].type == WeaponType.none)
			{
				return (weapons[i]);
			}
		}
		return (null);
	}


	void ClearWeapons()
	{
		foreach (WeaponTest w in weapons)
		{
			w.SetType(WeaponType.none);
		}
	}


	public float shieldLevel
	{
		get {return (_shieldLevel);}
		set 
		{
			_shieldLevel = Mathf.Min(value, 4);
			if (value < 0)
			{
				Destroy(this.gameObject);
				//tell main.s to restart the game after a delay
				Main.S.DelayedRestart(gameResetDelay);
			}
		}
	}
}
