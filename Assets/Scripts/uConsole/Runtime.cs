using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace UConsole
{
    public class Runtime : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        public KeyCode ActivationKeyBind = KeyCode.BackQuote;

        private string commandStr;

        Dictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>();

        void Start()
        {
            IEnumerable<MethodInfo> methodsInfo = GetMethodsWith<ConsoleCmd>();
            foreach (var methodInfo in methodsInfo)
            {
                methods.Add(methodInfo.Name, methodInfo);
                // ParameterInfo[] paramsInfo = methodInfo.GetParameters();
            }
        }

        // [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            IEnumerable<MethodInfo> methods = GetMethodsWith<ConsoleCmd>();
            foreach (var method in methods)
            {
                Debug.Log(method.Name);
                // Debug.Log(method.GetType());
                // Debug.Log(method.MemberType);	
                // Debug.Log(method.ReflectedType);
                ParameterInfo[] paramsInfo = method.GetParameters();
                System.Object[] methodParams = new System.Object[paramsInfo.Count()];
                // System.Object invokerObj = method.IsStatic ? this : FindObjectOfType(method.ReflectedType);

                for (int i = 0; i < paramsInfo.Count(); i++)
                {
                    ParameterInfo paramInfo = paramsInfo[i];
                    if (paramInfo.ParameterType == typeof(int))
                    {
                        methodParams[i] = 100;
                    }
                }
                // method.Invoke(invokerObj, methodParams);
            }
        }

        private Rect matchListRect;
        public const float CommandConsoleHeight = 50;

        public static IEnumerable<MethodInfo> GetMethodsWith<TAttribute>(bool inherit = true)
            where TAttribute : System.Attribute
        {
            return from assemblies in System.AppDomain.CurrentDomain.GetAssemblies()
                   from types in assemblies.GetTypes()
                   from methods in types.GetMethods()
                   where methods.IsDefined(typeof(TAttribute), inherit)
                   select methods;
        }

        void Update()
        {
            if (Input.GetKeyDown(ActivationKeyBind))
            {
                IsActive = !IsActive;
            }
        }

        string methodName;
        object methodTarget;
        object[] methodParams = new Object[10];
        void InvokeMethod()
        {
            if (methods.ContainsKey(methodName))
            {
                MethodInfo method = methods[methodName];

                if (method.IsStatic)
                {
                    methodTarget = this;
                }
                else
                {
                }

                if (methodTarget != null)
                {
                    method.Invoke(methodTarget, methodParams);
                }
            }
        }

        Regex methodNameRegex;
        const string SeachBarControlName = "SearchBarTextfield";

        int selectedEntry = -1;
        Vector2 scrollVec = Vector2.zero;
        const float scrollHeight = 300;
        const float searchResultLabelHeight = 50;
        List<string> searchResult = new List<string>();

        void OnGUI()
        {
            if (IsActive)
            {
                GUI.FocusControl(SeachBarControlName);

                GUI.SetNextControlName(SeachBarControlName);
                var searchBarStyle = new GUIStyle(GUI.skin.textField);
                GUI.skin.textField.fontSize = 32;
                searchBarStyle.fixedHeight = 0;
                searchBarStyle.fixedHeight = searchBarStyle.CalcHeight(new GUIContent(commandStr), Screen.width);
                commandStr = GUI.TextField(new Rect(0, Screen.height - searchBarStyle.fixedHeight, Screen.width, searchBarStyle.fixedHeight), commandStr, searchBarStyle);

                if (GUI.changed)
                {
                    if (!string.IsNullOrEmpty(commandStr))
                    {
                        // toggle search bar 
                        if ((KeyCode)commandStr.Last() == ActivationKeyBind)
                        {
                            commandStr = string.Empty;
                            IsActive = false;
                        }
                        else
                        {
                            //@todo: state of parser
                            foreach (string methodName in methods.Keys)
                            {
                                if (methodName.ToLower().Contains(commandStr.ToLower()) && !searchResult.Contains(methodName))
                                {
                                    searchResult.Add(methodName);
                                }
                            }
                        }
                    }
                    else
                    {
                        searchResult.Clear();
                    }

                    // navigate through search result
                    if (Event.current.keyCode == KeyCode.UpArrow)
                    {
                        selectedEntry--;
                        if (selectedEntry < 0)
                        {
                            selectedEntry = searchResult.Count - 1;
                        }
                    }
                    else if (Event.current.keyCode == KeyCode.DownArrow)
                    {
                        selectedEntry++;
                        if (selectedEntry >= searchResult.Count)
                        {
                            selectedEntry = 0;
                        }
                    }
                }

                // excecute command
                if (Event.current.type == EventType.keyDown)
                {
                    if (Event.current.keyCode == KeyCode.Return)
                    {
                        IsActive = false;
                    }
                }

                // search result scrollview
                float ScrollStartY = Screen.height - searchBarStyle.fixedHeight - scrollHeight;
                GUI.Box(new Rect(0, ScrollStartY, Screen.width, scrollHeight), string.Empty);
                GUI.skin.label.fontSize = 24;
                scrollVec = GUI.BeginScrollView(new Rect(0, ScrollStartY, Screen.width, scrollHeight), scrollVec, new Rect(0, 0, Screen.width, searchResultLabelHeight * searchResult.Count));
                for (int i = 0; i < searchResult.Count; i++)
                {
                    GUI.color = (selectedEntry == i) ? Color.yellow : Color.white;
                    GUI.Label(new Rect(0, i * searchResultLabelHeight, Screen.width, searchResultLabelHeight), searchResult[i]);
                }
                GUI.EndScrollView();

                // commandStr = GUI.TextArea(seachBarRect, commandStr);
                // if (GUI.changed)
                // {
                //     string[] tokens = commandStr.Split(' ');
                //     string methodNameToken = tokens[0];

                //     foreach (string methodName in methods.Keys)
                //     {
                //         if (Regex.IsMatch(methodName, methodNameToken))
                //         {
                //         }
                //     }
                // }

                // GUI.BeginScrollView(matchListRect, Vector2.zero, matchListRect);
                // GUI.EndScrollView();
            }
        }
    }
}
