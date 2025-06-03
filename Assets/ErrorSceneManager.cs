using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ErrorSceneManager : MonoBehaviour
{
    public TextMeshProUGUI errorText;

    void Start()
    {
        // Алдааны утга хадгалагдсан бол харуулах
        string message = PlayerPrefs.GetString("errorMessage", "Тодорхойгүй алдаа гарлаа.");
        errorText.text = message;
    }

    public void Retry()
    {
        // Дахин guide scene рүү очно
        SceneManager.LoadScene("guide");
    }
}
