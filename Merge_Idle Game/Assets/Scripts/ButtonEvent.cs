using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    [SerializeField] Button[] _buttons;
    [SerializeField] RectTransform _weponArea;

    private const float X_POS = 2.5f;
    private const float Y_POS_MIN = -2f;
    private const float Y_POS_MAX = 1f;

    private const int CREATE_WEAPON_BUTTON = 0;

    private int _weaponCounts = -1;
    public int WeaponCounts { get { return _weaponCounts; } set { _weaponCounts = value; } }

    private IEnumerator _moveLerpWeapon;

    void Awake()
    {
        foreach (Button button in _buttons)
        {
            button.onClick.RemoveAllListeners();
        }

        _buttons[CREATE_WEAPON_BUTTON].onClick.AddListener(CreateWeapon);
    }

    private void CreateWeapon()
    {
        if (_weaponCounts < Ability.Instance.NowCanMaskCount - 1)
        {
            _weaponCounts = (_weaponCounts + 1) % ObjectPool.Instance.WeaponPool.Count;
        }
        else
        {
            return;
        }

        Vector2 pos = new Vector2(Random.Range(-X_POS, X_POS), Random.Range(Y_POS_MIN, Y_POS_MAX));

        StartCoroutine(MoveLerpWeapon(ObjectPool.Instance.WeaponPool[_weaponCounts], pos));
    }

    private IEnumerator MoveLerpWeapon(GameObject weapon, Vector3 movePos)
    {
        weapon.SetActive(true);
        float t = 0.01f;

        while (t < 1f)
        {
            weapon.transform.position = Vector3.Lerp(weapon.transform.position, movePos, 0.01f);

            t += 0.01f;

            yield return null;
        }

        yield return null;
    }
}
