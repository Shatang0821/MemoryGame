using System;
using UnityEditor;
using UnityEngine;

public class UICreator : EditorWindow
{
    private static string _filePath = "/_Scripts/Game/UIControllers/";
    
    [MenuItem("MyTool/UICreator")]
    public static void CreateUI()
    {
        UICreator win = EditorWindow.GetWindow<UICreator>();
        win.titleContent.text = "UICreator";
        win.Show();
    }

    public void OnGUI()
    {
        GUILayout.Label("UIを選んでください");

        if (Selection.activeGameObject != null)
        {
            GUILayout.Label(Selection.activeGameObject.name);
            
            GUILayout.Label(_filePath + Selection.activeGameObject.name + "Ctrl.cs");
        }
        else
        {
            {
                GUILayout.Label("選択していないため、生成できない");
            }
        }

        if (GUILayout.Button("UIスクリプトを生成"))
        {
            if (Selection.activeGameObject != null)
            {
                string className = Selection.activeGameObject.name + "Ctrl";
                UICreatorUtil.UICtrlFileGenerator(_filePath,className);
            }
        }
    }

    public void OnSelectionChange()
    {
        this.Repaint();
    }
}