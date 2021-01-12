using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    Character character;

    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    void ShootEnd()
    {
        character.SetState(Character.State.Idle);
    }

    void AttackEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
    }

    void PunchEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
    }

    void DoDamage()
    {
        Character targetCharacter = character.target.GetComponent<Character>();
        targetCharacter.DoDamage();
    }
}
