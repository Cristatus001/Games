using UnityEngine;
using System.Collections.Generic;

public class FollowPlayer : MonoBehaviour
{
    [Header("Formation Settings")]
    public Transform player;             // The player's transform to follow.
    public float followSpeed = 20f;      // Speed at which the stickman moves into formation.
    public float formationSpacing = 1.5f; // Spacing between stickmen in the formation (set by CrowdManager).

    [HideInInspector]
    public int index;                    // Index of this stickman in the formation (auto-updated).

    // Static list to track all stickmen in the formation.
    private static List<FollowPlayer> allStickmen = new List<FollowPlayer>();

    private Vector3 targetLocalPosition;
    private Quaternion targetRotation;

    private void Awake()
    {
        // Ensure the static list is initialized.
        if (allStickmen == null)
            allStickmen = new List<FollowPlayer>();
    }

    private void Start()
    {
        // Register this stickman in the formation list.
        allStickmen.Add(this);

        // Parent the stickman to the player for easier formation management.
        if (player != null)
        {
            transform.SetParent(player);
        }
        else
        {
            Debug.LogError("FollowPlayer: Player transform is not assigned.");
        }
    }

    private void OnDestroy()
    {
        // Unregister this stickman when it's destroyed.
        allStickmen.Remove(this);
    }

    private void Update()
    {
        if (player == null)
            return;

        // Update the index based on the current formation list.
        index = allStickmen.IndexOf(this);
        int totalStickmen = allStickmen.Count;

        // Determine grid dimensions.
        int columns = Mathf.CeilToInt(Mathf.Sqrt(totalStickmen));
        int rows = Mathf.CeilToInt((float)totalStickmen / columns);

        // Determine the row and column of this stickman.
        int row = index / columns;
        int col = index % columns;

        // Calculate the center of the grid.
        float centerX = (columns - 1) / 2f;
        float centerZ = (rows - 1) / 2f;

        // Calculate the offset so that the formation is centered on the player.
        float offsetX = (col - centerX) * formationSpacing;
        float offsetZ = (row - centerZ) * formationSpacing;

        // You can add a small Y offset if needed.
        float offsetY = 0.1f;

        // Set the target local position relative to the player.
        targetLocalPosition = new Vector3(offsetX, offsetY, offsetZ);

        // Smoothly update the stickman's local position.
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, followSpeed * Time.deltaTime);

        // Rotate the stickman to face the same direction as the player.
        Vector3 direction = player.forward;
        if (direction.sqrMagnitude > 0.001f)
        {
            targetRotation = Quaternion.LookRotation(direction);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, followSpeed * Time.deltaTime);
        }
    }
}
