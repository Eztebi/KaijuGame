using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private bool _addToDontDestroyOnLoad = false;

    private GameObject _emptyHolder;

    private static GameObject _bulletEmpty;
    private static GameObject _walkFXsEmpty;
    private static GameObject _gameObjectsEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
    private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

    public enum PoolType
    {
        WalkParticles,
        BulletType,
        GameObjects
    }

    private void Awake()
    {
        _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();

        SetUpEmpties();
    }

    private void SetUpEmpties()
    {
        _emptyHolder = new GameObject("Object Pools");

        _bulletEmpty = new GameObject("Bullet Pools");
        _bulletEmpty.transform.SetParent(_emptyHolder.transform);

        _walkFXsEmpty = new GameObject("Walk FX Pools");
        _walkFXsEmpty.transform.SetParent(_emptyHolder.transform);

        if (_addToDontDestroyOnLoad) DontDestroyOnLoad(_bulletEmpty.transform.root);
    }

    private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, pos, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyGameObject
            );
        _objectPools.Add(prefab, pool);
    }
    private static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        GameObject obj = Instantiate(prefab, pos, rot);
        obj.SetActive(false);

        GameObject parentObject = SetParentObject(poolType);
        if (parentObject != null)
        {
            obj.transform.SetParent(parentObject.transform);
        }

        return obj;

    }
    private static void OnGetObject(GameObject obj)
    {

    }
    private static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }
    private static void OnDestroyGameObject(GameObject obj) {
        if (_cloneToPrefabMap.ContainsKey(obj))
        {
            _cloneToPrefabMap.Remove(obj);
        }
    }
    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.GameObjects:
                return _gameObjectsEmpty;
            case PoolType.BulletType:
                return _bulletEmpty;
            case PoolType.WalkParticles:
                return _walkFXsEmpty;
            default: 
                return null;
        }
    }
    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Object
    {
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPos, spawnRotation, poolType);
        }

        GameObject obj = _objectPools[objectToSpawn].Get();

        if(obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            obj.transform.position = spawnPos;
            obj.transform.rotation = spawnRotation;
            obj.SetActive(true);

            if(typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }
            T component = obj.GetComponent<T>();
            if(component == null)
            {
                Debug.LogError($"{objectToSpawn.name} No tinene componente type {typeof(T)}");
                return null;
            }
            return component;
        }
        return null;
    }

    public static T SpawnObject<T>(T typePrefab, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, spawnPos, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(objectToSpawn, spawnPos, spawnRotation, poolType);
    }

    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObjects)
    {
        if(_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObjec = SetParentObject(poolType);

            if(obj.transform.parent != parentObjec.transform)
            {
                obj.transform.SetParent(parentObjec.transform);
            }

            if(_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("Trying To return object that is not pooled" + obj.name);
        }
    }

}
