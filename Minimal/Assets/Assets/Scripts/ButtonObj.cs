using UnityEngine;
using System.Collections;

// Represents a Button to use in a GUI. 
public class ButtonObj {

	/////// Fields //////////////////////////////////////////////////////////
	private Rect rect; // the rectangle setting for this button.
	private GameObject gameObj; // the object used to display text on screen.
	private string str; // string that represents this button



	////// Constructors ///////////////////////////////////////////////////////

	// creates a ButtonObject with the specified rectangle, gameobject, and
	// string. 
	public ButtonObj(Rect theRect, GameObject theObj, string theStr) {
		rect = theRect; 
		gameObj = theObj;
		str = theStr; 
	}

	// Creates a ButtonObject with the specified gameobject and string. The 
	// rectangle is set to its default. 
	public ButtonObj(GameObject theOb, string theStr) {
		rect = new Rect(); 
		gameObj = theOb ; 
		str = theStr;
	}

	// Creates a ButtonObject with the specified string. Both the rectangle
	// and the gameobject are set to their respective defaults. 
	public ButtonObj(string theStr) {
		rect = new Rect(); 
		gameObj = new GameObject() ; 
		str = theStr;
	}



	////// Methods ///////////////////////////////////////////////////////////

	// changes this button's rectangle to the passed rectangle. 
	public void setRect(Rect theRect) {
		rect = theRect; 
	}

	// changes this button's gameobject to the passed gameobject. 
	public void setGameObj(GameObject theObj) {
		gameObj = theObj;
	}

	// changes this button's string to the passed string. 
	public void setString(string theStr) {
		str = theStr;
	}

	// returns this button's rectangle. 
	public Rect getRect() {
		return rect;
	}

	// Returns this button's gameobject.
	public GameObject getGameObj() {
		return gameObj;
	}

	// Returns this button's string. 
	public string getString() {
		return str; 
	}

} // end of ButtonObject
