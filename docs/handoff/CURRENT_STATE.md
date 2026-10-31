# Current Project State (Báo Cáo Trạng Thái Dự Án)

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Ngày cập nhật:** 31/10/2026  
**Chế độ hiện tại:** `MODE A — ROADMAP` $\to$ Chuẩn bị sang `MODE D — IMPLEMENT: M1`

---

## 1. Thông Tin Môi Trường Phát Triển & Đội Ngũ

* **Cấu hình máy:** Intel Core i5-13500HX, 16GB RAM, NVIDIA GeForce RTX 4060 8GB VRAM.
* **Hệ điều hành:** Windows 11.
* **Game Engine:** Unity 6 LTS (`6000.3.27f1`) cài đặt tại `C:\Program Files\Unity\Hub\Editor\6000.3.27f1\Editor\Unity.exe`.
* **Công cụ cài đặt:** Unity Hub, Unity Editor 6000.3.27f1, Git 2.49.0, Git LFS 3.6.1, VS Code 1.143.1.
* **Quỹ thời gian:** 48 giờ / tuần (Toàn thời gian).
* **Quy mô đội ngũ:** 6 thành viên (phân bổ theo 6 vai trò trong `PROJECT_ROADMAP.md`).
* **AI Coding Assistants:** GPT-6 Astra & Gemini 4 Argon.

---

## 2. Trạng Thái Cột Mốc (Milestone Status)

* **Current Milestone:** **M5 — Inventory, Items & Crafting**
* **Milestone Status:** **READY_FOR_VERIFICATION**
* **Completed Milestones:**
  * **M0 — Feasibility, Scope & Technical Planning:** COMPLETED (Bộ tài liệu kiến trúc, ADR, ranh giới Vertical Slice, Glossary danh pháp chuẩn).
  * **M1 — Development Environment & Unity Foundation:** COMPLETED (Unity 6 URP, Git & LFS, Packages sạch lỗi, Scene Sandbox 376 FPS, Build Windows `.exe` độc lập thành công tại `Builds/Windows/HuyetNguyetDaiNgan_TestBuild.exe`).
  * **M2 — 3D Character Controller & Camera:** COMPLETED (Di chuyển Isometric 3/4 camera-relative, chạy nước rút Shift, xoay Visual độc lập, Cinemachine Virtual Camera damping mượt mà, Prefab `Player_Prototype`, Scene `Test_Movement.unity` leo dốc $15^\circ - 45^\circ$, bộ Unit Tests di chuyển).
  * **M3 — Combat Prototype:** COMPLETED (Hệ thống máu `HealthComponent` + `IDamageable`, vũ khí `WeaponDataSO` Trường Kiếm Sắt, chém quét hình quạt không cấp phát rác, AI Sói FSM tuần tra/đuổi/cào cắn bằng NavMesh, thanh máu World Space Billboard, Scene `Test_CombatArena.unity`, bộ Unit Tests combat).
  * **M4 — Survival Core:** COMPLETED (4 thanh HP, Hunger, Stamina, Mana; nút tài nguyên Cây gỗ, Mỏ đá, Bụi quả; hút vật phẩm rơi ngoài đất; HUD kính mờ trong suốt góc dưới bên phải).
* **Completed Tasks in M5:**
  * `M5-T1`: Xây dựng `ItemDataSO.cs` (ScriptableObject định nghĩa tài nguyên, thức ăn, công cụ, vũ khí).
  * `M5-T2`: Lập trình `InventorySystem.cs` & `InventorySlot.cs` quản lý túi đồ 20 ô ($4 \times 5$), xử lý cộng dồn stack, hoán đổi và dùng đồ.
  * `M5-T3`: Lập trình `CraftingSystem.cs` & `CraftingRecipeSO.cs` kiểm tra nguyên liệu và chế tạo thành phẩm.
  * `M5-T4`: Tích hợp `WorldItemPickup` nạp trực tiếp tài nguyên vào túi đồ người chơi.
  * `M5-T5`: Thiết kế `InventoryCraftingUI.cs` (Bật/tắt bằng phím `I`/`Tab`, phong cách kính mờ Dark Fantasy viền vàng kim, cột trái 20 ô túi đồ, cột phải 4 công thức chế tạo).
  * `M5-T6`: Xây dựng bộ Unit Tests `InventoryCraftingTests.cs` (EditMode) kiểm thử trọn vẹn logic thêm đồ, stack đồ, kiểm tra nguyên liệu và chế tạo.
  * `M5-T7`: Menu Item 1-click tạo Scene `Test_InventoryCrafting.unity` (`M5_InventorySceneSetupHelper.cs`).
* **Next Action:** Người dùng click chạy Menu `Huyết Nguyệt Đại Ngàn -> M5: Setup Inventory & Crafting Scene` trong Unity Editor để nghiệm thu.

---

## 3. Các Quyết Định Kỹ Thuật Đã Được Phê Duyệt (Approved Decisions)

1. **Engine & Render:** Unity 6 LTS (6000.3.27f1) + URP + C# (.NET Standard 2.1).
2. **Camera & Art:** 3D Stylized Low/Mid-Poly, camera Isometric / 3/4 cố định góc nhìn (kiểu Albion Online, không xoay 360 độ tự do).
3. **Thế giới & Hầm mỏ:** Mô hình Zone-based World kết nối qua cổng chuyển vùng + Cửa Hầm Mỏ chuyển tầng địa chất (Instanced Underground Sub-scenes) + Cơ chế Độn Thổ (Burrow State) cho quái đào hầm.
4. **Sói Vua & Vòng lặp Sinh tồn Hardcore:** Sói Vua tăng chỉ số vĩnh viễn mỗi đêm không đặt trần (Fail Condition chủ ý); người chơi phải tích cực Đánh Phủ Đầu chặn nguồn thịt ban đêm hoặc chấp nhận thất bại để rút kinh nghiệm cho màn chơi sau.
5. **Hệ thống Nghi kỵ & Phát hiện Sói:** Chuẩn hóa 3 tầng (Cảnh báo diện rộng $\to$ Nghi vấn hàng hóa $\to$ Xác thực tốn tài nguyên) + Hình phạt chém nhầm Người Du Mục thật là thương đoàn ngừng ghé làng trong 7 ngày.
6. **Mana, Hóa Sói & Albion Mastery:** Người Sói không giới hạn Stamina nhưng có Mana. Mana đóng vai trò thời gian duy trì hóa Sói (rút dần theo giây) và dùng làm Huyết Năng cho kỹ năng Huyết Thống. Trang bị và kỹ năng tuân thủ triết lý Albion Equipment + Mastery.
7. **Mô phỏng AI Thế giới (Hybrid Simulation):** Bộ lập lịch sự kiện (`WorldEventScheduler`) mô phỏng toán học ngầm để phe Sói/quái không ngừng tiến hóa; chỉ spawn thực thể 3D vật lý khi người chơi tiếp cận khu vực liên quan.
8. **Chiến lược Đồ họa:** Dùng 3D Placeholder + Mixamo animations ở giai đoạn M1–M8 để tập trung 100% hoàn thành Vertical Slice (M8) trước khi đầu tư art chi tiết.

---

## 4. Các File Đã Tạo / Cập Nhật Trong M0

* `docs/roadmap/PROJECT_ROADMAP.md` (Lộ trình 16 Milestones và phân vai team 6 người).
* `docs/architecture/TECHNICAL_DECISIONS.md` (8 quyết định kiến trúc cốt lõi).
* `docs/vision/MVP_SCOPE.md` (Ranh giới tính năng của Vertical Slice M8).
* `docs/roadmap/RISK_REGISTER.md` (Bảng theo dõi và xử lý rủi ro).
* `docs/architecture/GLOSSARY.md` (Từ điển ánh xạ thuật ngữ GDD $\leftrightarrow$ C#).
* `docs/handoff/CURRENT_STATE.md` (File báo cáo này).

---

## 5. Hành Động Tiếp Theo (Next Action)

* **Bắt đầu Milestone M1:** Tạo Unity Project chuẩn URP 3D bằng Unity Editor `6000.3.27f1`, thiết lập `.gitignore`, cấu trúc thư mục `Assets/_Project/`, cài đặt các package cốt lõi (`Input System`, `Cinemachine`, `AI Navigation`) và kiểm thử bản build `.exe` đầu tiên.

