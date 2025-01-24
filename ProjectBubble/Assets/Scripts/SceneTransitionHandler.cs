using System;
using UnityEngine;

[Serializable]
public class SceneField
{
    [SerializeField] private UnityEngine.Object _sceneAsset;
    [SerializeField] private string _sceneName;
    public string SceneName => _sceneName;

    public static implicit operator string(SceneField obj) { return obj.SceneName; }
}

public class SceneTransitionHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
