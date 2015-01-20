using UnityEngine;
using System.Collections;

public class testCamera : MonoBehaviour {


	public int level_Min_x;
	public int level_Max_x;
	public int level_Min_y;
	public int level_Max_y;
	public GameObject focus;

	// Use this for initialization
	void Start () {
		if (focus == null)
		{
			focus = GameObject.Find("_Hero");
		}
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		Vector3 pos = transform.position;
		pos = new Vector3(Mathf.Clamp(pos.x, -level_Min_x, level_Max_x), Mathf.Clamp(pos.y, -level_Min_y, level_Min_y), -817f);
		transform.position = pos;
		//transform.position = new Vector3(Mathf.Clamp(transform.position.x, -level_Min_x, level_Max_x), Mathf.Clamp(transform.position.y, -level_Min_y, level_Max_y), transform.position.z);
	}
}
