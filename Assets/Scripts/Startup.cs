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
    void Start()
    {
        if(!initialized) {
 	GameObject mainMenu = Instantiate (Resources.Load ("MainMenuCanvas")) as GameObject;
        mainMover = mainMenu.transform.Find("Mover").gameObject;
	mainMover.transform.position = new Vector3(400, 300, 0);
        GameObject charSelect = Instantiate (Resources.Load ("CharSelectCanvas")) as GameObject;
        charMover = charSelect.transform.Find("Mover").gameObject;
	charMover.transform.position = new Vector3(5500, 5500, 0);
     //   GameObject levSelect = Instantiate (Resources.Load ("LevCanvas")) as GameObject;
     //   levMover = lev.transform.Find("Mover").gameObject;
     //   levMover.transform.position = new Vector3(5500, 5500, 0);
        GameObject opt = Instantiate (Resources.Load ("OptionsCanvas")) as GameObject;
        optMover = opt.transform.Find("Mover").gameObject;
	optMover.transform.position = new Vector3(5500, 5500, 0);
     //   GameObject tutorial = Instantiate (Resources.Load ("TutorialCanvas")) as GameObject;
     //   tutorialMover = tutorial.transform.Find("Mover").gameObject;
     //   tutorialMover.transform.position = new Vector3(5500, 5500, 0);
        initialized = true;	
}
    }

    void Update()
    {
        
    }

    
}
