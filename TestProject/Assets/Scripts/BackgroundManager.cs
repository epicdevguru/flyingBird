using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*CLASS: BackgroundManager
 * used for changing background by moving passed background 
 * object into the right end of the objects
 * so that the background looks change seemlessly
 */

public class BackgroundManager : MonoBehaviour {
	// static variable for BackgroundManager
	public static BackgroundManager bgManager;
	// transform of bird object
	public Transform transBird;
	//child objects of BackgroundManager and currently 4 objects in test project
	public GameObject[] bgMountains;
	//used to determine which mountain object should be moved whenever needed
	public int nMoveCounter = 0;

	//focusedObject = next moving bg mountain object
	Transform focusedBGObj;

	//Instance object of the BackgroundManager class
	public static BackgroundManager Instance() {
		if (!bgManager) {
			bgManager = FindObjectOfType (typeof (BackgroundManager)) as BackgroundManager;
		}
		return bgManager;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//seemlessly update the background mountains
		MoveBGMountains ();
	}

	/*FUNCTION: MoveBGMountains()
	 * CALLED BY: Update()/BackgroundManager.cs
	 * Brings left most mountain object into right most whenever the bird pass it out.
	 * */
	public void MoveBGMountains() {
		focusedBGObj = bgMountains [nMoveCounter].transform;
		if (transBird.position.x - focusedBGObj.position.x > Config.fBGDistanceMeasure) {
			focusedBGObj.position += Vector3.right * Config.fBGUpdatePositionX;
			nMoveCounter++;
			nMoveCounter = nMoveCounter % Config.BG_CHILD_COUNTER;
		}
	}
}
