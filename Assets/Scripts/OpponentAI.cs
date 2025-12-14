using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentAI : BaseCharacter
{
    [Header("Opponent AI")]
    public FightingController[] fightingControllers;
    public Transform[] players;
    public bool isTakingDamage;

    protected override void Awake()
    {
        base.Awake();

        attackStrategies = new IAttackStrategy[]
        {
            new BasicAttackStrategy("Attack1Animation", players),
            new BasicAttackStrategy("Attack2Animation", players),
            new BasicAttackStrategy("Attack3Animation", players),
            new BasicAttackStrategy("Attack4Animation", players)
        };
    }

    protected override void Update()
    {
        HandleInputOrAI(); // AI logic
    }

    protected override void HandleInputOrAI()
    {
        for (int i = 0; i < fightingControllers.Length; i++)
        {
            if (players[i].gameObject.activeSelf)
            {
                if (Vector3.Distance(transform.position, players[i].position) <= attackRadius)
                {
                    animator.SetBool("Walking", false);
                    if (Time.time - lastAttackTime > attackCooldown && !isTakingDamage)
                    {
                        int randomAttack = Random.Range(0, attackStrategies.Length);
                        PerformAttack(randomAttack);
                    }
                }
                else
                {
                    Vector3 direction = (players[i].position - transform.position).normalized;
                    PerformMovement(direction);
                }
            }
        }
    }
}
