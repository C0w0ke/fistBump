using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingController : MonoBehaviour
{
    [Header("Movement Status")]
    public float movementSpeed = 1f;
    public float rotationSpeed = 10f;
    private CharacterController characterController;
    private Animator animator;

    [Header("Fighting Status")]
    public float attackCooldown = 0.5f;
    public int attackDamage = 5;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    private float lastAttackTime;

    [Header("Dodge Status")]
    public float dodgeCooldown = 1f;
    public float dodgeDistance = 2f;
    private float lastDodgeTime;

    [Header("Effects and Sounds")]
    public ParticleSystem attack1Effect;
    public ParticleSystem attack2Effect;
    public ParticleSystem attack3Effect;

    void Awake()
    {
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

        Vector3 movement = new Vector3(0f, 0f, horizontalInput);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            // transform.rotation = targetRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 4f * Time.deltaTime);
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
        characterController.Move(movement * movementSpeed * Time.deltaTime);
    }

    void PerformAttack(int attackIndex)
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.Play(attackAnimations[attackIndex]);

            int damage = attackDamage;
            Debug.Log("Performed attack" + (attackIndex + 1) + "dealing" + damage + "damage");

            lastAttackTime = Time.time;
        }
        else
        {
            Debug.Log("Endlag");
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