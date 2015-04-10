using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    #region Variables
    public float acceleration;  // how quickly the player gains speed.
    public float decceleration; // how quickly the player slows down.
    public float maxSpeed; // the maximum speed the player can move at.
    public float jumpHeight; // the force the player jumps with.
    public float maxJumpVelocity; //the maximum velocity the player can move upwards at.
    public float jumpSpeedModifier; // changes the speed the player can move at while in the air.
    bool jumpActive = false; // tells us whether or not the player is currently jumping.
    public PhysicMaterial noFriction; //holds the physic material used to stop the player sticking to walls.
	Vector3 fullSize = new Vector3(0.55f, 0.9f, 2); // the original size of the player
	Vector3 halfSize; // half the player's the original size.
	AudioSource scaleDown, scaleUp, gravFlip, gravNormal, jump, land; //holds audio clips attached to the player.
    GameObject previousObject;  // the last object that the player collided with.
    public GameObject roof, floor, startPortal, endPortal; //holds the roof, floor, start portal, and end portal game objects.
	Queue<GameObject> inactivePowerUps; // stores inactive GameObjects.
	public int powerUpRespawn = 2; //respawn time for the power ups.
    bool gravityFlipped; //used to see if the gravity is currently flipped or not.
    bool scaled; // used to see if the player has been resize.
	GameObject gameController; //used to find the game controller object.
	GameController script; //used to get the game controller script from the object.
	public GameObject saveText; //used to hold the save text to display above the player.
	bool justTeleported; //used to hold whether or not the player has already teleported. 
    #endregion
    
    // Use this for initialization
    void Start()
    {   
        //Screen.showCursor = false; //hide the users mouse.
        inactivePowerUps = new Queue<GameObject>(); //creates the queue. 
        halfSize = new Vector3(fullSize.x / 2, fullSize.y / 2, 2); //calculates half of the player size.
	    
		#region loading
		AudioSource[] sounds = this.GetComponents<AudioSource>(); //store all the sounds attached to the player in an array.
		scaleDown = sounds[0]; //the first sound in the array holds the scale down sound.
		scaleUp =  sounds[1]; //the second sound in the array holds the scale up sound.
		gravFlip =  sounds[2]; //the third sound in the array holds the gravity flip sound.
		gravNormal = sounds[3]; //the fourth sound in the array holds the gravity revert sound.
		land = sounds[4]; //the sixth sound in the array holds the landing sound.
		jump = sounds[5]; //the fith sound in the array holds the jumping sound.

		gameController = GameObject.Find("GameController"); //find the game controller object so we can access its script.
		script = gameController.GetComponent<GameController>(); //holds the script attached to the game controller object.
		this.GetComponent<Renderer>().material.color = script.playerColor; //set the color of the player to that stored in the game controller.
		this.transform.position =  script.savedPosition; //set the players position to that stored in the game controller.
		Camera.main.transform.position =  script.cameraPosition; //set the camera position to that stored in the game controller.
		Camera.main.backgroundColor = Color.white; //set the camera background color to white at the beginning of the game.
		#endregion
    } 
    
    // Update is called once per frame
    void Update()
    {
        PlayerMovement(); //call the player movement method
		PlayerAbilities(); //call the player abilities method
        CameraFollow(); //call the camera follow method.
	
		if (Input.GetKeyDown(KeyCode.Escape)) //if the player presses the escape key.
        { 
            Physics.gravity = new Vector3(0, -13, 0); //reset the gravity to its default value.
            Application.LoadLevel("Menu"); //return to the main menu.
        }
    }

    //deals with player movement/controls.
    void PlayerMovement()
    {
        #region User Input
		if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))) //if the player is holding A & D at the same time.
        {  
            GetComponent<Collider>().sharedMaterial = null; //then give the player the friction material to stop sliding.
        } 
		else if (Input.GetKey(KeyCode.A)) //if the player is holding the A key down. 
		{ 
            GetComponent<Rigidbody>().AddForce(new Vector3(-acceleration, GetComponent<Rigidbody>().velocity.y, 0)); //move them at a constantly increasing speed to the left.
            GetComponent<Collider>().sharedMaterial = noFriction; //set the physic material of the player to one with no friction.
        } 
		else if (Input.GetKey(KeyCode.D)) //if the player is holding the D key down. 
        { 
			GetComponent<Rigidbody>().AddForce(new Vector3(acceleration, GetComponent<Rigidbody>().velocity.y, 0)); //move them at a constantly increasing speed to the right.
            GetComponent<Collider>().sharedMaterial = noFriction; //set the physic material of the player to one with no friction.
        }

		if (Input.GetKeyUp(KeyCode.A)) //when the player releases the A key. 
        { 
			if (GetComponent<Rigidbody>().velocity.x < -maxSpeed / 2) //if they are moving above a certain speed. 
            { 
				GetComponent<Rigidbody>().AddForce(new Vector3(decceleration, GetComponent<Rigidbody>().velocity.y, 0)); //then push them slightly in the opposite direction to decrease the slide duration.
            }
            GetComponent<Collider>().sharedMaterial = null; //give the player friction again when they stop moving to prevent sliding.
        } 
		else if (Input.GetKeyUp(KeyCode.D)) //when the player releases the D key. 
        {
			if (GetComponent<Rigidbody>().velocity.x > (maxSpeed / 2)) //if they are moving above a certain speed. 
            { 
				GetComponent<Rigidbody>().AddForce(new Vector3(-decceleration, GetComponent<Rigidbody>().velocity.y, 0)); //then push them slightly in the opposite direction to decrease the slide duration.
            }
            GetComponent<Collider>().sharedMaterial = null; //give the player friction again when they stop moving to prevent sliding.
        }
        
		if (Input.GetKeyDown(KeyCode.W)) //if the player presses the W key.
        {  
			if (!jumpActive) //if they are not already jumping.
            {  
				jump.Play();
                GetComponent<Rigidbody>().velocity += new Vector3(0, jumpHeight, 0) * Vector3.up.y; //adds a force upwards to the player.
                jumpActive = true; //set the jump to active so they cannot jump again.
            }
        }
        #endregion

        #region Velocity Control
		if (jumpActive) //if the player is currently jumping.
        {  
			if (GetComponent<Rigidbody>().velocity.x > maxSpeed / jumpSpeedModifier) //if the players velocity is above the maximum movement speed while jumping.
            { 
                GetComponent<Rigidbody>().velocity = new Vector3(maxSpeed / jumpSpeedModifier, GetComponent<Rigidbody>().velocity.y, 0); //move the player velocity back to the maximum speed allowed while jumping.
            } 
			else if (GetComponent<Rigidbody>().velocity.x < -maxSpeed / jumpSpeedModifier) //if the players velocity is above the maximum movement speed while jumping. 
            {  
                GetComponent<Rigidbody>().velocity = new Vector3(-maxSpeed / jumpSpeedModifier, GetComponent<Rigidbody>().velocity.y, 0); //move the player velocity back to the minimum speed allowed while jumping.. 
            }
        } 
		else // else the jump is NOT currently active 
        {
			if (GetComponent<Rigidbody>().velocity.x > maxSpeed) //if the player is moving above the maximum speed to the right. 
            { 
                GetComponent<Rigidbody>().velocity = new Vector3(maxSpeed, GetComponent<Rigidbody>().velocity.y, 0); //then move them back to their maximum speed.
            } 
			else if (GetComponent<Rigidbody>().velocity.x < -maxSpeed) //if the player is moving above the maximum speed to the left.  
            { 
                GetComponent<Rigidbody>().velocity = new Vector3(-maxSpeed, GetComponent<Rigidbody>().velocity.y, 0); //then move them back to their maximum speed.
            }   
        }
        
		if (gravityFlipped) //if gravity is currently flipped
        { 
			if (GetComponent<Rigidbody>().velocity.y < maxJumpVelocity) //if the player velocity in the Y direction is greater than the maximum allowed. 
            { 
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, maxJumpVelocity, 0); //then decrease their velocity to the allowed value.
            }
        } 
		else //if gravity is not currently flipped.
        {  
			if (GetComponent<Rigidbody>().velocity.y > maxJumpVelocity) //if the players velocity in the Y direction is greater than the maximum allowed.
            {  
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, maxJumpVelocity, 0); //then decrease their velocity to the allowed value.
            }
        }
        #endregion
    }

	//deals with player abilities.
	void PlayerAbilities()
	{
		#region DeleteBlocks
		if(Input.GetMouseButton(0)) //if the player presses the left mouse button.
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //holds a raycast that can be fired into physics space.
			RaycastHit hit; //holds the position of the object hit.
			if (Physics.Raycast(ray, out hit, 100)) //if the raycast has hit an object.
			{
				Destroy(hit.collider.gameObject); //then destroy that game object.
			}
		}
		#endregion DeleteBlocks
	}
        
    // Used to make the camera follow the player as they go through rooms.
    void CameraFollow()
    {
		if (transform.position.x > Camera.main.transform.position.x + 9.1f) //if the player has went outside of the cameras view to the right. 
        { 
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + 18.2f, 0, Camera.main.transform.position.z); //then move the camera to the right.
        }
		else if (transform.position.x < Camera.main.transform.position.x - 9.1f) //if the player has went outside of the cameras view to the left. 
        { 
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - 18.2f, 0, Camera.main.transform.position.z); //then move the camera to the left.
        }
    }
        
    // deals with collisions with other game objects.
    void OnCollisionEnter(Collision other)
	{ 
        switch (other.gameObject.name)
        {
            case "Wall":
                ContactPoint contact = other.contacts [0]; //get point that the player has collided with.
				if (other.gameObject != previousObject) // as long as the current object isn't the one we collided with last time.
                {     
					if (contact.point.x < transform.position.x) //if the wall is to the left of the player
                    { 
                        GetComponent<Rigidbody>().AddForce(new Vector3(300, jumpHeight, 0)); //then push the player to the right and upwards.
                    } 
					else // else the wall is to the right of the player. 
                    { 
                        GetComponent<Rigidbody>().AddForce(new Vector3(-300, jumpHeight, 0)); //then push the player to the left and upwards.
                    }
					jumpActive = true; 
                }
				break; 

			case "Floor" :
			case "Platform":
				land.Play(); //play the landing sound.
				GetComponent<Rigidbody>().velocity =  new Vector3(GetComponent<Rigidbody>().velocity.x, 0 , 0); //stops the player from bouncing when they collide with the ground or a platform.
				jumpActive = false; //set the jump to inactive.
				break;

			case "Enemy":
				transform.position =  script.savedPosition; //set the players position to that stored in the game controller
				GetComponent<Rigidbody>().velocity = Vector3.zero; //set the velocity to zero.
				break;
        }

        previousObject = other.gameObject; //store the object we just collided with. 
    }

    // Used when the player enters a trigger.
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
			case "GravityBox": 
				if(gravityFlipped) //if the gravity is currently flipped.
				{ 
					gravNormal.Play(); //play the gravity reverting sound.
				} 
				else //if gravity is not currently flipped.
				{ 
					gravFlip.Play(); //play the gravity flip sound.
				}

                // switch the names of the floor and the roof 
                string temp = roof.name; 
                roof.name = floor.name; 
                floor.name = temp; 

                gravityFlipped ^= true; // invert gravity
                Physics.gravity = new Vector3(0, Physics.gravity.y * -1, 0); //flip the value of gravity to its inverse.
                jumpHeight *= -1; //flip the jump direction. 
                maxJumpVelocity *= -1; //flip the maximum jump velocity.
                other.gameObject.SetActive(false); // deactive the object
                inactivePowerUps.Enqueue(other.gameObject); // put the object in the queue
                Invoke("respawnPowerUp", powerUpRespawn); // reactivate the object in 3 seconds
            	break; 

            case "ScaleBox":
				if(scaled) //if the player is scaled.
				{ 
					scaleUp.Play(); //play the scale up sound.
				} 
				else //if they are not currently scaled.
				{ 
					scaleDown.Play(); //play the scale down sound/
				}

				transform.localScale = scaled ? fullSize : halfSize; // if player is has been resized, then set to full size. Otherwise set player to half size. 
                scaled ^= true; // invert scaled
                other.gameObject.SetActive(false);  // deactive the object
                inactivePowerUps.Enqueue(other.gameObject); // put the object in the inactive queue
                Invoke("respawnPowerUp", powerUpRespawn); // reactivate the object in 3 seconds
            	break; 

			case "SaveCube":
				Destroy(other.gameObject); //delete the save cube.
				saveText.SetActive(true); //display the saved text message.
				Invoke("deactivateSaveText", 1.5f); // deactivate the save text in 1.5 seconds
				script.SaveGame(this.transform.position,Camera.main.transform.position); //call the save game method from the game controller object and pass in the relevant variables.
				break;

			case "Button":
				Destroy(other.gameObject); //delete the button object.
				Camera.main.backgroundColor = (Camera.main.backgroundColor == Color.white) ? Color.black : Color.white; // if the camera's back color is currently white, set it to black. Otherwise, set it to white. 
				break;

			case "StartPortal":
				if (justTeleported) // if the player just teleported from another portal
				{ 
					justTeleported = false; //set just teleported to false.
				} 
				else // else the player has not just teleported from a portal
				{ 
					justTeleported = true; //set just teleported to true. 
					Transform portal = other.gameObject.transform.GetChild(0); // get the transform of the exit portal attached to this start portal.
					GetComponent<Rigidbody>().transform.position = portal.transform.position; //move the player to the position of the exit portal.
				}
				break;

			case "EndPortal":
				if (justTeleported) //if the player has just teleported from another portal.
				{
					justTeleported = false; //set just teleported to false.
				} 
				else // else the player has not just teleported from another portal
				{ 
					justTeleported = true; //set just teleported to true.
					GetComponent<Rigidbody>().transform.position =  other.gameObject.transform.parent.position; //move the player to the position of the start/parent portal.
					
				}
				break;
        }
    }

	// Deactivates the "Saved" text that appears above the player's body. 
	void deactivateSaveText() 
	{
		saveText.SetActive(false); // don't display the "Saved" message
	}

    // activates and removes the first powerUp in the inactive queue. 
    void respawnPowerUp()
    {
		if (inactivePowerUps.Count > 0) // if inactiveObjects isn't empty
        {  
            inactivePowerUps.Dequeue().SetActive(true); // remove the first powerUp in the queue & activate it
        }
    }
}

