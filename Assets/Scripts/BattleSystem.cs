﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    void EnemyTurn()
    {
        dialogueText.text = "Now the enemy will attack";
        int randomAttack = UnityEngine.Random.Range(0, 3);

        switch (randomAttack)
        {
            case 0:
                StartCoroutine(EnemyAttack());
                break;
            case 1:
                StartCoroutine(EnemySuperAttack());
                break;
            case 2:
                StartCoroutine(EnemyHeal());
                break;
            case 3:
                StartCoroutine(EnemyRun());
                break;
        }
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation.position, Quaternion.identity);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation.position, Quaternion.identity);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    //Player's side
    IEnumerator PlayerAttack()
    {
        if (playerUnit.attackPP <= 0)
        {
            dialogueText.text = "No PP left for regular attack!";
            yield return new WaitForSeconds(2f);
            EnemyTurn();
            yield break;
        }

        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful!";
        playerUnit.attackPP--; // Decrease PP for regular attack

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    IEnumerator SuperAttack()
    {
        if (playerUnit.superAttackPP <= 0)
        {
            dialogueText.text = "No PP left for super attack!";
            yield return new WaitForSeconds(2f);
            EnemyTurn();
            yield break;
        }

        bool isdead = enemyUnit.SuperAttack(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The player performed a super attack!";
        playerUnit.superAttackPP--; // Decrease PP for super attack

        yield return new WaitForSeconds(2f);
        if (isdead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    IEnumerator PlayerHeal()
    {
        if (playerUnit.healPP <= 0)
        {
            dialogueText.text = "No PP left for healing!";
            yield return new WaitForSeconds(2f);
            EnemyTurn();
            yield break;
        }

        playerUnit.Heal(20);
        playerUnit.healPP--; // Decrease PP for healing

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    IEnumerator PlayerRun()
    {
        dialogueText.text = "Player ran away from fight";
        yield return new WaitForSeconds(2f);
        EndBattle();
    }

    //Player's side of fighitng
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }

    public void OnRunButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerRun());
    }

    public void OnSuperButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(SuperAttack());
    }

    //Enemy's side
    IEnumerator EnemyAttack()
    {
        bool isdead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "The enemy performed a slash attack";

        yield return new WaitForSeconds(2f);

        if (isdead)
        {
            state = BattleState.LOST;
            EndBattle();
        }

        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EnemySuperAttack()
    {
        bool isdead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "The enemy performed a Super Attack";

        yield return new WaitForSeconds(2f);

        if (isdead)
        {
            state = BattleState.LOST;
            EndBattle();
        }

        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EnemyHeal()
    {
        enemyUnit.Heal(20);
        enemyUnit.healPP--; // Decrease PP for enemy healing

        dialogueText.text = "The enemy healed itself";
        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator EnemyRun()
    {
        dialogueText.text = "The enemy ran away";
        yield return new WaitForSeconds(2f);
        EndBattle();
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
    }
}
