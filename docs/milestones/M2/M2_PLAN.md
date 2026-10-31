# Kế Hoạch Triển Khai Chi Tiết: Milestone M2 (3D Character Controller & Isometric Camera)

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Cột mốc:** `M2 — 3D Character Controller & Camera`  
**Trạng thái:** Đang triển khai (IN_PROGRESS)

---

## A. Overview (Tổng Quan)

* **Mục tiêu:** 
  1. Lập trình bộ điều khiển nhân vật 3D (`PlayerMovementController`) bằng `CharacterController`, di chuyển mượt mà 8 hướng theo góc nhìn nghiêng của Camera (Camera-relative movement).
  2. Tích hợp hệ thống Input System hiện đại: Hỗ trợ phím `WASD` di chuyển, phím `Shift` chạy nước rút (Sprint), và xoay mặt mượt mà theo hướng di chuyển hoặc con trỏ chuột.
  3. Cấu hình **Cinemachine Virtual Camera** theo dõi nhân vật ở góc nhìn **Isometric 3/4** cố định (góc nghiêng $45^\circ$, độ cao $16\text{m}$) kèm độ mượt chống giật (`Damping`).
  4. Áp dụng kiến trúc tách biệt **Logic Root** (`Player`) và **Visual Root** (`PlayerVisual`), chuẩn bị sẵn máy trạng thái Animation (`Idle`, `Walk`, `Run`).
  5. Xử lý va chạm địa hình chuẩn mực: leo dốc đến $45^\circ$, bước qua gờ nhỏ ($0.3\text{m}$ step offset), bị cản lại bởi tường/đá và có trọng lực rơi tự nhiên khi rơi khỏi gờ đất.
* **Lý do cần milestone:** Di chuyển và quan sát là trải nghiệm xúc giác đầu tiên và quan trọng nhất của một game Action RPG sinh tồn. Nếu camera bị rung hoặc di chuyển bị trượt, toàn bộ các hệ thống chiến đấu (M3) và sinh tồn (M4) phía sau sẽ bị ảnh hưởng nghiêm trọng.
* **Người chơi làm được gì sau M2:** Điều khiển nhân vật 3D chạy khắp bản đồ thử nghiệm bằng phím WASD, chạy nước rút bằng Shift, nhân vật xoay mặt tự nhiên theo hướng đi, camera lướt theo êm ái, leo các con dốc $15^\circ - 45^\circ$ mà không bị kẹt hay rơi xuyên mặt đất.
* **Phạm vi bao gồm:** Input Controller, Character Controller, Cinemachine Follow, Decoupled Visual, Animation Controller cơ bản, Test Scene địa hình dốc.
* **Phạm vi không bao gồm:** Chưa làm cơ chế tấn công (M3), chưa trừ thanh thể lực Stamina (M4), chưa import model 3D cuối cùng.

---

## B. Prerequisites (Điều Kiện Tiên Quyết)

* **Milestone phụ thuộc:** `M1` (Đã hoàn thành 100%, standalone build chạy 376 FPS).
* **Package đã cài sẵn trong project:**
  * `com.unity.inputsystem` (1.20.1)
  * `com.unity.cinemachine` (2.10.7)
  * `com.unity.render-pipelines.universal` (17.0.1)

---

## C. Learning Plan (Khái Niệm Cần Học Dưới Góc Nhìn Java Web)

### 1. Khái niệm: Di chuyển theo góc nghiêng của Camera (Camera-Relative Movement)
* **Khái niệm:** Khi bấm phím `W` (Đi thẳng về phía trước), nếu ta chỉ cộng tọa độ trục $Z$ thế giới (`Vector3.forward`), nhân vật sẽ đi thẳng theo hướng Bắc thế giới. Nhưng trong góc nhìn Isometric 3/4 nghiêng $45^\circ$, "hướng thẳng trên màn hình" thực chất là đường chéo chếch hướng Đông Bắc!
* **Tại sao cần:** Người chơi luôn kỳ vọng bấm `W` là nhân vật đi lên phía trên màn hình họ đang nhìn, bấm `S` là đi xuống dưới, bấm `A` là sang trái, bấm `D` là sang phải.
* **Cách hoạt động:**
  1. Lấy hướng nhìn của Camera: `Vector3 camForward = Camera.main.transform.forward; Vector3 camRight = Camera.main.transform.right;`
  2. Triệt tiêu độ cao $Y$ để giữ vector chuyển động song song với mặt đất: `camForward.y = 0; camRight.y = 0;`
  3. Chuẩn hóa vector (`normalized`) để tốc độ đi chéo không bị nhanh hơn đi thẳng:
     $$\vec{V}_{\text{move}} = (\vec{V}_{\text{camRight}} \times \text{Input}_X + \vec{V}_{\text{camForward}} \times \text{Input}_Y).\text{normalized}$$
* **Liên hệ Java/Web:** Tương tự như việc bạn chuyển đổi tọa độ chuột từ Tọa độ Màn hình trình duyệt (Screen Coordinates - CSS px) sang Tọa độ Hệ thống bản đồ (World Coordinates / Canvas Map) trong giao diện Dashboard Web.
* **Lỗi thường gặp:** Quên triệt tiêu `camForward.y = 0`, làm nhân vật bị dìm xuống đất hoặc bay lên trời khi camera nghiêng.

### 2. Khái niệm: `CharacterController` vs `Rigidbody`
* **Tại sao cần:** Unity cung cấp 2 giải pháp chính để di chuyển vật thể 3D.
* **So sánh:**
  * **`Rigidbody`:** Hoạt động dựa trên mô phỏng vật lý chân thực (lực đẩy `AddForce`, ma sát, khối lượng). Rất tốt cho game đua xe, game ném đồ, nhưng rất khó kiểm soát trong game RPG (dễ bị nảy tưng tưng khi chạm mép dốc, trượt dốc khi đứng yên, hoặc phản lực lạ khi va chạm quái vật).
  * **`CharacterController` (Được chọn):** Là một viên nang va chạm chuyên dụng cho game nhân vật góc nhìn thứ 3 / Isometric. Không bị ảnh hưởng bởi quán tính vật lý không mong muốn, tự động xử lý bước qua bậc thang nhỏ (`stepOffset`) và giới hạn góc leo dốc (`slopeLimit`), di chuyển chính xác 100% bằng lệnh `controller.Move(velocity * Time.deltaTime)`.
* **Liên hệ Java/Web:** `Rigidbody` giống như mô phỏng bất đồng bộ dựa trên luồng sự kiện ngẫu nhiên bên ngoài, còn `CharacterController` giống như một Transaction có kiểm soát trạng thái rõ ràng do bạn toàn quyền làm chủ.

### 3. Khái niệm: Tách rời Logic Root và Visual Root (Decoupled Visual)
* **Cách hoạt động:**
  * GameObject cha (`Player`): Chứa `CharacterController`, `PlayerMovementController`, va chạm, trọng lực.
  * GameObject con (`PlayerVisual`): Chứa Model 3D (`MeshRenderer`) và `Animator`.
  * Khi nhân vật đổi hướng di chuyển, chỉ xoay GameObject con `PlayerVisual` bằng nội suy mượt mà (`Quaternion.Slerp`), giữ nguyên góc tọa độ gốc của GameObject cha.
* **Lợi ích:** Sau này khi thay thế từ khối hình hộp sang model 3D kiếm sĩ/người sói, chỉ cần tráo Prefab `PlayerVisual` mà không làm lệch hệ thống va chạm vật lý.

---

## D. Implementation Tasks (Chi Tiết Các Nhiệm Vụ)

### `M2-T1`: Lập Trình Xử Lý Input Hiện Đại (`PlayerInputHandler.cs`)
* Đọc dữ liệu từ New Input System:
  * Vector 2D di chuyển (`Move`): Nhận tín hiệu từ phím `WASD` / phím mũi tên.
  * Trạng thái chạy nhanh (`Sprint`): Nhận tín hiệu giữ phím `Left Shift`.
  * Tọa độ con trỏ chuột (`MousePosition`): Nhận vị trí chuột trên màn hình để phục vụ ngắm đánh.
* Không phụ thuộc vào `Update()` của hệ thống cũ.

### `M2-T2`: Cấu Hình Cinemachine Isometric Camera Rig
* Tạo Prefab `IsometricCameraRig` gồm:
  * Camera vật lý (`Camera.main`) có gắn `CinemachineBrain`.
  * Virtual Camera (`CinemachineVirtualCamera`) góc nhìn 3/4:
    * `Follow`: Gắn vào `Player`.
    * `Body`: Chọn `Transposer` với Offset `(-14, 16, -14)` (góc nhìn Isometric Tây Nam đặc trưng của Albion).
    * `Damping`: Đặt `X = 0.5, Y = 0.5, Z = 0.5` giúp camera lướt theo nhân vật cực kỳ êm ái, không bị giật khựng khi nhân vật đổi hướng đột ngột.

### `M2-T3`: Lập Trình Bộ Điều Khiển Nhân Vật (`PlayerMovementController.cs`)
* Tốc độ đi bộ (`WalkSpeed`): $5\text{ m/s}$.
* Tốc độ chạy nhanh (`SprintSpeed`): $8.5\text{ m/s}$.
* Tốc độ xoay mặt (`RotationSpeed`): $12\text{ rad/s}$ (xoay mượt theo hướng chuyển động).
* Trọng lực (`Gravity`): $-15\text{ m/s}^2$ với cơ chế kiểm tra chạm đất (`isGrounded`) để nhân vật không bị tích lũy vận tốc rơi quá mức khi đứng yên trên sàn.
* Giới hạn leo dốc (`SlopeLimit`): $45^\circ$; Độ cao bước chân (`StepOffset`): $0.3\text{m}$.

### `M2-T4`: Cấu Hình Prefab Nhân Vật Prototype & Animation Controller
* Tạo Prefab `Player_Prototype`:
  * Root: `CharacterController` (Bán kính $0.4\text{m}$, Chiều cao $1.8\text{m}$).
  * Visual: Capsule/Cylinder đại diện có gắn phụ kiện "mũi nón" (Cone/Cube nhỏ phía trước) để nhìn rõ hướng mặt nhân vật khi di chuyển.
  * Animator: Tạo `PlayerAnimatorController` điều khiển tham số `Speed` (Float) chuyển đổi giữa `Idle` $\leftrightarrow$ `Walk` $\leftrightarrow$ `Run`.

### `M2-T5`: Dựng Scene Thử Nghiệm Địa Hình Đa Dạng (`Test_Movement.unity`)
* Tạo scene mới `Assets/_Project/Scenes/Sandbox/Test_Movement.unity` bao gồm:
  1. Khu vực sàn phẳng rộng $40\text{m} \times 40\text{m}$ để test chạy 8 hướng và chạy nước rút.
  2. Ba con dốc với các góc nghiêng khác nhau: dốc thoai thoải $15^\circ$, dốc vừa $30^\circ$ và dốc giới hạn $45^\circ$.
  3. Một con dốc cấm leo dốc đứng $60^\circ$ (nhân vật trượt lại không trèo lên được).
  4. Hệ thống bậc thềm cao $0.2\text{m}$ (bước qua được) và $0.6\text{m}$ (bị chặn lại).
  5. Các cột đá chướng ngại vật để kiểm tra va chạm không bị xuyên tường.

### `M2-T6`: Unit Tests Cho Logic Di Chuyển
* Viết bộ kiểm thử `MovementMathTests.cs` (EditMode):
  * Kiểm tra tính toán vector camera-relative không bị lệch hướng.
  * Kiểm tra vector đường chéo luôn có độ dài $\le 1.0$ (không bị hack tốc độ khi bấm cùng lúc W và D).
  * Kiểm tra công thức gia tốc trọng lực.

---

## E. Architecture & Code Design

```text
Player (GameObject - Root)
 ├── CharacterController (Vật lý & Va chạm)
 ├── PlayerInputHandler (Hứng Input từ bàn phím & chuột)
 ├── PlayerMovementController (Tính toán vận tốc, trọng lực, camera-relative)
 └── PlayerVisual (GameObject con - Xoay độc lập)
      ├── Mesh (Capsule 3D + Mũi định hướng)
      └── Animator (Quản lý các trạng thái hoạt ảnh)
```

---

## F. Acceptance Criteria (Tiêu Chí Nghiệm Thu M2)

- [ ] Bấm `WASD` nhân vật di chuyển đúng 8 hướng tương ứng với màn hình hiển thị (bấm W nhân vật chạy chếch lên trên màn hình).
- [ ] Giữ `Left Shift` nhân vật tăng tốc độ từ đi bộ ($5\text{ m/s}$) sang chạy nhanh ($8.5\text{ m/s}$).
- [ ] Nhân vật xoay mặt mượt mà theo hướng di chuyển, không bị giật ngược $180^\circ$ tức thì.
- [ ] Camera Cinemachine bám theo nhân vật mượt mà, giữ góc nhìn Isometric $45^\circ$, không rung lắc khi đổi hướng.
- [ ] Nhân vật bước lên được các dốc nghiêng $\le 45^\circ$ và bậc thang $\le 0.3\text{m}$.
- [ ] Nhân vật bị chặn lại bởi tường đá, không xuyên vật cản và rơi xuống tự nhiên khi bước hụt khỏi gờ cao.
- [ ] Pass 100% Unit Tests tính toán vector di chuyển.

