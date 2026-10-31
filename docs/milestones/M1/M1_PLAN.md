# Kế Hoạch Triển Khai Chi Tiết: Milestone M1 (Development Environment & Unity Foundation)

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Cột mốc:** `M1 — Development Environment & Unity Foundation`  
**Trạng thái:** Sẵn sàng thực thi (Ready to Implement)

---

## A. Overview (Tổng Quan)

* **Mục tiêu:** 
  1. Khởi tạo một Unity 6 Project sạch chuẩn Universal Render Pipeline (URP 3D) ngay trong workspace.
  2. Cấu hình `.gitignore` và `.gitattributes` chuẩn cho Unity để chống phình to repo Git khi 6 thành viên cùng làm việc.
  3. Xây dựng cấu trúc thư mục chuẩn `Assets/_Project/` phân tách rõ ràng mã nguồn dự án khỏi các plugin tải thêm.
  4. Cài đặt và xác minh các package bắt buộc: `New Input System`, `Cinemachine`, `AI Navigation`, `Unity Test Framework`.
  5. Tạo Scene nền tảng đầu tiên (`Sandbox_Foundation.unity`) và thực hiện bản build `.exe` rỗng đầu tiên để xác nhận pipeline đóng gói hoạt động trơn tru.
* **Lý do cần milestone:** Trong phát triển game, một project rác hoặc thiếu `.gitignore` chuẩn sẽ làm Git nghẽn hàng chục GB thư mục tạm (`Library/`, `Temp/`), gây xung đột không thể sửa giữa 6 thành viên. Đây là nền móng kỹ thuật vững chắc trước khi viết bất kỳ dòng gameplay nào.
* **Người chơi làm được gì sau M1:** Chạy được file `.exe` độc lập trên Windows, thấy màn hình mở lên mượt mà với sàn phẳng, ánh sáng mặt trời URP và tốc độ 60+ FPS ổn định.
* **Phạm vi bao gồm:** Cấu hình project, Git, Packages, Folder structure, Test scene, Build pipeline.
* **Phạm vi không bao gồm:** Chưa import model nhân vật 3D, chưa làm logic di chuyển, chưa làm combat hay UI phức tạp.

---

## B. Prerequisites (Điều Kiện Tiên Quyết)

* **Milestone phụ thuộc:** `M0` (Đã hoàn thành).
* **Công cụ & Môi trường đã sẵn sàng:**
  * Unity Editor: `6000.3.27f1` (Unity 6 LTS) tại `C:\Program Files\Unity\Hub\Editor\6000.3.27f1\Editor\Unity.exe`.
  * Git: Version `2.49.0` kèm Git LFS `3.6.1`.
  * IDE: Visual Studio Code (hoặc Rider / Visual Studio 2022).
* **Quyết định đã chốt:** URP 3D, C# .NET Standard 2.1, New Input System.

---

## C. Learning Plan (Kế Hoạch Tiếp Thu Kiến Thức Cho Java Web Developer)

### 1. Khái niệm: Cấu trúc thư mục của một Unity Project
* **Tại sao cần:** Để biết file nào là mã nguồn cần đẩy lên Git, file nào là file rác/file tạm do Unity sinh ra.
* **Cách hoạt động:**
  * `Assets/`: Chứa toàn bộ mã nguồn C#, 3D models, textures, scenes của bạn (BẮT BUỘC theo dõi trên Git).
  * `Packages/manifest.json`: Quản lý danh sách thư viện (tương tự `pom.xml` của Maven hoặc `package.json` của npm).
  * `ProjectSettings/`: Lưu cấu hình độ phân giải, phím bấm, đồ họa (BẮT BUỘC theo dõi trên Git).
  * `Library/`, `Temp/`, `Logs/`, `obj/`: Thư mục cache cục bộ do Unity tự sinh khi nạp tài nguyên (TUYỆT ĐỐI KHÔNG đẩy lên Git, dung lượng có thể lên tới 5–20GB).
* **Liên hệ Java/Web:** `Assets/` giống như `src/main/`, `Packages/manifest.json` giống `pom.xml`, còn `Library/` chính là thư mục `target/` trong Maven hoặc `node_modules/` / `.next/` trong web.
* **Ứng dụng vào dự án:** Cấu hình file `.gitignore` để chặn toàn bộ `Library/`, `Temp/`, `Builds/`.
* **Lỗi thường gặp:** Quên file `.gitignore`, commit cả thư mục `Library/` lên Git khiến đồng đội pull về bị xung đột cache nhị phân.

### 2. Khái niệm: GameObject, Component, Prefab & Scene
* **Tại sao cần:** Đây là 4 khái niệm nền tảng tạo nên mọi thứ trong thế giới game Unity.
* **Cách hoạt động:**
  * **Scene:** Một màn chơi / không gian 3D chứa các thực thể (giống như một trang Web Page hoặc 1 View).
  * **GameObject:** Một thực thể rỗng tồn tại trong Scene có tọa độ vị trí, góc xoay và tỉ lệ (`Transform`).
  * **Component (`MonoBehaviour`):** Các module logic/tính năng gắn vào GameObject (ví dụ: `MeshRenderer` để vẽ hình, `Collider` để va chạm, `PlayerMovement` để di chuyển).
  * **Prefab (Pre-fabricated object):** Một khuôn mẫu GameObject đã cấu hình sẵn lưu thành file `.prefab` trong `Assets/` (tương tự như một Template / Component tái sử dụng trong React/Vue, hoặc Class OOP).
* **Liên hệ Java/Web:** 
  * `GameObject` giống như một Bean rỗng trong Spring.
  * Các `Component` giống như các `@Autowired` Service được tiêm vào Bean đó để cho nó khả năng xử lý.
  * `Prefab` giống như một bản thiết kế Class hoàn chỉnh có thể sinh ra (`Instantiate` ↔ `new`) hàng trăm cá thể (Instance) quái vật hoặc cây cối.
* **Lỗi thường gặp:** Thay đổi trực tiếp các GameObject trên Scene thay vì chỉnh sửa Prefab gốc, dẫn đến khi đổi scene khác thì mất hết thiết lập.

---

## D. Implementation Tasks (Chi Tiết Các Nhiệm Vụ Thực Hiện)

### Phase 1: Chuẩn Hóa Git & Khởi Tạo Unity Project

#### `M1-T1`: Thiết lập Git Ignore & Git LFS
* **Thứ tự phụ thuộc:** Không có.
* **Thao tác:** Tạo file `.gitignore` và `.gitattributes` chuẩn mực cho Unity 6 để loại trừ thư mục tạm (`Library/`, `Temp/`, `Build/`) và cấu hình Git LFS cho file 3D/Audio lớn (`.fbx`, `.png`, `.psd`, `.wav`).
* **File liên quan:** `.gitignore`, `.gitattributes`.
* **Kết quả mong đợi:** `git status` không hiển thị các file tạm của Unity.
* **Cách xác minh:** Chạy `git status` kiểm tra sạch sẽ.

#### `M1-T2`: Khởi tạo Project Unity 6 URP sạch
* **Thứ tự phụ thuộc:** Sau `M1-T1`.
* **Thao tác:** Khởi tạo cấu trúc project Unity 6000.3.27f1 URP 3D tại workspace `d:\4D1S_STARTUP_GAME_HUYETNGUYETDAINGAN\huyet-nguyet-dai-ngan`.
* **File liên quan:** `Packages/manifest.json`, `ProjectSettings/ProjectSettings.asset`.
* **Kết quả mong đợi:** Mở được project trên Unity Editor không có lỗi đỏ Console.

---

### Phase 2: Cấu Trúc Thư Mục & Cài Đặt Packages

#### `M1-T3`: Tạo Cây Thư Mục Chuẩn `Assets/_Project/`
* **Thứ tự phụ thuộc:** Sau `M1-T2`.
* **Thao tác:** Tạo đầy đủ các thư mục theo chuẩn đã phê duyệt trong `TECHNICAL_DECISIONS.md`:
  * `Assets/_Project/Art/` (`Characters/`, `Environment/`, `UI/`, `Materials/`)
  * `Assets/_Project/Data/` (Chứa `ScriptableObject`)
  * `Assets/_Project/Prefabs/` (`Player/`, `Enemies/`, `Environment/`, `UI/`)
  * `Assets/_Project/Scenes/` (`Core/`, `Sandbox/`, `Biomes/`)
  * `Assets/_Project/Scripts/` (`Core/`, `Player/`, `Combat/`, `AI/`, `World/`, `UI/`)
  * `Assets/_Project/Tests/` (`EditMode/`, `PlayMode/`)
* **Kết quả mong đợi:** Cấu trúc thư mục ngay ngắn, dễ quản lý cho team 6 người.

#### `M1-T4`: Cài đặt & Xác minh Packages Bắt Buộc
* **Thứ tự phụ thuộc:** Sau `M1-T2`.
* **Thao tác:** Đảm bảo `Packages/manifest.json` có đủ 4 gói:
  1. `com.unity.inputsystem` (New Input System).
  2. `com.unity.cinemachine` (Hệ thống Camera).
  3. `com.unity.ai.navigation` (NavMesh cho AI).
  4. `com.unity.test-framework` (Kiểm thử tự động).
* **Cách xác minh:** Mở Unity Editor kiểm tra Package Manager không báo lỗi dependency.

---

### Phase 3: Scene Thử Nghiệm & Kiểm Thử Build Độc Lập

#### `M1-T5`: Tạo Scene Thử Nghiệm `Sandbox_Foundation.unity`
* **Thứ tự phụ thuộc:** Sau `M1-T3` & `M1-T4`.
* **Thao tác:**
  * Tạo scene mới lưu tại `Assets/_Project/Scenes/Sandbox/Sandbox_Foundation.unity`.
  * Thêm 1 mặt sàn phẳng (Ground Plane $20\text{m} \times 20\text{m}$) có gắn Material URP màu xám trung tính.
  * Thêm 1 Directional Light (Ánh sáng mặt trời) nghiêng góc $50^\circ$.
  * Thêm 1 Main Camera đặt góc nghiêng Isometric ($45^\circ$).
* **Kết quả mong đợi:** Bấm Play trong Editor chạy mượt mà 60+ FPS, không có thông báo lỗi đỏ.

#### `M1-T6`: Thực hiện Bản Build Standalone `.exe` Đầu Tiên
* **Thứ tự phụ thuộc:** Sau `M1-T5`.
* **Thao tác:**
  * Mở `Build Settings` $\to$ Thêm `Sandbox_Foundation.unity` vào danh sách Scene in Build.
  * Chọn nền tảng `Windows (x86_64)`.
  * Đặt thư mục xuất tại `Builds/Windows/HuyetNguyetDaiNgan_TestBuild.exe`.
* **Kết quả mong đợi:** Quá trình compile và build thành công không lỗi. Mở file `.exe` lên cửa sổ game hiển thị đúng sàn phẳng và ánh sáng.
* **Cách xác minh:** Chạy file `.exe` độc lập ngoài Editor.

---

## E. Architecture & Code Design (Kiến Trúc & Quy Chuẩn)

* **Namespace Standard:** Mọi script viết mới đều phải nằm trong namespace:
  ```csharp
  namespace HuyetNguyetDaiNgan.Core { ... }
  namespace HuyetNguyetDaiNgan.Player { ... }
  namespace HuyetNguyetDaiNgan.Combat { ... }
  ```
* **Assembly Definitions (`.asmdef`):** Tạo file `HuyetNguyetDaiNgan.Runtime.asmdef` và `HuyetNguyetDaiNgan.Tests.asmdef` để tăng tốc độ compile và tách biệt code kiểm thử khỏi mã nguồn game.

---

## F. Asset Plan (Kế Hoạch Tài Nguyên)

* **Giai đoạn M1:** Hoàn toàn dùng **Primitive 3D của Unity** (Cube, Plane) và Material cơ bản của URP. Không cần tải asset bên ngoài.

---

## G. Test Plan (Kế Hoạch Kiểm Thử)

1. **Kiểm thử Compile:** Mở project, bấm tổ hợp `Ctrl + R` (hoặc Reload domain) $\to$ Console 0 Errors.
2. **Kiểm thử Play Mode:** Bấm nút Play $\to$ Khung hình duy trì $\ge 60\text{ FPS}$ trên máy RTX 4060, Game View hiển thị rõ sàn phẳng.
3. **Kiểm thử Git:** Thực hiện commit đầu tiên $\to$ Xác nhận không có file nhị phân trong thư mục `Library/` bị đẩy lên repo.
4. **Kiểm thử Build:** Chạy file `.exe` vừa xuất, bấm `Alt + F4` thoát game bình thường.

---

## H. Acceptance Criteria (Tiêu Chí Nghiệm Thu M1)

- [ ] File `.gitignore` và `.gitattributes` đã có trong thư mục gốc.
- [ ] Thư mục `Assets/_Project/` đã tạo đúng cấu trúc các thư mục con.
- [ ] Unity Editor mở project không có lỗi đỏ (`0 Red Errors`).
- [ ] Các package `Input System`, `Cinemachine`, `AI Navigation`, `Test Framework` đã được cài đặt trong `manifest.json`.
- [ ] Scene `Sandbox_Foundation.unity` mở lên và chạy được ở Play Mode.
- [ ] Bản build độc lập `.exe` chạy được trên máy tính của bạn.
- [ ] File `docs/handoff/CURRENT_STATE.md` được cập nhật trạng thái M1: `COMPLETED`.

---

## I. Risks & Troubleshooting (Lỗi Thường Gặp & Cách Sửa)

* **Lỗi 1: `Library\PackageCache\... EPERM / Lock error`:**
  * *Nguyên nhân:* Unity Editor đang chạy hoặc chương trình diệt virus khóa file.
  * *Cách sửa:* Tắt Unity Editor hoàn toàn trước khi chạy lệnh Git hoặc xóa thư mục cache.
* **Lỗi 2: Màn hình Game bị màu hồng cánh sen (Magenta/Pink):**
  * *Nguyên nhân:* Material đang ở chuẩn Built-in cũ thay vì Universal Render Pipeline (URP).
  * *Cách sửa:* Chọn Material $\to$ Shader $\to$ Đổi sang `Universal Render Pipeline/Lit`.
* **Lỗi 3: Xung đột Input System cũ và mới:**
  * *Nguyên nhân:* Unity hỏi khởi động lại khi cài gói Input System mới.
  * *Cách sửa:* Vào `Project Settings -> Player -> Active Input Handling` $\to$ Chọn `Input System Package (New)` và đồng ý Restart Editor.

---

## J. Handoff & Next Prompt (Bàn Giao & Bước Tiếp Theo)

Sau khi kiểm tra bản kế hoạch M1 trên, bước tiếp theo là bạn mở Unity Hub và tiến hành khởi tạo project sạch theo hướng dẫn, hoặc yêu cầu tôi tạo file `.gitignore`, `.asmdef` và cấu trúc thư mục trước.

