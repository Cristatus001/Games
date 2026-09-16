using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;       // Assign your enemy prefab here.
    public Vector3 centerPoint = Vector3.zero; // Center of the formation.
    public int numberOfEnemies = 8;        // Total enemies to spawn.
    public float formationRadius = 5f;     // Radius of the circle formation.

    void Start()
    {
        SpawnEnemyFormation();
    }

    void SpawnEnemyFormation()
    {
        float angleStep = 360f / numberOfEnemies;
        float angle = 0f;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            float enemyPosX = centerPoint.x + Mathf.Sin(angle * Mathf.Deg2Rad) * formationRadius;
            float enemyPosZ = centerPoint.z + Mathf.Cos(angle * Mathf.Deg2Rad) * formationRadius;
            Vector3 enemyPos = new Vector3(enemyPosX, centerPoint.y, enemyPosZ);
            Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
            angle += angleStep;
        }
    }
}
