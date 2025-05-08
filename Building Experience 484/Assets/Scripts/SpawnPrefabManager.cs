using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class SpawnPrefabManager : MonoBehaviour
{
    public GameObject prefab1;  // The first prefab to spawn
    public GameObject prefab2;  // The second prefab to spawn
    public GameObject prefab3;  // The third prefab to spawn

    public Button button1; // Button for spawning prefab1
    public Button button2; // Button for spawning prefab2
    public Button button3; // Button for spawning prefab3

    public Transform spawnLocation1; // Location to spawn prefab1
    public Transform spawnLocation2; // Location to spawn prefab2
    public Transform spawnLocation3; // Location to spawn prefab3

    public float spawnRadius = 0.5f; // The radius to check for collisions around the spawn point

    public LastGrabbed grabManager;
    public InputActionReference toggleFreezeAction;

    void Start()
    {
        button1.onClick.AddListener(() => TrySpawnPrefab(prefab1, spawnLocation1));
        button2.onClick.AddListener(() => TrySpawnPrefab(prefab2, spawnLocation2));
        button3.onClick.AddListener(() => TrySpawnPrefab(prefab3, spawnLocation3));
    }

    void TrySpawnPrefab(GameObject prefab, Transform spawnLocation)
    {
        if (prefab != null && spawnLocation != null)
        {
            // Check if the area around the spawn location is clear (no collisions)
            Collider[] colliders = Physics.OverlapSphere(spawnLocation.position, spawnRadius);

            if (colliders.Length == 0)
            {
                // If no colliders are found, instantiate the prefab
                GameObject spawnedPrefab = Instantiate(prefab, spawnLocation.position, spawnLocation.rotation);
                GrabbableObject grabbableObject = spawnedPrefab.GetComponent<GrabbableObject>();
                grabbableObject.grabManager = grabManager;
                grabbableObject.toggleFreezeAction = toggleFreezeAction;
            }
            else
            {
                Debug.Log("Spawn location is occupied. Try again.");
            }
        }
    }
}
