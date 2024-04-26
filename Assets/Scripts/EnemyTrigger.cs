using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTrigger : MonoBehaviour
{
    private Animator animator;
    private bool animationPlayed = false;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MC") && !animationPlayed)
        {
            Debug.Log("Collided with the player");
            // Play animation
            animator.SetTrigger("WALK");

            // Set a flag to prevent playing the animation again
            animationPlayed = true;

            // Invoke a method to load the combat scene after the animation duration
            Invoke("LoadCombatScene", animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    void LoadCombatScene()
    {
        string sceneName = "";

        // Check the tag of the enemy
        switch (gameObject.tag)
        {
            case "Enemy 1":
                sceneName = "Combat 1";
                break;
            case "Enemy 2":
                sceneName = "Combat 2";
                break;
            case "Enemy 3":
                sceneName = "Combat 3";
                break;
            default:
                Debug.LogWarning("Unknown enemy tag");
                break;
        }

        // Load the combat scene
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}