using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using UnityEngine.SceneManagement;
using System.Collections;

public class FaceDetectionController : MonoBehaviour
{
    public RawImage previewImage;
    public RawImage resultImage;
    public TextAsset facesCascade;

    private WebCamTexture webCamTexture;
    private CascadeClassifier faceClassifier;
    private bool isDetecting = false;

    void Start()
    {
        faceClassifier = new CascadeClassifier();
        faceClassifier.Load(facesCascade.text);
        StartWebcam();
    }

    void StartWebcam()
    {
        webCamTexture = new WebCamTexture();
        previewImage.texture = webCamTexture;
        webCamTexture.Play();
    }

    public void StartFaceDetection()
    {
        if (!isDetecting) StartCoroutine(DetectFace());
    }

    IEnumerator DetectFace()
    {
        isDetecting = true;
        yield return new WaitForEndOfFrame();

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        Mat image = TextureToMat(photo);
        Mat gray = new Mat();
        Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

        OpenCvSharp.Rect[] faces = faceClassifier.DetectMultiScale(gray, 1.1, 5);
        
        if (faces.Length > 0)
        {
            OpenCvSharp.Rect face = faces[0];
            Cv2.Rectangle(image, face, new Scalar(255, 0, 0), 2);
            
            Texture2D resultTex = MatToTexture(image);
            resultImage.texture = resultTex;
            FaceResultHolder.resultTexture = resultTex;
            SceneManager.LoadScene("astroFace");
        }
        else
        {
            PlayerPrefs.SetString("errorMessage", "Царай илрээгүй!");
            SceneManager.LoadScene("error");
        }

        isDetecting = false;
    }

    private Mat TextureToMat(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        return Cv2.ImDecode(bytes, ImreadModes.Color);
    }

    private Texture2D MatToTexture(Mat mat)
    {
        Texture2D texture = new Texture2D(mat.Width, mat.Height, TextureFormat.RGBA32, false);
        texture.LoadImage(mat.ToBytes(".png"));
        return texture;
    }

    void OnDestroy()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
            webCamTexture.Stop();
    }
}