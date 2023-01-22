using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteAtlas _spriteAtlas;
    [SerializeField] private Ability _ability;

    void Start()
    {
        _spriteRenderer.sprite = _spriteAtlas.GetSprite("Weapon_" + _ability.WeaponLevel);
    }

    void Update()
    {
        
    }
}
