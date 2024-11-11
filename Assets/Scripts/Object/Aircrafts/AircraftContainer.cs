using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AircraftContainer", menuName = "Aircraft/Aircraft Container")]
public class AircraftContainer : ScriptableObject
{
    public IReadOnlyList<AttackableAircraftInfo> Aircrafts => aircrafts;
    
    [SerializeField] 
    private List<AttackableAircraftInfo> aircrafts;
    
#if UNITY_EDITOR
    [ContextMenu("Find Player Aircrafts")]
    private void findPlayerAircrafts()
    {
        var aircrafts = new List<AttackableAircraftInfo>();
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(AttackableAircraftInfo)}");
        
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var aircraft = AssetDatabase.LoadAssetAtPath<AttackableAircraftInfo>(assetPath);

            aircrafts.Add(aircraft);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        this.aircrafts = aircrafts;
    }
#endif
}