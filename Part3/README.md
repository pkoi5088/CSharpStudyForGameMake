# 유니티 엔진
클라이언트 파트를 다룰 유니티 엔진에 대해서 알아보고 간단한 예제들을 통해 기능들을 학습하는 파트이다.
## 내용
* [기초 디자인 패턴](#기초-디자인-패턴)
    + [컴포넌트 패턴](#컴포넌트-패턴)
    + [싱글톤 패턴](#싱글톤-패턴)
* [트랜스폼](#트랜스폼)
    + [플레이어 설정](#플레이어-설정)
    + [Position](#position)
    + [Input Manager](#input-manager)
* [Prefab](#prefab)
    + [Resource Manager](#resource-manager)
* [Collision](#collision)
    + [RigidBody Collider](#rigidbody-collider)
    + [Wall Collision](#wall-collision)
    + [Raycasting](#raycasting)
* Camera
* Animation
* UI
* Scene
* Sound
* Object Pooling
* Coroutine
* Data
* Mini RPG
## 기초 디자인 패턴
>디자인 패턴이란?
간단히 말하면 코드를 작성하는 방법론을 의미하며 프로그램을 개발하는 과정에서 공통적인 문제점을 해결하기 위한 방법들을 묶어논 것이다.

디자인 패턴에는 생성, 구조, 행위 패턴이 존재하는데 유니티가 작동하는 방식을 이해하기 위해서 가장 중요한 컴포넌트 패턴과 싱글톤 패턴에 대해서 알아볼 것이다.
### 컴포넌트 패턴
>컴포넌트 패턴이란?
코드를 부품화 하는 개념으로 기능별로 다른 컴포넌트를 구현하는 패턴이다.

게임 개발에 있어서 한 개체에 대해 여러가지 로직을 한번에 구현할 수 있지만 이는 코드가 길어지고 기능이 많아질수록 나중에 작은 기능을 수정할 때 매우 많은 부분을 수정해야 할 수도 있다. 이를 해결하기 위해 기능별로 다른 컴포넌트를 만들어 유지보수를 쉽게 한다.
유니티에서는 Cube개체의 컴포넌트와 GameObject에 Cube의 컴포넌트들을 복사한 것은 같은 종류라고 할 수 있다는 것이다. 추가로 .cs파일을 컴포넌트로 추가해 Start()와 Update()를 정의할수도 있다.
### 싱글톤 패턴
>싱글톤 패턴이란?
전역 변수를 사용하지 않고 인스턴스를 하나만 생성하도록하여 어디에서든지 참조할 수 있도록 하는 패턴

**Scripts/Managers/Managers.cs 참조**
앞으로의 프로젝트에서 Managers.cs를 만들어 전체를 관리하는 관리자 객체를 만들건데 이를 호출할 때마다 인스턴스를 생성하게 된다면 Manager의 수가 많아져 관리와 처리가 힘들것이다. 이를 해결하기 위해 싱글톤 패턴을 사용할 것인데 이는 인스턴스가 존재한다면 new Object()로 생성할 것이고, 이미 존재한다면 존재하는 인스턴스를 return하는 구조로 구현할 것이다.
GameObject를 이름으로 찾는방법은 자주 사용하면 안되는 방법인데 역시 여기에도 싱글톤패턴을 사용할수 있다.
```c#
        if (s_instance == null)
        {
            //생성하고자 하는 인스턴스가 존재하는지 탐색
            GameObject go = GameObject.Find("@Managers");
            //없으면 새로 생성
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
```

## 트랜스폼
유니티의 Instpector창에 트랜스폼 컴포넌트라는 이동좌표관련된 컴포넌트가 있다. 본 내용에서는 앞으로 만들 플레이어의 이동, 회전 구현방법과 구현 과정에서 필요한 요소들을 다를 것이다.
학습을 위해서 에셋스토어에서 무료인 Unity-Chan이라는 무료 에셋을 사용할 예정이다.
### 플레이어 설정
**Scripts/PlayerController.cs 참조**
MMORPG를 만들기 위해서는 이동조작은 거의 필수적으로 필요한 요소이다. WASD를 이용해 상하좌우를 구현할 예정인데 구현할 내용은 다음과 같다.
```
    1. W를 누르면 앞으로 이동한다.
    2. S를 누르면 뒤로 이동한다.
    3. A를 누르면 왼쪽으로 이동한다.
    4. D를 누르면 오른쪽으로 이동한다.
```
위에서 언급했듯이 위치에 대한 조정을 하려면 transform을 조작해야 한다. 
```c#
        if (Input.GetKey(KeyCode.W))
            transform.position += new Vector3(0.0f, 0.0f, 1.0f);
        if (Input.GetKey(KeyCode.S))
            transform.position -= new Vector3(0.0f, 0.0f, 1.0f);
        if (Input.GetKey(KeyCode.A))
            transform.position -= new Vector3(1.0f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.D))
            transform.position += new Vector3(1.0f, 0.0f, 0.0f);
```
![PlayerController01](https://user-images.githubusercontent.com/44914802/126035611-62f49ed3-dc6d-4d09-b18d-96aac571b284.gif)  
하지만 움직이는 것을 보면 살짝만 눌렀음에도 너무 빠르게 많이 움직이는 것을 볼 수 있다.
### Position
속도가 너무 빠르다면 적절한 상수로 조절을 해야할 필요가 있다. 방향은 정해 졌으니 Time.deltaTIme과 _speed를 따로 정의해 속도를 조절할 수 있다.
```c#
        float _speed = 10.0f;
        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.forward * Time.deltaTime * _speed;
        if (Input.GetKey(KeyCode.S))
            transform.position -= Vector3.back * Time.deltaTime * _speed;
        if (Input.GetKey(KeyCode.A))
            transform.position -= Vector3.left * Time.deltaTime * _speed;
        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * Time.deltaTime * _speed;
```
여기서 문제점이 발생하게 되는데 transform.position은 게임내의 절대좌표를 의미하고 WASD를 입력할 때 움직이는 방향은 절대좌표가 기준이 아닌 캐릭터가 바라보는 방향에 대한 값을 가지고 있어야 한다. 이때 사용하는 기능이 TransformDirection이다. 이는 Local좌표를 World좌표로 바꾸는 기능을 가지는데 이함수를 사용하면 방향에 대한 걱정은 안해도 된다.
```c#
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.TransformDirection(Vector3.forward * Time.deltaTime * _speed);
        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.TransformDirection(Vector3.back * Time.deltaTime * _speed);
        if (Input.GetKey(KeyCode.A))
            transform.position -= transform.TransformDirection(Vector3.left * Time.deltaTime * _speed);
        if (Input.GetKey(KeyCode.D))
            transform.position += transform.TransformDirection(Vector3.right * Time.deltaTime * _speed);
```
### Input Manager
**Scripts/Managers/InputManager.cs 참조**
게임 규모가 작다면 Update()에서 키보드 입력을 하나씩 체크하는 것은 게임 속도에 영향을 크게 미치지 않는다. 하지만 게임규모가 커진다면 모든 플레이어의 키보드입력을 체크하는것은 굉장히 큰 성능부하가 될 것이다.
>Update()문에 직접 체크를 하지 않고 InputManager를 하나를 정의해 여기서 이벤트를 처리하도록 하자!

InputManager는 대표로 입력을 체크에 이벤트를 체크해 메시지를 전파하는 역할을 가진다.
#### 코드 설명
1. InputManager.cs
```c#
public class InputManager
{
    public Action KeyAction = null;

    public void OnUpdate()
    {
        //아무것도 눌리지 않았으면 함수종료
        if (Input.anyKey == false)
            return;

        //눌린게 있다면 해당 Action을 전파
        if (KeyAction != null)
            KeyAction.Invoke();

    }
}
```
2. Managers.cs
```c#
    //Manager를 총괄하는 Managers.cs
    //InputManager에 대한 초기화, 접근을 위한 Input정의
    InputManager _input = new InputManager();
    public static InputManager Input { get { return instance._input; } }
    ...
    
    //기존의 Update의 이벤트 처리를
    //위에서 생성한 _input에게 맡긴다 
        void Update()
    {
        _input.OnUpdate();
    }
```
## Prefab
게임을 구현하다 보면 캐릭터가 총을 여러발을 쏘거나 많은 몬스터가 젠되었을 때 하나씩 생성하고 위치를 조정하는 방법은 너무 복잡하고 효율적이지 않은 방법이다. 이 때 사용하는 것이 바로 Prefab이라는 것인데 Prefab은 미리 만들어 놓은 게임 포브젝트로 원하는 만큼 Prefab Instance를 생성해 사용하는 기능이다.
```
    1. 나중에 속성이 변경될 때 Prefab의 속성을 변경해주면 모든 Prefab Instance의 속성이 변경된다.
    2. 하이라키에서 Prefabs폴더에 드래그드롭으로 구현한다.
```
### Resource Manager
**Scripts/Managers/ResourceManager.cs 참조**
Object를 생성할 때 직접 함수를 호출함으로 Object를 생성하는 것도 하나의 방법이지만 코드가 많아지면 많아질수록 누가 생성하였는지 추적이 어려워진다. InputManager와 같이 Resource를 관리하는 Manager를 둬가지고 ResourceManager를 통해 생성하도록 수정할 필요가 있다.
>ResourceManager는 prefab불러오기, 생성, 삭제기능을 가져야 한다.
#### 코드 설명
1. ResourceManager.cs
```c#
public class ResourceManager
{
    public T Load<T>(string path) where T:Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        Object.Destroy(go);
    }
}
```
2. Managers.cs
```c#
    //Manager를 총괄하는 Managers.cs
    //ResourceManager 대한 초기화, 접근을 위한 Resource정의
    ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource { get { return instance._resource; } }
    ...
```
## Collision
게임에서 충돌은 정말 중요하고 많이 사용된다. 캐릭터가 활을 쏘거나 총을 쏠 때 화살, 총알의 충돌이나 캐릭터간의 충돌로 인해 이동에 방해를 받는 상황과 같이 실제 충돌은 많이 쓰이게 된다.
### RigidBody Collider
이는 캐릭터가 땅에 서있는 것도 포함되는데 충돌이 존재하지 않다면 다음과 같은 상황이 발생하게 된다.
![Collision01](https://user-images.githubusercontent.com/44914802/126871758-926acf13-65ad-4483-9e82-b5bd12a03f33.gif)  
실제 유니티에서 물리엔진을 구현하기 위해 RigidBody라는 컴포넌트를 사용하는데 이를 사용하게 되면 Object가 자유낙하를 할 수 있게 된다. RigidBody에서는 질량, 중력 여부와 같이 실제 물리엔진에 사용될 상수들을 설정할 수 있다. 또한, 충돌을 위해서 충돌이 될 범위를 지정해줘야 하는데 이는 Collider라는 컴포넌트를 사용한다. 잘 적용하였다면 다음과 같이된다.
![Collision02](https://user-images.githubusercontent.com/44914802/126871835-04e6993b-bc9b-42d0-81d4-212e01b9f7e4.gif)  
### Wall Collision
현재 바닥과 캐릭터의 충돌구현까지 하였다. 같은 원리로 물체를 하나 만들어서 두 물체를 충돌시킨다면 어떻게 되는지 확인해보자.  
![Collision03](https://user-images.githubusercontent.com/44914802/126889914-7938a64b-9dd9-4df0-a306-7a9035f3b928.gif)  
캐릭터가 충돌한 후 넘어지게 된다. 이를 해결하려면 충돌할 때 발생하는 이벤트를 처리해주어야 한다. 이벤트를 받기 위한 방법은 2가지가 있다.
#### Collision으로 받는 방법(OnCollisionEnter)
```
    1) 나한태 RigidBody가 있어야 한다.
    2) 나와 상대에게 Collider가 있어야 한다.
```
#### Trigger를 받는법(OnTriggerEnter)
```
    1) 나와 상대에게 Collider가 있어야 한다.
    2) 둘 중 하나는 IsTrigger속성을 On해줘야 한다.
    3) 둘 중 하나는 RigidBody가 있어야 한다.
```
### Ratcasting
현재까지의 내용으로는 어떤 구역을 클릭했을 때 어떤 물체가 있는지 알 수는 없다. 카메라의 시점에 따라 위치에 따라 클릭하는 좌표가 달라지기 때문이다. 이때 사용하는 기술이 Raycasting이라는 기술이다. Raycasting은 카메라 시점에서 누른 좌표로 광선을 쏜다고 가정해 어떤 물체에 부딪히게 된다면 해당 물체를 선택하게 된다는 접근을 기반으로한 기술이다.
```c#
    Physics.Raycast(위치(Vector3), 방향(Vector3))
```
Raycast의 매개변수의 자료형은 Vector3인데 실제 스크린의 점은 2D좌표이다. 그렇다면 스크린의 좌표를 실제 3D좌표로 변환하는 과정이 필요하다.
#### 좌표변환
여태까지 다룬 좌표영역은 Local과 World이다. 이는 흔히 알고있는 절대좌표와 상대좌표로 이해했는데 여기에 Screen 좌표계가 추가되는 것이다.  
![Collision04](https://user-images.githubusercontent.com/44914802/126890493-88b93c95-4ae7-4d1a-9c4f-1dd06080fb80.png)  
