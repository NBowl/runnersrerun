using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    float speed = 20.0f;
    float airSpeedMulti = .9f;
    float slowDown = 50f;
    float jumpForce = 15f;
    
    Rigidbody2D rb;
    Animator animator;
    int wallMask = (1 << 3) + (1 << 8);
    int grappableMask = (1 << 8);
    int walJumpableMask = (1 << 7);
    SpriteRenderer m_SpriteRenderer;

    private PlayerInput playerInput;
    private float jumpInput = 0f;
    float horizontalInput = 0f;
    float verticalVelocity = 0;

    public Animator anim;
    public GameObject hookPrefab;
    private GameObject hookObj;
    private LineRenderer hookBar;
    private HookScript hookScript;
    private int playerNumber = 0;

    bool facingRight = true;
    bool changeDirection = true;
    bool hook = false;
    bool hookingAction = false;
    bool airJump = true;
    bool slide = false;
    bool onGround = true;
    bool canWallJump = false;
    bool ignoreGravity = false;
        
    float deltaXVel = 0f;
    float deltaYVel = 0f;
    float anglePerSec = 10f;
    float currentAngle = 30f;
    float hookSpeed = 10f;
    float hurtTimer = 0f;

    float _maxHorizontalFreeSpeed = 40.0f;
    float _maxHorizontalSpeed = 10.0f;
    float _maxVerticalFreeSpeed = 10.0f;
    float _wallJumpSpeedHorizontal = 10f;
    float _wallJumpSpeedVertical = 10f;
    float _minHookSpeed = 10f;
    float _defaultHookAngle = 55f;

    Vector3 hookPosition = new Vector3();
    float dist = 0.0f;
    private BoxCollider2D m_hitbox;
    float timeHooking = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_hitbox = GetComponent<BoxCollider2D>();

        Controls controls = new Controls();
        controls.Gameplay.Enable();
        controls.Gameplay.Jump.performed += DoJump;
        controls.Gameplay.Move.performed += DoMovement;
        controls.Gameplay.Move.canceled += DoMovement;
        controls.Gameplay.Hook.performed += DoHook;
        controls.Gameplay.Hook.canceled += StopHook;

        hookObj = Instantiate(hookPrefab, transform);
        hookObj.transform.position += new Vector3(0, .5f, 0);
        
        hookScript = hookObj.transform.Find("HookEnd").GetComponent<HookScript>();
        hookScript.SendScript(this);
        hookBar = hookScript.GetHookBar();
        hookBar.enabled = false;

        hookObj.transform.localScale = new Vector3(0, 0, 1);
    }

    private void Update(){
        deltaXVel = 0f;
        deltaYVel = 0f;
        
        Vector3 bottom = new Vector3(0, -transform.lossyScale.y/2, 0);

        //Wall Jumping
        RaycastHit2D hitWall;
        if(facingRight){
            Vector3 right = new Vector3(transform.lossyScale.x/2, 0, 0);
            hitWall = Physics2D.Raycast(transform.position + right*.3f, Vector2.right, 0.3f, walJumpableMask);
        } else {
            Vector3 left = new Vector3(-transform.lossyScale.x/2, 0, 0);
            hitWall = Physics2D.Raycast(transform.position  + left*.3f, -Vector2.right, 0.3f, walJumpableMask);
        }
        if(hitWall.collider != null){
        Debug.Log(hitWall.collider);
            canWallJump = true;
            airJump = true;
        } else {
            canWallJump = false;
        }

        //Check if on ground using left and right sides of the character
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + bottom - Vector3.right*.1f, -Vector2.up, 0.5f, wallMask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + bottom + Vector3.right*.1f, -Vector2.up, 0.5f, wallMask);
        if(hitLeft.collider != null || hitRight.collider != null){
            if(onGround == false){
                OnLand();
            }
            onGround = true;
            airJump = true;
            airSpeedMulti = 1f;
        } else {
            onGround = false;
            airSpeedMulti = .5f;
        }

        
        if(hurtTimer > 0){
            slide = false;
            StopHook();
        }

        Move();

        //Change velocity
        if(changeDirection && hurtTimer <= 0){
            deltaXVel += horizontalInput * Time.deltaTime * speed * airSpeedMulti;
            if(Mathf.Abs(deltaXVel) > _maxHorizontalSpeed){
                deltaXVel = _maxHorizontalSpeed*Mathf.Sign(deltaXVel);
            }
        }

        //deltaYVel += verticalVelocity;
        float xVel = rb.velocity.x + deltaXVel;

        //Slow Down if no input OR opposite input
        if(!hookingAction && ((Mathf.Sign(xVel) != Mathf.Sign(horizontalInput) || (Mathf.Abs(horizontalInput) <= .3) && xVel != 0) || (slide))){
            float multiplier = slowDown*airSpeedMulti;

            if(slide){
                multiplier *= .2f;
            }

            float deltaVelocity = -Mathf.Sign(rb.velocity.x)*multiplier*Time.deltaTime;
            deltaXVel += deltaVelocity;
            if(Mathf.Abs(deltaXVel+rb.velocity.x) < .1f){
                //Set x velocity to zero
                deltaXVel = -rb.velocity.x;
            }
        }

        if(xVel > 0.2f && changeDirection && !slide){
            facingRight = true;
            m_SpriteRenderer.flipX = false;
        } else if (xVel < -0.2f && changeDirection && !slide) {
            facingRight = false;
            m_SpriteRenderer.flipX = true;
        }

        /*
        //Maximum Vertical Speed
        if(Mathf.Abs(rb.velocity.y + deltaYVel) >= _maxVerticalFreeSpeed){
            deltaYVel = Mathf.Sign(deltaYVel)*(_maxVerticalFreeSpeed - Mathf.Abs(rb.velocity.y));
        }*/

        if(ignoreGravity){
            rb.gravityScale = 0f;
        } else {
            rb.gravityScale = 2f;
        }

        //Set HookPosition
        Vector3[] p = {transform.position+Vector3.up*.5f, hookPosition+Vector3.up*.5f};
        hookBar.SetPositions(p);
        hookScript.moveHook(hookPosition+Vector3.up*.5f);

        rb.velocity += new Vector2(deltaXVel, deltaYVel);
        jumpInput -= Time.deltaTime*10;
        hurtTimer -= Time.deltaTime;

        //Maximum Horizontal Speed
        if(Mathf.Abs(rb.velocity.x) >= _maxHorizontalFreeSpeed){
            rb.velocity = new Vector3(Mathf.Sign(rb.velocity.x)*_maxHorizontalFreeSpeed, rb.velocity.y, 0);
        }

        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }


    // Update is called once per frame
    void Move()
    {
        float xComponent = Mathf.Cos(currentAngle);
        float yComponent = Mathf.Sin(currentAngle);

        if(hookingAction){
            float tempDist = Vector3.Distance(hookPosition, transform.position);
            dist = Mathf.Min(tempDist, dist);

            Vector3 radiusVector = hookPosition - transform.position;
            Vector3 perpVector = new Vector3(radiusVector.y, -radiusVector.x, 0);
            perpVector /= dist;

            float offset = 1;
            if(perpVector.y > 0){
                offset += Mathf.Abs(perpVector.y)*.1f;
            }
            perpVector *= Mathf.Abs(hookSpeed) * offset;

            if(!facingRight){
                perpVector *= -1;
            }

            rb.velocity = new Vector2(perpVector.x, perpVector.y);

            return;
        }

        if(hook){
            //Raycast to see if there is a object to lock hook onto
            RaycastHit2D grapple = Physics2D.Raycast(hookPosition+Vector3.up*.5f, new Vector2(xComponent, yComponent), 2f, grappableMask);

            if(grapple.collider != null){
                hookPosition = grapple.point - Vector2.up*.5f;
                HookConfirm();
            } else {
                timeHooking += Time.deltaTime;
                hookPosition = new Vector3(xComponent*120f*timeHooking, yComponent*120f*timeHooking, 0)
                           + transform.position;
            }

        }

        if(slide && !hook){
            return;
        }

        //Wall jump if conditions are true
        if(!onGround && canWallJump && jumpInput > 0){            
            jumpInput = 0f;
            if(facingRight){
                deltaXVel = -_wallJumpSpeedHorizontal - rb.velocity.x;
            } else {
                deltaXVel = _wallJumpSpeedHorizontal - rb.velocity.x;
            }
            
            deltaYVel = _wallJumpSpeedVertical- rb.velocity.y;
            return;
        }
        
        //Jump if conditions true
        if(onGround && jumpInput > 0){
            deltaYVel = jumpForce;
            jumpInput = 0f;
            ResetAllTriggers();
            anim.SetTrigger("jump");
            return;
        } else if(!onGround && jumpInput >= 1 && airJump){
            jumpInput = 0f;
            airJump = false;
            deltaYVel = jumpForce - rb.velocity.y;
            ResetAllTriggers();
            anim.SetTrigger("doubleJump");
            return;
        }
    }

    public void DoJump(InputAction.CallbackContext context)
    {
        jumpInput = 1f;
        
    }

    public void DoMovement(InputAction.CallbackContext context){
        Vector2 inputVector = context.ReadValue<Vector2>();
        horizontalInput = inputVector.x;
        if(inputVector.y <= -.5){
            slide = true;
            m_hitbox.size = new Vector2(1.4f, .55f);
            m_hitbox.offset = new Vector2(0f, 0.28f);
            ResetAllTriggers();
            anim.SetTrigger("intoSlide");
        } else {
            slide = false;
            m_hitbox.size = new Vector2(.6f, 1.3f);
            m_hitbox.offset = new Vector2(0f, 0.63f);
            ResetAllTriggers();
            anim.SetTrigger("outSlide");
        }
    }

    public void DoHook(InputAction.CallbackContext context){
        timeHooking = 0f;
        hook = true;
        anim.SetBool("hook", true);
        hookPosition = transform.position + Vector3.up*3f;
        hookObj.transform.localScale = new Vector3(1f, 1f, 1f);
        hookBar.enabled = true;

        changeDirection = false;
        currentAngle = _defaultHookAngle * (Mathf.PI/180);
        if(facingRight){
            hookObj.transform.localRotation = Quaternion.Euler(0,0, _defaultHookAngle);
        } else {
            hookObj.transform.localRotation = Quaternion.Euler(0,0, -_defaultHookAngle);
            currentAngle += 2*((Mathf.PI/2) - currentAngle); //Reflects angle across Y axis
        }

    }

    public void StopHook(InputAction.CallbackContext context){
        StopHook();
    }

    public void StopHook(){
        hook = false;
        anim.SetBool("hook", false);
        hookPosition = transform.position + Vector3.up*3f;
        hookBar.enabled = false;

        hookObj.transform.localScale = new Vector3(0f, 0f, 1f);
        changeDirection = true;
        hookingAction = false;
        anim.SetBool("hookConfirm", false);
        ignoreGravity = false;
    }

    public void HookConfirm(){
        if(hookingAction) return;

        hook = false;
        anim.SetBool("hook", false);
        hookingAction = true;
        anim.SetBool("hookConfirm", true);
        //hookPosition = hookScript.Position();
        

        hookSpeed = Mathf.Abs(rb.velocity.x*1.4f*Mathf.Cos(currentAngle));
        hookSpeed += Mathf.Abs(rb.velocity.y*1.4f*Mathf.Sin(currentAngle));


        if(Mathf.Abs(hookSpeed) < _minHookSpeed){
            hookSpeed = _minHookSpeed;
        }

        ignoreGravity = true;
        dist = Vector3.Distance(hookPosition, transform.position);

        airJump = true;
    }

    public void HookMiss(){
        
        hook = false;
        anim.SetBool("hook", false);
        hookBar.enabled = false;
        //hookBar.transform.localScale = new Vector3(1f, 1f, 1);
        hookBar.SetPositions(new Vector3[1]);
        hookObj.transform.localScale = new Vector3(0f, 0f, 1f);
        changeDirection = true;
    }

    private void OnLand(){
        anim.SetTrigger("falling");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(hookingAction) StopHook(); 
    }

    void OnTriggerEnter2D(Collider2D col){
        int layer = col.gameObject.layer;
        if(layer == 10) { //Box Obstacle
            hurtTimer = 1f;
            hurt();
            Destroy(col.gameObject);
        }
    }

    private void hurt(){
        ResetAllTriggers();
        anim.SetTrigger("hurt");
        Debug.Log("ouchy");

    }

    private void ResetAllTriggers()
    {
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                anim.ResetTrigger(param.name);
            }
        }
    }
}
