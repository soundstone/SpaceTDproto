using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	static public Main S;
	static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;

	public GameObject[] prefabEnemies;
	public float enemySpawnPerSecond = 0.5f;
	public float enemySpawnPadding = 1.5f;

	public WeaponDefinition[] weaponDefinitions;
	public GameObject prefabPowerUp;
	public WeaponType[] powerUpFrequency = new WeaponType[]
	{
		WeaponType.blaster, 
		WeaponType.blaster, 
		WeaponType.spread, 
		WeaponType.shield
	};

	public bool ___________;

	public WeaponType[] activeWeaponTypes;
	public float enemySpawnRate;

	void Awake()
	{
		S = this;
		//set utils.cambounds
		Utils.SetCameraBounds(this.camera);
		enemySpawnRate = 1f/enemySpawnPerSecond;
		Invoke ("SpawnEnemy", enemySpawnRate);

		W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
		foreach (WeaponDefinition def in weaponDefinitions)
		{
			W_DEFS[def.type] = def;
		}

	}

	static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
	{
		if (W_DEFS.ContainsKey(wt))
			return (W_DEFS[wt]);

		//will return weapon definition for type.none
		return (new WeaponDefinition());
	}


	public void ShipDestroyed(Enemy e)
	{
		//potentially generate a powerUp
		if (Random.value <= e.powerUpDropChance)
		{
			//random.value generates a value between 0 & 1. If e.powerupdropchance is .50f, a powerup will be generated 50% of the time. For testing, its now set to 1f.

			//choose which powerup to pick
			//pick one of the possibilities of powerupfrequency
			int ndx = Random.Range(0, powerUpFrequency.Length);
			WeaponType puType = powerUpFrequency[ndx];

			//spawn a powerup
			GameObject go = Instantiate(prefabPowerUp) as GameObject;
			PowerUp pu = go.GetComponent<PowerUp>();
			//set it to the proper weapontype
			pu.SetType(puType);

			//set position to that of destroyed ship
			pu.transform.position = e.transform.position;
		}
	}


	void Start()
	{
		activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
		for (int i = 0; i < weaponDefinitions.Length; i++) 
		{
			activeWeaponTypes[i] = weaponDefinitions[i].type;
		}
	}

	public void SpawnEnemy()
	{
		/**
		//pick a random enemy prefab to instantiate
		int ndx = Random.Range(0, prefabEnemies.Length);
		GameObject go = Instantiate(prefabEnemies[ndx]) as GameObject;
		//position the enemy above the screen with a random x position
		Vector3 pos = Vector3.zero;
		float xMin = Utils.camBounds.min.x+enemySpawnPadding;
		float xMax = Utils.camBounds.max.x-enemySpawnPadding;
		pos.x = Random.Range(xMin, xMax);
		pos.y = Utils.camBounds.max.y + enemySpawnPadding;
		go.transform.position = pos;

		//call spawnEnemy again in couple seconds
		Invoke ("SpawnEnemy", enemySpawnRate);
		*/
	}

	public void DelayedRestart(float delay)
	{
		Invoke ("Restart", delay);
	}

	public void Restart()
	{
		Application.LoadLevel("_Scene_Main");
	}
}
