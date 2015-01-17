using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public Vector2 rotMinMax = new Vector2(15,90);
	public Vector2 driftMinMax = new Vector2(.25f, 2);
	public float lifeTime = 6f;
	public float fadeTime = 4f;

	public bool _____________;
	public WeaponType  type;
	public GameObject cube;
	public TextMesh	letter;
	public Vector3 rotPerSecond;
	public float birthTime;

	void Awake()
	{
		cube = transform.Find("Cube").gameObject;
		letter = GetComponent<TextMesh>();

		//set random velocity
		Vector3 vel = Random.onUnitSphere;
		vel.z = 0;
		vel.Normalize();
		vel *= Random.Range(driftMinMax.x, driftMinMax.y);
		rigidbody.velocity = vel;

		//set rotation of the gameobject to 0,0,0
		transform.rotation = Quaternion.identity;

		//set up the rotpersecond for cube
		rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
		                           Random.Range(rotMinMax.x, rotMinMax.y),
		                           Random.Range(rotMinMax.x, rotMinMax.y));

		//check off screen every 2 seconds
		InvokeRepeating("CheckOffScreen", 2f, 2f);

		birthTime = Time.time;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//manually rotate cube
		cube.transform.rotation = Quaternion.Euler(rotPerSecond*Time.time);

		//fade out the powerup over time
		float u = (Time.time - (birthTime+lifeTime)) / fadeTime;
		//for lifetime seconds, u will be <= 0. then it will transition to 1 over fadetime seconds
		if (u >= 1)
		{
			Destroy (this.gameObject);
			return;
		}

		//use u to determine the alpha value of the cube and letter
		if (u>0)
		{
			Color c = cube.renderer.material.color;
			c.a = 1f-u;
			cube.renderer.material.color = c;
			//fade the letter too
			c = letter.color;
			c.a = 1f - (u*0.5f);
			letter.color = c;
		}
	}

	public void SetType(WeaponType wt)
	{
		WeaponDefinition def = Main.GetWeaponDefinition(wt);
		cube.renderer.material.color = def.color;
		letter.text = def.letter;
		type = wt;
	}

	public void AbsorbedBy(GameObject target)
	{
		Destroy (this.gameObject);
		/*

		if (this.gameObject.tag == "PowerUp")
		{
			Hero.S.inventoryList.Add((LootableItem)this);
			Destroy (this.gameObject);
		}
		else 
		{
			print ("somethings wrong in absorb method");
		}*/
	}

	void CheckOffScreen()
	{
		if (Utils.ScreenBoundsCheck(cube.collider.bounds, BoundsTest.offScreen) != Vector3.zero)
		{
			Destroy(this.gameObject);
		}
	}
}
