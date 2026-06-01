using System.Collections;
using UnityEngine;

/// <summary>
/// 在世界坐标中生成 HolyCross 特效的工具类。
/// HolyCross 原始预制体是 UI 元素，需要放在世界空间 Canvas 中才能在场景中渲染。
/// </summary>
public class HolyVfxSpawner : MonoBehaviour
{
    [Header("VFX 设置")]
    [SerializeField] private GameObject holyVfxPrefab;   // HolyCross UI 预制体
    [SerializeField] private float vfxDuration = 2.1f;   // 特效播放时长（秒），约 3 个循环
    [SerializeField] private float canvasSize = 0.01f;   // 世界空间 Canvas 缩放（像素→世界单位）

    /// <summary>
    /// 在指定位置生成 HolyCross 特效。
    /// </summary>
    /// <param name="position">世界坐标位置</param>
    public void SpawnAt(Vector3 position)
    {
        if (holyVfxPrefab == null)
        {
            Debug.LogWarning("HolyVfxSpawner: holyVfxPrefab 未配置。");
            return;
        }

        StartCoroutine(SpawnVfxCoroutine(position));
    }

    private IEnumerator SpawnVfxCoroutine(Vector3 position)
    {
        // 创建世界空间 Canvas
        GameObject canvasObj = new GameObject("HolyVfxCanvas");
        canvasObj.transform.position = position;

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingLayerName = "Default";
        canvas.sortingOrder = 100; // 确保在角色上方渲染

        // 获取 RectTransform 并设置缩放
        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(64f, 64f);

        // 添加 CanvasScaler（世界空间模式下用于控制 DPI 适配）
        var scaler = canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = 1f;

        // 设置 Canvas 的世界空间缩放，使 64x64 像素对应合理的大小
        canvasObj.transform.localScale = Vector3.one * canvasSize;

        // 实例化 HolyCross 特效
        GameObject vfx = Instantiate(holyVfxPrefab, canvasObj.transform);
        vfx.transform.localPosition = Vector3.zero;
        vfx.transform.localScale = Vector3.one;
        vfx.SetActive(true);

        // 等待特效播放完毕
        yield return new WaitForSeconds(vfxDuration);

        // 销毁整个 Canvas（包括特效）
        if (canvasObj != null)
            Destroy(canvasObj);
    }
}
