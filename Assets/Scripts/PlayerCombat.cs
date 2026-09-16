using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    private CrowdManager crowdManager;
    private PlayerController playerController; // Your movement script controlling forward motion.
    private Enemy currentEnemy;
    private bool inCombat = false;
    public float attackDelay = 0.5f; // Time (in seconds) between each attack (reduces 1 health per attack)

    void Start()
    {
        crowdManager = GetComponent<CrowdManager>();
        if (crowdManager == null)
        {
            Debug.LogError("CrowdManager component is missing on the player.");
        }

        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("PlayerController component not found on the player.");
        }
    }

    // Use OnTriggerEnter since enemy colliders should be set as triggers.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !inCombat)
        {
            currentEnemy = other.GetComponent<Enemy>();
            if (currentEnemy != null && currentEnemy.IsAlive())
            {
                // Stop player movement.
                if (playerController != null)
                {
                    playerController.enabled = false;
                }
                inCombat = true;
                StartCoroutine(CombatRoutine());
            }
        }
    }

    // Coroutine to attack the enemy gradually.
    private IEnumerator CombatRoutine()
    {
        // Continue attacking while in combat, enemy is alive, and there is at least one stickman.
        while (inCombat && currentEnemy != null && currentEnemy.IsAlive() && crowdManager.GetStickmanCount() > 0)
        {
            // Deal 1 damage and remove one stickman.
            currentEnemy.TakeDamage(1);
            crowdManager.RemoveStickmen(1);
/*
            Debug.Log("Attacked enemy. Remaining enemy health: " + currentEnemy.GetHealth() +
                      " | Remaining stickmen: " + crowdManager.GetStickmanCount());*/

            yield return new WaitForSeconds(attackDelay);
        }
        EndCombat();
    }

    void EndCombat()
    {
        inCombat = false;
        currentEnemy = null;
        // Resume player movement.
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}
