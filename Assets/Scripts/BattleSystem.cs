using System.Collections;
using System.Security.Cryptography.X509Certificates;
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

        // Calculate the health percentage of both player and enemy
        float playerHealthPercentage = (float)playerUnit.currentHP / playerUnit.maxHP;
        float enemyHealthPercentage = (float)enemyUnit.currentHP / enemyUnit.maxHP;

        // Calculate the difference in health between player and enemy
        int healthDifference = playerUnit.currentHP - enemyUnit.currentHP;

        // Define a threshold for low health
        float lowHealthThreshold = 0.3f;

        // Use rule-based system to decide the action
        if (enemyHealthPercentage < lowHealthThreshold)
        {
            StartCoroutine(EnemyHeal());
        }
        else if (playerHealthPercentage < lowHealthThreshold)
        {
            StartCoroutine(EnemySuperAttack());
        }
        else if (healthDifference > 20)
        {
            StartCoroutine(EnemySuperAttack());
        }
        else
        {
            StartCoroutine(EnemyAttack());
        }
    }

<<<<<<< HEAD
=======

>>>>>>> Manas/main
    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation.position, Quaternion.identity);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation.position, Quaternion.identity);
        enemyUnit = enemyGO.GetComponent<Unit>();

        // Calculate the direction from the enemy to the player
        Vector3 directionToPlayer = playerBattleStation.position - enemyBattleStation.position;
<<<<<<< HEAD
        // Ignore the y-axis to ensure the enemy faces the player horizontally
        directionToPlayer.y = 0f;
        // Rotate the enemy to face the player
        enemyGO.transform.rotation = Quaternion.LookRotation(directionToPlayer);

        // Rotate the enemy a little bit around the y-axis
        float randomAngle = Random.Range(-15f, 15f);
        enemyGO.transform.Rotate(Vector3.up, randomAngle);

=======
        // Rotate the enemy to face the player
        enemyGO.transform.rotation = Quaternion.LookRotation(directionToPlayer);

>>>>>>> Manas/main
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
<<<<<<< HEAD
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
=======
>>>>>>> Manas/main
        if (playerUnit.attackPP <= 0)
        {
            dialogueText.text = "No PP left for regular attack!";
            yield return new WaitForSeconds(2f);
            EnemyTurn();
            yield break;
        }

        Animator playerAnimator = playerUnit.GetComponentInChildren<Animator>();
        if (playerUnit.attackPP > 0 && playerAnimator != null)
        {
<<<<<<< HEAD
            dialogueText.text = "The attack is successful!";
        }

        playerAnimator.SetTrigger("Slash");
        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(3f);

=======
            playerAnimator.SetTrigger("Slash");
        }

        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(4f);

        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        
>>>>>>> Manas/main
        playerUnit.attackPP--; // Decrease PP for regular attack

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
<<<<<<< HEAD
        bool isdead = enemyUnit.SuperAttack(playerUnit.damage);
        if (playerUnit.superAttackPP <= 0)
        {
            dialogueText.text = "No PP left for super attack!";
            yield return new WaitForSeconds(3f);
=======
        if (playerUnit.superAttackPP <= 0)
        {
            dialogueText.text = "No PP left for super attack!";
            yield return new WaitForSeconds(2f);
>>>>>>> Manas/main
            EnemyTurn();
            yield break;
        }

        Animator playerAnimator = playerUnit.GetComponentInChildren<Animator>();
        if (playerUnit.attackPP > 0 && playerAnimator != null)
        {
<<<<<<< HEAD
            dialogueText.text = "The player performed a super attack!";
        }
        playerAnimator.SetTrigger("Super");
        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(3f);
=======
            playerAnimator.SetTrigger("Super");
        }

        dialogueText.text = "The player performed a super attack!";

        yield return new WaitForSeconds(4f);

        bool isdead = enemyUnit.SuperAttack(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
>>>>>>> Manas/main

        playerUnit.superAttackPP--; // Decrease PP for super attack

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

<<<<<<< HEAD
        yield return new WaitForSeconds(3f);
=======
        yield return new WaitForSeconds(4f);
>>>>>>> Manas/main

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

        // Start the player attack coroutine
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
<<<<<<< HEAD
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        Animator EnemyAnimator = enemyUnit.GetComponentInChildren<Animator>();
        Animator playerAnimator = playerUnit.GetComponentInChildren<Animator>();

        if (EnemyAnimator != null && playerAnimator != null)
        {

            dialogueText.text = "The enemy performed a slash attack";

        }
        EnemyAnimator.SetTrigger("Slash");
        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(3f);
        playerAnimator.SetTrigger("Hurt");
        yield return new WaitForSeconds(3f); // This delay ensures the "Hurt" animation finishes playing before proceeding

        if (isDead)
=======
        Animator EnemyAnimator = enemyUnit.GetComponentInChildren<Animator>();
        if (EnemyAnimator != null)
        {
            EnemyAnimator.SetTrigger("Slash");
        }

        dialogueText.text = "The enemy performed a slash attack";

        yield return new WaitForSeconds(4f);

        bool isdead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);

        if (isdead)
>>>>>>> Manas/main
        {
            state = BattleState.LOST;
            EndBattle();
        }
<<<<<<< HEAD
=======

>>>>>>> Manas/main
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

<<<<<<< HEAD

    IEnumerator EnemySuperAttack()
    {
        bool isdead = playerUnit.TakeDamage(enemyUnit.damage);
        Animator EnemyAnimator = enemyUnit.GetComponentInChildren<Animator>();
        Animator playerAnimator = playerUnit.GetComponentInChildren<Animator>();
        if (EnemyAnimator != null)
        {
            dialogueText.text = "The enemy performed a Super Attack";

        }
        EnemyAnimator.SetTrigger("SUPER");
        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(3f);
        playerAnimator.SetTrigger("Hurt");
        yield return new WaitForSeconds(3f);
=======
    IEnumerator EnemySuperAttack()
    {
        Animator EnemyAnimator = enemyUnit.GetComponentInChildren<Animator>();
        if (EnemyAnimator != null)
        {
            EnemyAnimator.SetTrigger("SUPER");
        }

        dialogueText.text = "The enemy performed a Super Attack";

        yield return new WaitForSeconds(4f);

        bool isdead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);
>>>>>>> Manas/main

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
<<<<<<< HEAD
        yield return new WaitForSeconds(3f);
=======
        yield return new WaitForSeconds(4f);
>>>>>>> Manas/main

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
            Animator EnemyAnimator = enemyUnit.GetComponentInChildren<Animator>();
            if (EnemyAnimator != null)
            {
                EnemyAnimator.SetTrigger("DIE");
            }
            dialogueText.text = "You won the battle!";
        }
        else if (state == BattleState.LOST)
        {
            Animator playerAnimator = playerUnit.GetComponentInChildren<Animator>();
            if (playerUnit.attackPP > 0 && playerAnimator != null)
            {
                playerAnimator.SetTrigger("DIE");
            }
            dialogueText.text = "You were defeated.";
        }
    }
}
