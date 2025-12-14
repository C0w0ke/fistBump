using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Game/Character Stats")]
public class StatConfig : ScriptableObject
{
    [Header("Movement")]
    public float movementSpeed = 2f;
    public float rotationSpeed = 10f;

    [Header("Fighting")]
    public float attackCooldown = 0.5f;
    public int attackDamage = 10;
    public float attackRadius = 2f;

    [Header("Dodge")]
    public float dodgeCooldown = 1f;
    public float dodgeDistance = 2f;

    [Header("Health")]
    public int maxHealth = 100;
}
