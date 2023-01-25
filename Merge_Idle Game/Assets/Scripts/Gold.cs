using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Gold : MonoBehaviour , ITextUpdate
{
    [SerializeField] private ClickAndDrop _clickAndDrop;
    [SerializeField] private TextMeshProUGUI _collectGoldText;

    public int CollectMyGold { get; set; }
    private const int GOLD = 1;

    private const string COLLECT_GOLD_TEXT = "Collect Gold : ";

    public UnityEvent CollectGoldEvent = new UnityEvent();

    private void Start()
    {
        CollectGoldEvent.RemoveListener(CollectGold);
        CollectGoldEvent.AddListener(CollectGold);
    }

    private void CollectGold()
    {
        if (_clickAndDrop.EquipWeaponLevel == 0)
        {
            return;
        }

        CollectMyGold += _clickAndDrop.EquipWeaponLevel * GOLD;

        UpdateText(_collectGoldText, COLLECT_GOLD_TEXT, CollectMyGold);
    }

    public void UpdateText(TextMeshProUGUI text, string constStr = "", int? num = null)
    {
        text.text = constStr + num;
    }
}
