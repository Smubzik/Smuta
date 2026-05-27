using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreateUpgradeAssets
{
    #if UNITY_EDITOR
    [MenuItem("Tools/Create Turret Upgrade")]
    public static void CreateTurretUpgrade()
    {
        TurretUpgradeData asset = ScriptableObject.CreateInstance<TurretUpgradeData>();
        AssetDatabase.CreateAsset(asset, "Assets/TurretUpgrade.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        Debug.Log("Создан Turret Upgrade!");
    }
    
    [MenuItem("Tools/Create Player Upgrade")]
    public static void CreatePlayerUpgrade()
    {
        PlayerUpgradeData asset = ScriptableObject.CreateInstance<PlayerUpgradeData>();
        AssetDatabase.CreateAsset(asset, "Assets/PlayerUpgrade.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        Debug.Log("Создан Player Upgrade!");
    }
    #endif
}
