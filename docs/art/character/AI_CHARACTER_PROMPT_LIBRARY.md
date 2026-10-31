# THƯ VIỆN PROMPT TẠO HÌNH NHÂN VẬT BẰNG AI
## (AI CHARACTER GENERATION PROMPT LIBRARY — HUYẾT NGUYỆT ĐẠI NGÀN)

---

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mã tài liệu:** `CAB-SPEC-09`  
**Phiên bản:** `1.0.0`  
**Ngày cập nhật:** 31/10/2026  
**Trạng thái tài liệu:** `PROPOSED` (Thư viện prompt thực chiến cho AI 2D/3D Concept)  
**Tài liệu cha:** [CHARACTER_ART_BIBLE.md](../CHARACTER_ART_BIBLE.md)  
**Tác giả:** Character Concept Artist & Pipeline Architect  

---

## MỤC LỤC
1. [Nguyên Tắc Sử Dụng AI Trong Sản Xuất Game](#1-nguyên-tắc-sử-dụng-ai-trong-sản-xuất-game)
2. [Cấu Trúc Chuẩn Của Một Bộ Prompt Sản Xuất](#2-cấu-trúc-chuẩn-của-một-bộ-prompt-sản-xuất)
3. [Thư Viện 22 Bộ Prompt Chuyên Biệt](#3-thư-viện-22-bộ-prompt-chuyên-biệt)
   - [Prompt 01: Tạo Concept Nhân Vật Tổng Thể (Stylized Vietnamese Hero)](#prompt-01-tạo-concept-nhân-vật-tổng-thể-stylized-vietnamese-hero)
   - [Prompt 02: Bản Vẽ 3 Góc Chiếu Nhân Vật (Character Turnaround Sheet)](#prompt-02-bản-vẽ-3-góc-chiếu-nhân-vật-character-turnaround-sheet)
   - [Prompt 03: Góc Chiếu Trực Diện (Front Orthographic View)](#prompt-03-góc-chiếu-trực-diện-front-orthographic-view)
   - [Prompt 04: Góc Chiếu Cạnh Bên (Side Orthographic View)](#prompt-04-góc-chiếu-cạnh-bên-side-orthographic-view)
   - [Prompt 05: Góc Chiếu Sau Lưng (Back Orthographic View)](#prompt-05-góc-chiếu-sau-lưng-back-orthographic-view)
   - [Prompt 06: Bản Vẽ Tư Thế Chuẩn A-Pose (A-Pose Reference Sheet)](#prompt-06-bản-vẽ-tư-thế-chuẩn-a-pose-a-pose-reference-sheet)
   - [Prompt 07: Bản Vẽ Tư Thế T-Pose (T-Pose Reference Sheet)](#prompt-07-bản-vẽ-tư-thế-t-pose-t-pose-reference-sheet)
   - [Prompt 08: Phân Rã Chi Tiết Trang Bị (Equipment Breakdown Sheet)](#prompt-08-phân-rã-chi-tiết-trang-bị-equipment-breakdown-sheet)
   - [Prompt 09: Thiết Kế Bộ Giáp Trụ Đông Sơn (Armor Design - Light/Medium/Heavy)](#prompt-09-thiết-kế-bộ-giáp-trụ-đông-sơn-armor-design---lightmediumheavy)
   - [Prompt 10: Thiết Kế Binh Khí Cổ Truyền (Weapon Design - Sword, Bow, Crossbow, Machete)](#prompt-10-thiết-kế-binh-khí-cổ-truyền-weapon-design---sword-bow-crossbow-machete)
   - [Prompt 11: Mẫu Tham Chiếu Bề Mặt Vẽ Tay (Texture Style Reference)](#prompt-11-mẫu-tham-chiếu-bề-mặt-vẽ-tay-texture-style-reference)
   - [Prompt 12: Mẫu Tham Chiếu Vật Liệu Đặc Trưng (Material Swatches)](#prompt-12-mẫu-tham-chiếu-vật-liệu-đặc-trưng-material-swatches)
   - [Prompt 13: Biến Thể Dân Cư Làng Xã (Villager & Hunter Variations)](#prompt-13-biến-thể-dân-cư-làng-xã-villager--hunter-variations)
   - [Prompt 14: Người Sói Dạng Cải Trang Du Mục (Disguised Werewolf Human Form)](#prompt-14-người-sói-dạng-cải-trang-du-mục-disguised-werewolf-human-form)
   - [Prompt 15: Người Sói Dạng Quái Thú (Werewolf Beast Form - Tier 1 to 5)](#prompt-15-người-sói-dạng-quái-thú-werewolf-beast-form---tier-1-to-5)
   - [Prompt 16: Tộc Người Lùn Rèn Đúc (Dwarf Blacksmith & King)](#prompt-16-tộc-người-lùn-rèn-đúc-dwarf-blacksmith--king)
   - [Prompt 17: Phù Thủy Cổ Đại (Good Witch & Swamp Dark Witch)](#prompt-17-phù-thủy-cổ-đại-good-witch--swamp-dark-witch)
   - [Prompt 18: Nhân Vật Phụ & Trưởng Làng (Village Elder & NPC Roster)](#prompt-18-nhân-vật-phụ--trưởng-làng-village-elder--npc-roster)
   - [Prompt 19: Dã Thú Đại Ngàn (Wild Boar & Indochinese Tiger)](#prompt-19-dã-thú-đại-ngàn-wild-boar--indochinese-tiger)
   - [Prompt 20: Trùm Cuối Thế Giới: Sói Vua Tier 9 (Boss Wolf King)](#prompt-20-trùm-cuối-thế-giới-sói-vua-tier-9-boss-wolf-king)
   - [Prompt 21: Phác Thảo Khối 3D Từ Ảnh (Image-to-3D Generation - Rodin/Meshy)](#prompt-21-phác-thảo-khối-3d-từ-ảnh-image-to-3d-generation---rodinmeshy)
   - [Prompt 22: Tinh Chỉnh & Dọn Dẹp Mô Hình 3D (3D Refinement Guidelines)](#prompt-22-tinh-chỉnh--dọn-dẹp-mô-hình-3d-3d-refinement-guidelines)

---

## 1. NGUYÊN TẮC SỬ DỤNG AI TRONG SẢN XUẤT GAME

> [!CAUTION]
> **Tuyên ngôn bắt buộc:** Các công cụ AI tạo sinh hình ảnh (Midjourney, Stable Diffusion XL, Rodin, Meshy) chỉ đóng vai trò là **nguồn tham khảo cảm hứng và tạo phôi nháp ban đầu**. Tuyệt đối **KHÔNG** tuyên bố hay coi bất kỳ sản phẩm thuần AI nào là tài nguyên đạt chuẩn Game-Ready nếu chưa trải qua khâu dọn dẹp lưới, retopology, chỉnh UV và kiểm định kỹ thuật thủ công từ nghệ sĩ con người!

### Mục tiêu của thư viện prompt:
- Giúp các họa sĩ và lập trình viên trong đội ngũ (vốn chưa có chuyên môn sâu về mỹ thuật theo [PROJECT_ROADMAP.md](../../roadmap/PROJECT_ROADMAP.md)) có thể sinh ra các bản vẽ concept chuẩn xác, đồng nhất phong cách chỉ trong vài phút.
- Giữ vững bản sắc văn hóa Đông Sơn Việt Nam, triệt tiêu xu hướng AI tự động thiên lệch về phong cách Giáp Trung Cổ phương Tây hoặc Anime Nhật Bản.

---

## 2. CẤU TRÚC CHUẨN CỦA MỘT BỘ PROMPT SẢN XUẤT

Mỗi bộ prompt trong thư viện bắt buộc phải bao gồm 6 trường thông tin tiêu chuẩn:
1. **Main Prompt (Câu lệnh chính):** Viết bằng tiếng Anh kỹ thuật chi tiết cao, chứa từ khóa phong cách, tỷ lệ, trang phục và ánh sáng.
2. **Negative Prompt (Câu lệnh cấm):** Liệt kê các yếu tố rác, phong cách cấm và lỗi dị tật giải phẫu thường gặp.
3. **Reference Requirements (Yêu cầu ảnh tham chiếu):** Tỷ lệ khung hình, góc máy, ControlNet model khuyến nghị.
4. **Expected Output (Đầu ra kỳ vọng):** Mô tả chi tiết những gì nghệ sĩ cần nhìn thấy trên bức tranh đạt chuẩn.
5. **Common Failure Cases (Lỗi AI thường gặp):** Các lỗi phổ biến mà AI hay mắc phải đối với chủ đề này.
6. **Human Review Requirements (Hành động con người bắt buộc):** Các bước chỉnh sửa thủ công (Overpaint/Cleanup) cần làm.

---

## 3. THƯ VIỆN 22 BỘ PROMPT CHUYÊN BIỆT

---

### Prompt 01: Tạo Concept Nhân Vật Tổng Thể (Stylized Vietnamese Hero)
- **Main Prompt:**
  ```text
  masterpiece character concept art of a seasoned ancient Vietnamese warrior, male hero in his late 30s, stylized 3D Albion Online art style, chunky stylized heroic proportions 1:6.8, broad shoulders, bold clean forms. Wearing layered Đông Sơn bronze scale armor on torso with circular solar motif chest mirror, smoked brown water buffalo leather under-tunic with dark crimson dyed accents, thick snowy white wolf fur mantle draped over shoulders. Holding a broad double-edged ancient bronze sword with subtle golden glowing engravings. Clean neutral grey studio background, dramatic top-down rim lighting, hand-painted texture aesthetic, vibrant stylized fantasy palette, game-ready character design, 8k resolution, artstation trending --ar 3:4 --stylize 250 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  photorealistic, western knight full plate armor, crusader cross, gothic steel armor, steampunk, guns, neon colors, anime chibi, deformed hands, extra fingers, blurry textures, cluttered noisy details, modern clothing, sunglasses, high-poly micro details
  ```
- **Reference Requirements:** Tỷ lệ khung hình $3:4$. Ánh sáng studio 3 điểm (Three-point lighting).
- **Expected Output:** Nhân vật chiến binh tráng kiện, toát lên phong thái Lạc Việt cổ xưa, giáp vảy đồng nẹp da đỏ, áo choàng lông sói trắng nổi bật, thanh kiếm vàng uy dũng.
- **Common Failure Cases:** AI hay vẽ kiếm hiệp sĩ phương Tây có chuôi kiếm hình chữ thập, hoặc giáp toàn thân bằng sắt bóng lộn.
- **Human Review Requirements:** Dùng Photoshop xóa bỏ chuôi chữ thập nếu có; vẽ đè họa tiết chim Lạc vào đốc kiếm.

---

### Prompt 02: Bản Vẽ 3 Góc Chiếu Nhân Vật (Character Turnaround Sheet)
- **Main Prompt:**
  ```text
  full body character turnaround model sheet of an ancient Vietnamese warrior hero, showing three views side-by-side: front view, side profile view, and back view. Perfect A-pose modeling stance with arms 45 degrees down and palms forward. Exact horizontal height alignment guidelines for head, shoulders, hips, knees, and feet across all three angles. Stylized 3D hand-painted Blizzard and Albion Online art style, wearing silver dragon-scale armor, dark red leather gambeson, wolf fur cape. Isolated on solid flat neutral grey background #808080, orthographic camera projection, no perspective distortion, flat lighting, clean outlines for 3D modeling blueprint --ar 16:9 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  perspective view, dramatic action pose, dynamic angle, foreshortening, shadows on ground, different outfits between views, inconsistent height, smiling, asymmetry between left and right front, photo, high contrast shadows
  ```
- **Reference Requirements:** Tỷ lệ $16:9$. Mặt phẳng chiếu trực giao (*Orthographic*), không có bóng đổ sàn nhà.
- **Expected Output:** Bức ảnh chứa đúng 3 góc chiếu: Thẳng, Nghiêng và Sau lưng thẳng hàng nhau tuyệt đối từ đầu đến chân.
- **Common Failure Cases:** Chiều cao nhân vật ở góc nghiêng bị ngắn hơn góc thẳng; trang phục góc sau không khớp góc trước.
- **Human Review Requirements:** Kẻ các đường gióng ngang (Guides) trong Photoshop để căn chỉnh đỉnh đầu, thắt lưng và mắt cá chân khớp nhau từng pixel.

---

### Prompt 03: Góc Chiếu Trực Diện (Front Orthographic View)
- **Main Prompt:**
  ```text
  full body character orthographic front view of an ancient Vietnamese warrior, perfect symmetrical A-pose, arms 45 degrees away from torso, fingers slightly curled, feet shoulder-width apart. Stylized 3D game model blueprint, hand-painted aesthetic, wearing silver chestplate with Dong Son sun symbol, dark red leather sleeves, fur collar, leather boots. Flat neutral grey background, pure flat diffuse lighting, zero perspective vanishing point, clean silhouette for 3D artists --ar 1:2 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  perspective, isometric angle, dynamic pose, shadows, weapon in hand, tilted head, asymmetrical arms, photorealistic, noise
  ```
- **Reference Requirements:** Khung hình dọc $1:2$. Không cầm vũ khí trên tay để lộ rõ cấu trúc găng tay.
- **Expected Output:** Góc nhìn thẳng đối xứng tuyệt đối, thấy rõ vị trí các khớp xương để nạp vào Blender Background Image.
- **Common Failure Cases:** Đầu hơi nghiêng hoặc một chân bước lên trước.
- **Human Review Requirements:** Cắt nửa thân trái nhân vật và lật gương (Mirror) để kiểm tra độ đối xứng.

---

### Prompt 04: Góc Chiếu Cạnh Bên (Side Orthographic View)
- **Main Prompt:**
  ```text
  full body character orthographic side profile view of an ancient Vietnamese warrior in A-pose, facing left. Stylized 3D game character blueprint, showing silhouette of chestplate curvature, slight back hunch of a warrior, boot profile, shoulder armor profile. Flat neutral grey background, orthographic projection, no perspective, zero cast shadow, hand-painted style --ar 1:2 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  three quarter angle, front view, back view, perspective, action pose, dynamic lighting, high contrast
  ```
- **Reference Requirements:** Nhìn từ mạn sườn trái nhân vật.
- **Expected Output:** Thấy rõ độ lồi của ngực, độ cong của mông, độ dày của giáp vai và độ vát của đế giày.
- **Common Failure Cases:** Cánh tay che khuất hoàn toàn lồng ngực và đai lưng.
- **Human Review Requirements:** Vẽ mờ cánh tay (*Ghost Arm*) để nghệ sĩ 3D quan sát được đường nét eo và ngực.

---

### Prompt 05: Góc Chiếu Sau Lưng (Back Orthographic View)
- **Main Prompt:**
  ```text
  full body character orthographic back view of an ancient Vietnamese warrior in A-pose, rear view showing details of back plate, belt straps, leather pouch, shoulder blades, back of wolf fur cape, boots from behind. Stylized 3D hand-painted texture style, flat grey background, pure orthographic, zero perspective --ar 1:2 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  front view, turning head, perspective, shadows, front details, weapons
  ```
- **Reference Requirements:** Nhìn thẳng vào lưng nhân vật.
- **Expected Output:** Thể hiện rõ cách dây đai da chéo qua lưng, nếp gấp của áo choàng và đường may sau ống quần.
- **Common Failure Cases:** Áo choàng che kín từ cổ đến chân làm mất cấu trúc giáp lưng.
- **Human Review Requirements:** Làm 2 phiên bản: Một phiên bản có áo choàng và một phiên bản cởi bỏ áo choàng.

---

### Prompt 06: Bản Vẽ Tư Thế Chuẩn A-Pose (A-Pose Reference Sheet)
- **Main Prompt:**
  ```text
  technical character concept art of a male fantasy warrior in default neutral A-pose for rigging, standing relaxed, feet flat on ground 30cm apart, arms held down at 45 degree angle from body, palms rotated slightly forward. Clear visibility of armpits, crotch, neck seam, and waist joints. Stylized hand-painted 3D aesthetic, bold shapes, clean uncluttered joints for 3D animators, neutral light grey background --ar 3:4 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  T-pose, closed armpits, crossed legs, dynamic fighting stance, holding weapons, clenched fists, shadows
  ```
- **Reference Requirements:** Vùng nách và háng phải có khe hở tối thiểu $15\text{cm}$ để phục vụ quét trọng lượng xương (*Skin Weighting*).
- **Expected Output:** Tư thế phôi mẫu lý tưởng nhất cho Technical Animator bắt đầu gắn xương.
- **Common Failure Cases:** Tay ép quá sát vào mạng sườn làm dính khối.
- **Human Review Requirements:** Tách rời cánh tay ra nếu bị chạm vào thân.

---

### Prompt 07: Bản Vẽ Tư Thế T-Pose (T-Pose Reference Sheet)
- **Main Prompt:**
  ```text
  technical character model sheet in classic T-pose, arms fully extended horizontally parallel to floor at 90 degrees, palms facing down, legs straight, neutral expression. Ancient Vietnamese warrior wearing undergarment leather armor, stylized hand-painted game art, pure blueprint sheet, neutral grey backdrop --ar 16:9 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  A-pose, bent elbows, weapons, dynamic, perspective distortion, high contrast lighting
  ```
- **Reference Requirements:** Dành riêng cho việc kiểm tra khung xương Mixamo hoặc khớp vai tự động.
- **Expected Output:** Hai tay dang ngang hoàn hảo tạo thành hình chữ T.
- **Common Failure Cases:** Cánh tay bị võng xuống hoặc cơ vai nhô lên quá cao làm gãy khớp.
- **Human Review Requirements:** Dùng thước căn ngang đảm bảo trục xương cánh tay tạo góc $90^\circ$ chuẩn xác với cột sống.

---

### Prompt 08: Phân Rã Chi Tiết Trang Bị (Equipment Breakdown Sheet)
- **Main Prompt:**
  ```text
  video game modular equipment breakdown sheet, showing individual armor pieces of a Silver Wolf Warrior laid out neatly on a wooden workshop table: 1. Silver dragon scale chestplate with circular mirror, 2. Pair of silver pauldrons, 3. Pair of dark red studded leather vambraces, 4. Heavy wolf fur mantle collar, 5. Thick leather boots with fur trim, 6. Broad leather belt with bronze buckle. Stylized 3D hand-painted texture style, inventory asset sheet, soft ambient studio lighting, clean outlines --ar 16:9 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  armor worn on body, character wearing clothes, modern military gear, sci-fi, blurry, messy pile
  ```
- **Reference Requirements:** Từng món trang bị được tách rời như các icon trong túi đồ.
- **Expected Output:** Bảng bóc tách chi tiết từng mảnh giáp giúp nghệ sĩ 3D dựng riêng từng sub-mesh.
- **Common Failure Cases:** Các mảnh giáp dính chùm vào nhau hoặc vẽ thành áo giáp liền thân.
- **Human Review Requirements:** Đánh số thứ tự từ 1 đến 6 và gắn nhãn tên khe cắm tương ứng (`Slot_Chest`, `Slot_Boots`).

---

### Prompt 09: Thiết Kế Bộ Giáp Trụ Đông Sơn (Armor Design - Light/Medium/Heavy)
- **Main Prompt:**
  ```text
  concept sheet displaying 3 tiers of ancient Vietnamese warrior armor progression, from left to right: Tier 1 Light Armor made of palm leaves, bamboo slats, woven straw conical hat, and coarse linen; Tier 2 Medium Armor made of dark boiled buffalo leather, bronze scale plates on chest, and lacquered wood arm guards; Tier 3 Heavy Armor made of heavy black-bronze plates, embossed Dong Son sun emblem, thick wolf fur collar, and layered dragon scales. Stylized hand-painted 3D aesthetic, clear visual progression, white clean background --ar 16:9 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  western knight armor, iron chainmail, crusader plate, sci-fi cybernetic, same armor 3 times, photorealistic
  ```
- **Reference Requirements:** Thể hiện rõ 3 cấp độ từ trái sang phải: Nhẹ $\to$ Trung bình $\to$ Nặng.
- **Expected Output:** Phân cấp rõ rệt: T1 thô sơ dân dã, T2 chiến binh lành nghề, T3 thủ lĩnh tối cao.
- **Common Failure Cases:** Cấp 1 trông quá lộng lẫy hoặc Cấp 3 biến thành người máy bọc thép phương Tây.
- **Human Review Requirements:** Kiểm tra chất liệu bản địa: T1 phải có nón lá/áo tơi; T2 phải có da trâu/đồng đỏ; T3 có bạc/lông thú.

---

### Prompt 10: Thiết Kế Binh Khí Cổ Truyền (Weapon Design - Sword, Bow, Crossbow, Machete)
- **Main Prompt:**
  ```text
  game weapon concept art set featuring traditional ancient Vietnamese weaponry: 1. Long double-edged village iron sword with lacquered wood hilt and bronze pommel shaped like Lac bird head; 2. Curved jungle machete with thick heavy blade and bamboo handle; 3. Long bamboo reflex bow reinforced with horn; 4. Legendary multi-shot turtle crossbow mechanism made of dark wood and bronze gears. Stylized 3D hand-painted texture style, isolated on clean grey background, sharp silhouette, crisp game asset presentation --ar 16:9 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  guns, futuristic laser weapons, oversized anime buster sword, medieval European broadsword with crossguard, blurry, photorealistic
  ```
- **Reference Requirements:** Đặt vũ khí nằm ngang hoặc nghiêng $45^\circ$, tỷ lệ cầm nắm thực tế ($0.8\text{m} - 1.2\text{m}$).
- **Expected Output:** Bộ 4 vũ khí đặc hữu của game: Trường Kiếm Sắt, Mã Tấu Rừng, Cung Tre và Nỏ Thần.
- **Common Failure Cases:** Lưỡi kiếm bị uốn lượn kỳ dị kiểu kiếm ma thuật phương Tây hoặc nỏ có ống ngắm hiện đại.
- **Human Review Requirements:** Đảm bảo chuôi kiếm có tỷ lệ vừa vặn với nắm tay nhân vật $1.8\text{m}$ ($25 - 30\text{cm}$).

---

### Prompt 11: Mẫu Tham Chiếu Bề Mặt Vẽ Tay (Texture Style Reference)
- **Main Prompt:**
  ```text
  texture style guide and surface reference swatches for a stylized 3D fantasy game, hand-painted aesthetic, vibrant color gradients, strong top-down light bake, painted edge highlights, warm shadows, cool highlights, brush stroke details visible. Showing panels of: painted bronze with green patina, aged red leather with scuffs, chiseled river stone with moss, snowy wolf fur clumps, and dark aged bamboo. Texture artist reference palette, flat square layout --ar 1:1 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  PBR photorealistic scans, noisy micro scratches, flat solid colors without gradients, photographic noise
  ```
- **Reference Requirements:** Khung hình vuông $1:1$, bố cục dạng ô gạch mẫu chất liệu.
- **Expected Output:** Bảng mẫu cọ vẽ tay chuẩn mực làm kim chỉ nam cho nghệ sĩ Texture trong Substance Painter.
- **Common Failure Cases:** AI xuất ra ảnh chụp vật liệu thật (Photo Scans) thay vì tranh vẽ tay cách điệu.
- **Human Review Requirements:** Trích xuất mã màu Hex của các vùng sáng và vùng tối đưa vào bảng màu chuẩn.

---

### Prompt 12: Mẫu Tham Chiếu Vật Liệu Đặc Trưng (Material Swatches)
- **Main Prompt:**
  ```text
  stylized 3D material study balls rendering 4 distinct materials under fantasy lighting: 1. Polished silver armor with dragon scale engraving and soft blue rim light; 2. Ancient Dong Son bronze with verdigris patina and golden sun relief; 3. Weathered dark brown buffalo leather; 4. Glowing orange-red Fire Element crystal with inner pulse. Stylized URP game look, hand-painted texture feel, clean render on dark background --ar 16:9 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  chrome mirror reflection, raytraced glass, flat cartoon without depth, plastic toy material
  ```
- **Reference Requirements:** 4 quả cầu vật liệu (*Material Spheres*) đặt cạnh nhau.
- **Expected Output:** Phản quang đặc trưng của Bạc, Đồng Đông Sơn, Da thuộc và Tinh thể Linh Thạch.
- **Common Failure Cases:** Quả cầu tinh thể bị biến thành viên bi ve trong suốt không có năng lượng ma thuật.
- **Human Review Requirements:** Đối chiếu thông số Metallic và Smoothness với [CHARACTER_TEXTURE_MATERIAL_SPEC.md](CHARACTER_TEXTURE_MATERIAL_SPEC.md).

---

### Prompt 13: Biến Thể Dân Cư Làng Xã (Villager & Hunter Variations)
- **Main Prompt:**
  ```text
  character concept sheet featuring 3 ancient Vietnamese villagers, stylized 3D Albion Online style: 1. Male Blacksmith with muscular build, leather apron, holding iron tongs; 2. Female Rice Gatherer wearing palm leaf rain cape, conical hat, holding a wicker basket; 3. Veteran Village Hunter with brown leather tunic, quiver on back, holding a bamboo spear. Friendly but vigilant expressions, rustic Vietnamese village clothing, earthy color palette, neutral background --ar 16:9 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  European medieval peasants, modern clothes, royal silk dresses, high fantasy wizard robes, photorealistic
  ```
- **Reference Requirements:** 3 nhân vật đứng cạnh nhau cùng một phong cách nghệ thuật.
- **Expected Output:** Thợ Rèn làng, Thôn Nữ hái lượm và Thợ Săn dày dạn kinh nghiệm.
- **Common Failure Cases:** Dân làng trông quá giàu sang quyền quý như quý tộc phong kiến.
- **Human Review Requirements:** Thêm các chi tiết lấm lem bùn đất và vết chai sạn trên bàn tay.

---

### Prompt 14: Người Sói Dạng Cải Trang Du Mục (Disguised Werewolf Human Form)
- **Main Prompt:**
  ```text
  character concept art of a wandering nomad merchant who is secretly a disguised werewolf, male traveler in rough patched nomadic clothes, carrying a heavy bamboo backpack loaded with strange exotic goods and bones. Subtle sinister signs: unnatural golden-yellow predatory wolf eyes, slightly hunched posture, sharp jagged nails, dried blood stains on sleeve cuffs. Stylized 3D hand-painted fantasy art, atmospheric lighting, mystery and suspicion theme, dark fantasy Vietnamese frontier --ar 3:4 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  full werewolf monster, obvious monster, furry, cute smiling trader, bright modern clothing, photorealistic
  ```
- **Reference Requirements:** Bề ngoài $90\%$ là con người, chỉ có $10\%$ dấu hiệu bất thường để người chơi tinh mắt soi xét theo [ADR-05](../../architecture/TECHNICAL_DECISIONS.md).
- **Expected Output:** Thương nhân du mục bí ẩn, toát lên sự nguy hiểm ngầm của kẻ săn mồi giả trang.
- **Common Failure Cases:** AI biến hẳn thành quái vật nửa người nửa sói quá lộ liễu, làm mất tính năng "Nghi kỵ".
- **Human Review Requirements:** Giảm bớt các đặc điểm quái vật nếu quá lộ; chỉ giữ lại ánh mắt vàng và ngón tay khẳng khiu.

---

### Prompt 15: Người Sói Dạng Quái Thú (Werewolf Beast Form - Tier 1 to 5)
- **Main Prompt:**
  ```text
  fearsome werewolf beast form concept art, Tier 1 Feral Werewolf, terrifying lupine predator, powerful hunchbacked muscular body, digitigrade wolf legs with long sharp black claws, snarling wolf head with glowing red eyes, jagged fangs, thick dark grey and blood-matted fur around neck and shoulders. Stylized 3D stylized game asset look, aggressive dynamic stalking pose, feral beast anatomy, dark fantasy forest background with red moon rim light --ar 3:4 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  cute wolf, friendly dog, cartoon wolf, anime anthropomorphic fox, wearing human armor, human face, photorealistic fur
  ```
- **Reference Requirements:** Dã thú 4 chân hoặc nửa đứng (Semi-quadruped), cơ bắp gồ ghề, móng vuốt cào xé.
- **Expected Output:** Sói Bậc 1 hung dữ, đói khát, hiện thân của cơn ác mộng Đêm Huyết Nguyệt.
- **Common Failure Cases:** Trông giống chó sói bình thường hoặc giống người sói mặc quần jeans kiểu Hollywood.
- **Human Review Requirements:** Kiểm tra cấu hình khớp chân sau (Digitigrade Hock) khớp với [CHARACTER_RIG_ANIMATION_SPEC.md](CHARACTER_RIG_ANIMATION_SPEC.md).

---

### Prompt 16: Tộc Người Lùn Rèn Đúc (Dwarf Blacksmith & King)
- **Main Prompt:**
  ```text
  character concept art of an ancient Vietnamese Dwarf Master Blacksmith, stocky and immensely broad-shouldered dwarf, height 1.25 meters, ratio 1:4.5, thick braided dark beard tucked into belt, rugged weathered face. Wearing heavy black-iron forge apron over rough linen tunic, heavy bronze gauntlets, holding a glowing hot masterwork forging hammer. Stylized 3D hand-painted Blizzard style, solid rocky silhouette, volcanic forge glow background --ar 3:4 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  tall human, Scottish kilt dwarf cliché, elf, cartoon gnome, sci-fi cybernetic, photorealistic
  ```
- **Reference Requirements:** Chiều cao thấp đậm ($1.25\text{m}$), chiều rộng thân nở nang, tạo hình như khối đá tảng.
- **Expected Output:** Nghệ nhân rèn đúc Người Lùn cổ đại sống trong hang đá Karst, người nắm giữ bí thuật rèn Ngũ Linh Thạch.
- **Common Failure Cases:** Râu tóc mang phong cách Viking/Scotland phương Tây với bím tóc thắt nơ.
- **Human Review Requirements:** Chỉnh sửa trang phục theo hơi hướng thợ đúc đồng cổ truyền, đai lưng cài then đồng.

---

### Prompt 17: Phù Thủy Cổ Đại (Good Witch & Swamp Dark Witch)
- **Main Prompt:**
  ```text
  character concept art of an ancient Vietnamese Forest Witch, wise hermit sorceress living in the Trường Sơn jungle. Slender elderly woman with mystical calm expression, long silver-streaked dark hair adorned with forest herbs and sacred seeds. Wearing flowing dark emerald green and hemp linen robes with hand-painted cloud motifs, carrying a gnarled hardwood staff topped with a living green Wood Element crystal. Stylized 3D hand-painted art, magical nature aesthetic, soft misty forest backdrop --ar 3:4 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  western pointy hat witch, broomstick, sexy young witch, Halloween witch with green skin, modern magic wand, high-tech
  ```
- **Reference Requirements:** Tuyệt đối **không** dùng nón nhọn phù thủy phương Tây hay chổi bay.
- **Expected Output:** Bà mo / Thảo y tiên cổ xưa của núi rừng Việt Nam, gắn liền với Mộc Linh Thạch và thảo dược cứu người.
- **Common Failure Cases:** AI tự động thêm chiếc mũ chóp nhọn Halloween màu đen.
- **Human Review Requirements:** Xóa bỏ mũ chóp nhọn; thay bằng khăn vấn đầu hoặc mái tóc rủ cài cành thảo mộc.

---

### Prompt 18: Nhân Vật Phụ & Trưởng Làng (Village Elder & NPC Roster)
- **Main Prompt:**
  ```text
  character concept art of an ancient Vietnamese Village Elder, respected leader of the jungle settlement, wise patriarch in his 60s with long white beard and dignified posture. Wearing a traditional dark indigo dyed tunic with embroidered bronze sun borders, a woven bamboo ceremonial headband, holding a polished bamboo walking staff. Stylized 3D game NPC design, warm inviting yet stern expression, village communal house (Đình Làng) in soft background --ar 3:4 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  monarch with gold crown, western elder, Chinese emperor dragon robe, young man, anime fantasy wizard, photorealistic
  ```
- **Reference Requirements:** Phong thái uy nghiêm của Trưởng Làng, người giao nhiệm vụ và dẫn dắt cộng đồng sinh tồn.
- **Expected Output:** Già làng phúc hậu nhưng kiên định, trang phục vải chàm viền họa tiết Đông Sơn.
- **Common Failure Cases:** Bị nhầm sang trang phục quan lại triều đình phong kiến Trung Hoa (Áo thụng thêu rồng).
- **Human Review Requirements:** Giản lược các chi tiết quan trường, đưa về nét mộc mạc dân gian của người thủ lĩnh bộ tộc.

---

### Prompt 19: Dã Thú Đại Ngàn (Wild Boar & Indochinese Tiger)
- **Main Prompt:**
  ```text
  creature concept art of a ferocious Trường Sơn Wild Boar, native jungle game monster, massive muscular hunch, thick razor-sharp curved tusks, coarse bristly dark brown hide, mud-caked shoulders. Stylized 3D hand-painted beast design, aggressive charging stance, heavy powerful silhouette for action RPG combat, misty jungle floor background --ar 16:9 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  cute farm pig, pink domestic pig, cartoon Pumbaa, sci-fi cybernetic beast, photorealistic fur particles, blurry
  ```
- **Reference Requirements:** Dã thú phục vụ săn bắt lấy thịt trong Vertical Slice M8 ([MVP_SCOPE.md](../../vision/MVP_SCOPE.md)).
- **Expected Output:** Lợn rừng hung dữ, mõm dài nanh nhọn, lưng gồ cơ bắp, sẵn sàng húc đổ người chơi.
- **Common Failure Cases:** Trông giống lợn nuôi chuồng hoặc lợn hoạt hình dễ thương.
- **Human Review Requirements:** Tăng cường độ nhọn của cặp nanh và độ xù xì của bờm lông sống lưng.

---

### Prompt 20: Trùm Cuối Thế Giới: Sói Vua Tier 9 (Boss Wolf King)
- **Main Prompt:**
  ```text
  epic boss creature concept art of the legendary Tier 9 Wolf King (Sói Vua), supreme apex predator of the Blood Moon, colossal demonic werewolf lord, height 3.5 meters, immense muscular frame covered in ancient jagged obsidian armor plates and glowing red blood runes. Crowned with sweeping demonic horns made of blood crystal, glowing crimson infernal eyes, jaw dripping with mystical mist, standing on a shattered snow mountain peak under a massive crimson Eclipse Blood Moon. Stylized 3D dark fantasy boss design, terrifying menacing silhouette, masterpiece game boss art --ar 16:9 --stylize 400 --v 6.0
  ```
- **Negative Prompt:**
  ```text
  weak feral wolf, human warrior, cute wolf, friendly, sci-fi armor, guns, bright cheerful colors, flat lighting
  ```
- **Reference Requirements:** Cá thể duy nhất của toàn thế giới theo [PRODUCT_CANON.md C07](../../vision/PRODUCT_CANON.md). Tầm vóc khổng lồ $3.5\text{m}$.
- **Expected Output:** Trùm cuối uy nghi, đáng sợ, bóng đen trùm lấp chiến trường, cơ thể tích tụ tà năng Huyết Nguyệt.
- **Common Failure Cases:** Thiếu sừng huyết thạch hoặc bị biến thành một con sói đen bình thường chỉ to xác hơn.
- **Human Review Requirements:** Đảm bảo Sói Vua có các vết nứt bùa chú trên ngực để phục vụ pha đánh trúng điểm yếu (Weak Spot).

---

### Prompt 21: Phác Thảo Khối 3D Từ Ảnh (Image-to-3D Generation - Rodin/Meshy)
- **Cấu hình & Prompt cho công cụ 3D AI (Rodin Gen-1 / Meshy v3):**
  - **Input Image:** Tải lên ảnh `Ref_Turnaround_[Name]_Apose.png` đã làm sạch ở Prompt 02.
  - **Text Prompt bổ trợ:**
    ```text
    stylized 3D game character model, ancient warrior in A-pose, bold chunky shapes, clean topology, symmetry, hand-painted texture style, low poly game asset, no floating loose geometry
    ```
  - **Cấu hình tham số công cụ (Settings):**
    - Target Polycount: `10,000 Triangles` (hoặc `Low/Mid`).
    - Topology Mode: `Quad Preferred` (nếu có).
    - Texture Generation: `Bật (Enable Albedo Map)`.
    - PBR Maps: `Tắt (Disable PBR - Chỉ lấy Albedo làm phôi)`.
- **Expected Output:** File OBJ/FBX chứa khối thể tích tổng thể của nhân vật đúng tỷ lệ A-Pose.
- **Common Failure Cases:** Hai bàn tay bị dính tịt vào đùi; ngón tay bị dính thành một khối bánh mì; mặt sau lõm vào trong.
- **Human Review Requirements:** Nhập vào Blender, xóa bỏ toàn bộ bàn tay và bàn chân dị tật để lắp phôi tay chân chuẩn từ thư viện Base Mesh.

---

### Prompt 22: Tinh Chỉnh & Dọn Dẹp Mô Hình 3D (3D Refinement Guidelines)
- **Hướng dẫn nghệ sĩ thực hiện dọn dẹp phôi 3D từ AI trong Blender:**
  1. **Bước 1 — Đo đạc & Căn chỉnh Kích thước:**
     - Bật thanh công cụ đo `Measure`: Kéo từ đỉnh đầu xuống sàn $\to$ Nhấn phím `S` scale cho đến khi đạt đúng $1.80\text{m}$.
     - Nhấn `Ctrl + A` $\to$ Chọn **Apply All Transforms**.
  2. **Bước 2 — Cắt đôi đối xứng (Bisect & Mirror):**
     - Chọn trục đối xứng $X=0$. Xóa bỏ nửa thân bị lỗi nhiều hơn.
     - Thêm modifier `Mirror` với tùy chọn `Clipping: Bật` để chỉnh sửa 1 bên tự động ăn sang bên kia.
  3. **Bước 3 — Retopology (Làm sạch lưới):**
     - Sử dụng công cụ `PolyQuilt` hoặc `RetopoFlow` để đi lại lưới Quad phủ lên khối phôi AI.
     - Khóa cứng số lượng đỉnh tại các khớp cổ tay và cổ chân theo chuẩn [CHARACTER_MODELING_SPEC.md](CHARACTER_MODELING_SPEC.md).
  4. **Bước 4 — Nướng chi tiết (Bake Details):**
     - Lấy màu sắc từ texture nháp của AI nướng sang UV Map chuẩn mới trải bằng công cụ `Bake -> Diffuse` của Blender.

