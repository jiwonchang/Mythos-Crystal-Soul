using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config Variables
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] int numberOfJumps = 2;
    [SerializeField] float health = 100f;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float deathVFXDuration = 2f;
    [SerializeField] float attack1Time = 0.03f;
    [SerializeField] float attack2Time = 0.02f;
    [SerializeField] float attack3Time = 0.008f;
    [SerializeField] float jumpAttack1Time = 0.02f;
    [SerializeField] float jumpingAttackDuration = 0.08f;
    [SerializeField] float invulAfterDamTime = 2f;
    int jumpedNumber = 0;

    // State Variables
    bool isAlive = true;
    bool currentlyClimbing = false;
    bool isInvulnerable = false;
    bool isFlashingInvulnerable = false;

    // Cached Component References
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeet;
    SpriteRenderer mySpriteRenderer;
    GameObject saberAttack1Collision;
    GameObject saberAttack2Collision;
    GameObject saberAttack3Collision;
    GameObject saberAttack4Collision;
    float gravityScaleAtStart;
    float defaultAnimatorSpeed;

    // Other variables
    Coroutine flashingAfterDamageCoroutine;

    // Message then methods
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeet = transform.GetChild(3).GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        saberAttack1Collision = transform.GetChild(0).gameObject;
        saberAttack2Collision = transform.GetChild(1).gameObject;
        saberAttack3Collision = transform.GetChild(2).gameObject;
        saberAttack4Collision = transform.GetChild(4).gameObject;
        gravityScaleAtStart = myRigidBody.gravityScale;
        defaultAnimatorSpeed = myAnimator.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }

        Run();
        Jump();
        FlipSprite();
        Descend();
        Attack();
        TakeDamage();
        ClimbLadder();
    }

    private void Run()
    {
        if (MeetsMoveCondition())
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 to +1
            Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
            myRigidBody.velocity = playerVelocity;

            bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
            if (MeetsRunCondition())
            {
                myAnimator.SetBool("Running", playerHasHorizontalSpeed);
                myAnimator.SetBool("Landing", false);
            }
        }
    }

    private void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            if (MeetsJumpCondition())
            {
                myAnimator.SetBool("Jumping", true);
                myAnimator.SetBool("Running", false);
                myAnimator.SetBool("Landing", false);
                myAnimator.SetBool("Jump Descending", false);
                Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0);
                myRigidBody.velocity += jumpVelocityToAdd;

                jumpedNumber += 1;
            }
        }
        if (myAnimator.GetBool("Jumping"))
        {
            myAnimator.SetBool("Landing", false);
        }
        JumpDescend();
    }

    private void JumpDescend()
    {
        bool playerHasNegVertSpeed = myRigidBody.velocity.y <= 0f;
        if (myAnimator.GetBool("Jumping") && playerHasNegVertSpeed)
        {
            myAnimator.SetBool("Jumping", false);
            myAnimator.SetBool("Jump Descending", true);
        }
    }

    public void JumpDescendToDescendAnimation()
    {
        myAnimator.SetBool("Jump Descending", false);
        myAnimator.SetBool("Descending", true);
    }

    private void Descend()
    {
        bool playerHasNegVertSpeed = myRigidBody.velocity.y < 0;
        if (playerHasNegVertSpeed && !myAnimator.GetBool("Jump Descending") && !myAnimator.GetBool("Climbing"))
        {
            myAnimator.SetBool("Descending", true);
        }
        Land();
    }

    private void Land()
    {
        bool playerHasTouchedGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (playerHasTouchedGround && (myAnimator.GetBool("Jump Descending") || myAnimator.GetBool("Descending")))
        {
            myAnimator.SetBool("Descending", false);
            myAnimator.SetBool("Jump Descending", false);
            myAnimator.SetBool("Landing", true);
            jumpedNumber = 0;
        }
    }

    public void DoneLanding()
    {
        myAnimator.SetBool("Landing", false);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        // if the player is moving horizontally,
        if (playerHasHorizontalSpeed)
        {
            if (MeetsRunCondition())
            {
                myAnimator.SetBool("Running", true);
            }
            // reverse the current scaling of the x axis
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private bool MeetsMoveCondition()
    {
        return !(myAnimator.GetFloat("Attack") > 0) && !myAnimator.GetBool("TakingMajorDamage") && !myAnimator.GetBool("TakingMinorDamage");
    }

    private bool MeetsRunCondition()
    {
        return !(myAnimator.GetBool("Jumping") || myAnimator.GetBool("Descending") || myAnimator.GetBool("Jump Descending") || currentlyClimbing);
    }

    private bool MeetsJumpCondition()
    {
        return (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) || jumpedNumber < numberOfJumps) && (myAnimator.GetFloat("Attack") <= 0f) 
            && !(myAnimator.GetBool("TakingMajorDamage") || myAnimator.GetBool("TakingMinorDamage"));
    }

    private void ClimbLadder()
    {
        bool touchingLadder = myFeet.IsTouchingLayers(LayerMask.GetMask("Ladders"));
        if (!touchingLadder)
        {
            myAnimator.SetBool("Climbing", false);
            if (currentlyClimbing)
            {
                myRigidBody.gravityScale = gravityScaleAtStart;
            }
            currentlyClimbing = false;
            myAnimator.speed = defaultAnimatorSpeed;
            return;
        }

        float controlFlow = CrossPlatformInputManager.GetAxis("Vertical");

        if (controlFlow != 0.0f && !CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            currentlyClimbing = true;
            myAnimator.speed = Mathf.Abs(controlFlow) * defaultAnimatorSpeed;

            Vector2 playerClimbVelocity = new Vector2(myRigidBody.velocity.x, controlFlow * climbSpeed);
            myRigidBody.velocity = playerClimbVelocity;

            bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
            myAnimator.SetBool("Descending", false);
            myRigidBody.gravityScale = 0;
        }
        else if (controlFlow == 0.0f && currentlyClimbing && !CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            myRigidBody.gravityScale = 0;
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);
        }
        else if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            currentlyClimbing = false;
            myAnimator.SetBool("Climbing", false);
            myAnimator.speed = defaultAnimatorSpeed;
            myRigidBody.gravityScale = gravityScaleAtStart;
        }
    }

    private void Attack()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            if (MeetsRunCondition())
            {
                float attackFloatCount = myAnimator.GetFloat("Attack");
                //if (attackFloatCount < 3 && MeetsRunCondition())
                if (attackFloatCount < 3)
                {
                    myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);
                    myAnimator.SetFloat("Attack", attackFloatCount + 1f);
                    //EnableAttackCollision(attackFloatCount + 1f);
                }
            }
            else if (myAnimator.GetBool("Jump Descending") || myAnimator.GetBool("Descending") || myAnimator.GetBool("Jumping"))
            {
                if (myAnimator.GetBool("Jumping"))
                {
                    if (myRigidBody.velocity.y > 0f)
                    {
                        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);
                    }
                    myAnimator.SetBool("Jumping", false);
                    myAnimator.SetBool("Descending", true);
                }
                StartCoroutine(JumpAttack1Coroutine());
            }
        }
    }

    /*
    public void EnableAttackCollision(float attackIndex)
    {
        if (attackIndex == 1)
        {
            Debug.Log("attack 1 activate!");
            saberAttack1Collision.SetActive(true);
        }
        else if (attackIndex == 2)
        {
            Debug.Log("attack 2 activate!");
            saberAttack2Collision.SetActive(true);
        }
        else if (attackIndex == 3)
        {
            Debug.Log("attack 3 activate!");
            saberAttack3Collision.SetActive(true);
        }
    }

    public void DisableAttackCollision(float attackIndex)
    {
        if (attackIndex == 1)
        {
            Debug.Log("attack 1 retract!");
            saberAttack1Collision.SetActive(false);
        }
        else if (attackIndex == 2)
        {
            Debug.Log("attack 2 retract!");
            saberAttack2Collision.SetActive(false);
        }
        else if (attackIndex == 3)
        {
            Debug.Log("attack 3 retract!");
            saberAttack3Collision.SetActive(false);
        }
    }
    */

    public void EnableAttackCollision(float attackIndex)
    {
        StartCoroutine(AttackCollisionCoroutine(attackIndex));
    }

    IEnumerator AttackCollisionCoroutine(float attackIndex)
    {
        float attackDuration = attack1Time;
        if (attackIndex == 1)
        {
            attackDuration = attack1Time;
            //Debug.Log("attack 1 activate for " + attackDuration + " seconds!");
            saberAttack1Collision.SetActive(true);
        }
        else if (attackIndex == 2)
        {
            attackDuration = attack2Time;
            //Debug.Log("attack 2 activate for " + attackDuration + " seconds!");
            saberAttack2Collision.SetActive(true);
        }
        else if (attackIndex == 3)
        {
            attackDuration = attack3Time;
            //Debug.Log("attack 3 activate for " + attackDuration + " seconds!");
            saberAttack3Collision.SetActive(true);
        }
        else if (attackIndex == 4)
        {
            attackDuration = jumpAttack1Time;
            //Debug.Log("attack 3 activate for " + attackDuration + " seconds!");
            saberAttack4Collision.SetActive(true);
        }
        yield return new WaitForSeconds(attackDuration);
        if (attackIndex == 1)
        {
            //Debug.Log("attack 1 retract!");
            saberAttack1Collision.SetActive(false);
        }
        else if (attackIndex == 2)
        {
            //Debug.Log("attack 2 retract!");
            saberAttack2Collision.SetActive(false);
        }
        else if (attackIndex == 3)
        {
            //Debug.Log("attack 3 retract!");
            saberAttack3Collision.SetActive(false);
        }
        else if (attackIndex == 4)
        {
            //Debug.Log("attack 3 retract!");
            saberAttack4Collision.SetActive(false);
        }
    }

    public void ResetAttackCount()
    {
        myAnimator.SetFloat("Attack", 0);
    }

    public void NeedAttackEndAnimation()
    {
        myAnimator.SetBool("AttackEndAnimFinished", false);
    }

    public void FinishAttackEndAnimation()
    {
        myAnimator.SetBool("AttackEndAnimFinished", true);
    }

    IEnumerator JumpAttack1Coroutine()
    {
        bool jumpAttackBool = myAnimator.GetBool("Jumping Attack");
        myAnimator.SetBool("Jumping Attack", true);
        yield return new WaitForSeconds(jumpingAttackDuration);
        myAnimator.SetBool("Jumping Attack", false);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Enemy enemy = otherCollider.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            if (!isInvulnerable)
            {
                if (enemy)
                {
                    health -= enemy.GetCollisionDamage();
                }
                myRigidBody.velocity = new Vector2(0f, 0f);
                myRigidBody.gravityScale = 0f;
                if (health > 0f)
                {
                    MakeInvulnerableAfterDamage();
                    myAnimator.SetBool("TakingMajorDamage", true);
                }
                else
                {
                    Die();
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D otherCollider)
    {
        Enemy enemy = otherCollider.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            if (!isInvulnerable)
            {
                if (enemy)
                {
                    health -= enemy.GetCollisionDamage();
                }
                myRigidBody.velocity = new Vector2(0f, 0f);
                myRigidBody.gravityScale = 0f;
                if (health > 0f)
                {
                    MakeInvulnerableAfterDamage();
                    myAnimator.SetBool("TakingMajorDamage", true);
                }
                else
                {
                    Die();
                }
            }
        }
    }

    private void TakeDamage()
    {
        /*
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")) && !isInvulnerable)
        {
            health -= 10f;
            myRigidBody.velocity = new Vector2(0f, 0f);
            myRigidBody.gravityScale = 0f;
            if (health > 0f)
            {
                MakeInvulnerableAfterDamage();
                myAnimator.SetBool("TakingMajorDamage", true);
            }
            else
            {
                Die();
            }
        }
        */
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards")) && !isInvulnerable)
        {
            health -= 1000000000f;
            myRigidBody.gravityScale = 0f;
            if (health <= 0f)
            {
                Die();
                myRigidBody.gravityScale = gravityScaleAtStart;
            }
        }
    }

    private void MakeInvulnerableAfterDamage()
    {
        StartCoroutine(InvincibilityAfterDamageCoroutine());
    }

    public void DisableTakingMajorDamageAnimation()
    {
        myAnimator.SetBool("TakingMajorDamage", false);
        flashingAfterDamageCoroutine = StartCoroutine(DamageFlashing());
        myRigidBody.gravityScale = gravityScaleAtStart;
    }

    IEnumerator InvincibilityAfterDamageCoroutine()
    {
        isInvulnerable = true;
        isFlashingInvulnerable = true;
        yield return new WaitForSeconds(invulAfterDamTime);
        if (flashingAfterDamageCoroutine != null)
        {
            StopCoroutine(flashingAfterDamageCoroutine);
        }
        mySpriteRenderer.enabled = true;
        isInvulnerable = false;
        isFlashingInvulnerable = false;
    }

    IEnumerator DamageFlashing()
    {
        while (isFlashingInvulnerable)
        {
            mySpriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
            mySpriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
        }
    }

    private void Die()
    {
        isAlive = false;
        //gameObject.SetActive(false);
        Destroy(gameObject);
        GameObject deathExplosion = Instantiate(deathVFX, transform.position, Quaternion.identity) as GameObject;
        Destroy(deathExplosion, deathVFXDuration);
    }
}
