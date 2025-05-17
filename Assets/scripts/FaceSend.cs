// using UnityEngine;
// using UnityEngine.UI;
// using System.Net.Http;
// using System.Threading.Tasks;
// using TMPro;
// using UnityEngine.SceneManagement;
// using System.Collections;

// // public class USBCameraController : MonoBehaviour
// public class FaceSend : MonoBehaviour
// {
//     [Header("UI Хэсэг")]
//     [SerializeField] private RawImage cameraDisplay;
//     [SerializeField] private Button captureButton;
//     [SerializeField] private RawImage resultDisplay;
//     [SerializeField] private TextMeshProUGUI statusText;

//     [Header("Шилжилтийн Эффект")]
//     [SerializeField] private Animator sceneTransition;
//     [SerializeField] private float transitionDuration = 1f;

//     private WebCamTexture webcamFeed;
//     private bool isCameraActive = false;

//     void Start()
//     {
//         // Камерын зөвшөөрөл шалгах
//         CheckCameraPermission();
        
//         // Товчлуурт функц оноох
//         if (captureButton != null)
//         {
//             captureButton.onClick.AddListener(async () => {
//                 await CaptureAndProcessImage();
//             });
//         }
//     }

//     void CheckCameraPermission()
//     {
//         #if UNITY_ANDROID
//         if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
//         {
//             UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
//             ShowStatus("Камерын зөвшөөрөл шаардлагатай");
//             return;
//         }
//         #endif

//         InitializeUSBCamera();
//     }

//     void InitializeUSBCamera()
//     {
//         // Бүх камерын төхөөрөмжийг шалгах
//         WebCamDevice[] cameras = WebCamTexture.devices;
        
//         if (cameras.Length == 0)
//         {
//             ShowStatus("Ямар ч камер олдсонгүй");
//             return;
//         }

//         // USB камер хайх
//         foreach (var cam in cameras)
//         {
//             if (cam.name.Contains("UVC") || cam.name.Contains("USB"))
//             {
//                 StartCamera(cam.name);
//                 return;
//             }
//         }

//         // USB камер олдохгүй бол эхний камерыг ашиглах
//         StartCamera(cameras[0].name);
//     }

//     void StartCamera(string cameraName)
//     {
//         if (webcamFeed != null)
//         {
//             webcamFeed.Stop();
//             Destroy(webcamFeed);
//         }

//         webcamFeed = new WebCamTexture(cameraName, 1280, 720, 30);
        
//         if (cameraDisplay != null)
//         {
//             cameraDisplay.texture = webcamFeed;
//         }

//         webcamFeed.Play();
//         isCameraActive = true;
//         ShowStatus(cameraName + " камер ажиллаж байна");
//     }

//     async Task CaptureAndProcessImage()
//     {
//         if (!isCameraActive || webcamFeed == null || !webcamFeed.isPlaying)
//         {
//             ShowStatus("Камер бэлэн биш байна");
//             return;
//         }

//         // Зураг авах
//         Texture2D photo = new Texture2D(webcamFeed.width, webcamFeed.height);
//         photo.SetPixels(webcamFeed.GetPixels());
//         photo.Apply();

//         // Сервер рүү илгээх (эсвэл өөр процесс хийх)
//         bool success = await SendToServer(photo);
        
//         if (success)
//         {
//             ShowStatus("Амжилттай боловсрууллаа");
//             if (resultDisplay != null)
//             {
//                 resultDisplay.texture = photo;
//             }
//         }
        
//         Destroy(photo);
//     }

//     async Task<bool> SendToServer(Texture2D image)
//     {
//         // Энд сервер рүү илгээх код оруулна
//         // Жишээ нь:
//         /*
//         byte[] imageData = image.EncodeToJPG();
//         using HttpClient client = new HttpClient();
//         try {
//             var response = await client.PostAsync("http://yourserver.com/api", new ByteArrayContent(imageData));
//             return response.IsSuccessStatusCode;
//         }
//         catch {
//             return false;
//         }
//         */
        
//         return await Task.FromResult(true); // Туршилтын хувьд
//     }

//     void ShowStatus(string message)
//     {
//         if (statusText != null)
//         {
//             statusText.text = message;
//         }
//         Debug.Log(message);
//     }

//     void OnDestroy()
//     {
//         if (webcamFeed != null)
//         {
//             webcamFeed.Stop();
//             Destroy(webcamFeed);
//         }
//     }

//     void OnApplicationPause(bool pauseStatus)
//     {
//         if (pauseStatus && webcamFeed != null && webcamFeed.isPlaying)
//         {
//             webcamFeed.Stop();
//             isCameraActive = false;
//         }
//         else if (!pauseStatus && webcamFeed != null && !webcamFeed.isPlaying)
//         {
//             webcamFeed.Play();
//             isCameraActive = true;
//         }
//     }

//     public void ChangeScene(string sceneName)
//     {
//         StartCoroutine(TransitionToScene(sceneName));
//     }

//     IEnumerator TransitionToScene(string sceneName)
//     {
//         if (webcamFeed != null && webcamFeed.isPlaying)
//         {
//             webcamFeed.Stop();
//         }


//         if (sceneTransition != null)
//         {
//             sceneTransition.SetTrigger("Start");
//         }

//         yield return new WaitForSeconds(transitionDuration);
//         SceneManager.LoadScene(sceneName);
//     }
// }



//delguur deer ajilluulkaad boloogu kod, ZUVHUN UUR DEEREE CAMERATAI TUHUURUMJ DEER L AJILNA 
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

    private WebCamTexture webCamTexture;

    void Start()
    {
        // Камерын зөвшөөрөл Android дээр шалгах
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
            {
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
                return; // Зөвшөөрлийг хэрэглэгч өгөхийг хүлээж зогсооно
            }
        }

        InitCamera();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // Апп-руу буцаж ороход зөвшөөрөл өгсөн эсэхийг шалгаж, InitCamera() дуудаж болно
        if (hasFocus && Application.platform == RuntimePlatform.Android)
        {
            if (UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera) && webCamTexture == null)
            {
                InitCamera();
            }
        }
    }

    void InitCamera()
    {
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
            }
        });
    }

    async Task CaptureAndSend()
    {
        if (webCamTexture == null || !webCamTexture.isPlaying)
        {
            Debug.LogError("Камер ажиллахгүй байна!");
            return;
        }

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        try
        {
            photo.SetPixels(webCamTexture.GetPixels());
            photo.Apply();

            byte[] bytes = photo.EncodeToJPG();

            using HttpClient client = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(bytes), "image", "face.jpg");

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5000/detect_face", form); // ESP32 IP хаяг байж болно

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



//5 15hurtelh kod min 
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

//     void Start()
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

//         sendButton.onClick.AddListener(async () => {
//             try
//             {
//                 await CaptureAndSend();
//             }
//             catch (System.Exception e)
//             {
//                 Debug.LogError("CaptureAndSend алдаа: " + e.Message);
//             }
//         });
//     }

//     async Task CaptureAndSend()
//     {
//         if (webCamTexture == null || !webCamTexture.isPlaying)
//         {
//             Debug.LogError("Камер ажиллахгүй байна!");
//             return;
//         }

//         Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
//         try
//         {
//             photo.SetPixels(webCamTexture.GetPixels());
//             photo.Apply();

//             byte[] bytes = photo.EncodeToJPG();

//             using HttpClient client = new HttpClient();
//             MultipartFormDataContent form = new MultipartFormDataContent();
//             form.Add(new ByteArrayContent(bytes), "image", "face.jpg");

//             try
//             {
//                 HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5000/detect_face", form);

//                 if (response.IsSuccessStatusCode)
//                 {
//                     byte[] resultBytes = await response.Content.ReadAsByteArrayAsync();
//                     Texture2D detectedFace = new Texture2D(2, 2);

//                     if (detectedFace.LoadImage(resultBytes))
//                     {
//                         if (resultBytes.Length > 1000)
//                         {
//                             CreateResultImage(detectedFace);
//                             DetectedFaceImageHolder.faceTexture = detectedFace;

//                             // Шилжилттэйгээр scene ачааллах
//                             StartCoroutine(LoadWithTransition("astroFace"));
//                         }
//                         else
//                         {
//                             ShowError("Царай илрээгүй тул дахин зургаа дарна уу.");
//                         }
//                     }
//                     else
//                     {
//                         ShowError("Зураг буцааж уншиж чадсангүй.");
//                     }
//                 }
//                 else
//                 {
//                     ShowError("Царай илрүүлж чадсангүй. Дахин зураг дарна уу");
//                 }
//             }
//             catch (HttpRequestException e)
//             {
//                 ShowError("Сервертэй холбогдож чадсангүй: " + e.Message);
//             }
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

///////////////////////////////////////////
///

// using UnityEngine;
// using UnityEngine.UI;
// using System.Net.Http;
// using System.Threading.Tasks;

// public class FaceSend : MonoBehaviour
// {
//     [SerializeField] private RawImage cameraFeed;
//     [SerializeField] private Button sendButton;
//     [SerializeField] private RawImage resultDisplay;

//     private WebCamTexture webCamTexture;

//     void Start()
//     {
//         // 1. SerializeField хувьсагчдыг шалгах
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

//         // 2. Веб камерын текстурыг эхлүүлэх
//         webCamTexture = new WebCamTexture();
        
//         try
//         {
//             cameraFeed.texture = webCamTexture;
//             webCamTexture.Play();
            
//             sendButton.onClick.AddListener(async () => {
//                 try 
//                 {
//                     await CaptureAndSend();
//                 }
//                 catch (System.Exception e)
//                 {
//                     Debug.LogError("CaptureAndSend алдаа: " + e.Message);
//                 }
//             });
//         }
//         catch (System.Exception e)
//         {
//             Debug.LogError("Камер эхлүүлэхэд алдаа: " + e.Message);
//         }
//     }

//     async Task CaptureAndSend()
//     {
//         if (webCamTexture == null || !webCamTexture.isPlaying)
//         {
//             Debug.LogError("Камер ажиллахгүй байна!");
//             return;
//         }

//         Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
//         try
//         {
//             photo.SetPixels(webCamTexture.GetPixels());
//             photo.Apply();

//             byte[] bytes = photo.EncodeToJPG();
//             using HttpClient client = new HttpClient();
            
//             MultipartFormDataContent form = new MultipartFormDataContent();
//             form.Add(new ByteArrayContent(bytes), "image", "face.jpg");

//             // HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5000/detect_face", form);
//             HttpResponseMessage response = await client.PostAsync("http://192.168.1.100:5000/detect_face", form);

            
//             if (response.IsSuccessStatusCode)
//             {
//                 byte[] resultBytes = await response.Content.ReadAsByteArrayAsync();
//                 Texture2D detectedFace = new Texture2D(2, 2);
                
//                 if (detectedFace.LoadImage(resultBytes))
//                 {
//                     CreateResultImage(detectedFace);
//                     DetectedFaceImageHolder.faceTexture = detectedFace;
//                 }
//             }
//         }
//         finally
//         {
//             Destroy(photo);
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

//     public void PlanetsScene()
//     {
//         UnityEngine.SceneManagement.SceneManager.LoadScene("planets");
//     }
// }
// ///////////////////////////////////////////////////
// /////////////////////////////////////////////////////
// ////////////////////////////////////////////////
// //////////////////////////////////////////////


// //webcameraCapture tai haritsuulj uuruu bichiv
// // using UnityEngine;
// // using UnityEngine.UI;
// // using System.Net.Http;
// // using System.Threading.Tasks;

// // public class FaceSend : MonoBehaviour
// // {

// //     [SerializeField] private RawImage cameraFeed;
// // [SerializeField] private Button sendButton;
// // [SerializeField] private RawImage resultDisplay;

// //     private WebCamTexture webcamTexture;

// //     void Start()
// //     {
// //         Debug.Log("Scene1 Start");
// //         if (webCamTexture == null)
// //         {
// //             Debug.Log("Creating new WebCamTexture");
// //             webCamTexture = new WebCamTexture();
// //         }

// //         cameraFeed.texture = webCamTexture;
// //         cameraFeed.material.mainTexture = webCamTexture;

// //         if (!webCamTexture.isPlaying)
// //         {
// //             webCamTexture.Play();
// //         }

// //         cameraFeed.gameObject.SetActive(true);
// //         sendButton.onClick.AddListener(() => { _ = CaptureAndSend(); });
// //     }

// //      void OnEnable()
// //     {
// //         Debug.Log("Scene1 OnEnable");
// //         if (webCamTexture != null && !webCamTexture.isPlaying)
// //         {
// //             webCamTexture.Play();
// //         }
// //     }

// //     void OnDisable()
// //     {
// //         Debug.Log("Scene1 OnDisable");
// //         if (webCamTexture != null && webCamTexture.isPlaying)
// //         {
// //             webCamTexture.Stop();
// //         }
// //     }

// //     void OnDestroy()
// //     {
// //         Debug.Log("Scene1 OnDestroy");
// //         if (webCamTexture != null)
// //         {
// //             webCamTexture.Stop();
// //             Destroy(webCamTexture);
// //             webCamTexture = null;
// //         }
// //     }


// //     async Task CaptureAndSend()
// //     {
// //         await Task.Yield();

// //         Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);
// //         photo.SetPixels(webcamTexture.GetPixels());
// //         photo.Apply();

// //         byte[] bytes = photo.EncodeToJPG();



// //         using (HttpClient client = new HttpClient())
// //         {
// //             try
// //             {
// //                 MultipartFormDataContent form = new MultipartFormDataContent();
// //                 form.Add(new ByteArrayContent(bytes), "image", "face.jpg");

// //                 HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5000/detect_face", form);
                
// //                 if (response.IsSuccessStatusCode)
// //                 {
// //                     byte[] resultBytes = await response.Content.ReadAsByteArrayAsync();
// //                     Texture2D detectedFace = new Texture2D(2, 2);
                    
// //                     if (detectedFace.LoadImage(resultBytes))
// //                     {
// //                         CreateResultImage(detectedFace);
// //                         // 📸 Зураг хадгалах
// //                         FaceImageHolder.faceTexture = detectedFace;
// //                         Debug.Log("Зургийг амжилттай харууллаа, hadgalla");
// //                     }
// //                     // byte[] resultBytes = await response.Content.ReadAsByteArrayAsync();
// //                     // string path = Path.Combine(Application.persistentDataPath, "detected_face.jpg");
// //                     // File.WriteAllBytes(path, resultBytes);
// //                     // Debug.Log("✅ Зураг хадгалагдлаа: " + path);
// //                 }

// //             }
// //             catch (System.Exception e)
// //             {
// //                 Debug.LogError("Алдаа: " + e.Message);
// //             }
// //         }
// //     }

// //     void CreateResultImage(Texture2D texture)
// //     {
// //         if (resultDisplay != null)
// //         {
// //             resultDisplay.texture = texture;
// //             // resultDisplay.gameObject.SetActive(true);
// //         }
// //         else
// //         {
// //             Debug.LogWarning("ResultDisplay холбогдоогүй байна. Шинээр үүсгэж байна...");
// //             GameObject newDisplay = new GameObject("ResultImage");
// //             newDisplay.transform.SetParent(cameraFeed.canvas.transform);
// //             RawImage img = newDisplay.AddComponent<RawImage>();
// //             img.texture = texture;
// //             img.rectTransform.sizeDelta = new Vector2(200, 200);
// //         }
// //     }
// // }



// // /////////////////////////////////////////////////////////
// // ////////////////////////////////////////////////////////
// // /////////////////////////////////////////////////////////
// // /////////////////////////////////////////////////////////

// // //4/22nd uurchilluh gj commentlov, yg goy ajillana2
// // // using UnityEngine;
// // // using UnityEngine.UI;
// // // using System.Net.Http;
// // // using System.Threading.Tasks;

// // // public class FaceSend : MonoBehaviour
// // // {
// // //     // public RawImage cameraFeed;
// // //     // public Button sendButton;
// // //     // public RawImage resultDisplay; // Хэрэглэгчийн интерфейсээс холбох
// // // [SerializeField] private RawImage cameraFeed;
// // // [SerializeField] private Button sendButton;
// // // [SerializeField] private RawImage resultDisplay;

// // //     private WebCamTexture webcamTexture;

// // //     void Start()
// // //     {
// // //         webcamTexture = new WebCamTexture();
// // //         cameraFeed.texture = webcamTexture;
// // //         webcamTexture.Play();

// // //         sendButton.onClick.AddListener(() => { _ = CaptureAndSend(); });
// // //     }

// // //     async Task CaptureAndSend()
// // //     {
// // //         await Task.Yield();

// // //         Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);
// // //         photo.SetPixels(webcamTexture.GetPixels());
// // //         photo.Apply();

// // //         byte[] bytes = photo.EncodeToJPG();



// // //         using (HttpClient client = new HttpClient())
// // //         {
// // //             try
// // //             {
// // //                 MultipartFormDataContent form = new MultipartFormDataContent();
// // //                 form.Add(new ByteArrayContent(bytes), "image", "face.jpg");

// // //                 HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5000/detect_face", form);
                
// // //                 if (response.IsSuccessStatusCode)
// // //                 {
// // //                     byte[] resultBytes = await response.Content.ReadAsByteArrayAsync();
// // //                     Texture2D receivedTexture = new Texture2D(2, 2);
                    
// // //                     if (receivedTexture.LoadImage(resultBytes))
// // //                     {
// // //                         CreateResultImage(receivedTexture);
// // //                         Debug.Log("Зургийг амжилттай харууллаа");
// // //                     }
// // //                     // byte[] resultBytes = await response.Content.ReadAsByteArrayAsync();
// // //                     // string path = Path.Combine(Application.persistentDataPath, "detected_face.jpg");
// // //                     // File.WriteAllBytes(path, resultBytes);
// // //                     // Debug.Log("✅ Зураг хадгалагдлаа: " + path);
// // //                 }

// // //             }
// // //             catch (System.Exception e)
// // //             {
// // //                 Debug.LogError("Алдаа: " + e.Message);
// // //             }
// // //         }
// // //     }

// // //     void CreateResultImage(Texture2D texture)
// // //     {
// // //         if (resultDisplay != null)
// // //         {
// // //             resultDisplay.texture = texture;
// // //             resultDisplay.gameObject.SetActive(true);
// // //         }
// // //         else
// // //         {
// // //             Debug.LogWarning("ResultDisplay холбогдоогүй байна. Шинээр үүсгэж байна...");
// // //             GameObject newDisplay = new GameObject("ResultImage");
// // //             newDisplay.transform.SetParent(cameraFeed.canvas.transform);
// // //             RawImage img = newDisplay.AddComponent<RawImage>();
// // //             img.texture = texture;
// // //             img.rectTransform.sizeDelta = new Vector2(200, 200);
// // //         }
// // //     }
// // // }




// // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
// // // ///
// // // using UnityEngine;
// // // using UnityEngine.UI;
// // // using System.Net.Http;
// // // using System.Threading.Tasks;

// // // public class FaceSend : MonoBehaviour
// // // {
// // //     [SerializeField] private RawImage cameraFeed;
// // //     [SerializeField] private Button sendButton;
// // //     [SerializeField] private RawImage resultDisplay;

// // //     private WebCamTexture webcamTexture;
// // //     private bool isProcessing = false;

// // //     void Start()
// // //     {
// // //         // Initialize webcam
// // //         webcamTexture = new WebCamTexture();
        
// // //         // Set up camera feed
// // //         if (cameraFeed != null)
// // //         {
// // //             cameraFeed.texture = webcamTexture;
// // //         }
        
// // //         webcamTexture.Play();

// // //         // Set up button click handler
// // //         if (sendButton != null)
// // //         {
// // //             sendButton.onClick.AddListener(OnSendButtonClicked);
// // //         }
// // //         else
// // //         {
// // //             Debug.LogError("SendButton is not assigned!");
// // //         }
// // //     }

// // //     private async void OnSendButtonClicked()
// // //     {
// // //         // Prevent multiple clicks while processing
// // //         if (isProcessing) return;
        
// // //         isProcessing = true;
// // //         sendButton.interactable = false; // Disable button during processing
        
// // //         try
// // //         {
// // //             await CaptureAndSendImage();
// // //         }
// // //         catch (System.Exception e)
// // //         {
// // //             Debug.LogError("Error processing image: " + e.Message);
// // //         }
// // //         finally
// // //         {
// // //             isProcessing = false;
// // //             sendButton.interactable = true; // Re-enable button
// // //         }
// // //     }

// // //     private async Task CaptureAndSendImage()
// // //     {
// // //         // Check if webcam is ready
// // //         if (webcamTexture == null || !webcamTexture.isPlaying)
// // //         {
// // //             Debug.LogError("Webcam is not ready!");
// // //             return;
// // //         }

// // //         // Create texture from webcam
// // //         Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);
// // //         photo.SetPixels(webcamTexture.GetPixels());
// // //         photo.Apply();

// // //         // Encode to JPG
// // //         byte[] imageBytes = photo.EncodeToJPG();
// // //         Destroy(photo); // Clean up

// // //         // Send to server
// // //         using (HttpClient client = new HttpClient())
// // //         {
// // //             try
// // //             {
// // //                 MultipartFormDataContent form = new MultipartFormDataContent();
// // //                 form.Add(new ByteArrayContent(imageBytes), "image", "face.jpg");

// // //                 HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5000/detect_face", form);
                
// // //                 if (response.IsSuccessStatusCode)
// // //                 {
// // //                     byte[] resultBytes = await response.Content.ReadAsByteArrayAsync();
// // //                     Texture2D resultTexture = new Texture2D(2, 2);
                    
// // //                     if (resultTexture.LoadImage(resultBytes))
// // //                     {
// // //                         DisplayResultImage(resultTexture);
// // //                     }
// // //                 }
// // //             }
// // //             catch (System.Exception e)
// // //             {
// // //                 Debug.LogError("Network error: " + e.Message);
// // //                 throw;
// // //             }
// // //         }
// // //     }

// // //     private void DisplayResultImage(Texture2D texture)
// // //     {
// // //         if (resultDisplay != null)
// // //         {
// // //             resultDisplay.texture = texture;
// // //             resultDisplay.gameObject.SetActive(true);
// // //         }
// // //         else
// // //         {
// // //             Debug.LogWarning("Creating new result display...");
// // //             GameObject newDisplay = new GameObject("ResultImage");
            
// // //             // Find canvas in hierarchy
// // //             Canvas canvas = FindObjectOfType<Canvas>();
// // //             if (canvas != null)
// // //             {
// // //                 newDisplay.transform.SetParent(canvas.transform, false);
// // //                 RawImage img = newDisplay.AddComponent<RawImage>();
// // //                 img.texture = texture;
// // //                 img.rectTransform.sizeDelta = new Vector2(200, 200);
// // //                 img.rectTransform.anchoredPosition = Vector2.zero;
// // //             }
// // //             else
// // //             {
// // //                 Debug.LogError("No Canvas found in scene!");
// // //                 Destroy(newDisplay);
// // //             }
// // //         }
// // //     }

// // //     void OnDestroy()
// // //     {
// // //         // Clean up webcam
// // //         if (webcamTexture != null && webcamTexture.isPlaying)
// // //         {
// // //             webcamTexture.Stop();
// // //         }
        
// // //         // Remove button listener
// // //         if (sendButton != null)
// // //         {
// // //             sendButton.onClick.RemoveListener(OnSendButtonClicked);
// // //         }
// // //     }
// // // }