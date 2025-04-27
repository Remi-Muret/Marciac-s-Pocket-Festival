using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public static MainUI Instance { get; private set; }

    public GameObject boutonCartePrefab;
    public Transform conteneurCartes;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void AfficherCartes(List<CarteData> mainJoueur)
    {
        foreach (Transform child in conteneurCartes)
            Destroy(child.gameObject);

        foreach (var carte in mainJoueur)
        {
            GameObject bouton = Instantiate(boutonCartePrefab, conteneurCartes);
            Button buttonComponent = bouton.GetComponent<Button>();
            Image imageComponent = bouton.GetComponent<Image>();

            imageComponent.sprite = carte.icone;

            buttonComponent.onClick.AddListener(() => 
            {
                SelectionManager.Instance.SelectionnerCarte(carte, bouton);
            });
        }
    }

    private void SelectionnerCarte(CarteData carte)
    {
        SelectionManager.Instance.SelectionnerCarte(carte, null);
    }
}
