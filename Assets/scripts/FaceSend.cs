////tablet ashiglah gher hoid camera garch ireed bdg thde zgr ajilna
// using UnityEngine;
// using UnityEngine.UI;
// using System.Net.Http;
// using System.Threading.Tasks;
// using TMPro;
// using UnityEngine.SceneManagement;
// using System.Collections;

// public class FaceSend : MonoBehaviour
// {
//     [SerializeField] private RawImage cameraFeed;
//     [SerializeField] private Button sendButton;
//     [SerializeField] private RawImage resultDisplay;
//     [SerializeField] private TextMeshProUGUI errorText;

//     [SerializeField] private Animator transition;
//     [SerializeField] private float transitionTime = 1f;

//     private WebCamTexture webCamTexture;

//     private string GetServerUrl()
//     {
//         if (string.IsNullOrEmpty(IPInputManager.ServerIP))
//         {
//             ShowError("IP хаяг хоосон байна!");
//             return null;
//         }

//         return $"http://{IPInputManager.ServerIP}:5000/detect_face";
//     }

//     void Start()
//     {
//         if (Application.platform == RuntimePlatform.Android)
//         {
//             if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
//             {
//                 UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
//                 return;
//             }
//         }

//         InitCamera();
//     }

//     void OnApplicationFocus(bool hasFocus)
//     {
//         if (hasFocus && Application.platform == RuntimePlatform.Android)
//         {
//             if (UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera) && webCamTexture == null)
//             {
//                 InitCamera();
//             }
//         }
//     }

//     void InitCamera()
//     {
//         if (cameraFeed == null)
//         {
//             Debug.LogError("Camera Feed RawImage холбогдоогүй байна!");
//             return;
//         }

//         if (sendButton == null)
//         {
//             Debug.LogError("Send Button холбогдоогүй байна!");
//             return;
//         }

//         if (errorText != null)
//         {
//             errorText.gameObject.SetActive(false);
//         }

//         webCamTexture = new WebCamTexture();
//         cameraFeed.texture = webCamTexture;
//         webCamTexture.Play();

//         sendButton.onClick.AddListener(async () =>
//         {
//             try
//             {
//                 await CaptureAndSend();
//             }
//             catch (System.Exception e)
//             {
//                 Debug.LogError("CaptureAndSend алдаа: " + e.Message);
//                 ShowError("Алдаа гарлаа: " + e.Message);
//             }
//         });
//     }

//     async Task CaptureAndSend()
//     {
//         string url = GetServerUrl();
//         if (string.IsNullOrEmpty(url))
//         {
//             Debug.LogError("IP хаяг буруу эсвэл хоосон байна!");
//             return;
//         }

//         if (webCamTexture == null || !webCamTexture.isPlaying)
//         {
//             Debug.LogError("Камер ажиллахгүй байна!");
//             return;
//         }

//         Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
//         photo.SetPixels(webCamTexture.GetPixels());
//         photo.Apply();

//         byte[] bytes = photo.EncodeToJPG();

//         using HttpClient client = new HttpClient();
//         MultipartFormDataContent form = new MultipartFormDataContent();
//         form.Add(new ByteArrayContent(bytes), "image", "face.jpg");

//         try
//         {
//             HttpResponseMessage response = await client.PostAsync(url, form);

//             if (response.IsSuccessStatusCode)
//             {
//                 byte[] resultBytes = await response.Content.ReadAsByteArrayAsync();
//                 Texture2D detectedFace = new Texture2D(2, 2);

//                 if (detectedFace.LoadImage(resultBytes))
//                 {
//                     if (resultBytes.Length > 1000)
//                     {
//                         CreateResultImage(detectedFace);
//                         DetectedFaceImageHolder.faceTexture = detectedFace;
//                         StartCoroutine(LoadWithTransition("astroFace"));
//                     }
//                     else
//                     {
//                         ShowError("Царай илрээгүй тул дахин зургаа дарна уу.");
//                     }
//                 }
//                 else
//                 {
//                     ShowError("Зураг буцааж уншиж чадсангүй.");
//                 }
//             }
//             else
//             {
//                 ShowError("Царай илрүүлж чадсангүй. Дахин зураг дарна уу");
//             }
//         }
//         catch (HttpRequestException e)
//         {
//             ShowError("Сервертэй холбогдож чадсангүй: " + e.Message);
//         }
//         finally
//         {
//             Destroy(photo);
//         }
//     }

//     void ShowError(string message)
//     {
//         if (errorText != null)
//         {
//             errorText.text = message;
//             errorText.gameObject.SetActive(true);
//         }
//         else
//         {
//             Debug.LogWarning("Error Text холбоогүй байна.");
//         }
//     }

//     void CreateResultImage(Texture2D texture)
//     {
//         if (resultDisplay != null)
//         {
//             resultDisplay.texture = texture;
//         }
//         else
//         {
//             Debug.LogWarning("Result Display RawImage холбогдоогүй байна");
//         }
//     }

//     void OnDestroy()
//     {
//         if (webCamTexture != null)
//         {
//             webCamTexture.Stop();
//             Destroy(webCamTexture);
//         }
//     }

//     void OnEnable()
//     {
//         if (webCamTexture != null && !webCamTexture.isPlaying)
//         {
//             webCamTexture.Play();
//         }
//     }

//     void OnDisable()
//     {
//         if (webCamTexture != null && webCamTexture.isPlaying)
//         {
//             webCamTexture.Stop();
//         }
//     }

//     IEnumerator LoadWithTransition(string sceneName)
//     {
//         if (transition != null)
//         {
//             transition.SetTrigger("Start");
//         }

//         yield return new WaitForSeconds(transitionTime);
//         SceneManager.LoadScene(sceneName);
//     }

//     public void PlanetsScene()
//     {
//         SceneManager.LoadScene("guide");
//     }
// }




using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class FaceSend : MonoBehaviour
{
    [SerializeField] private RawImage cameraFeed;
    [SerializeField] private Button sendButton;
    [SerializeField] private RawImage resultDisplay;
    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

   
    [SerializeField] private Button switchCameraButton; // Камер солих товч
    [SerializeField] private TextMeshProUGUI switchCameraText;
    private WebCamTexture webCamTexture;
     private int currentCameraIndex = 0;
    private bool isFrontFacing = true;


    private string GetServerUrl()
    {
        if (string.IsNullOrEmpty(IPInputManager.ServerIP))
        {
            ShowError("IP хаяг хоосон байна!");
            return null;
        }

        return $"http://{IPInputManager.ServerIP}:5000/detect_face";
    }

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
            {
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
                return;
            }
        }

        
        // Камер солих товчны event нэмэх
        if (switchCameraButton != null)
        {
            switchCameraButton.onClick.AddListener(SwitchCamera);
        }
        InitCamera();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && Application.platform == RuntimePlatform.Android)
        {
            if (UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera) && webCamTexture == null)
            {
                InitCamera();
            }
        }
    }

 // Камер солих функц
    public void SwitchCamera()
    {
        if (WebCamTexture.devices.Length < 2)
        {
            Debug.Log("Камер солих боломжгүй - 1 камер байна");
            return;
        }

        // Одоогийн камерыг зогсоох
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            webCamTexture.Stop();
            Destroy(webCamTexture);
        }

        // Камерын индексийг солих
        currentCameraIndex = (currentCameraIndex + 1) % WebCamTexture.devices.Length;
        isFrontFacing = !isFrontFacing;

        // Шинэ камер эхлүүлэх
        InitCamera();
        // Текст шинэчлэх
    if (switchCameraText != null)
    {
        switchCameraText.text = isFrontFacing ? "Арын камер" : "Урд камер";
    }
    }

    void InitCamera()
    {
        // Камерын төхөөрөмж сонгох
        WebCamDevice device = WebCamTexture.devices[currentCameraIndex];
        webCamTexture = new WebCamTexture(device.name);
        if (cameraFeed == null)
        {
            Debug.LogError("Camera Feed RawImage холбогдоогүй байна!");
            return;
        }

        if (sendButton == null)
        {
            Debug.LogError("Send Button холбогдоогүй байна!");
            return;
        }

        if (errorText != null)
        {
            errorText.gameObject.SetActive(false);
        }

        webCamTexture = new WebCamTexture();
        cameraFeed.texture = webCamTexture;
        webCamTexture.Play();

        sendButton.onClick.AddListener(async () =>
        {
            try
            {
                await CaptureAndSend();
            }
            catch (System.Exception e)
            {
                Debug.LogError("CaptureAndSend алдаа: " + e.Message);
                ShowError("Алдаа гарлаа: " + e.Message);
            }
        });
    }

void Update()
{
    if (webCamTexture != null && webCamTexture.isPlaying)
    {
        UpdateCameraOrientation();
    }
}

void UpdateCameraOrientation()
{
    // Эргэлтийн өнцөг
    cameraFeed.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTexture.videoRotationAngle);
    
    // Толин тусгал (зөвхөн урд камерын хувьд)
    if (isFrontFacing)
    {
        cameraFeed.rectTransform.localScale = new Vector3(
            webCamTexture.videoVerticallyMirrored ? -1 : 1,
            1, 1);
    }
    else
    {
        cameraFeed.rectTransform.localScale = Vector3.one;
    }
}

    async Task CaptureAndSend()
    {
        string url = GetServerUrl();
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("IP хаяг буруу эсвэл хоосон байна!");
            return;
        }

        if (webCamTexture == null || !webCamTexture.isPlaying)
        {
            Debug.LogError("Камер ажиллахгүй байна!");
            return;
        }

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        byte[] bytes = photo.EncodeToJPG();

        using HttpClient client = new HttpClient();
        MultipartFormDataContent form = new MultipartFormDataContent();
        form.Add(new ByteArrayContent(bytes), "image", "face.jpg");

        try
        {
            HttpResponseMessage response = await client.PostAsync(url, form);

            if (response.IsSuccessStatusCode)
            {
                byte[] resultBytes = await response.Content.ReadAsByteArrayAsync();
                Texture2D detectedFace = new Texture2D(2, 2);

                if (detectedFace.LoadImage(resultBytes))
                {
                    if (resultBytes.Length > 1000)
                    {
                        CreateResultImage(detectedFace);
                        DetectedFaceImageHolder.faceTexture = detectedFace;
                        StartCoroutine(LoadWithTransition("astroFace"));
                    }
                    else
                    {
                        ShowError("Царай илрээгүй тул дахин зургаа дарна уу.");
                    }
                }
                else
                {
                    ShowError("Зураг буцааж уншиж чадсангүй.");
                }
            }
            else
            {
                ShowError("Царай илрүүлж чадсангүй. Дахин зураг дарна уу");
            }
        }
        catch (HttpRequestException e)
        {
            ShowError("Сервертэй холбогдож чадсангүй: " + e.Message);
        }
        finally
        {
            Destroy(photo);
        }
    }

    void ShowError(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Error Text холбоогүй байна.");
        }
    }

    void CreateResultImage(Texture2D texture)
    {
        if (resultDisplay != null)
        {
            resultDisplay.texture = texture;
        }
        else
        {
            Debug.LogWarning("Result Display RawImage холбогдоогүй байна");
        }
    }

    void OnDestroy()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
            Destroy(webCamTexture);
        }
    }

    void OnEnable()
    {
        if (webCamTexture != null && !webCamTexture.isPlaying)
        {
            webCamTexture.Play();
        }
    }

    void OnDisable()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            webCamTexture.Stop();
        }
    }

    IEnumerator LoadWithTransition(string sceneName)
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    public void PlanetsScene()
    {
        SceneManager.LoadScene("guide");
    }
}





