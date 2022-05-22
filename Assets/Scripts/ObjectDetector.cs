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

    void Awake()
    {
        // 'MainCamera' 태그를 가지고 있는 오브젝트를 탐색 후 Camera 컴포넌트 정보 전달
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 와 동일
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 카메라 위치에서 화면의 마우스 커서를 관통하는 광선(ray) 생성
            // ray.origin : 광선의 시작 위치 (= 카메라 위치)
            // ray.direction : 광선의 진행 방향
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // 2D 모니터를 통해 3D 월드의 오브젝트를 마우스로 선택하는 방법
            // 광선에 부딪히는 오브젝트를 검출해서 hit에 저장
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // 광선에 부딪힌 타겟이 타워라면 마우스를 떼기 전까지 계속해서 마우스포인터를 따라감
                if (hit.transform.CompareTag("Tower"))
                {
                    _clickTower = hit.transform.GetComponent<Tower>();
                    _tempPosition = _clickTower.transform.position;
                    _clickTower.isMove = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            // 마우스를 뗐을 때 타워를 움직이는 중이었다면 중단한다.
            if(_clickTower != null && _clickTower.isMove)
            {
                _clickTower.isMove = false;

                // 카메라 위치에서 화면의 마우스 커서를 관통하는 광선(ray) 생성
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                hits = Physics.RaycastAll(ray, Mathf.Infinity);

                for (int i = 0; i < hits.Length; i++)
                {
                    // 광선에 부딪힌 타겟이 Tile일 때 수행.
                    if (hits[i].transform.CompareTag("Tile"))
                    {
                        Tile tile = hits[i].transform.GetComponent<Tile>();

                        // 마우스를 뗀 좌표에 위치한 타일이 클릭중인 타워가 되돌아갈 타일이라면
                        // 타워를 타일 위에 배치하거나 타워 합치기 작업을 수행할 필요가 없다.
                        if (tile.transform.position == _tempPosition)
                            break;

                        // 현재 비어있는 타일이라면 해당 타일 위로 타워를 배치.
                        if (tile.collocationTower == null)
                        {
                            _clickTower.onTile = tile;
                            return;
                        }
                        // 다른 타워가 이미 배치된 타일이라면 타워 합치기 시도.
                        else
                        {
                            // 타워 합치기에 성공했다면 함수를 종료한다.
                            if (tile.collocationTower.MergeTower(_clickTower))
                                return;
                        }
                    }
                }

                // 타워 합치기에 성공하거나 빈 타일위로 이동시키는 경우가 아니라면 타워를 원래 위치로 되돌린다.
                _clickTower.transform.position = _tempPosition;
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