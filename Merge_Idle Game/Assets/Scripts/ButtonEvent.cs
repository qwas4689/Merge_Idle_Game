using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonEvent : MonoBehaviour , ITextUpdate
{
    [SerializeField] TextMeshProUGUI[] _abilityTexts;
    [SerializeField] Button[] _buttons;
    [SerializeField] GameObject[] _panels;
    [SerializeField] TextMeshProUGUI _equipWeaponLevelText;
    [SerializeField] ClickAndDrop _clickAndDrop;

    // 무기 생산되는 영역
    private const float X_POS = 2.5f;
    private const float Y_POS_MIN = -2f;
    private const float Y_POS_MAX = 0.1f;

    // 버튼 인덱스
    private const int CREATE_WEAPON_BUTTON = 0;
    private const int ABILITY_UI_BUTTON = 1;
    private const int MERGE_UI_BUTTON = 2;
    private const int STAGE_UI_BUTTON = 3;
    private const int ATTACK_POWER_UP_BUTTON = 4;
    private const int MAKE_SPEED_UP_BUTTON = 5;
    private const int WEAPON_LEVEL_UP_BUTTON = 6;
    private const int NOW_CAN_MAKE_UP_MAX_BUTTON = 7;
    private const int CAN_HAS_WEAPON_UP_BUTTON = 8;
    private const int SORT_BUTTON = 9;

    // 판넬 인덱스
    private const int ABILITY_PANEL = 0;
    private const int STAGE_PANEL = 1;

    // 텍스트 인덱스
    private const int ATTACK_POWER = 0;
    private const int MAKE_SPEED = 1;
    private const int WEAPON_LEVEL = 2;
    private const int NOW_CAN_Max_MAKE = 3;
    private const int CAN_HAS_WEAPON = 4;
    private const int CREATE_WEAPON = 5;
    private const int MAKE_SPEED_UP_BUTTON_TEXT = 6;
    private const int WEAPON_LEVEL_UP_BUTTON_TEXT = 7;
    private const int NOW_CAN_MAKE_UP_MAX_BUTTON_TEXT = 8;
    private const int CAN_HAS_WEAPON_UP_BUTTON_TEXT = 9;


    // 텍스트
    private const string MAX = "Max";
    private const string EQUIP_WEAPON_TEXT = "Equip Weapon Level : ";

    // 어빌리티 올라가는 수치
    private const int UP_VALUE_INT = 1;

    private List<GameObject> _sortList = new List<GameObject>();

    private Vector3 _equipWeaponPos = new Vector3(0f, 0.95f, 0f);
    private int _equipWeaponLevel;

    private const float SORT_POS_X = 1f;
    private const float SORT_POS_Y = -0.8f;
    private Vector2 _pos = new Vector2(-2f, -0.3f);
    private Vector2 _constPos = new Vector2(-2, -0.3f);
    private Vector2 _posX = new Vector2(SORT_POS_X, 0f);
    private Vector2 _posY = new Vector2(-4f, SORT_POS_Y);

    private void Awake()
    {
        foreach (Button button in _buttons)
        {
            button.onClick.RemoveAllListeners();
        }

        _buttons[CREATE_WEAPON_BUTTON].onClick.AddListener(CreateWeapon);
        _buttons[ABILITY_UI_BUTTON].onClick.AddListener(() => MenuButtonClick(STAGE_PANEL, false, ABILITY_PANEL, true));
        _buttons[MERGE_UI_BUTTON].onClick.AddListener(() => MenuButtonClick(STAGE_PANEL, false, ABILITY_PANEL, false));
        _buttons[STAGE_UI_BUTTON].onClick.AddListener(() => MenuButtonClick(STAGE_PANEL, true, ABILITY_PANEL, false));
        _buttons[ATTACK_POWER_UP_BUTTON].onClick.AddListener(() => ClickAttackPowerUpButton(UP_VALUE_INT));
        _buttons[MAKE_SPEED_UP_BUTTON].onClick.AddListener(() => ClickMakeSpeedUPButton(UP_VALUE_INT));
        _buttons[WEAPON_LEVEL_UP_BUTTON].onClick.AddListener(() => ClickWeaponLevelUpButton(UP_VALUE_INT));
        _buttons[NOW_CAN_MAKE_UP_MAX_BUTTON].onClick.AddListener(() => ClickNowCanMakeMaxUpButton(UP_VALUE_INT));
        _buttons[CAN_HAS_WEAPON_UP_BUTTON].onClick.AddListener(() => ClickCanHasWeaponUpButton(UP_VALUE_INT));
        _buttons[SORT_BUTTON].onClick.AddListener(() => ClickSortButton(ref _sortList));
    }

    private void Start()
    {
        UpdateText(_abilityTexts[ATTACK_POWER], string.Empty, Ability.Instance.AttackPower);
        UpdateText(_abilityTexts[MAKE_SPEED], string.Empty, Ability.Instance.MakeSpeed);
        UpdateText(_abilityTexts[WEAPON_LEVEL], string.Empty, Ability.Instance.WeaponLevel);
        UpdateText(_abilityTexts[NOW_CAN_Max_MAKE], string.Empty, Ability.Instance.NowCanMakeCount);
        UpdateText(_abilityTexts[CAN_HAS_WEAPON], string.Empty, Ability.Instance.CanHasWeapon);
        UpdateText(_abilityTexts[CREATE_WEAPON], Ability.Instance.NowCanMakeCount.ToString() + " / " + Ability.Instance.NowCanMakeMaxCount.ToString());


    }

    /// <summary>
    /// 무기 생성 버튼 클릭
    /// </summary>
    private void CreateWeapon()
    {
        if (0 < Ability.Instance.NowCanMakeCount)
        {
            int count = 0;
            for (int i = 0; i < Ability.Instance.MaxHasWeapon; ++i)
            {
                if (ObjectPool.Instance.WeaponPool[i].activeSelf == true)
                {
                    ++count;
                }
            }

            if (count == Ability.Instance.CanHasWeapon)
            {
                return;
            }

            --Ability.Instance.NowCanMakeCount;

            _abilityTexts[CREATE_WEAPON].text = Ability.Instance.NowCanMakeCount.ToString() + " / " + Ability.Instance.NowCanMakeMaxCount.ToString();
        }
        else
        {
            return;
        }

        Vector2 pos = new Vector2(Random.Range(-X_POS, X_POS), Random.Range(Y_POS_MIN, Y_POS_MAX));

        for (int i = 0; i < Ability.Instance.CanHasWeapon; ++i)
        {
            if (ObjectPool.Instance.WeaponPool[i].activeSelf == false)
            {
                ObjectPool.Instance.WeaponPool[i].GetComponent<Weapon>().WeaponLevel = Ability.Instance.WeaponLevel;
                StartCoroutine(MoveLerpWeapon(ObjectPool.Instance.WeaponPool[i], pos));
                break;
            }
        }

        _clickAndDrop.IsSort = false;
    }

    /// <summary>
    /// 무기 생성시 실행되는 코루틴 선형보간
    /// </summary>
    /// <param name="weapon">생성되는 무기</param>
    /// <param name="movePos">선형보간으로 이동할 위치</param>
    /// <returns></returns>
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

    /// <summary>
    /// 메뉴 판넬 변경에 쓰이는 함수
    /// </summary>
    /// <param name="index1">스테이지 판넬</param>
    /// <param name="value1">값</param>
    /// <param name="index2">어빌리티 판넬</param>
    /// <param name="value2">값</param>
    private void MenuButtonClick(int index1, bool value1, int index2, bool value2)
    {
        _panels[index1].SetActive(value1);
        _panels[index2].SetActive(value2);
    }

    /// <summary>
    /// 공격력 업 버튼 클릭
    /// </summary>
    /// <param name="value"></param>
    private void ClickAttackPowerUpButton(int value)
    {
        Ability.Instance.AttackPower += value;

        UpdateText(_abilityTexts[ATTACK_POWER], string.Empty, Ability.Instance.AttackPower);
    }

    /// <summary>
    /// 제작속도 업 버튼 클릭
    /// </summary>
    /// <param name="value"></param>
    private void ClickMakeSpeedUPButton(int value)
    {
        Ability.Instance.MakeSpeed += value;

        if (Ability.Instance.MaxMakeSpeed - value < Ability.Instance.MakeSpeed)
        {
            _buttons[MAKE_SPEED_UP_BUTTON].enabled = false;
            _abilityTexts[MAKE_SPEED_UP_BUTTON_TEXT].text = MAX;
        }

        UpdateText(_abilityTexts[MAKE_SPEED], string.Empty, Ability.Instance.MakeSpeed);
    }

    /// <summary>
    /// 제작 무기 레벨 업 버튼 클릭
    /// </summary>
    /// <param name="value"></param>
    private void ClickWeaponLevelUpButton(int value)
    {
        Ability.Instance.WeaponLevel += value;

        for (int i = 0; i < Ability.Instance.MaxHasWeapon; ++i)
        {
            if (ObjectPool.Instance.WeaponPool[i].GetComponent<Weapon>().WeaponLevel < Ability.Instance.WeaponLevel)
            {
                ObjectPool.Instance.WeaponPool[i].GetComponent<Weapon>().WeaponLevel = Ability.Instance.WeaponLevel;
            }
        }

        if (Ability.Instance.MaxWeaponLevel - value < Ability.Instance.WeaponLevel)
        {
            _buttons[WEAPON_LEVEL_UP_BUTTON].enabled = false;
            _abilityTexts[WEAPON_LEVEL_UP_BUTTON_TEXT].text = MAX;
        }

        UpdateText(_abilityTexts[WEAPON_LEVEL], Ability.Instance.WeaponLevel.ToString());

        if (_clickAndDrop.IsEquip)
        {
            UpdateText(_equipWeaponLevelText, EQUIP_WEAPON_TEXT, Ability.Instance.WeaponLevel);
        }
    }

    /// <summary>
    /// 제작가능한 최대 갯수 버튼 클릭
    /// </summary>
    /// <param name="value"></param>
    private void ClickNowCanMakeMaxUpButton(int value)
    {
        Ability.Instance.NowCanMakeMaxCount += value;

        if (Ability.Instance.MaxCanMake - value < Ability.Instance.NowCanMakeMaxCount)
        {
            _buttons[NOW_CAN_MAKE_UP_MAX_BUTTON].enabled = false;
            _abilityTexts[NOW_CAN_MAKE_UP_MAX_BUTTON_TEXT].text = MAX;
        }

        UpdateText(_abilityTexts[NOW_CAN_Max_MAKE], string.Empty, Ability.Instance.NowCanMakeMaxCount);
        UpdateText(_abilityTexts[CREATE_WEAPON], Ability.Instance.NowCanMakeCount.ToString() + " / " + Ability.Instance.NowCanMakeMaxCount.ToString());
    }

    /// <summary>
    /// 가질 수 있는 무기 수 버튼 클릭
    /// </summary>
    /// <param name="value"></param>
    private void ClickCanHasWeaponUpButton(int value)
    {
        Ability.Instance.CanHasWeapon += value;

        if (Ability.Instance.MaxHasWeapon - value < Ability.Instance.CanHasWeapon)
        {
            _buttons[CAN_HAS_WEAPON_UP_BUTTON].enabled = false;
            _abilityTexts[CAN_HAS_WEAPON_UP_BUTTON_TEXT].text = MAX;
        }

        UpdateText(_abilityTexts[CAN_HAS_WEAPON], string.Empty, Ability.Instance.CanHasWeapon);
    }

    /// <summary>
    /// 정렬 버튼 클릭 
    /// </summary>
    private void ClickSortButton(ref List<GameObject> sortList)
    {
        // 정렬상태인지 아닌지 확인하고 리턴할 수 있게 하자
        if (_clickAndDrop.IsSort)
        {
            return;
        }

        MySort(sortList);

        Debug.Log(_sortList.Count);

        _pos = _constPos;
        _clickAndDrop.IsSort = true;
        _equipWeaponLevel = 0;
        _sortList.Clear();
    }

    /// <summary>
    /// 정렬
    /// </summary>
    /// <param name="sortList">정렬할 리스트</param>
    private void MySort(List<GameObject> sortList)
    {
        for (int i = 0; i < ObjectPool.Instance.WeaponPool.Count; ++i)
        {
            if (ObjectPool.Instance.WeaponPool[i].activeSelf == true)
            {
                sortList.Add(ObjectPool.Instance.WeaponPool[i]);
            }
        }

        _equipWeaponLevel = sortList[0].GetComponent<Weapon>().WeaponLevel;

        for (int i = 0; i < sortList.Count; ++i)
        {
            if (_equipWeaponLevel < sortList[i].GetComponent<Weapon>().WeaponLevel)
            {
                _equipWeaponLevel = sortList[i].GetComponent<Weapon>().WeaponLevel;
            }
        }

        while (true)
        {
            for (int i = 0; i < sortList.Count; ++i)
            {
                if (_equipWeaponLevel == sortList[i].GetComponent<Weapon>().WeaponLevel)
                {
                    sortList[i].transform.position = _pos;

                    _pos = sortList[i].transform.position.x < 2f ? _pos += _posX : _pos += _posY;
                }
            }

            --_equipWeaponLevel;

            if (_equipWeaponLevel == Ability.Instance.WeaponLevel -1)
            {
                break;
            }
        }
    }

    public void UpdateText(TextMeshProUGUI text, string constStr = "", int? num = null)
    {
        text.text = constStr + num;
    }

}