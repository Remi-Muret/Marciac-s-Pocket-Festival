using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public static FadeUI Instance;

    public float fadeDuration;

    private Image image;
    private Color color;

    void Awake()
    {
        Instance = this;

        image = GetComponent<Image>();

        color.a = 0f;
        image.color = color;
    }

    public void Fade()
    {
        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        yield return StartCoroutine(FadeTo(1f, fadeDuration));

        PoseManager.Instance.SpawnCharacters();

        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(FadeTo(0f, fadeDuration));
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = color.a;
        float time = 0f;

        while (time < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            color.a = newAlpha;
            image.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        color.a = targetAlpha;
        image.color = color;
    }
}
