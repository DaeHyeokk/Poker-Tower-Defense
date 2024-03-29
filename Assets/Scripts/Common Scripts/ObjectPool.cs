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
        if (_objectStack != null)
            for (int i = 0; i < initCount; i++)
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

    public void ClearObjectPool()
    {
        while(_objectStack.Count > 0)
        {
            T clearObject = _objectStack.Pop();
            UnityEngine.Object.Destroy(clearObject);
        }
    }
}

/*
 * File : ObjectPool.cs
 * First Update : 2022/04/21 THU 13:20
 * 런타임 중에 생성과 파괴가 빈번하게 발생하게 될 가능성이 높은 Enemy, Tower 오브젝트를 재사용하기 위한 Object Pool 스크립트
 * 싱글톤 패턴으로 구현하였고, Stack 를 이용하여 오브젝트를 효율적으로 관리도록 구현
 * Enemy 오브젝트의 경우 한 웨이브당 40마리의 몬스터를 생성할 예정이기 때문에 Initialize() 를 통해 40개의 Enemy 오브젝트를 미리 생성함.
 * 
 * Update : 2022/04/29 FRI 05:30
 * Tower 오브젝트의 오브젝트 풀링을 구현하였다.
 * TowerWeapon 컴포넌트를 통해 Tower의 종류를 구분하는데, 타워 종류가 10가지여서 Prefab이 총 10개 존재하기 때문에 List 배열에 TowerWeapon 오브젝트를 나눠 담았다.
 * 이런것을 쉽게 관리하려고 TowerWeapon 클래스를 추상클래스로 선언한 것인데.. 뭔가 이상한거 같다. 오브젝트 구조를 어떻게 짤지 계속해서 고민해봐야겠다.
 * 동일한 함수명으로 매개변수를 다르게 받는 함수 오버로딩을 통해 외부에서 호출할 때 조금이나마 효율성을 높였다.
 * 
 * Update : 2022/04/29 FRI 13:50
 * 하나의 싱글톤 오브젝트풀에서 각각의 오브젝트 타입마다 다른 메서드를 호출하는 방식으로 구현했던 것이 비효율적이라고 판단.
 * ObjectPool을 제네릭 클래스로 선언하여 <T> 로 받은 타입에 따라 동적으로 동작할 수 있도록 리팩토링함.
 * 풀링할 오브젝트 중에 현재 타워 오브젝트가 여러개의 프리팹을 가지고 있기 때문에 매개변수를 다르게 받는 메서드를 오버로딩하여 구현하였음.
 * 
 * Update : 2022/04/30 SAT 15:40
 * 해당 ObjectPool을 사용하며 일어날 수 있는 예외 상황에 대비하여 예외 처리 해주는 구문 추가.
 * 
 * Update : 2022/05/25 WED 08:00
 * 여러개의 프리팹을 관리하는 로직 삭제.
 */