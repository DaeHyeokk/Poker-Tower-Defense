# 포타디 - 포커 타워 디펜스
![Poker Tower Defense Logo_small](https://user-images.githubusercontent.com/63538183/194304386-344da7a8-1c64-4851-806b-3ef5c7bdc07f.png)  
+ Google Play 출시를 목표로 시작한 모바일 타워 디펜스 프로젝트.
+ [Play 스토어 링크](https://play.google.com/store/apps/details?id=com.devdduck.pokertowerdefense)

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
### 1. 포커
   - 카드의 갯수가 총 **52장**이며 중복이 없다는 특성을 이용하여 **64비트 자료형**인 long 타입의 변수에 뽑은 카드 정보를 저장하는 **비트마스킹 기법**으로 구현하였다.  
   - **족보 판별**  
      - 리얼함을 위해 랜덤으로 뽑힌 카드의 순서를 저장하기 위한 별도의 카드 배열을 두는 것은 불가피 하기 때문에 카드 배열을 순회하며 족보를 판별하는 방식보다 **64비트만큼의 추가 메모리가 더 필요하다는 단점**이 있지만, 카드의 존재 유무를 확인하는 작업을 뽑은 카드 배열을 순회할 필요 없이 **비트 연산으로 O(1) 시간에 확인할 수 있어 매우 빠르게 족보를 판별할 수 있다는 장점**이 있다.  
      - https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/b66d7a91b37c93b3e7960b6c117fa64669fac160/Assets/Scripts/Stage%20Scripts/Card%20Scripts/CardDrawer.cs#L115-L236  
   - **카드 뽑기**
      - 랜덤으로 카드를 뽑은 다음 카드의 인덱스에 해당하는 비트를 켰을 때 키기 전 마스킹 변수와 값을 비교하여 값이 같을 경우 이미 뽑힌 카드를 뽑은 것이므로 다시 뽑는 방식으로 구현하였다.  
      - 
      - ㅇㅇㅇㅇ  
   - **카드 변경**
      - **카드 뽑기** 로직을 수행한 다음, 바꿀 카드의 인덱스에 해당하는 비트를 끄는 방식으로 카드 변경을 구현하였다.  
      바꿀 카드의 인덱스에 해당하는 비트를 마지막에 끄는 이유는 이미 뽑힌 카드가 중복으로 뽑히지 않도록 하기 위함이다.  
      - https://github.com/DaeHyeokk/Poker-Tower-Defense/blob/b66d7a91b37c93b3e7960b6c117fa64669fac160/Assets/Scripts/Stage%20Scripts/Card%20Scripts/CardDrawer.cs#L52-L66  
   - **조커 카드**
      - 
   
### 2. 카드
   - 룰
   - 설명
   - ㅎㅎㅎ
