using UnityEngine;

using System.IO;
public class UICreatorUtil
{
    // ReSharper disable Unity.PerformanceAnalysis
    public static void UICtrlFileGenerator(string filePath, string className)
    {
        var path = Application.dataPath + filePath + className +".cs";
        if (File.Exists(path))
        {
            Debug.LogWarning("file existed");
            return;
        }

        StreamWriter sw = null;
        sw = new StreamWriter(path);
        
        sw.WriteLine("using UnityEngine;\n" +
                     "using System.Collections;\n" +
                     "using UnityEngine.UI;\n" +
                     "using FrameWork.Utils;\n" +
                     "using System.Collections.Generic;\n\n ");
        sw.WriteLine("public class " + className + " : UICtrl {\n");
        sw.WriteLine("\t" + "public override void Awake() {\n");
        sw.WriteLine("\t\t" + "base.Awake();\n");
        sw.WriteLine("\t" + "}" + "\n");
        
        sw.WriteLine("\t" + "void Start() {");
        sw.WriteLine("\t" + "}" + "\n");
        sw.WriteLine("}");
        
        sw.Flush();
        sw.Close();
    }
}
