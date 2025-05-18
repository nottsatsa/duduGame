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
    public GameObject[] planetInfoObjects; // 9 гаригийн мэдээлэл
    public TMP_Text planetNameText; // Нэрийг харуулах текст

    private bool[] hasBeenShown; // Анх удаа харуулагдсан эсэх
    private int currentShownIndex = -1;

    void Start()
    {
        hasBeenShown = new bool[planetInfoObjects.Length];
        HideAllPlanetInfo();

        if (planetInfoObjects == null || planetInfoObjects.Length == 0)
        {
            Debug.LogError("planetInfoObjects массив холбогдоогүй байна!");
        }
    }

    public void ShowPlanetInfo(int planetIndex)
    {
        if (planetIndex < 0 || planetIndex >= planetInfoObjects.Length) return;
        if (planetInfoObjects[planetIndex] == null) return;

        // Өмнөхийг хаах
        if (currentShownIndex != -1)
        {
            planetInfoObjects[currentShownIndex].SetActive(false);
        }

        // Шинийг харуулах
        planetInfoObjects[planetIndex].SetActive(true);
        currentShownIndex = planetIndex;

        // Нэр шинэчлэх
        if (planetNameText != null)
        {
            planetNameText.text = planetInfoObjects[planetIndex].name;
        }

        // Анхны удаа бол animation тоглуулах
        if (!hasBeenShown[planetIndex])
        {
            PlayIntroAnimation(planetIndex);
            hasBeenShown[planetIndex] = true;
        }
    }

    void PlayIntroAnimation(int index)
    {
        Animator anim = planetInfoObjects[index].GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("FirstShow");
        }
    }

    void HideAllPlanetInfo()
    {
        foreach (GameObject info in planetInfoObjects)
        {
            if (info != null)
            {
                info.SetActive(false);
            }
        }
    }

    // Түр debug шалгах зориулалттай
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ShowPlanetInfo(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ShowPlanetInfo(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ShowPlanetInfo(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ShowPlanetInfo(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ShowPlanetInfo(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) ShowPlanetInfo(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) ShowPlanetInfo(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) ShowPlanetInfo(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) ShowPlanetInfo(8);
    }
}
