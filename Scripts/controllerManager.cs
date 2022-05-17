using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerManager : MonoBehaviour
{


RectTransform[] charSelect = new RectTransform[4];

    void Start()
    {
        for(int i = 0; i < charSelect.Length; i++) {
		charSelect[i] = Instantiate (Resources.Load ("PlayerSelect")) as RectTransform;
	}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
