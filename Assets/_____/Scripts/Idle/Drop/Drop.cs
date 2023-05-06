using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Random = UnityEngine.Random;

public enum DropType
{
    Money,
}

public abstract class Drop : MonoBehaviour
{
    internal UnityEvent<Drop> CollectedEvent { get; private set; }
    internal UnityEvent<Drop, TextPopup> SpawnedPopupEvent { get; private set; }
    internal UnityEvent<Drop> OnStackedEvent { get; private set; }
    internal bool IsInStack => _IsInStack;
    internal int Stack => _stackNum;
    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private GameObject[] _stackCoins;
    public DropType Type;
    private Sequence seq;
    private Sequence rotSeq;
    private Tween moveToPlayerTweeen;
    private bool _IsPickedUp;
    private bool _IsPickupable;
    private bool _CheckTrigger;
    protected TextPopup _pickupPopup;
    private Vector3 _pickupPosition;
    protected int _stackNum = 0;
    private bool _IsInStack;
    private bool _IsHoming;
    protected bool _canSpawnPopup = true;
    private float _rangeMin, _rangeMax;
    private TextPopup _textPopupPrefab;
    private Tween _t1;
    private Tween _t2;
    private TextPopup.Factory _textPopupFactory;
    private PlayerIdleMover _playerIdleMover;

    [Inject]
    public void Construct(Settings settings, TextPopup.Factory textPopupFactory, PlayerIdleMover playerIdleMover)
    {
        _playerIdleMover = playerIdleMover;
        _textPopupFactory = textPopupFactory;
        _IsPickupable = false;
        _rangeMin = settings.RangeMin;
        _rangeMax = settings.RangeMax;
        _IsHoming = settings.IsHoming;
        this.transform.position = settings.StartPosition;
        _IsPickupable = false;
        _stackNum = settings.Count;
        CollectedEvent = new UnityEvent<Drop>();
        SpawnedPopupEvent = new UnityEvent<Drop, TextPopup>();
        OnStackedEvent = new UnityEvent<Drop>();
        _canSpawnPopup = true;
    }


    internal void Launch()
    {
        StartCoroutine(MoveRoutine());
    }

    internal void ForbidSpawningPopup()
    {
        _canSpawnPopup = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_IsPickedUp || !_IsPickupable || _IsInStack) return;

        //=Stack===============
        if (other.TryGetComponent(out Drop drop))
        {
            if (drop.Type == Type
            && !drop.IsInStack
            && !drop._IsPickedUp
            && drop._IsPickupable)
            {
                AddToStack(drop);
            }
            return;
        }

        FlyToPlayer();
    }

    private void FlyToPlayer()
    {
        //=Player============
        seq.Kill();
        rotSeq.Kill();

        this.transform.parent = _playerIdleMover.transform;
        moveToPlayerTweeen = this.transform.DOLocalMove(new Vector3(0f, this.transform.localPosition.y, 0f), 0.2f)
            .OnComplete(OnMovedToPlayer);

        _IsPickedUp = true;

        void OnMovedToPlayer()
        {
            CollectedEvent.Invoke(this);

            if (_canSpawnPopup)
            {
                _pickupPopup = _textPopupFactory.Create();
                _pickupPopup.transform.position = this.transform.position + (_pickupPosition - this.transform.position) * 0.5f;
                SpawnedPopupEvent.Invoke(this, _pickupPopup);
            }
            OnPickedUp();

            Destroy(gameObject);
        }
    }

    private void AddToStack(Drop drop)
    {
        _stackNum += drop.Stack;
        SetStackVisually();
        drop.OnAddedToStack();
    }

    private void SetStackVisually()
    {
        if (_stackNum / 3 >= _stackCoins.Length)
        {
            for (int i = 0; i < _stackCoins.Length; i++)
            {
                _stackCoins[i].SetActive(true);
            }
        }
        if (_stackNum / 3 < _stackCoins.Length && _stackNum > 1)
        {
            for (int i = 0; i < _stackNum / 3; i++)
            {
                _stackCoins[i].SetActive(true);

            }
        }
    }

    internal void OnAddedToStack()
    {
        _IsInStack = true;
        OnStackedEvent.Invoke(this);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_CheckTrigger)
        {
            //if (other.TryGetComponent(out Player player))
            //{
            //    _CheckTrigger = false;
            //}
            OnTriggerEnter(other);
        }
    }

    protected abstract void OnPickedUp();


    private IEnumerator MoveRoutine()
    {
        this.transform.eulerAngles = new Vector3(
    0f,
    UnityEngine.Random.Range(0f, 360f),
    0f);

        float angle = Random.Range(0f, Mathf.PI * 2);
        float power = Random.Range(_rangeMin, _rangeMax);
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * power;

        if (Physics.SphereCast(this.transform.position, 0.5f, direction, out RaycastHit hit, direction.magnitude, _wallMask))
        {
            direction = (hit.point + hit.normal * 0.5f) - this.transform.position;
        }

        float vertPower = Random.Range(2f, 3f);
        float time = vertPower / 5f;
        _t2 = this.transform.DOBlendableMoveBy(direction, time).SetEase(Ease.Linear);

        _t1 = DOTween.Sequence()
            .Append(this.transform.DOMoveY(vertPower, time * 0.5f).SetEase(Ease.OutQuad))
            .Append(this.transform.DOMoveY(0.4f, time * 0.5f).SetEase(Ease.InQuad));



        yield return new WaitForSeconds(time);
        seq = DOTween.Sequence()
            .Append(this.transform.DOBlendableLocalMoveBy(Vector3.up * 0.2f, 0.4f).SetEase(Ease.InOutQuad))
            .Append(this.transform.DOBlendableLocalMoveBy(Vector3.down * 0.2f, 0.4f).SetEase(Ease.InOutQuad))
            .SetLoops(-1);

        _pickupPosition = this.transform.position;
        _IsPickupable = true;
        _CheckTrigger = true;

        if (_IsHoming)
            FlyToPlayer();
    }

    private void OnDestroy()
    {
        _t1.Kill();
        _t2.Kill();
        seq.Kill();
        moveToPlayerTweeen.Kill();
    }

    public class Settings
    {
        public float RangeMin;
        public float RangeMax;
        public int Count;
        public Vector3 StartPosition;
        public bool IsHoming;
    }
}
