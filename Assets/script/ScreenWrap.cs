using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    [Header("画面外に出る余白")]
    public float margin = 0.3f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null)
        {
            return;
        }

        // 画面の左端と右端をワールド座標に変換
        float leftEdge =
            mainCamera.ViewportToWorldPoint(
                new Vector3(0f, 0.5f, 0f)
            ).x;

        float rightEdge =
            mainCamera.ViewportToWorldPoint(
                new Vector3(1f, 0.5f, 0f)
            ).x;

        Vector3 position = transform.position;

        // 右から出たら左へ
        if (position.x > rightEdge + margin)
        {
            position.x = leftEdge - margin;
        }

        // 左から出たら右へ
        else if (position.x < leftEdge - margin)
        {
            position.x = rightEdge + margin;
        }

        transform.position = position;
    }
}