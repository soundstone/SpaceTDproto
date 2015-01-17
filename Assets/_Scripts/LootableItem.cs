using UnityEngine;
using System.Collections;
/*
public enum Rarity
{
	GOLD,			//rarity for money drop
	COMMON,			//green (1 effect 1:1 (good:bad) )
	RARE,			//blue (2 effects 1:1, 1:0 )
	MYTHIC,			//magenta (2 effects 1:0, 1:0)
	SHIELD			//teal (shield regeneration material)
};

public enum ModifiableVariables
{
	damageOnHit,
	continuousDamage,
	delayBetweenShots,
	velocity,
	shieldLevel
};


public class Effect
{
	string text;
	string modifyingVariableone;
	string modifyingVariableTwo;
	float modValueOne;
	float modValueTwo;

	public Effect(string t, string mvOne, string mvTwo, float vOne, float vTwo)
	{
		this.text = t;
		this.modifyingVariableone = mvOne;
		this.modifyingVariableTwo = mvTwo;
		this.modValueOne = vOne;
		this.modValueTwo = vTwo;
	}


};

public class LootableItem : PowerUp {

	public float MYTHIC_THRESHOLD = 0.98f; //above this grants mythic item
	public float RARE_THRESHOLD = 0.78f;   //above this grants rare item
	public float COMMON_THRESHOLD = 0.40f;  //above this grants common item
	public float SHIELD_THRESHOLD = .25f;   //above this grants shield material
	public float GOLD_THRESHOLD = 0f;  //above this grants gold (but below shield threshold)

	public float DamageModThreshold = .80f;
	public float continuousModThreshold = .60f;
	public float delayBetweenShots = .40f;
	public float velocity = .20f;
	public float shieldLevel = 0f;


	string itemName;					//name of the lootable item
	Rarity rarity;					//rarity of item
	Effect effect;
	public int value = 0;
	bool equipped = false;
	Random RNGesus = new Random();

	
	// Use this for initialization
	void Start () 
	{
		if (Random.value > MYTHIC_THRESHOLD)
		{
			rarity = Rarity.MYTHIC;
			effect = ChooseEffect();
			value = Random.Range(500, 1000);
		}
		else if (Random.value > RARE_THRESHOLD)
		{
			rarity = Rarity.RARE;
			effect = ChooseEffect();
			value = Random.Range(250, 499);
		}
		else if (Random.value > COMMON_THRESHOLD)
		{
			rarity = Rarity.COMMON;
			effect = ChooseEffect();
			value = Random.Range(50, 225);
		}
		else if (Random.value > SHIELD_THRESHOLD)
		{
			rarity = Rarity.SHIELD;
			effect = new Effect("Shield Increase", "shieldLayer", "", 1, 0);
			value = 0;
		}
		else
		{
			rarity = Rarity.GOLD;
			value = Random.Range(25, 100);
		}


	}

	Effect ChooseEffect()
	{
		ModifiableVariables modvar = new ModifiableVariables();
		Effect tempEffect;
		switch (Random.Range(0, 5))
		{
		case 0:
			modvar = ModifiableVariables.continuousDamage;
			break;
		case 1:
			modvar = ModifiableVariables.damageOnHit;
			break;
		case 2:
			modvar = ModifiableVariables.delayBetweenShots;
			break;
		case 3: 
			modvar = ModifiableVariables.shieldLevel;
			break;
		case 4:
			modvar = ModifiableVariables.velocity;
			break;
		}


		if (Random.value > DamageModThreshold)
		{
			tempEffect = new Effect("Damage Amplifyer", ModifiableVariables.damageOnHit.ToString(), modvar.ToString(), Random.Range(1,3), Random.Range(1, 3));
		}
		else if (Random.value > continuousModThreshold)
		{
			tempEffect = new Effect("continual Doom", ModifiableVariables.continuousDamage.ToString(), modvar.ToString(), Random.Range(1,3), Random.Range(1,3));
		}
		else if (Random.value > delayBetweenShots)
		{
			tempEffect = new Effect("quick Shot", ModifiableVariables.delayBetweenShots.ToString(), modvar.ToString(), Random.Range(1,3), Random.Range(1,3));
		}
		else if (Random.value > velocity)
		{
			tempEffect = new Effect("speed Shot", ModifiableVariables.velocity.ToString(), modvar.ToString(), Random.Range(1,3), Random.Range(1,3));
		}
		else 
		{
			tempEffect = new Effect("Armor", ModifiableVariables.shieldLevel.ToString(), "", 1, 0);
		}

		return tempEffect;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
*/