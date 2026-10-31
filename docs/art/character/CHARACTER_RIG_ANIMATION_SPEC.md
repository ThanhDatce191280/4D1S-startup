# QUY CHUẨN KỸ THUẬT KHUNG XƯƠNG VÀ DIỄN HOẠT
## (CHARACTER RIG & ANIMATION SPECIFICATION)

---

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mã tài liệu:** `CAB-SPEC-03`  
**Phiên bản:** `1.0.0`  
**Ngày cập nhật:** 31/10/2026  
**Trạng thái tài liệu:** `PROPOSED` (Chờ tích hợp và kiểm thử diễn hoạt trong Unity Animator)  
**Tài liệu cha:** [CHARACTER_ART_BIBLE.md](../CHARACTER_ART_BIBLE.md)  
**Tác giả:** Character Technical Animator & Technical Artist  

---

## MỤC LỤC
1. [Triết Lý Khung Xương và Diễn Hoạt](#1-triết-lý-khung-xương-và-diễn-hoạt)
2. [Hệ Thống Khung Xương Chuẩn Humanoid (Humanoid Skeleton)](#2-hệ-thống-khung-xương-chuẩn-humanoid-humanoid-skeleton)
3. [Hệ Thống Khung Xương Dã Thú và Sói (Wolf & Beast Skeleton)](#3-hệ-thống-khung-xương-dã-thú-và-sói-wolf--beast-skeleton)
4. [Quy Tắc Đặt Tên Xương và Cấu Trúc Thứ Bậc (Bone Hierarchy)](#4-quy-tắc-đặt-tên-xương-và-cấu-trúc-thứ-bậc-bone-hierarchy)
5. [Chính Sách Hoạt Cảnh Tại Chỗ (In-Place Animation vs Root Motion Policy)](#5-chính-sách-hoạt-cảnh-tại-chỗ-in-place-animation-vs-root-motion-policy)
6. [Quy Chuẩn Quét Trọng Lượng Xương (Skinning & Weight Painting)](#6-quy-chuẩn-quét-trọng-lượng-xương-skinning--weight-painting)
7. [Hệ Thống Động Học Ngược (Inverse Kinematics - IK)](#7-hệ-thống-động-học-ngược-inverse-kinematics---ik)
8. [Danh Mục Hoạt Cảnh Tiêu Chuẩn (Animation Clip Library)](#8-danh-mục-hoạt-cảnh-tiêu-chuẩn-animation-clip-library)
9. [Hệ Thống Sự Kiện Hoạt Cảnh (Animation Events Architecture)](#9-hệ-thống-sự-kiện-hoạt-cảnh-animation-events-architecture)
10. [Cấu Trúc Unity Animator Controller](#10-cấu-trúc-unity-animator-controller)
11. [Đồng Bộ Hoạt Cảnh Qua Mạng LAN 4–6 Người (Multiplayer Synchronization)](#11-đồng-bộ-hoạt-cảnh-qua-mạng-lan-46-người-multiplayer-synchronization)
12. [Phân Cấp Hoạt Cảnh: MVP vs Lộ Trình Mở Rộng](#12-phân-cấp-hoạt-cảnh-mvp-vs-lộ-trình-mở-rộng)
13. [Đặc Tả Hoạt Cảnh Cho Nhân Vật Mẫu "Bạch Lang Kiếm Sĩ"](#13-đặc-tả-hoạt-cảnh-cho-nhân-vật-mẫu-bạch-lang-kiếm-sĩ)

---

## 1. TRIẾT LÝ KHUNG XƯƠNG VÀ DIỄN HOẠT

Trong một tựa game Action RPG sinh tồn góc nhìn Isometric như **Huyết Nguyệt Đại Ngàn**, animation không chỉ có nhiệm vụ tạo nên sự mượt mà thị giác mà còn là **huyết mạch của nhịp độ chiến đấu (Combat Pacing) và phản hồi xúc giác (Telegraphing)**:

1. **Rõ ràng và Dứt khoát (Snappy & Readable):** Động tác vung kiếm, giương cung hay cào xé của Sói phải có tư thế chuẩn bị rõ ràng (*Anticipation / Telegraph*), pha trúng đòn cực nhanh (*Active Hit Frame*), và thời gian dừng nhẹ tạo cảm giác lực cản thịt xương (*Hit Stop*).
2. **Khả năng tái sử dụng tối đa (Extreme Reusability):** Toàn bộ nhân vật dạng người (Player nam/nữ, NPC, Thương nhân, Thợ rèn) dùng chung khung xương Mecanim Humanoid. Toàn bộ hoạt cảnh di chuyển và thu thập tài nguyên được chia sẻ thông qua Retargeting.
3. **Tuyệt đối tuân thủ In-Place cho Multiplayer:** Nhằm phục vụ mạng LAN 4–6 người chơi và cơ chế chuyển giao máy chủ phòng (*Host Migration*) theo [ADR-09](../../architecture/TECHNICAL_DECISIONS.md), hoạt cảnh không được tự ý dịch chuyển tọa độ thế giới của nhân vật.

---

## 2. HỆ THỐNG KHUNG XƯƠNG CHUẨN HUMANOID (HUMANOID SKELETON)

Mọi nhân vật dạng người phải sử dụng cấu trúc khung xương gồm tối thiểu **53 xương cơ bản** (tương thích $100\%$ với Unity Mecanim Humanoid Avatar Mapping):

```
Root (0, 0, 0)
└── Hips (Pelvis)
    ├── Spine
    │   └── Spine1 (Chest)
    │       └── Spine2 (Upper Chest)
    │           ├── Neck
    │           │   └── Head
    │           │       └── Jaw (Xương hàm)
    │           ├── LeftShoulder (Clavicle)
    │           │   └── LeftUpperArm
    │           │       └── LeftForearm
    │           │           └── LeftHand
    │           │               ├── LeftHandThumb1..3
    │           │               ├── LeftHandIndex1..3
    │           │               ├── LeftHandMiddle1..3
    │           │               ├── LeftHandRing1..3
    │           │               └── LeftHandPinky1..3
    │           └── RightShoulder (Clavicle)
    │               └── RightUpperArm
    │                   └── RightForearm
    │                       └── RightHand
    │                           ├── RightHandThumb1..3
    │                           ├── RightHandIndex1..3
    │                           ├── RightHandMiddle1..3
    │                           ├── RightHandRing1..3
    │                           └── RightHandPinky1..3
    ├── LeftUpLeg (Thigh)
    │   └── LeftLeg (Calf)
    │       └── LeftFoot
    │           └── LeftToeBase
    └── RightUpLeg (Thigh)
        └── RightLeg (Calf)
            └── RightFoot
                └── RightToeBase
```

### Xương Phụ Trợ (Extra Bones / Socket Bones - Không thuộc Humanoid Avatar):
- `Socket_Weapon_R`: Gắn tại cổ tay phải (`RightHand`), xoay dọc theo lòng bàn tay.
- `Socket_Shield_L`: Gắn tại cẳng tay trái (`LeftForearm`), xoay vuông góc hướng ra ngoài.
- `Socket_Cape_01..03`: Chuỗi 3 xương rủ dọc lưng áo choàng lông sói phục vụ vật lý đu đưa nhẹ.

---

## 3. HỆ THỐNG KHUNG XƯƠNG DÃ THÚ VÀ SÓI (WOLF & BEAST SKELETON)

Khấu trúc khung xương Sói và Quái thú 4 chân (`Wolf_Generic_Armature` — sử dụng Generic Rig trong Unity):

```
Root (0, 0, 0)
└── Wolf_Pelvis
    ├── Wolf_Spine1 -> Wolf_Spine2 -> Wolf_Spine3 (Lưng cong dã thú)
    │   ├── Wolf_Neck1 -> Wolf_Neck2
    │   │   └── Wolf_Head
    │   │       ├── Wolf_Jaw (Xương hàm há cắn)
    │   │       ├── Wolf_Ear_L / Wolf_Ear_R (Cặp tai vểnh)
    │   │       └── Wolf_Snout (Mũi hít ngửi)
    │   ├── Wolf_Clavicle_L -> Wolf_UpperArm_L -> Wolf_Forearm_L -> Wolf_FrontPaw_L
    │   └── Wolf_Clavicle_R -> Wolf_UpperArm_R -> Wolf_Forearm_R -> Wolf_FrontPaw_R
    ├── Wolf_Thigh_L -> Wolf_Calf_L -> Wolf_Hock_L (Khớp khuỷu chân sau) -> Wolf_BackPaw_L
    ├── Wolf_Thigh_R -> Wolf_Calf_R -> Wolf_Hock_R -> Wolf_BackPaw_R
    └── Wolf_Tail1 -> Wolf_Tail2 -> Wolf_Tail3 -> Wolf_Tail4 (Chuỗi xương đuôi ngoe nguẩy)
```

- **Sói Vua Tier 9 (Boss):** Bổ sung thêm cặp xương `Wolf_Horn_L / R` (Sừng huyết thạch) và 2 xương giáp đá vai `Wolf_RockPauldron_L / R`.

---

## 4. QUY TẮC ĐẶT TÊN XƯƠNG VÀ CẤU TRÚC THỨ BẬC (BONE HIERARCHY)

1. **Quy tắc phân vế Trái / Phải:**
   - Dùng tiếp đầu ngữ hoặc hậu tố rõ ràng: `Left` / `Right` hoặc `_L` / `_R`.
   - *Khuyến nghị Unity:* Dùng chuẩn `LeftUpperArm`, `RightUpperArm` để Unity tự động nhận diện ánh xạ Humanoid $100\%$ không cần cấu hình bằng tay.
2. **Quy tắc Xương Gốc (Root Bone):**
   - Xương gốc mang tên chính xác: `Root`.
   - Tọa độ `Root` luôn cố định tại $(0, 0, 0)$ thế giới.
   - Xương chuyển động đầu tiên của thân mình là `Hips` (con trực tiếp của `Root`).

---

## 5. CHÍNH SÁCH HOẠT CẢNH TẠI CHỖ (IN-PLACE ANIMATION VS ROOT MOTION POLICY)

```
+-------------------------------------------------------------------------+
|     QUY TẮC PHÂN ĐỊNH: IN-PLACE ANIMATION VS ROOT MOTION (ADR-09)       |
+-------------------------------------------------------------------------+
| 1. HOẠT CẢNH TẠI CHỖ (IN-PLACE - BẮT BUỘC CHO PLAYER & QUÁI THƯỜNG):    |
|    - Toàn bộ hoạt cảnh Idle, Walk, Run, Sprint, Roll, Strafe.           |
|    - Xương Hips chỉ dao động lên xuống nhẹ (Z/Y), KHÔNG tịnh tiến trục Z |
|      vượt ra khỏi bán kính 0.2m của Root.                               |
|    - Nhân vật di chuyển trong thế giới thực hoàn toàn do C# script      |
|      CharacterController.Move() đẩy đi theo vận tốc (5m/s, 8.5m/s).     |
|    - LÝ DO: Giúp đồng bộ mạng LAN 4-6 client hoàn toàn deterministic,   |
|      chống lag giật và không gây lệch vị trí khi Host Migration xảy ra. |
+-------------------------------------------------------------------------+
| 2. HOẠT CẢNH ROOT MOTION (CHỈ ÁP DỤNG NGOẠI LỆ CÓ KIỂM SOÁT):          |
|    - Chỉ dùng cho Boss Sói Vua Tier 9 ở các chiêu thức đặc biệt:        |
|      "Cú Nhảy Vồ Đẫm Máu (Pounce Leap)" hoặc "Gầm Hóa Sói Thức Tỉnh".   |
|    - Chỉ Authority Peer mới được phép nướng chuyển động thực tế.        |
+-------------------------------------------------------------------------+
```

---

## 6. QUY CHUẨN QUÉT TRỌNG LƯỢNG XƯƠNG (SKINNING & WEIGHT PAINTING)

1. **Giới hạn ảnh hưởng xương (Bone Influences):**
   - **Tối đa 4 xương trên mỗi đỉnh (Max 4 Bones per Vertex)** ở LOD0.
   - Tuyệt đối không để đỉnh nào chịu ảnh hưởng bởi 5 xương trở lên (Unity sẽ tự cắt bỏ xương nhỏ nhất gây giật lưới).
2. **Loại bỏ đỉnh không có trọng lượng (Zero-Weight Vertices):**
   - Mọi đỉnh của mô hình phải có tổng trọng lượng đúng bằng **$1.0$** (`Normalize Weights`).
   - Tuyệt đối cấm để sót đỉnh rác mang trọng lượng 0 vì sẽ bị ghim cứng tại gốc $(0,0,0)$ tạo ra hiện tượng gai nhọn kéo dài xuyên sàn đấu.
3. **Xử lý các vùng khớp biến dạng phức tạp:**
   - **Gầm nách (Armpit) & Đáy chậu (Crotch):** Quét chuyển tiếp mượt mà giữa ngực/lưng và cánh tay. Khi hạ tay xuống A-Pose góc $45^\circ$, thịt nách không được dính vào mạng sườn.
   - **Váy/Tà áo xòe (Skirt/Cape):** Quét trọng lượng theo xương đùi tối đa $30\%$, còn lại quét vào chuỗi xương vải riêng biệt (`Bone_Cloth`) để tránh việc bước chân làm kéo rách vạt áo.

---

## 7. HỆ THỐNG ĐỘNG HỌC NGƯỢC (INVERSE KINEMATICS - IK)

Trong giai đoạn hoàn thiện sản xuất:
1. **Foot Placement IK (Đặt chân bám địa hình):**
   - Sử dụng Unity Animation Rigging Package hoặc Two-Bone IK Constraint.
   - Khi nhân vật đứng trên dốc $15^\circ - 45^\circ$ trong `Test_Movement.unity` ([M2_PLAN.md](../../milestones/M2/M2_PLAN.md)), hai bàn chân tự động nâng hạ bám sát mặt dốc, loại bỏ hiện tượng chân đứng lơ lửng trên không trung.
2. **Weapon Hand IK (Cầm vũ khí 2 tay):**
   - Bàn tay phải cầm chuôi kiếm (`Socket_Weapon_R`).
   - Bàn tay trái dùng IK bám theo điểm tựa phụ (`LeftHandGripTarget`) trên cán trường kiếm hoặc cung tre.

---

## 8. DANH MỤC HOẠT CẢNH TIÊU CHUẨN (ANIMATION CLIP LIBRARY)

Toàn bộ clip hoạt cảnh được chuẩn hóa tốc độ khung hình: **$30\text{ FPS}$** (hoặc $60\text{ FPS}$ nướng xuất).

### 8.1. Nhóm Di Chuyển & Sinh Tồn (Locomotion & Survival)
| Tên Hoạt Cảnh (Clip Name) | Thời Lượng | Chu Kỳ (Loop) | Tốc Độ Nhân Vật Tương Ứng | Mô Tả Thế Dáng Thị Giác |
| :--- | :--- | :--- | :--- | :--- |
| `Human_Unarmed_Idle` | $1.5 - 2.0\text{s}$ | Yes | $0.0\text{ m/s}$ | Đứng thở tự nhiên, hai tay buông nhẹ, mắt đảo quan sát |
| `Human_Combat_Idle_Sword` | $1.2\text{s}$ | Yes | $0.0\text{ m/s}$ | Chân trước chân sau, kiếm chúc mũi xuống $45^\circ$ cảnh giác |
| `Human_Walk_Forward` | $1.0\text{s}$ | Yes | $5.0\text{ m/s}$ | Bước đi chắc nịch, hai tay vung đều theo nhịp |
| `Human_Run_Forward` | $0.8\text{s}$ | Yes | $6.5\text{ m/s}$ | Thân người hơi chúi về trước $10^\circ$, bước dài |
| `Human_Sprint_Forward` | $0.65\text{s}$ | Yes | $8.5\text{ m/s}$ | Lao hết tốc lực, vạt áo choàng lông bay thẳng phía sau |
| `Human_Dodge_Roll` | $0.55\text{s}$ | No | Lăn né đòn | Cuộn tròn người tiếp đất bằng vai và đứng bật dậy |

### 8.2. Nhóm Tác Vụ Thu Thập (Gathering & Interactions — Cốt Lõi M4/M8)
| Tên Hoạt Cảnh | Thời Lượng | Chu Kỳ | Frame Kích Hoạt Đòn Đập (Impact Frame) |
| :--- | :--- | :--- | :--- |
| `Human_Gather_Pick` | $0.8\text{s}$ | No | Cúi người hái quả rừng ở Frame 12 |
| `Human_Tool_Chop_Wood` | $1.0\text{s}$ | Yes | Bổ rìu vào thân cây ở Frame 14 (Rìu cắm vào gỗ) |
| `Human_Tool_Mine_Rock` | $1.1\text{s}$ | Yes | Bổ cuốc/búa đập quặng đá ở Frame 16 (Tia lửa tóe ra) |
| `Human_Craft_Kneel` | $2.0\text{s}$ | Yes | Ngồi xổm nhóm lửa nấu Cơm Lam hoặc đun thảo dược |

### 8.3. Nhóm Chiến Đấu Cận Chiến (Melee Combat — Cốt Lõi M3/M8)
| Tên Hoạt Cảnh | Thời Lượng | Frame Đánh Trúng (Hitbox Active) | Loại Đòn & Nhịp Cử Động |
| :--- | :--- | :--- | :--- |
| `Human_Attack_Sword_Combo1` | $0.50\text{s}$ | Frame $6 - 9$ ($0.20\text{s}$) | Chém chéo từ vai phải xuống hông trái (Góc quét $110^\circ$) |
| `Human_Attack_Sword_Combo2` | $0.55\text{s}$ | Frame $7 - 10$ ($0.23\text{s}$) | Chém ngang quét ngược từ trái sang phải đầy uy lực |
| `Human_Hit_Reaction_Light` | $0.30\text{s}$ | Frame 1 | Ngửa người ra sau nhẹ, chớp đỏ material |
| `Human_Death_Collapse` | $1.5\text{s}$ | No (Dừng ở frame cuối)| Quỵ gối, buông rơi kiếm và đổ gục xuống đất thành mộ |

### 8.4. Nhóm Hoạt Cảnh Người Sói (Werewolf & Wolf Pack)
| Tên Hoạt Cảnh | Loài Áp Dụng | Thời Lượng | Mô Tả Cử Động |
| :--- | :--- | :--- | :--- |
| `Wolf_Feral_Idle_Snarl` | Sói Bậc 1 | $1.2\text{s}$ | Gầm gừ, nhe răng lộ nanh nhọn, đuôi vẫy cụp |
| `Wolf_Feral_Run` | Sói Bậc 1 | $0.6\text{s}$ | Lao 4 chân thoăn thoắt, bờm cổ dập dờn |
| `Wolf_Feral_Attack_Bite` | Sói Bậc 1 | $0.65\text{s}$ | Vồ tới đớp mạnh (Active ở Frame 8) |
| `Wolf_Feral_Death` | Sói Bậc 1 | $1.0\text{s}$ | Trượt dài trên đất rồi co giật bất động |
| `Wolf_Transform_To_Beast` | Bán Sói | $2.2\text{s}$ | Gù lưng ôm đầu, cơ bắp xé toạc y phục, gầm hú vang dội |

---

## 9. HỆ THỐNG SỰ KIỆN HOẠT CẢNH (ANIMATION EVENTS ARCHITECTURE)

Để liên kết chặt chẽ hoạt cảnh với mã nguồn C# `PlayerCombatController.cs` và `WorldItemPickup.cs` mà không phụ thuộc vào bộ đếm thời gian ảo:

```
[ TIMELINE HOẠT CẢNH: Human_Attack_Sword_Combo1 (Tổng 15 frames @ 30fps) ]

Frame 0       Frame 6        Frame 9                 Frame 15
  |--------------|--------------|-----------------------|
  ^              ^              ^                       ^
Bắt đầu vung   Event:         Event:                  Kết thúc chiêu
(Wind-up)      OnAttackHitboxStart  OnAttackHitboxEnd (Recovery)
               - Bật quét quạt      - Tắt quét va chạm
               - Phát SFX chém gió  - Cho phép chuỗi Combo 2
```

### Các Hàm Unity Animation Event Chuẩn Hóa:
1. `OnAttackHitboxStart()`: Gọi hàm `PlayerCombatController.PerformMeleeAttack()` kiểm tra va chạm không sinh rác bộ nhớ ([M3_PLAN.md](../../milestones/M3/M3_PLAN.md)).
2. `OnAttackHitboxEnd()`: Tắt cờ sát thương, chốt danh sách nạn nhân trúng đòn.
3. `OnFootstep(string foot)`: Kích hoạt tiếng bước chân đất/nước tương ứng theo loại địa hình va chạm raycast.
4. `OnGatherImpact()`: Trừ máu cây gỗ/mỏ đá và sinh hạt rơi `WorldItemPickup`.

---

## 10. CẤU TRÚC UNITY ANIMATOR CONTROLLER

Hệ thống Animator Controller `PlayerAnimatorController.controller` được tổ chức thành các Layer logic độc lập:

```
+-----------------------------------------------------------------------+
|              CẤU TRÚC LAYER TRONG UNITY ANIMATOR CONTROLLER           |
+-----------------------------------------------------------------------+
| LAYER 1: BASE LOCOMOTION LAYER (Trọng số Weight = 1.0)                |
| - State: Locomotion_BlendTree (2D Freeform Directional)               |
|   - Tham số: Speed (Float), Direction (Float)                         |
|   - Pha trộn: Idle (0.0) <-> Walk (5.0) <-> Run (6.5) <-> Sprint (8.5)|
| - Transition: Roll_Trigger -> Dodge_Roll State -> Return to Locomotion|
+-----------------------------------------------------------------------+
                                   | (Avatar Mask: Thân Trên - Upper Body)
                                   v
+-----------------------------------------------------------------------+
| LAYER 2: UPPER BODY COMBAT & ACTION LAYER (Weight = 1.0, Override)    |
| - Default State: Empty_Null (Cho phép Layer 1 hoạt động bình thường)  |
| - State: Attack_Combo1 -> Trigger: Attack -> Transition sang Combo2   |
| - State: Tool_Gathering (Chặt cây / Đập đá)                           |
| * TÁC DỤNG: Cho phép nhân vật VỪA CHẠY VỪA NẠP CUNG hoặc VỪA ĐI BỘ   |
|   VỪA UỐNG THUỐC mà chân không bị khựng cứng!                         |
+-----------------------------------------------------------------------+
                                   |
                                   v
+-----------------------------------------------------------------------+
| LAYER 3: HIT FEEDBACK & DEATH LAYER (Weight = 1.0, Additive/Override) |
| - State: Hit_Flinch (Kích hoạt khi nhận sát thương chí mạng)          |
| - State: Death_State (Kích hoạt khi IsDead = True -> Khóa di chuyển)  |
+-----------------------------------------------------------------------+
```

---

## 11. ĐỒNG BỘ HOẠT CẢNH QUA MẠNG LAN 4–6 NGƯỜI (MULTIPLAYER SYNCHRONIZATION)

Tuân thủ nghiêm ngặt nguyên tắc kiến trúc mạng [ADR-09](../../architecture/TECHNICAL_DECISIONS.md) và [SESSION_AND_PERSISTENCE.md](../../architecture/SESSION_AND_PERSISTENCE.md):

1. **Không đồng bộ toàn bộ biến Float liên tục:**
   - Tuyệt đối không gửi gói tin `Speed` hay `Direction` của Animator 60 lần/giây qua mạng (làm nghẽn băng thông LAN).
   - Chỉ đồng bộ vị trí thế giới thực (`Position`) và hướng quay (`Rotation`) của `Player`. Máy Client tự tính toán vận tốc cục bộ (`CurrentSpeed = (NewPos - OldPos) / dt`) và tự gán vào Animator cục bộ thông qua `PlayerVisualAnimationBridge.cs`.
2. **Đồng bộ trạng thái chủ động bằng Network RPC Triggers:**
   - Đòn chém, Lăn né đòn, Chết, Biến thành Sói được kích hoạt bằng Event RPC có mã định danh giao dịch (`Transaction ID` hoặc `ActionId`) để đảm bảo không bị kích hoạt lặp lại hai lần khi xảy ra Host Migration.

---

## 12. PHÂN CẤP HOẠT CẢNH: MVP VS LỘ TRÌNH MỞ RỘNG

Để tránh rủi ro đình trệ dự án do nút thắt hoạt cảnh ([RISK_REGISTER.md R-03](../../roadmap/RISK_REGISTER.md)):

| Hạng Mục Hoạt Cảnh | Giai Đoạn Áp Dụng | Trạng Thái Phê Duyệt | Nguồn Sản Xuất Đề Xuất |
| :--- | :--- | :--- | :--- |
| **Locomotion Cơ Bản (Idle, Walk, Run, Sprint)** | `[MVP REQUIRED — M8]` | `[CONFIRMED]` | Mixamo Retargeting + Chỉnh tay |
| **Combo Kiếm Sĩ 2 Nhát (Combo1, Combo2)** | `[MVP REQUIRED — M8]` | `[CONFIRMED]` | Animator chỉnh tay dứt khoát |
| **Lăn Né Đòn (Dodge Roll)** | `[MVP REQUIRED — M8]` | `[CONFIRMED]` | Keyframe Animator thủ công |
| **Thu Thập Cốt Lõi (Chặt Cây, Hái Lượm, Đập Đá)**| `[MVP REQUIRED — M8]` | `[CONFIRMED]` | Khung chuyển động chung 1 tay |
| **Sói Bậc 1 (Idle, Run, Bite, Death)** | `[MVP REQUIRED — M8]` | `[CONFIRMED]` | Dã thú cơ bản 4 chân |
| **Lợn Rừng (Idle, Run, Charge Húc Đầu, Death)** | `[MVP REQUIRED — M8]` | `[CONFIRMED]` | Dùng chung cấu trúc rig Sói/Heo |
| **Cung Thủ / Nỏ Thần Bắn Tên** | `[M9 — M10 Demo]` | `[PROPOSED]` | Chuỗi hoạt cảnh giương và kéo |
| **Pháp Sư Niệm Phép Hồi Sinh** | `[M9 — M10 Demo]` | `[PROPOSED]` | Dựng vòng tròn kết ấn 8 giây |
| **Biến Thể Người Hóa Sói (Transformation)** | `[M10 — M11 Demo]` | `[PROPOSED]` | Hoạt cảnh đặc biệt đổi Mesh |
| **Sói Vua Tier 9 (Full Boss Skillset)** | `[M11 Demo]` | `[PENDING BENCHMARK]`| Bộ hoạt cảnh độc quyền Boss |

---

## 13. ĐẶC TẢ HOẠT CẢNH CHO NHÂN VẬT MẪU "BẠCH LANG KIẾM SĨ"

```
+-----------------------------------------------------------------------+
|  BỘ HOẠT CẢNH TIÊN PHONG: CH_Human_Hero_SilverWolfKnight              |
+-----------------------------------------------------------------------+
| 1. Standing Combat Stance (Thế đứng Bạch Lang):                       |
|    - Cầm trường kiếm bằng tay phải, mũi kiếm hơi chếch về trước.      |
|    - Áo choàng lông sói sau lưng đu đưa nhẹ theo nhịp thở lồng ngực.   |
|    - Cảm giác: Vững chãi như cây lim nghìn năm tuổi giữa rừng thiêng. |
|                                                                       |
| 2. White Wolf Cleave (Nhát chém Trảm Nguyệt):                         |
|    - Frame 0-3: Hạ thấp trọng tâm xoay người 45 độ lấy đà.           |
|    - Frame 4: Kiếm vung quét ngang tạo dải vệt sáng vàng kim (VFX).   |
|    - Frame 5-8: Kiếm dừng đột ngột tạo quán tính dội ngược cơ bắp.   |
|    - Frame 9-14: Thu kiếm về tư thế thủ thanh thoát.                  |
+-----------------------------------------------------------------------+
```

