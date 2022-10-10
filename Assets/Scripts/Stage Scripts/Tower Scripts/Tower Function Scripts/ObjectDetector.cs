using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerColorChanger _towerColorChanger;
    [SerializeField]
    private TowerSales _towerSales;
    [SerializeField]
    private TowerDetailInfo _towerDetailInfo;
    [SerializeField]
    private GraphicRaycaster _towerInfoGraphicRay;
    [SerializeField]
    private GraphicRaycaster _towerDetailInfoCanvasGraphicRay;

    private Tower _clickTower;
    private Camera _mainCamera;
    private PointerEventData _pointerEventData;
    private Ray _ray;
    private RaycastHit[] _hits;
    private List<RaycastResult> _resultList = new();

    public int popupUICount { get; set; }

    private void Awake()
    {
        // 'MainCamera' �±׸� ������ �ִ� ������Ʈ�� Ž�� �� Camera ������Ʈ ���� ����
        _mainCamera = Camera.main;
        _pointerEventData = new(null);
        _clickTower = null;
    }

    private void Update()
    {
        // ȭ��� �˾� UI�� �Ѱ� �̻� Ȱ��ȭ�� ���¶�� �÷��̾��� Ÿ�� ��ġ�Է��� �������� �ʴ´�.
        if (popupUICount > 0)
        {
            if (_clickTower != null)
            {
                _clickTower.StopTower();
                _clickTower = null;
            }

            return;
        }

        // ���콺 ���� ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            // ���� Ÿ���� �����̴� ���̾��ٸ� ����Ѵ�. (���� ����)
            if (_clickTower != null)
            {
                _clickTower.StopTower();
                _clickTower = null;
            }

            // ī�޶� ��ġ���� ȭ���� ���콺 Ŀ���� �����ϴ� ����(ray) ����
            // ray.origin : ������ ���� ��ġ (= ī�޶� ��ġ)
            // ray.direction : ������ ���� ����
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            _hits = Physics.RaycastAll(_ray, Mathf.Infinity);


            for (int i = 0; i < _hits.Length; i++)
            {              
                // ������ �ε��� Ÿ���� Ÿ���� �� ����.
                if (_hits[i].transform.CompareTag("Tower"))
                {
                    // ���� ������ ���� Ÿ���� ���ų� �̹� �Ҵ�� _clickTower�� �����Ǻ��� z���� ���� ���(��ũ�� �� ���� ��ġ�ϴ� Ÿ���� ���) _clickTower�� �Ҵ��Ѵ�.
                    if (_clickTower == null || _clickTower.transform.position.z > _hits[i].transform.position.z)
                        _clickTower = _hits[i].transform.GetComponent<Tower>();
                }
            }

            // ������ ���� Ÿ���� ������ ��� Ÿ���� �����̴� �޼ҵ带 ȣ���Ѵ�.
            if (_clickTower != null)
                _clickTower.MoveTower();
        }
        // ���콺 ���� ��ư�� ���� ��
        if (Input.GetMouseButtonUp(0))
        {
            // Ÿ���� �����̴� ���̾��ٸ� ����.
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

                // ������ Tile �� �ε����� �ʾ��� �� ����
                if (!isHitTile)
                {
                    _pointerEventData.position = Input.mousePosition;

                    if (_resultList.Count != 0) _resultList.Clear();
                    // Tower Info Canvas �ȿ� ��ġ�� UI�� �浹�ϴ� ������ �߻��Ѵ�.
                    _towerInfoGraphicRay.Raycast(_pointerEventData, _resultList);

                    for (int i = 0; i < _resultList.Count; i++)
                    {
                        if (_resultList[i].gameObject.CompareTag("TowerColorChanger"))
                        {
                            _towerColorChanger.ChangeColor();
                            break;
                        }
                        if (_resultList[i].gameObject.CompareTag("TowerSales"))
                        {
                            _towerSales.SalesTower();
                            break;
                        }
                        if (_resultList[i].gameObject.CompareTag("TowerDetailInfo"))
                        {
                            _towerDetailInfo.ShowTowerDetailInfo();
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