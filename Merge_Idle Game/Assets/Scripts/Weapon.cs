using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteAtlas _spriteAtlas;

    private const string WEAPON = "Weapon_";

    void Start()
    {
        _spriteRenderer.sprite = _spriteAtlas.GetSprite(WEAPON + Ability.Instance.WeaponLevel);
    }
}
