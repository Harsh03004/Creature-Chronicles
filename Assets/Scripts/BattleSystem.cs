using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST ,RUN}

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
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Check for keyboard inputs during player's turn
        if (state == BattleState.PLAYERTURN)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnAttackButton();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                OnSuperButton();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                OnHealButton();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                OnRunButton();
            }
        }
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


    IEnumerator SetupBattle()
    {
        // Instantiate player and enemy GameObjects
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation.position, Quaternion.identity);
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation.position, Quaternion.identity);

        // Get references to the Unit components of the player and enemy
        playerUnit = playerGO.GetComponent<Unit>();
        enemyUnit = enemyGO.GetComponent<Unit>();

        // Reset player and enemy health
        playerUnit.ResetHealth();
        enemyUnit.ResetHealth();

        // Calculate direction to make the enemy face the player
        Vector3 directionToPlayer = playerBattleStation.position - enemyBattleStation.position;
        enemyGO.transform.rotation = Quaternion.LookRotation(directionToPlayer);

        // Update HUDs with initial health values
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        // Show initial dialogue
        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        // Wait for a moment before starting the battle
        yield return new WaitForSeconds(2f);

        // Set the state to player's turn and start the battle
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

        Animator playerAnimator = playerUnit.GetComponentInChildren<Animator>();
        if (playerUnit.attackPP > 0 && playerAnimator != null)
        {
            playerAnimator.SetTrigger("Slash");
        }

        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(4f);

        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        
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
        if (playerUnit.superAttackPP <= 0)
        {
            dialogueText.text = "No PP left for super attack!";
            yield return new WaitForSeconds(2f);
            EnemyTurn();
            yield break;
        }

        Animator playerAnimator = playerUnit.GetComponentInChildren<Animator>();
        if (playerUnit.attackPP > 0 && playerAnimator != null)
        {
            playerAnimator.SetTrigger("Super");
        }

        dialogueText.text = "The player performed a super attack!";

        yield return new WaitForSeconds(4f);

        bool isdead = enemyUnit.SuperAttack(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);

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

        yield return new WaitForSeconds(4f);

        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    IEnumerator PlayerRun()
    {
        dialogueText.text = "Player ran away from fight";
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainGame");
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
        Animator EnemyAnimator = enemyUnit.GetComponentInChildren<Animator>();
        if (EnemyAnimator != null)
        {
            EnemyAnimator.SetTrigger("SUPER");
        }

        dialogueText.text = "The enemy performed a Super Attack";

        yield return new WaitForSeconds(4f);

        bool isdead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

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
        yield return new WaitForSeconds(4f);

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
        StartCoroutine(EndBattleCoroutine());
    }

    IEnumerator EndBattleCoroutine()
    {
        if (state == BattleState.WON)
        {
            Animator EnemyAnimator = enemyUnit.GetComponentInChildren<Animator>();
            if (EnemyAnimator != null)
            {
                EnemyAnimator.SetTrigger("DIE");
            }
            dialogueText.text = "You won the battle!";

            // Wait for a few seconds to see the animation and health depletion
            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene("MainGame");
        }
        else if (state == BattleState.LOST)
        {
            Animator playerAnimator = playerUnit.GetComponentInChildren<Animator>();
            if (playerUnit.attackPP > 0 && playerAnimator != null)
            {
                playerAnimator.SetTrigger("DIE");
            }
            dialogueText.text = "You were defeated.";

            // Wait for a few seconds to see the animation and health depletion
            yield return new WaitForSeconds(5f);

            SceneManager.LoadScene("GameOver");
        }
    }

}
