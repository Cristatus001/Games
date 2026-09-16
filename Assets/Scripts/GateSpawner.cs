using UnityEngine;
using TMPro;

public class GateSpawner : MonoBehaviour
{
    public GameObject gatePrefab;         // Assign your gate prefab here.
    public GameObject textPrefab;         // Assign your TextMeshPro text prefab here.
    public CrowdManager crowdManager;     // Reference to the player's CrowdManager.

    public float planeWidth = 50f;        // Width of the plane.
    public float gateSpacing = 3f;        // Distance between paired gates.
    public int numberOfPairs = 5;         // Number of gate pairs to spawn.
    public float startingZPosition = -25f; // Starting Z position for gate spawning.

    private void Start()
    {
        // Validate prefabs and references.
        if (gatePrefab == null)
        {
            Debug.LogError("Gate prefab is not assigned.");
            return;
        }
        if (textPrefab == null)
        {
            Debug.LogError("Text prefab is not assigned.");
            return;
        }
        if (crowdManager == null)
        {
            Debug.LogError("CrowdManager is not assigned. Please assign the player's CrowdManager.");
            return;
        }

        SpawnGatePairs();
    }

    void SpawnGatePairs()
    {
        float currentZ = startingZPosition;

        for (int i = 0; i < numberOfPairs; i++)
        {
            // Determine pair type:
            // Most pairs are addition/subtraction.
            // Every 5th pair (when (i+1) % 5 == 0) will be multiplication/division.
            bool isAddSubtractPair = true;
            if ((i + 1) % 5 == 0)
                isAddSubtractPair = false;

            int value1 = 0;
            int value2 = 0;

            // Get current total stickman count (player counts as 1).
            int currentTotal = crowdManager.GetStickmanCount() + 1;

            if (isAddSubtractPair)
            {
                // For addition, allowed range is now 10 to 40.
                // Ensure (currentTotal + value) <= 200.
                int maxAdd = 200 - currentTotal;
                value1 = (maxAdd < 10) ? Mathf.Max(maxAdd, 0) : Random.Range(10, Mathf.Min(41, maxAdd + 1));
                // For subtraction, use a larger fixed range (60-100).
                value2 = Random.Range(60, 101);
            }
            else
            {
                // For multiplication/division, allowed factors are between 2 and 5.
                int maxFactor = Mathf.FloorToInt(200f / currentTotal);
                if (maxFactor < 2)
                    maxFactor = 2;
                value1 = Random.Range(2, Mathf.Min(6, maxFactor + 1));
                value2 = Random.Range(2, 6);
            }

            // Create positions for the gates side by side along the X-axis.
            Vector3 position1 = new Vector3(-gateSpacing / 2.8f, 0, currentZ); // Left gate.
            Vector3 position2 = new Vector3(gateSpacing / 2.8f, 0, currentZ);  // Right gate.

            // Spawn first gate.
            GameObject gate1 = Instantiate(gatePrefab, position1, Quaternion.identity);
            Gate gateScript1 = gate1.GetComponent<Gate>();
            if (gateScript1 != null)
            {
                if (isAddSubtractPair)
                    gateScript1.Initialize(Gate.Operation.Add, value1);
                else
                    gateScript1.Initialize(Gate.Operation.Multiply, value1);
                // For text, use side -1 for left gate.
                SpawnGateText(gate1.transform, gateScript1.operation, value1, -1);
            }
            else
            {
                Debug.LogError("Gate prefab does not have a Gate component.");
            }

            // Spawn second gate.
            GameObject gate2 = Instantiate(gatePrefab, position2, Quaternion.identity);
            Gate gateScript2 = gate2.GetComponent<Gate>();
            if (gateScript2 != null)
            {
                if (isAddSubtractPair)
                    gateScript2.Initialize(Gate.Operation.Subtract, value2);
                else
                    gateScript2.Initialize(Gate.Operation.Divide, value2);
                // For text, use side 1 for right gate.
                SpawnGateText(gate2.transform, gateScript2.operation, value2, 1);
            }
            else
            {
                Debug.LogError("Gate prefab does not have a Gate component.");
            }

            // Increment the Z position for the next gate pair.
            currentZ += gateSpacing * 3;
        }
    }

    void SpawnGateText(Transform gateTransform, Gate.Operation operation, int value, int side)
    {
        // Position the text in front of the gate.
        // Here, side is used to adjust horizontal offset (if needed).
        Vector3 textOffset = new Vector3(side * 0f, 0, -0.1f);
        GameObject textObj = Instantiate(textPrefab, gateTransform.position + textOffset, Quaternion.identity);
        textObj.transform.SetParent(gateTransform); // Parent the text to the gate.

        TextMeshPro textMeshPro = textObj.GetComponent<TextMeshPro>();
        if (textMeshPro != null)
        {
            textMeshPro.text = $"{GetOperationSymbol(operation)} {value}";
        }
        else
        {
            Debug.LogError("Text prefab does not have a TextMeshPro component.");
        }
    }

    string GetOperationSymbol(Gate.Operation operation)
    {
        switch (operation)
        {
            case Gate.Operation.Add: return "+";
            case Gate.Operation.Subtract: return "-";
            case Gate.Operation.Multiply: return "x";
            case Gate.Operation.Divide: return "÷";
            default: return "";
        }
    }
}
