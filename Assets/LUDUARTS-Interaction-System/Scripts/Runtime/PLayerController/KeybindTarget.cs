using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-2)]
public class KeybindTarget : MonoBehaviour
{
    [Header("Input")]
    public InputActionAsset inputActions;
    public string actionMapName = "Player";
    public string actionName = "Interact";

    InputAction action;
    InputActionRebindingExtensions.RebindingOperation rebindOp;
    int bindingIndex = 0;

    string SaveKey => $"REBIND_{actionMapName}_{actionName}";

#if UNITY_EDITOR
    string EditorKeyPrefix => $"EDITOR_REBIND_{actionMapName}_{actionName}";
#endif

    void Awake()
    {
        Init();
    }

#if UNITY_EDITOR
    void OnEnable()
    {
        if (!Application.isPlaying)
            Init();
    }
#endif

    bool initialized;

    void Init()
    {
        if (initialized) return;

        if (inputActions == null)
        {
            Debug.LogWarning("inputActions is null");
            return;
        }

        var map = inputActions.FindActionMap(actionMapName, true);
        action = map.FindAction(actionName, true);

        LoadBinding();
        action.Enable();
        initialized = true;
    }

    // ======================
    // PUBLIC API
    // ======================

    public void StartRebind()
    {
        if (rebindOp != null)
            return;

        if(action == null)
        {
            Init();
        }

        action.Disable();

        rebindOp = action
            .PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .WithControlsExcluding("<Mouse>/delta")
            .OnComplete(op =>
            {
                FinishRebind();
            })
            .OnCancel(op =>
            {
                CancelRebind();
            });

        rebindOp.Start();
    }

    public void CancelRebind()
    {
        if (rebindOp == null)
            return;

        rebindOp.Dispose();
        rebindOp = null;

        action.Enable();
    }

    void FinishRebind()
    {
        SaveBinding();
        rebindOp.Dispose();
        rebindOp = null;

        action.Enable();
        Debug.Log($"[{actionName}] rebound");
    }

    // ======================
    // SAVE / LOAD
    // ======================

    void SaveBinding()
    {
        string json = inputActions.SaveBindingOverridesAsJson();

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorPrefs.SetString(EditorKeyPrefix, json);
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
            return;
        }
#endif
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }


    void LoadBinding()
    {
        string json = null;

        bool runtimeloaded = false;
        if (PlayerPrefs.HasKey(SaveKey))
        {
            json = PlayerPrefs.GetString(SaveKey);
            runtimeloaded = true;
        }
#if UNITY_EDITOR
        if (!runtimeloaded && EditorPrefs.HasKey(EditorKeyPrefix))
        {
            json = EditorPrefs.GetString(EditorKeyPrefix);
        }
#endif

        if (string.IsNullOrWhiteSpace(json))
            return;

        if (json.Trim() == "{}")
            return;

        try
        {
            inputActions.LoadBindingOverridesFromJson(json);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"[Keybind] Load failed, keeping defaults.\n{e}");
        }
    }


    public bool IsListening()
    {
        return rebindOp != null;
    }

    public InputAction GetAction()
    {
        return action;
    }
}
