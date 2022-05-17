using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public List<GameObject> players;
    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 avg = new Vector3();

        int weightSum = 0;

        for(int i = 0; i < players.Count; i++){
            int weight = (players.Count-i*3 + 5);
            avg += players[i].transform.position * weight;
            weightSum += weight;
        }

        avg /= weightSum;

        avg = new Vector3(avg.x, avg.y+5, -10);

        transform.position = avg;
    }
}
