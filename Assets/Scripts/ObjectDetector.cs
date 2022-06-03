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
        // 'MainCamera' 태그를 가지고 있는 오브젝트를 탐색 후 Camera 컴포넌트 정보 전달
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 와 동일
        _mainCamera = Camera.main;
        _graphicRay = _towerInfoCanvas.GetComponent<GraphicRaycaster>();
        _pointerEventData = new(null);
        //_resultList = new List<RaycastResult>();
        _clickTower = null;
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 이미 타워를 움직이고 있는 상태라면 건너뛴다.
            if (_clickTower != null) return;

            // 카메라 위치에서 화면의 마우스 커서를 관통하는 광선(ray) 생성
            // ray.origin : 광선의 시작 위치 (= 카메라 위치)
            // ray.direction : 광선의 진행 방향
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            // 광선에 부딪히는 오브젝트를 검출해서 hit2D에 저장
            if(Physics.Raycast(_ray, out _hit, Mathf.Infinity))
            {
                // 광선에 부딪힌 타겟이 타워라면 마우스를 떼기 전까지 계속해서 마우스포인터를 따라감
                if (_hit.transform.CompareTag("Tower"))
                {
                    _clickTower = _hit.transform.GetComponent<Tower>();
                    _clickTower.MoveTower();
                }
            }
        }
        // 마우스 왼쪽 버튼을 뗐을 때
        if (Input.GetMouseButtonUp(0))
        {
            // 타워를 움직이는 중이었다면 중단한다.
            if (_clickTower != null)
            {
                // 카메라 위치에서 화면의 마우스 커서를 관통하는 광선(ray) 생성.
                _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                _hits = Physics.RaycastAll(_ray, Mathf.Infinity);

                bool isHitTile = false;

                for (int i = 0; i < _hits.Length; i++)
                {
                    // 광선에 부딪힌 타겟이 Tile일 때 수행.
                    if (_hits[i].transform.CompareTag("Tile"))
                    {
                        isHitTile = true;
                        Tile tile = _hits[i].transform.GetComponent<Tile>();

                        // 마우스를 뗀 좌표에 위치한 타일이 클릭중인 타워가 원래 배치 돼있던 타일이라면
                        // 타워를 타일 위에 배치하거나 타워 합치기 작업을 수행할 필요가 없다.
                        if (_clickTower.onTile != null && tile.transform.position == _clickTower.onTile.transform.position)
                            break;

                        // 현재 비어있는 타일이라면 해당 타일 위로 타워를 배치.
                        if (tile.collocationTower == null)
                        {
                            _clickTower.onTile = tile;
                            break;
                        }
                        // 다른 타워가 이미 배치된 타일이라면 타워 합치기 시도.
                        else
                        {
                            // 타워 합치기에 성공했다면 for문을 빠져나온다.
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
 * 플레이어의 마우스 클릭(모바일에서는 화면 터치)를 인식하고 그에 대한 작업을 담당하는 스크립트
 * Camera 컴포넌트의 ScreenPointToRay() 메서드를 통해 플레이어가 클릭한 좌표를 향해 광선을 발사하여
 * 광선과 충돌하는 오브젝트의 정보를 hit에 담아 오브젝트와의 상호작용을 수행한다.
 * 
 * Update : 2022/05/02 MON 02:12
 * 필드 디자인을 바꾸면서 타일 위에 짓는 방식이 아닌 맵의 중앙에서 생성되는 방식으로 바뀌었으므로, 
 * 플레이어가 타일을 터치했는지 확인하는 로직 삭제.
 * 
 * Update : 2022/05/02 MON 19:38
 * 마우스로 타워를 클릭했을 때 타워가 마우스를 따라다니도록 하고, 
 * 마우스를 떼면 타워가 마우스를 따라다니던 것을 멈추도록 하여 타워의 이동 구현.
 * 
 * Update : 2022/05/18 WED
 * 타워를 움직여서 타일 위에 배치하는 로직 구현.
 * 타일이 아닌 포지션으로 이동시키거나, 이미 타워가 배치 되어 있는 타일 위로 이동시키는 경우 원래 위치로 되돌아 가도록 구현하였음.
 * 
 * Update : 2022/05/21 SAT
 * 타워를 움직여서 타워 위에 놓을 경우 타워 합치기를 시도하는 로직 구현.
 */