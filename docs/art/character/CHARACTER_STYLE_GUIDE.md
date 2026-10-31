# HƯỚNG DẪN ĐỊNH HƯỚNG PHONG CÁCH MỸ THUẬT NHÂN VẬT
## (CHARACTER STYLE GUIDE — HUYẾT NGUYỆT ĐẠI NGÀN)

---

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mã tài liệu:** `CAB-SPEC-01`  
**Phiên bản:** `1.0.0`  
**Ngày cập nhật:** 31/10/2026  
**Trạng thái tài liệu:** `PROPOSED` (Chờ phê duyệt từ Art Director)  
**Tài liệu cha:** [CHARACTER_ART_BIBLE.md](../CHARACTER_ART_BIBLE.md)  
**Tác giả:** Art Director & Senior Character Concept Artist  

---

## MỤC LỤC
1. [Tầm Nhìn Mỹ Thuật Tổng Quan](#1-tầm-nhìn-mỹ-thuật-tổng-quan)
2. [Cảm Hứng Albion-Inspired Nhưng Khác Biệt Hóa](#2-cảm-hứng-albion-inspired-nhưng-khác-biệt-hóa)
3. [Tỷ Lệ Cơ Thể và Giải Phẫu Stylized (Stylized Anatomy)](#3-tỷ-lệ-cơ-thể-và-giải-phẫu-stylized-stylized-anatomy)
4. [Nguyên Tắc Cường Điệu Hình Khối (Exaggeration vs Simplification)](#4-nguyên-tắc-cường-điệu-hình-khối-exaggeration-vs-simplification)
5. [Quy Chuẩn Thiết Kế Từng Thành Phần Nhân Vật](#5-quy-chuẩn-thiết-kế-từng-thành-phần-nhân-vật)
   - 5.1. Khuôn mặt và Biểu cảm
   - 5.2. Mái tóc và Râu
   - 5.3. Giáp trụ và Y phục
   - 5.4. Áo choàng và Khăn choàng
   - 5.5. Lông thú và Da thú
   - 5.6. Binh khí và Khiên chắn
6. [Hệ Thống Hoa Văn và Biểu Tượng Văn Hóa Việt Cổ](#6-hệ-thống-hoa-văn-và-biểu-tượng-văn-hóa-việt-cổ)
7. [Bảng Màu Chủ Đạo (Color Palette Architecture)](#7-bảng-màu-chủ-đạo-color-palette-architecture)
   - 7.1. Bảng màu theo Phe Phái (Faction)
   - 7.2. Bảng màu theo Biome Môi Trường
   - 7.3. Bảng màu Ngũ Linh Thạch
8. [Phân Cấp Thị Giác Theo Thứ Bậc Nhân Vật (Visual Hierarchy)](#8-phân-cấp-thị-giác-theo-thứ-bậc-nhân-vật-visual-hierarchy)
9. [Kiểm Thử Khả Năng Nhận Diện Ở Góc Nhìn Isometric (Camera Readability)](#9-kiểm-thử-khả-năng-nhận-diện-ở-góc-nhìn-isometric-camera-readability)
10. [Nghiên Cứu Điển Hình: Nhân Vật Mẫu "Bạch Lang Kiếm Sĩ" (Silver Wolf Knight)](#10-nghiên-cứu-điển-hình-nhân-vật-mẫu-bạch-lang-kiếm-sĩ-silver-wolf-knight)
11. [Điều Cấm Kỵ Trong Thiết Kế (Art Direction Don'ts)](#11-điều-cấm-kỵ-trong-thiết-kế-art-direction-donts)

---

## 1. TẦM NHÌN MỸ THUẬT TỔNG QUAN

Thế giới **Huyết Nguyệt Đại Ngàn** được xây dựng trên ngôn ngữ thị giác **Stylized 3D Hand-Painted**, mang không khí u tối, ngột ngạt của huyền thoại cổ xưa giao thoa với sinh tồn khắc nghiệt. 

Thẩm mỹ của game không phải là phong cách hoạt hình thiếu nhi sặc sỡ (*Cartoon/Casual*), cũng không phải phong cách siêu tả thực bóng bẩy (*Photorealistic PBR*). Đó là vẻ đẹp **Mộc Mạc, Đậm Đà Bản Sắc, Chắc Khối và Giàu Tính Kể Chuyện**.

---

## 2. CẢM HỨNG ALBION-INSPIRED NHƯNG KHÁC BIỆT HÓA

### 2.1. Tiếp thu từ Albion Online (Kế thừa nền tảng kỹ thuật và cấu trúc gameplay)
- **Cấu trúc hình khối lớn (Chunky Form Language):** Mọi chi tiết giáp, vũ khí, cánh tay đều được thiết kế đậm đà để nổi bật rõ rệt dưới góc máy Isometric $45^\circ - 55^\circ$.
- **Vật liệu vẽ tay kết hợp PBR tối giản:** Giảm thiểu phản chiếu phức tạp, tập trung vào ánh sáng giả lập nướng sẵn trên chất liệu sơn tay (*Hand-painted lighting bake*).
- **Hệ thống giáp dạng Modun rõ ràng:** Đầu, Thân, Tay, Chân, Vũ khí tách biệt để người chơi tự do phối hợp trang bị.

### 2.2. Khác biệt hóa tuyệt đối bản sắc Huyết Nguyệt Đại Ngàn

| Yếu Tố Thị Giác | Albion Online (Phương Tây) | Huyết Nguyệt Đại Ngàn (Đại Ngàn Việt Cổ) |
| :--- | :--- | :--- |
| **Văn hóa gốc** | Châu Âu Trung Cổ, Hiệp sĩ Arthur, Giáp tấm sắt bóng | Cổ phong Việt Nam, Văn hóa Đông Sơn, Thần thoại Lạc Việt |
| **Chất liệu giáp** | Thép rèn đe, xích sắt châu Âu, giáp hiệp sĩ toàn thân | Đồng thau Đông Sơn, vảy da trâu sấy khói, nẹp tre nứa, giáp lá cọ |
| **Binh khí** | Trường kiếm Broadsword, Đại búa Warhammer, Nỏ sắt | Trường kiếm đốc đồng nạm ngọc, Mã tấu Trường Sơn, Cung tre cánh trâu |
| **Họa tiết** | Chữ thập, Phù hiệu hiệp sĩ (Heraldry), Đại bàng La Mã | Mặt trời 12/14 tia, Chim Lạc cách điệu, Vòng đồng tâm, Vân mây, Sóng nước |
| **Không khí thế giới**| Fantasy huyền ảo rực rỡ, thảo nguyên xanh ngát sáng sủa | U uất, rừng thiêng nước độc, sương lam khói bùa, Huyết Nguyệt rợn ngợp |

---

## 3. TỶ LỆ CƠ THỂ VÀ GIẢI PHẪU STYLIZED (STYLIZED ANATOMY)

```
        TỶ LỆ NHÂN TỘC NAM (1 : 6.8 ĐẦU)
             [ 0.0m - 1.80m ]
    
    1/7  [ (o_o) ]  -> Đầu & Mặt (Tóc bạc, râu xám)
    2/7  [  /  \  ]  -> Giáp vai vuông vắn nhô cao
    3/7  [ [==]  ]  -> Giáp ngực hộ tâm Đông Sơn (Thân nở rộng)
    4/7  [  \__/ ]  -> Thắt lưng da & Khóa đồng
    5/7  [  /  \  ]  -> Vạt giáp hông & Đùi cơ bắp
    6/7  [  ||  ]  -> Xà cạp bó chân & Đầu gối bảo hộ
    7/7  [ _/\_ ]  -> Giày da lội rừng (Đế dầy vững chãi)
```

- **Quy tắc tỷ lệ đầu/thân:**
  - **Nhân Tộc Nam (Chiến binh / Tiêu chuẩn):** $1 : 6.8$ (Chiều cao $1.80\text{m}$). Vai rộng gấp 2.3 lần chiều rộng đầu.
  - **Nhân Tộc Nữ (Thợ săn / Pháp sư):** $1 : 6.5$ (Chiều cao $1.68\text{m}$). Khung người thon thả hơn, eo cao, tà áo thướt tha.
  - **Người Lùn (Dwarf Craftsman):** $1 : 4.5$ (Chiều cao $1.25\text{m}$). Lồng ngực dày, cánh tay dài gần chạm gối, râu rậm che kín cổ.
  - **Người Sói Dạng Quái Thú (Tier 3+):** $1 : 5.5$ (Chiều cao $2.50\text{m}$). Lưng gập, cơ vai nở gấp 3 lần đầu, chân khớp dã thú (Digitigrade).

---

## 4. NGUYÊN TẮC CƯỜNG ĐIỆU HÌNH KHỐI (EXAGGERATION VS SIMPLIFICATION)

Trong góc nhìn Isometric 3/4 từ xa, mắt người chơi chỉ tiếp nhận các mảng khối lớn. Do đó áp dụng nghiêm ngặt nguyên tắc sau:

### 4.1. Những chi tiết ĐƯỢC PHÉP CƯỜNG ĐIỆU (Phóng to $115\% - 130\%$)
- **Giáp vai (Pauldrons):** Nhô cao, đường nét sắc cạnh hoặc vát chéo dũng mãnh để tạo khung hình chữ V cho chiến binh.
- **Bàn tay và Găng tay:** Phóng to $120\%$ kích thước thực tế để làm rõ động tác nắm chuôi kiếm, kéo dây cung hoặc kết ấn niệm chú.
- **Vũ khí và Cổ kiếm:** Lưỡi kiếm rộng hơn thực tế $20\%$, chuôi kiếm dài và hoa văn cán kiếm dập nổi lớn.
- **Bờm cổ áo và Lông thú (Fur Collar):** Phồng dày, xù xì, gợn sóng rõ rệt để tăng vẻ hoang dã sinh tồn.
- **Bàn chân và Giày ống:** To bản $115\%$ tạo cảm giác nhân vật cắm rễ vững chắc xuống mặt đất.

### 4.2. Những chi tiết BẮT BUỘC GIẢN LƯỢC (Simplification Rules)
- **Không dựng lỗ xỏ chỉ, đường kim khâu li ti:** Toàn bộ đường chỉ may quần áo được giản lược thành các đường nét vẽ cọ trên texture, không tốn đa giác hình học.
- **Không dựng dây xích sắt mắt xích nhỏ:** Mắt xích kim loại dạng vòng tròn nhỏ bị cấm hoàn toàn vì gây hiện tượng răng cưa méo lưới (Aliasing/Moire) từ xa.
- **Không dựng sợi tóc đơn lẻ:** Tóc phải được gộp thành từng lọn khối lớn (*Clumpy stylized hair strands*).
- **Không dựng gai nhọn vi mô trên giáp:** Gai nhọn nhỏ li ti làm vỡ viền Silhouette; chỉ giữ lại các mấu xương lớn hoặc ngấn đồng rõ nét.

---

## 5. QUY CHUẨN THIẾT KẾ TỪNG THÀNH PHẦN NHÂN VẬT

### 5.1. Khuôn mặt và Biểu cảm
- **Đặc điểm diện mạo:** Đậm nét Á Đông / Lạc Việt: Xương gò má rõ, sống mũi thẳng và chắc, mắt phượng hoặc mắt hổ cương nghị.
- **Biểu cảm gốc:** Nghiêm nghị, cảnh giác, khắc khổ vì sinh tồn. Tránh các biểu cảm tươi cười ngây thơ không phù hợp bối cảnh thế giới Huyết Nguyệt.
- **Râu và Vết sẹo:** Chiến binh dạn dày có râu quai nón tỉa gọn hoặc râu dài xám bạc, vết sẹo cào xước do nanh sói trên má hoặc trán để kể câu chuyện quá khứ.

### 5.2. Mái tóc và Râu
- Tạo hình theo từng mảng khối điêu khắc (*Sculpted Hair Clumps*).
- Đầu ngọn tóc vát cạnh sắc nét, rủ tự nhiên che nhẹ một phần trán hoặc buộc búi tó củ hành cổ truyền phía sau gáy.
- Màu tóc phản ánh tuổi tác và trạng thái: Đen huyền truyền thống, muối tiêu sương gió, hoặc trắng bạc ma mị (như nhân vật tham chiếu Bạch Lang Kiếm Sĩ).

### 5.3. Giáp trụ và Y phục
- **Giáp vảy cá / Vảy rồng (Scale Armor):** Lấy cảm hứng từ giáp đồng cổ Đông Sơn. Từng phiến đồng/bạc hình bán nguyệt xếp so le, mép vảy hơi cong tạo độ nảy ánh sáng.
- **Tấm hộ tâm (Chest Plate/Mirror):** Hình tròn hoặc ô-van lớn đặt giữa ngực, khắc họa tiết mặt trời tỏa tia.
- **Đai lưng (Sash & Belt):** Dải lụa hoặc da dày quấn nhiều vòng quanh eo, gắn khóa đồng đúc hình đầu hổ hoặc mặt quỷ phù đồ hộ mệnh.

### 5.4. Áo choàng và Khăn choàng
- Tà áo choàng không rơi thẳng tuột nhàm chán mà có độ xòe nhẹ góc $15^\circ$ về phía sau.
- Gấu áo rách tưa hoặc viền lông thú sờn rách chứng tỏ trải qua nhiều đợt bão tuyết và chiến trận.
- Khi nhân vật di chuyển, tà áo có độ trễ vung vẩy (*Secondary motion*) tăng tính sống động.

### 5.5. Lông thú và Da thú
- Không dùng công nghệ hạt lông Hair/Fur Particle thời gian thực vì cực kỳ tốn hiệu năng.
- Sử dụng các lớp vỏ đa giác xếp tầng vát nhọn (*Layered Shell Alpha/Mesh Geometry*) kết hợp texture vẽ tay phân lớp tối ở chân lông và sáng ở ngọn lông.
- Lông sói xám tuyết, da lợn rừng, da hổ sọc vàng là các chất liệu biểu tượng.

### 5.6. Binh khí và Khiên chắn
- **Trường Kiếm Sắt Ấp Làng:** Lưỡi kiếm thẳng, thon dài nhưng đầm bản, chuôi kiếm bọc da cá đuối hoặc da thú quấn dây gai, đốc kiếm đúc đồng hình mắt chim Lạc.
- **Vũ khí truyền thống:** Mã tấu bản rộng chặt cây phạt cỏ, Cung tre uốn cong bọc sừng trâu, Nỏ thần cơ quan chốt ngậm tên bằng đồng.
- **Độ phát quang (Glow):** Vũ khí cấp cao phát ra ánh sáng mờ ảo ở rãnh huyết kiếm hoặc đá khảm (như kiếm ánh vàng kim của Bạch Lang Kiếm Sĩ).

---

## 6. HỆ THỐNG HOA VĂN VÀ BIỂU TƯỢNG VĂN HÓA VIỆT CỔ

```
+-------------------------------------------------------------------+
|               KHO TÀNG HỌA TIẾT ĐẠI NGÀN ĐÔNG SƠN                 |
+-------------------------------------------------------------------+
| 1. MẶT TRỜI ĐÔNG SƠN:                                              |
|    - Ngôi sao 12 hoặc 14 cánh nhọn ở tâm tấm giáp hộ tâm         |
|    - Tượng trưng cho ánh sáng xua tan ma quỷ và lời nguyền        |
+-------------------------------------------------------------------+
| 2. CHIM LẠC BAY NGƯỢC CHIỀU KIM ĐỒNG HỒ:                          |
|    - Khắc chìm dọc theo rãnh kiếm, viền khiên hoặc dải đai lưng   |
|    - Tượng trưng cho nguồn cội Lạc Hồng bất diệt                  |
+-------------------------------------------------------------------+
| 3. HỌA TIẾT RĂNG CƯA & CHỮ GÃY (GÉOMÉTRIQUE):                     |
|    - Sử dụng trên viền xà cạp, cổ tay áo giáp và viền cán búa     |
|    - Tạo nhịp điệu thị giác khỏe khoắn, góc cạnh                  |
+-------------------------------------------------------------------+
| 4. HOA VĂN SÓNG NƯỚC & VÂN MÂY CUỘN:                              |
|    - Khắc trên giáp vai của Pháp Sư hoặc phù hiệu Đầm Lầy         |
|    - Thể hiện sự luân chuyển nguyên tố mềm mại                    |
+-------------------------------------------------------------------+
```

---

## 7. BẢNG MÀU CHỦ ĐẠO (COLOR PALETTE ARCHITECTURE)

### 7.1. Bảng màu theo Phe Phái (Faction Colors)

```
[ NHÂN TỘC / DÂN LÀNG ]
- Nâu Đất Nung:    #8B4513 (Primary)
- Xanh Tre Già:    #556B2F (Secondary)
- Vàng Đồng Hun:   #D4AF37 (Accent)
- Trắng Vải Thô:   #DCDCDC (Sub-tone)

[ PHE NGƯỜI SÓI / HUYẾT NGUYỆT ]
- Đỏ Huyết Nguyệt: #8B0000 (Primary Glow)
- Xám Tro Tàn:     #2F4F4F (Secondary)
- Bạc Lạnh Tuyết:  #C0C0C0 (Armor/Fur)
- Tím Độc Ma Quái: #4B0082 (Sub-tone)

[ TỘC NGƯỜI LÙN / LÒ RÈN ]
- Đen Quặng Sắt:   #1C1C1C (Primary)
- Đỏ Than Hồng:    #FF4500 (Secondary Glow)
- Đồng Đỏ Nguyên:  #B87333 (Accent)
- Vàng Rỉ Sét:     #8B5A2B (Leather)

[ PHÙ THỦY CỔ ĐẠI ]
- Xanh Rêu Sâu:    #2E8B57 (Good Witch)
- Tím Thẫm Độc:    #483D8B (Evil Swamp Witch)
- Nâu Vỏ Cây:      #3E2723 (Staff/Fabric)
```

### 7.2. Bảng màu theo Biome Môi Trường
1. **Biome 1 — Rừng Rậm Trường Sơn:** Xanh lục thẫm tán cây nhiệt đới ($#1E4D2B$), nâu đất ẩm ướt ($#4A3728$), ánh nắng vàng rọi qua kẽ lá ($#F4D03F$).
2. **Biome 2 — Đầm Lầy Ngập Mặn:** Xanh rêu đục sình lầy ($#3B5323$), xám sương mù độc ($#708090$), tím nhạt hoa lục bình ma quái ($#9370DB$).
3. **Biome 3 — Miền Trung Khô Hạn / Sa Mạc Cát:** Vàng cát khô bỏng ($#EDC9AF$), cam đất nung ($#CC5500$), xám đá vôi nứt nẻ ($#8B8589$).
4. **Biome 4 — Hang Động Karst:** Xanh lam thạch nhũ ngầm ($#1B4965$), đen quặng huyền thiết ($#141414$), xanh phát quang địa y ($#00FF7F$).
5. **Biome 5 — Núi Tuyết Hoàng Liên:** Trắng tuyết tinh khôi ($#F8F9FA$), xanh ngọc băng hà ($#A8DADC$), xanh sẫm bầu trời rét buốt ($#1D3557$).

### 7.3. Bảng màu Ngũ Linh Thạch
- **Hỏa Linh Thạch (5 viên):** Cam rực đỏ than cháy ($#FF4500$), tâm vàng rực ($#FFD700$).
- **Thủy Linh Thạch (4 viên):** Xanh dương sâu thẳm biển khơi ($#1E90FF$), bọt sóng trắng ($#E0FFFF$).
- **Mộc Linh Thạch (3 viên):** Xanh ngọc lục bảo sự sống ($#32CD32$), ánh xanh ngọc bích ($#00FA9A$).
- **Thổ Linh Thạch (2 viên):** Nâu hổ phách thạch anh ($#CD853F$), viền vàng kim sa ($#DAA520$).
- **Hàn Linh Thạch (1 viên duy nhất):** Lam ngọc băng giá siêu thực ($#00FFFF$), ánh tím hàn cực ($#E6E6FA$).

---

## 8. PHÂN CẤP THỊ GIÁC THEO THỨ BẬC NHÂN VẬT (VISUAL HIERARCHY)

Để đảm bảo người chơi có thể thẩm định ngay độ nguy hiểm của đối phương trên chiến trường:

```
+-------------------------------------------------------------------------+
|                  THỨ BẬC THỊ GIÁC (VISUAL HIERARCHY)                    |
+-------------------------------------------------------------------------+
| [ CẤP ĐỘ 1: DÂN THƯỜNG / SƠ KHỞI (TIER 1) ]                            |
| - Hình khối: Gọn gàng, bo tròn, ít góc nhọn.                            |
| - Màu sắc: Đơn sắc, gam màu đất mờ (Desaturated Earth Tones).          |
| - Vũ khí / Giáp: Gỗ, vải vụn, da mỏng. Hoàn toàn KHÔNG phát quang.     |
+-------------------------------------------------------------------------+
                                   v
+-------------------------------------------------------------------------+
| [ CẤP ĐỘ 2: CHIẾN BINH / QUÁI ĐẶC BIỆT (TIER 2 - 4) ]                   |
| - Hình khối: Mở rộng ngang (Vai giáp to, vạt áo xòe, cơ bắp cuồn cuộn).  |
| - Màu sắc: Độ tương phản cao, nẹp đồng đỏ, lông thú xù xì tương phản.   |
| - Điểm nhấn: Mặt giáp đúc nổi hoa văn, mắt ánh tia đỏ nhẹ ban đêm.      |
+-------------------------------------------------------------------------+
                                   v
+-------------------------------------------------------------------------+
| [ CẤP ĐỘ 3: THỦ LĨNH / BOSS SÓI VUA TIER 9 ]                           |
| - Hình khối: Khổng lồ (Gấp 1.8 - 2 lần người), góc cạnh nhọn hình chữ V.|
| - Màu sắc: Tương phản cực độ giữa giáp đen/bạc và ánh đỏ Huyết Nguyệt. |
| - Hiệu ứng đặc biệt: Rãnh nứt trên cơ thể phát hào quang, khói đen bao  |
|   quanh bàn chân, sừng/ngọc gai xương vươn cao chọc trời.              |
+-------------------------------------------------------------------------+
```

---

## 9. KIỂM THỬ KHẢ NĂNG NHẬN DIỆN Ở GÓC NHÌN ISOMETRIC (CAMERA READABILITY)

Mọi concept nhân vật trước khi chuyển sang khâu 3D Modeling bắt buộc phải trải qua bài kiểm tra **"3-Silhouette Greybox Test"**:

1. **Kiểm tra Phủ Đen Toàn Bộ (Pure Black Test):**
   - Đặt mô hình/concept vào nền xám trung tính ($RGB 128, 128, 128$).
   - Tô toàn bộ nhân vật thành màu đen kịt ($RGB 0, 0, 0$).
   - *Đạt yêu cầu:* Ở cự ly $16\text{m}$, vẫn phân biệt rõ đâu là đầu, đâu là tay cầm kiếm, hướng mặt nhìn về đâu, và loại hình giáp đang mặc.
2. **Kiểm tra Tương Phản Giá Trị Sáng Tối (Value Contrast Test):**
   - Chuyển màu toàn bộ nhân vật về chế độ Trắng Đen (Grayscale).
   - Tỷ lệ sáng tối phải tuân theo công thức: $50\%$ Vùng trung tính (Mid-tones) $\to 30\%$ Vùng tối đậm (Darks) $\to 20\%$ Vùng sáng rực (Highlights tập trung ở đầu, vai, ngực).
3. **Kiểm tra Hòa Lẫn Nền (Background Blend Test):**
   - Đặt nhân vật lên ảnh chụp màn hình thực tế của Rừng Rậm Trường Sơn và Đầm Lầy trong Unity.
   - Nhân vật không được phép bị "chìm nghỉm" vào bụi cỏ hay nền đất nhờ đường viền viền bóng tương phản nhẹ (*Subtle Rim Light / Contrast Outline*).

---

## 10. NGHIÊN CỨU ĐIỂN HÌNH: NHÂN VẬT MẪU "BẠCH LANG KIẾM SĨ" (SILVER WOLF KNIGHT)

Nhân vật tham chiếu được định danh kỹ thuật: **`CH_Human_Hero_SilverWolfKnight`**.

```
+-----------------------------------------------------------------------+
|          BẢN THIẾT KẾ MẪU: BẠCH LANG KIẾM SĨ (SILVER WOLF KNIGHT)      |
+-----------------------------------------------------------------------+
| ĐẶC ĐIỂM NHẬN DIỆN THỊ GIÁC:                                         |
| 1. Đầu & Tóc: Nam nhân trung niên (~35-40 tuổi), mái tóc dài màu     |
|    trắng bạc buộc nửa đầu, râu quai nón tỉa tót màu xám sương gió.    |
|    Ánh mắt cương nghị nhìn thẳng, sống mũi cao thẳng tắp.             |
|                                                                       |
| 2. Áo Choàng Lông Thú: Áo choàng da sói tuyết dày cộm quàng qua hai   |
|    vai, phần cổ áo dựng xù lông trắng muốt lấm tấm bụi sương. Mặt     |
|    trong áo lót lụa màu đỏ tía (Crimson Red) tạo sự vương giả.        |
|                                                                       |
| 3. Giáp Trụ:                                                          |
|    - Giáp vai: Phiến bạc chạm vảy rồng cách điệu, nẹp chỉ đồng đen.   |
|    - Giáp ngực: Hộ tâm kính bằng bạc sáng bóng, khắc mặt trời Đông Sơn.|
|    - Chi tiết da: Lớp áo giáp da bò sấy khói nhuộm đỏ sẫm (Dark Red)  |
|      ôm khít lồng ngực và cánh tay.                                   |
|    - Đai lưng: Da bản rộng thắt đai kim loại bạc đầu sói ngậm ngọc.   |
|                                                                       |
| 4. Binh Khí (Trường Kiếm Thần Sáng):                                 |
|    - Lưỡi kiếm bạc bản rộng, ở rãnh giữa phát hào quang vàng kim rực  |
|      rỡ (Holy Gold Glow) tượng trưng cho lưỡi kiếm được thợ rèn      |
|      Người Lùn tôi luyện với quặng lửa cổ.                           |
|    - Chuôi kiếm bọc da thuộc màu điều, đốc kiếm đầu chim Lạc ngậm ngọc.|
+-----------------------------------------------------------------------+
```

### Khả năng Kế Thừa Sản Xuất Từ Mẫu Bạch Lang Kiếm Sĩ:
- **Khung lưới cơ sở (Base Human Mesh):** Tái sử dụng $100\%$ cho toàn bộ nhân vật người chơi nam khác.
- **Khung xương Humanoid (Armature):** Làm khung xương gốc (*Master Skeleton*) cho toàn bộ animation bộ binh.
- **Hệ thống Socket:** Định vị chuẩn mực cho vị trí cầm kiếm tay phải, vạt áo choàng sau lưng và nón bảo hộ.
- **Thư viện Material:** Shader giáp bạc, da đỏ sẫm và lông sói được lưu thành Material mẫu (*Master Material Template*) trong Unity URP.

---

## 11. ĐIỀU CẤM KỴ TRONG THIẾT KẾ (ART DIRECTION DON'TS)

1. **CẤM:** Dùng mũ sắt trùm kín đầu kiểu hiệp sĩ Thập Tự Chinh phương Tây (Full Helm Crusader). Thay vào đó dùng nón lá chiến binh, mũ da sói hở mặt hoặc băng trán đúc đồng.
2. **CẤM:** Đưa họa tiết súng đạn, bánh răng cơ khí Steampunk, ốc vít lục giác vào trang bị.
3. **CẤM:** Dùng tông màu Neon sặc sỡ kiểu Cyberpunk.
4. **CẤM:** Thiết kế vũ khí siêu to khổng lồ mất trọng tâm kiểu Anime Nhật Bản phóng đại lố bịch ($2 - 3\text{m}$). Vũ khí phải giữ được cảm giác trọng lượng đầm tay, vung chém có lực cơ bắp.
5. **CẤM:** Tạo chất liệu da trơn tru nhẵn bóng như cao su hoặc kim loại sáng lóa gương cầu PBR tả thực.
