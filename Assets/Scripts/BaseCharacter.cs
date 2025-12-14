using System.Collections;
using UnityEngine;

public enum CharacterState { Idle, Walking, Attacking, Dodging, TakingDamage, Dead }

public abstract class BaseCharacter : MonoBehaviour
{
    [Header("Config")]
    public StatConfig stats; // Assign the stats asset here (e.g., PlayerStats or AIStats)

    [Header("Movement")]
    private float movementSpeed;
    public float rotationSpeed;
    protected CharacterController characterController;
    public Animator animator;

    [Header("Fighting")]
    public float attackCooldown;
    public int attackDamage;
    public float attackRadius;
    protected float lastAttackTime;
    public IAttackStrategy[] attackStrategies;

    [Header("Dodge")]
    public float dodgeCooldown;
    public float dodgeDistance;
    protected float lastDodgeTime;

    [Header("Effects and Sounds")]
    public ParticleSystem attack1Effect;
    public ParticleSystem attack2Effect;
    public ParticleSystem attack3Effect;
    public AudioClip[] hitSounds;

    [Header("Health")]
    public HealthComponent healthComponent;

    protected CharacterState currentState = CharacterState.Idle;

    protected virtual void Awake()
    {
        if (stats == null)
        {
            Debug.LogError("CharacterStats not assigned to " + gameObject.name + "! Create and assign a CharacterStats asset.");
            return;
        }

        movementSpeed = stats.movementSpeed;
        rotationSpeed = stats.rotationSpeed;
        attackCooldown = stats.attackCooldown;
        attackDamage = stats.attackDamage;
        attackRadius = stats.attackRadius;
        dodgeCooldown = stats.dodgeCooldown;
        dodgeDistance = stats.dodgeDistance;

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        healthComponent = GetComponent<HealthComponent>();

        if (healthComponent == null)
        {
            Debug.LogError("HealthComponent missing on " + gameObject.name);
        }
        else
        {
            healthComponent.maxHealth = stats.maxHealth;
            healthComponent.currentHealth = stats.maxHealth;
            healthComponent.healthBar?.SetFullHealth(stats.maxHealth);
        }
    }


    protected virtual void Update()
    {

    }

    // Movement logic
    protected void PerformMovement(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 4f * Time.deltaTime);
            animator.SetBool("Walking", true);
            characterController.Move(direction * movementSpeed * Time.deltaTime);
            SetState(CharacterState.Walking);
        }
        else
        {
            animator.SetBool("Walking", false);
            SetState(CharacterState.Idle);
        }
    }

    // Attack logic using Strategy Pattern
    protected void PerformAttack(int attackIndex)
    {
        if (Time.time - lastAttackTime > attackCooldown && attackStrategies.Length > attackIndex)
        {
            attackStrategies[attackIndex].Execute(this);
            lastAttackTime = Time.time;
            SetState(CharacterState.Attacking);
        }
        else
        {
            Debug.Log("Attack on cooldown!");
        }
    }

    // Dodge logic
    protected void PerformDodge(Vector3 direction)
    {
        if (Time.time - lastDodgeTime > dodgeCooldown)
        {
            animator.Play("DodgeFrontAnimation");
            characterController.Move(direction * dodgeDistance);
            lastDodgeTime = Time.time;
            SetState(CharacterState.Dodging);
        }
        else
        {
            Debug.Log("Dodge on cooldown");
        }
    }

    // Hit/Damage logic
    public virtual IEnumerator PlayHitDamageAnimation(int takeDamage)
    {
        SetState(CharacterState.TakingDamage);
        yield return new WaitForSeconds(0.5f);

        if (hitSounds != null && hitSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSounds.Length);
            AudioSource.PlayClipAtPoint(hitSounds[randomIndex], transform.position);
        }
        animator.Play("HitDamageAnimation");
        healthComponent.TakeDamage(takeDamage);
        Debug.Log(gameObject.name + " got hit!");
        SetState(CharacterState.Idle);
    }

    // Death logic
    public virtual void Die()
    {
        SetState(CharacterState.Dead);
        Debug.Log(gameObject.name + " died!");
    }

    // Effect
    public void Attack1Effect() { attack1Effect?.Play(); }
    public void Attack2Effect() { attack2Effect?.Play(); }
    public void Attack3Effect() { attack3Effect?.Play(); }

    // State management
    protected void SetState(CharacterState newState)
    {
        currentState = newState;
    }

    protected abstract void HandleInputOrAI();
}
