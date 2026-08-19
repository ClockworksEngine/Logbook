// Copyright Digital Descent, All rights reserved.

using DigitalDescent.Logbook;
using UnityEngine;

/// <summary>
/// Basic static class used for testing the Logbook logging system's various scenarios and ensuring the output provided is as expected.
/// </summary>
public static class LoggingTestClass
{
    /// <summary>
    /// Executes basic logging tests using both the Logging class and Unity's Debug class to
    /// verify things are working as expected.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        // Logging
        Logging.Debug($"This is a class debug message using {nameof(Logging)}.");
        Logging.Info($"This is an class info message using {nameof(Logging)}.");
        Logging.Warning($"This is a class warning message using {nameof(Logging)}.");
        Logging.Error($"This is an class error message using {nameof(Logging)}.");
        Logging.Exception(new System.Exception("Test Exception"));

        Logging.BlankLine();

        // UnityEngine.Debug
        Debug.Log($"This is a class debug message using {nameof(Debug)}.");
        Debug.LogWarning($"This is a class warning message using {nameof(Debug)}.");
        Debug.LogError($"This is an class error message using {nameof(Debug)}.");
        Debug.LogException(new System.Exception("Test Exception"));

        Logging.BlankLine();
    }
}
