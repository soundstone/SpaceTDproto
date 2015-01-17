using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum BoundsTest
{
	center, //is the center of the go on screen?
	onScreen, //are the bounds entirely on screen?
	offScreen //are the bounds entirely off screen?
}

public class Utils : MonoBehaviour {

	//======================================= Bounds Functions ========================================\\

	//creates bounds that encapsulate the two bounds passed in
	public static Bounds BoundsUnion(Bounds b0, Bounds b1)
	{
		//if the size of one of the bounds is vector3.zero, ignore that one
		if (b0.size == Vector3.zero && b1.size != Vector3.zero)
		{
			return (b1);
		}
		else if (b0.size != Vector3.zero && b1.size == Vector3.zero)
		{
			return (b0);
		}
		else if (b0.size == Vector3.zero && b1.size == Vector3.zero)
		{
			return (b0);
		}

		//stretch b0 to include the b1.min and b1.max
		b0.Encapsulate(b1.min);
		b0.Encapsulate(b1.max);
		return (b0);
	}

	public static Bounds CombineBoundsOfChildren(GameObject go)
	{
		//create an empty bounds b
		Bounds b = new Bounds(Vector3.zero, Vector3.zero);

		//if this gameobject has a Renderer component
		if (go.renderer != null)
		{
			//expand b to contain the renderer's bounds
			b = BoundsUnion(b, go.renderer.bounds);
		}

		//if this gameobject has a collider component
		if (go.collider != null)
		{
			//expand b to contain the collider bounds
			b = BoundsUnion(b, go.collider.bounds);
		}

		//recursively iterate through each child of this gameobject.transform
		foreach (Transform t in go.transform)
		{
			//expand b to contain their bounds as well
			b = BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));
		}

		return (b);
	}

	//make a static read-only public property camBounds
	static public Bounds camBounds
	{
		get 
		{
			//if _camBounds hasn't been set yet
			if (_camBounds.size == Vector3.zero)
			{
				//setcamerabounds using the default camera
				SetCameraBounds();
			}
			return (_camBounds);
		}
	}

	static public Bounds levelBounds
	{
		get 
		{
			if (_levelBounds.size == Vector3.zero)
			{
				SetLevelBounds();
			}
			return (_levelBounds);
		}
	}

	//private static field that cambounds uses
	static private Bounds _camBounds;
	static private Bounds _levelBounds;

	//used by camBounds to set _camBounds and can also be called directly
	public static void SetCameraBounds(Camera cam=null)
	{
		//if no camera was passed in, use the main camera
		if (cam == null) cam = Camera.main;
		//this makes a couple of important assumptions about the camera:
		//1. the camera is orthographic
		//2. the camera is at a rotation of R:[0,0,0]

		//make vector3s at the topLeft and bottomRight of the Screen coords
		Vector3 topLeft = new Vector3(0,0,0);
		Vector3 bottomRight = new Vector3(Screen.width, Screen.height, 0);

		//convert these to world coordinates
		Vector3 boundTLN = cam.ScreenToWorldPoint(topLeft);
		Vector3 boundBRF = cam.ScreenToWorldPoint(bottomRight);

		//adjust their zs to be at the near and far camera clipping planes
		boundTLN.z += cam.nearClipPlane;
		boundBRF.z += cam.farClipPlane;

		//find the center of the bounds
		Vector3 center = (boundTLN + boundBRF)/2f;
		_camBounds = new Bounds(center, Vector3.zero);

		//expand _camBounds to encapsulate the extents
		_camBounds.Encapsulate(boundTLN);
		_camBounds.Encapsulate(boundBRF);
	}

	public static void SetLevelBounds()
	{
		_levelBounds = new Bounds(Vector3.zero, Vector3.zero);
		foreach (Renderer r in FindObjectsOfType(typeof(Renderer)))
			_levelBounds.Encapsulate(r.bounds);
	}

	//checks to see whether the bounds bnd are within the cambounds
	public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest BoundsTest = BoundsTest.center)
	{
		return (BoundsInBoundsCheck(camBounds, bnd, BoundsTest));
	}

	//checks to see whether bounds lilb are with bounds bigb
	public static Vector3 BoundsInBoundsCheck(Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.onScreen)
	{
		//the behavior of this function is different based on the boundstest that has been selected

		//get the center of lilB
		Vector3 pos = lilB.center;

		//initialize the offset at [0,0,0]
		Vector3 off = Vector3.zero;

		switch(test)
		{
			//the center test determins what off (offset) would have to be applied to lilb to move its center back inside bigB
		case BoundsTest.center:
			if (bigB.Contains(pos))
			{
				return (Vector3.zero);
			}

			if (pos.x > bigB.max.x)
			{
				off.x = pos.x - bigB.max.x;
			}
			else if (pos.x < bigB.min.x)
			{
				off.x = pos.x - bigB.min.x;
			}
			if (pos.y > bigB.max.y)
			{
				off.y = pos.y - bigB.max.y;
			}
			else if (pos.y < bigB.min.y)
			{
				off.y = pos.y - bigB.min.y;
			}
			if (pos.z > bigB.max.z)
			{
				off.z = pos.z - bigB.max.z;
			}
			else if (pos.z < bigB.min.z)
			{
				off.z = pos.z - bigB.min.z;
			}
			return (off);

			//the onscreen test determines what off would have to be applied to keep all of lilb inside bigb
		case BoundsTest.onScreen:
			if (bigB.Contains(lilB.min) && bigB.Contains(lilB.max))
			{
				return (Vector3.zero);
			}

			if (lilB.max.x > bigB.max.x)
			{
				off.x = lilB.max.x - bigB.max.x;
			}
			else if (lilB.min.x < bigB.min.x)
			{
				off.x = lilB.min.x - bigB.min.x;
			}
			if (lilB.max.y > bigB.max.y)
			{
				off.y = lilB.max.y - bigB.max.y;
			}
			else if (lilB.min.y < bigB.min.y)
			{
				off.y = lilB.min.y - bigB.min.y;
			}
			if (lilB.max.z > bigB.max.z)
			{
				off.z = lilB.max.z - bigB.max.z;
			}
			else if (lilB.min.z < bigB.min.z)
			{
				off.z = lilB.min.z - bigB.min.z;
			}
			return (off);

			//the offscreen test determines what off would need to be applied to move any tiny part of lilb inside bigb
		case BoundsTest.offScreen:
			bool cMin = bigB.Contains(lilB.min);
			bool cMax = bigB.Contains(lilB.max);
			if (cMin || cMax)
			{
				return (Vector3.zero);
			}

			if (lilB.min.x > bigB.max.x)
			{
				off.x = lilB.min.x - bigB.max.x;
			}
			else if (lilB.max.x < bigB.min.x)
			{
				off.x = lilB.max.x - bigB.min.x;
			}
			if (lilB.min.y > bigB.max.y)
			{
				off.y = lilB.min.y - bigB.max.y;
			}
			else if (lilB.max.y < bigB.min.y)
			{
				off.y = lilB.max.y - bigB.min.y;
			}
			if (lilB.min.z > bigB.max.z)
			{
				off.z = lilB.min.z - bigB.max.z;
			}
			else if (lilB.max.z < bigB.min.z)
			{
				off.z = lilB.max.z - bigB.min.z;
			}
			return (off);

		}
		return (Vector3.zero);
	}

	//======================================= Transform Functions =========================================\\

	//this function will teratively climb up the transform.parent tree
	//until it either finds a parent with a tag != "Untagged" or no parent
	public static GameObject FindTaggedParent(GameObject go)
	{
		if (go.tag != "Untagged")
		{
			return (go);
		}

		if (go.transform.parent == null)
		{
			return (null);
		}

		return (FindTaggedParent(go.transform.parent.gameObject));
	}

	//this version handles if transform is passed into FindTaggedParent
	public static GameObject FindTaggedParent(Transform T)
	{
		return (FindTaggedParent(T.gameObject));
	}

	//======================================= Materials Functions =========================================\\

	//returns a list of all materials on this gameobject or its children
	static public Material[] GetAllMaterials(GameObject go)
	{
		List<Material> mats = new List<Material>();
		if (go.renderer != null)
		{
			mats.Add (go.renderer.material);
		}

		foreach (Transform t in go.transform)
		{
			mats.AddRange(GetAllMaterials(t.gameObject));
		}
		return (mats.ToArray());
	}

}
