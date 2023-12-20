using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private RectTransform crosshairRectTransform;

    public void SetCrosshair(Vector3 worldPosition) {
        if (Camera.main == null) {
            Debug.LogError("Cannot find Camera.main in function " + nameof(SetCrosshair));
            return;
        }
        Vector2 anchoredPosition = Camera.main.WorldToScreenPoint(worldPosition);
        crosshairRectTransform.anchoredPosition = anchoredPosition;
    }
}
