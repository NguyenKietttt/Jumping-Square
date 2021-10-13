using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class CustomLogs : GenericSingleton<CustomLogs>
{
    private const string TAG = "Custom Log";
    private const string ENABLE_LOG = "ENABLE_LOG";

    private string currentClass
    {
        get 
        {
            var stackTrace = new StackTrace();
            var index = Mathf.Min(stackTrace.FrameCount - 1, 2);

            if (index < 0)
                return "{NoClass}";

            return "{" + stackTrace.GetFrame(index).GetMethod().DeclaringType.Name + "}";
        }
    }

    
    [Conditional(ENABLE_LOG)]
    public void Log(string message)
    {
        if (Debug.isDebugBuild)
            Debug.Log(string.Format("{0}: {1}: {2}", TAG, currentClass, message));
    }

    [Conditional(ENABLE_LOG)]
    public void Warning(bool condition, string message)
    {
        if (condition)
            Debug.LogWarning(string.Format("{0}: {1}: {2}", TAG, currentClass, message));
    }

    [Conditional(ENABLE_LOG)]
    public void Error(bool condition, string message)
    {
        if (condition)
            Debug.LogWarning(string.Format("{0}: {1}: {2}", TAG, currentClass, message));
    }
}