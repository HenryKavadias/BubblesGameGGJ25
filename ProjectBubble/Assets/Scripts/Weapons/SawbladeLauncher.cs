using UnityEngine;

public class SawbladeLauncher : WeaponParent
{
    void Update()
    {
        if (shootInput.inputState && Time.time >= nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + 1f / FireRate;
        }
    }
}
