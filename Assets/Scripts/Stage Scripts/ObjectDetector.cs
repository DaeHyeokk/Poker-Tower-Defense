using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private CardSelector _cardSelector;
    [SerializeField]
    private TowerColorChanger _towerColorChanger;
    [SerializeField]
    private TowerSales _towerSales;
    [SerializeField]
    private TowerDetailInfo _towerDetailInfo;
    [SerializeField]
    private TowerDetailInfoUIController _towerDetailInfoUIController;
    [SerializeField]
    private GraphicRaycaster _towerInfoGraphicRay;
    [SerializeField]
    private GraphicRaycaster _towerDetailInfoCanvasGraphicRay;

    private Tower _clickTower;
    private Camera _mainCamera;
    private PointerEventData _pointerEventData;
    private Ray _ray;
    private RaycastHit _hit;
    private RaycastHit[] _hits;
    private List<RaycastResult> _resultList;

    private int _popupUICount;

    private void Awake()
    {
        // 'MainCamera' 태그를 가지고 있는 오브젝트를 탐색 후 Camera 컴포넌트 정보 전달
        _mainCamera = Camera.main;
        _pointerEventData = new(null);
        _resultList = new List<RaycastResult>();
        _clickTower = null;

        // 게임이 끝나거나 일시정지 되면 플레이어의 오브젝트 터치 입력을 받지 않도록 비활성화하고,
        // 게임이 재게되면 오브젝트 터치 입력을 다시 받도록 활성화 한다.
        StageManager.instance.onStageEnd += () => _popupUICount++;
        StageManager.instance.onStagePaused += () => _popupUICount++;
        StageManager.instance.onStageResumed += () => _popupUICount--;

        StageUIManager.instance.missionUIController.onShowMissionDetailUI += () => _popupUICount++;
        StageUIManager.instance.missionUIController.onHideMissionDetailUI += () => _popupUICount--;
    }

    private void Update()
    {
        // 화면상에 팝업 UI가 한개 이상 활성화된 상태라면 플레이어의 타워 터치입력을 수행하지 않는다.
        if(_popupUICount > 0)
        {
            if (_clickTower != null)
            {
                _clickTower.StopTower();
                _clickTower = null;
            }

            return;
        }

        // 마우스 왼쪽 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 이미 타워를 움직이고 있는 상태라면 건너뛴다.
            if (_clickTower != null)
            {
                // return;
                _clickTower.StopTower();
                _clickTower = null;
            }

            // Tower Detail Info UI 또는 Card Selector UI가 화면에 활성화 되어 있는 상태라면 오브젝트 클릭을 입력받지 않는다.
            if (_towerDetailInfoUIController.gameObject.activeSelf 
                || _cardSelector.gameObject.activeSelf)
                return;

            // 카메라 위치에서 화면의 마우스 커서를 관통하는 광선(ray) 생성
            // ray.origin : 광선의 시작 위치 (= 카메라 위치)
            // ray.direction : 광선의 진행 방향
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            // 광선에 부딪히는 오브젝트를 검출해서 hit에 저장
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
            {
                // 광선에 부딪힌 타겟이 타워라면 타워가 마우스 포인터를 따라다니는 MoveTower() 메소드를 호출한다.
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

                // 광선이 Tile 에 부딪히지 않았을 때 실행
                if (!isHitTile)
                {
                    _pointerEventData.position = Input.mousePosition;

                    if(_resultList.Count != 0) _resultList.Clear();
                    // Tower Info Canvas 안에 배치된 UI와 충돌하는 광선을 발사한다.
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
                        if(_resultList[i].gameObject.CompareTag("TowerDetailInfo"))
                        {
                            _towerDetailInfo.ShowTowerDetailInfo();
                            break;
                        }
                    }
                }

                _clickTower.StopTower();
                _clickTower = null;
            }
            // 타워 상세정보 UI가 화면에 활성화 되어 있는 상태일 때 실행
            else if (_towerDetailInfoUIController.gameObject.activeSelf)
            {
                _pointerEventData.position = Input.mousePosition;

                if (_resultList.Count != 0) _resultList.Clear();

                // Tower Detail Info Canvas 안에 배치된 UI와 충돌하는 광선을 발사한다.
                // GraphicRay 범위: 상단의 Wave UI 영역과 하단의 Card Gamble UI 아래 영역을 제외한 나머지 영역
                _towerDetailInfoCanvasGraphicRay.Raycast(_pointerEventData, _resultList);

                if (_resultList.Count != 0)
                {
                    for (int i = 0; i < _resultList.Count; i++)
                    {
                        // 타워 상세정보 UI를 터치하는 경우 자동으로 비활성화 되는 타이머를 초기화 시킨다.
                        if (_resultList[i].gameObject.CompareTag("TowerDetailInfoUI"))
                        {
                            _towerDetailInfoUIController.ResetHideDelay();
                            break;
                        }
                    }
                }
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