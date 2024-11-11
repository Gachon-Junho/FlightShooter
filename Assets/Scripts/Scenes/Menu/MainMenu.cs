using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private TMP_Dropdown aircrafts;

    [SerializeField] 
    private TMP_InputField stageNumber;

    [SerializeField] 
    private Button start;

    private int aircraftIndex;
    private int stageIndex;
    
    void Awake()
    {
        aircrafts.onValueChanged.AddListener(i => aircraftIndex = i);
        stageNumber.onEndEdit.AddListener(v =>
        {
            stageIndex = int.Parse(v) - 1;
            stageNumber.text = Math.Clamp(stageIndex + 1, 1, 40).ToString();
        });
        start.onClick.AddListener(loadScene); 
    }

    private void loadScene()
    {
        
    }

    void Update()
    {
        
    }
    
#if UNITY_EDITOR
    [ContextMenu("findPlayerAircrafts")]
    private void findPlayerAircrafts()
    {
        var aircrafts = new List<PlayerAircraft>();
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(PlayerAircraft)}");
        
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var aircraft = AssetDatabase.LoadAssetAtPath<PlayerAircraft>(assetPath);

            aircrafts.Add(aircraft);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
