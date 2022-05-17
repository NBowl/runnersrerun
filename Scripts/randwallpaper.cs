using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randwallpaper : MonoBehaviour
{
    public Sprite[] Backgrounds;
    public SpriteRenderer Render;
    void Start () {
            Render = GetComponent<SpriteRenderer>(); 
            int rand = Random.Range(0, Backgrounds.Length);
            Render.sprite = Backgrounds[rand]; 
            if(rand == 1){
                transform.localScale = new Vector3(1.25f,1.25f,1);
            }
         }
     // Update is called once per frame
     void Update () {
    
        
    }
}
