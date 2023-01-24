using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : SingletonBehaviour<ObjectPool>
{
    [SerializeField] private Ability _ability;
    [SerializeField] private GameObject[] _gameObject;

    private const int WEAPON = 0;

    private GameObject[] _weaponArr;
    public List<GameObject> WeaponPool;


    private void Start()
    {
        _weaponArr = new GameObject[_ability.MaxCanMake + _ability.MaxCanMake];
        WeaponPool = new List<GameObject>(_ability.MaxCanMake + _ability.MaxCanMake);


        _weaponArr = new GameObject[Ability.Instance.MaxHasWeapon];
        WeaponPool = new List<GameObject>(Ability.Instance.MaxHasWeapon);

        for (int i = 0; i < Ability.Instance.MaxHasWeapon; ++i)
        {
            _weaponArr[i] = Instantiate(_gameObject[WEAPON]);
            _weaponArr[i].transform.SetParent(transform);

            _weaponArr[i].SetActive(false);
        }

        WeaponPool.AddRange(_weaponArr);
    }
    
    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        
    }
}
