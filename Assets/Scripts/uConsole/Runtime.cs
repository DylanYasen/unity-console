using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UConsole
{
    public class Runtime : MonoBehaviour
    {
        public bool IsActive { get; private set; }
        public KeyCode ActivationKeyBind = KeyCode.BackQuote;

        private Rect rect;
        private string token;

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            GetCache();
            IEnumerable<MethodInfo> methods = GetMethodsWith<Exec>();
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

        private static Cache GetCache()
        {
            Cache cache = AssetUtility.GetAsset<Cache>("uConsole");
            if (cache)
            {
                print("found cache");
            }
            else
            {
                cache = AssetUtility.CreateAsset<Cache>("uConsole");
            }

            // print("cache loaded: " + cache.CachedMethodInfo.Count());
            
            return cache;
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            rect = new Rect(100, 100, 200, 50);
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

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(ActivationKeyBind))
            {
                IsActive = !IsActive;
            }
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        void OnGUI()
        {
            if (IsActive)
            {
                token = GUI.TextArea(rect, token);
            }
        }
    }
}
