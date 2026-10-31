# HỆ THỐNG TÀI LIỆU MỸ THUẬT NHÂN VẬT 3D (CHARACTER ART DOCUMENTATION)
## HUYẾT NGUYỆT ĐẠI NGÀN

---

Thư mục này chứa toàn bộ hệ thống tài liệu quy chuẩn sản xuất mỹ thuật nhân vật 3D cho dự án **Huyết Nguyệt Đại Ngàn**.

## 1. TÀI LIỆU TỔNG CHỈ HUY (MASTER ART BIBLE)
* [**CHARACTER_ART_BIBLE.md**](CHARACTER_ART_BIBLE.md): Văn kiện trung tâm điều hướng toàn bộ 19 mục chiến lược, định hướng phong cách, kiến trúc modular, và quản trị sản xuất.

## 2. CÁC TÀI LIỆU KỸ THUẬT CHUYÊN NGÀNH
Nằm trong thư mục [**character/**](character/):
1. [**CHARACTER_STYLE_GUIDE.md**](character/CHARACTER_STYLE_GUIDE.md): Hướng dẫn Art Direction, thẩm mỹ hand-painted, tỷ lệ Stylized 1:6.8, bảng màu 5 Biome/Phe phái, hoa văn Đông Sơn.
2. [**CHARACTER_MODELING_SPEC.md**](character/CHARACTER_MODELING_SPEC.md): Quy chuẩn mô hình 3D, Blender setup, đơn vị $1\text{m} = 1\text{ unit}$, topology 3-edge loops, ngân sách đa giác LOD0/1/2, xuất FBX.
3. [**CHARACTER_RIG_ANIMATION_SPEC.md**](character/CHARACTER_RIG_ANIMATION_SPEC.md): Quy chuẩn khung xương Humanoid 53 bones, rig Sói 4 chân, chính sách In-Place Animation cho mạng LAN & Host Migration.
4. [**CHARACTER_EQUIPMENT_SPEC.md**](character/CHARACTER_EQUIPMENT_SPEC.md): Kiến trúc trang bị Modular 9 khe cắm, phân tách Skinned Mesh vs Sockets, cờ ẩn da thịt chống xuyên giáp.
5. [**CHARACTER_TEXTURE_MATERIAL_SPEC.md**](character/CHARACTER_TEXTURE_MATERIAL_SPEC.md): Quy chuẩn Texture & Vật liệu Unity URP Lit, Texel Density 10.24 px/cm, Master Atlas 2K, nướng gradient sáng tối, 10 chất liệu Đại Ngàn.
6. [**CHARACTER_LOD_OPTIMIZATION.md**](character/CHARACTER_LOD_OPTIMIZATION.md): Chiến lược LODGroup, Culling, cắt giảm xương, ngân sách Draw Call trên fixture 6 player + 20 sói + 10 NPC.
7. [**CHARACTER_ASSET_WORKFLOW.md**](character/CHARACTER_ASSET_WORKFLOW.md): Quy trình sản xuất 12 giai đoạn từ AI Concept $\to$ Turnaround $\to$ 3D Blockout $\to$ Blender $\to$ Unity Prefab $\to$ QA.
8. [**CHARACTER_QA_CHECKLIST.md**](character/CHARACTER_QA_CHECKLIST.md): Bộ tiêu chí nghiệm thu QA 13 nhóm kiểm định (Blocker/Critical/Major) trước khi đóng dấu Approved.
9. [**AI_CHARACTER_PROMPT_LIBRARY.md**](character/AI_CHARACTER_PROMPT_LIBRARY.md): Thư viện 22 bộ prompt sản xuất độc lập cho AI 2D/3D Concept.

---

## 3. HƯỚNG DẪN BẮT ĐẦU VỚI 1 NHÂN VẬT MỚI

Khi bạn hoặc thành viên trong team bắt đầu sản xuất một nhân vật mới, hãy sử dụng **Khung Task Brief chuẩn** sau để ra lệnh cho AI hoặc bàn giao cho Artist:

```markdown
# TASK BRIEF: SẢN XUẤT NHÂN VẬT [TÊN NHÂN VẬT]
Tham chiếu tài liệu gốc: docs/art/CHARACTER_ART_BIBLE.md

### 1. THÔNG TIN ĐỊNH DANH & PHẠM VI
- Tên nhân vật: [Ví dụ: Thợ Săn Rừng Rậm / Sói Bậc 1 / Dân Làng]
- Tên định danh kỹ thuật: [Ví dụ: CH_Human_Hunter_Jungle_LOD0 / CH_Werewolf_Tier1_Feral_LOD0]
- Phân nhóm nhân vật: [Nhân Tộc / Người Sói / Người Lùn / Phù Thủy / Quái Vật]
- Mốc phát triển: [MVP M8 Core / Demo M12 / Post-M12] (Tham chiếu: docs/vision/MVP_SCOPE.md)
- Biome trực thuộc: [Biome 1: Rừng Rậm Trường Sơn / Biome 2: Đầm Lầy / ...]

### 2. YÊU CẦU MỸ THUẬT & HÌNH KHỐI (docs/art/character/CHARACTER_STYLE_GUIDE.md)
- Chiều cao & Tỷ lệ: [Ví dụ: 1.80m, tỷ lệ 1:6.8 / 1.30m dã thú 4 chân]
- Vai trò & Silhouette: [Ví dụ: Thon gọn, nón lá nghiêng, đeo ống tên sau lưng, tay cầm cung tre]
- Chất liệu & Hoa văn: [Ví dụ: Da trâu sấy khói, tre nứa, vải thô nhuộm củ nâu, hoa văn răng cưa Đông Sơn]
- Bảng màu: [Liệt kê mã màu chính theo Faction và Biome]

### 3. QUY CHUẨN KỸ THUẬT & LƯỚI (docs/art/character/CHARACTER_MODELING_SPEC.md)
- Loại hình cấu trúc: [Modular Skinned Mesh thay thế / Socket Attachment / Khối độc lập]
- Ngân sách đa giác: LOD0 [Ví dụ: <= 9,000 Tris], LOD1 [<= 4,500 Tris], LOD2 [<= 1,800 Tris]
- Trọng tâm & Pivot: (0, 0, 0) tại chân, mặt nhìn về -Y (Blender) -> +Z Forward (Unity)
- Khung xương & Diễn hoạt: [Humanoid Mecanim 53 xương / Wolf Beast Rig] - BẮT BUỘC IN-PLACE ANIMATION.

### 4. ĐẦU RA YÊU CẦU
1. Bảng phân rã chi tiết trang bị & khe cắm (Equipment Breakdown).
2. Câu lệnh Prompt AI chi tiết để tạo Concept 2D và Turnaround Sheet A-Pose (từ docs/art/character/AI_CHARACTER_PROMPT_LIBRARY.md).
3. Hướng dẫn thiết lập Blender và các mốc Animation Events cần nướng.
4. Bảng kiểm tra nghiệm thu trích xuất từ docs/art/character/CHARACTER_QA_CHECKLIST.md.
```
