using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FightingController : MonoBehaviour
{
    [Header("Movement Status")]
    private float movementSpeed = 2f;
    public float rotationSpeed = 10f;
    private CharacterController characterController;
    private Animator animator;

    [Header("Fighting Status")]
    public float attackCooldown = 0.5f;
    public int attackDamage = 5;
    private float attackRadius = 2f;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    private float lastAttackTime;
    public Transform[] opponents;

    [Header("Dodge Status")]
    public float dodgeCooldown = 1f;
    public float dodgeDistance = 2f;
    private float lastDodgeTime;

    [Header("Effects and Sounds")]
    public ParticleSystem attack1Effect;
    public ParticleSystem attack2Effect;
    public ParticleSystem attack3Effect;
    public AudioClip[] hitSounds;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;


    void Awake()
    {
        currentHealth = maxHealth;
        healthBar.SetFullHealth(currentHealth);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        PerformMovement();
        PerformDodge();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PerformAttack(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PerformAttack(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PerformAttack(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PerformAttack(3);
        }
    }

    void PerformMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0f)
        {
            // Get the main camera's transform
            Transform cameraTransform = Camera.main.transform;
            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            Vector3 worldMovement = cameraRight * horizontalInput;
            Quaternion targetRotation = Quaternion.LookRotation(worldMovement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 4f * Time.deltaTime);
            
            animator.SetBool("Walking", true);
            characterController.Move(worldMovement * movementSpeed * Time.deltaTime);
        }
        else
        {
            // No input: stop walking animation
            animator.SetBool("Walking", false);
        }
    }

    void PerformAttack(int attackIndex)
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.Play(attackAnimations[attackIndex]);

            int damage = attackDamage;
            //Debug.Log("Performed attack" + (attackIndex + 1) + "dealing" + damage + "damage");

            lastAttackTime = Time.time;

            // Loop each opponent
            foreach (Transform opponent in opponents)
            {
                if (Vector3.Distance(transform.position, opponent.position) <= attackRadius)
                {
                    opponent.GetComponent<OpponentAI>().StartCoroutine(opponent.GetComponent<OpponentAI>().PlayHitDamageAnimation(attackDamage));
                }
            }

        }
        else
        {
            // Stop player from attacking too quickly
            Debug.Log("Endlag! On cooldown...");
        }
    }

    void PerformDodge()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time - lastDodgeTime > dodgeCooldown)
            {
                animator.Play("DodgeFrontAnimation");
                float horizontalInput = Input.GetAxis("Horizontal");
                Vector3 dodgeDirection;
                if (horizontalInput > 0)
                {
                    dodgeDirection = Vector3.forward * dodgeDistance;
                }
                else if (horizontalInput < 0)
                {
                    dodgeDirection = -Vector3.forward * dodgeDistance;
                }
                else
                {
                    dodgeDirection = transform.forward * dodgeDistance;
                }
                characterController.Move(dodgeDirection);
                lastDodgeTime = Time.time;
            }
            else
            {
                Debug.Log("Dodge on cooldown");
            }
        }
    }

    public IEnumerator PlayHitDamageAnimation(int takeDamage)
    {
        yield return new WaitForSeconds(0.5f);

        // Play a random hit sound
        if (hitSounds != null && hitSounds.Length > 0 )
        {
            int randomIndex = Random.Range(0, hitSounds.Length);
            AudioSource.PlayClipAtPoint(hitSounds[randomIndex], transform.position);
        }

        // Decrease health
        currentHealth -= takeDamage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        animator.Play("HitDamageAnimation");
        Debug.Log("Player got hit!");
    }

    void Die()
    {
        Debug.Log("Player died!");
    }

    public void Attack1Effect()
    {
        attack1Effect.Play();
    }

    public void Attack2Effect()
    {
        attack2Effect.Play();
    }

    public void Attack3Effect()
    {
        attack3Effect.Play();
    }
}