using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Obj : MonoBehaviour
{
    [SerializeField] GameObject sprite;
    [SerializeField] CircleCollider2D coll;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPoint;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] List<Color> colors;

    private IObjectPool<Obj> myPool;
    private float bulletVelocity = 250;
    private int hp;
    private Vector3 myPos;
    private float rotateTimer;
    private float shootTimer;
    private float rotateDelay;
    private bool go = false;
    private float scale;
    public IObjectPool<Obj> MyPool { set => myPool = value; }

    public void Setup(Vector3 pos, float scale, int colorIndex) {
        hp = 3;
        rotateTimer = shootTimer = 0;
        transform.position = pos;
        transform.localScale = 2 * scale * Vector3.one;
        go = false;
        this.scale = scale;
        rotateDelay = Random.value;
        Invoke(nameof(StartTimers), Random.value);
        Controller.gameOverEvent += Release;
        sr.color = colors[colorIndex];
        myPos = transform.position;
        coll.enabled = true;
        sprite.SetActive(true);
    }

    void StartTimers() {
        go = true;
    }
    private void Update() {
        if (go && Controller.Instance.gameOn) {
            shootTimer += Time.deltaTime;
            rotateTimer += Time.deltaTime;
            if(shootTimer >= 1) {
                Shoot();
                shootTimer = 0;
            }
            if(rotateTimer >= rotateDelay) {
                rotateTimer = 0;
                rotateDelay = Random.value;
                Rotate();
            }
        }
    }

    void Rotate() {
        transform.rotation  =Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    void Shoot() {
        Bullet bull = Controller.Instance.pooler.GetBullet();
        bull.transform.position = shootPoint.position;
        bull.rb.AddForce(transform.up * bulletVelocity);
        bull.transform.localScale = 2 * scale * Vector3.one;
        bull.Setup();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (go) {
            collision.GetComponent<Bullet>().Destroy();
            Damage();
        }
    }

    private void Damage() {
        go = false;
        hp--;
        coll.enabled = false;
        sprite.SetActive(false);

        //add position back to the list before getting new one, so it MIGHT get the same position
        Controller.Instance.positions.Add(myPos);
        if (hp > 0) {
            //darken the color for a better visibility of hp
            sr.color = new Color(.55f*sr.color.r , .55f * sr.color.g, .55f * sr.color.b);

            myPos = Controller.Instance.GetNewPosition();
            Invoke(nameof(Respawn), 2f);
        } else {
            Controller.Instance.ObjDied();
            Release();
        }
    }

    public void Release() {
        Controller.gameOverEvent -= Release;
        myPool.Release(this);
    }

    void Respawn() {
        go = true;
        sprite.SetActive(true);
        transform.position = myPos;
        coll.enabled = true;
    }
}
