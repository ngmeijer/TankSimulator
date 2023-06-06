using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private float _transitionTime = 2f;
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private Image _fadeImage;
    private AsyncOperation _operation;
    private bool _currentlyFading;
    private PlayerInputActions _inputActions;
    
    private void Awake()
    {
        FadeSceneIn();
        LoadSceneAsyncWrapper();
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    private void LoadSceneAsyncWrapper()
    {
        StartCoroutine(LoadSceneAsync());
    }
    
    private IEnumerator LoadSceneAsync()
    {
        if (_sceneToLoad == "")
            yield break;

        yield return null;
        
        _operation = SceneManager.LoadSceneAsync(_sceneToLoad);
        _operation.allowSceneActivation = false;
        while (!_operation.isDone)
        {
            yield return null;
        }
    }

    private void FadeSceneIn()
    {
        _currentlyFading = true;
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.DOFade(0, _transitionTime).OnComplete(() => _currentlyFading = false);
    }
    
    private IEnumerator FadeSceneOut()
    {
        _currentlyFading = true;
        _fadeImage.DOFade(1, _transitionTime);
        yield return new WaitForSeconds(_transitionTime + 1);
        _currentlyFading = false;
        _operation.allowSceneActivation = true;
    }

    public void OnPlaySceneClicked()
    {
        StartCoroutine(PlayNextSceneSequence());
    }
    
    public void OnQuitClicked()
    {
        StartCoroutine(QuitGameSequence());
    }

    private IEnumerator PlayNextSceneSequence()
    {
        while (_currentlyFading)
            yield return null;
        
        if (_operation.progress >= 0.9f)
        {
            StartCoroutine(FadeSceneOut());
        }
    }

    private IEnumerator QuitGameSequence()
    {
        StartCoroutine(FadeSceneOut());

        while (_currentlyFading)
            yield return null;
        
        Application.Quit();
    }
}
