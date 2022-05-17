using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Startup : MonoBehaviour
{
    private static bool initialized;
    public static GameObject mainMover;
    public static GameObject charMover;
    public static GameObject levMover;
    public static GameObject optMover;
    public static GameObject tutorialMover;

    public static Vector3 screenPos = new Vector3(440, 300, 0);
    public static Vector3 offScreen = new Vector3(2500, 300, 0);

    void Start()
    {
        if(!initialized) {
 	GameObject mainMenu = Instantiate (Resources.Load ("MainMenuCanvas")) as GameObject;
        mainMover = mainMenu.transform.Find("Mover").gameObject;
	mainMover.transform.position = screenPos;
        GameObject charSelect = Instantiate (Resources.Load ("CharSelectCanvas")) as GameObject;
        charMover = charSelect.transform.Find("Mover").gameObject;
	charMover.transform.position = offScreen;
     //   GameObject levSelect = Instantiate (Resources.Load ("LevCanvas")) as GameObject;
     //   levMover = lev.transform.Find("Mover").gameObject;
     //   levMover.transform.position = offScreen;
        GameObject opt = Instantiate (Resources.Load ("OptionsCanvas")) as GameObject;
        optMover = opt.transform.Find("Mover").gameObject;
	optMover.transform.position = offScreen;
     //   GameObject tutorial = Instantiate (Resources.Load ("TutorialCanvas")) as GameObject;
     //   tutorialMover = tutorial.transform.Find("Mover").gameObject;
     //   tutorialMover.transform.position = offScreen;
        initialized = true;	
}
    }

    void Update()
    {
        
    }

    
}
