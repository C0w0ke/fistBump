using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingController : BaseCharacter
{
    [Header("Player-Specific")]
    public Transform[] opponents; // Assign in inspector

    protected override void Awake()
    {
        base.Awake();
        // Initialize attack strategies (e.g., for keys 1-4)
        attackStrategies = new IAttackStrategy[]
        {
            new BasicAttackStrategy("Attack1Animation", opponents),
            new BasicAttackStrategy("Attack2Animation", opponents),
            new BasicAttackStrategy("Attack3Animation", opponents),
            new BasicAttackStrategy("Attack4Animation", opponents)
        };
    }

    protected override void Update()
    {
        HandleInputOrAI(); // Player input
    }

    protected override void HandleInputOrAI()
    {
        // Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        Transform cameraTransform = Camera.main.transform;
        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        Vector3 worldMovement = cameraRight * horizontalInput;
        PerformMovement(worldMovement);

        // Attacks
        if (Input.GetKeyDown(KeyCode.Alpha1)) PerformAttack(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) PerformAttack(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) PerformAttack(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) PerformAttack(3);

        // Dodge
        if (Input.GetKeyDown(KeyCode.E))
        {
            float hInput = Input.GetAxis("Horizontal");
            Vector3 dodgeDir = (hInput > 0) ? Vector3.forward : (hInput < 0) ? -Vector3.forward : transform.forward;
            PerformDodge(dodgeDir);
        }
    }
}
