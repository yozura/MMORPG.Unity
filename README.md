## MMORPG.Unity    
### 지식공유자 : [Rookiss](https://www.inflearn.com/instructors/230375/courses)  
## 교육 일기 
### 0920_Singleton, Component
유니티는 Component 패턴 으로 이루어져있고 해당 패턴을 이해할 수 있고 가장 많이 쓰이는 패턴 중 하나인 **Singleton 패턴** 을 이용해 Managers 클래스를 만들어 각종 Manager들을 관리함. 
*Ex)SoundManger, ResourcesManager, InputManager 등...* 
### 0921_Position 
유니티 내부의 **World 좌표계**와 **Local 좌표계**를 구분하고 필요에 따라 사용할 수 있다. 
### 0922_Vector3, Rotation, Action  
**Vector3**가 어떤식으로 작동하는지에 대해 직접 **MyVector를 구현**하면서 배웠고 **operator(함수 연산자)** 문법에 대해 더 자세히 알게 되었음.
**System namespace**에서 기본적으로 지원해주는 **Action Delegate**를 사용해봤고 **Rotation**을 바꾸는 함수 및 게임에서 회전을 구현할 때 **Quaternion(사원수)** 을 이용하는 이유에 대해 배움  *Ex) Gimbal lock(짐벌락)*      
### 0923_Prefab, Collider, Rigidbody    
**Nested Prefab(프리팹 안의 프리팹), Prefab Variant(변형 프리팹)** 에 대해 자세히 알게 되었고 어떤 상황에 사용되는지 구분할 수 있다.
**Physic Rigidbody, Collider** 를 사용할 수 있고 유니티에서 지원하는 **OnCollision 및 OnTrigger 함수** 를 용도에 맞게 사용할 수 있다.
### 0925_Raycast, Camera, Animation 
**Physics.RayCast** 함수를 이용해 오브젝트의 충돌을 구현할 수 있고 **CameraController**를 만들어서 카메라의 위치 변환을 전담하도록 했다. 
**LayerMask**를 이용해 충돌에서 제외시키거나 원하는 Layer만 충돌할 수 있도록 할 수 있고 그 점을 이용해 카메라와 플레이어 사이에 벽이 있을 경우 벽을 Raycast로 감지해서 카메라가 플레이어에게 근접하도록 구현했다. **Animator.Play()** 함수를 이용해 원하는 부분에 사용할 수 있도록 했다. **Animation Blend Tree**를 이용해서 IDLE 애니메이션과 RUN 애니메이션 사이의 부자연스러운 점을 믹싱해서 자연스럽게 구현했다. 
