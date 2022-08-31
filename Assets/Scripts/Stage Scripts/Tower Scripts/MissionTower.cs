using UnityEngine;
using UnityEngine.UI;

public class MissionTower : MonoBehaviour
{
    private enum SelectTowerType { 선택안함, 탑, 원페어, 투페어, 트리플, 스트레이트, 마운틴, 플러쉬, 풀하우스, 포카인드, 스트레이트플러쉬 }
    private enum SelectTowerColor { 선택안함, 빨강, 초록, 파랑 }

    [SerializeField]
    private HorizontalLayoutGroup _levelLayout;
    [SerializeField]
    private SelectTowerType _selectTowerType;
    [SerializeField]
    private SelectTowerColor _selectTowerColor;
    [SerializeField]
    private int _selectTowerLevel;

    private TowerBuilder _towerBuilder;
    private Image _towerImage;
    private Image[] _levelImages;

    private int _towerIndex;
    private int _level;
    private TowerColor.ColorType _colorType;

    
    private void Awake()
    {
        _towerBuilder = FindObjectOfType<TowerBuilder>();
        _towerImage = GetComponent<Image>();
        _levelImages = _levelLayout.GetComponentsInChildren<Image>(true);

        Setup();
    }
    

    // 처음 생성되면 타워의 종류, 레벨, 색깔타입을 랜덤으로 설정한다.
    public void Setup()
    {
        if (_selectTowerType == SelectTowerType.선택안함)
            _towerIndex = Random.Range(0, Tower.towerTypeNames.Length); // 0이상 타워타입의 개수 미만인 수 중에서 랜덤으로 추출.
        else
            _towerIndex = (int)_selectTowerType - 1;

        if (_selectTowerLevel == 0)
            _level = Random.Range(0, 4); // 0이상 4미만인 수 중에서 랜덤으로 추출.
        else
            _level = _selectTowerLevel - 1;

        if (_selectTowerColor == SelectTowerColor.선택안함)
            _colorType = (TowerColor.ColorType)Random.Range((int)TowerColor.ColorType.Red, (int)TowerColor.ColorType.Blue + 1); // Red, Green, Blue 중에서 랜덤으로 추출.
        else
            _colorType = (TowerColor.ColorType)_selectTowerColor; // Red, Green, Blue 중에서 랜덤으로 추출.

        SetTowerRenderer();
        SetTowerLevelImage();
    }
    private void SetTowerRenderer()
    {
        _towerImage.sprite = _towerBuilder.towerSprites[_towerIndex];

        switch (_colorType)
        {
            case TowerColor.ColorType.Red:
                _towerImage.color = Color.red;
                break;

            case TowerColor.ColorType.Green:
                _towerImage.color = Color.green;
                break;

            case TowerColor.ColorType.Blue:
                _towerImage.color = Color.blue;
                break;

            default:
                _towerImage.color = Color.white;
                break;
        }
    }

    private void SetTowerLevelImage()
    {
        for (int i = 0; i < _level; i++)
            _levelImages[i].gameObject.SetActive(true);
    }

    public bool IsCompareTower(Tower tower)
    {
        if (_towerIndex == tower.towerIndex && _level == tower.level && _colorType == tower.towerColor.colorType)
            return true;
        else
            return false;
    }

    public bool IsCompareTower(MissionTower missionTower)
    {
        if (_towerIndex == missionTower._towerIndex && _level == missionTower._level && _colorType == missionTower._colorType)
            return true;
        else
            return false;
    }
}
