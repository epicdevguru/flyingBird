using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*CLASS: ObstacleContainer
 * Used for managing obstacles -> wheel objects
 * by generating new wheels and destroying old wheel object.
 * */
public class ObstacleContainer : MonoBehaviour {
	
	//transform of bird object
	public Transform transBird;
	//transform of main camera
	public Transform transCam;

	//prefab of the wheel obstacle
	public GameObject prefObstacle;
	//list of obstacles maximum 2 child nodes for optimization of the project
	public List<GameObject> listObstacles;

	// initial scale value of the wheel obstacle when it is generated and belongs to this ObstacleContainer object
	public Vector3 v3ObstacleScale = new Vector3 (2.5f, 2.5f, 1f);
	//static variable of ObstacleContainer
	public static ObstacleContainer obsContainer;


	//Instance object of ObstacleContainer class
	public static ObstacleContainer Instance() {
		if (!obsContainer) {
			obsContainer = FindObjectOfType (typeof(ObstacleContainer)) as ObstacleContainer;
		}
		return obsContainer;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// check each frame and when needed generate new obstacle
		GenerateObstacle ();
	}

	/*FUNTION: GenerateObstacle()
	 * CALLED BY: Update()/ObstacleContainer.cs
	 * Destroy old obstacles and instantiate new obstacles 
	 * */
	public void GenerateObstacle() {
		// if the bird reached the current showing wheel with almost nearest position then it is okay to destry the old ( passed object )
		// obstacle because it is not showing on camera now, or you can even do it on camera movement too.
		if (transBird.position.x < transCam.position.x 
			&& Mathf.Abs (transBird.position.x - transCam.position.x) < Config.fBirdSpeed * Config.fTolerateRange * Time.deltaTime ) {
			if (listObstacles.Count == 2) {
				Destroy (listObstacles[0]); //destroy the old object 
				listObstacles.RemoveAt (0); //remove the destroyed object from the list
			}
		}
		//if the bird pass the current wheel then need to generate new wheel at the far right side
		//so that the camera still not render it. It will be shown when camera moves step right.
		else if (transBird.position.x > transCam.position.x) {
			//check if the old wheel was deleted and still have no new wheel added in list
			if (listObstacles.Count == 1) {
				//instantiate (generate) new wheel obstacle from the prefab
				GameObject newObstacle = Instantiate (prefObstacle) as GameObject;
				//make the new wheels parent object into this obstacleContainer object
				newObstacle.transform.parent = transform;
				//randomize the wheel X axis position a little in center of the scene
				float fPosX = CalculateRealWorldOffset () + Random.Range (-0.2f, 0.5f);
				//randomize the wheel's Y axis position
				float fPosY = Random.Range (-8.5f, -5f);
				// adjust the local position and scale, add in the list
				newObstacle.transform.localPosition = new Vector3 (listObstacles [0].transform.localPosition.x + fPosX, fPosY, 0);
				newObstacle.transform.localScale = v3ObstacleScale;
				listObstacles.Add (newObstacle);
			}
		}
	}

	/*FUNCTION: GenerateObstacle4Bird()
	 * CALLED BY:  OnTriggerEnter2D(Collider2D collider)/BirdController.cs
	 * not used in current logic, was usable if we need some way point check functionality
	 * but not for this project
	 * */
	public void GenerateObstacle4Bird() {
		if (listObstacles.Count == 2) {
			Destroy (listObstacles[0]);
			listObstacles.RemoveAt (0);
		}
		if (listObstacles.Count == 1) {
			GameObject newObstacle = Instantiate (prefObstacle) as GameObject;
			newObstacle.transform.parent = transform;
			float fPosX = CalculateRealWorldOffset ();
			float fPosY = Random.Range (-1f, 2.5f);

			newObstacle.transform.localPosition = new Vector3 (listObstacles [0].transform.localPosition.x + fPosX, fPosY, 0);
			newObstacle.transform.localScale = v3ObstacleScale;
			listObstacles.Add (newObstacle);
		}

	}

	/*FUNCTION: CalculateRealWorldOffset()
	 * CALLED BY: GenerateObstacle(), GenerateObstacle4Bird() /ObstacleContainer.cs
	 * used for calcuate the matching world position distance from left to right corner of the screen
	 * so means that how much distance in world coordinate the screen width is ...
	 * used for adding new obstacle wheel and locating it far right side of the current scene
	 * */
	public float CalculateRealWorldOffset() {
		Vector3 leftBottom = transCam.gameObject.GetComponent<Camera> ().ScreenToWorldPoint (new Vector3(0, 0, 0));
		Vector3 rightBottom = transCam.gameObject.GetComponent<Camera> ().ScreenToWorldPoint (new Vector3(Screen.width * 1f, 0 , 0));
		float fRealDistanceX = rightBottom.x - leftBottom.x;
		return fRealDistanceX;
	}

}
