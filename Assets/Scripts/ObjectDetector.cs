using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _followTowerPrefabs;

    private FollowTower[] _followTowers;
    private Tower _clickTower;
    private Vector3 _tempPosition;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private RaycastHit[] hits;

    void Awake()
    {
        // 'MainCamera' �±׸� ������ �ִ� ������Ʈ�� Ž�� �� Camera ������Ʈ ���� ����
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); �� ����
        mainCamera = Camera.main;

        _clickTower = null;
    }

    private void Update()
    {
        // ���콺 ���� ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            // �̹� Ÿ���� �����̰� �ִ� ���¶�� �ǳʶڴ�.
            if (_clickTower != null) return;

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
                    _clickTower.MoveTower();
                }
            }
        }
        // ���콺 ���� ��ư�� ���� ��
        if (Input.GetMouseButtonUp(0))
        {
            // Ÿ���� �����̴� ���̾��ٸ� �ߴ��Ѵ�.
            if (_clickTower != null)
            {
                _clickTower.StopTower();

                // ī�޶� ��ġ���� ȭ���� ���콺 Ŀ���� �����ϴ� ����(ray) ����.
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                // ������ �ε����� ��� ������Ʈ�� �迭�� ����.
                hits = Physics.RaycastAll(ray, Mathf.Infinity);

                for (int i = 0; i < hits.Length; i++)
                {
                    // ������ �ε��� Ÿ���� Tile�� �� ����.
                    if (hits[i].transform.CompareTag("Tile"))
                    {
                        Tile tile = hits[i].transform.GetComponent<Tile>();

                        // ���콺�� �� ��ǥ�� ��ġ�� Ÿ���� Ŭ������ Ÿ���� ���� ��ġ ���ִ� Ÿ���̶��
                        // Ÿ���� Ÿ�� ���� ��ġ�ϰų� Ÿ�� ��ġ�� �۾��� ������ �ʿ䰡 ����.
                        if (_clickTower.onTile != null && tile.transform.position == _clickTower.onTile.transform.position)
                            break;

                        // ���� ����ִ� Ÿ���̶�� �ش� Ÿ�� ���� Ÿ���� ��ġ.
                        if (tile.collocationTower == null)
                        {
                            _clickTower.onTile = tile;
                            break;
                        }
                        // �ٸ� Ÿ���� �̹� ��ġ�� Ÿ���̶�� Ÿ�� ��ġ�� �õ�.
                        else
                        {
                            // Ÿ�� ��ġ�⿡ �����ߴٸ� for���� �������´�.
                            if (tile.collocationTower.MergeTower(_clickTower))
                                break;
                        }
                    }
                }

                _clickTower = null;
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
 * 
 * Update : 2022/05/21 SAT
 * Ÿ���� �������� Ÿ�� ���� ���� ��� Ÿ�� ��ġ�⸦ �õ��ϴ� ���� ����.
 */