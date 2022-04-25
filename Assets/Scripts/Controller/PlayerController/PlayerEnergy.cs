/****************************************************
    文件：PlayerEnergy.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/15 11:00:12
    功能：玩家能量系统
*****************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] private StatsBarHUD playerEnergyBar;
    [SerializeField] private GameObject triggerVFX;
    [SerializeField] private GameObject engineVFXNormal;
    [SerializeField] private GameObject engineVFXOverdrive;

    [SerializeField] private AudioData startSfx;
    [SerializeField] private AudioData stopSfx;

    [SerializeField] private float overdriveInterval = 0.1f;

    public static UnityAction StartOverdriveAction = delegate { };
    public static UnityAction StopOverdriveAction = delegate { };

    private PlayerController playerController;
    private WaitForSeconds waitForOverdrive;
    public const int MaxEnergy = 100;
    public const int Percent = 1;
    private float currentEnergy;

    private void OnEnable()
    {
        StartOverdriveAction += PlayerOverdriveOn;
        StopOverdriveAction += PlayerOverdriveOff;
    }

    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        waitForOverdrive = new WaitForSeconds(overdriveInterval);
        playerEnergyBar.Initialize(currentEnergy, MaxEnergy);
        EnergyObtain(MaxEnergy);
    }

    private void OnDisable()
    {
        StartOverdriveAction -= PlayerOverdriveOn;
        StopOverdriveAction -= PlayerOverdriveOff;
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

    public void EnergyExpend(float value)
    {
        currentEnergy -= value;
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
            if (playerController.isOverdrive)
            {
                StopOverdriveAction.Invoke();
            }
        }

        playerEnergyBar.UpdateStats(currentEnergy, MaxEnergy);
    }

    public bool EnergyIsEnough(float value)
    {
        return currentEnergy >= value;
    }

    private void PlayerOverdriveOn()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomPitch(startSfx);

        StartCoroutine(nameof(OverdriveCoroutine));
    }

    private void PlayerOverdriveOff()
    {
        triggerVFX.SetActive(false);
        engineVFXNormal.SetActive(true);
        engineVFXOverdrive.SetActive(false);
        AudioManager.Instance.PlayRandomPitch(stopSfx);

        StopCoroutine(nameof(OverdriveCoroutine));
    }

    private IEnumerator OverdriveCoroutine()
    {
        while (gameObject.activeSelf && currentEnergy > 0)
        {
            yield return waitForOverdrive;
            EnergyExpend(Percent);
        }
    }
}