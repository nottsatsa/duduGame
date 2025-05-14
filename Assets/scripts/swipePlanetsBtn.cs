using UnityEngine;
using UnityEngine.UI;

public class swipePlanetsBtn : MonoBehaviour
{
    public GameObject scrollbar;          // Scrollbar-ын объект
    public float lerpSpeed = 10f;         // Хөдөлгөөний хурд
    public float snapThreshold = 0.01f;   // "Засварлах" бүс
    
    private float scroll_pos = 0f;        // Одоогийн байрлал
    private float[] pos;                  // Боломжит байрлалууд
    private bool isLerping = false;       // Одоо шилжиж байгаа эсэх
    private float targetPos;              // Очих байрлал
    private float distance;               // Элементүүдийн хоорондох зай

    void Start()
    {
        InitializePositions();
    }

    void InitializePositions()
    {
        pos = new float[transform.childCount];
        distance = 1f / (pos.Length - 1f);  // Зайг тооцоолох
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;  // Байрлалуудыг тохируулах (0-1 хооронд)
        }
    }

    void Update()
    {
        // Хэрэв дарж эхэлбэл (мөн touch)
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            isLerping = false;
        }
        
        // Хэрэв чирээгүй бол, хамгийн ойр байрлал руу шилжих
        if (!isLerping && !Input.GetMouseButton(0) && (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Moved))
        {
            FindNearestPosition();
        }

        // Хэрэв шилжиж байгаа бол
        if (isLerping)
        {
            float currentValue = scrollbar.GetComponent<Scrollbar>().value;
            float newValue = Mathf.Lerp(currentValue, targetPos, lerpSpeed * Time.deltaTime);
            scrollbar.GetComponent<Scrollbar>().value = newValue;

            // Хэрэв ойрхон ирвэл зогсоох
            if (Mathf.Abs(newValue - targetPos) < snapThreshold)
            {
                scrollbar.GetComponent<Scrollbar>().value = targetPos;
                isLerping = false;
            }
        }

        // Хүүхдүүдийн хэмжээг өөрчлөх (төвлөрсөн элементийг томруулах)
        for (int i = 0; i < pos.Length; i++)
        {
            float childScale = 0.8f;  // Жижиг хэмжээ
            if (scrollbar.GetComponent<Scrollbar>().value >= pos[i] - (distance / 2) && 
                scrollbar.GetComponent<Scrollbar>().value <= pos[i] + (distance / 2))
            {
                childScale = 1.1f;  // Том хэмжээ (төвд байгаа)
            }
            
            // Гөлгөр өөрчлөлт
            transform.GetChild(i).localScale = Vector2.Lerp(
                transform.GetChild(i).localScale, 
                new Vector2(childScale, childScale), 
                5f * Time.deltaTime
            );
        }
    }

    void FindNearestPosition()
    {
        float currentScrollPos = scrollbar.GetComponent<Scrollbar>().value;
        float minDistance = float.MaxValue;
        int nearestIndex = 0;

        for (int i = 0; i < pos.Length; i++)
        {
            float dist = Mathf.Abs(currentScrollPos - pos[i]);
            if (dist < minDistance)
            {
                minDistance = dist;
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
