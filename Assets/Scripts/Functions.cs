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
    }

    public void OptionsClick() {
	Debug.Log("options initiated");
    }

    public void TutorialClick() {
	Debug.Log("tutorials initiated");
    }

    public void StartGameClick() {
	Debug.Log("level select initiated");
    }
}
