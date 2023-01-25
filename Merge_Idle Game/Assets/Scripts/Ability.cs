using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : SingletonBehaviour<Ability>
{
    /// <summary>
    /// 공격력
    /// </summary>
    public int AttackPower { get; set; }
    
    /// <summary>
    /// 제작 속도
    /// </summary>
    public int MakeSpeed { get; set; }

    public int MaxMakeSpeed { get; private set; } = 30;
    
    /// <summary>
    /// 제작 가능한 무기의 레벨
    /// </summary>
    public int WeaponLevel { get; set; }

    public int MaxWeaponLevel { get; private set; } = 42;

    /// <summary>
    /// 최대 제작 가능한 무기의 수
    /// </summary>
    public int MaxCanMake { get; private set; } = 20;

    /// <summary>
    /// 지금 제작 가능한 무기의 수
    /// </summary>
    public int NowCanMakeCount { get; set; }

    /// <summary>
    /// 지금 제작 가능한 최대의 무기 수
    /// </summary>
    public int NowCanMakeMaxCount { get; set; }

    /// <summary>
    /// 최대 가질 수 있는 무기의 수
    /// </summary>
    public int MaxHasWeapon { get; private set; } = 20;

    /// <summary>
    /// 가질 수 있는 무기의 수
    /// </summary>
    public int CanHasWeapon { get; set; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        // playerPrefs에 저장된 데이터를 불러오기

        // PlayerPrefs 에 데이터가 없을 시 실행
        AttackPower = 1;
        MakeSpeed = 1;
        WeaponLevel = 0;
        NowCanMakeCount = 5;
        NowCanMakeMaxCount = 5;
        CanHasWeapon = 5;
    }
}
