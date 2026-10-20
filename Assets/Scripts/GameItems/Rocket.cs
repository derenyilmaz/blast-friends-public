using System.Collections;
using UnityEngine;

public class Rocket : GameItem
{
    private Coroutine _explodingRoutine;
    
    protected override void TakeDamage()
    {
        if (_explodingRoutine != null)
        {
            return;
        }

        base.TakeDamage();
    }

    public override void Tap()
    {
        EventManager.RocketTapped(this);
    }

    public void Explode()
    {
        if (_explodingRoutine != null)
        {
            return;
        }
        
        _explodingRoutine = StartCoroutine(ExplodeRoutine());
    }
    
    private IEnumerator ExplodeRoutine()
    {
        yield return null;
        
        var size = 4.5f;

        var position = transform.position;
        var raycastHit2Ds = Physics2D.BoxCastAll(
            origin: new Vector2(position.x, position.y), 
            size: Vector2.one * size, 
            angle: 0f, 
            direction:Vector3.forward, 
            distance: 100f,
            layerMask: 1);
        
        foreach (var raycastHit in raycastHit2Ds)
        {
            if (raycastHit.collider != null)
            {
                var affectedItem = raycastHit.collider.GetComponent<GameItem>();

                if (affectedItem != null && affectedItem != this)
                {
                    affectedItem.TakeDamageFromRocket(this);
                }
            }
        }

        _explodingRoutine = null;
        
        Destroy(gameObject);
    }

    public override void Break()
    {
        EventManager.GameItemBroken(this);
        
        Explode();
    }
}
