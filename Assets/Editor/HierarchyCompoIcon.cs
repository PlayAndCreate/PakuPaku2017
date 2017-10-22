// HierarchyCompoIcon.cs
// copyright Kazuki_FUKUNAGA 2016 un-Pro.
// 参考：https://gist.github.com/anchan828/046f77585b9e7bac1edb

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Question9_practical : MonoBehaviour
{
    const int iconSize = 16;

    static Question9_practical()
    {
        EditorApplication.hierarchyWindowItemOnGUI += DrawComponentIcons;
    }

    static void DrawComponentIcons(int instanceID, Rect rect)
    {
        rect.x += rect.width;
        rect.width = iconSize;

        Component[] components;
        bool comp = true;
        try {
	         components= GetComponents(instanceID);
	    } catch (System.Exception) { comp = false; }
	    if (!comp) return;

        components = GetComponents(instanceID);
        HashSet<Texture> textures = new HashSet<Texture>();

        foreach (Component component in components)
        {
            textures.Add(AssetPreview.GetMiniThumbnail(component));
        }

        foreach (var texture in textures)
        {
            rect.x -= iconSize;
            GUI.DrawTexture(rect, texture);
        }
    }

    static Component[] GetComponents(int instanceID)
    {
        GameObject sceneGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        return sceneGameObject.GetComponents<Component>();
    }
}