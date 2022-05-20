using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    private Tower _clickTower;
    private Vector3 _tempPosition;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private RaycastHit[] hits;

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
                    _clickTower = hit.transform.GetComponent<Tower>();
                    _tempPosition = _clickTower.transform.position;
                    _clickTower.isOnTile = false;
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

                // ī�޶� ��ġ���� ȭ���� ���콺 Ŀ���� �����ϴ� ����(ray) ����
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                hits = Physics.RaycastAll(ray, Mathf.Infinity);

                for (int i = 0; i < hits.Length; i++)
                {
                    // ������ �ε��� Ÿ���� Tile�̰� ���� ����ִ� Ÿ���̶�� �ش� Ÿ�� ���� Ÿ���� ��ġ.
                    if (hits[i].transform.CompareTag("Tile"))
                    {
                        Tile tile = hits[i].transform.GetComponent<Tile>();
                        if (tile.isEmpty)
                        {
                            // �̵���Ű�� Ÿ���� ������ �ٸ� Ÿ�� ���� �־��ٸ� ���� Ÿ���� �� Ÿ�Ϸ� �ٲ��ش�.
                            if (_clickTower.onTile != null)
                                _clickTower.onTile.ToggleIsEmpty();

                            _clickTower.onTile = tile;
                            _clickTower.transform.position = tile.transform.position;
                            _clickTower.isOnTile = true;
                            tile.ToggleIsEmpty();
                            return;
                        }
                    }
                }

                // �� Ÿ������ �̵���Ű�� ��찡 �ƴ϶�� ���� ��ġ�� �ǵ�����.
                _clickTower.transform.position = _tempPosition;

                if (_clickTower.onTile != null)
                    _clickTower.isOnTile = true;
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
 * 
 * Update : 2022/05/18 WED
 * Ÿ���� �������� Ÿ�� ���� ��ġ�ϴ� ���� ����.
 * Ÿ���� �ƴ� ���������� �̵���Ű�ų�, �̹� Ÿ���� ��ġ �Ǿ� �ִ� Ÿ�� ���� �̵���Ű�� ��� ���� ��ġ�� �ǵ��� ������ �����Ͽ���.
 */