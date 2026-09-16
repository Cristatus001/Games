using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public GameObject stickmanPrefab; // Prefab for the stickman.
    public Transform player;          // Reference to the player's transform.
    public float formationSpacing = 2.0f; // Spacing between stickmen in the formation.
    private List<GameObject> stickmen = new List<GameObject>(); // List to store stickmen.

    // Add stickmen to the crowd.
    public void AddStickmen(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newStickman = Instantiate(stickmanPrefab, transform.position, Quaternion.identity);
            FollowPlayer followScript = newStickman.GetComponent<FollowPlayer>();
            if (followScript != null)
            {
                followScript.player = player;          // Assign the player reference.
                followScript.index = stickmen.Count;   // Set index based on current count.
                followScript.formationSpacing = formationSpacing; // Set formation spacing.
            }
            stickmen.Add(newStickman);
        }
    }

    // Returns the number of stickmen in the crowd (excluding the player).
    public int GetStickmanCount()
    {
        return stickmen.Count;
    }

    // Remove stickmen from the crowd.
    public void RemoveStickmen(int count)
    {
        for (int i = 0; i < count && stickmen.Count > 0; i++)
        {
            GameObject stickmanToRemove = stickmen[stickmen.Count - 1];
            stickmen.RemoveAt(stickmen.Count - 1);
            Destroy(stickmanToRemove);
        }
    }

    // Multiply the total unit count (player as 1 unit + stickmen) by a factor.
    public void MultiplyStickmen(int factor)
    {
        // Total units include the player (1) plus current stickmen.
        int totalCount = stickmen.Count + 1;
        int targetTotal = totalCount * factor;
        int difference = targetTotal - totalCount;
        if (difference > 0)
        {
            AddStickmen(difference);
        }
    }

    // Divide the total unit count (player as 1 unit + stickmen) by a divisor.
    public void DivideStickmen(int divisor)
    {
        if (divisor <= 0)
        {
            Debug.LogWarning("Divisor must be greater than zero.");
            return;
        }
        int totalCount = stickmen.Count + 1;
        int targetTotal = totalCount / divisor;
        int difference = totalCount - targetTotal;
        if (difference > 0)
        {
            RemoveStickmen(difference);
        }
    }
}