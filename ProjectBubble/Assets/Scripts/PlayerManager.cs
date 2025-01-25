using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private WeaponParent weaponParentController;
    [SerializeField] private GameObject gunHolder;
    
    public void AssignCameraToWeapon(GameObject camera)
    {
        weaponParentController.fpsCam = camera;
    }

    public void RotateGunHolder(float x, float y)
    {
        if (!gunHolder) { return; }
        gunHolder.transform.rotation = Quaternion.Euler(x, y, 0);
    }

    public void HandlePlayerInputs(bool input)
    {
        weaponParentController.HandlePlayerInputs(input);
    }

    public void OnDestroy()
    {
       Destroy(gameObject);
    }
}
