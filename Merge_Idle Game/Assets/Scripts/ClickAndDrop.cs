using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAndDrop : MonoBehaviour
{
    private List<GameObject> _weaponSetActiveTrue = new List<GameObject>();
    private Vector2 _weaponPos;
    private Touch _touch;
    private GameObject _select;

    private const float X_POS_MIN = 10f;
    private const float X_POS_MAX = 1070f;
    private const float Y_POS_MIN = 350f;
    private const float Y_POS_MAX = 1150f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
             _touch = Input.GetTouch(0);
            _weaponPos = Camera.main.ScreenToWorldPoint(_touch.position);

            // 지금 UI가 생성 할 수 있는 UI면 이거 실행 아니면 return

            
            // 무기가 있을 수 있는 위치만 터지를 받음
            if (CanMoveArea(_touch))
            {
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
                            _weaponSetActiveTrue.Add(ObjectPool.Instance.WeaponPool[i]);
                        }
                    }

                    float offset = 0.1f;

                    for (int i = 0; i < _weaponSetActiveTrue.Count; ++i)
                    {
                        if (_weaponPos.x - offset < _weaponSetActiveTrue[i].transform.position.x && _weaponSetActiveTrue[i].transform.position.x > _weaponPos.x - offset && _weaponPos.y - offset < _weaponSetActiveTrue[i].transform.position.y && _weaponSetActiveTrue[i].transform.position.y > _weaponPos.y - offset)
                        {
                            _select = _weaponSetActiveTrue[i];
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    EndTouch();
                }

                if (_touch.phase == TouchPhase.Moved)
                {
                    if (!CanMoveArea(_touch))
                    {
                        EndTouchAndClear();
                        return;
                    }
                    if (_select != null)
                    {
                        _select.transform.position = _weaponPos;
                    }
                }

                if (_touch.phase == TouchPhase.Ended)
                {
                    EndTouchAndClear();
                }
            }
            else
            {
                Init();
                return;
            }
        }
    }

    private bool CanMoveArea(Touch touchPos)
    {
        return touchPos.position.x < X_POS_MAX && touchPos.position.x > X_POS_MIN && touchPos.position.y < Y_POS_MAX && touchPos.position.y > Y_POS_MIN;
    }

    /// <summary>
    /// 초기화
    /// </summary>
    private void Init()
    {
        _weaponPos = Vector2.zero;
        _weaponSetActiveTrue.Clear();
    }


    private void EndTouch()
    {
        if (_select == null)
        {
            Init();
            return;
        }
    }

    private void EndTouchAndClear()
    {
        Init();
        _select = null;
    }
}