using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _createWeaponText;

    private float _time;

    void Update()
    {
        _time += Time.deltaTime * (Ability.Instance.MakeSpeed * 0.01f);

        if (_time > 1f)
        {
            _time -= _time;

            ++Ability.Instance.NowCanMakeCount;
            _createWeaponText.text = Ability.Instance.NowCanMakeCount.ToString() + " / " + Ability.Instance.NowCanMakeMaxCount.ToString();
        }
    }
}
