using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CarteDatabase")]
public class CarteDatabase : ScriptableObject
{
    [SerializeField] private List<CarteData> carteData = new();

    public List<CarteData> GetCarteData() => carteData;
}
