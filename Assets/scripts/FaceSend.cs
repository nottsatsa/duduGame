using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.SceneManagement;
public class FaceSend : MonoBehaviour
{
    [SerializeField] private RawImage cameraFeed;
    [SerializeField] private Button sendButton;
    [SerializeField] private RawImage resultDisplay;
    [SerializeField] private TextMeshProUGUI errorText; // ← Text UI холбоно

    private WebCamTexture webCamTexture;

    void Start()
    {
        // Холболтуудыг шалгах
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
            errorText.gameObject.SetActive(false); // Эхэндээ алга байлга
        }

        // Камер эхлүүлэх
        webCamTexture = new WebCamTexture();
        cameraFeed.texture = webCamTexture;
        webCamTexture.Play();

        // Товч дарахад функц дуудах
        sendButton.onClick.AddListener(async () => {
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

            HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5000/detect_face", form);

            if (response.IsSuccessStatusCode)
            {
                byte[] resultBytes = await response.Content.ReadAsByteArrayAsync();
                Texture2D detectedFace = new Texture2D(2, 2);

                if (detectedFace.LoadImage(resultBytes))
                {
                    if (resultBytes.Length > 1000) // Царай илэрсэн гэж үзэх хязгаар
                    {
                        CreateResultImage(detectedFace);
                        DetectedFaceImageHolder.faceTexture = detectedFace;

                        // Scene сольж байна
                        UnityEngine.SceneManagement.SceneManager.LoadScene("guide");
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
                ShowError("Серверийн хариу амжилтгүй боллоо.");
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

    public void PlanetsScene()
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("guide");
        SceneManager.LoadScene("guide");
    }
}


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