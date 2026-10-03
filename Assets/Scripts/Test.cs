using UnityEngine;

public class Test : MonoBehaviour
{

    public WaterJet waterJetPrefab;

    void Start()
    {
        WaterJet waterJet = Instantiate(waterJetPrefab, transform);
        waterJet.Generate(transform);
    }

}
