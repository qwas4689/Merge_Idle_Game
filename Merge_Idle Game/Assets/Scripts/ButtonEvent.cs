using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    [SerializeField] Button[] _buttons;

    private const float X_POS = 2.5f;
    private const float Y_POS_MIN = -2f;
    private const float Y_POS_MAX = 1f;

    private const int CREATE_WEAPON_BUTTON = 0;

    public int WeaponCounts { get; set; } = -1;

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
        if (WeaponCounts < Ability.Instance.NowCanMaskCount - 1)
        {
            WeaponCounts = (WeaponCounts + 1) % ObjectPool.Instance.WeaponPool.Count;
        }
        else
        {
            return;
        }

        Vector2 pos = new Vector2(Random.Range(-X_POS, X_POS), Random.Range(Y_POS_MIN, Y_POS_MAX));

        for (int i = 0; i < Ability.Instance.NowCanMaskCount; ++i)
        {
            if (ObjectPool.Instance.WeaponPool[i].activeSelf == false)
            {
                StartCoroutine(MoveLerpWeapon(ObjectPool.Instance.WeaponPool[i], pos));
                break;
            }
        }

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
