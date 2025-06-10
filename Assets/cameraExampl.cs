// ///// 6/11nd server ashiglahguigeer tsarai ilruulhiin tuld commentlov 100% ajildag bsn 
// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;

// public class cameraExampl : MonoBehaviour
// {
//     public RawImage rawImage;
//     public faceupload faceupload;

//     public static Texture2D lastCapturedPhoto; // Статик хувьсагч

//     // Камер асаах функц
//     public void StartCamera()
//     {
//         Debug.Log("StartCamera called");
//         TakePicture();
//     }

//     public void TakePicture()
//     {
//         if (NativeCamera.IsCameraBusy())
//         {
//             Debug.Log("Camera is busy");
//             return;
//         }

//         StartCoroutine(TakePictureCoroutine());
//     }

//     private IEnumerator TakePictureCoroutine()
//     {
//         yield return new WaitForEndOfFrame();

//         NativeCamera.TakePicture((path) =>
//         {
//             if (path != null)
//             {
//                 byte[] imageBytes = System.IO.File.ReadAllBytes(path);
//                 lastCapturedPhoto = new Texture2D(2, 2);
//                 lastCapturedPhoto.LoadImage(imageBytes);

//                 // Preview харуулах бол хүсвэл энд нэмэж болно
//                 if (rawImage != null)
//                 {
//                     rawImage.texture = lastCapturedPhoto;
//                     rawImage.gameObject.SetActive(true);
//                 }

//                 // Upload эхлүүлнэ
//                 if (faceupload != null)
//                 {
//                     faceupload.StartUpload();
//                 }
//                 else
//                 {
//                     Debug.LogWarning("FaceUpload script is not assigned!");
//                 }
//             }
//             else
//             {
//                 Debug.LogWarning("Image path is null");
//             }
//         }, 1024);
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class cameraExampl : MonoBehaviour
{
    public RawImage rawImage;
    public FaceDetectLocal faceDetectLocal;

    public static Texture2D lastCapturedPhoto;

    public void StartCamera()
    {
        Debug.Log("StartCamera called");
        TakePicture();
    }

    public void TakePicture()
    {
        if (NativeCamera.IsCameraBusy())
        {
            Debug.Log("Camera is busy");
            return;
        }

        StartCoroutine(TakePictureCoroutine());
    }

    private IEnumerator TakePictureCoroutine()
    {
        yield return new WaitForEndOfFrame();

        NativeCamera.TakePicture((path) =>
        {
            if (path != null)
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(path);
                lastCapturedPhoto = new Texture2D(2, 2);
                lastCapturedPhoto.LoadImage(imageBytes);

                if (rawImage != null)
                {
                    rawImage.texture = lastCapturedPhoto;
                    rawImage.gameObject.SetActive(true);
                }

                // Нүүр илрүүлэлт
                if (faceDetectLocal != null)
                {
                    var faces = faceDetectLocal.DetectFaces(lastCapturedPhoto);
                    Debug.Log($"Detected {faces.Count} faces.");

                    if (faces.Count > 0)
                    {
                        Rect faceRect = faces[0]; // эхний царай
                        Texture2D cropped = CropTexture(lastCapturedPhoto, faceRect);

                        FaceResultHolder.resultTexture = cropped;
                        SceneManager.LoadScene("astroFace");
                    }
                    else
                    {
                        PlayerPrefs.SetString("errorMessage", "Царай илрээгүй байна.");
                        SceneManager.LoadScene("error");
                    }
                }
                else
                {
                    Debug.LogWarning("FaceDetectLocal script is not assigned!");
                }
            }
            else
            {
                Debug.LogWarning("Image path is null");
            }
        }, 1024);
    }

    private Texture2D CropTexture(Texture2D source, Rect faceRect)
    {
        int x = Mathf.Clamp((int)faceRect.x, 0, source.width - 1);
        int y = Mathf.Clamp((int)faceRect.y, 0, source.height - 1);
        int width = Mathf.Clamp((int)faceRect.width, 1, source.width - x);
        int height = Mathf.Clamp((int)faceRect.height, 1, source.height - y);

        Color[] pixels = source.GetPixels(x, y, width, height);
        Texture2D cropped = new Texture2D(width, height);
        cropped.SetPixels(pixels);
        cropped.Apply();

        return cropped;
    }
}
