using UnityEngine;

public class SpawnPlayerOnStart : MonoBehaviour
{
    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private GameObject playerCharacter;

    [SerializeField] private Transform spawnPosition;

    private void Start()
    {
        PoolManager.ClearPools();   
        if (!cameraHolder || !playerCharacter) { return; }

        GameObject ch = Instantiate(cameraHolder, spawnPosition.position, transform.rotation);
        GameObject pc = Instantiate(playerCharacter, spawnPosition.position, transform.rotation);

        if (ch && pc && ch.TryGetComponent(out InputHandler ih))
        {
            ih.AssignAndSetupPlayerCharacter(pc);
        }
    }
}
