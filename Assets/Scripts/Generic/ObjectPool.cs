using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    Queue<T> _pool;
    T _gameObject;
    Transform _position;
    Func<T, Transform, T> _createFunction;
    int _initObjCounts = 0;

    public ObjectPool(Func<T, Transform, T> createFunction, T gameObject, Transform position)
    {
        _pool = new Queue<T>();
        _createFunction = createFunction;
        _gameObject = gameObject;
        _position = position;
    }

    public ObjectPool(Func<T, Transform, T> createFunction, T gameObject, Transform position, int initialObjectsCount)
    {
        _pool = new Queue<T>();
        _createFunction = createFunction;
        _gameObject = gameObject;
        _position = position;
        _initObjCounts = initialObjectsCount;
        InitPool();
    }

    public void Enqueue(T item)
    {
        lock (_pool)
        {
            if (!_pool.Contains(item))
            {
                _pool.Enqueue(item);
            }
        }
    }

    public T Dequeue()
    {
        lock (_pool)
        {
            if (_pool.Count == 0)
            {
                T item = _createFunction(_gameObject, _position);
                _pool.Enqueue(item);
            }
            T obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
    }

    public int GetCount()
    {
        return _pool.Count;
    }

    public void ClearPool()
    {
        _pool.Clear();
    }

    public void InitPool()
    {
        if (_initObjCounts > 0)
        {
            for (int i = 0; i < _initObjCounts; i++)
            {
                T item = _createFunction(_gameObject, _position);
                _pool.Enqueue(item);
            }
        }
    }
}
