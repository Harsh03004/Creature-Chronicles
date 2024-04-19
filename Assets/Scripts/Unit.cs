using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;
    public int SuperDamage;
    public int maxHP;
    public int currentHP;

    public int attackPP; // PP for regular attack
    public int superAttackPP; // PP for super attack
    public int healPP; // PP for healing

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public bool SuperAttack(int SuperDamage)
    {
        if (superAttackPP <= 0)
            return false; // Cannot use super attack if no PP remaining

        currentHP -= SuperDamage;
        superAttackPP--;

        if (currentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
