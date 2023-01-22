using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAndDrop : MonoBehaviour
{
    private List<GameObject> _weaponSetActiveTrue = new List<GameObject>();
    private Vector2 _weaponPos;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
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
            }

            if (touch.phase == TouchPhase.Moved)
            {
                
            }

            if (touch.phase == TouchPhase.Ended)
            {
                _weaponSetActiveTrue.Clear();
            }
        }
    }
}
