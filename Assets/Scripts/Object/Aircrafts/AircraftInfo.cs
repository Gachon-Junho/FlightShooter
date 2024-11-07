﻿using UnityEngine;

public abstract class AircraftInfo : ScriptableObject
{
    public string Name => aircraftName;
    public float Speed => speed;
    public HitPoint HP => hitPoint ??= new HitPoint(hp);
    
    [SerializeField]
    private string aircraftName;
    
    [SerializeField]
    private float speed;
    
    [SerializeField]
    private int hp;

    [SerializeField] 
    private Aircraft targetPrefab;

    private HitPoint hitPoint;
}