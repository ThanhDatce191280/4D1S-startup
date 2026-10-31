# QUY CHUẨN HỆ THỐNG TRANG BỊ MODULAR
## (CHARACTER EQUIPMENT SPECIFICATION — MODULAR & SOCKET ATTACHMENTS)

---

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mã tài liệu:** `CAB-SPEC-04`  
**Phiên bản:** `1.0.0`  
**Ngày cập nhật:** 31/10/2026  
**Trạng thái tài liệu:** `PROPOSED` (Đã đồng bộ với cấu trúc `ItemDataSO` và `WeaponDataSO`)  
**Tài liệu cha:** [CHARACTER_ART_BIBLE.md](../CHARACTER_ART_BIBLE.md)  
**Tác giả:** Technical Artist & Senior 3D Character Artist  

---

## MỤC LỤC
1. [Triết Lý Kiến Trúc Trang Bị Đổi Mới](#1-triết-lý-kiến-trúc-trang-bị-đổi-mới)
2. [Cấu Trúc Nhân Vật Cơ Sở (Base Character Architecture)](#2-cấu-trúc-nhân-vật-cơ-sở-base-character-architecture)
3. [Danh Sách Khe Cắm Trang Bị (Equipment Slots)](#3-danh-sách-khe-cắm-trang-bị-equipment-slots)
4. [Hai Phương Thức Lắp Ghép Trang Bị (Skinned Mesh vs Socket Attachment)](#4-hai-phương-thức-lắp-ghép-trang-bị-skinned-mesh-vs-socket-attachment)
5. [Quy Tắc Xử Lý Lớp Phủ và Chống Xuyên Thấu (Clipping Prevention)](#5-quy-tắc-xử-lý-lớp-phủ-và-chống-xuyên-thấu-clipping-prevention)
6. [Tương Tác Giữa Mũ Nón, Mái Tóc và Khuôn Mặt](#6-tương-tác-giữa-mũ-nón-mái-tóc-và-khuôn-mặt)
7. [Điểm Gắn Binh Khí và Tay Cầm (Weapon Sockets & Grips)](#7-điểm-gắn-binh-khí-và-tay-cầm-weapon-sockets--grips)
8. [Tích Hợp Dữ Liệu Với Mã Nguồn C# (`ItemDataSO` & `WeaponDataSO`)](#8-tích-hợp-dữ-liệu-với-mã-nguồn-c-itemdataso--weapondataso)
9. [Cấu Trúc Unity Equipment Prefab](#9-cấu-trúc-unity-equipment-prefab)
10. [Quy Chuẩn Riêng Cho Lốt Người Sói (Werewolf Form Equipment Rules)](#10-quy-chuẩn-riêng-cho-lốt-người-sói-werewolf-form-equipment-rules)
11. [Phân Cấp Chế Tác: Khả Năng Tái Sử Dụng Giữa Các Giới Tính / Nghề](#11-phân-cấp-chế-tác-khả-năng-tái-sử-dụng-giữa-các-giới-tính--nghề)
12. [Đặc Tả Phân Rã Trang Bị Của "Bạch Lang Kiếm Sĩ" (Silver Wolf Knight Breakdown)](#12-đặc-tả-phân-rã-trang-bị-của-bạch-lang-kiếm-sĩ-silver-wolf-knight-breakdown)

---

## 1. TRIẾT LÝ KIẾN TRÚC TRANG BỊ ĐỔI MỚI

Hệ thống trang bị trong **Huyết Nguyệt Đại Ngàn** được xây dựng dựa trên nguyên lý cốt lõi của *Albion Online*: **"Kỹ năng và bản sắc chiến đấu do trang bị trên người quyết định"** ([ADR-06](../../architecture/TECHNICAL_DECISIONS.md)).

Mục tiêu kỹ thuật mỹ thuật:
- **Thay đổi ngoại hình tức thời:** Người chơi nhặt giáp, chế tạo hoặc đổi vũ khí thì mô hình 3D trên màn hình thay đổi ngay lập tức.
- **Không dựng lại toàn bộ nhân vật:** Mỗi bộ giáp chỉ là một tập hợp các mảnh lưới (*Submeshes*) độc lập được gắn vào bộ khung xương có sẵn.
- **Không phát sinh rác bộ nhớ runtime:** Việc hoán đổi trang bị chỉ bật/tắt Renderer hoặc tráo `Mesh` trong `SkinnedMeshRenderer`, không hủy (`Destroy`) hay sinh mới (`Instantiate`) GameObject liên tục trong trận đánh.

---

## 2. CẤU TRÚC NHÂN VẬT CƠ SỞ (BASE CHARACTER ARCHITECTURE)

Nhân vật cơ sở (Mannequin Base Mesh) bao gồm một bộ khung xương chuẩn Humanoid và các mảng thịt cơ thể được phân vùng:

```
        CƠ THỂ GỐC ĐƯỢC CHIA VÙNG (SLICED BASE MESH)
                   [ Đầu & Cổ ]       -> Luôn hiển thị
                      /     \
             [ Ngực, Bụng & Lưng ]   -> Ẩn khi mặc Giáp Thân Kín
                 /     |     \
     [ Cánh Tay ] [Đai Lưng] [ Cánh Tay ] -> Ẩn khi mặc Giáp Ống Tay
          |                       |
     [ Bàn Tay ]             [ Bàn Tay ]  -> Ẩn khi đeo Găng Tay
                   [ Đùi & Háng ]         -> Ẩn khi mặc Quần Giáp
                   [ Cẳng Chân ]          -> Ẩn khi đi Ủng Cao
                   [ Bàn Chân ]           -> Ẩn khi đi Giày
```

### Lợi ích của kỹ thuật Phân Vùng Lưới (Mesh Slicing):
Khi nhân vật trang bị một chiếc áo giáp sắt kín mít, phần thân ngực trần bên trong được gỡ bỏ khỏi lệnh vẽ GPU. Điều này giúp:
- **Tiêu diệt $100\%$ hiện tượng đâm xuyên lưới (Zero Mesh Clipping)** khi nhân vật chạy hoặc chém kiếm.
- **Tiết kiệm hàng ngàn đa giác rác** đang bị che khuất dưới lớp giáp dày.

---

## 3. DANH SÁCH KHE CẮM TRANG BỊ (EQUIPMENT SLOTS)

Hệ thống hỗ trợ chính thức **9 khe cắm trang bị hiển thị ngoại hình** (khớp với túi đồ 20 ô trong [M5_PLAN.md](../../milestones/M5/M5_PLAN.md)):

| Mã Khe Cắm (Slot ID) | Tên Tiếng Việt | Phương Thức Lắp Ghép | Đối Tượng Đại Diện Trong Code |
| :--- | :--- | :--- | :--- |
| `Slot_Head` | Mũ nón / Khăn bịt mặt | Socket Attachment | Nón lá, Mũ da sói, Băng trán đồng |
| `Slot_Chest` | Giáp Thân / Áo tơi lá | Skinned Mesh Replacement | Giáp vảy đồng Đông Sơn, Áo tơi lá cọ |
| `Slot_Shoulder_L` | Giáp vai trái | Socket Attachment | Phiến bạc chạm vảy rồng |
| `Slot_Shoulder_R` | Giáp vai phải | Socket Attachment | Nẹp da thú sấy khói |
| `Slot_Gloves` | Găng tay / Hộ cổ tay | Skinned Mesh Replacement | Găng da hổ, Vòng hộ uyển đồng thau |
| `Slot_Legs` | Quần giáp / Váy thổ cẩm | Skinned Mesh Replacement | Quần thô nhuộm củ nâu, Xà cạp chiến |
| `Slot_Boots` | Ủng chân / Giày da rừng | Skinned Mesh Replacement | Ủng da sói tuyết, Giày cỏ đan dây gai |
| `Slot_Cape` | Áo choàng lưng / Khăn | Skinned Mesh / Sockets | Áo choàng lông sói tuyết viền chỉ đỏ |
| `Slot_MainHand` | Vũ khí tay phải | Socket Attachment | Trường kiếm sắt, Rìu đá, Cung tre |
| `Slot_OffHand` | Vũ khí phụ / Đuốc / Khiên| Socket Attachment | Khiên đồng, Đuốc ngải cứu, Bùa phép |

---

## 4. HAI PHƯƠNG THỨC LẮP GHÉP TRANG BỊ (SKINNED MESH VS SOCKET ATTACHMENT)

```
+-------------------------------------------------------------------------+
|                  HAI CƠ CHẾ TÍCH HỢP TRANG BỊ TRÊN MODEL                |
+-------------------------------------------------------------------------+
| CƠ CHẾ 1: SKINNED MESH REPLACEMENT (Thay thế lưới gắn xương)             |
| - Áp dụng cho: Giáp Ngực, Găng Tay, Quần Giáp, Ủng Chân.                |
| - Cách thức hoạt động:                                                  |
|   1. Mảnh giáp 3D được gắn chung khung xương (Bones) với nhân vật.      |
|   2. Khi trang bị, SkinnedMeshRenderer của giáp nhận tham chiếu tới mảng|
|      xương gốc của PlayerVisual (Bones = MasterSkeleton.Bones).         |
|   3. Giáp sẽ uốn lượn, co giãn đồng bộ 100% theo từng chuyển động chân, |
|      tay của nhân vật.                                                  |
+-------------------------------------------------------------------------+
| CƠ CHẾ 2: SOCKET ATTACHMENT (Gắn cứng vào điểm xương định sẵn)          |
| - Áp dụng cho: Nón lá, Giáp vai cứng, Vũ khí, Khiên, Đuốc.              |
| - Cách thức hoạt động:                                                  |
|   1. Mảnh trang bị là một vật thể Mesh tĩnh (Static Mesh).              |
|   2. Được gắn làm con (Child Transform) của một điểm Socket xương cụ thể|
|      (ví dụ: Socket_Weapon_R trên xương RightHand).                     |
|   3. Transform Position và Rotation nội bộ được khóa cứng về (0, 0, 0).  |
|   4. Trang bị di chuyển và xoay hoàn toàn theo xương cha tương ứng.      |
+-------------------------------------------------------------------------+
```

---

## 5. QUY TẮC XỬ LÝ LỚP PHỦ VÀ CHỐNG XUYÊN THẤU (CLIPPING PREVENTION)

Hiện tượng xuyên giáp (*Clipping*) là lỗi thường gặp nhất trong game nhập vai. Dự án áp dụng bộ quy chuẩn phòng chống nghiêm ngặt:

1. **Khoảng cách đệm độ dày (Offset Clearance):**
   - Lớp quần áo da/vải trong cùng: Cách bề mặt da thịt $0.5\text{cm} - 1.0\text{cm}$.
   - Lớp giáp cứng (Đồng, Sắt, Đá): Cách lớp áo trong tối thiểu $1.5\text{cm} - 2.5\text{cm}$.
   - Lớp lông thú xù xì và áo choàng: Cách bề mặt giáp ngoài cùng $3.0\text{cm} - 4.0\text{cm}$.
2. **Cắt tỉa mặt trong của Giáp Vai (Hollow Pauldron Interiors):**
   - Toàn bộ các mặt đa giác phía trong của giáp vai úp vào cơ thể người phải được xóa bỏ (*Delete Hidden Polygons*). Vừa tiết kiệm lưới, vừa không bao giờ đâm thủng vào đầu nhân vật khi giơ tay chém.
3. **Cờ Ẩn Tự Động (Auto-Hide Visibility Flags):**
   - Mỗi file cấu hình `EquipmentVisualConfig` có mảng enum `BodyPartsToHide`:
     - Trang bị `Armor_Chest_DongSonBronze`: Bật cờ ẩn `Hide_Torso`, `Hide_UpperArms`.
     - Trang bị `Armor_Boots_WolfFur`: Bật cờ ẩn `Hide_Calves`, `Hide_Feet`.

---

## 6. TƯƠNG TÁC GIỮA MŨ NÓN, MÁI TÓC VÀ KHUÔN MẶT

Khi nhân vật đội nón lá hoặc mũ lông sói, mái tóc dài có nguy cơ đâm thủng vành nón. Giải pháp xử lý:

```
+-- Mesh_Hair (Tóc cơ sở) được chia làm 3 biến thể Mesh:
    1. Hair_Full (Tóc đầy đủ): Hiển thị khi KHÔNG đội nón.
    2. Hair_Compressed (Tóc nén ép sát đầu): Hiển thị khi đội Mũ da sói hoặc Băng trán.
    3. Hair_Hidden (Tóc ẩn hoàn toàn): Chỉ để lại búi tó gáy hoặc phần tóc mai khi đội Nón lá rộng vành.
```

- **Quy tắc gán cờ trong ScriptableObject:**
  Trường dữ liệu `HairHandlingMode`: `KeepOriginal` | `UseCompressedHair` | `HideHairCompletely`.

---

## 7. ĐIỂM GẮN BINH KHÍ VÀ TAY CẦM (WEAPON SOCKETS & GRIPS)

Toàn bộ vũ khí được căn chỉnh tâm điểm nắm chính xác tại $(0, 0, 0)$ của model vũ khí để khi snap vào Socket sẽ khớp khít hoàn hảo:

```
        VỊ TRÍ CÁC SOCKET TRÊN KHUNG XƯƠNG
                   ( o )
                [Head_Hat]
              /     |     \
     [Shoulder_L]   |   [Shoulder_R]
          |     [Back_Weapon] |
     [Shield_L]               |
                           [Weapon_R] -> Tay phải cầm Kiếm/Rìu
```

1. **`Socket_Weapon_R` (Cổ tay phải):**
   - Tọa độ gốc nằm giữa lòng bàn tay khum lại.
   - Trục $+Z$ hướng dọc theo lưỡi kiếm vươn về phía trước.
   - Trục $+Y$ hướng theo sống lưng kiếm.
2. **`Socket_Shield_L` (Cẳng tay trái):**
   - Nằm cách cẳng tay $3\text{cm}$ về phía ngoài. Trục $+Z$ vuông góc với mặt khiên hướng ra ngoài đỡ đòn.
3. **`Socket_Back_Weapon` (Sau lưng):**
   - Dùng khi nhân vật cất vũ khí vào bao hoặc đeo cung tre sau lưng lúc đang chặt cây, nấu nướng.

---

## 8. TÍCH HỢP DỮ LIỆU VỚI MÃ NGUỒN C# (`ItemDataSO` & `WeaponDataSO`)

Khớp nối trực tiếp với kiến trúc C# hiện hữu tại [M5_PLAN.md](../../milestones/M5/M5_PLAN.md) và `Assets/_Project/Scripts/Inventory/ItemDataSO.cs`:

### 8.1. Mở rộng dữ liệu Visual trong `ItemDataSO`
Để không làm đảo lộn mã nguồn logic đã viết, trường `visualPrefab` được tích hợp vào `ItemDataSO`:

```csharp
// Đề xuất trường tích hợp trong ItemDataSO.cs (Tương thích không phá vỡ API)
public enum EquipmentVisualType
{
    None,
    SkinnedMeshPart,
    SocketAttachment
}

[System.Serializable]
public struct EquipmentVisualData
{
    public EquipmentVisualType visualType;
    public string targetSocketName;       // VD: "Socket_Weapon_R", "Socket_Head"
    public GameObject visualPrefab;        // Prefab chứa MeshRenderer hoặc SkinnedMeshRenderer
    public BodyPartToHide bodyHideMask;   // Enum cờ đánh dấu phần thịt cần ẩn
}
```

### 8.2. Kết nối với `WeaponDataSO`
Vũ khí `Weapon_IronSword_T1.asset` ([M3_PLAN.md](../../milestones/M3/M3_PLAN.md)) mang tham chiếu trực tiếp tới Prefab hiển thị:
- `visualPrefab`: `Assets/_Project/Prefabs/Weapons/WP_Sword_Iron_T1.prefab`
- Khi người chơi gọi hàm `PlayerCombatController.EquipWeapon(newWeapon)`, hệ thống tự động sinh hoặc gắn prefab này vào `Socket_Weapon_R`.

---

## 9. CẤU TRÚC UNITY EQUIPMENT PREFAB

Tất cả trang bị được đóng gói thành Prefab chuẩn hóa nằm trong thư mục `Assets/_Project/Prefabs/Equipment/` hoặc `Prefabs/Weapons/`:

```
[ WP_Sword_Iron_T1.prefab ]
├── Model_Mesh (MeshFilter + MeshRenderer với Material M_Iron_T1)
├── Socket_Grip_Offset (Transform bù trừ góc cầm nếu có)
├── Trail_Renderer (Vệt sáng chém kiếm - chỉ bật khi chém)
└── Audio_Whoosh_Source (Âm thanh vung kiếm)

[ EQ_Armor_Chest_DongSonBronze.prefab ]
└── SkinnedMeshRenderer (Nhận mesh Armor_Chest_DongSonBronze_LOD0)
    ├── Material: M_DongSon_Bronze_T2
    └── RootBone: Trống (Sẽ được code runtime gán tự động sang Hips của PlayerVisual)
```

---

## 10. QUY CHUẨN RIÊNG CHO LỐT NGƯỜI SÓI (WEREWOLF FORM EQUIPMENT RULES)

> [!CAUTION]
> **Quy định bất biến:** Tuyệt đối **KHÔNG** ép hình dạng Sói (Werewolf Form) sử dụng toàn bộ hệ thống giáp vải, quần ủng hay mũ nón của Nhân Tộc!

1. **Khi biến hóa từ Người sang Sói (Transformation Event):**
   - Toàn bộ GameObject trang bị dạng người (`Equipment Slots`) được **Tắt hoàn toàn (`SetActive(false)`)**.
   - Model `PlayerVisual` chuyển sang sử dụng mô hình Quái thú Sói chuyên biệt (`CH_Werewolf_Beast_Form`).
2. **Trang bị độc quyền của Người Sói (Beast Form Adornments):**
   - Chỉ hỗ trợ các chi tiết đặc hữu của loài Sói:
     - Dây xích sắt gãy đứt găm sâu vào cổ chân.
     - Vòng đai bùa chú quấn quanh khớp vai.
     - Hoa văn ấn ký huyết mạch phát sáng trên lưng.
   - Vũ khí của Người Sói là **Móng Vuốt Tự Nhiên (Claws)**, không cầm kiếm hay giương nỏ.

---

## 11. PHÂN CẤP CHẾ TÁC: KHẢ NĂNG TÁI SỬ DỤNG GIỮA CÁC GIỚI TÍNH / NGHỀ

Nhằm tiết kiệm $60\%$ công sức sản xuất cho đội ngũ:

| Nhóm Tài Nguyên | Tái Sử Dụng Hoàn Toàn ($100\%$) | Phải Chỉnh Sửa Nhẹ ($20\% - 30\%$) | Bắt Buộc Dựng Mới Riêng Biệt |
| :--- | :--- | :--- | :--- |
| **Binh Khí (Kiếm, Rìu, Cung, Đuốc)**| Nam, Nữ, Dân Làng, Sói Giả Dạng | Người Lùn (Thu nhỏ tỷ lệ $70\%$) | Sói Dạng Thú (Không dùng) |
| **Khiên Đỡ & Nón Lá** | Dùng chung toàn bộ nhân tộc | Người Lùn (Hạ thấp quai nón) | Quái vật |
| **Găng Tay & Ủng Chân** | Nam và Dân Làng | Nữ (Thu hẹp bắp chân $15\%$) | Người Lùn, Sói Thú |
| **Giáp Ngực (Chest Armor)** | Dân Làng cùng phom dáng | Nam $\leftrightarrow$ Nữ (Phom ngực riêng) | Người Lùn, Sói Thú |
| **Áo Choàng Lưng (Cape)** | Nam, Nữ | Cần căn chỉnh khớp vai | Người Lùn |

---

## 12. ĐẶC TẢ PHÂN RÃ TRANG BỊ CỦA "BẠCH LANG KIẾM SĨ" (SILVER WOLF KNIGHT BREAKDOWN)

Phân rã chi tiết bộ trang bị của nhân vật tham chiếu phục vụ sản xuất:

```
+-------------------------------------------------------------------------+
|     DANH MỤC SUB-ASSETS: BỘ TRANG BỊ BẠCH LANG KIẾM SĨ                  |
+-------------------------------------------------------------------------+
| 1. EQ_Head_SilverWolf_Circlet (Băng trán bạc & Búi tóc xám):             |
|    - Mesh: Khối tóc bạc uốn gợn sóng + Băng trán bạc chạm mắt sói.      |
|    - Vị trí: Socket_Head. Polycount: 1,450 Tris.                         |
|                                                                         |
| 2. EQ_Armor_Chest_SilverWolf (Giáp ngực bạc & Thân áo da đỏ):           |
|    - Mesh: Tấm hộ tâm kính bạc + Thân áo da đỏ sẫm ôm sát cơ thể.       |
|    - Loại: Skinned Mesh Replacement. Polycount: 2,450 Tris.             |
|    - Cờ ẩn: Ẩn phần Torso và UpperArm của thân trần gốc.                |
|                                                                         |
| 3. EQ_Cape_SilverWolf_Fur (Áo choàng da sói tuyết):                     |
|    - Mesh: Bờm cổ áo lông trắng dựng cao + Tà áo xòe sau lưng.          |
|    - Vị trí: Socket_Back_Cape (Kèm chuỗi 3 xương rủ). Polycount: 1,420 Tris.|
|                                                                         |
| 4. EQ_Gloves_SilverWolf (Găng tay da bó nẹp bạc):                       |
|    - Mesh: Găng tay da sẫm quấn đai bạc ở cổ tay.                       |
|    - Loại: Skinned Mesh Replacement. Polycount: 520 Tris.               |
|                                                                         |
| 5. EQ_Boots_SilverWolf (Ủng da cao cổ viền lông):                       |
|    - Mesh: Giày bốt da cao cổ có viền lông sói ở mép gối.               |
|    - Loại: Skinned Mesh Replacement. Polycount: 650 Tris.               |
|                                                                         |
| 6. WP_Sword_SacredSilver_T3 (Trường kiếm thần sáng):                    |
|    - Mesh: Lưỡi kiếm bạc bản rộng, rãnh kiếm phát hào quang vàng kim.   |
|    - Vị trí: Socket_Weapon_R. Polycount: 1,000 Tris.                    |
+-------------------------------------------------------------------------+
```

