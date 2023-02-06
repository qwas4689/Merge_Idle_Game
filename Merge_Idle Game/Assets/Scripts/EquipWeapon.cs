using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class EquipWeapon : MonoBehaviour
{
    [SerializeField] private Image _equipeWeapopnImage;
    [SerializeField] private SpriteAtlas _spriteAtlas;

    private const string WEAPON = "Weapon_";

    private void OnEnable()
    {
        _equipeWeapopnImage.sprite = _spriteAtlas.GetSprite(WEAPON + Ability.Instance.WeaponLevel);
    }
}
