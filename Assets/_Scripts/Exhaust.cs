using UnityEngine;
using System.Collections;

public class Exhaust : MonoBehaviour {

	Component particles;

	// Use this for initialization
	void Start () {
		particles = this.particleEmitter;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StopParticles()
	{
		this.GetComponent<ParticleSystem>().enableEmission = false;
	}

	public void StartParticles()
	{
		this.GetComponent<ParticleSystem>().enableEmission = true;
	}

	public void TurnDownThrusterVolume()
	{
		this.GetComponentInChildren<AudioSource>().volume -= .1f;
		this.GetComponentInChildren<AudioSource>().pitch -= .12f;
	}

	public void TurnUpThrusterVolume()
	{
		this.GetComponentInChildren<AudioSource>().volume += .1f;
		this.GetComponentInChildren<AudioSource>().pitch += .12f;
	}
}
