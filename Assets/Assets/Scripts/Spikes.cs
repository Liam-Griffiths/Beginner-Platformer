using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

	public int damageValue = 2;
	public float damageRate = 1;

	bool colliding = false;
	bool waiting = false;
	Collider2D collideInfo;

	// Update is called once per frame
	void Update () 
	{
		if (colliding == true && waiting == false) 
		{
			Invoke("DamageRate",damageRate);
			waiting = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{

		collideInfo = other;

		if (other.gameObject.tag == "Player")
		{
			Invoke("DamageRate",damageRate);
			waiting = true;
			colliding = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			colliding = false;
		}
	}

	void DamageRate()
	{
		SendDamage();
		waiting = false;
	}

	void SendDamage()
	{
		collideInfo.gameObject.BroadcastMessage("DamagePlayer",damageValue,SendMessageOptions.DontRequireReceiver);
	}

}
