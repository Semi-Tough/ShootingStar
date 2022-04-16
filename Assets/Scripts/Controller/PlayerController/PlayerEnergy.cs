/****************************************************
    文件：PlayerEnergy.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/15 11:00:12
    功能：玩家能量系统
*****************************************************/

using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] private StatsBarHUD playerEnergyBar;
    private const int MaxEnergy = 100;
    public const int Percent = 1;

    private int currentEnergy;

    private void Start()
    {
        playerEnergyBar.Initialize(currentEnergy, MaxEnergy);
        EnergyObtain(MaxEnergy);
    }

    public void EnergyObtain(int value)
    {
        currentEnergy += value;
        if (currentEnergy >= MaxEnergy)
        {
            currentEnergy = MaxEnergy;
        }

        playerEnergyBar.UpdateStats(currentEnergy, MaxEnergy);
    }

    public void EnergyExpend(int value)
    {
        currentEnergy -= value;
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
        }

        playerEnergyBar.UpdateStats(currentEnergy, MaxEnergy);
    }

    public bool EnergyIsEnough(int value)
    {
        return currentEnergy >= value;
    }
}