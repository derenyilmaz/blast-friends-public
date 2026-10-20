using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class GameItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> stageSprites;
    
    public Constants.GameItemType gameItemType;
    public bool canFallDown;
    
    public int health;
    public int xIndex;
    public int yIndex;


    protected FlyToTaskPanelComponent FlyToTaskPanelComponent;
    
    
    private const int ShakeLoopCount = 3;
    private const float RotationOnTapDegrees = 3f;
    private const float RotationOnTapDurationInSeconds = 0.132f;

    
    private List<ParticleSystem> _breakParticles;
    
    private RocketDamageSource _rocketDamageSource;
    private BoundaryDamageSource _boundaryDamageSource;
    
    
    private Sequence _tapSequence;
    
    
    private void Awake()
    {
        _rocketDamageSource = GetComponent<RocketDamageSource>();
        _boundaryDamageSource = GetComponent<BoundaryDamageSource>();

        FlyToTaskPanelComponent = GetComponent<FlyToTaskPanelComponent>();
        
        _breakParticles = GetComponentsInChildren<ParticleSystem>().ToList();
    }

    public virtual void Tap()
    {
    }

    public virtual void TakeDamageFromRocket(Rocket rocket)
    {
        if (_rocketDamageSource != null)
        {
            TakeDamage();
        }
    }

    public virtual void TakeDamageFromBoundary()
    {
        if (_boundaryDamageSource != null)
        {
            TakeDamage();
        }
    }

    protected virtual void TakeDamage()
    {
        health--;

        if (health == 0)
        {
            Break();
            return;
        }

        spriteRenderer.sprite = stageSprites[health - 1];
    }

    public virtual void Break()
    {
        EventManager.GameItemBroken(this);

        foreach (var particle in _breakParticles)
        {
            particle.transform.SetParent(null);
            particle.Play();
        }
        
        Destroy(gameObject);
    }

    public void SetSortingOrder(int sortingOrder)
    {
        spriteRenderer.sortingOrder = sortingOrder;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }

    public virtual void AttachAction(Tile tile)
    {
        
    }
    
    protected void PlayShakeTween()
    {
        if (_tapSequence != null && _tapSequence.IsPlaying())
        {
            return;
        }

        _tapSequence = DOTween.Sequence();

        _tapSequence.Append(
            transform.DORotate(
                new Vector3(0f, 0f, -RotationOnTapDegrees), RotationOnTapDurationInSeconds / 4));

        _tapSequence.Append(
            transform.DORotate(
                new Vector3(0f, 0f, RotationOnTapDegrees), RotationOnTapDurationInSeconds / 2));

        _tapSequence.Append(
            transform.DORotate(
                new Vector3(0f, 0f, 0), RotationOnTapDurationInSeconds / 4));


        _tapSequence.SetLoops(ShakeLoopCount);
    }
}
