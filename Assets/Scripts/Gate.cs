using UnityEngine;

public class Gate : MonoBehaviour
{
    public enum Operation { Add, Subtract, Multiply, Divide }
    public Operation operation;
    public int value;

    public void Initialize(Operation op, int val)
    {
        operation = op;
        value = val;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CrowdManager crowdManager = other.GetComponent<CrowdManager>();
            if (crowdManager != null)
            {
                ApplyOperation(crowdManager);
            }
        }
    }

    private void ApplyOperation(CrowdManager crowdManager)
    {
        switch (operation)
        {
            case Operation.Add:
                crowdManager.AddStickmen(value);
                break;
            case Operation.Subtract:
                crowdManager.RemoveStickmen(value);
                break;
            case Operation.Multiply:
                crowdManager.MultiplyStickmen(value);
                break;
            case Operation.Divide:
                crowdManager.DivideStickmen(value);
                break;
        }
    }
}