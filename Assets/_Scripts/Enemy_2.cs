using UnityEngine;
using System.Collections;

public class Enemy_2 : Enemy {

	//enemy 2 uses a sin wave to modify a 2point linear interpolation
	public Vector3[] points;
	public float birthTime;
	public float lifeTime = 10;

	//determines how much the sine wave will affect movement
	public float sinEccentricity = 0.6f;

	// Use this for initialization
	void Start () 
	{
		//initialize the points
		points = new Vector3[2];

		//find utils.cambounds
		Vector3 cbMin = Utils.camBounds.min;
		Vector3 cbMax = Utils.camBounds.max;

		Vector3 v = Vector3.zero;
		//pick any point on the left side of the screen
		v.x = cbMin.x - Main.S.enemySpawnPadding;
		v.y = Random.Range (cbMin.y, cbMax.y);
		points[0] = v;

		//pick any point on the right side of the screen
		v = Vector3.zero;
		v.x = cbMax.x + Main.S.enemySpawnPadding;
		v.y = Random.Range(cbMin.y, cbMax.y);
		points[1] = v;

		//possibly swap sides
		if (Random.value < 0.5f)
		{
			//setting the .x of each point to its negative will move it to the other side of the screen
			points[0].x *= -1;
			points[1].x *= -1;
		}

		//set the birthTime to the current time
		birthTime = Time.time;
	}

	public override void Move()
	{
		//Bezier curves work based on a u value between 0 & 1
		float u = (Time.time - birthTime) / lifeTime;

		//if u > 1 then it has been longer than lifetime since birthtime
		if (u > 1)
		{
			//enemy has finished life
			Destroy (this.gameObject);
			return;
		}

		//adjust u by adding an easing curve based on sine wave
		u = u + sinEccentricity*(Mathf.Sin(u*Mathf.PI*2));

		//interpolate the two linear interpolation points
		pos = (1-u)*points[0] + u*points[1];
	}
}
