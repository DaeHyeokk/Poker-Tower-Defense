using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRotator : MonoBehaviour
{
    private Tower _tower;
    private float dx;
    private float dy;
    private float degree;

    void Start()
    {
        _tower = GetComponent<Tower>();
    }

    void Update()
    {
        if (_tower.targetDetector.targetList.Count > 0)
        {
            // 원점으로부터의 거리와 수평축으로부터의 각도를 이용해 위치를 구하는 극좌표계 이용
            // 각도 = arctan(y/x)
            // x, y 변위값 구하기
            dx = _tower.targetDetector.targetList[0].transform.position.x - this.transform.position.x;
            dy = _tower.targetDetector.targetList[0].transform.position.y - this.transform.position.y;
            // x, y 변위값을 바탕으로 각도 구하기
            // 각도가 radian 단위이기 때문에 Mathf.Rad2Deg를 곱해 도 단위를 구함
            degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
            _tower.towerRenderer.transform.rotation = Quaternion.Euler(0, 0, degree);
        }
    }
}
