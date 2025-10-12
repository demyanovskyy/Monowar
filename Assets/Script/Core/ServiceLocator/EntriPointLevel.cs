using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Internal;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EntriPointLevel : MonoBehaviour
{

    [SerializeField] private DialogueManeger _dialogueManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private SaveLoadManager _saveloadManager;

    private List<IDisposable> _disposables = new List<IDisposable>();

    private void Awake()
    {
        //_eventBus = new EventBus();

        RegisterServices();
        Init();
        AddDisposables();
    }

    private void RegisterServices()
    {
        ServiceLocator.Initialize();

       
        ServiceLocator.Current.Register<DialogueManeger>(_dialogueManager);
        ServiceLocator.Current.Register<LevelManager>(_levelManager);
        ServiceLocator.Current.Register<SaveLoadManager>(_saveloadManager);
    }

    private void Init()
    {
        _dialogueManager.Init();
        _levelManager.Init();
        _saveloadManager.Init();

    }

    private void AddDisposables()
    {
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}
