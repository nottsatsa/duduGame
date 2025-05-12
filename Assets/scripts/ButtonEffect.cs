using UnityEngine;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour
{
    private Vector3 originalScale;
    public float scaleFactor = 1.2f;
    public float duration = 0.1f;

    void Start()
    {
        originalScale = transform.localScale;

        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => StartCoroutine(ClickAnimation()));
        }
    }

    System.Collections.IEnumerator ClickAnimation()
    {
        transform.localScale = originalScale * scaleFactor;

        yield return new WaitForSeconds(duration);

        transform.localScale = originalScale;
    }
}
