

// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;
// using UnityEngine.SceneManagement;
// public class cameraExampl : MonoBehaviour
// {
//     public RawImage rawImage;
    
//     public faceupload faceupload;
//     // public FaceUploader faceupload; // Нэмсэн!!!

//     public static Texture2D lastCapturedPhoto; // Статик хувьсагч

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
                
//                 // Шууд upload эхлүүлэх
//                 FindObjectOfType<faceupload>().StartUpload();
//             }
//         }, 1024);
//     }
// }



using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class cameraExampl : MonoBehaviour
{
    public RawImage rawImage;
    public faceupload faceupload;

    public static Texture2D lastCapturedPhoto; // Статик хувьсагч

    // Камер асаах функц
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

                // Preview харуулах бол хүсвэл энд нэмэж болно
                if (rawImage != null)
                {
                    rawImage.texture = lastCapturedPhoto;
                    rawImage.gameObject.SetActive(true);
                }

                // Upload эхлүүлнэ
                if (faceupload != null)
                {
                    faceupload.StartUpload();
                }
                else
                {
                    Debug.LogWarning("FaceUpload script is not assigned!");
                }
            }
            else
            {
                Debug.LogWarning("Image path is null");
            }
        }, 1024);
    }
}
