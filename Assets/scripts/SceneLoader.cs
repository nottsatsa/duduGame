using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
     public Animator transition;
    public float transitionTime = 1f;
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            LoadNextLevel();
        }
    }
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1)) ;
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}


/////////////////////////////
// ///
// using System.Collections; 
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class SceneLoader : MonoBehaviour
// {
//     public Animator transition;
//     public float transitionTime = 1f;

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             LoadNextLevel();
//         }
//     }

//     public void LoadNextLevel()
//     {
//         int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
//         string currentSceneName = SceneManager.GetActiveScene().name;
//         string nextSceneName = SceneManager.GetSceneByBuildIndex(nextSceneIndex).name;

//         // Зөвхөн 'test' → 'guide' үед fade ашиглана
//         if (currentSceneName == "test" && nextSceneName == "guide")
//         {
//             StartCoroutine(LoadLevelWithTransition(nextSceneIndex));
//         }
//         else
//         {
//             SceneManager.LoadScene(nextSceneIndex); // Шууд шилжих
//         }
//     }

//     IEnumerator LoadLevelWithTransition(int levelIndex)
//     {
//         transition.SetTrigger("Start");
//         yield return new WaitForSeconds(transitionTime);
//         SceneManager.LoadScene(levelIndex);
//     }
// }
