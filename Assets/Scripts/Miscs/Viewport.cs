/****************************************************
    文件：Viewport.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/11 12:04:24
    功能：角色移动范围限制
*****************************************************/
using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;

    private void Start()
    {
        Camera mainCamera = Camera.main;
        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        if (mainCamera)
        {
            bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0));
            topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1));
        }

        _minX = bottomLeft.x;
        _minY = bottomLeft.y;
        _maxX = topRight.x;
        _maxY = topRight.y;
    }

    public Vector3 PlayerMobilePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPosition.x, _minX + paddingX, _maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, _minY + paddingY, _maxY - paddingY);
        return position;
    }
}
