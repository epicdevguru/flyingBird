using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config {

	//bird's horizontal speed for each second
	public static float fBirdSpeed 	= 6f;
	//the Z axis distance between camera and bird object
	public static float fOffsetZ	= 18f;

	//the basic movement step amount whenever the background mountains moves, or distance each other
	public static float fBGStep 	= 25f;
	//the measure value if the background mountains need to be moved by checking the x axis distance with bird object
	public static float fBGDistanceMeasure = 50f;
	//the update amount in x axis when the background mountains moves right end
	public static float fBGUpdatePositionX = 100f;
	//the background child object counts
	public static int 	BG_CHILD_COUNTER = 4;

	//tolerate range for checking the new wheel object generation, but not used
	public static float fTolerateRange = 3f;
	//the narration phrase change time interval
	public static float fNarrationTimeGap = 2f;
}
