using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    /* 어빌리티 내용
     * 
     * 공격력 int
     * 생산속도 증가 (델타타임에 곱해서 기준으로 생산속도를 증가시킬거임) flaot
     * 제작가능한 레벨
     * 제작가능 최대치 int
     * 보유 가능한 무기 수 int
     * 
     */

    
    /// <summary>
    /// 공격력
    /// </summary>
    public int AttackPower { get; set; }
    
    /// <summary>
    /// 제작 속도
    /// </summary>
    public float MakeSpeed { get; set; }
    
    /// <summary>
    /// 제작 가능한 무기의 레벨
    /// </summary>
    public int WeaponLevel { get; set; }

    /// <summary>
    /// 최대 제작 가능한 무기의 수
    /// </summary>
    public int MaxCanMake { get; private set; } = 20;

    /// <summary>
    /// 지금 제작 가능한 무기의 수
    /// </summary>
    public int NowCanMaskCount { get; set; }

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
        AttackPower = 10;
        MakeSpeed = 0.01f;
        WeaponLevel = 0;
        NowCanMaskCount = 5;
        CanHasWeapon = 10;
    }
}
