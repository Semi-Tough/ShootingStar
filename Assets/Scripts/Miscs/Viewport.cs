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
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float middleX;

    private void Start()
    {
        Camera mainCamera = Camera.main;
        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        if (mainCamera)
        {
            bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0));
            topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1));
            middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0, 0)).x;
        }

        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }

    public Vector3 PlayerMobilePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);
        return position;
    }

    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 randomPosition = Vector3.zero;
        randomPosition.x = maxX + paddingX;
        randomPosition.y = Random.Range(minY + paddingY, maxY - paddingY);
        return randomPosition;
    }

    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {
        Vector3 randomPosition = Vector3.zero;
        randomPosition.x = Random.Range(middleX, maxX - paddingX);
        randomPosition.y = Random.Range(minY + paddingY, maxY - paddingY);
        return randomPosition;
    }
}