using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IPInputManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private TextMeshProUGUI statusText;

    public static string ServerIP;

    public void OnContinueClicked()
    {
        if (ipInputField == null || string.IsNullOrEmpty(ipInputField.text.Trim()))
        {
            statusText.text = "IP хаяг оруулна уу!";
            return;
        }

        ServerIP = ipInputField.text.Trim();

        // Энгийн IP формат шалгалт
        if (!ServerIP.Contains(".") || ServerIP.Length < 7)
        {
            statusText.text = "IP формат буруу байна!";
            return;
        }

        statusText.text = "Амжилттай!";
        SceneManager.LoadScene("test");
    }
}
