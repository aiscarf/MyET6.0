using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ET;
using UnityEditor;
using UnityEngine;
//Object并非C#基础中的Object，而是 UnityEngine.Object
using Object = UnityEngine.Object;

//自定义ReferenceCollector类在界面中的显示与功能
[CustomEditor(typeof (ReferenceCollector))]
//没有该属性的编辑器在选中多个物体时会提示“Multi-object editing not supported”
[CanEditMultipleObjects]
public class ReferenceCollectorEditor: Editor
{
    //输入在textfield中的字符串
    private string searchKey
    {
        get
        {
            return _searchKey;
        }
        set
        {
            if (_searchKey != value)
            {
                _searchKey = value;
                heroPrefab = referenceCollector.Get<Object>(searchKey);
            }
        }
    }

    private ReferenceCollector referenceCollector;

    private Object heroPrefab;

    private string _searchKey = "";

    private void DelNullReference()
    {
        var dataProperty = serializedObject.FindProperty("data");
        for (int i = dataProperty.arraySize - 1; i >= 0; i--)
        {
            var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("gameObject");
            if (gameObjectProperty.objectReferenceValue == null)
            {
                dataProperty.DeleteArrayElementAtIndex(i);
                EditorUtility.SetDirty(referenceCollector);
                serializedObject.ApplyModifiedProperties();
                serializedObject.UpdateIfRequiredOrScript();
            }
        }
    }

    private void OnEnable()
    {
        //将被选中的gameobject所挂载的ReferenceCollector赋值给编辑器类中的ReferenceCollector，方便操作
        referenceCollector = (ReferenceCollector)target;
    }

    public override void OnInspectorGUI()
    {
        //使ReferenceCollector支持撤销操作，还有Redo，不过没有在这里使用
        Undo.RecordObject(referenceCollector, "Changed Settings");
        var dataProperty = serializedObject.FindProperty("data");
        //开始水平布局，如果是比较新版本学习U3D的，可能不知道这东西，这个是老GUI系统的知识，除了用在编辑器里，还可以用在生成的游戏中
        GUILayout.BeginHorizontal();
        //下面几个if都是点击按钮就会返回true调用里面的东西
        if (GUILayout.Button("添加引用"))
        {
            //添加新的元素，具体的函数注释
            // Guid.NewGuid().GetHashCode().ToString() 就是新建后默认的key
            AddReference(dataProperty, Guid.NewGuid().GetHashCode().ToString(), null);
        }

        if (GUILayout.Button("全部删除"))
        {
            referenceCollector.Clear();
        }

        if (GUILayout.Button("删除空引用"))
        {
            DelNullReference();
        }

        if (GUILayout.Button("排序"))
        {
            referenceCollector.Sort();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        //可以在编辑器中对searchKey进行赋值，只要输入对应的Key值，就可以点后面的删除按钮删除相对应的元素
        searchKey = EditorGUILayout.TextField(searchKey);
        //添加的可以用于选中Object的框，这里的object也是(UnityEngine.Object
        //第三个参数为是否只能引用scene中的Object
        EditorGUILayout.ObjectField(heroPrefab, typeof (Object), false);
        if (GUILayout.Button("删除"))
        {
            referenceCollector.Remove(searchKey);
            heroPrefab = null;
        }

        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        var delList = new List<int>();
        SerializedProperty property;
        //遍历ReferenceCollector中data list的所有元素，显示在编辑器中
        for (int i = referenceCollector.data.Count - 1; i >= 0; i--)
        {
            GUILayout.BeginHorizontal();
            //这里的知识点在ReferenceCollector中有说
            property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("key");
            EditorGUILayout.TextField(property.stringValue, GUILayout.Width(150));
            property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("gameObject");
            property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof (Object), true);
            if (GUILayout.Button("X"))
            {
                //将元素添加进删除list
                delList.Add(i);
            }

            GUILayout.EndHorizontal();
        }

        var eventType = Event.current.type;
        //在Inspector 窗口上创建区域，向区域拖拽资源对象，获取到拖拽到区域的对象
        if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
        {
            // Show a copy icon on the drag
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (eventType == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var o in DragAndDrop.objectReferences)
                {
                    AddReference(dataProperty, o.name, o);
                }
            }

            Event.current.Use();
        }

        //遍历删除list，将其删除掉
        foreach (var i in delList)
        {
            dataProperty.DeleteArrayElementAtIndex(i);
        }

        #region 自动获取引用, 自动生成脚本.

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("自动生成UI组件并绑定"))
        {
            AutoGenerateUICode();
        }

        GUILayout.EndHorizontal();

        #endregion

        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }

    //添加元素，具体知识点在ReferenceCollector中说了
    private void AddReference(SerializedProperty dataProperty, string key, Object obj)
    {
        int index = dataProperty.arraySize;
        dataProperty.InsertArrayElementAtIndex(index);
        var element = dataProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("key").stringValue = key;
        element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
    }

    #region 自动生成UI组件并绑定

    private void AutoGenerateUICode()
    {
        // DONE: 自动获取所有引用.
        AutoGetReference();

        // DONE: 自动生成UI组件.
        AutoGenerateUIComponent();

        // DONE: 自动生成UI组件Partial.
        AutoGenerateUIPartial();

        // DONE: 自动生成UIEvent.
        AutoGenerateUIEvent();

        // DONE: 自动生成UIBindidng.
        AutoGenerateUIBinding();

        // DONE: 自动生成UIMediator模板.
        AutoGenerateUIMediator();
    }

    private void AutoGetReference()
    {
        if (this.referenceCollector == null)
            return;
        // DONE: 重新获取UI组件.
        this.referenceCollector.Clear();

        // DONE: 递归获取所有组件.
        GetReferenceRecursion(this.referenceCollector.transform);

        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();

        Debug.Log("自动获取组件引用成功!");
    }

    private void GetReferenceRecursion(Transform root)
    {
        int count = root.childCount;
        if (count <= 0) return;
        for (int i = 0; i < count; i++)
        {
            var item = root.GetChild(i).gameObject;
            if (item == null)
                continue;
            string itemName = item.name;
            if (itemName.StartsWith("EUI_"))
            {
                this.referenceCollector.Add(itemName, item);
            }

            GetReferenceRecursion(item.transform);
        }
    }

    private void AutoGenerateUIComponent()
    {
        if (this.referenceCollector == null)
            return;
        string codeName = this.referenceCollector.name;
        string outputPath = Define.UIComponentOutputDir + codeName;
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        HashSet<string> hsUsing = new HashSet<string>() { "UnityEngine", "UnityEngine.UI", };
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"namespace ET");
        sb.AppendLine("{");
        sb.AppendLine("\t/// *************************************************************************************************");
        sb.AppendLine("\t/// The following code is automatically generated by EUI framework. Please do not modify it manually.");
        sb.AppendLine("\t/// *************************************************************************************************");

        sb.AppendLine($"\t[UITag(UIType.{codeName})]");
        sb.AppendLine($"\tpublic partial class {codeName}Component : Entity");
        sb.AppendLine("\t{");
        var list = this.referenceCollector.data;
        foreach (var info in list)
        {
            var itemName = info.key;
            string[] arr = itemName.Split('_');
            if (arr.Length <= 1)
                continue;
            string classTypeName = arr[1];
            var go = (GameObject)info.gameObject;
            var component = go.GetComponent(classTypeName);
            if (component == null)
                continue;
            string strNamespace = component.GetType().Namespace;
            if (!string.IsNullOrEmpty(strNamespace) && !string.IsNullOrWhiteSpace(strNamespace) && !hsUsing.Contains(strNamespace))
                hsUsing.Add(strNamespace);
            sb.AppendLine($"\t\t public {classTypeName} {itemName};");
        }

        sb.AppendLine("\t}");
        sb.AppendLine("}");

        StringBuilder sbUsing = new StringBuilder();
        foreach (string s in hsUsing)
        {
            sbUsing.AppendLine($"using {s};");
        }

        if (hsUsing.Count > 0)
        {
            sbUsing.AppendLine();
        }

        string csPath = Path.Combine(outputPath, $"{codeName}Component.cs");
        using FileStream txt = new FileStream(csPath, FileMode.Create);
        using StreamWriter sw = new StreamWriter(txt);
        sw.Write(sbUsing.ToString());
        sw.Write(sb.ToString());
        sw.Dispose();
        txt.Dispose();

        Debug.Log($"自动生成{csPath}脚本成功!");
    }

    private void AutoGenerateUIPartial()
    {
        if (this.referenceCollector == null)
            return;
        string codeName = this.referenceCollector.name;
        string outputPath = Define.UIComponentOutputDir + codeName;
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        string csPath = Path.Combine(outputPath, $"{codeName}ComponentPartial.cs");
        if (File.Exists(csPath))
        {
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"namespace ET");
        sb.AppendLine("{");
        sb.AppendLine($"\tpublic partial class {codeName}Component");
        sb.AppendLine("\t{");
        sb.AppendLine("\t}");
        sb.AppendLine("}");

        using FileStream txt = new FileStream(csPath, FileMode.Create);
        using StreamWriter sw = new StreamWriter(txt);
        sw.Write(sb.ToString());
        sw.Dispose();
        txt.Dispose();

        Debug.Log($"自动生成{csPath}脚本成功!");
    }

    private void AutoGenerateUIEvent()
    {
        if (this.referenceCollector == null)
            return;
        string codeName = this.referenceCollector.name;
        string outputPath = Define.UIBindingOutputDir + codeName;
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        string csPath = Path.Combine(outputPath, $"{codeName}Event.cs");
        if (File.Exists(csPath))
        {
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("namespace ET");
        sb.AppendLine("{");
        sb.AppendLine($"\t[UIEventTag(UIType.{codeName})]");
        sb.AppendLine($"\tpublic class {codeName}Event : UIEvent<{codeName}Component>");
        sb.AppendLine("\t{");
        sb.AppendLine("\t}");
        sb.AppendLine("}");

        using FileStream txt = new FileStream(csPath, FileMode.Create);
        using StreamWriter sw = new StreamWriter(txt);
        sw.Write(sb.ToString());
        sw.Dispose();
        txt.Dispose();

        Debug.Log($"自动生成{csPath}脚本成功!");
    }

    private void AutoGenerateUIBinding()
    {
        if (this.referenceCollector == null)
            return;
        string codeName = this.referenceCollector.name;
        string outputPath = Define.UIBindingOutputDir + codeName;
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        HashSet<string> hsUsing = new HashSet<string>() { "UnityEngine", "UnityEngine.UI", };
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"namespace ET");
        sb.AppendLine("{");
        sb.AppendLine("\t/// *************************************************************************************************");
        sb.AppendLine("\t/// The following code is automatically generated by EUI framework. Please do not modify it manually.");
        sb.AppendLine("\t/// *************************************************************************************************");

        sb.AppendLine($"\tpublic partial class {codeName}Mediator");
        sb.AppendLine("\t{");
        sb.AppendLine($"\t\tpublic override void OnAutoBind()");
        sb.AppendLine("\t\t{");
        var list = this.referenceCollector.data;
        foreach (var info in list)
        {
            var itemName = info.key;
            string[] arr = itemName.Split('_');
            if (arr.Length <= 1)
                continue;
            string classTypeName = arr[1];
            var go = (GameObject)info.gameObject;
            var component = go.GetComponent(classTypeName);
            if (component == null)
                continue;
            string strNamespace = component.GetType().Namespace;
            if (!string.IsNullOrEmpty(strNamespace) && !string.IsNullOrWhiteSpace(strNamespace) && !hsUsing.Contains(strNamespace))
                hsUsing.Add(strNamespace);
            sb.AppendLine(
                $"\t\t\t self.{itemName} = this.referenceCollector.Get<GameObject>(nameof(self.{itemName})).GetComponent<{classTypeName}>();");
        }

        sb.AppendLine("\t\t}");
        sb.AppendLine("\t}");
        sb.AppendLine("}");

        StringBuilder sbUsing = new StringBuilder();
        foreach (string s in hsUsing)
        {
            sbUsing.AppendLine($"using {s};");
        }

        if (hsUsing.Count > 0)
        {
            sbUsing.AppendLine();
        }

        string csPath = Path.Combine(outputPath, $"{codeName}Binding.cs");
        using FileStream txt = new FileStream(csPath, FileMode.Create);
        using StreamWriter sw = new StreamWriter(txt);
        sw.Write(sbUsing.ToString());
        sw.Write(sb.ToString());
        sw.Dispose();
        txt.Dispose();

        Debug.Log($"自动生成{csPath}脚本成功!");
    }

    private void AutoGenerateUIMediator()
    {
        if (this.referenceCollector == null)
            return;
        string codeName = this.referenceCollector.name;
        string outputPath = Define.UIBindingOutputDir + codeName;
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        // DONE: 仅首次创建该代码模板.
        string csPath = Path.Combine(outputPath, $"{codeName}Mediator.cs");
        if (File.Exists(csPath))
        {
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"namespace ET");
        sb.AppendLine("{");
        sb.AppendLine($"\tpublic partial class {codeName}Mediator : UIMediator<{codeName}Component>");
        sb.AppendLine("\t{");
        sb.AppendLine($"\t\tpublic override void OnInit()");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t}");
        sb.AppendLine($"\t\tpublic override void OnDestroy()");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t}");
        sb.AppendLine($"\t\tpublic override void OnOpen()");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t}");
        sb.AppendLine($"\t\tpublic override void OnClose()");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t}");
        sb.AppendLine($"\t\tpublic override void OnBeCover()");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t}");
        sb.AppendLine($"\t\tpublic override void OnUnCover()");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t}");
        sb.AppendLine("\t}");
        sb.AppendLine("}");

        using FileStream txt = new FileStream(csPath, FileMode.Create);
        using StreamWriter sw = new StreamWriter(txt);
        sw.Write(sb.ToString());
        sw.Dispose();
        txt.Dispose();

        Debug.Log($"自动生成{csPath}脚本成功!");
    }

    #endregion
}