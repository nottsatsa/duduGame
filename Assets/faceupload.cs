// using UnityEngine;
// using UnityEngine.Networking;
// using System.Collections;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;

// public class faceupload : MonoBehaviour
// {
//     public RawImage resultImage; // Заавал биш: урьдчилж харуулах зориулалттай
//     public string serverUrl = "http://192.168.4.2:5000/detect_face";

//     public void StartUpload()
//     {
//         Debug.Log("Start Upload: " + serverUrl);
//         StartCoroutine(UploadAndGetFace());
//     }

//     IEnumerator UploadAndGetFace()
//     {
//         if (cameraExampl.lastCapturedPhoto == null)
//         {
//             PlayerPrefs.SetString("errorMessage", "Зураг байхгүй байна!");
//             SceneManager.LoadScene("error");
//             yield break;
//         }

//         // Зураг 90 градус эргүүлж серверт илгээнэ
//         Texture2D rotatedImage = RotateTexture(cameraExampl.lastCapturedPhoto, 90);
//         byte[] imageData = rotatedImage.EncodeToJPG();

//         WWWForm form = new WWWForm();
//         form.AddBinaryData("image", imageData, "upload.jpg", "image/jpeg");

//         using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, form))
//         {
//             yield return www.SendWebRequest();

//             if (www.result != UnityWebRequest.Result.Success)
//             {
//                 PlayerPrefs.SetString("errorMessage", "Сервертэй холбогдож чадсангүй:\n" + www.error);
//                 SceneManager.LoadScene("error");
//             }
//             else
//             {
//                 Texture2D faceTexture = new Texture2D(2, 2);
//                 bool loaded = faceTexture.LoadImage(www.downloadHandler.data);

//                 if (!loaded || faceTexture.width < 10)
//                 {
//                     PlayerPrefs.SetString("errorMessage", "Царай илрээгүй байна.");
//                     SceneManager.LoadScene("error");
//                 }
//                 else
//                 {
//                     // Царай илэрсэн бол хадгалаад дараагийн scene рүү
//                     FaceResultHolder.resultTexture = faceTexture;

//                     if (resultImage != null)
//                         resultImage.texture = faceTexture;

//                     SceneManager.LoadScene("astroFace");
//                 }
//             }
//         }
//     }

//     // Зураг эргүүлэх туслах функц
//     Texture2D RotateTexture(Texture2D originalTexture, float angle)
//     {
//         angle = angle % 360f;
//         if (angle == 0) return originalTexture;

//         int width = originalTexture.width;
//         int height = originalTexture.height;
//         Color32[] originalPixels = originalTexture.GetPixels32();
//         Color32[] rotatedPixels = new Color32[originalPixels.Length];

//         Vector2 center = new Vector2(width / 2f, height / 2f);

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 Vector2 pos = new Vector2(x, y);
//                 Vector2 dir = pos - center;
//                 dir = Quaternion.Euler(0, 0, angle) * dir;
//                 Vector2 rotatedPos = center + dir;

//                 int xRot = Mathf.RoundToInt(rotatedPos.x);
//                 int yRot = Mathf.RoundToInt(rotatedPos.y);

//                 if (xRot >= 0 && xRot < width && yRot >= 0 && yRot < height)
//                 {
//                     rotatedPixels[y * width + x] = originalPixels[yRot * width + xRot];
//                 }
//             }
//         }

//         Texture2D result = new Texture2D(width, height);
//         result.SetPixels32(rotatedPixels);
//         result.Apply();
//         return result;
//     }
// }



using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class faceupload : MonoBehaviour
{
    public RawImage resultImage;
    public string serverUrl = "http://192.168.4.2:5000/detect_face";

    public void StartUpload()
    {
        Debug.Log("Start Upload: " + serverUrl);
        StartCoroutine(UploadAndGetFace());
    }

    IEnumerator UploadAndGetFace()
    {
        if (cameraExampl.lastCapturedPhoto == null)
        {
            PlayerPrefs.SetString("errorMessage", "Зураг байхгүй байна!");
            SceneManager.LoadScene("error");
            yield break;
        }

        // Зургийг багасгаад эргүүлнэ
        Texture2D resized = ResizeTexture(cameraExampl.lastCapturedPhoto, 480, 270);
        Texture2D rotatedImage = RotateTexture(resized, 90);
        byte[] imageData = rotatedImage.EncodeToJPG();

        WWWForm form = new WWWForm();
        form.AddBinaryData("image", imageData, "upload.jpg", "image/jpeg");

        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                PlayerPrefs.SetString("errorMessage", "Сервертэй холбогдож чадсангүй:\n" + www.error);
                SceneManager.LoadScene("error");
            }
            else
            {
                Texture2D faceTexture = new Texture2D(2, 2);
                bool loaded = faceTexture.LoadImage(www.downloadHandler.data);

                if (!loaded || faceTexture.width < 10)
                {
                    PlayerPrefs.SetString("errorMessage", "Царай илрээгүй байна.");
                    SceneManager.LoadScene("error");
                }
                else
                {
                    FaceResultHolder.resultTexture = faceTexture;

                    if (resultImage != null)
                        resultImage.texture = faceTexture;

                    SceneManager.LoadScene("astroFace");
                }
            }
        }
    }

    // Зураг эргүүлэх функц
    Texture2D RotateTexture(Texture2D originalTexture, float angle)
    {
        angle = angle % 360f;
        if (angle == 0) return originalTexture;

        int width = originalTexture.width;
        int height = originalTexture.height;
        Color32[] originalPixels = originalTexture.GetPixels32();
        Color32[] rotatedPixels = new Color32[originalPixels.Length];

        Vector2 center = new Vector2(width / 2f, height / 2f);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 pos = new Vector2(x, y);
                Vector2 dir = pos - center;
                dir = Quaternion.Euler(0, 0, angle) * dir;
                Vector2 rotatedPos = center + dir;

                int xRot = Mathf.RoundToInt(rotatedPos.x);
                int yRot = Mathf.RoundToInt(rotatedPos.y);

                if (xRot >= 0 && xRot < width && yRot >= 0 && yRot < height)
                {
                    rotatedPixels[y * width + x] = originalPixels[yRot * width + xRot];
                }
            }
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels32(rotatedPixels);
        result.Apply();
        return result;
    }

    // Зураг багасгах функц
    Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        Graphics.Blit(source, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D result = new Texture2D(newWidth, newHeight);
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }
}
