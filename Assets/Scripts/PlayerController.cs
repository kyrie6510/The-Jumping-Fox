using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    public float speed;
    public float JumpForce;
    
    public Collider2D collider;
        
    public LayerMask ground;

    public int cherryCount=0;

    public Text TxtCherryNum;
    
    
    [SerializeField]
    private Animator anim;
    private Rigidbody2D rb;

    public bool isHurt=false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHurt)
        {
            Movement();
        }
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

        if (Input.GetButtonDown("Jump")&&collider.IsTouchingLayers(ground))
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

        else if(isHurt)
        {
            anim.SetFloat("running",0);
            anim.SetBool("hurt",true);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt",false);
                anim.SetBool("idel",true);
                isHurt = false;
            }
        }
        
        //判断是否接触地面
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
            TxtCherryNum.text = "Cherry:" + cherryCount;
            Destroy(other.gameObject);
        }
        
    }
    
    //消灭敌人
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (anim.GetBool("falling"))
            {
                Destroy(other.gameObject);
                rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.deltaTime);
                anim.SetBool("jumping", true);
            }
            
            else if (transform.position.x < other.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-3, rb.velocity.y);
                isHurt = true;
            }
            else if (transform.position.x > other.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(3, rb.velocity.y);
                isHurt = true;
            }

            
            
        }

      
       
        
    }
}
