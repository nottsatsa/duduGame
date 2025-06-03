using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startTurshilt : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    [SerializeField] private Button startButton;

    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(LoadNextLevel);
        }
        else
        {
            Debug.LogWarning("Start товч холбогдоогүй байна!");
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}

