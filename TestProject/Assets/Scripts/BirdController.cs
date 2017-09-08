using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour {
	//新しい　ブロジェト　の　main controller 　
	public Animator aniBird;		//bird animation controller - > animator added on bird object
	public Vector3 birdSpawnPos;	//spawn position of the bird object, it is needed at starting up
									//calculated by converting left center position of the screen

	/*---------------------------------------------------------------------
	 * Parameters for randomized updown movement of the bird
	 * If the bird flies straight from left to right, then it is less interesting
	 * and have no sense as a game developer, so I decided to make the bird 
	 * move up and down while moving from left to right.
	 * fIntervalUpDown: it is a counter of the time for randomized interval limit
	 * 					It is compared with fRandomDurationUpDown which is a random
	 * 					time limiter.
	 * fRadomDurationUpDown: Ramdomized time limiter for updown direction of bird
	 * 						It is updated when fIntervalUpDown hits fRandomDuration.
	 * fRandomUpDown: random amount of up or down ward of the bird
	 * 
	 * The bird will move up or down into fRandomUpDown amount in 
	 * fRandomDurationUpDown time duration, then updated into new param values.
	 * --------------------------------------------------------------------*/
	public float fIntervalUpDown;	
	public float fRandomDurationUpDown;
	public float fRandomUpDown;
	//-----------------------------------------------------------------------


	public Camera mainCam;							// main camera of the project which renders all objects
	public static BirdController birdController;	// static object of this class so that other classes can access its instance value
	public AudioSource audioBump;					//audio clip for bird bump with wheel sound.

	//instance function for static BirdController
	public static BirdController Instance() {
		if (birdController) {
			birdController = FindObjectOfType (typeof(BirdController)) as BirdController;
		}
		return birdController;
	}

	// Use this for initialization
	void Start () {
		//Initialize bird position by spawning it at the left center of the screen
		InitBirdPosition ();
		//set the random parameters for bird's up/down random movement
		SetRandomDirectionParams ();
	}


	
	// Update is called once per frame
	void Update () {
		//handle the bird flying by checking random param updates and camera movement needs
		FlyBird ();
	}

	/*FUNCTION: FlyBird()
	 * CALLED BY: Update() / BirdController.cs
	 * used for updating camera position when the bird out of screen
	 * as well as reset the random updown movement of the bird
	 * plus the movement of horizontal fly of bird from left to right
	 * */
	void FlyBird() {
		if (fIntervalUpDown < fRandomDurationUpDown) {
			UpdateBirdPosition ();  // update the current frame's bird position
			// if bird is out of screen, move the camera position
			if (CheckScreenOut ()) { 
				ResetCameraPosition ();
			}
			// update fIntervalUpDown parameter in sec unit
			fIntervalUpDown += Time.deltaTime;
		} else {
			// if fIntervalUpDown param hit the fRandomDurationUpDown
			// reset the randomize parameters for updown movement
			SetRandomDirectionParams ();
		}
	}

	/*FUNCTION: InitBirdPosition()
	 * CALLED BY: Start()/BirdController.cs
	 * used for initialize the bird game object position into left center screen
	 * It needs convert screen 2d position into game's 3d world position
	 * Why it is needed? 
	 * There are lots of devices with different screen resolution so we need to 
	 * get the spawn position dynamically.
	 * */
	void InitBirdPosition() {
		float fInitialScreenX = 0; //left screen
		float fInitialScreenY = Screen.height / 2f; //center y position
		birdSpawnPos = mainCam.ScreenToWorldPoint (new Vector3(fInitialScreenX, fInitialScreenY, Config.fOffsetZ)); 
		this.gameObject.transform.position = birdSpawnPos;
	}

	/*FUNCTION: SetRandomDirectionParams()
	 * USED BY: FlyBird()/BirdController.cs
	 * used by resetting the fIntervalUpDown, 
	 * fRandomDurationUpDown, fRandomUpDown
	 * */
	void SetRandomDirectionParams() {
		fIntervalUpDown = 0;
		fRandomDurationUpDown = Random.Range (2f, 5f);
		fRandomUpDown = Random.Range (-6f, 1f);
	}

	/*FUNCTION: CheckScreenOut()
	 * USED BY: FlyBird()/BIrdController.cs
	 * Check if the bird position is out of the camera view
	 */
	bool CheckScreenOut() {
		Vector3 screenPos = mainCam.WorldToScreenPoint (gameObject.transform.position);
		if (screenPos.x > Screen.width * 1f) {
			return true;
		}
		return false;
	}


	/*FUNCTION: ResetCameraPosition()
	 * CALLED BY: FlyBird()/BirdController.cs
	 * Whenever the bird out of camera view, update the camera position
	 */
	void ResetCameraPosition() {
		float fChangeCamPos = transform.position.x - mainCam.gameObject.transform.position.x;
		mainCam.gameObject.transform.position += Vector3.right * fChangeCamPos * 2f;
		BackgroundManager.Instance ().MoveBGMountains ();
	}

	/*FUNCTION: UpdateBirdPosition()
	 * CALLED BY: FlyBird()/BirdController.cs
	 * update the bird position every frame
	 */
	void UpdateBirdPosition () {
		float fDeltaPosX = Config.fBirdSpeed * Time.deltaTime;
		float fDeltaPosY = Mathf.Lerp (transform.position.y, fRandomUpDown, fIntervalUpDown/fRandomDurationUpDown);
		Vector3 v3NewPos = transform.position + new Vector3 (fDeltaPosX, 0, 0);
		v3NewPos.y = fDeltaPosY;
		transform.position = v3NewPos;
	}

	/*FUNCTION: BumpWithWheel()
	 * CALLED BY: OnTriggerEnter2D(Collider2D collider)/BirdController.cs
	 * Whenever the bird collides with Wheel then it is called and 
	 * make the bird rotate and play the bump sound.
	 * */
	void BumpWithWheel() {
		if (aniBird.GetCurrentAnimatorStateInfo (0).IsName ("birdFly")) {
			aniBird.SetTrigger ("BumpWheel");
			audioBump.Play ();
		}
	}

	/*FUNCTION: OnTriggerEnter2D(Collider2D collider)
	 * CALLED BY: Trigger event from the collider added on bird object
	 * */
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Wheel") {
			BumpWithWheel ();
		} else if (collider.tag == "WheelCheckpoint") {
			ObstacleContainer.Instance ().GenerateObstacle4Bird ();
		}
	}

}
