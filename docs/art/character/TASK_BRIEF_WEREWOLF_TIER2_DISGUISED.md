# TASK BRIEF: SẢN XUẤT NHÂN VẬT — NGỤY LANG NHÂN
## (WEREWOLF TIER 2: DISGUISED YOUNG WEREWOLF)

---

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mã tài liệu:** `TB-CHAR-WOLF-T2`  
**Phiên bản:** `1.0.0`  
**Ngày lập:** 31/10/2026  
**Trạng thái:** `APPROVED FOR PRODUCTION`  
**Tài liệu tham chiếu chuẩn:**
- Master Art Bible: [CHARACTER_ART_BIBLE.md](../CHARACTER_ART_BIBLE.md)
- Phong cách & Bản sắc: [CHARACTER_STYLE_GUIDE.md](./CHARACTER_STYLE_GUIDE.md)
- Dựng hình 3D & Blender: [CHARACTER_MODELING_SPEC.md](./CHARACTER_MODELING_SPEC.md)
- Khung xương & Hoạt cảnh: [CHARACTER_RIG_ANIMATION_SPEC.md](./CHARACTER_RIG_ANIMATION_SPEC.md)
- Hệ thống trang bị Modular: [CHARACTER_EQUIPMENT_SPEC.md](./CHARACTER_EQUIPMENT_SPEC.md)
- Vật liệu & URP Shader: [CHARACTER_TEXTURE_MATERIAL_SPEC.md](./CHARACTER_TEXTURE_MATERIAL_SPEC.md)
- Thư viện Prompt AI: [AI_CHARACTER_PROMPT_LIBRARY.md](./AI_CHARACTER_PROMPT_LIBRARY.md)
- Tiêu chí nghiệm thu QA: [CHARACTER_QA_CHECKLIST.md](./CHARACTER_QA_CHECKLIST.md)
- Phạm vi phát triển: [MVP_SCOPE.md](../../vision/MVP_SCOPE.md) & [PRODUCT_CANON.md](../../vision/PRODUCT_CANON.md)
- Danh pháp & Thuật ngữ: [GLOSSARY.md](../../architecture/GLOSSARY.md)

---

## 1. THÔNG TIN ĐỊNH DANH & PHẠM VI (IDENTITY & SCOPE)

![Concept Ngụy Nhân Lang (Level 02)](file:///C:/Users/NgKaitou/.gemini/antigravity-cli/brain/f6de8c76-536d-45e9-a1cc-1d58bbc5e01d/.user_uploaded/uploaded_media_0_1793455426162.jpg)

### 1.1. Ma trận định danh
- **Tên nhân vật tiếng Việt:** Ngụy Lang Nhân (hoặc *Ngụy Nhân Lang* theo bản vẽ concept mỹ thuật).
- **Tên tiếng Anh:** Young Werewolf / Disguised Werewolf Wanderer.
- **Tên định danh kỹ thuật (Enum C#):** [`WerewolfTier.Tier2_Disguised`](../../architecture/GLOSSARY.md#L54).
- **Tên Prefab Unity:** `Enemy_Werewolf_Tier2_Disguised.prefab` (Logic Root) & `CH_Werewolf_Tier2_Disguised_Visual.prefab` (Visual Root).
- **Tên file mô hình 3D:** `CH_Werewolf_Tier2_Disguised_LOD0.fbx`.
- **Phân nhóm nhân vật:** Nhóm B — Người Sói ([CHARACTER_ART_BIBLE.md](../CHARACTER_ART_BIBLE.md#L140)), tiểu nhóm B1: *Dạng Người (Cải trang / Bán Sói du mục)*.

### 1.2. Mốc phát triển & Ranh giới Gameplay ([MVP_SCOPE.md](../../vision/MVP_SCOPE.md))
- **Mốc M8 (Core Vertical Slice):** Cung cấp mô hình Bán Sói cơ sở để kiểm thử hệ thống chiến đấu action combat và khả năng chia sẻ Mecanim Humanoid Rig.
- **Mốc M12 (MVP Demo công khai):** Đóng vai trò hạt nhân trong cơ chế "Đoàn buôn đáng ngờ" và "Nghi kỵ phe phái" (Suspicion / Faction Infiltration). Ban ngày giả trang làm thương nhân du mục mang vác hàng hóa, ban đêm hoặc khi bị dồn vào chân tường sẽ lộ nguyên hình cào xé người chơi.
- **Biome trực thuộc:** **Biome 1 — Rừng Rậm Trường Sơn** (khu vực bìa rừng âm u, đường mòn giao thương và sườn đồi sương mù).

---

## 2. YÊU CẦU MỸ THUẬT & HÌNH KHỐI (ARTISTIC STYLE & SILHOUETTE)
*(Đối chiếu trực tiếp từ bản vẽ thiết kế Level 02: Ngụy Nhân Lang và [CHARACTER_STYLE_GUIDE.md](./CHARACTER_STYLE_GUIDE.md))*

### 2.1. Tỷ lệ cơ thể & Dáng thế (Proportions & Posture)
- **Chiều cao kỹ thuật:** $1.85\text{m} \pm 0.03\text{m}$ (tính từ đế chân đến đỉnh chóp tai sói khi đứng thẳng).
- **Tư thế đặc trưng (Stance):** Dáng đứng khom săn mồi (*Predatory Hunch*), lưng hơi gù gập $8^\circ - 10^\circ$, hạ thấp trọng tâm để tạo cảm giác quái thú đang gồng mình kìm nén bản năng dã man dưới lớp áo du mục.
- **Tỷ lệ đầu/thân:** $1:6.5$ (chuẩn Heroic Stylized). Khối bàn tay và bàn chân được phóng đại $115\% - 120\%$ để làm bật móng vuốt cào xé ở góc máy Isometric $16\text{m}$.
- **Cấu trúc chân:** Bán nhân thú (Semi-digitigrade / Anthropomorphic lupine), cẳng chân cong nhẹ của loài săn mồi, tiếp đất vững chãi.

### 2.2. Nhận diện hình khối (Silhouette Readability)
- **Khối đầu & Mũ trùm:** Hình khối chóp nón du mục đặc trưng với **hai chóp tai sói nhọn vểnh cao đâm xuyên qua mũ trùm**. Mõm sói dài vươn ra khỏi bóng tối của vành mũ.
- **Khối thân & Áo choàng:** Áo choàng ngụy trang rủ dài qua đầu gối, xẻ tà sau lưng và xơ rách tua rua ở gấu áo tạo nhịp điệu chuyển động phấp phới khi rình rập.
- **Khối đuôi xù:** Đuôi sói dày lông xơ xác ("Đuôi xù xơ xác") thò dài ra phía sau vạt áo choàng, đóng vai trò cân bằng thị giác với phần mõm vươn về phía trước.
- **Khối móng vuốt:** Bàn tay khẳng khiu, khớp ngón to, sở hữu 5 ngón kết thúc bằng móng vuốt đen dài uốn cong sắc lẹm ("Móng vuốt đen").

### 2.3. Bảng màu & Xử lý bề mặt (Color Palette & Hand-Painted Textures)
| Phân Vùng | Mã Màu Hex Tham Chiếu | Tên Sắc Độ | Mô Tả Xử Lý Bề Mặt |
| :--- | :--- | :--- | :--- |
| **Lông Sói (Fur)** | `#2B2E34` $\to$ `#1A1C20` | Charcoal Wolf Grey | Lông màu tro đen thẫm, các mảng lông vẽ tay phân mảng lớn (*Chunky stylized fur clumps*), viền sáng nhẹ ở sống mũi và chóp tai. |
| **Áo Choàng (Cloak)** | `#5C4D3E` $\to$ `#42372C` | Smoked Earth Brown | Vải thô dệt sợi lớn sấy khói, mép rách gấu nhuộm màu bùn đất sình lầy Trường Sơn. |
| **Băng Vải Quấn (Wraps)**| `#948472` $\to$ `#C2B4A3` | Weathered Sand Tan | Băng vải thô quấn quanh cổ tay, cẳng chân; sờn mép, dính vết ố phèn tự nhiên. |
| **Đai & Túi Da (Pouch)** | `#483425` $\to$ `#2E1F15` | Buffalo Dark Leather | Da trâu khô thuộc thô sơ, khâu chỉ viền gồ ghề, có khuy cài bằng xương thú. |
| **Mắt Sói (Eyes)** | `#FFB800` $\to$ `#FF8800` | Predatory Amber | Mắt màu vàng hổ phách sắc lẹm, có rãnh đồng tử hẹp, phát quang nhẹ (`Emission` $1.5\times$) trong màn đêm. |
| **Móng Vuốt (Claws)** | `#151618` $\to$ `#0A0B0C` | Obsidian Black | Vuốt sừng đen bóng mờ (`Smoothness: 0.65`), mép vuốt cào xước nhẹ phản quang ánh trăng. |

---

## 3. QUY CHUẨN KỸ THUẬT & LƯỚI (TECHNICAL & MODELING SPECIFICATIONS)
*(Đối chiếu [CHARACTER_MODELING_SPEC.md](./CHARACTER_MODELING_SPEC.md) & [CHARACTER_LOD_OPTIMIZATION.md](./CHARACTER_LOD_OPTIMIZATION.md))*

### 3.1. Loại hình cấu trúc
- **Kiến trúc kết hợp:** **Modular Skinned Mesh thay thế kết hợp Socket Attachment**:
  - `Base Body (Wolf Mannequin)`: Chứa toàn bộ cơ thể dã thú bên trong (Đầu sói, Cổ, Ngực trong, Bàn tay vuốt, Chân sói, Đuôi).
  - `Skinned Mesh Parts`: Áo choàng có mũ, Áo trong, Quần xé gấu, Băng quấn tay chân.
  - `Socket Attachment`: Túi đồ du mục (`Socket_Hip_Pouch_R`).
  *(Lưu ý: Khi nhân vật kích hoạt chiêu biến hình lộ lốt Sói Bậc 3+ theo [CHARACTER_EQUIPMENT_SPEC.md](./CHARACTER_EQUIPMENT_SPEC.md#L226), toàn bộ các phần vải áo sẽ tự động tắt `SetActive(false)` để chuyển sang dạng Quái thú thuần túy).*

### 3.2. Ngân sách đa giác đa cấp độ (Polygon Budget Architecture)
| Cấp Độ Chi Tiết (LOD) | Cự Ly Camera Tham Chiếu | Ngân Sách Tối Đa (Triangles) | Trọng Số Giảm Đa Giác | Số Xương Tác Động (Bone Weights) |
| :--- | :--- | :--- | :--- | :--- |
| **LOD0 (High Detail)** | $0\text{m} - 15\text{m}$ (Gần & UI Inspect) | $\le \mathbf{9,000\text{ Tris}}$ (Target: ~7,800) | $100\%$ gốc | 4 Bones / Vertex |
| **LOD1 (Medium Detail)**| $15\text{m} - 25\text{m}$ (Isometric chuẩn) | $\le \mathbf{4,500\text{ Tris}}$ | Giảm $50\%$ | 2 Bones / Vertex |
| **LOD2 (Low / Far)** | $> 25\text{m}$ (Sương mù / Rìa màn hình) | $\le \mathbf{1,800\text{ Tris}}$ | Giảm $80\%$ | 1-2 Bones / Vertex |

### 3.3. Trọng tâm, Trục tọa độ và Thiết lập Blender
- **Hệ trục không gian:**
  - Trong Blender: $-Y$ Forward, $+Z$ Up (Mặt nhìn về $-Y$).
  - Trong Unity 6 LTS URP: $+Z$ Forward, $+Y$ Up, $+X$ Right.
- **Tọa độ Pivot Point:** Cố định chính xác tại $(0.0, 0.0, 0.0)$ tại mặt phẳng sàn, nằm chính giữa hai gót chân chạm đất.
- **Quy chuẩn Transform:** Trước khi xuất file, bắt buộc áp dụng `Ctrl + A` $\to$ **All Transforms** (Position: $0,0,0$, Rotation: $0^\circ,0^\circ,0^\circ$, Scale: $1.0, 1.0, 1.0$).

### 3.4. Cấu hình Khung xương & Khớp nối (Rigging & Extra Bones)
- **Hệ xương chuẩn Humanoid (53 xương cơ bản):** Tương thích hoàn toàn với Mecanim Avatar của dự án để kế thừa các động tác đi lại, chạy trốn, gục ngã.
- **Hệ xương phụ trợ bắt buộc (Extra Bones):**
  - `Bone_Tail_01` $\to$ `Bone_Tail_02` $\to$ `Bone_Tail_03` $\to$ `Bone_Tail_04`: Chuỗi 4 xương đuôi sói uốn lượn.
  - `Bone_Ear_L`, `Bone_Ear_R`: Cặp xương tai sói cử động vểnh/cụp theo trạng thái cảnh giác.
  - `Bone_Jaw`: Xương hàm dưới phục vụ nhe nanh gầm gừ và cắn xé.
  - `Bone_Cloak_Back_01..03`: Chuỗi 3 xương tà áo choàng sau lưng tạo độ đung đưa theo chuyển động chạy.
- **Điểm neo trang bị (Sockets):**
  - `Socket_Hip_Pouch_R`: Gắn túi đồ du mục bên hông phải (gắn theo xương `Hips`).
  - `Socket_Weapon_R`: Điểm cầm vũ khí phụ hoặc gậy du mục (gắn theo xương `RightHand`).

---

## 4. BẢNG PHÂN RÃ CHI TIẾT TRANG BỊ & KHE CẮM (EQUIPMENT BREAKDOWN)

```
+----------------------------------------------------------------------------------------------------+
|                BẢNG PHÂN RÃ MODULAR SUB-ASSETS: NGỤY LANG NHÂN (TIER 2 DISGUISED)                  |
+----------------------------------------------------------------------------------------------------+
| 1. CH_Werewolf_Tier2_Disguised_Body_LOD0 (Khung thân sói cơ sở):                                   |
|    - Cấu trúc: Đầu sói nhe nanh, mắt hổ phách, ngực trong, bàn tay 5 ngón móng vuốt, chân & đuôi. |
|    - Loại liên kết: Skinned Mesh Renderer (Gắn kết Master Skeleton 53 bones + Tail/Ears/Jaw).      |
|    - Ngân sách: 3,400 Triangles (LOD0) / 1,700 Tris (LOD1) / 750 Tris (LOD2).                     |
|                                                                                                    |
| 2. EQ_Cloak_NomadCowl_Tier2_LOD0 (Áo choàng ngụy trang du mục & Mũ trùm khoét tai):               |
|    - Cấu trúc: Mũ trùm che kín nửa mặt, khoét 2 lỗ viền mép nổi gồ, tà áo choàng rủ xé gấu tua rua.|
|    - Khe cắm (Slot ID): Slot_Cape / Slot_Head (Ghép tổ hợp).                                       |
|    - Loại liên kết: Skinned Mesh (Weight theo Chest, UpperArm, Spine + Bone_Cloak_Back_01..03).    |
|    - Cờ ẩn mesh: Bật cờ Hide_Hair / Hide_Neck để triệt tiêu hoàn toàn xuyên thấu cổ.                |
|    - Ngân sách: 2,200 Triangles (LOD0) / 1,100 Tris (LOD1) / 450 Tris (LOD2).                     |
|                                                                                                    |
| 3. EQ_Tunic_NomadCloth_Tier2_LOD0 (Áo chẽn vải thô cộc tay):                                       |
|    - Cấu trúc: Áo cổ chữ V tối giản, dây đai quấn chéo ngang ngực giữ nếp áo.                     |
|    - Khe cắm (Slot ID): Slot_Chest.                                                                |
|    - Loại liên kết: Skinned Mesh (Weight theo Spine, Chest, Hips).                                 |
|    - Cờ ẩn mesh: Bật cờ Hide_Torso (Ẩn phần da thịt ngực và bụng của Base Body bên trong).         |
|    - Ngân sách: 950 Triangles (LOD0) / 450 Tris (LOD1) / 200 Tris (LOD2).                         |
|                                                                                                    |
| 4. EQ_Pants_NomadTattered_Tier2_LOD0 (Quần xẻ gấu vải thô & Khăn thắt đai lưng):                  |
|    - Cấu trúc: Quần thô cộc chạm đầu gối xơ xác gấu, đai vải quấn eo có vạt rủ phía trước.         |
|    - Khe cắm (Slot ID): Slot_Legs.                                                                 |
|    - Loại liên kết: Skinned Mesh (Weight theo Hips, LeftUpLeg, RightUpLeg).                        |
|    - Cờ ẩn mesh: Bật cờ Hide_Thighs (Ẩn vùng thịt đùi trên). Lỗ hổng cho đuôi sói vươn ra sau.     |
|    - Ngân sách: 850 Triangles (LOD0) / 420 Tris (LOD1) / 180 Tris (LOD2).                         |
|                                                                                                    |
| 5. EQ_Wraps_Limbs_Tier2_LOD0 (Băng vải thô quấn cẳng tay & cẳng chân):                             |
|    - Cấu trúc: Các dải băng vải gai quấn nhiều vòng quanh cổ tay, bàn tay và ống chân.             |
|    - Khe cắm (Slot ID): Slot_Gloves & Slot_Boots.                                                  |
|    - Loại liên kết: Skinned Mesh (Weight theo Forearm/Hand & Calf/Foot).                           |
|    - Cờ ẩn mesh: Không ẩn thịt móng vuốt; giữ nguyên móng đen thò ra ngoài dải băng.              |
|    - Ngân sách: 720 Triangles (LOD0) / 350 Tris (LOD1) / 140 Tris (LOD2).                         |
|                                                                                                    |
| 6. EQ_Prop_NomadPouch_Tier2_LOD0 (Túi đồ du mục đeo hông):                                         |
|    - Cấu trúc: Túi da trâu khô đeo chéo bằng quai da mảnh, khuy cài xương dã thú.                 |
|    - Khe cắm (Slot ID): Socket_Hip_Pouch_R.                                                        |
|    - Loại liên kết: Socket Attachment (Static Mesh gắn làm con của Transform Socket).              |
|    - Ngân sách: 380 Triangles (LOD0) / 180 Tris (LOD1) / 80 Tris (LOD2).                          |
+----------------------------------------------------------------------------------------------------+
| TỔNG CỘNG TOÀN BỘ NHÂN VẬT (FULL ASSEMBLED LOD0): ~8,500 Triangles (ĐẠT CHUẨN <= 9,000 TRIS)      |
+----------------------------------------------------------------------------------------------------+
```

---

## 5. THƯ VIỆN PROMPT AI TẠO CONCEPT 2D & TURNAROUND SHEET
*(Được xây dựng chuẩn xác theo cấu trúc 6 trường quy định tại [AI_CHARACTER_PROMPT_LIBRARY.md](./AI_CHARACTER_PROMPT_LIBRARY.md))*

### 5.1. Prompt Bộ 1: Tranh Concept Chi Tiết Nhân Vật Tổng Thể (Stylized Concept Art)
- **Main Prompt:**
  ```text
  masterpiece character concept art of Level 02 Disguised Young Werewolf wanderer, Vietnamese dark fantasy Albion Online and Torchlight 3D stylized art style, athletic anthropomorphic wolf build, height 1.85m, slightly hunched predatory posture. Wearing a tattered hooded nomad cowl and cloak in smoked earthy brown fabric with natural holes allowing tall pointed black-furred wolf ears to poke through cleanly, sharp lupine snout visible from beneath the deep hood shadow, fierce glowing amber-yellow predatory eyes, snarl revealing white sharp fangs. Minimalist rustic dark tunic beneath, crossed leather shoulder straps holding a small weathered nomad leather satchel at the hip, ragged cropped pants, coarse sandy-tan cloth bandages tightly wrapped around forearms and lower legs, exposing terrifying long black claws on hands and digitigrade wolf feet, scruffy bushy charcoal wolf tail protruding from the back of cloak. Solid neutral grey background #808080, cinematic top-down key rim lighting, hand-painted texture aesthetic, bold readable chunky silhouettes, game-ready character design, 8k resolution, artstation trending --ar 3:4 --stylize 250 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  photorealistic, realistic human face, western gothic knight armor, knight helmet, sci-fi, steampunk, firearms, cute anime furry, chibi, oversaturated neon, extra limbs, deformed fingers, extra ears, smooth plastic textures, modern zippers, cluttered micro-noise, bright cheerful colors, high-poly micro pores
  ```
- **Reference Requirements:** Tỷ lệ khung hình $3:4$. Ánh sáng studio 3 điểm với viền sáng rọi từ trên cao xuống để tách biệt áo choàng nâu khỏi nền xám trung tính.
- **Expected Output:** Nhân vật toát lên vẻ ngoài bí ẩn, nguy hiểm của loài sói săn mồi giả trang du mục; mũ trùm khoét tai, vuốt đen sắc nhọn, băng quấn tay chân và đuôi xù xơ xác đúng nguyên mẫu thiết kế.
- **Common Failure Cases:** AI hay vẽ mũ trùm trùm kín tai sói làm phẳng đầu, hoặc biến đổi thành người thường hoàn toàn không có mõm và móng vuốt.
- **Human Review Requirements:** Đảm bảo 2 tai sói vểnh cao qua mũ trùm, kiểm tra màu mắt vàng hổ phách và độ sắc của vuốt tay vuốt chân.

---

### 5.2. Prompt Bộ 2: Bản Vẽ 3 Góc Chiếu Turnaround Sheet Chuẩn A-Pose (3D Modeling Blueprint)

![Bản vẽ Turnaround Sheet 3 góc A-Pose đã tạo sinh](file:///C:/Users/NgKaitou/.gemini/antigravity-cli/brain/f6de8c76-536d-45e9-a1cc-1d58bbc5e01d/disguised_young_werewolf_turnaround_1793456436894.jpg)

- **Main Prompt:**
  ```text
  full body character turnaround model sheet of Level 02 Disguised Young Werewolf, showing three orthographic views side-by-side: front view, side profile view, and back view. Standing in perfect symmetrical A-pose modeling stance with arms angled 45 degrees down, palms slightly turned forward displaying sharp black claws, feet shoulder-width apart, tail resting naturally behind. Horizontal height alignment reference lines across head, ear tips, shoulders, waist, knees, and paws across all three views. Wearing tattered earthy brown nomad hooded cloak with wolf ears poking out, rustic inner tunic, coarse cloth bandage wraps around forearms and shins, nomad leather hip pouch, bushy ragged charcoal tail extending from rear. Solid flat neutral grey background #808080, pure orthographic projection, zero perspective distortion, flat diffuse lighting, clean outlines for 3D modeling blueprint in Blender --ar 16:9 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  perspective angle, dynamic action pose, foreshortening, drop shadows on floor, inconsistent height between views, different clothing details between front and back, open mouth with tongue, smiling, photorealistic, cinematic volumetric lighting, 3D render artifacts
  ```
- **Reference Requirements:** Tỷ lệ khung hình rộng $16:9$. Chiếu trực giao (*Orthographic*), không có bóng đổ mặt sàn làm che lấp bàn chân.
- **Expected Output:** Bức tranh kỹ thuật gồm 3 góc chiếu: Chính diện (Front), Góc nghiêng (Side Profile), Phía sau (Back) gióng thẳng hàng từng pixel ở đỉnh tai, mắt, khuỷu tay, thắt lưng và bàn chân.
- **Common Failure Cases:** Góc nghiêng bị biến thành góc 3/4; đuôi ở góc sau và góc nghiêng không trùng khớp độ dài; vị trí túi đeo hông bị nhảy từ phải sang trái.
- **Human Review Requirements:** Dùng thước gióng ngang (*Ruler Guides*) trong Photoshop/GIMP để căn chỉnh tuyệt đối chiều cao các khớp trước khi nạp vào Background Image của Blender.

---

## 6. HƯỚNG DẪN THIẾT LẬP BLENDER & CÁC MỐC ANIMATION EVENTS
*(Đối chiếu [CHARACTER_MODELING_SPEC.md](./CHARACTER_MODELING_SPEC.md) & [CHARACTER_RIG_ANIMATION_SPEC.md](./CHARACTER_RIG_ANIMATION_SPEC.md))*

### 6.1. Thiết lập không gian làm việc trong Blender 4.2 LTS
1. **Đơn vị đo lường (Scene Properties):**
   - Unit System: `Metric`
   - Unit Scale: `1.0` (Đảm bảo $1.0\text{ BU} = 1.0\text{ Meter} = 1.0\text{ Unity Unit}$)
   - Length: `Meters`, Angle: `Degrees`
2. **Quy tắc trục tọa độ:**
   - Hướng nhìn của nhân vật: Hướng mặt quay về **$-Y$ Forward**, lưng quay về $+Y$, đầu hướng lên $+Z$.
3. **Áp dụng Transform trước khi Rig/Export:**
   - Chọn Mesh và Armature $\to$ Phím tắt `Ctrl + A` $\to$ Chọn **All Transforms**.
   - Kiểm tra bảng `N-Panel`: Location $(0,0,0)$, Rotation $(0^\circ,0^\circ,0^\circ)$, Scale $(1.0, 1.0, 1.0)$.

### 6.2. Thiết lập Preset xuất file FBX (`Unity_Character_Export`)
Khi xuất mô hình từ Blender sang thư mục `Assets/_Project/Art/Characters/Werewolf_Tier2/`:
- **Include:** `Selected Objects` (Chỉ chọn Mesh và Armature).
- **Transform:**
  - Scale: `1.0` | Apply Scalings: `FBX All`
  - Forward: **`-Z Forward`** | Up: **`Y Up`**
  - Apply Unit: `Checked` | Use Space Transform: `Checked`
- **Geometry:**
  - Apply Modifiers: `Checked`
  - Smoothing: `Face`
  - Tangent Space: `Checked` (Bắt buộc cho URP Normal Mapping).
- **Armature:**
  - Primary Bone Axis: `Y Axis` | Secondary Bone Axis: `X Axis`
  - Armature FBXNode Type: `Null`
  - **Add Leaf Bones: `UNCHECKED (TẮT HOÀN TOÀN)`** *(Tránh sinh xương rác làm hỏng Mecanim)*.

---

### 6.3. Danh mục Animation Clips & Mốc Animation Events cần nướng
Toàn bộ hoạt cảnh di chuyển của Ngụy Lang Nhân **BẮT BUỘC IN-PLACE ANIMATION** để đồng bộ mạng LAN 4-6 người chơi theo [ADR-09](../../architecture/TECHNICAL_DECISIONS.md).

```
[ TIMELINE CLAW COMBO: Werewolf_T2_Attack_Claw_Slash (18 Frames @ 30 FPS) ]

Frame 0         Frame 6            Frame 10                    Frame 18
  |----------------|------------------|---------------------------|
  ^                ^                  ^                           ^
Lấy đà co vuốt    Event:             Event:                      Thu chiêu hồi thế
(Wind-up/Telegraph) OnAttackHitboxStart OnAttackHitboxEnd         (Recovery)
                   - Bật Collider     - Tắt Collider              - Cho phép combo tiếp
                   - Phát âm cào xé   - Gán trạng thái trúng đòn
```

| Tên Hoạt Cảnh (Clip Name) | Chu Kỳ (Loop) | Tổng Frame (@30fps) | Tên Mốc Sự Kiện (Animation Event) | Frame Kích Hoạt | Tác Vụ Code C# Thực Thi (`PlayerCombatController` / `WerewolfAI`) |
| :--- | :--- | :--- | :--- | :--- | :--- |
| `Werewolf_T2_Loco_Idle` | Yes | 45 frames (1.5s) | `OnWolfSnarlBreath` | Frame 20 | Kích hoạt hiệu ứng thở ra khói sương và âm thanh gầm nhẹ. |
| `Werewolf_T2_Loco_Walk` | Yes | 30 frames (1.0s) | `OnFootstep("L")`<br>`OnFootstep("R")` | Frame 8<br>Frame 22 | Phát âm thanh bước chân dẫm lá khô/bùn đất Trường Sơn. |
| `Werewolf_T2_Loco_Prowl_Run` | Yes | 20 frames (0.67s)| `OnFootstep("L")`<br>`OnFootstep("R")` | Frame 5<br>Frame 15 | Dáng chạy vồ khom lưng tốc lực $6.5\text{ m/s}$ (In-Place). |
| `Werewolf_T2_Attack_Claw_Combo1` | No | 15 frames (0.5s) | `OnAttackHitboxStart`<br>`OnAttackHitboxEnd` | Frame 5<br>Frame 8 | Vung trảo chéo từ trên xuống; bật quét quạt va chạm sát thương. |
| `Werewolf_T2_Attack_Claw_Combo2` | No | 18 frames (0.6s) | `OnAttackHitboxStart`<br>`OnAttackHitboxEnd` | Frame 6<br>Frame 10 | Vung trảo ngang quét ngược; gây hiệu ứng Chảy Máu (*Bleed*). |
| `Werewolf_T2_Attack_Leap_Bite` | No | 24 frames (0.8s) | `OnWolfBiteImpact` | Frame 11 | Lao vập hàm cắn xé; trừ máu mục tiêu và hút sinh lực nếu trúng. |
| `Werewolf_T2_Hit_React` | No | 10 frames (0.33s)| `OnHitRecovery` | Frame 8 | Giật người ra sau, chớp đỏ Shader `_HitFlash` trong URP. |
| `Werewolf_T2_Death_Collapse` | No | 36 frames (1.2s) | `OnDeathSpawnRemains` | Frame 30 | Gục ngã xuống đất; chuyển hóa thành Mộ hoặc vũng huyết cốt. |
| `Werewolf_T2_Disguise_Trade_Idle`| Yes | 60 frames (2.0s) | `OnNomadAdjustBag` | Frame 30 | Động tác xốc lại quai túi đeo hông để che giấu móng vuốt khi gặp dân. |
| `Werewolf_T2_Transform_Burst` | No | 66 frames (2.2s) | `OnTransformBeastTrigger` | Frame 45 | Gầm xé áo choàng, tắt trang bị người, chuyển sang Beast Form. |

---

## 7. BẢNG TIÊU CHÍ NGHIỆM THU CHẤT LƯỢNG MỸ THUẬT (QA ACCEPTANCE CHECKLIST)
*(Trích xuất đặc thù cho Ngụy Lang Nhân từ [CHARACTER_QA_CHECKLIST.md](./CHARACTER_QA_CHECKLIST.md))*

| Nhóm Kiểm Tra | Mã ID | Tiêu Chí Kiểm Tra Kỹ Thuật Cho Ngụy Lang Nhân | Điều Kiện ĐẠT (PASS) | Điều Kiện TRƯỢT (FAIL) | Phương Pháp Kiểm Tra | Mức Độ Nghiêm Trọng |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **A. Concept & Style** | **QA-A01** | Bản sắc Bán Sói Đại Ngàn | Thể hiện đúng chất du mục rừng rậm Trường Sơn, vải sấy khói thô sơ, không dính giáp sắt hiệp sĩ phương Tây. | Mang phong cách Gothic, người sói người máy Sci-fi hoặc Anime lông xù. | Đối chiếu Concept Art & Art Director duyệt | `CRITICAL` |
| **A. Concept & Style** | **QA-A03** | Silhouette nhận diện 2 tai sói | Nhận diện rõ 2 tai sói nhọn đâm xuyên qua mũ trùm và vệt mắt vàng ở khoảng cách $16\text{m}$. | Tai bị giấu trong mũ khiến đầu tròn vo như dân làng bình thường. | Pure Black Silhouette Test ở cự ly $16\text{m}$ | `CRITICAL` |
| **B. Modeling** | **QA-B01** | Chiều cao kỹ thuật tuyệt đối | Cao đúng $1.85\text{m} \pm 0.03\text{m}$ (tính cả tai) khi đứng thẳng trong Blender. | Chiều cao lệch quá $0.05\text{m}$ làm hỏng Capsule Collider của Unity. | Đo bằng Blender Measure Tool | `BLOCKER` |
| **B. Modeling** | **QA-B03** | Gốc tọa độ Pivot Point | Pivot nằm chính xác tại $(0, 0, 0)$ giữa hai bàn chân chạm sàn. | Pivot nằm ở hông, ngực hoặc lơ lửng trên không. | Kiểm tra Object Origin trong 3D Viewport | `BLOCKER` |
| **B. Modeling** | **QA-B04** | Trạng thái Transform sạch | Position $(0,0,0)$, Rotation $(0,0,0)$, Scale $(1.0, 1.0, 1.0)$. | Scale bị âm hoặc khác $1.0$ (như $0.01$ hay $100.0$). | Xem bảng Transform `N-Panel` Blender | `BLOCKER` |
| **C. Topology** | **QA-C01** | Tuyệt đối không có N-gons | $0$ mặt có số cạnh $\ge 5$. Lưới Quad chiếm $\ge 90\%$. | Tồn tại bất kỳ mặt N-gon nào trên mô hình. | Lệnh `Select -> By Number of Vertices > 4` | `BLOCKER` |
| **C. Topology** | **QA-C02** | Edge Loops khớp cử động | Tối thiểu 3 vòng edge loop tại đầu gối, khuỷu tay, ngón tay móng vuốt; vòng lặp kép tại vai. | Khi uốn gập tay cào xé $90^\circ$, cẳng tay bị xẹp lép mất thể tích. | Xoay thử xương Pose Mode $90^\circ$ trong Blender | `CRITICAL` |
| **D. UV Layout** | **QA-D01** | Texel Density đồng nhất | Đạt trung bình $10.24\text{ px/cm} \pm 10\%$ trên Master Atlas 2K; cho phép $14\text{ px/cm}$ ở mặt sói. | Chênh lệch texel $> 40\%$ giữa các bộ phận (đầu quá nét, chân mờ căm). | Kiểm tra bằng Texel Density Checker | `MAJOR` |
| **E. Textures** | **QA-E01** | Stylized Top-down Gradient | BaseMap có dải sáng giảm dần từ đỉnh tai/vai xuống chân ($100\% \to 65\%$). | Nhân vật sáng phẳng lì hoặc chân sáng hơn đầu. | Xem Texture chế độ Unlit trong Unity Scene | `CRITICAL` |
| **F. Materials** | **QA-F01** | Tương thích URP Lit Shader | Sử dụng Shader `Universal Render Pipeline/Lit`, tương thích $100\%$ SRP Batcher. | Dùng shader cũ Built-in bị báo lỗi tím hồng; không tương thích SRP. | Mở Material Inspector xem dòng SRP Batcher | `BLOCKER` |
| **G. Rig & Skinning** | **QA-G01** | Khung xương Mecanim Avatar | Avatar Mecanim ánh xạ xanh toàn bộ ($100\%$ Green Mapping) cho 53 xương cơ bản. | Avatar báo đỏ hoặc thiếu xương ngón tay bắt buộc. | Cửa sổ `Configure Avatar` trong Unity | `BLOCKER` |
| **G. Rig & Skinning** | **QA-G03** | Khử đỉnh mang trọng lượng rác | Không có đỉnh nào mang tổng trọng lượng $\ne 1.0$; không đỉnh dính tại $(0,0,0)$. | Xuất hiện gai nhọn dị tật kéo dài từ cơ thể xuống sàn khi nhân vật chạy. | Xoay xương Hips kiểm tra đỉnh đứng yên | `BLOCKER` |
| **H. Animation** | **QA-H01** | Tuyệt đối In-Place Animation | Toàn bộ clips đi bộ, chạy, né đòn di chuyển tại chỗ, Root Bone đứng im tại $(0,0,0)$. | Nhân vật bị trôi dạt ra khỏi gốc tọa độ trong hoạt cảnh. | Kiểm tra đường cong Transform của Root Bone | `BLOCKER` |
| **H. Animation** | **QA-H03** | Mốc sự kiện chém đòn chính xác | `OnAttackHitboxStart` và `End` kích hoạt đúng cung vung vuốt cào xé. | Hitbox bật trễ sau khi đã thu tay hoặc bật sớm trước khi vung. | So khớp Frame Event với vệt chém thực tế | `CRITICAL` |
| **I. FBX Export** | **QA-I01** | Trục xoay không bị lệch $90^\circ$ | Nhân vật trong Unity đứng thẳng trục $+Y$-Up, nhìn thẳng hướng $+Z$-Forward. | Nhân vật nằm ngửa trên sàn hoặc quay mặt về $-Z$. | Kéo FBX vào Scene trắng kiểm tra Transform | `BLOCKER` |
| **I. FBX Export** | **QA-I02** | Đã tắt Leaf Bones | Tùy chọn `Add Leaf Bones` được TẮT khi xuất FBX từ Blender. | Tồn tại các xương cụt `_end` ở đầu ngón tay và chóp tai. | Xem cây phân cấp Armature trong Unity | `CRITICAL` |
| **K. Equipment** | **QA-K01** | Chống đâm xuyên (Zero Clipping) | Khi chạy nhảy cào xé: Da thịt ngực và đùi trong không đâm xuyên ra ngoài lớp áo vải thô. | Thịt đen của sói lồi xuyên qua áo choàng và quần thô. | Bật cờ ẩn mesh `BodyPartToHide` tương ứng | `CRITICAL` |
| **L. Performance** | **QA-L01** | Ngân sách đa giác LOD0 | Toàn bộ mô hình lắp ghép hoàn chỉnh $\le 9,000$ Triangles. | Vượt quá ngân sách $> 10\%$ ($> 9,900$ Tris). | Kiểm tra Unity Rendering Statistics | `CRITICAL` |
| **M. Readability** | **QA-M03** | Độ tương phản trong bóng tối | Nhận diện rõ vệt mắt vàng hổ phách và dáng khom nguy hiểm trong đêm sương mù. | Bị chìm màu hoàn toàn biến thành đốm đen vô hình trong bụi rậm. | Test hiển thị ban đêm trong Scene Rừng Biome 1 | `CRITICAL` |

---

### Quy trình ký duyệt bàn giao:
- [ ] **Lead Character Artist:** Phê duyệt độ trung thực Concept và Topology lưới sạch.
- [ ] **Lead Technical Artist:** Phê duyệt Transform, Rig Mecanim, In-Place Animation và SRP Batcher.
- [ ] **Lead QA Coordinator:** Phê duyệt vượt qua 100% tiêu chí `BLOCKER` & `CRITICAL`.
- **Trạng thái xuất xưởng:** `READY FOR ENGINE INTEGRATION` khi đủ 3 chữ ký.
