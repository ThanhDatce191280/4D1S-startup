# QUY CHUẨN KỸ THUẬT MÔ HÌNH NHÂN VẬT 3D
## (CHARACTER MODELING SPECIFICATION — BLENDER & UNITY)

---

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mã tài liệu:** `CAB-SPEC-02`  
**Phiên bản:** `1.0.0`  
**Ngày cập nhật:** 31/10/2026  
**Trạng thái tài liệu:** `PROPOSED` (Chờ benchmark hiệu năng thực tế trên thiết bị mục tiêu)  
**Tài liệu cha:** [CHARACTER_ART_BIBLE.md](../CHARACTER_ART_BIBLE.md)  
**Tác giả:** Senior 3D Character Artist & Technical Artist  

---

## MỤC LỤC
1. [Môi Trường Phát Triển và Phiên Bản Công Cụ](#1-môi-trường-phát-triển-và-phiên-bản-công-cụ)
2. [Thiết Lập Không Gian Làm Việc Blender (Scene Setup & Units)](#2-thiết-lập-không-gian-làm-việc-blender-scene-setup--units)
3. [Quy Chuẩn Kích Thước và Chiều Cao Nhân Vật](#3-quy-chuẩn-kích-thước-và-chiều-cao-nhân-vật)
4. [Cấu Trúc Lưới Cơ Sở (Base Mesh & Topology Guidelines)](#4-cấu-trúc-lưới-cơ-sở-base-mesh--topology-guidelines)
5. [Ngân Sách Đa Giác (Polygon Budget Architecture)](#5-ngân-sách-đa-giác-polygon-budget-architecture)
6. [Tách Phân Mảnh Lưới và Mô Hình Modular (Mesh Separation)](#6-tách-phân-mảnh-lưới-và-mô-hình-modular-mesh-separation)
7. [Quy Chuẩn Trải UV (UV Unwrapping & Texel Density)](#7-quy-chuẩn-trải-uv-uv-unwrapping--texel-density)
8. [Quy Tắc Đặt Tên Tài Nguyên Chuẩn Hóa (Naming Conventions)](#8-quy-tắc-đặt-tên-tài-nguyên-chuẩn-hóa-naming-conventions)
9. [Gốc Tọa Độ, Trục Xoay và Transform (Origin, Pivot & Transforms)](#9-gốc-tọa-độ-trục-xoay-và-transform-origin-pivot--transforms)
10. [Quy Trình và Thiết Lập Xuất File FBX Từ Blender](#10-quy-trình-và-thiết-lập-xuất-file-fbx-từ-blender)
11. [Xác Thực Tích Hợp Trong Unity (Unity Import Validation)](#11-xác-thực-tích-hợp-trong-unity-unity-import-validation)
12. [Đặc Tả Kỹ Thuật Cho Nhân Vật Tham Chiếu "Bạch Lang Kiếm Sĩ"](#12-đặc-tả-kỹ-thuật-cho-nhân-vật-tham-chiếu-bạch-lang-kiếm-sĩ)

---

## 1. MÔI TRƯỜNG PHÁT TRIỂN VÀ PHIÊN BẢN CÔNG CỤ

- **Phần mềm 3D DCC chính:** **Blender 4.2 LTS** (hoặc tối thiểu Blender 4.0+).  
  *Lý do:* Hệ thống Node Shader và Modifier ổn định, khả năng xuất FBX giữ trọn vẹn vertex groups và custom normals sang Unity 6.
- **Engine tích hợp mục tiêu:** **Unity 6 LTS (`6000.3.27f1`)** sử dụng Universal Render Pipeline (URP) theo [ADR-01](../../architecture/TECHNICAL_DECISIONS.md).
- **Quy chuẩn tỷ lệ kỹ thuật bắt buộc:**
  $$\mathbf{1.0\text{ Blender Unit} = 1.0\text{ Meter} = 1.0\text{ Unity Unit}}$$

---

## 2. THIẾT LẬP KHÔNG GIAN LÀM VIỆC BLENDER (SCENE SETUP & UNITS)

Mọi file `.blend` sản xuất nhân vật bắt buộc phải tuân thủ cấu hình Scene Properties:
- **Unit System:** `Metric`
- **Unit Scale:** `1.0`
- **Length Unit:** `Meters`
- **Angle Unit:** `Degrees`
- **Hệ trục tọa độ làm việc mặc định trong Blender:**
  - Trục $+X$: Hướng sang bên Trái của nhân vật (Right-hand rule).
  - Trục $+Y$: Hướng ra phía Sau lưng nhân vật (Mặt nhân vật nhìn về hướng $-Y$ Forward).
  - Trục $+Z$: Hướng Thẳng Đứng lên trời (Up).
- **Clip Start / End của 3D Viewport:**
  - `Clip Start: 0.01m (1cm)`
  - `Clip End: 100m`

---

## 3. QUY CHUẨN KÍCH THƯỚC VÀ CHIỀU CAO NHÂN VẬT

Để bảo đảm tính tương thích với hệ thống vật lý `CharacterController` (`Height: 1.8m, Radius: 0.4m`) và hệ thống NavMesh Agent trong Unity ([M2_PLAN.md](../../milestones/M2/M2_PLAN.md)):

| Nhóm Nhân Vật | Chiều Cao Đỉnh Đầu (Không tính nón/sừng) | Chiều Rộng Bờ Vai | Bán Kính Va Chạm Đề Xuất | Trọng Tâm Y (Hips/Pelvis) |
| :--- | :--- | :--- | :--- | :--- |
| **Nhân Tộc Nam (Player Base)** | $1.80\text{m} \pm 0.02\text{m}$ | $0.55\text{m} - 0.60\text{m}$ | $0.40\text{m}$ | $0.98\text{m}$ |
| **Nhân Tộc Nữ** | $1.68\text{m} \pm 0.02\text{m}$ | $0.45\text{m} - 0.50\text{m}$ | $0.35\text{m}$ | $0.92\text{m}$ |
| **Dân Làng / NPC Già Làng** | $1.72\text{m} \pm 0.03\text{m}$ | $0.50\text{m}$ | $0.40\text{m}$ | $0.94\text{m}$ |
| **Tộc Người Lùn (Dwarf)** | $1.25\text{m} \pm 0.03\text{m}$ | $0.65\text{m} - 0.70\text{m}$ | $0.45\text{m}$ | $0.65\text{m}$ |
| **Phù Thủy (Witch)** | $1.70\text{m} \pm 0.02\text{m}$ | $0.48\text{m}$ | $0.38\text{m}$ | $0.93\text{m}$ |
| **Sói Bậc 1 (Quái 4 chân)** | $1.25\text{m}$ (đỉnh đầu) / $0.90\text{m}$ (vai) | $0.45\text{m}$ | $0.50\text{m}$ | $0.75\text{m}$ |
| **Người Sói Dạng Quái Thú (T3-T5)**| $2.40\text{m} - 2.60\text{m}$ | $1.10\text{m} - 1.25\text{m}$ | $0.70\text{m}$ | $1.35\text{m}$ |
| **Sói Vua Tier 9 (Boss)** | $3.20\text{m} - 3.50\text{m}$ | $1.60\text{m} - 1.80\text{m}$ | $1.20\text{m}$ | $1.80\text{m}$ |

---

## 4. CẤU TRÚC LƯỚI CƠ SỞ (BASE MESH & TOPOLOGY GUIDELINES)

```
        VÒNG LẶP CẠNH (EDGE LOOPS) KHỚP UỐN
                    (O)  -> Cổ: 3 vòng edge loop
                   / | \
           (===)  /  |  \  (===)
      Khớp Vai:  |   |   |  -> Khớp Vai: 4 vòng tròn hội tụ
      Vát chéo   |  ( )  |
                 [=======] -> Eo: 3 vòng hỗ trợ xoay vặn
                 |  / \  |
      Khuỷu tay: | || || | -> Đầu gối: 3 vòng kép mặt trước
      3 vòng lặp | || || |    1 vòng khép mặt sau chống móp
                 (_)(_)(_)
```

### 4.1. Quy tắc Topology cốt lõi
1. **Ưu tiên Quad Mesh ($>90\%$):** Toàn bộ vùng biến dạng chuyển động phải sử dụng lưới 4 cạnh (Quad). Lưới tam giác (Tris) chỉ được phép xuất hiện ở các bề mặt phẳng cứng không co dãn (đế giày, mặt khiên, lưỡi kiếm, đỉnh mũ).
2. **Tuyệt đối cấm N-gons ($>4$ cạnh):** N-gons sẽ gây lỗi tính toán phân mảnh không đồng nhất khi Unity tự động triangulate mesh lúc import.
3. **Phân bố Edge Loops tại các khớp cử động:**
   - **Khuỷu tay và Đầu gối:** Cấu trúc **"3-Edge Loop"** (1 loop trung tâm nằm đúng trục xoay của xương, 2 loop cận kề cách đều $2 - 3\text{cm}$) để khi uốn $90^\circ - 120^\circ$ không bị co rút thể tích (*No Volume Loss*).
   - **Bờ vai và Khớp háng:** Sử dụng kỹ thuật nối topology hình chữ V hoặc vòng khép kín (*Topology Ring Flow*) để tránh kéo giãn lưới ngực khi giơ tay lên cao.
   - **Vùng Eo và Cổ:** Tối thiểu 3 vòng edge loops cách đều để phục vụ vặn người chém kiếm và nghiêng đầu.

### 4.2. Tư thế chuẩn bị dựng hình (Modeling Pose)
- **A-Pose (Tư thế A - BẮT BUỘC cho Humanoid):**
  - Hai tay hạ thấp một góc $45^\circ$ so với thân mình.
  - Lòng bàn tay úp nghiêng góc $45^\circ$ về phía trước, các ngón tay hơi cong tự nhiên.
  - Hai chân mở rộng ngang vai, cách nhau khoảng $0.25 - 0.30\text{m}$.
  *Lý do:* Giảm thiểu biến dạng kéo giãn cơ vai và cơ ngực so với tư thế T-Pose truyền thống, giúp việc vẽ trọng lượng xương (*Skin Weighting*) chính xác hơn $40\%$.

---

## 5. NGÂN SÁCH ĐA GIÁC (POLYGON BUDGET ARCHITECTURE)

Các chỉ số dưới đây được phân loại chính thức theo thẩm quyền tài liệu:
- `[CONFIRMED]`: Đã duyệt trong Technical Decisions.
- `[PROPOSED]`: Đề xuất kỹ thuật chuyên gia mỹ thuật.
- `[PENDING BENCHMARK]`: Cần kiểm chứng FPS thực tế trên máy thử nghiệm yếu nhất (Client Low Profile theo [MVP_SCOPE.md](../../vision/MVP_SCOPE.md)).

| Đối Tượng Tài Nguyên | Ngân Sách LOD0 (Triangles) | Ngân Sách LOD1 (Triangles) | Ngân Sách LOD2 (Triangles) | Trạng Thái Phê Duyệt | Ghi Chú Kỹ Thuật |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Nhân Vật Người Chơi (Full Set)**| $8,000 - 12,000$ | $4,000 - 6,000$ | $1,500 - 2,500$ | `[PROPOSED]` | Bao gồm cả giáp, tóc, nón, không tính vũ khí |
| **Lưới Cơ Thể Trần (Body Base Mesh)**| $3,500 - 4,500$ | $1,800 - 2,200$ | $800 - 1,000$ | `[PROPOSED]` | Làm phôi chuẩn để đắp trang bị |
| **Vũ Khí 1 Tay (Trường Kiếm, Rìu)**| $800 - 1,500$ | $400 - 600$ | $150 - 250$ | `[PROPOSED]` | Đủ sắc cạnh ở lưỡi và cán |
| **Vũ Khí 2 Tay / Cung Tre** | $1,200 - 2,000$ | $600 - 900$ | $250 - 400$ | `[PROPOSED]` | Bao gồm cánh cung và dây cung |
| **Khiên Đỡ (Đồng / Da)** | $500 - 800$ | $250 - 400$ | $100 - 150$ | `[PROPOSED]` | Mặt khiên cong nhẹ |
| **Sói Bậc 1 (Feral Wolf - M8 Core)**| $3,000 - 4,500$ | $1,500 - 2,200$ | $600 - 1,000$ | `[PROPOSED]` | Quái xuất hiện số lượng lớn (20 con) |
| **Lợn Rừng Trường Sơn (M8 Core)** | $2,500 - 3,500$ | $1,200 - 1,800$ | $500 - 800$ | `[PROPOSED]` | Dã thú săn bắt cơ bản |
| **Dân Làng / NPC Thường** | $4,000 - 6,000$ | $2,000 - 3,000$ | $800 - 1,200$ | `[PROPOSED]` | Tối đa 10 NPC đồng thời |
| **Người Sói Chiến Binh (Tier 5)** | $7,000 - 9,000$ | $3,500 - 4,500$ | $1,500 - 2,000$ | `[PROPOSED]` | Có giáp và cơ bắp lớn |
| **Sói Vua Tier 9 (Boss Duy Nhất)** | $15,000 - 20,000$| $8,000 - 10,000$ | $3,500 - 5,000$ | `[PENDING BENCHMARK]` | Đỉnh cao chi tiết, hiệu ứng sừng đá |

---

## 6. TÁCH PHÂN MẢNH LƯỚI VÀ MÔ HÌNH MODULAR (MESH SEPARATION)

Để phục vụ hệ thống trang bị Modular trong [CHARACTER_EQUIPMENT_SPEC.md](CHARACTER_EQUIPMENT_SPEC.md), nhân vật người chơi không được gộp thành 1 khối duy nhất (*Single Mesh*) mà phải tách thành các Sub-Object logic trong Blender:

```
+-- CH_Human_Male_ModularRoot (Armature)
    +-- Mesh_Head (Đầu, mắt, miệng, cổ)
    +-- Mesh_Hair (Tóc cơ sở)
    +-- Mesh_Torso_Base (Thân ngực trần - Có thể tắt khi mặc giáp)
    +-- Mesh_Hands_Base (Bàn tay trần)
    +-- Mesh_Legs_Base (Chân trần / Quần lót)
    +-- Mesh_Feet_Base (Bàn chân trần)
    +-- Armor_Chest_SilverWolf_LOD0 (Giáp ngực thay thế)
    +-- Armor_Gloves_SilverWolf_LOD0 (Găng tay thay thế)
    +-- Armor_Boots_SilverWolf_LOD0 (Ủng thay thế)
    +-- Prop_Cape_SilverWolf_LOD0 (Áo choàng lông)
```

> [!IMPORTANT]
> **Quy tắc đường biên mép cắt (Seam Boundary Alignment):**  
> Vòng đỉnh (Vertex Ring) tại ranh giới giữa Cổ $\leftrightarrow$ Ngực, Cổ tay $\leftrightarrow$ Bàn tay, Cổ chân $\leftrightarrow$ Bàn chân phải có **tọa độ đỉnh ($X, Y, Z$) và số lượng đỉnh giống hệt nhau $100\%$**. Khi tráo đổi trang bị, không được xuất hiện khe hở nhìn xuyên thấu da thịt nhân vật.

---

## 7. QUY CHUẨN TRẢI UV (UV UNWRAPPING & TEXEL DENSITY)

1. **Texel Density chuẩn hóa:**
   $$\mathbf{\text{Texel Density} = 10.24\text{ pixels / cm}\quad (1024\text{px cho } 1.0\text{m thực tế})}$$
   - Được phép tăng lên $15.36\text{ px/cm}$ cho vùng Khuôn mặt và Chi tiết Hộ tâm kính ngực.
   - Được phép giảm xuống $5.12\text{ px/cm}$ cho vùng Lòng bàn chân, mặt trong đùi hoặc gầm nách.
2. **Quy tắc đặt đường cắt UV Seam:**
   - Giấu đường cắt UV ở các vị trí khuất: Mặt trong cánh tay, mặt trong đùi, đường may dọc lưng áo, gáy tóc.
   - Tránh tuyệt đối đặt Seam chạy ngang qua giữa mặt, giữa ngực hoặc trên sống mũi kiếm.
3. **Đối xứng và Bất đối xứng (Symmetry vs Asymmetry):**
   - Các bộ phận đối xứng thuần túy (tay áo thường, ống quần, hai bên bắp chân) được phép chồng UV (Overlapping UV) để tiết kiệm $40\%$ diện tích texture sheet.
   - Các bộ phận mang hoa văn chữ viết, mặt trời Đông Sơn, sẹo mặt, hoặc giáp vai phi đối xứng **bắt buộc phải có đảo UV riêng biệt (Unique UV Islands)**.
4. **Khoảng cách đệm giữa các đảo UV (UV Padding):**
   - Texture $1024 \times 1024$: Tối thiểu **$8\text{ pixels}$** padding.
   - Texture $2048 \times 2048$: Tối thiểu **$16\text{ pixels}$** padding.  
   *Mục đích:* Ngăn ngừa hiện tượng lem màu xuyên đảo UV khi hệ thống Mipmap hoạt động ở góc máy xa.

---

## 8. QUY TẮC ĐẶT TÊN TÀI NGUYÊN CHUẨN HÓA (NAMING CONVENTIONS)

Tuân thủ nghiêm ngặt bảng danh pháp [GLOSSARY.md](../../architecture/GLOSSARY.md) và cấu trúc thư mục Unity:

### 8.1. Cú pháp định danh Mesh & Model
$$\mathbf{[Category]\_[Species]\_[Archetype/Name]\_[Slot/Part]\_[LOD]}$$

- **`CH_`**: Ký hiệu Nhân vật (Character).
- **`EQ_`**: Ký hiệu Trang bị tách rời (Equipment).
- **`WP_`**: Ký hiệu Vũ khí (Weapon).
- **`PR_`**: Ký hiệu Phụ kiện/Vật phẩm trang trí (Prop).

*Ví dụ chuẩn xác:*
- `CH_Human_Male_Base_Body_LOD0.fbx`
- `EQ_Armor_Chest_DongSonBronze_T2_LOD0.fbx`
- `WP_Sword_Iron_T1_LOD0.fbx` (Khớp `weapon_sword_iron_t1` trong code)
- `CH_Werewolf_Tier1_Feral_LOD0.fbx` (Khớp `WerewolfTier.Tier1_Feral`)
- `CH_Werewolf_Tier9_WolfKing_LOD0.fbx` (Khớp `Boss_WolfKing`)

---

## 9. GỐC TỌA ĐỘ, TRỤC XOAY VÀ TRANSFORM (ORIGIN, PIVOT & TRANSFORMS)

Để loại bỏ hoàn toàn các lỗi runtime khi nhân vật xoay mặt độc lập trong `PlayerVisual` ([M2_PLAN.md](../../milestones/M2/M2_PLAN.md)):

```
       MẶT BẰNG ĐẶT PIVOT (GỐC TỌA ĐỘ)
                   ( o )
                   / | \
                  [=====]
                   |   |
                  /     \
             [X]-------[X]   <- Hai gót chân chạm sàn
                   * (0, 0, 0) -> Pivot Point chính giữa
```

1. **Tọa độ Pivot Point:**
   - **Nhân vật:** Nằm tại điểm chính giữa hai gót chân tiếp xúc mặt sàn: $(X=0, Y=0, Z=0)$.
   - **Vũ khí cầm tay (Kiếm, Rìu):** Nằm tại tâm điểm nắm của bàn tay trên chuôi kiếm (Grip Center).
   - **Khiên:** Nằm tại vị trí chốt xỏ cẳng tay trái.
2. **Trạng thái Transform bắt buộc trước khi lưu file:**
   - **Position:** $(0.0, 0.0, 0.0)$
   - **Rotation:** $(0.0^\circ, 0.0^\circ, 0.0^\circ)$
   - **Scale:** $(1.0, 1.0, 1.0)$
   - *Thao tác thực hiện trong Blender:* Chọn toàn bộ đối tượng $\to$ Nhấn `Ctrl + A` $\to$ Chọn **All Transforms**.

---

## 10. QUY TRÌNH VÀ THIẾT LẬP XUẤT FILE FBX TỪ BLENDER

Khi xuất file từ Blender sang Unity (`File -> Export -> FBX (.fbx)`), bắt buộc phải lưu Preset mang tên: **`Unity_Character_Export`** với các thông số sau:

### 10.1. Tab Main & Include
- **Selected Objects:** `Bật (Checked)` (Chỉ xuất các mesh và armature được chọn).
- **Object Types:** Chọn `Armature` và `Mesh` (Tắt Camera, Light, Empty).

### 10.2. Tab Transform
- **Scale:** `1.0`
- **Apply Scalings:** `FBX All`
- **Forward:** `-Z Forward`
- **Up:** `Y Up`
- **Apply Unit:** `Bật (Checked)`
- **Use Space Transform:** `Bật (Checked)`

### 10.3. Tab Geometry
- **Apply Modifiers:** `Bật (Checked)` (Tự động nướng modifier trước khi xuất).
- **Smoothing:** `Face` hoặc `Normals Only`
- **Tangent Space:** `Bật (Checked)` (Để nướng Normal Map chuẩn xác trong URP).

### 10.4. Tab Armature
- **Primary Bone Axis:** `Y Axis`
- **Secondary Bone Axis:** `X Axis`
- **Armature FBXNode Type:** `Null`
- **Add Leaf Bones:** `TẮT (UNCHECKED - CỰC KỲ QUAN TRỌNG)`  
  *Lý do:* Nếu bật Leaf Bones, Blender sẽ tự sinh ra hàng loạt xương thừa ở ngón tay và đỉnh đầu, làm hỏng cấu hình Unity Mecanim Humanoid Avatar!

### 10.5. Tab Animation (Nếu xuất kèm hoạt cảnh)
- **Bake Animation:** Bật nếu xuất clip hoạt cảnh. Tắt nếu chỉ xuất Mesh gốc T-pose.
- **Key All Bones:** `Bật`
- **Simplify:** `0.0` (Không tự ý làm tròn mất khung hình của Animator).

---

## 11. XÁC THỰC TÍCH HỢP TRONG UNITY (UNITY IMPORT VALIDATION)

Khi file FBX được đưa vào thư mục `Assets/_Project/Art/Characters/`, người tích hợp phải kiểm tra cấu hình Inspector của Model:

```
[ Model Tab ]
- Scale Factor: 1.0
- Convert Units: Checked
- Import BlendShapes: Checked (nếu có biểu cảm)
- Read/Write: Unchecked (trừ khi cần tạo mesh nén động)
- Mesh Compression: Off (hoặc Low ở bản release)
- Generate Colliders: UNCHECKED (Collider do Logic Root đảm nhiệm)

[ Rig Tab ]
- Animation Type: Humanoid (đối với nhân tộc) / Generic (đối với Sói/Thú)
- Avatar Definition: Create From This Model (cho Base Mesh) / Copy From Other Avatar (cho trang bị/skin)
- Skin Weights: Standard (4 Bones)

[ Materials Tab ]
- Material Creation Mode: None (Sẽ gán thủ công Material URP Lit chuẩn của dự án)
```

---

## 12. ĐẶC TẢ KỸ THUẬT CHO NHÂN VẬT THAM CHIẾU "BẠCH LANG KIẾM SĨ"

```
+-------------------------------------------------------------------------+
|     THÔNG SỐ KỸ THUẬT: CH_Human_Hero_SilverWolfKnight_LOD0.fbx          |
+-------------------------------------------------------------------------+
| 1. Tổng số đa giác (Polycount): 11,240 Triangles (Full Setup)           |
|    - Base Head + Silver Hair & Beard: 2,650 Tris                        |
|    - Base Body + Pants: 2,800 Tris                                      |
|    - Silver Pauldron & Chest Armor: 2,450 Tris                          |
|    - Wolf Fur Cape (Áo choàng lông sói): 1,420 Tris                     |
|    - Boots & Vambraces: 920 Tris                                        |
|    - Sacred Silver Broadsword (Trường kiếm thần sáng): 1,000 Tris       |
|                                                                         |
| 2. Kích thước đo đạc (Bounding Dimensions):                            |
|    - Chiều cao (Đế giày đến đỉnh tóc): Chính xác 1.83m                  |
|    - Độ rộng hai vai (Pauldron span): 0.62m                             |
|    - Chiều dài trường kiếm: 1.15m (Lưỡi kiếm 0.85m, Chuôi 0.30m)        |
|                                                                         |
| 3. Cấu hình UV & Material:                                              |
|    - 01 Master Texture Sheet 2048x2048 (Body & Armor)                   |
|    - 01 Weapon Texture Sheet 1024x1024 (Thanh kiếm & Hào quang)         |
|    - Texel Density trung bình: 10.45 px/cm                              |
+-------------------------------------------------------------------------+
```

