﻿/* ---------------------------------------
 * Sourced from:    https://wiki.unity3d.com/index.php/Singleton
 * Modified by:     Martín Pane (martintayx@gmail.com) (@tayx94)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            07-Jul-17
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;

namespace Tayx.Graphy.Utils
{
    /// <summary>
    /// Be aware this will not prevent a non singleton constructor
    ///   such as `T myT = new T();`
    /// To prevent that, add `protected T () {}` to your singleton class.
    /// 
    /// As a note, this is made as MonoBehaviour because we need Coroutines.
    /// </summary>
    public class G_Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /* ----- TODO: ----------------------------
         * Check if we can seal this class.
         * Add summaries to the variables.
         * Add summaries to the functions.
         * --------------------------------------*/

        #region Variables -> Private

        private static  T       _instance;

        private static  object  _lock       = new object();

        #endregion

        #region Properties -> Public

        public static T Instance
        {
            get
            {
                
                if (_applicationIsQuitting)
                {
                    return null;
                }
                
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            Debug.Log
                            (
                                "[Singleton] An instance of " + typeof(T) +
                                " is trying to be accessed, but it wasn't initialized first. " +
                                "Make sure to add an instance of " + typeof(T) + " in the scene before " +
                                " trying to access it."
                            );
                        }
                    }

                    return _instance;
                }
            }
        }

        #endregion

        #region Methods -> Unity Callbacks

        void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = GetComponent<T>();
            }
        }

        private static bool _applicationIsQuitting = false;
        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it has been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        void OnDestroy()
        {
            if (_instance == this)
            {
                _applicationIsQuitting = true;
            }
        }

        #endregion
    }
}