using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //public Vector3 velocity;
    public float speed = 20.0f;
    public float airSpeedMulti = .9f;
    float _maxHorizontalFreeSpeed = 30.0f;
    float _maxHorizontalSpeed = 10.0f;
    float _maxVerticalFreeSpeed = 10.0f;
    float slowDown = 15f;
    bool facingRight = true;

    public float jumpForce = .3f;
    Rigidbody2D rb;
    bool onGround = true;
    bool canWallJump = false;
    Animator animator;
    float _wallJumpSpeedHorizontal = 5f;
    float _wallJumpSpeedVertical = 15f;
    float verticalVelocity = 0;
    int wallMask = 1 << 3;
    SpriteRenderer m_SpriteRenderer;
    float horizontalInput = 0f;

    private PlayerInput playerInput;
    private float jumpInput = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        Controls controls = new Controls();
        controls.Gameplay.Enable();
        controls.Gameplay.Jump.performed += DoJump;
        controls.Gameplay.Move.performed += DoMovement;
        controls.Gameplay.Move.canceled += DoMovement;
    }


    // Update is called once per frame
    void Update()
    {

        //Get Input
        

        //Change velocity
        float xChange = horizontalInput * Time.deltaTime * speed * airSpeedMulti;
        if(xChange + rb.velocity.x > _maxHorizontalSpeed){
            xChange = _maxHorizontalSpeed - rb.velocity.x;
        }

        float yChange = verticalVelocity;
        rb.velocity += new Vector2(xChange, yChange);
        verticalVelocity = 0;
        float xVel = rb.velocity.x;

        if(xVel > 0){
            facingRight = true;
            m_SpriteRenderer.flipX = false;
        } else if (xVel < 0) {
            facingRight = false;
            m_SpriteRenderer.flipX = true;
        }
        
        Vector3 bottom = new Vector3(0, -transform.lossyScale.y/2, 0);

        //Check if on ground using left and right sides of the character
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + bottom + Vector3.right*.5f, -Vector2.up, 0.01f, wallMask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + bottom - Vector3.right*.5f, -Vector2.up, 0.01f, wallMask);
        if(hitLeft.collider != null || hitRight.collider != null){
            onGround = true;
            airSpeedMulti = 1f;

            Debug.Log(hitLeft.transform.gameObject.layer);

            if((hitLeft.collider != null && hitLeft.transform.gameObject.layer == 6) || (hitRight.collider != null && hitRight.transform.gameObject.layer == 6)){ //Slope Right
                rb.velocity += new Vector2(Mathf.Abs(rb.velocity.y/2), 0);
                Debug.Log(rb.velocity);
            } else if((hitLeft.collider != null && hitLeft.transform.gameObject.layer == 7) || (hitRight.collider != null && hitRight.transform.gameObject.layer == 7)){ //Slope Left
                rb.velocity += new Vector2(-1 * Mathf.Abs(rb.velocity.y/2), 0);
                Debug.Log(rb.velocity);
            }

        } else {
            onGround = false;
            airSpeedMulti = .9f;
        }
        if(onGround && jumpInput > 0){
            verticalVelocity = jumpForce;
            jumpInput = 0f;
        }
        
        //Wall Jumping
        RaycastHit2D hitWall;
        if(facingRight){
            Vector3 right = new Vector3(transform.lossyScale.x/2, 0, 0);
            hitWall = Physics2D.Raycast(transform.position + right, Vector2.right, 0.05f, wallMask);
        } else {
            Vector3 left = new Vector3(-transform.lossyScale.x/2, 0, 0);
            hitWall = Physics2D.Raycast(transform.position  + left, -Vector2.right, 0.05f, wallMask);
        }
        if(hitWall.collider != null){
            canWallJump = true;
            rb.velocity = new Vector2(rb.velocity.x, -.5f);
        } else {
            canWallJump = false;
        }

        //Wall jump if conditions are true
        if(!onGround && canWallJump && jumpInput > 0){            
            jumpInput = 0f;
            if(facingRight){
                rb.velocity = new Vector2(-_wallJumpSpeedHorizontal, _wallJumpSpeedVertical);
            } else {
                rb.velocity = new Vector2(_wallJumpSpeedHorizontal, _wallJumpSpeedVertical);
            }
        }

        //Slow Down if no input OR opposite input
        if((Mathf.Sign(xVel) != Mathf.Sign(horizontalInput) || Mathf.Abs(horizontalInput) <= .3) && xVel != 0){
            float deltaVelocity = -Mathf.Sign(rb.velocity.x)*slowDown*Time.deltaTime*airSpeedMulti + xVel;
            if(Mathf.Sign(deltaVelocity) != Mathf.Sign(xVel)){
                rb.velocity = new Vector2(0, rb.velocity.y);
            } else {
                rb.velocity = new Vector2(deltaVelocity, rb.velocity.y);
            }
        }

        //Maximum Horizontal Speed
        if(Mathf.Abs(rb.velocity.x) >= _maxHorizontalFreeSpeed){
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x)*_maxHorizontalFreeSpeed, rb.velocity.y);
        }
        //Maximum Vertical Speed
        if(Mathf.Abs(rb.velocity.y) >= _maxVerticalFreeSpeed){
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y)*_maxVerticalFreeSpeed);
        }

        jumpInput -= Time.deltaTime;

        //horizontalInput = 0f;

        //Setting Animations
        //animator.SetFloat("Walking", Mathf.Abs(rb.velocity.x));
    }

    public void DoJump(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump " + context.phase);
        if(!context.performed) return;

        jumpInput = .1f;
        
    }

    public void DoMovement(InputAction.CallbackContext context){
        Vector2 inputVector = context.ReadValue<Vector2>();
        horizontalInput = inputVector.x;
    }
}
