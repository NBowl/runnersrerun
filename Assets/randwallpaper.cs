using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randwallpaper : MonoBehaviour
{
    public Sprite[] Backgrounds;
    public SpriteRenderer Render;
    void Start () {
            Render = GetComponent<SpriteRenderer>(); 
            Render.sprite = Backgrounds[Random.Range(0, Backgrounds.Length)]; 
         }
     // Update is called once per frame
     void Update () {
    
        
    }
}
