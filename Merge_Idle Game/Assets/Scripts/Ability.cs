using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    /* �����Ƽ ����
     * 
     * ���ݷ� int
     * ����ӵ� ���� (��ŸŸ�ӿ� ���ؼ� �������� ����ӵ��� ������ų����) flaot
     * ���۰����� ����
     * ���۰��� �ִ�ġ int
     * ���� ������ ���� �� int
     * 
     */

    
    /// <summary>
    /// ���ݷ�
    /// </summary>
    public int AttackPower { get; set; }
    
    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    public float MakeSpeed { get; set; }
    
    /// <summary>
    /// ���� ������ ������ ����
    /// </summary>
    public int WeaponLevel { get; set; }

    /// <summary>
    /// �ִ� ���� ������ ������ ��
    /// </summary>
    public int MaxCanMake { get; private set; } = 20;

    /// <summary>
    /// ���� ���� ������ ������ ��
    /// </summary>
    public int NowCanMaskCount { get; set; }

    /// <summary>
    /// ���� �� �ִ� ������ ��
    /// </summary>
    public int CanHasWeapon { get; set; }



    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        // playerPrefs�� ����� �����͸� �ҷ�����

        // PlayerPrefs �� �����Ͱ� ���� �� ����
        AttackPower = 10;
        MakeSpeed = 0.01f;
        WeaponLevel = 0;
        NowCanMaskCount = 5;
        CanHasWeapon = 10;
    }
}
