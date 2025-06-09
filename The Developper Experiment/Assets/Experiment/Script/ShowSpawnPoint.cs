using UnityEngine;

public class ShowSpawnPoint : MonoBehaviour
{
    public GameObject prefabToSpawn;

    private void OnDrawGizmos()
    {
        if (prefabToSpawn != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, prefabToSpawn.transform.localScale);
        }
    }
}
