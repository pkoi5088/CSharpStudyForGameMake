# 게임 서버
본격적인 서버를 다루는 파트. 네트워크, 멜티쓰레드, OS에 대해서 배울 것이며 Part4에서는 게임서버를 바닥에서부터 완성까지 다룰 예정이다.
## 내용
* [멀티쓰레드 프로그래밍](#멀티쓰레드-프로그래밍)
    + [멀티쓰레드](#멀티쓰레드)
    + [쓰레드 생성](#쓰레드-생성)
## 멀티쓰레드 프로그래밍
### 멀티쓰레드
알고리즘이나 유니티엔진을 공부할때는 특별한 컴퓨터 지식이 필요가 없었는데 서버가 들어가게 되기 시작하면 멀티쓰레드를 적용해야 하는데 이를 위해서는 컴퓨터구조 원리와 운영체제에 대한 지식이 필요하다.
### 쓰레드 생성
1. 쓰레드 생성하는 법
```c#
    Thread t=new Thread(function);
    t.Start();
```
c#에서 쓰레드는 기본적으로 foreground로 실행된다. background로 실행시키기 위해서는 t.IsBackgound=true로 해주어야 한다. background로 실행된다면 Main함수에서 쓰레드가 실행되던 말던 프로그램을 종료시킨다.
2. 쓰레드가 종료될 때까지 기다리기
```c#
    t.Join();
```
만약 구조가 t.Start(funtion) ... t.Join()구조라면 메인쓰레드에서 ... 부분을 수행한 뒤 t.Join()에서 메인쓰레드는 t가 종료될때까지 기다린다.
3. 쓰레드 이름 정하기
```c#
    t.Name="ThreadName";
```
t의 Name속성(문자열)을 바꿔줌으로 이름을 바꿔준다.
4. 쓰레드 풀(Thread Pool)
```c#
    ThreadPool.QueueUsersWorkItem(callback,object);
    ThreadPool.SetMinThreads(n,iothread);
    ThreadPool.SetMaxThreads(m,iothread);
```
간단한 일을 수행할 때마다 직접 쓰레드를 만들고 실행시키기에는 부담이 크다. c#에는 쓰레드 풀(Thread Pool)이라는 클래스가 있는데 이는 이미 쓰레드 풀을 만들어 놓고 작업 쓰레드를 할당 받아 사용하는 방식이다. 또한 SetMinThreads, SetMaxThreads를 이용해 쓰레드의 개수를 조절할 수 있다. 위의 상태는 최소쓰레드는 n개 최대쓰레드는 m개인 상황이다.
5. Task
```c#
    Task t=new Task(action,cancellationToken);
```
쓰레드 풀같은 경우는 작업하는데 오래걸리는 일을 할당하면 전체 프로그램이 먹통이 될 수 있다는 단점을 가지고 있는데 이를 극복할 수 있는 방법은 Task를 이용하는 방법이 있다. Task는 쓰레드가 직원을 고용하는 것이라면 Task는 직원이 할 일단위를 만드는 느낌이다. Task 역씨 쓰레드와 마찬가지로 쓰레드풀에 들어가긴 하는데 Task의 인자에 cancellationToken이라는 것을 이용해 오래걸리는 작업이라는것을 표현할 수 있다. cancellationToken에 TaskCreationOptions.LongRunning을 넣는다면 쓰레드 풀에 넣을 때 오래 걸리는 작업임을 알기 때문에 쓰레드풀에 넣는것이 아니라 별도로 처리하기 때문에 쓰레드개수에 영향을 주지 않는다.
### 컴파일러 최적화
쓰레드를 사용할 때, 쓰레드마다 스택메모리가 할당 되는데 전역으로된 변수들은 모든 쓰레드들이 공통으로 사용해 접근이 가능하다.
```c#
    static bool _stop = false;
```
쓰레드들의 상호작용을 위해 다음과 같은 쓰레드함수와 메인함수를 정의한다.
```c#
        static void ThreadMain()
    {
        Console.WriteLine("쓰레드 시작!");
        while (_stop == false)
        {
            //누군가가 stop신호를 해주기를 기다린다.
        }
        Console.WriteLine("쓰레드 종료!");
    }
    
        static void Main(string[] args)
    {
        Task t = new Task(ThreadMain);
        t.Start();
    
        Thread.Sleep(1000);
        _stop = true;
    
        Console.WriteLine("Stop호출");
        Console.WriteLine("종료 대기중");
        t.Wait();
        Console.WriteLine("종료 성공");
    }
```
위의 코드들의 결과를 예측해보면 메인쓰레드가 생성한 쓰레드의 이름을 A라고 하자. 메인쓰레드에서는 A를 만들고 1000ms(1초)동안 Sleep상태로 들어가고 되고 이 1초동안 A는 "쓰레드 시작!"을 출력하고 _stop이 false이기 때문에 무한루프에 들어가게 된다. Sleep이 끝난뒤, 메인쓰레드는 _stop을 true로 바꿔주고 "Stop호출", "종료 대기중"을 출력한 뒤 A가 끌나기를 기다린 후 "종료 성공"을 출력한다.  
![CompilerOptimiztion01](https://user-images.githubusercontent.com/44914802/145533541-7b97d2f2-61bf-4ae4-b17f-291b998488d9.PNG)
#### 문제점
이제까지 우리는 코드를 디버그모드로 만들고 있었다. 나중에 게임을 배포하게 되면 디버그가 아닌 릴리즈로 실행하게 된다. 릴리즈모드는 디버그모드보다 최적화가 잘되어 있어 속도가 더 빨라지게 된다. 릴리즈모드로 위와 같은 코드를 실행시키면 이상하게도 무한루프에 빠지게 된다.  
![CompilerOptimiztion02](https://user-images.githubusercontent.com/44914802/145534064-cca70dc6-407a-4147-ad9c-63c1efe1e0b2.PNG)
##### 원인
이게 발생한 원인은 바로 릴리스모드로 실행하면서 컴파일러가 최적화를 시켰기 때문이다.
```c#
        static void ThreadMain()
    {
        Console.WriteLine("쓰레드 시작!");
        while (_stop == false)
        {
            //누군가가 stop신호를 해주기를 기다린다.
        }
    }
```
위 ThreadMain함수를 보면 _stop이 변경되는 부분이 없기 때문에 컴파일러가 위의 while문을 다음과 비슷한 형태로 변경한다.
```c# 
    if(_stop==false){
        while(true){
            
        }
    }
```
이렇게 바꾸는 것이 어떻게 합당하냐고 판단하는지 생각해보면 while문안에 _stop을 바꿔주는 부분이 없기 때문에 불필요하게 여러번 _stop==false를 따지는 것보다는 한번만 따지고 무한루프를 돌게 하는 것이 더 빠르다고 판단했을 것이다.
##### 해결법
이를 해결하는 가장 간단한 방법은 _stop변수를 선언할 때 volatile를 앞에 붙이는 것이다. 이는 이 변수는 언제 변할지 모르니 최적하하지말라는 의미이다.
```c#
    volatile static bool _stop = false;
```