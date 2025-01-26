using UnityEngine;
using UnityEngine.Events;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private WeaponParent[] weaponParentController;
    [SerializeField] private GameObject gunHolder;
    private int currentWeapon = 0;
    private InputDetector swapWeaponInput;
    
    public UnityEvent OnSwap;

    public void AssignCameraToWeapon(GameObject camera)
    {
        for (int i = 0; i < weaponParentController.Length; i++)
        {
            weaponParentController[i].fpsCam = camera;
        }
        
    }

    public void RotateGunHolder(float x, float y)
    {
        if (!gunHolder) { return; }
        gunHolder.transform.rotation = Quaternion.Euler(x, y, 0);
    }

    public void HandlePlayerInputs(bool shootInput, bool swapInput)
    {
        swapWeaponInput.inputState = swapInput;

        weaponParentController[currentWeapon].HandlePlayerInputs(shootInput);
    }

    private void Update()
    {
        int inputHasChanged = swapWeaponInput.HasStateChanged();

        // weapon swap
        if (inputHasChanged == 0)
        {
            if (currentWeapon + 1 >= weaponParentController.Length)
            {
                weaponParentController[currentWeapon].gameObject.SetActive(false);
                currentWeapon = 0;
                weaponParentController[currentWeapon].gameObject.SetActive(true);

            }
            else
            {
                weaponParentController[currentWeapon].gameObject.SetActive(false);
                currentWeapon++;
                weaponParentController[currentWeapon].gameObject.SetActive(true);

            }

            OnSwap?.Invoke();
        }
    }

    public void OnDestroy()
    {
       Destroy(gameObject, 0.75f);
    }
}
