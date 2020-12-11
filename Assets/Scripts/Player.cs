using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void DeadEventHandler();

public class Player : MonoBehaviour
{
    private static Player instance;

    public event DeadEventHandler Dead;

    [SerializeField]
    private Stat healthStat;

    [SerializeField]
    private GameObject PopUpWin;

    [SerializeField]
    private GameObject PopUpLose;

    public static Player Instance
    {
        get {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    private Rigidbody2D myRigidbody;

    private Animator myAnimator;

    [SerializeField]
    private float movementSpeed;

    private bool attack;

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    private bool facingRight;

    [SerializeField]
    private int health;

    [SerializeField]
    private EdgeCollider2D swordCollider;

    public EdgeCollider2D SwordCollider
	{
		get
		{
			return swordCollider;
		}
	}

    [SerializeField]
    private List<string> damageSources;

    private bool IsDead 
    { 
        get 
        {
            if(healthStat.CurrentVal <= 0)
            {
                OnDead();
            }
            
            return healthStat.CurrentVal <= 0;
        }
    }

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    private bool immortal = false;

    private SpriteRenderer spriteRenderer;

    private Vector3 startPos;

    [SerializeField]
    private float immortalTime;

    // Use this for initialization
    void Start()
    {
        facingRight = true;

        startPos = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();

        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        healthStat.Initialize();
    }

    void Update()
    {
        if(!TakingDamage && !IsDead)
        {
            if(transform.position.y <= -14f)
            {
                Death();
            }
                HandleInput();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!TakingDamage  && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");

            isGrounded = IsGrounded();

            HandleMovement(horizontal);

            Flip(horizontal);

            HandleAttacks();

            HandleLayers();

            ResetValues();
        }
    }

    public void OnDead() 
    {
        if(Dead != null)
        {
            Dead();
            Time.timeScale = 0;
            PopUpLose.SetActive(true);
        }
    }

    private void HandleMovement(float horizontal)
    {
        if (myRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }

        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && (isGrounded || airControl))
        {
            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
        }

        if (isGrounded && jump)
        {
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetTrigger("jump");
        }

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleAttacks()
    {
        if (attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myAnimator.SetTrigger("attack");
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            attack = true;
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
        }
    }

    private void ResetValues()
    {
        attack = false;

        jump = false;
    }

    private bool IsGrounded()
    {
        if (myRigidbody.velocity.y <= 1)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }

    private IEnumerator IndicateImmortal()
    {
        while(immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    public IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            healthStat.CurrentVal -= 10;

            if(!IsDead)
            {
                myAnimator.SetTrigger("damage");
                immortal = true;

                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                myAnimator.SetLayerWeight(1,0);
                myAnimator.SetTrigger("die");
            }
        }
    }

    public void MeleeAttack()
    {
        SwordCollider.enabled = true;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }

    public void Death()
    {
        myRigidbody.velocity = Vector2.zero;
        myAnimator.SetTrigger("jump");
        healthStat.CurrentVal = healthStat.MaxVal;
        transform.position = startPos;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Sword")
        {
            GameManager.Instance.CollectedSwords++;
            Destroy(other.gameObject);

            if(GameManager.Instance.CollectedSwords == 5)
            {
                Time.timeScale = 0;
                PopUpWin.SetActive(true);
            }
        }
    }
}
