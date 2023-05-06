using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TutorialTask : MonoBehaviour
{
    internal UnityEvent SucceedEvent { get; private set; } = new UnityEvent();
    private TutorialExtention[] _extentions;
    private bool _IsInitialized;
    private Animator _animator;

    [SerializeField] private ParticleSystem _successParticles;
    [SerializeField] private ParticleSystem _regressParticles;

    internal void Setup()
    {
        if (_IsInitialized) return;
        _animator = this.GetComponent<Animator>();
        _IsInitialized = true;
        _extentions = this.GetComponents<TutorialExtention>();
        _successParticles = this.GetComponentInChildren<ParticleSystem>();
        foreach (var ext in _extentions)
        {
            ext.Setup();
        }
    }

    internal void Show(bool delay)
    {
        gameObject.SetActive(true);
        EnableExt();
        Show_Specific();
        if (delay)
            StartCoroutine(ShowDelay());
        else
            _animator.enabled = true;


        IEnumerator ShowDelay()
        {
            yield return new WaitForSeconds(1f);
            _animator.enabled = true;
        }
    }

    private void EnableExt()
    {
        foreach (var ext in _extentions)
        {
            ext.TurnOn();
        }
    }

    internal void Regress()
    {
        if (_animator != null) _animator.SetTrigger("Regress");
        DisableExt();
        StartCoroutine(HideRoutine());
    }

    internal void Success()
    {
        if (_animator != null) _animator.SetTrigger("Success");
        DisableExt();
        StartCoroutine(HideRoutine());
        Success_Specific();
        SucceedEvent.Invoke();
    }

    private IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void DisableExt()
    {
        foreach (var ext in _extentions)
        {
            ext.TurnOff();
        }
    }

    public void PlayParticles()
    {
        _successParticles.Play();
    }

    public void PlayParticles_regress()
    {
        _regressParticles.Play();
    }



    internal abstract void Show_Specific();
    internal abstract void Success_Specific();

    internal abstract void Progress(int count = 1);
}
