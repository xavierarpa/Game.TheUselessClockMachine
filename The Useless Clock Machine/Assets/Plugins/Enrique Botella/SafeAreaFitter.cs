using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour {
    private void Awake() {
        RectTransform _rect = GetComponent<RectTransform>();
        Rect _safeArea = Screen.safeArea;
        Vector2 _anchorMin = _safeArea.position;
        Vector2 _anchorMax = _anchorMin + _safeArea.size;

        _anchorMin.x /= Screen.width;
        _anchorMin.y /= Screen.height;

        _anchorMax.x /= Screen.width;
        _anchorMax.y /= Screen.height;

        _rect.anchorMin = _anchorMin;
        _rect.anchorMax = _anchorMax;

        _rect.ForceUpdateRectTransforms();
    }
}