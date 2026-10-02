using UnityEngine;

public class Test : MonoBehaviour
{

    public WaterJets waterJetPrefab;

    void Start()
    {
        WaterJets waterJet = Instantiate(waterJetPrefab, transform);
        waterJet.Generate(transform);
    }

}
