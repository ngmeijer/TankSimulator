using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private float _transitionTime = 2f;

    private AsyncOperation _operation;
    [SerializeField] private Image _fadeImage;
    private bool _currentlyFading;

    private void Awake()
    {
        FadeSceneIn();
        StartCoroutine(LoadSceneAsync());
    }
    
    private IEnumerator LoadSceneAsync()
    {
        yield return null;

        _operation = SceneManager.LoadSceneAsync("FinalScene");
        _operation.allowSceneActivation = false;
        while (!_operation.isDone)
        {
            yield return null;
        }
    }

    private void FadeSceneIn()
    {
        _currentlyFading = true;
        _fadeImage.DOFade(0, _transitionTime).OnComplete(() => _currentlyFading = false);
    }
    
    private IEnumerator FadeSceneOut()
    {
        _currentlyFading = true;
        _fadeImage.DOFade(1, _transitionTime);
        yield return new WaitForSeconds(_transitionTime + 1);

        _currentlyFading = false;
    }


    public void OnPlayClicked()
    {
        StartCoroutine(PlayGameSequence());
    }
    
    public void OnQuitClicked()
    {
        StartCoroutine(QuitGameSequence());
    }

    private IEnumerator PlayGameSequence()
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
