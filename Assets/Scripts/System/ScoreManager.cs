using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bonusScoreText;

    private int score;

    [System.Serializable]
    public class SynergyRule
    {
        public string tileName;
        public string influence;
        public int bonusScore;
    }

    [SerializeField] private List<SynergyRule> synergyRules = new List<SynergyRule>();

    void Awake()
    {
        Instance = this;

        if (bonusScoreText != null)
            bonusScoreText.gameObject.SetActive(false);
    }

    void Update()
    {
        scoreText.text = "Score: " + score;
    }

    public void AddScore(TileData tileData, Dictionary<string, int> surroundingInfluences = null)
    {
        Dictionary<string, int> bonusBreakdown = new Dictionary<string, int>();

        if (surroundingInfluences != null)
            bonusBreakdown = CalculateSynergyBonus(tileData.tileName, surroundingInfluences);

        int bonus = 0;
        foreach (var kvp in bonusBreakdown)
            bonus += kvp.Value;

        int total = tileData.score + bonus;
        score += total;

        if (bonusScoreText != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowBonusCoroutine(bonusBreakdown));
        }
    }

    Dictionary<string, int> CalculateSynergyBonus(string tileName, Dictionary<string, int> influences)
    {
        Dictionary<string, int> bonusBreakdown = new Dictionary<string, int>();

        foreach (var rule in synergyRules)
        {
            if (rule.tileName != tileName) continue;

            if (influences.TryGetValue(rule.influence, out int count))
            {
                int total = rule.bonusScore * count;

                if (bonusBreakdown.ContainsKey(rule.influence))
                    bonusBreakdown[rule.influence] += total;
                else
                    bonusBreakdown[rule.influence] = total;
            }
        }

        return bonusBreakdown;
    }

    IEnumerator ShowBonusCoroutine(Dictionary<string, int> bonusBreakdown)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (var kvp in bonusBreakdown)
            sb.AppendLine((kvp.Value > 0 ? "+" : "") + kvp.Value + $" ({kvp.Key})");

        bonusScoreText.text = sb.ToString();
        bonusScoreText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        bonusScoreText.gameObject.SetActive(false);
    }

}
