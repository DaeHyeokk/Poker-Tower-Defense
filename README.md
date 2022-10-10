# 포타디 - 포커 타워 디펜스
![Poker Tower Defense Logo_small](https://user-images.githubusercontent.com/63538183/194304386-344da7a8-1c64-4851-806b-3ef5c7bdc07f.png)  

## 프로젝트 개요  
+ [Play 스토어 링크](https://play.google.com/store/apps/details?id=com.devdduck.pokertowerdefense)
+ 포트폴리오와 Google Play 출시를 목표로 시작한 모바일 타워 디펜스 프로젝트.
+ 제작 기간: 2022/04/20 ~ 2022/09/03 (계속 업데이트 중)
+ 제작 인원: 1명 (개인 프로젝트)
+ 사용한 프로그램: Unity, Visual Studio 2022

## 프로젝트 목표
+ C# Naming Rule에 맞춰 스크립트를 작성하여 누구나 읽기 쉬운 코드를 작성할 것.
참고 링크: [C# 코딩 규칙](https://docs.microsoft.com/ko-kr/dotnet/csharp/fundamentals/coding-style/coding-conventions)
+ 클래스를 꼼꼼히 설계하여 코드의 중복을 최소화 할 것.
+ 더 효율적인 방식으로 코딩하여 성능을 최적화 할 것.
+ Play 스토어에 내놓아도 부끄럽지 않은 게임다운 게임을 만들 것.

## 게임 특징
+ 포커의 특성을 이용하여 운의 영향을 받도록 설계한 타워 디펜스 게임이다.
+ 모바일 게임 특성 상 조작법이 단순해야 좋기 때문에 타워 배치, 타워 합치기, 색 변경, 판매 등 타워의 여러 기능들을 드래그 앤 드롭으로 간편하게 조작할 수 있도록 하였다.
+ 게임 중 랜덤으로 등장하는 조커 카드와 보스 처치, 미션 클리어를 통해 획득하는 카드 교환권을 활용하여 플레이어가 원하는 등급의 족보를 맞출 수 있다.
+ 단일 데미지가 높아 보스 몬스터에게 효율이 높지만 다수의 적에게는 효율이 낮은 타워와, 단일 데미지는 낮지만 범위 공격을 통해 다수의 적에게 높은 효율을 발휘하는 타워를 나눔으로써 플레이어가 타워를 적절히 조합해야 게임을 클리어 할 수 있도록 설계하였다.
+ 플레이어가 게임 플레이를 통해 타워의 킬수를 획득하고 타워가 일정 킬수에 도달하면 레벨업 하는 타워 성장 시스템을 구현함으로써 게임을 많이 플레이 할수록 게임 클리어가 쉬워지고, 높은 기록을 달성하는데 유리하도록 설계하였다.
+ 마지막 보스를 처치하는데 걸린 시간을 기록하고 최고 기록을 달성하면 이를 리더보드에 등록함으로써 다른 플레이어들과 경쟁할 수 있도록 하였다.

## 주요 기능
### 1. 포커 관련 로직
   - 카드의 갯수가 총 **52장**이며 중복이 없다는 특성을 이용하여 **64비트 자료형**인 long 타입의 변수에 뽑은 카드 정보를 저장하는 **비트마스킹 기법**으로 구현하였다.
   - 리얼함을 위해 랜덤으로 뽑힌 카드의 순서를 저장하기 위한 별도의 카드 배열을 두는 것은 불가피 하기 때문에 카드 배열을 순회하며 족보를 판별하는 방식보다 **64비트만큼의 추가 메모리가 더 필요하다는 단점**이 있지만, 카드의 존재 유무를 확인하는 작업을 뽑은 카드 배열을 순회할 필요 없이 **비트 연산으로 O(1) 시간에 확인할 수 있어 매우 빠르게 족보를 판별할 수 있다는 장점**이 있다.
   - **족보 판별**  
      - 먼저 각 숫자마다 몇 개씩 존재하는지 검사하여 원페어, 투페어, 트리플, 풀하우스, 포카인드 조건을 검사하고, 연속되는 숫자를 카운팅하여 스트레이트, 마운틴 조건을 검사한다.  
      그 다음 각 무늬마다 몇 개씩 존재하는지 검사하여 플러쉬 조건을 검사하고, 연속되는 숫자를 카운팅하여 스트레이트 플러쉬 조건을 검사하는 방식으로 구현하였다.
      - <details>
        <summary>코드 보기/숨기기</summary>
   
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/11ecd87d756b2c837c9664faea20d6a0e1572099/Assets/Scripts/Stage%20Scripts/Card%20Scripts/CardDrawer.cs#L115-L238  
        </details>
      
   - **카드 뽑기**
      - 랜덤으로 카드를 뽑은 다음 카드의 인덱스에 해당하는 비트를 켰을 때 키기 전 마스킹 변수와 값을 비교하여 값이 같을 경우 이미 뽑힌 카드를 뽑은 것이므로 다시 뽑는 방식으로 카드를 중복해서 뽑지 않도록 구현하였다.
      - bool 타입의 매개변수인 isFirst를 통해 이미 뽑힌 카드를 뽑아서 다시 시도하는 경우가 아닌 처음으로 시도하는 경우에만 일정 확률로 조커 카드가 뽑히도록 구현하였다.
      - <details>
        <summary>코드 보기/숨기기</summary>
   
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/11ecd87d756b2c837c9664faea20d6a0e1572099/Assets/Scripts/Stage%20Scripts/Card%20Scripts/CardDrawer.cs#L80-L113  
        </details>
   
   - **카드 랜덤 변경**
      - **카드 뽑기** 로직을 수행한 다음, 바꿀 카드의 인덱스에 해당하는 비트를 끄는 방식으로 구현하였다.  
      바꿀 카드의 인덱스에 해당하는 비트를 마지막에 끄는 이유는 이미 뽑았던 카드가 중복으로 뽑히지 않도록 하기 위함이다.
      - <details>
        <summary>코드 보기/숨기기</summary>
   
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/11ecd87d756b2c837c9664faea20d6a0e1572099/Assets/Scripts/Stage%20Scripts/Card%20Scripts/CardDrawer.cs#L52-L66  
        </details>
   
   - **카드 선택 변경**
      - 플레이어가 [Card Selector](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Card%20Scripts/CardSelector.cs)를 통해 선택한 카드의 인덱스에 해당하는 비트를 켜는 방식으로 구현하였다.
      - <details>
        <summary>코드 보기/숨기기</summary>
   
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/11ecd87d756b2c837c9664faea20d6a0e1572099/Assets/Scripts/Stage%20Scripts/Card%20Scripts/CardDrawer.cs#L68-L78  
        </details>
      
### 2. 타워 관련 로직
   - [Tower 클래스 다이어그램](https://user-images.githubusercontent.com/63538183/194644398-d17f904d-1d06-4251-bca5-3b1fc86e439e.png)
   - 추상 클래스 [Tower](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Tower.cs)를 정의하고 Tower를 상속받는 여러 종류의 타워 클래스를 정의 하였다.
   - 모든 타워가 공통으로 가지는 변수, 메소드를 Tower 클래스에 정의하고, 공통으로 가지고 있지만 다르게 동작하는 프로퍼티나 메소드를 abstract 또는 virtual로 선언함으로써 코드의 중복을 최소화 하고 관리 및 유지보수가 용이하도록 구현하였다.
   - 타워의 기능 중 사거리 내의 적을 찾는 기능은 [Target Detector](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/TargetDetector.cs), 타워 색상과 관련된 기능은 [Tower Color](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/TowerColor.cs), 타워의 레벨과 관련된 기능은 [Tower Level](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/TowerLevel.cs) 클래스로 세분화 하였다.
   - **타워 생성**
      - 타워는 [Tower Builder](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/TowerBuilder.cs) 오브젝트에서 생성 된다.
      - 타워 합치기, 타워 판매 기능으로 인해 자주 생성되고 파괴될 것으로 예상되는 오브젝트임으로 [Object Pool](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Common%20Scripts/ObjectPool.cs)을 통해 활성화 및 비활성화 되도록 구현하여 효율성을 높였다.
      - 타워가 생성되면 Tower Builder의 멤버 변수인 Tower List에 담기게 된다.  
        Tower List는 여러가지 타워 수집 미션의 조건을 만족하는지 확인하기 위해 생성된 타워의 목록을 탐색하는 용도로 사용되는데,  
        List의 맨 앞에서부터 뒤로 탐색하는 로직만 수행하기 때문에 **List의 원소에 인덱스로 직접 접근할 일이 없고** 타워 합치기, 타워 판매 기능으로 인해 **List의 중간 원소를 삭제할 일이 많기 때문에** List가 아닌 **LinkedList**에 Tower를 담도록 구현하여 효율성을 높였다.
      - 타워가 생성되는 Spawn Point 좌표가 동일한 경우 여러개의 타워가 겹쳐서 생성 되었을 때 나중에 생성된 타워가 가장 위쪽에 배치되지 않는 경우가 종종 발생하였다.  
        이를 해결하기 위해 타워를 생성할 때마다 Spawn Point의 Z값에 0.00001f 만큼 작은 값을 빼줌으로써 나중에 생성한 타워일수록 가장 위쪽에 배치되도록 구현하였다.
      - <details>
        <summary>코드 보기/숨기기</summary>
   
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/467e3225f95ec4320cf6fe0b2760f75b7d9b0ce8/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/TowerBuilder.cs#L45-L71  
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/32fb7abb2728e3909b6eb8ec99ef3c0dce747680/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Tower.cs#L232-L257  
        </details>  
      
   - **타워의 공격**  
      - 타워의 공격은 사거리 내 적 탐색 -> 발사체 생성 -> 발사체 충돌 -> 충돌한 대상 또는 대상 주변에 피해를 입힘 순으로 이루어진다.
      - **사거리 내 적 탐색**
        - 타워가 타일에 배치 되면 Update() 함수를 통해 매 프레임마다 Target Detector의 SearchTarget() 함수를 호출하여 사거리 내의 적을 탐색하게 된다.
        - Vector2.Distance() 함수를 통해 타워와 적의 거리를 계산하고 사거리보다 가까울 경우 적을 Target List에 추가한다.  
          Target List에 추가된 적의 수가 타워의 Max Target Count와 같아지면 탐색을 종료한다.
        - 이전 탐색에서 Target List에 추가된 적이 있을 경우 해당 적이 아직 사거리 내에 있는지 확인하여 있으면 유지하고 없으면 리스트에서 꺼냄으로써 한번 타겟으로 정한 적이 사거리를 벗어나기 전까지 계속해서 공격하도록 구현하였다.
        - 가장 먼저 사거리 내 활성화 된 보스몬스터가 있는지 체크함으로써 보스를 우선 타격하도록 구현하였으며, 보스 중에서도 Special Boss(행성 보스)를 가장 먼저 체크해서 최우선으로 타격하도록 구현하였다.
        - <details>
          <summary>코드 보기/숨기기</summary>
   
          https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/9b94d91eed95fc8a78f671560cdb89df383e96c3/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/TargetDetector.cs#L30-L157
          </details>
        
      - **발사체 생성**
        - 타워는 Target List에 적이 존재할 때 Attack Delay가 0이 되면 ShotProjectile() 함수를 호출하여 적을 추격하는 발사체를 생성하고, [Projectile](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Projectile%20Scripts/Projectile.cs)의 actionOnCollision 대리자에 이벤트 발생 시 수행할 작업을 추가한다.
        - 자주 생성되고 파괴되는 오브젝트임으로 [Object Pool](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Common%20Scripts/ObjectPool.cs)을 통해 활성화 및 비활성화 되도록 구현하여 효율성을 높였다.
        - 발사체를 생성 할 때마다 타워의 AttackCount를 1씩 증가시키고 AttackCount가 10이 되면 더 강한 효과를 가진 발사체를 생성하도록 함으로써 타워의 특수 공격 기능을 구현하였다.
        - - 발사체를 생성하는 ShotProjectile() 함수를 가상함수로 선언함으로써 다른 특성의 발사체를 생성하는 타워들도 함수 오버라이딩을 통해 동일한 함수명으로 호출할 수 있도록 구현하였다.
        - <details>
          <summary>코드 보기/숨기기</summary>
   
          https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/612d96a888002a10f0fca286f2d94d8b4da738aa/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Tower.cs#L201-L230  
          https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/899f6010601e98a6fda6ed3b721d635c4b93f171/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Tower.cs#L259-L310  
          </details>  
   
      - **발사체 충돌 & 적에게 피해를 입힘**
        - 발사체는 Update() 함수를 통해 매 프레임마다 추격하는 적을 향해 이동하며, 일정 거리 이하로 가까워지면 충돌한다.
        - 충돌 시 actionOnCollision 대리자를 호출하여 비동기적으로 적에게 피해를 입히도록 구현하였다.
        - <details>
          <summary>코드 보기/숨기기</summary>
   
          https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/3f4f46c29d7543a7178b0b9316b9cb465d5f14b0/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Projectile.cs#L33-L65  
          </details>
        
   - **Enemy, Tower와의 상호작용**
      - [IInflictable 클래스 다이어그램](https://user-images.githubusercontent.com/63538183/194796793-b9b61b9f-ff01-4a5d-a54a-c3b2dea046a0.png)
      - 타워는 [IInflictable](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Inflictors/IInflictable.cs) 인터페이스를 상속받는 여러가지 객체를 사용하여 적에게 피해를 입히거나 상태이상 디버프를 걸고, 타워의 능력치를 상승 시키는 등의 기능을 수행한다.
      - IInflictable의 UpdateInflictorInfo() 함수를 통해 Attributes에 할당된 값에 따라 동적으로 Inflictable의 inflictorInfo 문자열이 갱신되도록 하여 추후 값 변경에 용이하도록 구현하였다.
      - inflictorInfo 문자열은 길이가 길고 여러 문자열이 결합된 형태로 이루어져 있기 때문에 Garbage 생성을 최소화 하기 위해 StringBuilder를 사용하여 구현하였다.
      - <details>
        <summary>코드 보기/숨기기</summary>
   
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/54e72f783991b9373f7816fe910ef2a6259eb657/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Tower.cs#L312-L391  
        </details>
      
   - **타워 드래그 앤 드롭 기능**
      - [Object Detector](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Tower%20Function%20Scripts/ObjectDetector.cs)에서 플레이어의 타워 터치 입력을 감지하여 타워의 이동, 합치기, 색 변경, 판매, 상세 정보 보기 기능을 수행한다.
      - Scene에서 [PopupUI](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/UI%20Scripts/PopupUI.cs) 컴포넌트를 가진 오브젝트가 활성화 되면 Object Detector의 popupUICount 변수가 1 증가하고, 비활성화 되면 다시 1 감소 시키는 방식으로 화면에 Popup UI가 활성화 되어 있는 경우(popupUiCount가 1 이상일 경우) 플레이어의 터치 입력을 받지 않도록 구현하였다.
      - 플레이어가 타워를 터치하면 마우스 포인터를 따라다니는 FollowTower 오브젝트를 활성화 시켜 타워 드래그 기능을 구현하였다.
      - 플레이어가 손을 떼면 FollowTower 오브젝트를 비활성화 하고, 플레이어가 손을 뗀 좌표에서 Ray를 생성하여 타워 드롭 기능을 구현하였다.
      - **타워의 이동**
         - Ray가 Tile 오브젝트와 충돌했을 때 해당 Tile에 배치된 타워가 없다면 Tile의 좌표로 타워를 이동시킨다.
      - **타워 합치기**
         - Ray가 Tile 오브젝트와 충돌했을 때 해당 Tile에 배치된 타워가 있다면 배치된 타워와 합치기를 시도한다.
      - **색 변환, 판매, 상세 정보 보기**
         - Ray가 Tile 오브젝트와 충돌하지 않았을 경우 GraphicRaycaster.Raycast() 함수를 호출하여 캔버스 영역에 존재하는 UI 오브젝트와 충돌하는 Ray를 생성하고, Ray가 충돌한 UI 오브젝트의 Tag를 검사하여 각각의 기능을 수행하게 된다.
         - 상세 정보 문자열은 길이가 길고 여러 문자열이 결합된 형태로 이루어져 있기 때문에 Garbage 생성을 최소화 하기 위해 StringBuilder를 사용하여 구현하였다.
         - [Tower Color Changer](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Tower%20Function%20Scripts/TowerColorChanger.cs)
         - [Tower Sales](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Tower%20Function%20Scripts/TowerSales.cs)
         - [Tower Detail Info](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Tower%20Function%20Scripts/TowerDetailInfo.cs)
      
      - <details>
        <summary>코드 보기/숨기기</summary>
   
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/472502f47071761127922feffc28a640af6e1342/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Tower%20Function%20Scripts/ObjectDetector.cs#L37-L157  
        </details>

   - **타워 성장 기능**
      - 타워는 멤버 함수 AccumulateKillCount()를 통해 몬스터를 처치할 때마다 킬 카운트를 획득한다.
      - 킬 카운트는 모든 타워가 공유하는 데이터이므로 전역 변수로 선언하였고, 게임을 패배하거나 클리어할 경우 타워가 기록한 킬 카운트를 [Game Manager](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/a2d22a6b713ac10c1a7ee226d654f2d42d5bfd26/Assets/Scripts/Common%20Scripts/DontDestroyObjects/GameManager.cs)의 Player Tower Data에 누적시키는 방식으로 구현하였다.  
        전역 변수이므로 Scene을 새로 로드해도 데이터가 유지되기 때문에 Scene을 로드할 때마다 킬 카운트를 초기화 하도록 구현하였다.
      - 플레이어의 데이터는 GPGS에서 제공하는 데이터 Save, Load 기능을 통해 구글 클라우드에서 안전하게 관리되도록 구현하였으며, Player Game Data클래스를 JsonUtility를 사용하여 Json 문자열로 변환하고 이를 byte 배열로 인코딩하여 변조하기 어려운 암호화된 데이터로 저장 및 로드한다.
      - <details>
        <summary>코드 보기/숨기기</summary>
   
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/a2d22a6b713ac10c1a7ee226d654f2d42d5bfd26/Assets/Scripts/Stage%20Scripts/Tower%20Scripts/Tower.cs#L12-L45  
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/a2d22a6b713ac10c1a7ee226d654f2d42d5bfd26/Assets/Scripts/Stage%20Scripts/Manager%20Scripts/StageManager.cs#L298-L403  
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/a2d22a6b713ac10c1a7ee226d654f2d42d5bfd26/Assets/Scripts/Common%20Scripts/DontDestroyObjects/GameManager.cs#L217-L335  
        </details>
        
### 2. 몬스터 관련 로직
   - [Enemy 클래스 다이어그램](https://user-images.githubusercontent.com/63538183/194795302-425230a6-1722-4a45-abbc-c847af091bf5.png)
   - Enemy 클래스를 몬스터 각각의 특징에 따라 하위 클래스로 세분화 함으로써 코드의 중복을 최소화 하고 관리 및 유지보수가 용이하도록 구현하였다.
      - [Enemy](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/Enemy.cs): 최상위 추상 클래스
      - [Field Enemy](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/FieldEnemy.cs), [Special Boss Enemy](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/SpecialBossEnemy.cs): 움직이는지 여부에 따라 Enemy를 상속 받는 추상 클래스와 기본 클래스
      - [Field Boss Enemy](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/FieldBossEnemy.cs), [Round Enemy](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/RoundEnemy.cs): 보스 몬스터인지 여부에 따라 Field Enemy를 상속 받는 추상 클래스와 기본 클래스
      - [Round Boss Enemy](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/RoundBossEnemy.cs), [Mission Boss Enemy](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/MissionBossEnemy.cs): 보스 종류에 따라 Field Boss Enemy를 상속 받는 기본 클래스
   - Draw Call 최적화를 위해 몬스터의 체력바가 동시에 그려지도록 Slider 컴포넌트가 아닌 Square 오브젝트 두 개를 이용하여 구현하였다.
   - **몬스터 생성**
      - 몬스터는 [Enemy Spawner](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/EnemySpawner.cs) 오브젝트에서 생성 된다.
      - 몬스터 중에서 Round Enemy는 자주 생성되고 파괴되는 오브젝트이므로 [Object Pool](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Common%20Scripts/ObjectPool.cs)을 통해 활성화 및 비활성화 되도록 구현하여 효율성을 높였다.
      - Round Enemy가 생성되면 Enemy Spawner의 멤버 변수인 Round Enemy List에 담기게 된다.  
      Round Enemy List는 필드 위에 활성화 된 Round Enemy를 참조하거나 활성화 된 Round Enemy가 총 몇 마리인지 확인하기 위한 용도로 사용되는데,  
      List의 맨 앞에서부터 뒤로 탐색하는 로직만 수행하기 때문에 **List의 원소에 인덱스로 직접 접근할 일이 없고** Enemy의 Die() 함수로 인해 **List의 중간 원소를 삭제할 일이 많기 때문에** List가 아닌 **LinkedList**에 Round Enemy를 담도록 구현하여 효율성을 높였다.
   - **몬스터 이동**
      - 몬스터 중에서 Field Enemy는 [Enemy Movement](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/EnemyMovement.cs) 객체를 통해 4개의 Way Point를 순환한다.
      - 몬스터는 매 프레임마다 Way Point를 향해 이동하는데, 만약 이동할 거리가 Way Point와의 거리보다 클 경우 초과한 거리만큼 다음 Way Point가 위치한 방향으로 이동 시키는 방식으로 몬스터가 Way Point 경로를 이탈하지 않도록 구현하였다.
      - 구현 참고: [고박사의 유니티 노트 - [Unity 2D Game] Tower Defense #01 - 맵 배치, 적 생성 및 이동](https://www.youtube.com/watch?v=Qu_JVnwWn7w&list=PLC2Tit6NyVicvqMTDJl8e-2IB4v_I7ddd&index=9)
      - <details>
        <summary>코드 보기/숨기기</summary>
   
        https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/d66a4e9eca3197299f5bf40d522c3500a18414f6/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/EnemyMovement.cs#L29-L74  
        </details>
        
   - **몬스터 피격 및 디버프**
      - 몬스터는 타워로부터 데미지를 받거나 스턴, 슬로우, 방어력 감소 디버프를 받는다.
      - **몬스터 피격**
         - 몬스터가 데미지를 받게 되면 체력이 감소하고 [Stage UI Manager](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/a2d22a6b713ac10c1a7ee226d654f2d42d5bfd26/Assets/Scripts/Stage%20Scripts/Manager%20Scripts/StageUIManager.cs)의 ShowDamageTakenText() 함수를 호출하여 받은 데미지를 나타내는 [Damage Taken Text](https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/main/Assets/Scripts/Stage%20Scripts/UI%20Scripts/Dynamic%20UI%20Scripts/DamageTakenText.cs)를 생성 한다.
         - Damage Taken Text는 매우 자주 생성되고 파괴되는 오브젝트이므로 Object Pool을 통해 활성화 및 비활성화 되도록 구현하여 효율성을 높였고, 커졌다 작아진 다음 빠르게 올라가며 사라지는 애니메이션을 추가하여 타격감과 생동감을 느낄 수 있도록 구현하였다.
         - <details>
           <summary>코드 보기/숨기기</summary>
   
           https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/a2d22a6b713ac10c1a7ee226d654f2d42d5bfd26/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/Enemy.cs#L96-L115  
           https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/816bb38391d16d0ecb006ece6895e63f94026efb/Assets/Scripts/Stage%20Scripts/Manager%20Scripts/StageUIManager.cs#L116-L125  
           https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/816bb38391d16d0ecb006ece6895e63f94026efb/Assets/Scripts/Stage%20Scripts/UI%20Scripts/Dynamic%20UI%20Scripts/DamageTakenText.cs#L50-L85  
           </details>

      - **몬스터 스턴**
         - Field Enemy가 스턴 공격을 받게 되면 공격 받은 스턴의 지속 시간동안 이동을 멈추고 Stun 파티클을 활성화 한다.
         - 지속 시간이 감소하는 로직은 코루틴을 이용하였으며, 현재 남아있는 지속 시간보다 더 짧은 지속 시간의 스턴 공격을 받게 되는 상황과 같이 스턴 공격을 중첩해서 받는 경우에 대한 예외 처리 로직을 간단하게 구현하기 위해 Reference Counting 기법을 참고하여 스턴 공격을 받게 되면 stunCount를 증가 시키고, 지속 시간이 종료되면 stunCount를 다시 감소시키는 방식으로 구현하였다.
         - stunCount 값이 1 이상이 되면 이동을 멈추고 stun 파티클을 활성화 하며, 0이 되면 이동을 재게하고 stun 파티클을 비활성화 하는 로직을 프로퍼티로 구현하였다.
         - <details>
           <summary>코드 보기/숨기기</summary>
   
           https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/a2d22a6b713ac10c1a7ee226d654f2d42d5bfd26/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/FieldEnemy.cs#L16-L39  
           https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/a2d22a6b713ac10c1a7ee226d654f2d42d5bfd26/Assets/Scripts/Stage%20Scripts/Enemy%20Scripts/FieldEnemy.cs#L76-L94  
           </details>
