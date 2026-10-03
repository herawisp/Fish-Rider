using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Obstacle {WaterJet, Piranha, LaserBeam}
public enum FishCoinClusterPattern {Wave, Fish, Coin, Arrow}

public class ContentSpawner : MonoBehaviour {

    //==============================================================================================//
    //==============================================================================================//

    public float Cooldown;
    public float CooldownReductionRate;
    public float Speed;
    public float SpeedIncrementRate;
    public float MinimumCooldown;
    public float MaximumSpeed;
    
    [Header("Content Chances")]
    public int FishCoinClusterChance;
    public int ObstacleChance;

    [Header("Obstacle Chances")]
    public int WaterJetChance;
    public int PiranhaChance;
    public int LaserBeamChance;

    [Header("Object Pools")]
    public ObjectPool WaterJetPool;
    public ObjectPool PiranhaPool;
    public ObjectPool LaserBeamPool;
    public ObjectPool FishCoinClusterPool;

    private float _contentLifetime = 10f;
    private float _time;
    
    //==============================================================================================//
    //==============================================================================================//

    public static ContentSpawner Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    void Update() {
        _time += Time.deltaTime;
        if (_time < Cooldown) return;
        SpawnContent();
        Speed = Mathf.Min(Speed + SpeedIncrementRate, MaximumSpeed);
        Cooldown = Mathf.Max(Cooldown - CooldownReductionRate, MinimumCooldown);
        _time = 0;
    }

    //==============================================================================================//
    //==============================================================================================//

    void SpawnContent() {
        int totalWeight = FishCoinClusterChance + ObstacleChance;
        int randomInt = UnityEngine.Random.Range(0, totalWeight);

        if (randomInt < FishCoinClusterChance) {
            SpawnFishCoinCluster();
            return;
        } else SpawnObstacle();
    }

    void SpawnObstacle() {
        Obstacle obstacle = PickRandomObstacle();
        GameObject obstacleObj;

        switch (obstacle) {
            case Obstacle.WaterJet:
                obstacleObj = WaterJetPool.GetObject();
                Transform obstacleTransform = obstacleObj.transform;
                obstacleTransform.position = new(12, obstacleTransform.position.y, obstacleTransform.position.z);

                WaterJet waterJet = obstacleObj.GetComponent<WaterJet>();
                waterJet.Generate();
                StartCoroutine(WaterJetPool.ReturnLater(obstacleObj, _contentLifetime));
                break;

            case Obstacle.Piranha:
                obstacleObj = PiranhaPool.GetObject();
                Piranha piranha = obstacleObj.GetComponent<Piranha>();
                piranha.Generate();
                StartCoroutine(PiranhaPool.ReturnLater(obstacleObj, _contentLifetime));
                break;

            case Obstacle.LaserBeam:
                obstacleObj = LaserBeamPool.GetObject();
                LaserBeam laserBeam = obstacleObj.GetComponent<LaserBeam>();
                laserBeam.Generate();
                StartCoroutine(LaserBeamPool.ReturnLater(obstacleObj, _contentLifetime));
                break;

            default: break;
        }
    }

    Obstacle PickRandomObstacle() {
        int totalWeight = WaterJetChance + PiranhaChance + LaserBeamChance;
        int randomInt = UnityEngine.Random.Range(0, totalWeight);

        if (randomInt < WaterJetChance) return Obstacle.WaterJet;
        randomInt -= WaterJetChance;

        if (randomInt < PiranhaChance) return Obstacle.Piranha;
        randomInt -= PiranhaChance;

        if (randomInt < LaserBeamChance) return Obstacle.LaserBeam;
        return Obstacle.WaterJet;
    }

    //==============================================================================================//
    //==============================================================================================//
    
    void SpawnFishCoinCluster() {
        GameObject fishCoinClusterObj = FishCoinClusterPool.GetObject();
        Transform transform = fishCoinClusterObj.transform;
        transform.position = new(15, transform.position.y, transform.position.z);

        FishCoinCluster fishCoinCluster = fishCoinClusterObj.GetComponent<FishCoinCluster>();
        fishCoinCluster.Generate();
        StartCoroutine(FishCoinClusterPool.ReturnLater(fishCoinClusterObj, _contentLifetime));
    }

    //==============================================================================================//
    //==============================================================================================//
}
