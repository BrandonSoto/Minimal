using UnityEngine;
using System.Collections;

public class BasicAI : MonoBehaviour {

	public float speed; //the speed the enemy can move at.
	public float range; //the range at which the enemy notices the player.
	public float jumpHeight; //the height that the enemy can jump.
	GameObject target; //the player game object. 
	Vector3 startPos; //the starting position of the enemy.

	// Use this for initialization
	void Start () 
	{
		target = GameObject.Find("Player"); //get the player game object.
		startPos=transform.position; //save the starting position.
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(target.transform.position.x <= transform.position.x + range && target.transform.position.x >= transform.position.x - range) //if the player is within range of the enemy from left to right. 
		{
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y , 0), speed*Time.deltaTime); //move the enemy towards the player at the defined speed.
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.name=="Platform")
		{
			GetComponent<Rigidbody>().velocity += new Vector3(0, jumpHeight, 0) * Vector3.up.y; //adds a force upwards to the player.
		}
		if(other.gameObject.name=="Player")
		{
			transform.position=startPos; //move the enemy back to its staring positon.
			GetComponent<Rigidbody>().velocity = Vector3.zero; //set its velocity to zero.
		}
	}
}
