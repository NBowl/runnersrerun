 using UnityEngine;  
 using System.Collections;  
 using UnityEngine.EventSystems;  
 using UnityEngine.UI;

 
 public class textColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
 
     public Text theText;
 
     public void OnPointerEnter(PointerEventData eventData)
     {
         theText.color = Color.blue; 
     }
 
     public void OnPointerExit(PointerEventData eventData)
     {
         theText.color = Color.white; 
     }
 }

