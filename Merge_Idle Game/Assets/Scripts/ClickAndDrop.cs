﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAndDrop : MonoBehaviour
{
    [SerializeField] ButtonEvent _buttonEvent;

    private Vector2 _touchPos;
    private Touch _touch;
    private GameObject _select;

    private const float X_POS_MIN = 10f;
    private const float X_POS_MAX = 1070f;
    private const float Y_POS_MIN = 350f;
    private const float Y_POS_MAX = 1150f;

    private const float TOUCH_OFFSET = 0.1f;
    private const float MERGE_OFFEST = 0.5F;

    private Vector2 _startPos = new Vector2(0, -3.3f);

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            // 1회 터치만 받음
            _touch = Input.GetTouch(0);
            // 터치한 곳을 월드좌표로 받아 옴
            _touchPos = Camera.main.ScreenToWorldPoint(_touch.position);

            // 지금 UI가 생성 할 수 있는 UI면 이거 실행 아니면 return


            // 무기가 있을 수 있는 위치만 터지를 받음
            if (CanMoveArea(_touch))
            {
                // 터치 눌렀을 때
                if (_touch.phase == TouchPhase.Began)
                {
                    // 무기를 터치했는지 확인
                    for (int i = 0; i < Ability.Instance.NowCanMaskCount + Ability.Instance.MaxHasWeapon; ++i)
                    {
                        if (TouchWeapon(_touchPos, ObjectPool.Instance.WeaponPool[i].transform.position, TOUCH_OFFSET))
                        {
                            _select = ObjectPool.Instance.WeaponPool[i];
                            break;
                        }
                    }

                    EndTouch();
                }

                // 드래그 상태
                if (_touch.phase == TouchPhase.Moved)
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

                // 터치 땠을 때
                if (_touch.phase == TouchPhase.Ended)
                {
                    if (_select != null)
                    {
                        // 땠을 때 선택된 것과 인접한 위치에 무기가 있으면 머지 함
                        for (int i = 0; i < Ability.Instance.NowCanMaskCount + Ability.Instance.MaxHasWeapon; ++i)
                        {
                            if (TouchWeapon(_select.transform.position , ObjectPool.Instance.WeaponPool[i].transform.position, MERGE_OFFEST))
                            {
                                // 선택한 것은 탐색 제외
                                if (_select != ObjectPool.Instance.WeaponPool[i])
                                {
                                    // 선택한 것과 영역 주변 영역의 무기레벨이 같으면 레벨업
                                    if (_select.GetComponent<Weapon>().WeaponLevel == ObjectPool.Instance.WeaponPool[i].GetComponent<Weapon>().WeaponLevel)
                                    {
                                        ++ObjectPool.Instance.WeaponPool[i].GetComponent<Weapon>().WeaponLevel;
                                        _select.transform.position = _startPos;
                                        _select.GetComponent<Weapon>().WeaponLevel = Ability.Instance.WeaponLevel;
                                        _select.SetActive(false);
                                        --_buttonEvent.WeaponCounts;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 초기화 및 리스트 초기화
                    EndTouchAndClear();
                }
            }
            else
            {
                // 초기화
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
    /// <param name="weaponPos">무기 았는 좌표</param>
    /// <returns></returns>
    private bool TouchWeapon(Vector2 touchPos, Vector2 weaponPos, float offset)
    {
        if (touchPos.x - offset < weaponPos.x && weaponPos.x < touchPos.x + offset && touchPos.y - offset < weaponPos.y && weaponPos.y < touchPos.y + offset)
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
}