using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonEvent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] _abilityTexts;
    [SerializeField] Button[] _buttons;
    [SerializeField] GameObject[] _panels;

    private const float X_POS = 2.5f;
    private const float Y_POS_MIN = -2f;
    private const float Y_POS_MAX = 1f;

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

    private const int UP_VALUE_INT = 1;
    private const float UP_VALUE_FLOAT = 0.01f;


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

    private const string MAX = "Max";

    //public int WeaponCounts { get; set; } = -1;

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
        _buttons[MAKE_SPEED_UP_BUTTON].onClick.AddListener(() => ClickMakeSpeedUPButton(UP_VALUE_FLOAT));
        _buttons[WEAPON_LEVEL_UP_BUTTON].onClick.AddListener(() => ClickWeaponLevelUpButton(UP_VALUE_INT));
        _buttons[NOW_CAN_MAKE_UP_MAX_BUTTON].onClick.AddListener(() => ClickNowCanMakeMaxUpButton(UP_VALUE_INT));
        _buttons[CAN_HAS_WEAPON_UP_BUTTON].onClick.AddListener(() => ClickCanHasWeaponUpButton(UP_VALUE_INT));
    }

    private void Start()
    {
        _abilityTexts[ATTACK_POWER].text = Ability.Instance.AttackPower.ToString();
        _abilityTexts[MAKE_SPEED].text = Ability.Instance.MakeSpeed.ToString();
        _abilityTexts[WEAPON_LEVEL].text = Ability.Instance.WeaponLevel.ToString();
        _abilityTexts[NOW_CAN_Max_MAKE].text = Ability.Instance.NowCanMakeCount.ToString();
        _abilityTexts[CAN_HAS_WEAPON].text = Ability.Instance.CanHasWeapon.ToString();
        _abilityTexts[CREATE_WEAPON].text = Ability.Instance.NowCanMakeCount.ToString() + " / " + Ability.Instance.NowCanMakeMaxCount.ToString();
    }

    /// <summary>
    /// 무기 생성
    /// </summary>
    private void CreateWeapon()
    {
        if (0 < Ability.Instance.NowCanMakeCount)
        {
            int count = 0;
            for (int i = 0; i < Ability.Instance.NowCanMakeCount + Ability.Instance.MaxHasWeapon; ++i)
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
    /// 공격력 업
    /// </summary>
    /// <param name="value"></param>
    private void ClickAttackPowerUpButton(int value)
    {
        Ability.Instance.AttackPower += value;
        _abilityTexts[ATTACK_POWER].text = Ability.Instance.AttackPower.ToString();
    }

    /// <summary>
    /// 제작속도 업
    /// </summary>
    /// <param name="value"></param>
    private void ClickMakeSpeedUPButton(float value)
    {
        Ability.Instance.MakeSpeed += value;

        if (Ability.Instance.MaxMakeSpeed - value < Ability.Instance.MakeSpeed)
        {
            _buttons[MAKE_SPEED_UP_BUTTON].enabled = false;
            _abilityTexts[MAKE_SPEED_UP_BUTTON_TEXT].text = MAX;
        }

        _abilityTexts[MAKE_SPEED].text = Ability.Instance.MakeSpeed.ToString();
    }

    /// <summary>
    /// 제작 무기 레벨 업
    /// </summary>
    /// <param name="value"></param>
    private void ClickWeaponLevelUpButton(int value)
    {
        Ability.Instance.WeaponLevel += value;

        for (int i = 0; i < Ability.Instance.NowCanMakeCount - value + Ability.Instance.MaxHasWeapon; ++i)
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

        _abilityTexts[WEAPON_LEVEL].text = Ability.Instance.WeaponLevel.ToString();
    }

    /// <summary>
    /// 제작가능한 최대 갯수
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

        _abilityTexts[NOW_CAN_Max_MAKE].text = Ability.Instance.NowCanMakeMaxCount.ToString();
        _abilityTexts[CREATE_WEAPON].text = Ability.Instance.NowCanMakeCount.ToString() + " / " + Ability.Instance.NowCanMakeMaxCount.ToString();
    }

    /// <summary>
    /// 가질 수 있는 무기 수
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

        _abilityTexts[CAN_HAS_WEAPON].text = Ability.Instance.CanHasWeapon.ToString();
    }
}