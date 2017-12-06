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
        public KeyCode ActivationKeyBind = KeyCode.BackQuote;

        private bool isActive;
        private string commandStr;

        private Dictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>();
        private Dictionary<string, ParameterInfo[]> methodParameters = new Dictionary<string, ParameterInfo[]>();

        private const string seachBarControlName = "SearchBarTextfield";
        private const int maxSearchResultCount = 10;
        private const float searchResultLabelHeight = 50;
        private int selectedEntry = -1;
        private Vector2 scrollVec = Vector2.zero;
        private List<string> searchResult = new List<string>();

        void Start()
        {
            IEnumerable<MethodInfo> methodsInfo = GetMethodsWith<ConsoleCmd>();
            foreach (var methodInfo in methodsInfo)
            {
                methods.Add(methodInfo.Name, methodInfo);
                methodParameters.Add(methodInfo.Name, methodInfo.GetParameters());
            }
        }

        // [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
        }


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
                isActive = !isActive;
            }
        }

        void InvokeMethod(string commandStr)
        {
            string[] tokens = commandStr.Split(' ');
            if (tokens.Length < 1) { return; }

            object methodTarget = null;
            string methodName = tokens[0];
            if (methods.ContainsKey(methodName))
            {
                int paramStartIndex = 0;
                MethodInfo method = methods[methodName];

                // determine method target
                if (method.IsStatic)
                {
                    methodTarget = this;
                    paramStartIndex = 1;
                }
                else
                {
                    Debug.LogWarning("uConsole: non-static methods are not supported yet.");
                    return;
                    // if (tokens.Length < 2) return;
                    // string targetObjName = tokens[1];

                    // // find the correct target to invoke the method
                    // GameObject TargetGameObj = GameObject.Find(targetObjName);
                    // if (TargetGameObj != null && methodTarget.GetType() != method.ReflectedType)
                    // {
                    //     methodTarget = TargetGameObj.GetComponent(method.ReflectedType);
                    // }
                    // paramStartIndex = 2;
                }

                // parse parameters
                if (!methodParameters.ContainsKey(methodName)) { return; }
                ParameterInfo[] paramInfos = methodParameters[methodName];
                object[] methodParams = new object[paramInfos.Length];
                for (int i = 0; i < paramInfos.Length; i++)
                {
                    ParameterInfo paramInfo = paramInfos[i];

                    if (tokens.Length <= paramStartIndex + i)
                    {
                        Debug.LogWarning("uConsole: not enough parameters provided");
                        break;
                    }
                    string paramStr = tokens[paramStartIndex + i];

                    if (paramInfo.ParameterType == typeof(int))
                    {
                        methodParams[i] = int.Parse(paramStr);
                    }
                    else if (paramInfo.ParameterType == typeof(float))
                    {
                        methodParams[i] = float.Parse(paramStr);
                    }
                    else if (paramInfo.ParameterType == typeof(string))
                    {
                        methodParams[i] = paramStr;
                    }
                }

                if (methodTarget != null)
                {
                    method.Invoke(methodTarget, methodParams);
                }
            }
        }

        void RefreshSearchResult(string commandStr)
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

        void NavigateSearchResult(KeyCode dirKey)
        {
            if (dirKey == KeyCode.UpArrow)
            {
                selectedEntry--;
                if (selectedEntry < 0)
                {
                    selectedEntry = searchResult.Count - 1;
                }
            }
            else if (dirKey == KeyCode.DownArrow)
            {
                selectedEntry++;
                if (selectedEntry >= searchResult.Count)
                {
                    selectedEntry = 0;
                }
            }
            scrollVec.y = selectedEntry * searchResultLabelHeight;
        }

        void CloseConsole()
        {
            isActive = false;
            selectedEntry = -1;
            commandStr = string.Empty;
            searchResult.Clear();
        }

        void OnGUI()
        {
            if (isActive)
            {
                GUI.FocusControl(seachBarControlName);

                GUI.SetNextControlName(seachBarControlName);
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
                            CloseConsole();
                        }
                        else
                        {
                            RefreshSearchResult(commandStr);
                        }
                    }
                    else
                    {
                        searchResult.Clear();
                    }

                    // navigate through search result
                    if (Event.current.keyCode == KeyCode.UpArrow || Event.current.keyCode == KeyCode.DownArrow)
                    {
                        NavigateSearchResult(Event.current.keyCode);
                    }
                }

                // keyboard interaction
                if (Event.current.type == EventType.KeyUp)
                {
                    // excecute command
                    if (Event.current.keyCode == KeyCode.Return)
                    {
                        InvokeMethod(commandStr);

                        CloseConsole();
                    }
                    // select search result
                    else if (Event.current.keyCode == KeyCode.Tab)
                    {
                        if (searchResult.Count <= selectedEntry) { return; }
                        commandStr = searchResult[selectedEntry];
                    }
                }

                // search result scrollview
                float searchResultHeight = searchResultLabelHeight * searchResult.Count;
                float clampedResultHeight = Mathf.Clamp(searchResultHeight, 0, maxSearchResultCount * searchResultLabelHeight);
                float scrollStartY = Screen.height - searchBarStyle.fixedHeight - clampedResultHeight;
                GUI.Box(new Rect(0, scrollStartY, Screen.width, clampedResultHeight), string.Empty);
                GUI.skin.label.fontSize = 24;
                scrollVec = GUI.BeginScrollView(new Rect(0, scrollStartY, Screen.width, clampedResultHeight), scrollVec, new Rect(0, 0, Screen.width, searchResultHeight));
                for (int i = 0; i < searchResult.Count; i++)
                {
                    GUI.color = (selectedEntry == i) ? Color.yellow : Color.white;
                    GUI.Label(new Rect(0, i * searchResultLabelHeight, Screen.width, searchResultLabelHeight), searchResult[i]);
                }
                GUI.EndScrollView();
            }
        }
    }
}
