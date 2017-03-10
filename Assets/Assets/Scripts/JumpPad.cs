using UnityEngine;
using System.Collections;

public class JumpPad : MonoBehaviour
{

	public float padVelocity = 10;
	Vector2 self_Velocity = new Vector2(0,0);

	void OnTriggerEnter2D(Collider2D other)
	{
		self_Velocity.y = padVelocity;

		if (other.gameObject.tag == "Player")
		{
			other.gameObject.rigidbody2D.velocity = self_Velocity;
		}
	}
	
}
