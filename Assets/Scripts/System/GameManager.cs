using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public CarteDatabase carteDatabase;  
    public List<CarteData> cartesDisponibles;
    public List<CarteData> mainUI;
    public int pioche;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        cartesDisponibles = carteDatabase.GetCarteData();  
        PiocherCartes(pioche);
    }

    public void PiocherCartes(int nombre)
    {
        mainUI.Clear();

        for (int i = 0; i < nombre; i++)
        {
            var carte = cartesDisponibles[Random.Range(0, cartesDisponibles.Count)];
            mainUI.Add(carte);
        }

        MainUI.Instance.AfficherCartes(mainUI);
    }
}
