using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	[SerializeField]
	private WeaponType	_type;
	public bool FrontCannon = true;
	public float age = 0f;
	private float birthTime;
	public float frequency = .5f;
	public float waveWidth = 8f;
	private float x0 = -12345;

	public WeaponType type
	{
		get {return _type;}
		set {SetType(value);}
	}

	void Awake()
	{
		birthTime = Time.time;
		InvokeRepeating("CheckOffscreen", 2f, 2f);

	}

	void Start()
	{
		x0 = Hero.S.pos.x;
	}

	public void SetType(WeaponType eType)
	{
		_type = eType;
		WeaponDefinition def = Main.GetWeaponDefinition(_type);
		renderer.material.color = def.projectileColor;
	}

	void CheckOffscreen()
	{
		if (Utils.ScreenBoundsCheck(collider.bounds, BoundsTest.offScreen) != Vector3.zero)
		{
			Destroy (this.gameObject);
		}
	}

	void FixedUpdate()
	{
		switch (_type)
		{
		case WeaponType.blaster:
			Vector3 tempposBlaster = transform.position;
			age = Time.time - birthTime;
			if (FrontCannon)
			{
				tempposBlaster.y += Vector3.up.y;
			}
			else 
			{
				tempposBlaster.y -= Vector3.down.y;
			}
			transform.position = tempposBlaster;
			break;
		case WeaponType.spread:
			break;
		case WeaponType.phaser:
			Vector3 tempposPhaser = transform.position;
			age = Time.time - birthTime;
			float theta = Mathf.PI * 2 * age / frequency;
			float sin = Mathf.Sin (theta);
			tempposPhaser.x = (x0 + waveWidth * sin);
			if (FrontCannon)
			{
				//temppos.y += 1f;
				tempposPhaser.y += Vector3.up.y;
			}
			else 
			{
				//temppos.y -= 1f;
				tempposPhaser.y -= Vector3.down.y;
			}
			transform.position = tempposPhaser;


			break;
		}
	}
}
