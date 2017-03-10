using UnityEngine;
using System.Collections;

public class InstantDeath : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other)
	{
		
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.BroadcastMessage("Death",SendMessageOptions.DontRequireReceiver);
		}
	}

}
