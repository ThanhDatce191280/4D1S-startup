# QUY CHUẨN TỐI ƯU HÓA ĐA CẤP ĐỘ CHI TIẾT (LOD) VÀ HIỆU NĂNG THỜI GIAN THỰC
## (CHARACTER LOD & RUNTIME OPTIMIZATION SPECIFICATION)

---

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mã tài liệu:** `CAB-SPEC-06`  
**Phiên bản:** `1.0.0`  
**Ngày cập nhật:** 31/10/2026  
**Trạng thái tài liệu:** `PROPOSED` (Cần benchmark thực tế trên fixture 6 người chơi theo [MVP_SCOPE.md](../../vision/MVP_SCOPE.md))  
**Tài liệu cha:** [CHARACTER_ART_BIBLE.md](../CHARACTER_ART_BIBLE.md)  
**Tác giả:** Technical Artist & Lead QA Coordinator  

---

## MỤC LỤC
1. [Bối Cảnh Hiệu Năng và Kịch Bản Thử Nghiệm Chuẩn (Performance Fixture)](#1-bối-cảnh-hiệu-năng-và-kịch-bản-thử-nghiệm-chuẩn-performance-fixture)
2. [Hệ Thống Phân Cấp Chi Tiết LOD (LOD Hierarchy Architecture)](#2-hệ-thống-phân-cấp-chi-tiết-lod-lod-hierarchy-architecture)
3. [Cấu Hình Unity LODGroup và Tỷ Lệ Màn Hình (Screen-Relative Heights)](#3-cấu-hình-unity-lodgroup-và-tỷ-lệ-màn-hình-screen-relative-heights)
4. [Tối Ưu Hóa Khung Xương và Lưới Gắn Xương (Skinned Mesh Optimization)](#4-tối-ưu-hóa-khung-xương-và-lưới-gắn-xương-skinned-mesh-optimization)
5. [Chiến Lược Cắt Tỉa Tầm Nhìn (Culling Strategies)](#5-chiến-lược-cắt-tỉa-tầm-nhìn-culling-strategies)
6. [Quản Lý Đổ Bóng và Chiếu Sáng Thời Gian Thực (Shadow Settings)](#6-quản-lý-đổ-bóng-và-chiếu-sáng-thời-gian-thực-shadow-settings)
7. [Tối Ưu Hóa Bộ Điều Khiển Hoạt Cảnh (Animator Culling Mode)](#7-tối-ưu-hóa-bộ-điều-khiển-hoạt-cảnh-animator-culling-mode)
8. [Ngân Sách Bộ Nhớ VRAM và Giới Hạn Lệnh Vẽ (Draw Call Budgets)](#8-ngân-sách-bộ-nhớ-vram-và-giới-hạn-lệnh-vẽ-draw-call-budgets)
9. [Bảng Phân Định Trạng Thái Các Cam Kết Hiệu Năng](#9-bảng-phân-định-trạng-thái-các-cam-kết-hiệu-năng)
10. [Quy Trình Kiểm Tra Hiệu Năng Tự Động (Automated Performance Verification)](#10-quy-trình-kiểm-tra-hiệu-năng-tự-động-automated-performance-verification)

---

## 1. BỐI CẢNH HIỆU NĂNG VÀ KỊCH BẢN THỬ NGHIỆM CHUẨN (PERFORMANCE FIXTURE)

Theo quy định kiến trúc [ADR-12](../../architecture/TECHNICAL_DECISIONS.md) và ranh giới kiểm thử [MVP_SCOPE.md](../../vision/MVP_SCOPE.md), trò chơi được thiết kế hướng tới khả năng tiếp cận rộng rãi tương tự *Albion Online*.

> [!WARNING]
> **Quy định bất biến:** Tuyệt đối không đưa ra lời hứa suông về chỉ số FPS cố định (ví dụ: "chắc chắn đạt 60 FPS trên mọi máy") khi chưa tiến hành đo đạc thực tế trên bản build độc lập!

### Kịch Bản Kiểm Thử Tải Trọng Tối Đa (Standard Stress-Test Fixture):
Mọi quyết định tối ưu hóa nhân vật phải dựa trên kịch bản chiến đấu đông thực thể nhất của giai đoạn Vertical Slice M8 / Demo M12:
- **06 Nhân vật người chơi đồng thời** (Multiplayer LAN 4–6 client).
- **20 Con Sói Bậc 1** (Đang bao vây công thành ban đêm).
- **10 NPC Dân làng / Thương nhân** (Đang di tản hoặc hỗ trợ thủ thành).
- **30 Công trình làng và chướng ngại vật** (Lũy tre, cổng đình làng, tháp nỏ).
- **Hiệu ứng sương mù và ánh sáng đỏ Đêm Huyết Nguyệt** đang hoạt động đồng thời.

---

## 2. HỆ THỐNG PHÂN CẤP CHI TIẾT LOD (LOD HIERARCHY ARCHITECTURE)

Hệ thống nhân vật sử dụng chuẩn **3 Cấp Độ Chi Tiết (LOD0 $\to$ LOD1 $\to$ LOD2)** kết hợp trạng thái Biến Mất Hoàn Toàn (*Culled*):

```
+-------------------------------------------------------------------------+
|                  HỆ THỐNG PHÂN CẤP CHI TIẾT (LOD PIPELINE)              |
+-------------------------------------------------------------------------+
| [ LOD0: CỰC CẬN & GÓC MÁY QUAN SÁT CHÍNH ]                              |
| - Khoảng cách camera: 0m - 18m (Bao phủ góc nhìn Isometric chuẩn 16m)   |
| - Đa giác: 100% ngân sách (8,000 - 12,000 Tris cho Người chơi)          |
| - Xương ảnh hưởng: 4 Bones / Vertex. Đầy đủ hiệu ứng đổ bóng & vải rủ.  |
+-------------------------------------------------------------------------+
                                   | (Thu nhỏ hình ảnh trên màn hình)
                                   v
+-------------------------------------------------------------------------+
| [ LOD1: TRẬN ĐỊA TẦM TRUNG ]                                            |
| - Khoảng cách camera: 18m - 30m (Quái vật ở rìa màn hình)               |
| - Đa giác: Giảm 50% - 60% (4,000 - 5,000 Tris)                          |
| - Xương ảnh hưởng: 2 Bones / Vertex. Tắt chuỗi xương áo choàng phụ.     |
| - Đổ bóng: Chuyển sang bóng cứng (Hard Shadows) hoặc đổ bóng thô.       |
+-------------------------------------------------------------------------+
                                   |
                                   v
+-------------------------------------------------------------------------+
| [ LOD2: TẦM XA & SƯƠNG MÙ FOG OF WAR ]                                  |
| - Khoảng cách camera: > 30m                                             |
| - Đa giác: Giảm 75% - 80% (1,500 - 2,000 Tris)                          |
| - Xương ảnh hưởng: 1 - 2 Bones / Vertex.                                |
| - Tắt hoàn toàn đổ bóng thời gian thực. Tắt Texture Normal Map.         |
+-------------------------------------------------------------------------+
                                   |
                                   v
+-------------------------------------------------------------------------+
| [ CULLED: NGOÀI TẦM QUAN SÁT ] (> 45m hoặc nằm ngoài góc nhìn Camera)   |
| - Tắt toàn bộ Mesh Renderer. Animator chuyển sang chế độ ngủ.           |
+-------------------------------------------------------------------------+
```

---

## 3. CẤU HÌNH UNITY LODGROUP VÀ TỶ LỆ MÀN HÌNH (SCREEN-RELATIVE HEIGHTS)

Trong Unity, thành phần `LODGroup` điều chỉnh việc chuyển đổi mức chi tiết dựa trên tỷ lệ chiều cao của nhân vật so với chiều cao khung hình màn hình (`Screen Relative Metric`):

```
+-- GameObject: Player_VisualRoot
    └── Component: LODGroup
        ├── LOD 0: Chiếm >= 35% chiều cao màn hình (Camera cận cảnh & Isometric gần)
        ├── LOD 1: Chiếm từ 15% - 35% chiều cao màn hình (Góc máy Isometric chuẩn)
        ├── LOD 2: Chiếm từ 4% - 15% chiều cao màn hình (Rìa màn hình / Tầm xa)
        └── Culled: Dưới 4% chiều cao màn hình (Ẩn hoàn toàn khỏi bộ nhớ render)
```

- **LOD Blending (Chuyển đổi mượt mà):**
  - Chế độ: `Cross Fade`
  - Fade Duration: $0.25\text{s}$ (Tránh hiện tượng vật thể bị "nhảy giật hình" đột ngột — *Popping Effect*).

---

## 4. TỐI ƯU HÓA KHUNG XƯƠNG VÀ LƯỚI GẮN XƯƠNG (SKINNED MESH OPTIMIZATION)

Lưới gắn xương (`SkinnedMeshRenderer`) là đối tượng tiêu tốn năng lực xử lý CPU/GPU lớn nhất trong game nhập vai. Các giải pháp bắt buộc:

1. **Giảm cấp số xương tác động trên đỉnh (Bone Weight Reduction):**
   - **LOD0:** `Quality = 4 Bones` (Cho phép ngón tay và khớp vai uốn cong hoàn hảo).
   - **LOD1:** `Quality = 2 Bones` (Giảm một nửa tải tính toán ma trận xương).
   - **LOD2:** `Quality = 1 Bone` (Tính toán như một khối cứng gắn xương cha).
2. **Loại bỏ xương phụ trợ ở LOD thấp (Bone Culling):**
   - Ở LOD1 và LOD2, ngắt liên kết các xương phụ: Chuỗi xương áo choàng lông (`Socket_Cape_01..03`), xương búi tóc, xương ngón tay cái. Toàn bộ bàn tay được gộp trọng lượng $100\%$ vào xương cổ tay (`Hand Bone`).
3. **Bật tùy chọn tối ưu lưới của Unity:**
   - Trong Model Import Settings: Bật **`Optimize Mesh Data`** (Tự động xóa các kênh UV/Color thừa không dùng trong Shader).
   - Bật **`GPU Skinning`** trong Player Settings để chuyển việc tính toán biến dạng đỉnh từ CPU sang GPU.

---

## 5. CHIẾN LƯỢC CẮT TỈA TẦM NHÌN (CULLING STRATEGIES)

Để CPU không phải gửi hàng ngàn lệnh vẽ vô nghĩa cho các đối tượng không nhìn thấy:

1. **Frustum Culling (Cắt tỉa hình chóp nón thị giác):**
   - Mặc định camera Isometric chỉ quét một hình chữ nhật trên mặt đất. Bất kỳ con quái vật nào nằm sau lưng camera hoặc ngoài mép màn hình lập tức bị loại khỏi danh sách dựng hình.
2. **Fog of War / Distance Culling (Cắt tỉa theo lớp sương mù):**
   - Giới hạn tầm nhìn nhân vật ban đêm là $25\text{m}$.
   - Thiết lập `Layer Cull Distances` trên Camera: Layer `Enemies_Small` (Sói thường) sẽ bị ẩn hoàn toàn ở cự ly $> 30\text{m}$.
3. **Occlusion Culling (Triệt tiêu vật thể bị che khuất):**
   - Khi nhân vật hoặc quái vật đứng sau Đình Làng to lớn hoặc vách đá hang động Karst, hệ thống nướng trước dữ liệu che khuất (*Baked Occlusion Map*) sẽ ngắt lệnh vẽ.

---

## 6. QUẢN LÝ ĐỔ BÓNG VÀ CHIẾU SÁNG THỜI GIAN THỰC (SHADOW SETTINGS)

Trong các trận đánh đông quái, việc tính toán bản đồ bóng đổ (*Shadow Map*) có thể ngốn tới $40\%$ hiệu năng GPU:

| Cấp Độ LOD | Thiết Lập Cast Shadows (Đổ Bóng) | Thiết Lập Receive Shadows (Nhận Bóng) | Phương Thức Thay Thế Nếu Lag |
| :--- | :--- | :--- | :--- |
| **LOD0** | `On (Hard / Soft Shadows)` | `On` | Bóng thời gian thực sắc nét |
| **LOD1** | `Hard Shadows Only` | `On` | Giảm độ phân giải bóng xuống 1/2 |
| **LOD2** | `OFF (TẮT HOÀN TOÀN)` | `Off` | Sử dụng vòng tròn bóng giả (*Blob Shadow Projector*) |
| **Sói Thường (20 con)**| Tối đa 5 con gần nhất bật bóng | Tắt | 15 con ở xa dùng Blob Shadow dưới chân |

---

## 7. TỐI ƯU HÓA BỘ ĐIỀU KHIỂN HOẠT CẢNH (ANIMATOR CULLING MODE)

Trong Unity, thành phần `Animator` trên các đối tượng ở xa hoặc ngoài màn hình không được phép tiếp tục tính toán ma trận chuyển động 60 lần/giây:

```
[ CẤU HÌNH ANIMATOR CULLING MODE CHO NHÂN VẬT & QUÁI ]
- Nhân vật Người Chơi (Local Player):
  Culling Mode = Always Animate (Để đảm bảo âm thanh bước chân và sự kiện không bị trễ)

- Quái Vật Sói Bậc 1 & NPC Dân Làng (Số lượng lớn):
  Culling Mode = CullUpdateTransforms (HOẶC BasedOnRenderers)
  * KHI QUÁI CHẠY NGOÀI MÀN HÌNH:
    - Ngừng cập nhật vị trí xương 3D (Tiết kiệm CPU tuyệt đối).
    - Logic FSM di chuyển (NavMeshAgent) và trừ máu vẫn hoạt động bình thường!
```

---

## 8. NGÂN SÁCH BỘ NHỚ VRAM VÀ GIỚI HẠN LỆNH VẼ (DRAW CALL BUDGETS)

Dựa trên mục tiêu tiếp cận máy tính phổ thông (GTX 1650+ / RTX 4060):

### 8.1. Ngân sách Lệnh Vẽ (Draw Call Budget) — [PROPOSED & BENCHMARK NEEDED]
- **Tổng số Batches toàn bộ khung hình gameplay:** Không vượt quá **$500 - 800\text{ Batches}$**.
- **Ngân sách dành riêng cho Nhân Vật & Quái Vật:** Tối đa **$150 - 200\text{ Batches}$**.
- **Quy tắc mỗi nhân vật:**
  - 01 Nhân vật người chơi hoàn chỉnh: Tối đa **$2 - 3\text{ Batches}$** (Thân/Giáp + Vũ khí).
  - 01 Con Sói Bậc 1: Tối đa **$1\text{ Batch}$**.

### 8.2. Ngân sách Bộ Nhớ Texture (VRAM Budget) — [PROPOSED]
- Toàn bộ Texture nạp vào VRAM cho hệ thống nhân vật trong một Scene không vượt quá **$256\text{ MB}$**:
  - Master Texture Nhân Tộc ($2048 \times 2048$ nén BC7): $\approx 21.3\text{ MB}$
  - Vũ khí & Công cụ ($1024 \times 1024$ nén BC7): $\approx 5.3\text{ MB}$
  - Sói & Quái vật Biome 1: $\approx 32\text{ MB}$

---

## 9. BẢNG PHÂN ĐỊNH TRẠNG THÁI CÁC CAM KẾT HIỆU NĂNG

| Thông Số Hiệu Năng | Giá Trị Đề Xuất | Trạng Thái Phê Duyệt | Điều Kiện Nghiệm Thu Kỹ Thuật |
| :--- | :--- | :--- | :--- |
| **Đa giác Nhân Vật (LOD0)** | $8,000 - 12,000$ Tris | `[PROPOSED]` | Kiểm tra bằng Blender Statistics & Unity Mesh Inspector |
| **Đa giác Sói Bậc 1 (LOD0)** | $3,000 - 4,500$ Tris | `[PROPOSED]` | Kiểm tra trong Scene Test_CombatArena |
| **Tổng Batches Nhân Vật** | $\le 200\text{ Batches}$ | `[PENDING BENCHMARK]`| Đo bằng Unity Profiler & Frame Debugger |
| **Bộ nhớ Texture Nhân Vật** | $\le 256\text{ MB}$ | `[PROPOSED]` | Đo bằng Unity Memory Profiler |
| **Thời gian tính Frame (p95)**| $\le 16.7\text{ ms}$ (Máy mục tiêu) | `[PENDING BENCHMARK]`| Cần đo trên Fixture 6 Player + 20 Sói theo MVP_SCOPE §4 |
| **Thời gian tính Frame (p95)**| $\le 33.3\text{ ms}$ (Client Low) | `[PENDING BENCHMARK]`| Cần mượn máy yếu nhất đội để đo thực tế |

---

## 10. QUY TRÌNH KIỂM TRA HIỆU NĂNG TỰ ĐỘNG (AUTOMATED PERFORMANCE VERIFICATION)

Trước khi đóng gói bản build thử nghiệm cho cột mốc M8 hoặc M12:
1. **Kiểm tra Frame Debugger:**
   - Mở Unity Frame Debugger $\to$ Chụp 1 frame giao tranh đông nhất.
   - Xác nhận SRP Batcher hoạt động: Hàng loạt nhân vật người chơi phải được gộp vào chung một nhóm vẽ (*"SRP Batch: 25 Draw Calls reduced to 1"*).
2. **Kiểm tra rò rỉ bộ nhớ (Memory Leak Check):**
   - Chạy phiên chơi liên tục 60 phút.
   - Xác nhận bộ nhớ SkinnedMeshRenderer và AnimationClip giải phóng đúng cách khi quái vật bị tiêu diệt và hồi sinh ([SESSION_AND_PERSISTENCE.md](../../architecture/SESSION_AND_PERSISTENCE.md)).

