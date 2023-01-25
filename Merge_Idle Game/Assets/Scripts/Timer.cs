using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour , ITextUpdate
{
    [SerializeField] private TextMeshProUGUI _createWeaponText;
    [SerializeField] private Gold _gold;

    private float _makeTime;
    private float _collectTime;

    void Update()
    {
        _makeTime += Time.deltaTime * (Ability.Instance.MakeSpeed * 0.01f);
        _collectTime += Time.deltaTime;

        MakeTimeDone();

        CollectTimeDone();
    }

    /// <summary>
    /// 무기 생산 가능 횟수 증가 텍스드
    /// </summary>
    private void MakeTimeDone()
    {
        if (_makeTime > 1f)
        {
            _makeTime -= _makeTime;

            if (Ability.Instance.NowCanMakeCount != Ability.Instance.NowCanMakeMaxCount)
            {
                ++Ability.Instance.NowCanMakeCount;
            }

            UpdateText(_createWeaponText, Ability.Instance.NowCanMakeCount.ToString() + " / " + Ability.Instance.NowCanMakeMaxCount.ToString());
        }
    }

    /// <summary>
    /// 골드 생산
    /// </summary>
    private void CollectTimeDone()
    {
        if (_collectTime < 60f)
        {
            _collectTime -= _collectTime;

            _gold.CollectGoldEvent.Invoke();
        }
    }

    public void UpdateText(TextMeshProUGUI text, string constStr = "", int? num = null)
    {
        text.text = constStr + num;
    }
}
