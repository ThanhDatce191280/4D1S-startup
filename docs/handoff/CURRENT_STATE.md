# Current Project State (Báo Cáo Trạng Thái Dự Án)

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Ngày cập nhật:** 31/10/2026  
**Chế độ hiện tại:** Cập nhật baseline sau review; nghiệm thu M5 rồi M5R → M6N. Chưa triển khai gameplay mới trong đợt cập nhật tài liệu.

---

## 1. Thông Tin Môi Trường Phát Triển & Đội Ngũ

* **Cấu hình máy:** Intel Core i5-13500HX, 16GB RAM, NVIDIA GeForce RTX 4060 8GB VRAM.
* **Hệ điều hành:** Windows 11.
* **Game Engine:** Unity 6 LTS (`6000.3.27f1`) cài đặt tại `C:\Program Files\Unity\Hub\Editor\6000.3.27f1\Editor\Unity.exe`.
* **Công cụ cài đặt:** Unity Hub, Unity Editor 6000.3.27f1, Git 2.49.0, Git LFS 3.6.1, VS Code 1.143.1.
* **Quỹ thời gian:** 48 giờ/người/tuần × 6 = 288 giờ danh nghĩa; tất cả chưa có kinh nghiệm Unity/networking/art/QA. Code gần 100% dùng AI, art/audio AI tự làm, không thuê ngoài.
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

## 3. Baseline hiện hành

Nguồn canon: `docs/vision/PRODUCT_CANON.md`; kỹ thuật: `TECHNICAL_DECISIONS.md`, `SESSION_AND_PERSISTENCE.md`; gameplay: `docs/design/FACTIONS_DEATH_AND_ENDGAME.md`.

- LAN 4–6, host migration khi chủ phòng tắt máy; hết người/hết giờ thì save/dừng, không tick offline.
- 3D zone-based, Mana dạng Sói, equipment/use-based mastery.
- Chọn/giấu phe cá nhân; quest/achievement riêng, world vẫn chạy với người còn online.
- Vua tier9 AI duy nhất/cận vệ; vua chết endgame; Sói thắng→tier10 endgame. Team wipe→bad ending.
- Chết rơi toàn đồ vào mộ, cần phép đồng đội; không tự respawn/checkpoint revive.
- Quota Linh Thạch đã chốt: Hỏa 5, Thủy 4, Mộc 3, Thổ 2, Hàn 1 — tổng 15 viên mỗi world.
- M8 core sinh tồn/combat nội bộ; M12 demo một biome/một chương có làng/nghi kỵ/phe ẩn.
- Chưa xác minh code đã đáp ứng baseline này; migration/quorum, cost hồi sinh, PvP permissions và máy tối thiểu cần spike/playtest.

---

## 4. Các File Đã Tạo / Cập Nhật Trong M0

* `docs/roadmap/PROJECT_ROADMAP.md` (Lộ trình 16 Milestones và phân vai team 6 người).
* `docs/architecture/TECHNICAL_DECISIONS.md` (8 quyết định kiến trúc cốt lõi).
* `docs/vision/MVP_SCOPE.md` (Ranh giới tính năng của Vertical Slice M8).
* `docs/roadmap/RISK_REGISTER.md` (Bảng theo dõi và xử lý rủi ro).
* `docs/architecture/GLOSSARY.md` (Từ điển ánh xạ thuật ngữ GDD $\leftrightarrow$ C#).
* `docs/handoff/CURRENT_STATE.md` (File báo cáo này).

---

## 5. Hành động tiếp theo

1. Nghiệm thu M5 bằng build và log; giữ READY_FOR_VERIFICATION tới khi có bằng chứng.
2. M5R: đối chiếu Roll/combo/Mana/equip, thống nhất contract mới và backlog.
3. M6N: spike LAN/authority/save/host migration, ưu tiên trước world expansion; dùng `docs/milestones/M6N/M6N_PLAN.md`.
4. Review lịch sử `docs/review/REVIEW_2026-10-31.md` giữ nguyên; giả định 1–4 co-op/no migration/no betrayal trong đó đã bị canon mới thay.

## 6. Nhật ký cập nhật 31/10/2026

Bổ sung canon, session/persistence, gameplay spec, scope/roadmap mới và risk/glossary. Đây chỉ là cập nhật tài liệu theo trả lời chủ dự án; không thay đổi trạng thái build/test và không chỉnh scene Unity.
