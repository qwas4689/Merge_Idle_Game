using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAndDrop : MonoBehaviour
{

    //private List<GameObject> _weaponSetActiveTrue = new List<GameObject>();
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
            // 1ȸ ��ġ�� ����
            _touch = Input.GetTouch(0);
            // ��ġ�� ���� ������ǥ�� �޾� ��
            _touchPos = Camera.main.ScreenToWorldPoint(_touch.position);

            // ���� UI�� ���� �� �� �ִ� UI�� �̰� ���� �ƴϸ� return


            // ���Ⱑ ���� �� �ִ� ��ġ�� ������ ����
            if (CanMoveArea(_touch))
            {
                // ��ġ ������ ��
                if (_touch.phase == TouchPhase.Began)
                {
                    //for (int i = 0; i < ObjectPool.Instance.transform.childCount; ++i)
                    //{
                    //    if (ObjectPool.Instance.WeaponPool[i].activeSelf == true)
                    //    {
                    //        _weaponSetActiveTrue.Add(ObjectPool.Instance.WeaponPool[i]);
                    //    }
                    //}

                    // ���⸦ ��ġ�ߴ��� Ȯ��
                    for (int i = 0; i < Ability.Instance.NowCanMaskCount + Ability.Instance.MaxHasWeapon; ++i)
                    {
                        if (TouchWeapon(_touchPos, ObjectPool.Instance.WeaponPool[i].transform.position))
                        {
                            _select = ObjectPool.Instance.WeaponPool[i];
                            break;
                        }
                    }


                    EndTouch();
                }

                // �巡�� ����
                if (_touch.phase == TouchPhase.Moved)
                {
                    // ������ ����� �巡�׻��� Ǯ��
                    if (!CanMoveArea(_touch))
                    {
                        EndTouchAndClear();
                    }

                    // ���õȰ� �������� ������
                    if (_select != null)
                    {
                        _select.transform.position = _touchPos;
                    }
                }

                // ��ġ ���� ��
                if (_touch.phase == TouchPhase.Ended)
                {
                    if (_select != null)
                    {
                        // ���� �� ���õ� �Ͱ� ������ ��ġ�� ���Ⱑ ������ ���� ��
                        for (int i = 0; i < Ability.Instance.NowCanMaskCount + Ability.Instance.MaxHasWeapon; ++i)
                        {
                            if (TouchWeapon(_select.transform.position , ObjectPool.Instance.WeaponPool[i].transform.position))
                            {
                                // ������ ���� Ž�� ����
                                if (_select != ObjectPool.Instance.WeaponPool[i])
                                {
                                    // ������ �Ͱ� ���� �ֺ� ������ ���ⷹ���� ������ ������
                                    if (_select.GetComponent<Weapon>().WeaponLevel == ObjectPool.Instance.WeaponPool[i].GetComponent<Weapon>().WeaponLevel)
                                    {
                                        ++ObjectPool.Instance.WeaponPool[i].GetComponent<Weapon>().WeaponLevel;
                                        _select.SetActive(false);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    // ���� ������ ��¶�� ��ġ�� �Ͱ�

                    // ����Ʈ�� ���� �ε����� �ϳ� �÷��ְ�
                    // �װͰ� ������ ���� �¿�Ƽ�� false �� �Ѵ�

                    // �ʱ�ȭ �� ����Ʈ �ʱ�ȭ
                    EndTouchAndClear();
                }
            }
            else
            {
                // �ʱ�ȭ
                Init();
            }
        }
    }

    /// <summary>
    /// ��ġ�ǰ��ִ� ������ ���Ⱑ ���� �� �ִ� ������ true �ƴϸ� false
    /// </summary>
    /// <param name="touchPos">��ġ�� �� ������ǥ</param>
    /// <returns></returns>
    private bool CanMoveArea(Touch touchPos)
    {
        return touchPos.position.x < X_POS_MAX && touchPos.position.x > X_POS_MIN && touchPos.position.y < Y_POS_MAX && touchPos.position.y > Y_POS_MIN;
    }

    /// <summary>
    /// ��ġ�� ��ġ �� ����Ʈ �ʱ�ȭ 
    /// </summary>
    private void Init()
    {
        _touchPos = Vector2.zero;
        //_weaponSetActiveTrue.Clear();
    }

    /// <summary>
    /// ������ ���Ⱑ Ŭ���� ��ġ�� �����ϸ� true �ƴϸ� false
    /// </summary>
    /// <param name="touchPos">��ġ�� ���� ������ǥ</param>
    /// <param name="weaponPos">���� �Ҵ� ��ǥ</param>
    /// <returns></returns>
    private bool TouchWeapon(Vector2 touchPos, Vector2 weaponPos)
    {
        float offset = 0.1f;

        if (touchPos.x - offset < weaponPos.x && weaponPos.x < touchPos.x + offset && touchPos.y - offset < weaponPos.y && weaponPos.y < touchPos.y + offset)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Ŭ������ �� ���õ� ���Ⱑ ������ ȣ��
    /// </summary>
    private void EndTouch()
    {
        if (_select == null)
        {
            Init();
        }
    }

    /// <summary>
    /// ��ġ�� ������ �� ȣ��
    /// </summary>
    private void EndTouchAndClear()
    {
        Init();
        _select = null;
    }
}