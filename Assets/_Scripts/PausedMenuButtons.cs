using UnityEngine;
using System.Collections;

public enum ButtonTrigger
{
	Mission,
	Inventory,
	SpaceStation,
	Options
};

public class PausedMenuButtons : MonoBehaviour {

	public ButtonTrigger triggerName;
	PlayMakerFSM FSM;

	public bool ____________;



	// Use this for initialization
	void Start () 
	{
		FSM = GameObject.Find("_MainCamera").GetComponent<PlayMakerFSM>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter()
	{
		print ("mouse inside trigger: "+ triggerName.ToString());
	}

	void OnMouseDown()
	{
		switch (triggerName)
		{
		case ButtonTrigger.Mission:
			print ("Triggered: Mission");
			FSM.SendEvent("UnPause");

			break;

		case ButtonTrigger.Inventory:
			print ("Triggered: Inventory");
			FSM.SendEvent("Inventory");
			break;

		case ButtonTrigger.Options:
			print ("Triggered: Options");
			FSM.SendEvent("Options");
			break;

		case ButtonTrigger.SpaceStation:
			print ("Triggered: SpaceStation");
			FSM.SendEvent("SpaceStation");
			break;
		}
	}

}
