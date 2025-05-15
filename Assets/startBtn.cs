using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startBtn : MonoBehaviour
{
    public Animator transition; // Шилжилтийн аниматор
    public float transitionTime = 1f; // Шилжилтийн хугацаа
    public void ToTheGuideScene()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("guide");
   }

   IEnumerator LoadLevel(string sceneName)
    {
        // Шилжилтийн анимацийг эхлүүлэх
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }
        
        // Хугацааны турш хүлээх
        yield return new WaitForSeconds(transitionTime);
        
        // Сцен ачаалах
        SceneManager.LoadScene(sceneName);
    }
}
