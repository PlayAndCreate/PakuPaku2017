 // FolderIcon.cs
// copyright Kazuki_FUKUNAGA 2016 un-Pro.
// 参考：http://baba-s.hatenablog.com/entry/2015/05/07/103743

using UnityEditor;
using UnityEngine;

static public class FolderIcon
{
    [InitializeOnLoadMethod]
    static private void OnLoad()
    {
        EditorApplication.projectWindowItemOnGUI += OnGUI;
    }
    
    static private void OnGUI( string guid, Rect selectionRect )
    {
        string path = AssetDatabase.GUIDToAssetPath( guid );

        string fileName = path.Split('/')[path.Split('/').Length -1];
        Texture texture;
        if (texture = Resources.Load<Texture>("FolderIcon/" + fileName))
        {
            Rect pos = selectionRect;
            if (IsUnityVersionNewerThan("5.0.1f1")) pos.x += 2;
            if (pos.height > 16) {
                pos.height -= 16;
            }
            if (pos.height > 64) {
                pos.x += (pos.height - 64)/2;
                pos.y += (pos.height - 64)/2;
                pos.height = 64;
            }
            pos.width = pos.height;
	        GUI.DrawTexture(pos, texture);
        }
    }

    static private bool IsUnityVersionNewerThan(string version = "5.0.1f1") {
        int thresholdVer = ParseUnityVersionToInt(version);
        int nowVer = ParseUnityVersionToInt(Application.unityVersion);
        return (nowVer > thresholdVer);
    }

    static private int ParseUnityVersionToInt(string version = "5.0.1f1") {
        int[] num = new int[]{5, 0, 1, 1};
        num[0] = int.Parse(version.Split('.')[0]);
        num[1] = int.Parse(version.Split('.')[1]);
        num[2] = int.Parse(version.Split('.')[2].Split('f')[0]);
        num[3] = int.Parse(version.Split('.')[2].Split('f')[1]);

        int sum = 0;
        for (int i=0; i<num.Length; i++) {
            sum += num[i] * Mathf.RoundToInt(Mathf.Pow(10, (num.Length - i) * 3));
        }

        return sum;
    }
}