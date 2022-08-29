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
            // �������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� ����ǥ�� �̿�
            // ���� = arctan(y/x)
            // x, y ������ ���ϱ�
            dx = _tower.targetDetector.targetList[0].transform.position.x - this.transform.position.x;
            dy = _tower.targetDetector.targetList[0].transform.position.y - this.transform.position.y;
            // x, y �������� �������� ���� ���ϱ�
            // ������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����
            degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
            _tower.towerRenderer.transform.rotation = Quaternion.Euler(0, 0, degree);
        }
    }
}
