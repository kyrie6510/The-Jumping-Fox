using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float speed;
    public float JumpForce;
    
    public Collider2D collider;
        
    public LayerMask ground;

    public int cherryCount=0;
    
    [SerializeField]
    private Animator anim;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        SwitchAnim();
    }

    void Movement()
    {
        float x =  Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float facedDiection = Input.GetAxisRaw("Horizontal");
        
        if (x != 0)
        {
            rb.velocity = new Vector2(x* speed*Time.deltaTime,rb.velocity.y);
            anim.SetFloat("running",Mathf.Abs(facedDiection));
        }

        if (facedDiection != 0)
        {
            transform.localScale = new Vector3(facedDiection, 1, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.deltaTime);
            anim.SetBool("jumping",true);
        }
        
        
        
        
    }

    void SwitchAnim()
    {
        anim.SetBool("idel",false);
        if (anim.GetBool("jumping"))
        {
          
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
            
            
        }
        else if (collider.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",false);
            anim.SetBool("idel",true);
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Collection")
        {
            cherryCount += 1;
            Destroy(other.gameObject);
        }
        
    }
}
