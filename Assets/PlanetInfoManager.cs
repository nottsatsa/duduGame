// using UnityEngine;
// using TMPro;

// public class PlanetInfoManager : MonoBehaviour
// {
//     public GameObject[] planetInfoObjects; // 9 гаригийн мэдээллийн GameObject-ууд
//     public TMP_Text planetNameText; // Нэрийг харуулах текст
    
//     private bool[] hasBeenShown; // Гариг анх харуулагдсан эсэх
//     private int currentShownIndex = -1;

//     void Start()
//     {
//         hasBeenShown = new bool[planetInfoObjects.Length];
//         HideAllPlanetInfo();
//     }

//     public void ShowPlanetInfo(int planetIndex)
//     {
//         // Хэрэв анхны харуулалт биш бол хуучин мэдээллийг нуух
//         if (currentShownIndex != -1)
//         {
//             planetInfoObjects[currentShownIndex].SetActive(false);
//         }

//         // Шинэ мэдээллийг харуулах
//         planetInfoObjects[planetIndex].SetActive(true);
//         currentShownIndex = planetIndex;

//         // Нэрийг шинэчлэх
//         planetNameText.text = planetInfoObjects[planetIndex].name;

//         // Хэрэв анх удаа харуулагдаж байгаа бол
//         if (!hasBeenShown[planetIndex])
//         {
//             PlayIntroAnimation(planetIndex);
//             hasBeenShown[planetIndex] = true;
//         }
//     }

//     void PlayIntroAnimation(int index)
//     {
//         // Анхны анимаци (жишээ нь томрох, эргэх)
//         Animator anim = planetInfoObjects[index].GetComponent<Animator>();
//         if (anim != null)
//         {
//             anim.Play("FirstShow");
//         }
//     }

//     void HideAllPlanetInfo()
//     {
//         foreach (GameObject info in planetInfoObjects)
//         {
//             info.SetActive(false);
//         }
//     }
// }



using UnityEngine;
using TMPro;

public class PlanetInfoManager : MonoBehaviour
{
    public GameObject[] planetInfoObjects; // 9 гаригийн мэдээллийн GameObject-ууд
    public TMP_Text planetNameText; // Нэрийг харуулах текст
    
    private bool[] hasBeenShown; // Гариг анх харуулагдсан эсэх
    private int currentShownIndex = -1;

    void Start()
    {
        Debug.Log("PlanetInfoManager идэвхжлээ - " + planetInfoObjects.Length + " гаригийн мэдээлэл бэлэн");
        
        hasBeenShown = new bool[planetInfoObjects.Length];
        HideAllPlanetInfo();

        // Массивын бэлэн байдлыг шалгах
        if (planetInfoObjects == null || planetInfoObjects.Length == 0)
        {
            Debug.LogError("Анхаар: planetInfoObjects массив хоосон эсвэл холбогдоогүй байна!");
        }
        else
        {
            Debug.Log("Гаригийн мэдээллийн объектууд амжилттай холбогдсон");
        }
    }

    public void ShowPlanetInfo(int planetIndex)
    {
        Debug.Log("Гариг сонголт ирлээ: " + planetIndex);

        // Индекс ба массивын хэмжээг шалгах
        if (planetIndex < 0 || planetIndex >= planetInfoObjects.Length)
        {
            Debug.LogError("Буруу индекс: " + planetIndex + " (Зөвхөн 0-" + (planetInfoObjects.Length-1) + " хооронд)");
            return;
        }

        if (planetInfoObjects[planetIndex] == null)
        {
            Debug.LogError("Алдаа: " + planetIndex + " индексийн GameObject хоосон байна");
            return;
        }

        // Хуучин мэдээллийг нуух
        if (currentShownIndex != -1)
        {
            Debug.Log("Өмнөх гаригийн мэдээлэл (" + currentShownIndex + ") хаагдаж байна");
            planetInfoObjects[currentShownIndex].SetActive(false);
        }

        // Шинэ мэдээллийг харуулах
        Debug.Log(planetIndex + " индексийн гаригийн мэдээлэл харуулж байна: " + planetInfoObjects[planetIndex].name);
        planetInfoObjects[planetIndex].SetActive(true);
        currentShownIndex = planetIndex;

        // Нэрийг шинэчлэх
        if (planetNameText != null)
        {
            planetNameText.text = planetInfoObjects[planetIndex].name;
            Debug.Log("Гаригийн нэр шинэчлэгдлээ: " + planetNameText.text);
        }
        else
        {
            Debug.LogError("Анхаар: planetNameText холбогдоогүй байна!");
        }

        // Анхны харуулалт эсэх
        if (!hasBeenShown[planetIndex])
        {
            Debug.Log("Энэ гариг АНХ удаа харуулагдаж байна");
            PlayIntroAnimation(planetIndex);
            hasBeenShown[planetIndex] = true;
        }
        else
        {
            Debug.Log("Энэ гариг өмнө нь харуулагдсан байна");
        }
    }

    void PlayIntroAnimation(int index)
    {
        Debug.Log("Анхны анимаци эхлэлээ: " + index);
        
        Animator anim = planetInfoObjects[index].GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("FirstShow");
            Debug.Log("FirstShow анимаци тоглов");
        }
        else
        {
            Debug.LogWarning("Анхаар: " + index + " индексийн объектод Animator олдсонгүй");
        }
    }

    void HideAllPlanetInfo()
    {
        Debug.Log("Бүх гаригийн мэдээлэл хаагдаж байна...");
        
        foreach (GameObject info in planetInfoObjects)
        {
            if (info != null)
            {
                info.SetActive(false);
            }
        }
    }

    // Нэмэлт debug хийхэд ашиглах
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ShowPlanetInfo(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ShowPlanetInfo(1);
        // ... бусад тоонуудыг нэмнэ
    }
}