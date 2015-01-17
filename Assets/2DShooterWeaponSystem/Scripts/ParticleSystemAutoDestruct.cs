using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestruct : MonoBehaviour
{

    private ParticleSystem particleSystem;

    void OnEnable()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (particleSystem != null)
	    {
	        if (!particleSystem.IsAlive())
	        {
                // Send the particle back to the object pool by setting the gameobject to disabled.
	            gameObject.SetActive(false);
	        }
	    }
	}
}
