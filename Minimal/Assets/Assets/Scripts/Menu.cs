using UnityEngine;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
	#region Variables
	public GameObject play, gameMode, options, credits, quit, on, off, reset ,back, taco, cksned; //holds the objects used to display text on screen.
	public GameObject orange, blue, purple, green, pink, cutout; //holds the objects used for color selection.
	ButtonObj orangeButton, blueButton , greenButton , purpleButton , pinkButton, onButton,  offButton, resetButton ,backButton, playButton, gameModesButton, optionsButton, creditsButton, tacoButton, cksnedButton ,quitButton; // declare all buttons in the GUI
	HashSet<ButtonObj> colorButtonSet; // stores all ButtonObjects responsible for a player's color.
	HashSet<ButtonObj> navButtonSet;  // stores all ButtonObjects responsible for GUI navigation. 
	Vector2 mousePos; //holds the position of the player mouse.
	SpriteRenderer renderer; //sprite renderer used to change text colors.
	GameObject gameController; //used to find the game controller object.
	GameController script; //used to get the game controller script from the object.
	int menuRoom = 0; //int to switch between GUI rooms and buttons.
	float cutoutPos; //the position of the cutout object.
	#endregion

	// Use this for initialization
	void Start ()
	{
		Cursor.visible = true; //show the users mouse.
		gameController = GameObject.Find("GameController"); //find the game controller object so we can access its script.
		script = gameController.GetComponent<GameController>(); //holds the script attached to the game controller object.
		cutoutPos = script.cutoutX; //get the position of the cutout from the save file.
		cutout.transform.position = new Vector3 (cutoutPos,cutout.transform.position.y,-1); //set the position of the cutout game object.

		#region initialize buttons
		// navigation buttons
		playButton = new ButtonObj(play, "Play");
		gameModesButton = new ButtonObj(gameMode, "Game Modes"); 
		optionsButton = new ButtonObj(options, "Options"); 
		creditsButton = new ButtonObj(credits, "Credits"); 
		quitButton = new ButtonObj(quit, "Quit"); 
		resetButton = new ButtonObj(reset,"Reset");
		backButton = new ButtonObj(back, "Back"); 

		// color buttons
		orangeButton = new ButtonObj(orange, "Orange");
		blueButton = new ButtonObj(blue, "Blue");
		greenButton = new ButtonObj(green, "Green");
		purpleButton = new ButtonObj(purple, "Purple");
		pinkButton = new ButtonObj(pink, "Pink"); 

		// sound buttons
		onButton = new ButtonObj(on, "On"); 
		offButton = new ButtonObj(off, "Off"); 

		//credits buttons
		tacoButton = new ButtonObj(taco,"BrokenTaco");
		cksnedButton = new ButtonObj(cksned,"CkSned");
	
		// initialize button sets 
		navButtonSet = new HashSet<ButtonObj>() {playButton, gameModesButton, optionsButton, creditsButton, quitButton, resetButton , backButton, tacoButton, cksnedButton}; // adds all nav buttons to this set
		colorButtonSet = new HashSet<ButtonObj> () {orangeButton, pinkButton, greenButton, purpleButton, blueButton};  // /adds all color buttons to this set
		#endregion
	}
	
	// Update is called once per frame
	void Update ()
	{
		mousePos = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y); //tracks the user mouse position

		#region set buttons' rectangles
		playButton.setRect(new Rect (Screen.width / 2 - Screen.width / 20, Screen.height / 2 - Screen.height / 7, Screen.width / 10, Screen.height / 12)); //the rectangle settings for the play button.
		gameModesButton.setRect(new Rect (Screen.width / 2 - Screen.width / 6.5f, Screen.height / 2 - Screen.height / 20, Screen.width / 3.2f, Screen.height / 12)); //the rectangle settings for the game modes button.
		optionsButton.setRect(new Rect (Screen.width / 2 - Screen.width / 11, Screen.height / 2 + Screen.height / 17, Screen.width / 5.5f, Screen.height / 12)); //the rectangle settings for the options button.
		creditsButton.setRect(new Rect (Screen.width / 2 - Screen.width / 12, Screen.height / 2 + Screen.height / 6, Screen.width / 6, Screen.height / 12)); //the rectangle settings for the credits button.
		quitButton.setRect(new Rect (Screen.width / 2 - Screen.width / 18, Screen.height / 2 + Screen.height / 3.9f, Screen.width / 9, Screen.height / 12)); //the rectangle settings for the quit button.

		orangeButton.setRect(new Rect (Screen.width / 2 - Screen.width / 3.6f, Screen.height / 2 - Screen.height / 3.65f, Screen.width / 16, Screen.height / 9.5f)); //the rectangle settings for the orange color button.
		blueButton.setRect(new Rect (Screen.width / 2 - Screen.width / 6.5f, Screen.height / 2 - Screen.height / 3.65f, Screen.width / 16, Screen.height / 9.5f)); //the rectangle settings for the blue color button.
		purpleButton.setRect(new Rect(Screen.width / 2- Screen.width/32, Screen.height / 2 - Screen.height / 3.65f, Screen.width / 16, Screen.height / 9.5f)); //the rectangle settings for the purple color button.
		greenButton.setRect(new Rect (Screen.width / 2 + Screen.width / 10.7f, Screen.height / 2 - Screen.height / 3.65f, Screen.width / 16, Screen.height / 9.5f)); //the rectangle settings for the green color button.
		pinkButton.setRect(new Rect (Screen.width / 2 + Screen.width / 4.6f, Screen.height / 2 - Screen.height / 3.65f, Screen.width / 16, Screen.height / 9.5f)); //the rectangle settings for the pink color button.

		onButton.setRect(new Rect (Screen.width / 2 - Screen.width/120, Screen.height / 2 - Screen.height/19, Screen.width / 11, Screen.height / 9.5f)); //the rectangle settings for the sound on button.
		offButton.setRect(new Rect(Screen.width / 2 + Screen.width/7.8f, Screen.height / 2 - Screen.height /19, Screen.width / 10, Screen.height / 9.5f)); //the rectangle settings for the sound off button.
		resetButton.setRect(new Rect(Screen.width / 2 - Screen.width/3.7f, Screen.height / 2 + Screen.height / 7, Screen.width / 1.9f, Screen.height / 9.5f)); //the rectangle settings for the reset button.
		backButton.setRect(new Rect (Screen.width / 2 -  Screen.width/15 , Screen.height / 2 + Screen.height / 2.85f, Screen.width / 7.5f, Screen.height / 9.5f)); //the rectangle settings for the back onbutton.

		tacoButton.setRect(new Rect(Screen.width / 2 - Screen.width/2.1f, Screen.height / 2 + Screen.height / 7, Screen.width /2.7f, Screen.height / 9.5f)); //the rectangle settings for the taco twitter button.
		cksnedButton.setRect(new Rect(Screen.width / 2 + Screen.width/8.2f, Screen.height / 2 + Screen.height / 7.2f, Screen.width /4.8f, Screen.height / 9.5f)); //the rectangle settings for the taco twitter button.
		#endregion

		#region highlight text
		foreach (ButtonObj navButton in navButtonSet) //for every button in the navigation button set
		{
			renderer = navButton.getGameObj().GetComponent<SpriteRenderer> (); 
			renderer.color = navButton.getRect().Contains (mousePos) ? script.playerColor : Color.black; // if button is hovered over, set it to the player's color. Otherwise, set it to black.
		}

		if (menuRoom == 1) // if the player is in the options menu
		{ 
			foreach (ButtonObj colorButton in colorButtonSet) //for every pair in the color button set
			{
				renderer = colorButton.getGameObj().GetComponent<SpriteRenderer> (); 
				colorButton.getGameObj().transform.localScale = colorButton.getRect().Contains (mousePos) ? new Vector3(1.3f,1.3f,1) : new Vector3(1,1,1); //if a button is hovered over then scale it up, otherwise make it normal size.
			}
		}

		SpriteRenderer renderer1 = on.GetComponent<SpriteRenderer>(); //get the sprite renderer from the on button.
		SpriteRenderer renderer2 = off.GetComponent<SpriteRenderer>(); //get the sprite rendered from the off button.
		if(script.sound) //if the sound is currently active.
		{
			renderer1.color = script.playerColor; //change the color of the on button to that of the player color.
			renderer2.color = Color.black; //change the color of the off button to black.
		}
		else //if the sound is currently muted.
		{
			renderer1.color = Color.black; //change the color of the on button to black.
			renderer2.color = script.playerColor; //change the color of the off button to that of the player color.
		}
		#endregion
		
	}

	void OnGUI ()
	{
		GUI.color = Color.clear; //set the background color to clear so that the buttons dont show.

		#region MainMenu
		if(menuRoom == 0) //if the GUI is currently in the first room.
		{
			Camera.main.transform.position =  new Vector3 (0,0,-4); //move the camera back to its default position.
			if (GUI.Button (playButton.getRect(), "")) //if the user presses the play button.
			{ 
				GetComponent<AudioSource>().Play (); //play the button click sound.
				Application.LoadLevel ("Level1"); //load the first level of the game.
			} 
			else if (GUI.Button (gameModesButton.getRect(), "")) //if the user presses the game modes button.
			{ 
				GetComponent<AudioSource>().Play (); //play the button click sound.
			} 
			else if (GUI.Button (optionsButton.getRect(), "")) //if the user presses the options modes button.
			{  
				menuRoom = 1; //set the menu room variable to the options room.
				GetComponent<AudioSource>().Play (); //play the button click sound.
			} 
			else if (GUI.Button (creditsButton.getRect(), "")) //if the user presses the credits modes button.
			{  
				GetComponent<AudioSource>().Play (); //play the button click sound.
				menuRoom=2; //set the menu room variable to the credits room.
			} 
			else if (GUI.Button (quitButton.getRect(), ""))  //if the user presses the quit button.
			{
				GetComponent<AudioSource>().Play (); //play the button click sound.
				Application.Quit (); //quit the game.
			}
		}
		#endregion

		#region Options
		else if(menuRoom == 1) //if the GUI is currently in the options room.
		{
			Camera.main.transform.position =  new Vector3 (18.2f,0,-4); //move the camera over to the options room.

			foreach (ButtonObj colorButton in colorButtonSet) 
			{
				if (GUI.Button(colorButton.getRect(), ""))  // if the user presses this color button
				{
					GetComponent<AudioSource>().Play(); //play the button click sound.
					cutout.transform.position = new Vector3 (colorButton.getGameObj().transform.position.x, cutout.transform.position.y, -1); //set the position of the cutout object to that of the orange button.
					script.ChangeColor(colorButton.getString(), cutout.transform.position.x); // change the highlight color of the buttons and place cutout in the correct spot
				}
			}

			if (GUI.Button(onButton.getRect(), "")) //if the user presses the sound on button
			{
				script.ToggleSound(true); //call the toggle sound method from the game controller and set sound to true
				GetComponent<AudioSource>().Play(); //play the button click sound
			}
			else if (GUI.Button(offButton.getRect(), ""))  // if the user presses the sound off button
			{
				script.ToggleSound(false); //call the toggle sound method from the game controller and set sound to false
				GetComponent<AudioSource>().Play(); //play the button click sound
			}
			else if (GUI.Button(resetButton.getRect(), "")) //if the user presses the reset button.
			{
				script.Reset(); //call the reset method in the game controller script.
				cutout.transform.position = new Vector3 (script.cutoutX, cutout.transform.position.y, -1); //set the position of the cutout object to that of the orange button.
				GetComponent<AudioSource>().Play();//play the button click sound.
				menuRoom=0; //move back to the main menu.
			}
		}
		#endregion

		#region Credits
		else if (menuRoom==2) //if the GUI is currently in the credits room.
		{
			Camera.main.transform.position = new Vector3(36.4f,0,-4); //move the camera over to the credits room.

			if(GUI.Button(tacoButton.getRect(),"")) //if the user presses tacos twitter button.
			{
				GetComponent<AudioSource>().Play(); //play the button click sound.
				Application.OpenURL("https://twitter.com/brokentaco09"); //open taco's twitter page.
			}
			else if(GUI.Button(cksnedButton.getRect(),"")) //if the user presses cks twitter button.
			{
				GetComponent<AudioSource>().Play(); //play the button click sound.
				Application.OpenURL("https://twitter.com/cksned"); //open cks twitter page.
			}
		}
		#endregion

		if(menuRoom != 0) //if we are not currently in the main menu room.
		{
			back.transform.position = new Vector3(Camera.main.transform.position.x,back.transform.position.y, back.transform.position.z); //move the back button to the center of the screen.
			if (GUI.Button(backButton.getRect(), "")) //if the user presses the back button.
			{
				GetComponent<AudioSource>().Play(); //play the button click sound.
				menuRoom = 0; //set the menu room back to the main menu.
			}
		}
	}

}
