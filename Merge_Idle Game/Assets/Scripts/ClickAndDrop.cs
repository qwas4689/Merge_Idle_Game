using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ClickAndDrop : MonoBehaviour, ITextUpdate
{
    [SerializeField] ButtonEvent _buttonEvent;
    [SerializeField] GameObject[] _panels;
    [SerializeField] Transform _equipWeapon;
    [SerializeField] TextMeshProUGUI _equipWeaponLevelText;

    private Vector2 _touchPos;
    private Touch _touch;
    private GameObject _select;

    private const float X_POS_MIN = 10f;
    private const float X_POS_MAX = 1070f;
    private const float Y_POS_MIN = 350f;
    private const float Y_POS_MAX = 1150f;

    private const float TOUCH_OFFSET = 0.1f;
    private const float MERGE_OFFEST = 0.45f;
    private const float EQUIP_OFFEST = 0.45f;

    private const int ABILITY_PANEL = 0;
    private const int STAGE_PANEL = 1;

    private const int NONE_WEAPON = 0;

    private const string EQUIP_WEAPON_TEXT = "Equip Weapon Level : ";

    private Vector2 _startPos = new Vector2(0, -3.3f);

    // 무기를 장착중이면 true 아니면 false 인 프로퍼티
    public bool IsEquip { get; private set; }

    // 소팅한 상태면 true 아니면 false 인 프로퍼티
    public bool IsSort { get; set; }

    // 장착한 무기의 레벨
    public int EquipWeaponLevel { get; set; }


    private void Update()
    {
        if (_panels[ABILITY_PANEL].activeSelf == true || _panels[STAGE_PANEL].activeSelf == true)
        {
            return;
        }

        // 지금 머지 메뉴창일 때 만

        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            _touchPos = Camera.main.ScreenToWorldPoint(_touch.position);

            if (CanMoveArea(_touch))
            {
                switch (_touch.phase)
                {
                    case TouchPhase.Began:

                        TouchScreen();

                        break;

                    case TouchPhase.Moved:

                        DragScreen();

                        break;

                    case TouchPhase.Ended:

                        TouchOffScreen();

                        break;
                }
            }
            else
            {
                Init();
            }
        }

    }

    /// <summary>
    /// 터치되고있는 영역이 무기가 있을 수 있는 범위면 true 아니면 false
    /// </summary>
    /// <param name="touchPos">터치한 곳 월드좌표</param>
    /// <returns></returns>
    private bool CanMoveArea(Touch touchPos)
    {
        return touchPos.position.x < X_POS_MAX && touchPos.position.x > X_POS_MIN && touchPos.position.y < Y_POS_MAX && touchPos.position.y > Y_POS_MIN;
    }

    /// <summary>
    /// 터치한 위치 및 리스트 초기화 
    /// </summary>
    private void Init()
    {
        _touchPos = Vector2.zero;
    }

    /// <summary>
    /// 선택한 무기가 클릭한 위치에 인접하면 true 아니면 false
    /// </summary>
    /// <param name="touchPos">터치한 곳의 월드좌표</param>
    /// <param name="goalPos">목적지 좌표</param>
    /// <returns></returns>
    private bool TouchWeapon(Vector2 touchPos, Vector2 goalPos, float offset)
    {
        if (touchPos.x - offset < goalPos.x && goalPos.x < touchPos.x + offset && touchPos.y - offset < goalPos.y && goalPos.y < touchPos.y + offset)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 클릭했을 때 선택된 무기가 없으면 호출
    /// </summary>
    private void EndTouch()
    {
        if (_select == null)
        {
            Init();
        }
    }

    /// <summary>
    /// 터치가 끝났을 때 호출
    /// </summary>
    private void EndTouchAndClear()
    {
        Init();
        _select = null;
    }

    public void UpdateText(TextMeshProUGUI text, string constStr = "", int? num = null)
    {
        text.text = constStr + num;
    }

    /// <summary>
    /// 터치를 했을 때 실행
    /// </summary>
    private void TouchScreen()
    {
        for (int i = 0; i < Ability.Instance.MaxHasWeapon; ++i)
        {
            if (TouchWeapon(_touchPos, ObjectPool.Instance.WeaponPool[i].transform.position, TOUCH_OFFSET))
            {
                _select = ObjectPool.Instance.WeaponPool[i];
                break;
            }
        }

        EndTouch();
    }

    /// <summary>
    /// 드래그 중일 때 실행
    /// </summary>
    private void DragScreen()
    {
        // 영역을 벗어나면 드래그상태 풀림
        if (!CanMoveArea(_touch))
        {
            EndTouchAndClear();
        }

        // 선택된게 있을때만 움직임
        if (_select != null)
        {
            _select.transform.position = _touchPos;
        }
    }

    /// <summary>
    /// 터치가 끝났을 때 실행
    /// </summary>
    private void TouchOffScreen()
    {
        if (_select == null)
        {
            EndTouchAndClear();
            return;
        }

        if (_select.GetComponent<Weapon>().WeaponLevel == Ability.Instance.MaxWeaponLevel)
        {
            return;
        }

        // 땠을 때 선택된 것과 인접한 위치에 무기가 있으면 머지 함
        for (int i = 0; i < Ability.Instance.MaxHasWeapon; ++i)
        {
            if (TouchWeapon(_select.transform.position, ObjectPool.Instance.WeaponPool[i].transform.position, MERGE_OFFEST))
            {
                // 선택한 것은 탐색 제외
                if (_select != ObjectPool.Instance.WeaponPool[i])
                {
                    // 선택한 것과 영역 주변 영역의 무기레벨이 같으면 레벨업
                    if (_select.GetComponent<Weapon>().WeaponLevel == ObjectPool.Instance.WeaponPool[i].GetComponent<Weapon>().WeaponLevel)
                    {
                        ++ObjectPool.Instance.WeaponPool[i].GetComponent<Weapon>().WeaponLevel;
                        _select.transform.position = _startPos;
                        _select.SetActive(false);
                        break;
                    }
                }
            }
        }

        // 장비장착칸의 근처에서 터치를 땠는지
        if (TouchWeapon(_select.transform.position, _equipWeapon.position, EQUIP_OFFEST) && !IsEquip)
        {
            _select.transform.position = _equipWeapon.position;

            UpdateEquipWeaponText(_select.GetComponent<Weapon>().WeaponLevel, true);
        }
        else
        {
            for (int i = 0; i < Ability.Instance.MaxHasWeapon; ++i)
            {
                if (ObjectPool.Instance.WeaponPool[i].transform.position == _equipWeapon.position)
                {
                    UpdateEquipWeaponText(ObjectPool.Instance.WeaponPool[i].GetComponent<Weapon>().WeaponLevel, true);

                    break;
                }

                EquipWeaponLevel = NONE_WEAPON;

                UpdateText(_equipWeaponLevelText);
                IsEquip = false;
            }
        }

        // 무기를 옮긴거니 소트를 할 수 있음
        IsSort = false;


        // 초기화 및 리스트 초기화
        EndTouchAndClear();
    }

    /// <summary>
    /// 장착한 장비의 텍스트 최신화
    /// </summary>
    /// <param name="weaponLevel">바꿀 무기의 레벨</param>
    /// <param name="isEquip">장착여부</param>
    private void UpdateEquipWeaponText(int weaponLevel, bool isEquip)
    {
        UpdateText(_equipWeaponLevelText, EQUIP_WEAPON_TEXT, weaponLevel);

        EquipWeaponLevel = weaponLevel;

        IsEquip = isEquip;
    }
}