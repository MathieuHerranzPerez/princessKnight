using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image img;
    public AnimationCurve curve;

    public GameObject loadScreen;
    public Text tipsText;
    public Image background;

    public string[] tipsArray;
    public Sprite[] spriteArray;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(FadeInAnim());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeTo(string scene)
    {
        FadeOut(scene);
    }



    private void FadeOut(string scene)
    {
        StartCoroutine(loadingScreen(scene));
    }


    IEnumerator loadingScreen(string scene)
    {

        {// -- FADE OUT --
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                float alpha = curve.Evaluate(t);
                img.color = new Color(0f, 0f, 0f, alpha);
                yield return 0;             // skip to the next frame
            }
        }// -- /FADE OUT --

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;

        float timeLoadingScreen = 0f;

        // set a tips into it
        if (tipsArray.Length > 0)
        {
            int index = Random.Range(0, tipsArray.Length);
            tipsText.text = tipsArray[index];
        }
        // set srpite
        if (spriteArray.Length > 0)
        {
            int index = Random.Range(0, spriteArray.Length);

            background.sprite = spriteArray[index];
        }

        // display the loading screen
        loadScreen.SetActive(true);

        // unfade to show the loadingScreen
        {// -- FADE IN --
            float t = 1f;
            while (t > 0f)
            {
                t -= Time.deltaTime;
                float alpha = curve.Evaluate(t);
                img.color = new Color(0f, 0f, 0f, alpha);
                yield return 0;             // skip to the next frame
            }
        }// -- /FADE IN --


        while (timeLoadingScreen <= 2f || !operation.isDone)
        {
            if (timeLoadingScreen > 2f)
            {
                operation.allowSceneActivation = true;
                if (operation.isDone)
                {
                    {// -- FADE OUT --
                        float t = 0f;
                        while (t < 1f)
                        {
                            t += Time.deltaTime;
                            float alpha = curve.Evaluate(t);
                            img.color = new Color(0f, 0f, 0f, alpha);
                            yield return 0;             // skip to the next frame
                        }
                    }// -- /FADE OUT --
                }
            }
            timeLoadingScreen += Time.deltaTime;
            // float progress = Mathf.Clamp01(operation.progress / 0.9f);  // if we want the progress
            yield return 0;
        }
    }

    IEnumerator FadeInAnim()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            float alpha = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, alpha);
            yield return 0;             // skip to the next frame
        }
    }
}
