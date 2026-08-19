// Copyright Digital Descent, All rights reserved.

using System;
using DigitalDescent.Logbook;
using UnityEngine;

/// <summary>
/// <see cref="MonoBehaviour"/> implementation used for testing the Logbook logging system's various
/// scenarios and ensuring the output provided is as expected.
/// </summary>
[DisallowMultipleComponent]
internal sealed class LoggingTestBehaviour : MonoBehaviour
{
    /// <summary>
    /// Called after Unity loads its first scene. This will automatically
    /// create our GameObjects used for testing the logging system.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        var obj = new GameObject(nameof(LoggingTestBehaviour));
        obj.AddComponent<LoggingTestBehaviour>();

        var obj2 = new GameObject("TestObject");
        obj2.AddComponent<LoggingTestBehaviour>();
    }

    /// <summary>
    /// When the component starts. Log a series of message using both the top level Logging class
    /// and Unity's Debug class to verify the output is as expected and that Unity's object context
    /// is properly supplied.
    /// </summary>
    private void Start()
    {
        // Logging
        Logging.Debug($"This is a Unity object debug message using {nameof(Logging)}.", this);
        Logging.Info($"This is a Unity object info message using {nameof(Logging)}.", this);
        Logging.Warning($"This is a Unity object warning message using {nameof(Logging)}.", this);
        Logging.Error($"This is a Unity object error message using {nameof(Logging)}.", this);
        Logging.Exception(new Exception("Test Exception"), this);

        Logging.BlankLine();

        // UnityEngine.Debug
        Debug.Log($"This is a Unity object debug message using {nameof(Debug)}.", this);
        Debug.LogWarning($"This is a Unity object warning message using {nameof(Debug)}.", this);
        Debug.LogError($"This is a Unity object error message using {nameof(Debug)}.", this);
        Debug.LogException(new Exception("Test Exception"), this);

        Logging.BlankLine();
    }
}