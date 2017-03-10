/*

--- Simple Unity3D Platformer
--- Author: Liam Griffiths
--- http://liam-griffiths.co.uk

--- The comments of this project assume you have only limited experience with Unity or C# programming.

--- The main tasks of this script are:

		- Move the player.
		- Jump the player.
		- Animate the player correctly.
		- Handle damage and output health to the screen.
		- Handle the death of the player.

*/

// First we include unity and some handy data structures.
using UnityEngine;
using System.Collections;

// Next is our class declaration, which inherits from MonoBehaviour.
// These first two steps are normally created for you by the unity editor.
public class PlayerController : MonoBehaviour 
{
	/*
	 * 
	 * Variable declarations. At the start of the class we will declare all our member variables,
	 * that will store information about our player. If a variable is declared as public, it will be editable
	 * in the unity editor.
	 * 
	 * The player GameObject will require four things attached to it.
	 * 		- A transform component
	 * 		- This script.
	 * 		- A 2D rigidbody.
	 * 		- A 2D box collider,
	 * 
	 */

	public Transform spawnPoint; // Where we want our player to start. This is set to a gameobject in the unity editor.

	public int playerHealth = 100; // Integer (whole number) that will store the players health.
	public GUIText healthGUI; // Also set in the editor, this is a reference to the GUI element that will output to the screen the health value.

	public float jumpVelocity = 5; // Velocity of jump.
	public float moveSpeed = 200; // Run speed.
	bool onSurface = false; // Track if we are on a surface. Set by the OnCollisionEnter function.
	
	public bool movingLeft = false; // Tracks the direction of player movement.
	public bool movingRight = false;

	Vector3 newScale; // This is a temporary variable which we store our characters sprite scale. Used to rotate the animation, between running left to running right.
	Animator anim; // Stores a reference to our animator component.
	int runAnim = Animator.StringToHash("RunParameter"); // Changes the string of "RunParameter" to a hash for the animator. Don't Worry!

	// End of variable declarations.

	// Ok this is where it all starts.
	// Start is an in built function that runs once when the player is loaded.
	void Start ()
	{
		anim = gameObject.GetComponent<Animator>(); // Set anim to the component of Animator.
		newScale = transform.localScale; // Get the current scale of the player. This is so as we know which way the player is facing initially.

		// First bit of actual doing stuff code!
		transform.position = spawnPoint.position; // Moves the player (transform.position) to the spawn position (spawnPoint.position) that we setup earlier. 
	}


	// Update is called once per frame
	// This is where the main logic of the player is done!
	void Update () 
	{
		Vector2 self_Velocity = gameObject.rigidbody2D.velocity; // We create a temporary variable to hold the velocity of the player. This is so as we only need to change the actual velocity once per update.

		/*
		 * 
		 * This might look complicated but is really isn't.
		 * Input.GetAxis("Horizontal") is just left and right, so on a pc it is A and D keys, or on a controller the analog stick, left and right.
		 * If left is pressed it will return the value -1 and for right 1.
		 * 
		 * So to move our player we simply move it along the x axis, negative values for left, positive values for right.
		 * 
		 * We multiply our input value (either -1 or 1), by our movespeed. Giving us our movement velocity.
		 * The movement speed is also multiplied by a number known as deltatime. This helps keep the player moving smoothly.
		 * 
		 */

		self_Velocity.x = (Input.GetAxis("Horizontal") * (moveSpeed * Time.deltaTime));

		// If our x velocity does not equal 0 we are moving so we need to animate the player.
		if(self_Velocity.x != 0)
		{
			/*
			 * In this section we will flip the player to make it face the correct direction when animating.
			 */

			if(Input.GetAxis("Horizontal") > 0) // Moving Right.
			{
				if(movingRight == false) // We only want to do this the moment we change direction. So this is only set to false if we were just moving in the opposite direction.
				{
					newScale.x = -transform.localScale.x; // Take the current scale of the sprite and invert it (flip).
					transform.localScale = newScale; // Apply the new flipped scale.
					movingRight = true; // Set the trackers to the correct direction.
					movingLeft = false;
				}
			}
			if(Input.GetAxis("Horizontal") < 0) // Moving Left.
			{
				// Do the same as before except in the opposite direction!
				if(movingLeft == false)
				{
					newScale.x = -transform.localScale.x;
					transform.localScale = newScale;
					movingRight = false;
					movingLeft = true;
				}
			}
			anim.SetBool(runAnim, true); // Play the animation.
		}
		else
		{
			anim.SetBool(runAnim, false); // We are not moving so turn off the animation.
		}

		/*
		 * 
		 * Next we make the player jump. Essentially before we allow the player to jump, it must meet two conditions.
		 * 		- Have a velocity this is 0, In this example i use a range between -2 and 2 to allow for jump on slopes.
		 * 		- Be touching a surface.
		 * 
		 * This isn't a perfect strategy but is close enough for most situations.
		 * 
		 */

		if((gameObject.rigidbody2D.velocity.y > -2) && (gameObject.rigidbody2D.velocity.y < 2)) // if we are not moving up or down ie, falling or in the middle of jumping.
		{
			if(Input.GetKeyDown(KeyCode.Space) && (onSurface == true)) // Check up and down axis and check if standing on a surface. If true Jump!
			{
				self_Velocity.y += jumpVelocity; // Add our jump velocity.
				onSurface = false; // Reset to check collision next frame.
			}
			else
			{
				onSurface = false;// Reset to check collision next frame.
			}
		}
		gameObject.rigidbody2D.velocity = self_Velocity; // Apply our new velocity vector back to the player.

		healthGUI.text = playerHealth.ToString(); // Update the GUI with the players health.
	}

	void OnCollisionStay2D() // If colliding.
	{
		onSurface = true; // On a surface is truue!
	}

	/*
	 * These function below are message listeners. The are called by other gameobjectd, such as spikes that might damage the player.
	 */

	public void Death()
	{
		Debug.Log("Waah waahh, you have died.");

		transform.position = spawnPoint.position; // Resets health to 100 and moves back to the start loction.
		playerHealth = 100;
	}

	public void DamagePlayer(int dmgVal)
	{
		Debug.Log("Ouch!");
		playerHealth -= dmgVal; // apply damage.

		if(playerHealth <= 0) // if Dead.
		{
			playerHealth = 0;
			Death(); // dead!
		}

		// Play death sound.
		// Add blood particle effect to character.
	}
	
}
