using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoScrollSnapAndScene : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;
    public RectTransform content;         // planets container
    public RectTransform viewport;        // Viewport
    public float snapSpeed = 10f;

    private bool isDragging = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        StartCoroutine(SnapToClosestItem());
    }

    IEnumerator SnapToClosestItem()
    {
        yield return new WaitForEndOfFrame(); // wait for scroll to settle

        Vector3 viewportCenter = viewport.TransformPoint(new Vector3(viewport.rect.width / 2, 0, 0));

        float closestDistance = Mathf.Infinity;
        RectTransform closestItem = null;

        foreach (RectTransform child in content)
        {
            Vector3 childCenter = child.TransformPoint(child.rect.center);
            float distance = Mathf.Abs(childCenter.x - viewportCenter.x);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestItem = child;
            }
        }

        if (closestItem != null)
        {
            // target anchored position
            Vector2 difference = (Vector2)viewport.InverseTransformPoint(viewportCenter) - (Vector2)viewport.InverseTransformPoint(closestItem.position);
            Vector2 targetPos = content.anchoredPosition + difference;

            Vector2 startPos = content.anchoredPosition;
            float t = 0;

            while (t < 1f)
            {
                t += Time.deltaTime * snapSpeed;
                content.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                yield return null;
            }

            // Optional: Load scene using button text or other logic
            string sceneName = closestItem.name;
            if (!string.IsNullOrEmpty(sceneName))
            {
                Debug.Log("Snap done. Ready to load: " + sceneName);
                // SceneManager.LoadScene(sceneName);
            }
        }
    }
}
