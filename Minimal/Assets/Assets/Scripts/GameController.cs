using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic; 

public class GameController : MonoBehaviour
{
	#region Variables
	public Vector3 savedPosition = new Vector3(0,-1,0); //used to hold the players most recent save position. Set to spawn at default. 
	public Vector3 cameraPosition = new Vector3(0,0,-4); //used to hold the most recent saved camera posiiton. Set to spawn at default.
	string error; //holds the error message if the save file cant be found.
	public Color playerColor; //holds the color the player has selected.
	public bool sound = true; //holds he bool to toggle game sound on/off.
	public float cutoutX = 0.0f; //holds the last position of the color cutour object.
	public Dictionary<string, Color> colorMap = new Dictionary<string, Color> () //dictionary that holds all the possible color choices.
	{ 
		{"Orange", new Color32(255, 58, 0, 255)},
		{"Blue", new Color32(0, 154, 255, 255)},
		{"Purple", new Color32(158,84,255,255)},
		{"Green", new Color32(51,233,51,255)},
		{"Pink", new Color32(255,102,178,255)}
	};
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		if(File.Exists(Application.dataPath + "/SaveFile.txt")) //if the save file has been found.
		{
			string[] allLines = File.ReadAllLines (Application.dataPath + "/SaveFile.txt"); //read and store all lines from the file in a string array.
			colorMap.TryGetValue(allLines[1], out playerColor); //set the player color to the one stored in the save file.
			savedPosition = new Vector3 (float.Parse(allLines[3]), float.Parse(allLines[5]),0); //load in the players last saved position from the save file.
			cameraPosition = new Vector3(float.Parse(allLines[7]), 0, -4); //load in the last saved camera position from the save file.
			sound =  bool.Parse(allLines[9]); //load in whether the sound was previously turned on or off.
			cutoutX =  float.Parse(allLines[11]); //load in the last position of the selected color cutout.
		}
		else //if the file canno't be found.
		{
			error = "CANT FIND SAVE FILE! PLACE IT IN THE CORRECT FOLDER"; //display the error message on screen.
		}
		
		DontDestroyOnLoad (this); //don't destroy this object when switching between scenes.
		Application.LoadLevel ("Menu"); //load the main menu scene. 
	}
	
	// Update is called once per frame
	void Update ()
	{
		Camera.main.aspect = 16f / 9f; //make sure the camera resolution is set to 16 by 9 so all of the content shows.
		AudioListener.volume = (sound) ? 1 : 0; // volume = 1 if sound is enabled. Otherwise, volume = 0
	}
	
	//used to save various game settings when the player enters a save box.
	public void SaveGame(Vector3 playerPos, Vector3 cameraPos)
	{
		savedPosition = playerPos; //store the saved position in the relevant variable from this script.
		cameraPosition =  cameraPos; //store the camera position in the relevant variable from this script.
		string[] allLines = File.ReadAllLines (Application.dataPath +"/SaveFile.txt"); //read the current saved file so we can grab the players color selection.
		File.WriteAllText(Application.dataPath + "/SaveFile.txt", "player color:\n" + allLines[1] + "\nplayer x:\n" + savedPosition.x + "\nplayer y:\n" + savedPosition.y + "\ncamera x:\n" + cameraPos.x + "\nsound:\n"+sound +"\ncutout x:\n"+cutoutX); //pass in the new positon variables to the save file.
	}
	
	//used to set the player color in the save file.
	public void ChangeColor(String selectedColor, float cutout)
	{
		colorMap.TryGetValue(selectedColor, out playerColor); //set the color to the value passed in from the menu script.
		cutoutX = cutout; //stores the cutout x position passed in.
		File.WriteAllText(Application.dataPath + "/SaveFile.txt", "player color:\n" + selectedColor + "\nplayer x:\n" + savedPosition.x + "\nplayer y:\n" + savedPosition.y + "\ncamera x:\n" + cameraPosition.x + "\nsound:\n"+sound+"\ncutout x:\n"+cutoutX); //change the color stored in the save file.
	}

	//used to set the sound on/off varibale in the save file.
	public void ToggleSound(bool choice)
	{
		sound = choice; //sets the sound option to whatever the player has chosen.
		string[] allLines = File.ReadAllLines (Application.dataPath +"/SaveFile.txt"); //read the current saved file so we can grab the players color selection.
		File.WriteAllText(Application.dataPath + "/SaveFile.txt", "player color:\n" + allLines[1] + "\nplayer x:\n" + savedPosition.x + "\nplayer y:\n" + savedPosition.y + "\ncamera x:\n" + cameraPosition.x + "\nsound:\n"+sound+"\ncutout x:\n"+cutoutX); //pass in the new sound variable to the save file.
	}

	//used to reset the game data back to default
	public void Reset()
	{
		colorMap.TryGetValue("Orange", out playerColor); //set the color to orange.
		savedPosition= new Vector3(0,-1,0); //move the player back to their starting position.
		cameraPosition= new Vector3(0,0,-4); //move the camera back to its starting position.
		sound=true; //turn the sound on.
		cutoutX=13.8f; //move the selected color cutout to appear above the orange box.
		File.WriteAllText(Application.dataPath + "/SaveFile.txt", "player color:\nOrange" + "\nplayer x:\n" + savedPosition.x + "\nplayer y:\n" + savedPosition.y + "\ncamera x:\n" + cameraPosition.x + "\nsound:\n"+sound +"\ncutout x:\n"+cutoutX); //reset all of the information in the save file.
	}

	void OnGUI()
	{
		GUI.color = Color.red; //set the gui text color to red
		GUI.Label(new Rect(30,30,600,100), error); //displays the error message on screen.
	}
}
