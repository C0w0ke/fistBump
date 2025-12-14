using UnityEngine;

public interface IAttackStrategy
{
    void Execute(BaseCharacter character);
}

public class BasicAttackStrategy : IAttackStrategy
{
    private string animationName;
    private Transform[] targets; // Opponents or players

    public BasicAttackStrategy(string animName, Transform[] targetList)
    {
        animationName = animName;
        targets = targetList;
    }

    public void Execute(BaseCharacter character)
    {
        character.animator.Play(animationName);
        foreach (Transform target in targets)
        {
            if (Vector3.Distance(character.transform.position, target.position) <= character.attackRadius)
            {
                var targetChar = target.GetComponent<BaseCharacter>();
                if (targetChar != null)
                {
                    targetChar.StartCoroutine(targetChar.PlayHitDamageAnimation(character.attackDamage));
                }
            }
        }
    }
}
