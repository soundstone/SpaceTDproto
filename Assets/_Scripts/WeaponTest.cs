using UnityEngine;
using System.Collections;

public enum WeaponType 
{
	none,
	blaster,
	spread,
	phaser,
	autoTurret,
	missile,
	laser,
	shield
}

[System.Serializable]
public class WeaponDefinition
{
	public WeaponType type = WeaponType.none;
	public string letter;							//Letter to show on power-up
	public Color color = Color.white;				//color of collar and power-up
	public GameObject projectilePrefab;
	public Color projectileColor = Color.white;
	public float damageOnHit = 0;					//amount of damage caused
	public float continuousDamage = 0;				//damage per second (laser types)
	public float delayBetweenShots = 0;
	public float velocity = 20;		//speed of projectiles
	
	
	//*****														*********\\
	//NOTE: Weapon_prefabs, colors, and so on are set in the class Main.*\\
	//*****														*********\\
}

public class WeaponTest : MonoBehaviour 
{

	static public Transform PROJECTILE_ANCHOR;
	
	public bool ________________________;
	[SerializeField]
	protected WeaponType _type = WeaponType.blaster;
	public WeaponDefinition	def;
	public GameObject collar;
	public float lastShot;	//time since last shot was fired
	
	
	
	void Awake()
	{
		collar = transform.Find("Collar").gameObject;
	}
	
	void Start()
	{
		SetType(_type);
		
		if (PROJECTILE_ANCHOR == null)
		{
			GameObject go = new GameObject("_Projectile_Anchor");
			PROJECTILE_ANCHOR = go.transform;
		}
		
		//find the firedelegate of the parent
		GameObject parentGO = transform.parent.gameObject;
		if (parentGO.tag == "Hero")
		{
			//Hero.S.fireDelegate += Fire;
			NewHero.S.fireDelegate += Fire;
		}
		//collar = transform.Find("Collar").gameObject;
	}
	
	public WeaponType type
	{
		get {return(_type);}
		set {SetType(value);}
	}
	
	public void SetType(WeaponType wt)
	{
		_type = wt;
		if (type == WeaponType.none)
		{
			this.gameObject.SetActive(false);
			return;
		}
		else 
		{
			this.gameObject.SetActive(true);
		}
		def = Main.GetWeaponDefinition(_type);
		collar.renderer.material.color = def.color;
		lastShot = 0;				//0 allows you to fire immediately after _type is set
	}
	
	public virtual void Fire()
	{
		if (!gameObject.activeInHierarchy) return;
		if (Time.time - lastShot < def.delayBetweenShots) return;
		
		Projectile p;
		switch (type)
		{
		case WeaponType.blaster:
			p = MakeProjectile(true);
			p.rigidbody.velocity = Vector3.up * def.velocity;
			break;
			
		case WeaponType.spread:
			p = MakeProjectile(true);
			p.rigidbody.velocity = Vector3.up * def.velocity;
			p = MakeProjectile(true);
			p.rigidbody.velocity = new Vector3(-.2f, 0.9f, 0) * def.velocity;
			p = MakeProjectile(true);
			p.rigidbody.velocity = new Vector3(.2f, 0.9f, 0) * def.velocity;
			break;
			
		case WeaponType.phaser:
			p = MakeProjectile(true);
			//Vector3 tempPos = pos;
			p.rigidbody.velocity = p.transform.position;
			//p = MakeProjectile();
			p = MakeProjectile(true);
			p.rigidbody.velocity = p.transform.position;
			
			
			//p.rigidbody.velocity = new Vector3(-.1f, 0.5f, 0) * def.velocity;
			break;
			
			
		}
	}
	
	public Vector3 pos
	{
		get { return (this.transform.position); }
		set { this.transform.position = value; }
	}
	
	public Projectile MakeProjectile(bool fromFront)
	{
		GameObject go = Instantiate(def.projectilePrefab) as GameObject;
		if (transform.parent.gameObject.tag == "Hero")
		{
			go.tag = "ProjectileHero";
			go.layer = LayerMask.NameToLayer("ProjectileHero");
		}
		else 
		{
			go.tag = "ProjectileEnemy";
			go.layer = LayerMask.NameToLayer("ProjectileEnemy");
		}
		go.transform.position = collar.transform.position;
		go.transform.parent = PROJECTILE_ANCHOR;
		Projectile p = go.GetComponent<Projectile>();
		p.type = type;
		p.FrontCannon = fromFront;
		lastShot = Time.time;
		return (p);
	}
}
