using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace SkalluUtils.Editor.Utils
{
    public static class MethodButtonAttributeHandler
    {
        public struct CustomMethodInfo
        {
            public readonly MethodInfo Method;
            public readonly string Signature;

            public CustomMethodInfo(MethodInfo method, string signature)
            {
                Method = method;
                Signature = signature;
            }
        }
        
        private const BindingFlags DEFAULT_BINDING_FLAGS = 
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetObj"></param>
        /// <param name="methodsToShow"></param>
        public static void DrawMethods([NotNull] Object targetObj, ref CustomMethodInfo[] methodsToShow, ref bool methodsShown)
        {
            CustomMethodInfo[] methods = GetObjectMethods(targetObj, ref methodsToShow);
        
            if (methods.Length > 0)
            {
                GUIStyle oldStyle = GUI.skin.button;
                GUIStyle style = new GUIStyle(GUI.skin.button)
                {
                    fixedHeight = 13f,
                    fontSize = 10
                };

                GUI.skin.button = style;
                if (GUILayout.Button(methodsShown ? "Hide methods" : "Show methods"))
                {
                    methodsShown = !methodsShown;
                }

                GUI.skin.button = oldStyle;

                if (methodsShown)
                {
                    foreach (CustomMethodInfo method in methods)
                    {
                        DrawMethod(targetObj, method);
                    }
                }
            }
        }
        
        private static void DrawMethod([NotNull] Object targetObj, CustomMethodInfo methodInfo)
        {
            ParameterInfo[] parameters = methodInfo.Method.GetParameters();
            if (parameters.Length == 0)
            {
                if (GUILayout.Button($"{methodInfo.Signature}"))
                {
                    methodInfo.Method.Invoke(targetObj, null);
                }
            }
        }

        private static CustomMethodInfo[] GetObjectMethods([NotNull] Object targetObj, ref CustomMethodInfo[] methodsToShow)
        {
            if (methodsToShow == null)
            {
                methodsToShow = targetObj.GetType().GetMethods(DEFAULT_BINDING_FLAGS)
                    .Where(m => m.GetParameters().Length == 0 && m.IsAbstract == false && m.IsStatic == false && m.ReturnType == typeof(void))
                    .Where(m => m.GetCustomAttributes<PropertyAttributes.SerializeMethodAttribute>().Any())
                    .Select(m => new CustomMethodInfo(m, GetMethodSignature(m)))
                    .ToArray();
            }
        
            return methodsToShow;
        }
        
        private static string GetMethodSignature(MethodBase method)
        {
            var buttonName = method.GetCustomAttribute<PropertyAttributes.SerializeMethodAttribute>().ButtonName;
            if (buttonName == string.Empty)
            {
                string accessQualifier = method.IsPublic ? "public" : method.IsPrivate ? "private" : "protected";
        
                return $"{accessQualifier} void {method.Name}()";
            }

            return buttonName;
        }
    }
}