using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;
    public CameraController cam;
    public int numPlayer = 1;
    private List<GameObject> players;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();    

        for(int i = 0; i < numPlayer; i++){
            players.Add(Instantiate(player, this.transform));
            cam.players.Add(players[i]);
        }
    }
}
