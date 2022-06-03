using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    private Tower _clickTower;
    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hit;
    private RaycastHit[] _hits;

    [SerializeField]
    private Canvas _towerInfoCanvas;
    private TowerColorChanger _towerColorChanger;
    private TowerSales _towerSales;
    private GraphicRaycaster _graphicRay;
    private PointerEventData _pointerEventData;
    private List<RaycastResult> _resultList;

    private Ray2D _ray2D;
    private RaycastHit2D _hit2D;
    private RaycastHit2D[] _hits2D;

    void Awake()
    {
        // 'MainCamera' �±׸� ������ �ִ� ������Ʈ�� Ž�� �� Camera ������Ʈ ���� ����
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); �� ����
        _mainCamera = Camera.main;
        _graphicRay = _towerInfoCanvas.GetComponent<GraphicRaycaster>();
        _pointerEventData = new(null);
        //_resultList = new List<RaycastResult>();
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
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            // ������ �ε����� ������Ʈ�� �����ؼ� hit2D�� ����
            if(Physics.Raycast(_ray, out _hit, Mathf.Infinity))
            {
                // ������ �ε��� Ÿ���� Ÿ����� ���콺�� ���� ������ ����ؼ� ���콺�����͸� ����
                if (_hit.transform.CompareTag("Tower"))
                {
                    _clickTower = _hit.transform.GetComponent<Tower>();
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
                // ī�޶� ��ġ���� ȭ���� ���콺 Ŀ���� �����ϴ� ����(ray) ����.
                _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                _hits = Physics.RaycastAll(_ray, Mathf.Infinity);

                bool isHitTile = false;

                for (int i = 0; i < _hits.Length; i++)
                {
                    // ������ �ε��� Ÿ���� Tile�� �� ����.
                    if (_hits[i].transform.CompareTag("Tile"))
                    {
                        isHitTile = true;
                        Tile tile = _hits[i].transform.GetComponent<Tile>();

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

                if (!isHitTile)
                {
                    _pointerEventData.position = Input.mousePosition;

                    _resultList = new List<RaycastResult>();
                    _graphicRay.Raycast(_pointerEventData, _resultList);

                    for (int i = 0; i < _resultList.Count; i++)
                    {
                        if (_resultList[i].gameObject.TryGetComponent<TowerColorChanger>(out _towerColorChanger))
                        {
                            _towerColorChanger.ChangeColor();
                            break;
                        }
                        if (_resultList[i].gameObject.TryGetComponent<TowerSales>(out _towerSales))
                        {
                            _towerSales.SalesTower();
                            break;
                        }
                    }
                }

                _clickTower.StopTower();
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