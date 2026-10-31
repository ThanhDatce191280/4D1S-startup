# QUY CHUẨN VẬT LIỆU VÀ TEXTURE STYLIZED URP
## (CHARACTER TEXTURE & MATERIAL SPECIFICATION)

---

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mã tài liệu:** `CAB-SPEC-05`  
**Phiên bản:** `1.0.0`  
**Ngày cập nhật:** 31/10/2026  
**Trạng thái tài liệu:** `PROPOSED` (Tương thích hoàn toàn Unity 6 Universal Render Pipeline)  
**Tài liệu cha:** [CHARACTER_ART_BIBLE.md](../CHARACTER_ART_BIBLE.md)  
**Tác giả:** Technical Artist & Senior Texture Artist  

---

## MỤC LỤC
1. [Triết Lý Thẩm Mỹ Texture Vẽ Tay (Hand-Painted Stylized Aesthetic)](#1-triết-lý-thẩm-mỹ-texture-vẽ-tay-hand-painted-stylized-aesthetic)
2. [Chiến Lược Texture Atlas và Độ Phân Giải (Resolution Strategy)](#2-chiến-lược-texture-atlas-và-độ-phân-giải-resolution-strategy)
3. [Cấu Trúc Các Kênh Texture URP Lit (Texture Channel Packing)](#3-cấu-trúc-các-kênh-texture-urp-lit-texture-channel-packing)
4. [Kỹ Thuật Nướng Ánh Sáng Giả Lập Vào Base Color (Gradient Lighting Bake)](#4-kỹ-thuật-nướng-ánh-sáng-giả-lập-vào-base-color-gradient-lighting-bake)
5. [Đặc Tả 10 Chất Liệu Biểu Tượng Của Thế Giới Đại Ngàn](#5-đặc-tả-10-chất-liệu-biểu-tượng-của-thế-giới-đại-ngàn)
   - 5.1. Giáp Bạc Sáng (Silver Armor)
   - 5.2. Đồng Thau Đông Sơn (Dong Son Bronze)
   - 5.3. Quặng Huyền Thiết / Sắt Đen (Dark Iron)
   - 5.4. Da Bò Sấy Khói & Nhuộm Điều (Weathered Leather)
   - 5.5. Vải Thô, Dây Gai & Lụa Cổ (Coarse Cloth & Silk)
   - 5.6. Lông Sói Tuyết & Bờm Dã Thú (Wolf Fur)
   - 5.7. Xương Thú & Nanh Vuốt Biến Dị (Ancient Bone)
   - 5.8. Tre Nứa, Gỗ Lim & Trầm Hương (Bamboo & Hardwood)
   - 5.9. Đá Suối & Thạch Nhũ Hang Karst (River Stone & Stalactite)
   - 5.10. Tinh Thể Ngũ Linh Thạch (Magical Elemental Crystals)
6. [Tương Thích Shader URP và Tối Ưu Hóa SRP Batcher](#6-tương-thích-shader-urp-và-tối-ưu-hóa-srp-batcher)
7. [Chuẩn Nén Texture Đa Nền Tảng (Texture Compression Standards)](#7-chuẩn-nén-texture-đa-nền-tảng-texture-compression-standards)
8. [Đặc Tả Bộ Kênh Texture Cho Nhân Vật Mẫu "Bạch Lang Kiếm Sĩ"](#8-đặc-tả-bộ-kênh-texture-cho-nhân-vật-mẫu-bạch-lang-kiếm-sĩ)

---

## 1. TRIẾT LÝ THẨM MỸ TEXTURE VẼ TAY (HAND-PAINTED STYLIZED AESTHETIC)

Phong cách texture của **Huyết Nguyệt Đại Ngàn** là sự kết hợp giữa **Nghệ thuật vẽ tay truyền thống (Hand-Painted Artisan)** và **Mô hình chiếu sáng PBR cách điệu của Unity 6 URP**:

1. **Khối lượng thông tin thị giác do cọ vẽ quyết định:** $70\%$ chi tiết bóng tối, rãnh khắc hoa văn, vết xước kim loại và độ phai màu của da thịt được vẽ trực tiếp vào Texture Base Color.
2. **Không lạm dụng Shader phức tạp:** Tránh các shader đa lớp (Multi-layer shaders) nặng nề gây sụt giảm khung hình. Mô hình nhân vật trông vẫn đẹp mắt và đầy đủ chiều sâu khối ngay cả khi tắt toàn bộ nguồn sáng thời gian thực trong scene (*Unlit Flat View*).
3. **Màu sắc giàu cảm xúc (Color Vibrancy with Grit):** Màu sắc không bị xỉn đục như game tả thực quân sự, nhưng cũng không bóng loáng đồ chơi nhựa. Vải có vết phai nắng, kim loại có vết ố rỉ đồng xanh (*Verdigris Patina*), da thịt có độ ửng hồng sinh mệnh.

---

## 2. CHIẾN LƯỢC TEXTURE ATLAS VÀ ĐỘ PHÂN GIẢI (RESOLUTION STRATEGY)

Nhằm tối ưu hóa số lệnh gọi vẽ (*Draw Calls*) và bộ nhớ VRAM ([ADR-12](../../architecture/TECHNICAL_DECISIONS.md)):

```
+-------------------------------------------------------------------------+
|                  CHIẾN LƯỢC PHÂN BỔ TEXTURE ATLAS (ATLAS MAP)           |
+-------------------------------------------------------------------------+
| MASTER ATLAS SHEET: 2048 x 2048 (Dùng chung cho toàn bộ Nhân Tộc)       |
| +-----------------------------------+---------------------------------+ |
| | [ GÓC 1: ĐẦU, MẶT, TÓC & RÂU ]    | [ GÓC 2: GIÁP NGỰC & THÂN ÁO ]  | |
| | - Độ phân giải: 1024 x 1024       | - Độ phân giải: 1024 x 1024     | |
| | - Texel Density: 15.36 px/cm      | - Texel Density: 12.00 px/cm    | |
| +-----------------------------------+---------------------------------+ |
| | [ GÓC 3: QUẦN GIÁP & ỦNG CHÂN ]   | [ GÓC 4: GĂNG TAY, VAI & PHỤ ]  | |
| | - Độ phân giải: 1024 x 1024       | - Độ phân giải: 1024 x 1024     | |
| | - Texel Density: 10.24 px/cm      | - Áo choàng lông, đai lưng      | |
| +-----------------------------------+---------------------------------+ |
+-------------------------------------------------------------------------+
```

### Bảng Định Mức Kích Thước Texture Theo Nhóm Tài Nguyên:
| Nhóm Tài Nguyên | Kích Thước Bản Vẽ Chính (PC Profile) | Kích Thước Bản Vẽ (Mobile/Low Profile) | Ghi Chú |
| :--- | :--- | :--- | :--- |
| **Nhân Vật Người Chơi (Master Sheet)**| $2048 \times 2048$ | $1024 \times 1024$ | Gộp chung Đầu, Thân, Chân, Tay |
| **Vũ Khí Cận Chiến (Kiếm, Rìu)**| $1024 \times 1024$ (hoặc $512 \times 1024$) | $512 \times 512$ | Chung cho 2-3 loại vũ khí cùng nhóm |
| **Khiên Đỡ & Nón Lá** | $512 \times 512$ | $256 \times 256$ | Tài nguyên phụ trợ gắn ngoài |
| **Sói Bậc 1 (Quái Thường - 20 con)**| $1024 \times 1024$ | $512 \times 512$ | Toàn bộ lông, da, răng nanh |
| **Lợn Rừng Trường Sơn** | $1024 \times 1024$ | $512 \times 512$ | Dã thú săn bắn |
| **Sói Vua Tier 9 (Boss Duy Nhất)** | $2048 \times 2048$ | $1024 \times 1024$ | Độc quyền cho boss thế giới |

---

## 3. CẤU TRÚC CÁC KÊNH TEXTURE URP LIT (TEXTURE CHANNEL PACKING)

Để giảm thiểu băng thông nạp texture (Texture Memory Bandwidth), dự án sử dụng cấu trúc **3 Texture Maps tối giản** cho mỗi nhân vật:

```
[ MAP 1: _BaseMap (Albedo + Alpha) ]
- RGB: Màu sắc vẽ tay đã nướng dải sáng môi trường (Ambient Light Bake)
- A (Alpha): Vệt trong suốt (Alpha Cutout) cho riềm áo rách hoặc lọn tóc

[ MAP 2: _BumpMap (Normal Map) ]
- RGB: Bản đồ pháp tuyến nướng từ High-Poly Sculpt hoặc vẽ tay chi tiết lớn
- Mục đích: Tạo độ nảy gờ vảy đồng, sống mũi kiếm và thớ vải dày

[ MAP 3: _MaskMap (Packed Channels - Tối Ưu) ]
- Kênh R (Red): Metallic (Độ kim loại: 0.0 cho vải/da, 0.85 cho bạc/đồng)
- Kênh G (Green): Occlusion (Bóng tối tự thân bổ sung)
- Kênh B (Blue): Detail Mask / Subsurface Scattering Mask (Vùng ửng da)
- Kênh A (Alpha): Smoothness (Độ nhẵn bóng bề mặt: Đảo ngược của Roughness)

[ MAP 4: _EmissionMap (Chỉ bật khi có ma thuật / Mắt Sói) ]
- RGB: Màu phát sáng rực rỡ + Cường độ (Intensity 1.0 - 5.0)
```

---

## 4. KỸ THUẬT NƯỚNG ÁNH SÁNG GIẢ LẬP VÀO BASE COLOR (GRADIENT LIGHTING BAKE)

Để nhân vật đạt hiệu ứng 3D khối nổi bật rõ ràng dưới góc nhìn Isometric $16\text{m}$, nghệ sĩ texture phải tuân thủ công thức nướng dải màu trên Base Map:

```
    CÔNG THỨC ÁNH SÁNG GIẢ LẬP TRÊN BASE COLOR
           ÁNH SÁNG TRỜI (Top Light - Tone Lạnh Nhẹ)
                     |  |  |
                  \  v  v  v  /
                   [ Đỉnh Đầu ]   -> Sáng nhất (100% Value, hơi ngả vàng ấm)
                   [  Bờ Vai  ]   -> Sáng mạnh (90% Value)
                   [  Lồng Ngực]  -> Trung tính (75% Value)
                   [  Đai Lưng]   -> Chuyển tối (60% Value)
                   [  Bắp Chân]   -> Tối dần (45% Value)
                   [ Bàn Chân ]   -> Đậm màu đất (30% Value, ám nâu ẩm)
                  /  ^  ^  ^  \
                     |  |  |
           PHẢN QUANG ĐẤT (Bounce Light - Tone Ấm Nâu)
```

1. **Dải sáng trên xuống (Vertical Gradient):** Màu của nhân vật từ đỉnh đầu xuống chân luôn có độ sáng giảm dần từ $100\% \to 60\%$.
2. **Viền sáng góc cạnh (Hand-Painted Edge Highlights):** Từng mép góc của giáp đồng, lưỡi kiếm, và vành nón lá được viền một nét cọ sáng hơn $20\%$ để tạo độ "bật khối" (*Pop Out*) khỏi cảnh nền.
3. **Bóng ấm - Sáng lạnh (Warm Darks, Cool Lights):** Vùng bóng tối trong nếp gấp vải và gầm giáp được pha sắc xanh rêu sẫm hoặc tím than; vùng đón sáng pha ánh vàng ấm của nắng rừng nhiệt đới.

---

## 5. ĐẶC TẢ 10 CHẤT LIỆU BIỂU TƯỢNG CỦA THẾ GIỚI ĐẠI NGÀN

### 5.1. Giáp Bạc Sáng (Silver Armor — Tiêu biểu trên Bạch Lang Kiếm Sĩ)
- **Base Color:** Trắng bạc ánh xanh lơ nhẹ ($RGB 210, 215, 225$), rãnh khắc hoa văn màu xám khói ($RGB 70, 75, 85$).
- **Metallic / Smoothness:** Metallic = $0.85$, Smoothness = $0.65$ (Bóng mờ, không sắc như gương).
- **Đặc trưng:** Vết xước li ti xước dăm dọc theo thớ kim loại, không có rỉ sét đỏ.

### 5.2. Đồng Thau Đông Sơn (Dong Son Bronze)
- **Base Color:** Vàng đồng sẫm cổ kính ($RGB 180, 130, 60$), các rãnh chìm có vết ố mốc đồng xanh (*Verdigris Patina*: $RGB 45, 120, 100$).
- **Metallic / Smoothness:** Metallic = $0.75$, Smoothness = $0.45$ (Bề mặt hơi thô ráp do đúc cát cổ truyền).
- **Họa tiết:** Mặt trời 12 tia nảy sáng trên nền đồng hun tối.

### 5.3. Quặng Huyền Thiết / Sắt Đen (Dark Iron)
- **Base Color:** Xám đen than chì ($RGB 35, 35, 40$), vết ghè búa của thợ rèn Người Lùn để lại vết nảy sáng xanh thép.
- **Metallic / Smoothness:** Metallic = $0.60$, Smoothness = $0.35$. Rất lì khối, thể hiện sức nặng phòng thủ.

### 5.4. Da Bò Sấy Khói & Nhuộm Điều (Weathered Leather)
- **Base Color:** Nâu đỏ sẫm ($RGB 115, 40, 30$), các nếp nhăn và mép mép gấp có màu nâu cháy xém.
- **Metallic / Smoothness:** Metallic = $0.0$, Smoothness = $0.25 - 0.35$ (Bề mặt da thuộc bóng dầu nhẹ ở các chỗ cọ xát).

### 5.5. Vải Thô, Dây Gai & Lụa Cổ (Coarse Cloth & Silk)
- **Vải Thô / Bao Tải:** Sợi dệt thưa, màu cháo lòng ngả vàng đất ($RGB 190, 180, 160$). Metallic = $0.0$, Smoothness = $0.10$ (Nhám hoàn toàn).
- **Lụa Tơ Tằm Cổ:** Mịn màng, óng ả, đổi màu nhẹ theo góc nhìn (*Fresnel Sheen*). Metallic = $0.0$, Smoothness = $0.50$.

### 5.6. Lông Sói Tuyết & Bờm Dã Thú (Wolf Fur)
- **Base Color:** Chân lông màu xám tro tối ($RGB 80, 85, 90$), thân lông trắng ngà ($RGB 230, 230, 235$), ngọn lông điểm xuyết hạt bụi rừng.
- **Metallic / Smoothness:** Metallic = $0.0$, Smoothness = $0.20$.
- **Vẽ cọ:** Từng dải lông được vuốt cọ mềm mại, tạo cảm giác dày dặn, ấm áp chống chọi bão tuyết.

### 5.7. Xương Thú & Nanh Vuốt Biến Dị (Ancient Bone)
- **Base Color:** Vàng xương cũ sần sùi ($RGB 215, 205, 180$), chân móng vuốt ngả màu đỏ thẫm đọng máu khô lâu ngày.
- **Metallic / Smoothness:** Metallic = $0.0$, Smoothness = $0.40$ (Ngà bóng do mài dũa tự nhiên).

### 5.8. Tre Nứa, Gỗ Lim & Trầm Hương (Bamboo & Hardwood)
- **Tre Nứa:** Xanh ngọc vỏ tre ($RGB 120, 145, 75$) chuyển vàng rơm ở mắt gióng tre. Nhẵn bóng tự nhiên (Smoothness = $0.55$).
- **Gỗ Lim Cổ Thụ:** Nâu hạt dẻ sẫm ($RGB 60, 40, 25$), thớ gỗ chạy dọc thẳng tắp, có vết nứt do búa chặt.

### 5.9. Đá Suối & Thạch Nhũ Hang Karst (River Stone & Stalactite)
- **Base Color:** Xám rêu ẩm ướt ($RGB 95, 105, 95$), có vệt đốm trắng của tinh thể thạch anh lắng đọng.
- **Metallic / Smoothness:** Metallic = $0.0$, Smoothness = $0.15$ (Chỗ khô) và $0.70$ (Chỗ đọng nước sình lầy).

### 5.10. Tinh Thể Ngũ Linh Thạch (Magical Elemental Crystals)
- **Chất liệu trong mờ giả lập:** Không dùng shader tán xạ ánh sáng kính thật (*Glass Refraction*) vì quá nặng. Dùng texture Albedo có độ tương phản cực đại, đường viền sắc như dao cạo kết hợp Emission phát sáng nhịp nhàng (*Pulse Bloom*):
  - **Hỏa Thạch:** Cam rực tâm vàng ($Emission: RGB 255, 80, 0 \times 3.0$).
  - **Hàn Thạch:** Băng lam ngọc ($Emission: RGB 0, 240, 255 \times 2.5$).

---

## 6. TƯƠNG THÍCH SHADER URP VÀ TỐI ƯU HÓA SRP BATCHER

Nhằm đạt chuẩn tương thích với cơ chế tối ưu hóa **SRP Batcher** và **GPU Resident Drawer** của Unity 6:

1. **Tuyệt đối không đổi tên thuộc tính Material ngẫu nhiên:**
   Mọi Shader Graph Stylized của dự án phải sử dụng đúng tên thuộc tính chuẩn của Unity URP:
   - `_BaseMap` (Texture2D) & `_BaseColor` (Color)
   - `_BumpMap` (Texture2D) & `_BumpScale` (Float)
   - `_Metallic` (Float) & `_Smoothness` (Float)
   - `_EmissionMap` (Texture2D) & `_EmissionColor` (Color)
2. **Quy tắc Batching:**
   - Khi 6 người chơi và 10 NPC đứng chung trong một khung hình, nếu tất cả cùng sử dụng Shader `URP/Lit` hoặc cùng một `Shader Graph Character Master`, Unity 6 sẽ gom toàn bộ lệnh vẽ vào duy nhất **1 SRP Batch** liên tục, giảm thiểu gánh nặng CPU xuống mức tối đa.

---

## 7. CHUẨN NÉN TEXTURE ĐA NỀN TẢNG (TEXTURE COMPRESSION STANDARDS)

Để tránh hiện tượng vỡ khối màu hoặc mờ nhòe trên các cấu hình máy khác nhau:

```
[ THIẾT LẬP NÉN TEXTURE TRONG UNITY INSPECTOR ]
- Texture Type: Default
- Texture Shape: 2D
- sRGB (Color Texture): BẬT cho BaseMap & EmissionMap / TẮT cho NormalMap & MaskMap!
- Alpha Source: Input Texture Alpha
- Alpha Is Transparency: Bật nếu có riềm trong suốt
- Generate Mip Maps: BẬT (Checked)
  - Mip Map Filtering: Kaiser (Giữ độ sắc nét cho texture stylized)

[ ĐỊNH DẠNG NÉN (COMPRESSION FORMAT) ]
- Nền tảng PC (Windows Standalone Build):
  - BaseMap / MaskMap: BC7 (Chất lượng đồ họa cao, không bị bệt màu gradient)
  - NormalMap: DXT5nm hoặc BC5
- Nền tảng Mobile Profile (Dự phòng):
  - ASTC 6x6 hoặc ASTC 8x8 block format
```

---

## 8. ĐẶC TẢ BỘ KÊNH TEXTURE CHO NHÂN VẬT MẪU "BẠCH LANG KIẾM SĨ"

```
+-------------------------------------------------------------------------+
|     BỘ TÀI NGUYÊN TEXTURE: CH_Human_Hero_SilverWolfKnight               |
+-------------------------------------------------------------------------+
| 1. T_CH_SilverWolfKnight_Body_BaseMap.png (2048 x 2048 - sRGB)          |
|    - Màu da hồng hào sương gió, tóc bạc trắng muốt viền cọ xám.         |
|    - Giáp ngực bạc bóng sáng với hộ tâm kính mặt trời Đông Sơn.         |
|    - Thân áo da bò nhuộm đỏ sẫm viền chỉ vàng chìm.                     |
|    - Bờm cổ áo lông sói tuyết vuốt từng lọn sợi sắc sảo.                |
|                                                                         |
| 2. T_CH_SilverWolfKnight_Body_Normal.png (2048 x 2048 - Linear)         |
|    - Nổi bật từng gờ vảy rồng trên giáp vai bạc.                        |
|    - Độ gợn sóng nhấp nhô của khối lông thú áo choàng.                  |
|    - Đường rãnh may da thuộc của ống xà cạp cẳng chân.                  |
|                                                                         |
| 3. T_CH_SilverWolfKnight_Body_MaskMap.png (2048 x 2048 - Linear)        |
|    - Kênh R (Metallic): Trắng sáng ở giáp bạc (0.85), Đen ở áo lông/da. |
|    - Kênh G (AO): Đậm tối ở gầm giáp vai và cổ áo.                      |
|    - Kênh B (Detail): Mặt trời Đông Sơn trước ngực.                     |
|    - Kênh A (Smoothness): Mịn mờ ở giáp bạc (0.65), sần nhám ở lông thú.|
|                                                                         |
| 4. T_WP_Sword_SacredSilver_Emission.png (1024 x 1024 - sRGB)           |
|    - Rãnh sống kiếm phát hào quang vàng kim rực rỡ (RGB: 255, 215, 0).  |
|    - Mắt chim Lạc trên đốc kiếm ánh hào quang hổ phách nhẹ.             |
+-------------------------------------------------------------------------+
```

