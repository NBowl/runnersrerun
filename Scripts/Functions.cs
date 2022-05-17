using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{
    private static Vector3 screenPos = Startup.screenPos;
    private static Vector3 offScreen = Startup.offScreen;
    private static float moveTime = 2f;
    private static bool allowMove = true;
    private static Functions functionInstance = null;


    private void Start(){
        if(functionInstance == null)
            functionInstance = this;
    }

    public static void QuitGame() {
	    Debug.Log("QUIT!");
	    Application.Quit();
    }

    public static void StartClick() {
	    MoveObjects(Startup.mainMover, Startup.charMover);
    }
    public static void backMenu() {
        MoveOffScreen(Startup.charMover);
	    MoveObjects(Startup.optMover, Startup.mainMover);
    }
    public static void OptionsClick() {
	    MoveObjects(Startup.mainMover, Startup.optMover);
    }

    public static void TutorialClick() {
	    //MoveObjects(Startup.mainMover, Startup.tutorialMover);
    }

    public static void StartGameClick() {
	    MoveObjects(Startup.mainMover, Startup.charMover);
    }

    private static void MoveObjects(GameObject moveOffScreen, GameObject moveOnScreen){
        if(allowMove == true){
            functionInstance.StartCoroutine(functionInstance.MoveOverSeconds(moveOffScreen, offScreen, moveTime));
            functionInstance.StartCoroutine(functionInstance.MoveOverSeconds(moveOnScreen, screenPos, moveTime));
            allowMove = false;
        }
    }
    
    private static void MoveOffScreen(GameObject moveOffScreen){
        if(allowMove == true){
            functionInstance.StartCoroutine(functionInstance.MoveOverSeconds(moveOffScreen, offScreen, moveTime));
        }
    }

	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
     float elapsedTime = 0;
     Vector3 startingPos = objectToMove.transform.position;
     while (elapsedTime < seconds)
     {
         objectToMove.transform.position = Vector3.Lerp(startingPos, end, (1/(seconds*seconds)) * (elapsedTime * elapsedTime));
         elapsedTime += Time.deltaTime;
         yield return new WaitForEndOfFrame();
     }
     objectToMove.transform.position = end;
     allowMove = true;
 }
}
