using UnityEngine;
using UnityEngine.UI;

public class swipePlanetsBtn : MonoBehaviour
{
    public GameObject scrollbar;
    public float lerpSpeed = 10f; // Make this configurable
    public float snapThreshold = 0.01f; // When to stop lerping
    
    private float scroll_pos = 0f;
    private float[] pos;
    private bool isLerping = false;
    private float targetPos;

    void Start()
    {
        InitializePositions();
    }

    void InitializePositions()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
    }

    void Update()
    {
        // Handle input
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            isLerping = false;
        }
        
        // Handle automatic snapping
        if (!isLerping && !Input.GetMouseButton(0) // Not dragging
            && (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Moved)) // Not touching
        {
            FindNearestPosition();
        }

        if (isLerping)
        {
            float currentValue = scrollbar.GetComponent<Scrollbar>().value;
            float newValue = Mathf.Lerp(currentValue, targetPos, lerpSpeed * Time.deltaTime);
            scrollbar.GetComponent<Scrollbar>().value = newValue;

            // Stop lerping when close enough
            if (Mathf.Abs(newValue - targetPos) < snapThreshold)
            {
                scrollbar.GetComponent<Scrollbar>().value = targetPos;
                isLerping = false;
            }
        }
    }

    void FindNearestPosition()
    {
        float currentScrollPos = scrollbar.GetComponent<Scrollbar>().value;
        float minDistance = float.MaxValue;
        int nearestIndex = 0;

        for (int i = 0; i < pos.Length; i++)
        {
            float distance = Mathf.Abs(currentScrollPos - pos[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestIndex = i;
            }
        }

        targetPos = pos[nearestIndex];
        isLerping = true;
    }
}



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class swipePlanetsBtn : MonoBehaviour
// {
//     public GameObject scrollbar;
//     private float scroll_pos = 0f;
//     private float[] pos;

//     void Update()
//     {
//         // int childCount = transform.childCount;
//         pos = new float[transform.childCount];
//         float distance = 1f / (pos.Length - 1f);

//         for (int i = 0; i < pos.Length; i++)
//         {
//             pos[i] = distance * i;
//         }

//         // if (Input.GetMouseButtonDown(0))
//         // {
//         //     scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
//         // }
//         if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//         {
//             scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
//         }
//         else {
//             for (int i = 0; i < pos.Length; i++)
//             {
//                 if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
//                 {
//                     float currentValue = scrollbar.GetComponent<Scrollbar>().value;
//                     float newValue = Mathf.Lerp(currentValue, pos[i], 0.1f);
//                     scrollbar.GetComponent<Scrollbar>().value = newValue;
//                 }
//             }
//         }
//     }
// }
