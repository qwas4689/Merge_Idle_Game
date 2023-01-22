using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAndDrop : MonoBehaviour
{
    private List<GameObject> _weaponSetActiveTrue = new List<GameObject>();
    private Vector2 _touchPos;
    private Touch _touch;
    private GameObject _select;

    private const float X_POS_MIN = 10f;
    private const float X_POS_MAX = 1070f;
    private const float Y_POS_MIN = 350f;
    private const float Y_POS_MAX = 1150f;

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
                    for (int i = 0; i < ObjectPool.Instance.WeaponPool.Count; ++i)
                    {
                        if (ObjectPool.Instance.WeaponPool[i].activeSelf == false)
                        {
                            break;
                        }
                        else
                        {
                            // 활성화 되어 있는 무기만 리스트에 추가
                            _weaponSetActiveTrue.Add(ObjectPool.Instance.WeaponPool[i]);
                        }
                    }

                    // 무기를 터치했는지 확인
                    for (int i = 0; i < _weaponSetActiveTrue.Count; ++i)
                    {
                        if (TouchWeapon(_touchPos, i))
                        {
                            _select = _weaponSetActiveTrue[i];
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
                    // 땠을 때 선택된 것과 인접한 위치에 무기가 있으면 머지 함
                    for (int i = 0; i < _weaponSetActiveTrue.Count; ++i)
                    {
                        if (TouchWeapon(_select.transform.position, i))
                        {
                            ObjectPool.Instance.WeaponPool[i].SetActive(false);
                            ++_select.GetComponent<Weapon>().WeaponLevel;
                        }
                    }

                    // 셀렉트의 무기 인덱스를 하나 올려주고
                    // 그것과 인접한 것을 셋엑티프 false 로 한다

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
        _weaponSetActiveTrue.Clear();
    }

    /// <summary>
    /// 선택한 무기가 클릭한 위치에 인접하면 true 아니면 false
    /// </summary>
    /// <param name="i">리스트의 인덱스</param>
    /// <returns></returns>
    private bool TouchWeapon(Vector2 touchPos, int i)
    {
        float offset = 0.1f;

        return touchPos.x - offset < _weaponSetActiveTrue[i].transform.position.x && _weaponSetActiveTrue[i].transform.position.x < touchPos.x + offset && touchPos.y - offset < _weaponSetActiveTrue[i].transform.position.y && _weaponSetActiveTrue[i].transform.position.y < touchPos.y + offset;
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