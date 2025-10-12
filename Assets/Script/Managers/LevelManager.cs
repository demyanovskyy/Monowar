using Assets.Scripts.Core.ObjectPooling;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, IService
{
 
    public Canvas levelManagerCanvas;

    [Header("Fade paramiters")]
    [SerializeField] private float fadeDuration;
    private CanvasGroup canvasCroup;

    public ObjectPool objectPoole;

    public void Init()
    {
        objectPoole = new ObjectPool();

        canvasCroup = levelManagerCanvas.GetComponent<CanvasGroup>();

        if (canvasCroup != null)
            StartCoroutine(FadeToTransperent());
    }

    private IEnumerator FadeToTransperent()
    {
        float time = 0;
        float startAlfa = canvasCroup.alpha;
        float endAlfa = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasCroup.alpha = Mathf.Lerp(startAlfa, endAlfa, time / fadeDuration);
            yield return null;
        }
        canvasCroup.alpha = 0;
    }

    // load scene to index==========
    private IEnumerator FadeToBlackInt(int bildIndex)
    {
        float time = 0;
        float startAlfa = canvasCroup.alpha;
        float endAlfa = 1;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasCroup.alpha = Mathf.Lerp(startAlfa, endAlfa, time / fadeDuration);
            yield return null;
        }
        canvasCroup.alpha = 1;

        SceneManager.LoadScene(bildIndex);
    }

    // load scene to name============
    private IEnumerator FadeToBlackString(string sceneName)
    {
        float time = 0;
        float startAlfa = canvasCroup.alpha;
        float endAlfa = 1;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasCroup.alpha = Mathf.Lerp(startAlfa, endAlfa, time / fadeDuration);
            yield return null;
        }
        canvasCroup.alpha = 1;

        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevelString(string scenename)
    {
        StartCoroutine(FadeToBlackString(scenename));
    }

    public void RestartLevel()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        StartCoroutine(FadeToBlackInt(SceneManager.GetActiveScene().buildIndex));
    }
}
