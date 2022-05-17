using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void OnPlayerJoin(PlayerInput pi){
        Debug.Log("eee " + pi);
    }

    public void OnPlayerExit(PlayerInput pi){
        Debug.Log("fff " + pi);
    }
}
