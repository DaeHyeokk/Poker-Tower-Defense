using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerBuilder towerBuilder;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    void Awake()
    {
        // 'MainCamera' �±׸� ������ �ִ� ������Ʈ�� Ž�� �� Camera ������Ʈ ���� ����
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); �� ����
        mainCamera = Camera.main;
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
                // ������ �ε��� ������Ʈ�� �±װ� "Tile"�̸�
                if (hit.transform.CompareTag("Tile"))
                {
                    // towerBuilder�� BuildTower(Transform) �޼��� ȣ��
                    towerBuilder.BuildTower(hit.transform);
                }
                else if (hit.transform.CompareTag("Tower"))
                {
                    hit.transform.GetComponent<TowerLevel>().LevelUp();
                }
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
 */