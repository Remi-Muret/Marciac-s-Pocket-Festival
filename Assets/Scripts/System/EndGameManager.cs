using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance;

    [Header("Fade UI")]
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeDuration;

    [Header("Dialogue UI")]
    [SerializeField] private RectTransform dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float dialogueMoveDistance;
    [SerializeField] private float dialogueMoveDuration;

    [Header("Score Messages")]
    [SerializeField] private int lowScoreThreshold;
    [SerializeField] private int highScoreThreshold;
    [TextArea] public string lowScoreMessage;
    [TextArea] public string midScoreMessage;
    [TextArea] public string highScoreMessage;

    [Header("Arrow")]
    [SerializeField] private GameObject arrow;

    [Header("Reaction Bubbles")]
    [SerializeField] private string angryTrigger;
    [SerializeField] private string happyTrigger;
    [SerializeField] private float lowScoreBubbleRatio;
    [SerializeField] private float midScoreBubbleRatio;
    [SerializeField] private float highScoreBubbleRatio;

    void Awake()
    {
        Instance = this;

        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 0f;
            fadePanel.color = c;
        }
    }

    public IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = fadePanel.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            Color c = fadePanel.color;
            c.a = newAlpha;
            fadePanel.color = c;
            time += Time.deltaTime;
            yield return null;
        }

        Color finalColor = fadePanel.color;
        finalColor.a = targetAlpha;
        fadePanel.color = finalColor;
    }

    public void TriggerEndGameSequence()
    {
        StartCoroutine(EndGameRoutine());
    }

    IEnumerator EndGameRoutine()
    {
        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(FadeTo(1f));

        PoseManager.Instance.SpawnCharacters();

        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(FadeTo(0f));

        yield return StartCoroutine(FinalResult());
    }

    IEnumerator FinalResult()
    {
        int score = ScoreManager.Instance.GetFinalScore();
        UpdateDialogueText(score);

        List<GameObject> characters = PoseManager.Instance.GetSpawnedCharacters();
        TriggerCharacterReactions(score, characters);

        Vector3 startPos = dialoguePanel.localPosition;
        Vector3 endPos = startPos + Vector3.up * dialogueMoveDistance;

        float elapsed = 0f;
        while (elapsed < dialogueMoveDuration)
        {
            dialoguePanel.localPosition = Vector3.Lerp(startPos, endPos, elapsed / dialogueMoveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        dialoguePanel.localPosition = endPos;

        yield return new WaitForSeconds(1f);

        arrow.SetActive(true);
    }


    void UpdateDialogueText(int score)
    {
        if (score < lowScoreThreshold)
            dialogueText.text = lowScoreMessage;
        else if (score < highScoreThreshold)
            dialogueText.text = midScoreMessage;
        else
            dialogueText.text = highScoreMessage;
    }

    void TriggerCharacterReactions(int score, List<GameObject> characters)
    {
        string animationTrigger = score < lowScoreThreshold ? angryTrigger : happyTrigger;

        int count = 0;
        if (score < lowScoreThreshold)
            count = Mathf.CeilToInt(characters.Count * lowScoreBubbleRatio);
        else if (score < highScoreThreshold)
            count = Mathf.CeilToInt(characters.Count * midScoreBubbleRatio);
        else
            count = Mathf.CeilToInt(characters.Count * highScoreBubbleRatio);

        List<GameObject> selected = new List<GameObject>(characters);
        Shuffle(selected);
        selected = selected.GetRange(0, Mathf.Min(count, selected.Count));

        foreach (var character in selected)
        {
            Transform bubble = character.transform.Find("Bubble");
            if (bubble != null)
            {
                bubble.gameObject.SetActive(true);

                Animator animator = bubble.GetComponent<Animator>();
                if (animator != null)
                    animator.SetTrigger(animationTrigger);
            }
        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
