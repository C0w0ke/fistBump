using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        Debug.DrawRay(character.transform.position, character.transform.forward * character.attackRadius, Color.red, 1f);

        float sphereRadius = 0.2f;
        RaycastHit[] hits = Physics.SphereCastAll(character.transform.position, sphereRadius, character.transform.forward, character.attackRadius);

        Debug.Log("Total hits: " + hits.Length);

        foreach (RaycastHit hit in hits)
        {
            Debug.Log("Hit object: " + hit.transform.name);
            Transform target = hit.transform;
            if (System.Array.IndexOf(targets, target) >= 0)
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

//public void Execute(BaseCharacter character)
//{
//    character.animator.Play(animationName);
//    foreach (Transform target in targets)
//    {
//        if (Vector3.Distance(character.transform.position, target.position) <= character.attackRadius)
//        {
//            var targetChar = target.GetComponent<BaseCharacter>();
//            if (targetChar != null)
//            {
//                targetChar.StartCoroutine(targetChar.PlayHitDamageAnimation(character.attackDamage));
//            }
//        }
//    }
//}