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
        _fadeImage.DOFade(0, _transitionTime);
    }
    
    private IEnumerator FadeSceneOut()
    {
        _fadeImage.DOFade(1, _transitionTime);
        yield return new WaitForSeconds(_transitionTime + 1);

        _operation.allowSceneActivation = true;
    }


    public void OnPlayClicked()
    {
        if (_operation.progress >= 0.9f)
        {
            StartCoroutine(FadeSceneOut());
        }
    }
    
    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
