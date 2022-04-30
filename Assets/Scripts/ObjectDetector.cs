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
                // 광선에 부딪힌 오브젝트의 태그가 "Tile"이면
                if (hit.transform.CompareTag("Tile"))
                {
                    // towerBuilder의 BuildTower(Transform) 메서드 호출
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
 * 플레이어의 마우스 클릭(모바일에서는 화면 터치)를 인식하고 그에 대한 작업을 담당하는 스크립트
 * Camera 컴포넌트의 ScreenPointToRay() 메서드를 통해 플레이어가 클릭한 좌표를 향해 광선을 발사하여
 * 광선과 충돌하는 오브젝트의 정보를 hit에 담아 오브젝트와의 상호작용을 수행한다.
 */