using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T> where T : Component
{
    private GameObject _prefab;
    private Stack<T> _objectStack;

    public ObjectPool(GameObject prefab, int initCount)
    {
        _prefab = prefab;
        _objectStack = new Stack<T>();
        Initialize(initCount);
    }

    private void Initialize(int initCount)
    {
        int newObjectCount = initCount;

        if (_objectStack != null)
            for (int i = 0; i < newObjectCount; i++)
                _objectStack.Push(CreateNewObject());
    }

    private T CreateNewObject()
    {
        T newObject = UnityEngine.Object.Instantiate(_prefab).GetComponent<T>();
        newObject.gameObject.SetActive(false);

        return newObject;
    }

    public T GetObject()
    {
        T retObject;

        if (_objectStack.Count > 0)
            retObject = _objectStack.Pop();
        else
            retObject = CreateNewObject();

        retObject.gameObject.SetActive(true);

        return retObject;
    }

    public void ReturnObject(T getObject)
    {
        getObject.gameObject.SetActive(false);
        _objectStack.Push(getObject);
    }
}

/*
 * File : ObjectPool.cs
 * First Update : 2022/04/21 THU 13:20
 * ��Ÿ�� �߿� ������ �ı��� ����ϰ� �߻��ϰ� �� ���ɼ��� ���� Enemy, Tower ������Ʈ�� �����ϱ� ���� Object Pool ��ũ��Ʈ
 * �̱��� �������� �����Ͽ���, Stack �� �̿��Ͽ� ������Ʈ�� ȿ�������� �������� ����
 * Enemy ������Ʈ�� ��� �� ���̺�� 40������ ���͸� ������ �����̱� ������ Initialize() �� ���� 40���� Enemy ������Ʈ�� �̸� ������.
 * 
 * Update : 2022/04/29 FRI 05:30
 * Tower ������Ʈ�� ������Ʈ Ǯ���� �����Ͽ���.
 * TowerWeapon ������Ʈ�� ���� Tower�� ������ �����ϴµ�, Ÿ�� ������ 10�������� Prefab�� �� 10�� �����ϱ� ������ List �迭�� TowerWeapon ������Ʈ�� ���� ��Ҵ�.
 * �̷����� ���� �����Ϸ��� TowerWeapon Ŭ������ �߻�Ŭ������ ������ ���ε�.. ���� �̻��Ѱ� ����. ������Ʈ ������ ��� ©�� ����ؼ� ����غ��߰ڴ�.
 * ������ �Լ������� �Ű������� �ٸ��� �޴� �Լ� �����ε��� ���� �ܺο��� ȣ���� �� �����̳��� ȿ������ ������.
 * 
 * Update : 2022/04/29 FRI 13:50
 * �ϳ��� �̱��� ������ƮǮ���� ������ ������Ʈ Ÿ�Ը��� �ٸ� �޼��带 ȣ���ϴ� ������� �����ߴ� ���� ��ȿ�����̶�� �Ǵ�.
 * ObjectPool�� ���׸� Ŭ������ �����Ͽ� <T> �� ���� Ÿ�Կ� ���� �������� ������ �� �ֵ��� �����丵��.
 * Ǯ���� ������Ʈ �߿� ���� Ÿ�� ������Ʈ�� �������� �������� ������ �ֱ� ������ �Ű������� �ٸ��� �޴� �޼��带 �����ε��Ͽ� �����Ͽ���.
 * 
 * Update : 2022/04/30 SAT 15:40
 * �ش� ObjectPool�� ����ϸ� �Ͼ �� �ִ� ���� ��Ȳ�� ����Ͽ� ���� ó�� ���ִ� ���� �߰�.
 * 
 * Update : 2022/05/25 WED 08:00
 * �������� �������� �����ϴ� ���� ����.
 */