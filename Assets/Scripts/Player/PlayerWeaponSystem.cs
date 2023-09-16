using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{
    public WeaponCategory weaponCategory;
    private int _currentWeaponIndex = -1;
    private Weapon _currentWeapon = null;
    [SerializeField] private Transform weaponHoldPoint;
    public float maxWeaponPitchAngleInDegree = 60f;
    public float minWeaponPitchAngleInDegree = -15f;
    public float weaponRotateSpeed = 5f;
    private float _initialWeaponPitchAngleInDegree;

    private void Awake()
    {
        EquipWeapon(0);
    }

    public void EquipWeapon(int index)
    {
        if (weaponCategory == null)
        {
            return;
        }
        if (_currentWeapon != null)
        {
            CancelEquipWeapon();
        }
        if (index >= 0 && index < weaponCategory.WeaponPrefabs.Count)
        {
            _currentWeaponIndex = index;
            GameObject newWeapon = Instantiate(weaponCategory.WeaponPrefabs[_currentWeaponIndex],weaponHoldPoint);
            _currentWeapon = newWeapon.GetComponent<Weapon>();
            _currentWeapon.BeEquipped(transform);
            _initialWeaponPitchAngleInDegree = newWeapon.transform.localRotation.eulerAngles.z;
        }
    }

    public void CancelEquipWeapon()
    {
        if (_currentWeapon == null)
        {
            return;
        }
        _currentWeapon.transform.parent = null;
        _currentWeapon.BeCanceledEquipped();
        _currentWeaponIndex = -1;
        _currentWeapon = null;
    }

    public void AdjustRotation(float inputVertical)
    {
        Transform weaponTransform = _currentWeapon.transform;
        Vector3 currentRotation = weaponTransform.localRotation.eulerAngles;
        float targetPitchAngleInDegree = Mathf.Clamp(currentRotation.z + inputVertical * weaponRotateSpeed,
            _initialWeaponPitchAngleInDegree + minWeaponPitchAngleInDegree,
            _initialWeaponPitchAngleInDegree + maxWeaponPitchAngleInDegree);
        _currentWeapon.transform.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, targetPitchAngleInDegree);
            
    }

    private void AimMouse()
    {
        Vector3 screenPosition = Input.mousePosition;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector2 mouse2DPosition = mousePosition;
        _currentWeapon.Aim(mouse2DPosition);
    }

    public void Shoot()
    {
        if (_currentWeapon)
        {
            _currentWeapon.TryShoot();
        }
    }
}
