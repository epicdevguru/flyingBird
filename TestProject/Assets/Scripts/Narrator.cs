using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*CLASS: Narrator
 * used for controlling narration blog
 * It updates phrases in every 2 seconds.
 * */
public class Narrator : MonoBehaviour {
	public Text txt_Narration;
	float fTimeInterval = 0;
	int nCounter = 0;
	public List<string> listNarrations;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateNarration ();
	}

	/*FUNCTION: UpdateNarration()
	 * CALLED BY: Update()/Narrator.cs
	 * */
	void UpdateNarration() {
		//Check if the time interval hits the narration time gap
		//and initialize the fTimeInterval and update the nCounter
		if (fTimeInterval > Config.fNarrationTimeGap) {
			fTimeInterval = 0;
			nCounter++;
		}
		//when ever fTimeInterval is zero then update the phrase in text box
		if (fTimeInterval == 0) {
			int nIndex = nCounter % listNarrations.Count;
			txt_Narration.text = listNarrations[nIndex];
		}
		fTimeInterval += Time.deltaTime;
	}
}
