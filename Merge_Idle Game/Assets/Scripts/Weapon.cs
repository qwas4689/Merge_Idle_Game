using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteAtlas _spriteAtlas;

    public int WeaponLevel { get; set; }
    private int _originalLevel;

    private const string WEAPON = "Weapon_";

    void Start()
    {
        _originalLevel = Ability.Instance.WeaponLevel;
        WeaponLevel = _originalLevel;

        _spriteRenderer.sprite = _spriteAtlas.GetSprite(WEAPON + Ability.Instance.WeaponLevel);
    }

    private void Update()
    {
        if (WeaponLevel != _originalLevel)
        {
            _spriteRenderer.sprite = _spriteAtlas.GetSprite(WEAPON + WeaponLevel);
            ++_originalLevel;
        }
    }

    // 어빌리티로 최소 무기 레벨을 올리면 무기의 스프라이트를 바꿔줘야 함 
}
