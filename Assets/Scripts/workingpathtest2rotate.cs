using UnityEngine;
    using UnityEngine.AI;

    public class WorkingPathTest2roatate : MonoBehaviour
    {
        public float rotationSpeed = 5f;
        [SerializeField] Animator anicontroller;
        public int maxHealth = 3;
        private int currentHealth;
        public GameObject bulletPrefab;
        public Transform player;
    /*    public playerhealth playerHealth;*/
        public Transform[] waypoints;
        private int currentWaypointIndex = 0;
        private NavMeshAgent agent;
        private bool isPlayerInRange = false;


        public float detectionRange = 10f; // Range within which the enemy detects the player
        public float fireRate = 1f; // Rate at which the enemy fires bullets (in bullets per second)
        private float nextFireTime;

        public float health=50f;
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            SetNextWaypoint();
            currentHealth = maxHealth;
        }

        void Update()
{
    // Check if the player is within detection range
    if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRange)
    {
        // Set the flag to indicate player detection
        isPlayerInRange = true;

        // Stop the enemy
        StopMoving();

        // Rotate towards the player
        RotateTowardsPlayer();

        // Fire at the player with delay
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate; // Calculate next fire time
        }

        // Set the shoot animation parameter to true
        anicontroller.SetBool("shoot", true);
    }
    else
    {
        // If the player is out of range, resume patrolling
        isPlayerInRange = false;
        ResumeMoving();

        // Set the shoot animation parameter to false
        anicontroller.SetBool("shoot", false);
    }

    // If not in range and not attacking, move to the next waypoint
    if (!isPlayerInRange && !agent.pathPending && agent.remainingDistance < 0.1f)
    {
        SetNextWaypoint();
        // Set the run animation parameter to true
        anicontroller.SetBool("run", true);
    }
    else
    {
        // Set the run animation parameter to false
        anicontroller.SetBool("run", false);
    }
}

void RotateTowardsPlayer()
{
    // Calculate the direction from the enemy to the player
    Vector3 direction = (player.position - transform.position).normalized;

    // Calculate the rotation to look at the player
    Quaternion lookRotation = Quaternion.LookRotation(direction);

    // Smoothly rotate the enemy towards the player
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
}
        void SetNextWaypoint()
        {
            if (!isPlayerInRange && waypoints.Length == 0)
            {
                Debug.LogError("No waypoints set!");
                return;
            }

            // Set the destination to the next waypoint
            if (!isPlayerInRange)
            {
                agent.SetDestination(waypoints[currentWaypointIndex].position);

                // Move to the next waypoint in a loop
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }

        void StopMoving()
        {
            // Stop the agent from moving
            agent.isStopped = true;

        }

        void ResumeMoving()
        {
            // Resume the agent's movement
            agent.isStopped = false;
        }

        void Fire()
        {
            // Instantiate bullet prefab
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // Set the bullet's direction towards the player
            bullet.transform.LookAt(player.position);

            // Check if the bullet hits the player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, bullet.transform.forward, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Player"))
                {
             /*       playerHealth.Takedamage(20);*/
                }
            }
        }
        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            anicontroller.SetBool("DIE",true);
            // Play death animation, sound effects, etc.
            gameObject.SetActive(false);
        }
    }

