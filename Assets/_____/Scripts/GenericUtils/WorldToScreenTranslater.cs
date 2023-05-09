using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldToScreenTranslater
{
    public static void MovePiece(
        Transform piece,
        Transform endPieceContainer,
        float time,
        Ease ease,
        Action callback = null
        )
    {
        piece.transform.parent = endPieceContainer;
        piece.transform.DOLocalMove(Vector3.zero, time).OnComplete(OnComplete).SetEase(ease);
        piece.transform.DOLocalRotate(Vector3.zero, time).SetEase(ease);
        piece.transform.DOScale(1f, time).SetEase(ease);

        void OnComplete()
        {
            callback?.Invoke();
            GameObject.Destroy(piece.gameObject);
        }
    }
}
