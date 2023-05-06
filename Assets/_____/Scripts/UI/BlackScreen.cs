using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    [SerializeField] private FadeTweenAnim _fadeIn;
    [SerializeField] private FadeTweenAnim _fadeOut;

    private Coroutine _startAnimationRoutine;

    public void FadeIn()
    {
        _fadeIn.Play();
    }

    public void FadeOut()
    {
        _fadeOut.Play();
    }

    public void PlayStartAnimation()
    {
        _startAnimationRoutine = StartCoroutine(StartAnimationRoutine());
    }

    public void StopStartAnimation()
    {
        StopCoroutine(_startAnimationRoutine);
    }

    private IEnumerator StartAnimationRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(4f-0.4f);
            _fadeIn.Play();
            yield return new WaitForSeconds(0.4f);
            _fadeOut.Play();
            yield return new WaitForSeconds(4f - 0.4f);
            _fadeIn.Play();
            yield return new WaitForSeconds(0.4f);
            _fadeOut.Play();
            yield return new WaitForSeconds(4f - 0.4f);
            _fadeIn.Play();
            yield return new WaitForSeconds(0.4f);
            _fadeOut.Play();
            yield return new WaitForSeconds(6f - 0.4f);
            _fadeIn.Play();
            yield return new WaitForSeconds(0.4f);
            _fadeOut.Play();
        }
    }
}
