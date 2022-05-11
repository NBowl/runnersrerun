using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{
    public void QuitGame() {
	Debug.Log("QUIT!");
	Application.Quit();
    }

    public void StartClick() {
	Debug.Log("char select initiated");
	Startup.mainMover.transform.position = new Vector3(5500, 5500, 0);
        Startup.charMover.transform.position = new Vector3(400,220, 0);
	Debug.Log(Startup.charMover.transform.position);
}
    public static void backMenu() {
	Debug.Log("returned to menu!");
	Startup.charMover.transform.position = new Vector3(5500, 5500, 0);
	Debug.Log(Startup.charMover.transform.position);
	Startup.optMover.transform.position = new Vector3(5500, 5500, 0);
	//Startup.tutorialMover.transform.position = new Vector3(5500, 5500, 0);
        Startup.mainMover.transform.position = new Vector3(400,220, 0);
	
}
    public void optionsClick() {
	Debug.Log("options initiated");
	Startup.mainMover.transform.position = new Vector3(5500, 5500, 0);
        Startup.optMover.transform.position = new Vector3(400,220, 0);
    }

    public void TutorialClick() {
	Debug.Log("tutorials initiated");
    }

    public void StartGameClick() {
	Debug.Log("level select initiated");
    }
}
