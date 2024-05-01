using UnityEditor;
using UnityEngine;

namespace DebugUI
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    internal sealed class UpdateDispatcher : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            instance = new GameObject(nameof(UpdateDispatcher)).AddComponent<UpdateDispatcher>();
            DontDestroyOnLoad(instance);
        }

        static UpdateDispatcher instance;

        readonly UpdateRunner updateRunner = new(ex => Debug.LogException(ex));

        public static void Register(IUpdateRunnerItem item)
        {
            instance.updateRunner.Add(item);
        }

        void Update()
        {
            updateRunner.Run();
        }
    }

#if UNITY_EDITOR
    internal sealed class EditorUpdateDispatcher
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            if (instance != null) return;
            instance = new EditorUpdateDispatcher();
            
            EditorApplication.update += Update;
        }
        
        readonly UpdateRunner updateRunner = new(ex => Debug.LogException(ex));
        
        static EditorUpdateDispatcher instance;
        
        public static void Register(IUpdateRunnerItem item)
        {
            instance.updateRunner.Add(item);
        }

        static void Update()
        {
           instance.updateRunner.Run();
        }
    }
#endif
}