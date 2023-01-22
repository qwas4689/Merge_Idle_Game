using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Ability _ability;
    [SerializeField] private GameObject[] _gameObject;

    private const int WEAPON = 0;

    private List<GameObject> _gameObjects;

    private void Start()
    {
        _gameObjects = new List<GameObject>(_ability.MaxCanMake);

        for (int i = 0; i < _ability.NowCanMaskCount; ++i)
        {
            _gameObjects.Add(_gameObject[WEAPON]);
            _gameObjects[i].SetActive(false);
        }
    }
    
    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        
    }
}
