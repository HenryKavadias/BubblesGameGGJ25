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

        GameObject ch = Instantiate(cameraHolder, spawnPosition.position, spawnPosition.transform.rotation);
        GameObject pc = Instantiate(playerCharacter, spawnPosition.position, spawnPosition.transform.rotation);

        if (ch && pc && ch.TryGetComponent(out InputHandler ih))
        {
            ih.AssignAndSetupPlayerCharacter(pc);
        }
        ch.GetComponent<InputHandler>().Y_RotationSetStart = spawnPosition.rotation.eulerAngles.y;
    }
}
