using UnityEditor;
using UnityEngine.Device;

public class BuildPipeline
{
    [MenuItem("Build/win/gamejolt")]
    public static void BuildGamejoltWin86()
    {
        var path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        var levels = new[]
        {
            "Assets/Scenes/Gamejolt/MainMenu.unity", 
            "Assets/Scenes/gamejolt/Game.unity"
        };
        
        UnityEditor.BuildPipeline.BuildPlayer(levels, 
            path + "/hexagon-" + Application.version + "-win-gamejolt", 
            BuildTarget.StandaloneWindows, 
            BuildOptions.None);
    }
    
    [MenuItem("Build/win64/gamejolt")]
    public static void BuildGamejoltWin64()
    {
        var path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        var levels = new[]
        {
            "Assets/Scenes/Gamejolt/MainMenu.unity", 
            "Assets/Scenes/gamejolt/Game.unity"
        };
        
        UnityEditor.BuildPipeline.BuildPlayer(levels, 
            path + "/hexagon-" + Application.version + "-win64-gamejolt", 
            BuildTarget.StandaloneWindows64, 
            BuildOptions.None);
    }
    
    [MenuItem("Build/win/epic")]
    public static void BuildEpicWin86()
    {
        var path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        var levels = new[]
        {
            "Assets/Scenes/Epic/MainMenu.unity", 
            "Assets/Scenes/Epic/Game.unity"
        };
        
        UnityEditor.BuildPipeline.BuildPlayer(levels, 
            path + "/hexagon-" + Application.version + "-win-epic", 
            BuildTarget.StandaloneWindows, 
            BuildOptions.None);
    }
    
    [MenuItem("Build/win64/epic")]
    public static void BuildEpicWin64()
    {
        var path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        var levels = new[]
        {
            "Assets/Scenes/Epic/MainMenu.unity", 
            "Assets/Scenes/Epic/Game.unity"
        };
        
        UnityEditor.BuildPipeline.BuildPlayer(levels, 
            path + "/hexagon-" + Application.version + "-win64-epic", 
            BuildTarget.StandaloneWindows64, 
            BuildOptions.None);
    }
}