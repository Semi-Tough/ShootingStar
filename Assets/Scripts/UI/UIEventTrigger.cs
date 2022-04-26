/****************************************************
    文件：UIEventTrigger.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/26 16:36:32
    功能：UI事件触发器
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, ISelectHandler, ISubmitHandler
{
    [SerializeField] private AudioData selectSfx;
    [SerializeField] private AudioData submitSfx;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySfx(selectSfx);
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySfx(selectSfx);

    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySfx(submitSfx);

    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySfx(submitSfx);

    }
}