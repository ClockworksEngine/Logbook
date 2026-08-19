// Copyright Digital Descent, All rights reserved.

using UnityEngine;

#nullable enable
namespace DigitalDescent.Logbook.Framework
{
    /// <summary>
    /// Singleton <see cref="ScriptableObject"/> implementation.
    /// </summary>
    /// <typeparam name="T">Type the singleton represents.</typeparam>
    public class SingletonAsset<T> : ScriptableObject where T : SingletonAsset<T>
    {
        private static T? _instance;

        /// <summary>
        /// Singleton instance of the asset. If it doesn't exist in resources, it will be created.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var results = Resources.FindObjectsOfTypeAll<T>();
                    if (results.Length > 0)
                        _instance = results[0];

                    if (_instance == null)
                        _instance = Resources.Load<T>(typeof(T).Name);

                    if (_instance == null)
                        _instance = CreateInstance<T>();
                }

                return _instance;
            }
        }
    }
}
