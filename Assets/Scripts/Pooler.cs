using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pooler : MonoBehaviour {
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Obj objPrefab;
    private int bulletCapacity = 200;
    private int bulletMaxSize = 700;
    private int objCapacity = 500;
    private int objMaxSize = 500;

    private IObjectPool<Bullet> bulletPool;
    private IObjectPool<Obj> objPool;

    private void Awake() {
        bulletPool = new ObjectPool<Bullet>(CreateBullet,
            bull => { bull?.gameObject.SetActive(true); },
            bull => { bull?.gameObject.SetActive(false); },
            bull => {  }, //Destroy(bull.gameObject);
            false, bulletCapacity, bulletMaxSize);

        objPool = new ObjectPool<Obj>(CreateObj,
            obj => { obj?.gameObject.SetActive(true); },
            obj => { obj?.gameObject.SetActive(false); },
            obj => {  }, //Destroy(obj.gameObject);
            false, objCapacity, objMaxSize);
    }
    private Obj CreateObj() {
        Obj objInstance = Instantiate(objPrefab);
        objInstance.MyPool = objPool;
        return objInstance;
    }

    private Bullet CreateBullet() {
        Bullet bullInstance = Instantiate(bulletPrefab);
        bullInstance.MyPool = bulletPool;
        return bullInstance;
    }
    public Obj GetObj() {
        return objPool.Get();
    }
    public Bullet GetBullet() {
        return bulletPool.Get();
    }
}
