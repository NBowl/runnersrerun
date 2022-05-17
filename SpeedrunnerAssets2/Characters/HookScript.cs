using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    public Vector3 scale;
    public GameObject hook;
    public LineRenderer hookBar;
    private PlayerController p;
    // Start is called before the first frame update

    // Update is called once per frame
    public void moveHook(Vector3 pos)
    {
        hook.transform.position = pos;
    }

    public LineRenderer GetHookBar(){
        return hookBar;
    }

    public Vector3 Position(){
        return transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8){ //Hookable
            p.HookConfirm();
        } else if (collision.gameObject.layer != 6 && collision.gameObject.layer != 9 ){ //Player and other Hooks
            p.HookMiss();
        }
    }

    public void SendScript(PlayerController p){
        this.p = p;
    }
}
