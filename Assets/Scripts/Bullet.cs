using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    private IObjectPool<Bullet> myPool;
    public IObjectPool<Bullet> MyPool { set => myPool = value; }
    public void Setup() {
        Controller.gameOverEvent += Destroy;
        StartCoroutine(DeactivateDelay());
    }

    IEnumerator DeactivateDelay() {
        yield return new WaitForSeconds(4);
        rb.velocity = Vector2.zero;
        Controller.gameOverEvent -= Destroy;
        myPool.Release(this);
    }
    public void Destroy() {
        Controller.gameOverEvent -= Destroy;
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
        myPool.Release(this);
    }

}
