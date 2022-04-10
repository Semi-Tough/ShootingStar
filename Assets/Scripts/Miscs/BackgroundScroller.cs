/****************************************************
    文件：BackgroundScroller.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/10 17:12:58
    功能：滚动背景
*****************************************************/
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Vector2 scrollerVelocity;
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        _material.mainTextureOffset += scrollerVelocity * Time.deltaTime;
    }
}
