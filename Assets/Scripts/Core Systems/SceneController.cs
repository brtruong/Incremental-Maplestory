using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioSystem;

namespace CoreSystems
{
    public class SceneController : MonoBehaviour
    {
        // Members
        public static SceneController Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private Logger _Logger;
        
        [Header("Scene Objects")]
        [SerializeField] private SceneObject[] _Scenes;

        // Private
        private Dictionary<string, SceneObject> _SceneTable;
        private SceneObject _LoadedScene;

#region Unity Functions
        private void Awake ()
        {
            Configure();
        }
#endregion
#region Public Functions
        public void LoadScene (string sceneName)
        {
            _Logger.Log(gameObject, "Loading Scene {" + sceneName + "}");

            if (!_SceneTable.ContainsKey(sceneName))
            {
                _Logger.Log(gameObject, "Unable to find Scene {" + sceneName + "}");
                return;
            }

            if (CheckIfLoaded(sceneName))
            {
                _LoadedScene = _SceneTable[sceneName];
                if (sceneName == "Level 1") GameManager.Instance.StartGame();
                return;
            }

            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            if (_LoadedScene != null)
            {
                _Logger.Log(gameObject, "Unloading Scene {" + _LoadedScene.Name + "}");
                SceneManager.UnloadSceneAsync(_LoadedScene.Name);
            }

            _LoadedScene = _SceneTable[sceneName];
            StartCoroutine(WaitLoadScene(sceneName));
        }
#endregion
#region Private Functions
        private void Configure ()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            // Generate Scene Table
            _SceneTable = new Dictionary<string, SceneObject>();
            foreach (SceneObject scene in _Scenes)
                _SceneTable.Add(scene.Name, scene);

            _LoadedScene = null;
            LoadUIScene();
        }

        private void LoadUIScene ()
        {
            if (!CheckIfLoaded("UI")) SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        }

        private bool CheckIfLoaded (string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).IsValid()) return true;
            else return false;
        }

        private IEnumerator WaitLoadScene (string sceneName)
        {
            while (!SceneManager.GetSceneByName(sceneName).isLoaded)
                yield return null;

            _Logger.Log(gameObject, "Done Loading Scene {" + sceneName + "}");
            if (sceneName == "Level 1") GameManager.Instance.StartGame();
            AudioManager.Instance.PlayAudio(_LoadedScene.BGM);
        }
#endregion
#region Scene Object
        [System.Serializable]
        public class SceneObject
        {
            public string Name;
            public AudioClip BGM;
        }
#endregion
    }
}
