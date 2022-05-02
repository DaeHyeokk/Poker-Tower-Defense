using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    private Tower _clickTower;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private bool _isTowerMove;

    void Awake()
    {
        // 'MainCamera' �±׸� ������ �ִ� ������Ʈ�� Ž�� �� Camera ������Ʈ ���� ����
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); �� ����
        mainCamera = Camera.main;
        _isTowerMove = false;
    }

    private void Update()
    {
        // ���콺 ���� ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            // ī�޶� ��ġ���� ȭ���� ���콺 Ŀ���� �����ϴ� ����(ray) ����
            // ray.origin : ������ ���� ��ġ (= ī�޶� ��ġ)
            // ray.direction : ������ ���� ����
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // 2D ����͸� ���� 3D ������ ������Ʈ�� ���콺�� �����ϴ� ���
            // ������ �ε����� ������Ʈ�� �����ؼ� hit�� ����
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // ������ �ε��� Ÿ���� Ÿ����� ���콺�� ���� ������ ����ؼ� ���콺�����͸� ����
                if (hit.transform.CompareTag("Tower"))
                {
                    //hit.transform.GetComponent<TowerLevel>().LevelUp();
                    _clickTower = hit.transform.GetComponent<Tower>();
                    _clickTower.MoveTower();
                    _isTowerMove = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            // ���콺�� ���� �� Ÿ���� �����̴� ���̾��ٸ� �ߴ��Ѵ�.
            if(_isTowerMove)
            {
                _clickTower.StopTower();
                _isTowerMove = false;
            }
        }
    }
}


/*
 * File : ObjectDetector.cs
 * First Update : 2022/04/25 MON 10:52
 * �÷��̾��� ���콺 Ŭ��(����Ͽ����� ȭ�� ��ġ)�� �ν��ϰ� �׿� ���� �۾��� ����ϴ� ��ũ��Ʈ
 * Camera ������Ʈ�� ScreenPointToRay() �޼��带 ���� �÷��̾ Ŭ���� ��ǥ�� ���� ������ �߻��Ͽ�
 * ������ �浹�ϴ� ������Ʈ�� ������ hit�� ��� ������Ʈ���� ��ȣ�ۿ��� �����Ѵ�.
 * 
 * Update : 2022/05/02 MON 02:12
 * �ʵ� �������� �ٲٸ鼭 Ÿ�� ���� ���� ����� �ƴ� ���� �߾ӿ��� �����Ǵ� ������� �ٲ�����Ƿ�, 
 * �÷��̾ Ÿ���� ��ġ�ߴ��� Ȯ���ϴ� ���� ����.
 * 
 * Update : 2022/05/02 MON 19:38
 * ���콺�� Ÿ���� Ŭ������ �� Ÿ���� ���콺�� ����ٴϵ��� �ϰ�, 
 * ���콺�� ���� Ÿ���� ���콺�� ����ٴϴ� ���� ���ߵ��� �Ͽ� Ÿ���� �̵� ����.
 */