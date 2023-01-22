using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    [SerializeField] Button[] _buttons;

    private const int CREATE_WEAPON_BUTTON = 0;

    void Awake()
    {
        foreach (Button button in _buttons)
        {
            button.onClick.RemoveAllListeners();
        }

        _buttons[CREATE_WEAPON_BUTTON].onClick.AddListener(CreateWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateWeapon()
    {
        Debug.Log("Asd");
    }

}
