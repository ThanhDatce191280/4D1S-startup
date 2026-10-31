# MVP Scope & Vertical Slice Boundary (M0 — M8)

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mục tiêu cốt lõi:** Xác định ranh giới nghiêm ngặt cho bản Vertical Slice (Milestone 8) — sản phẩm chơi được hoàn chỉnh đầu tiên trước khi mở rộng quy mô.

---

## 1. Định Nghĩa Vertical Slice (M8)

**Vertical Slice** là một lát cắt hoàn chỉnh xuyên suốt tất cả các lớp của trò chơi (từ đồ họa, camera, điều khiển, sinh tồn, chế tạo, chiến đấu, AI, chu kỳ thời gian đến lưu/tải dữ liệu) nhưng chỉ diễn ra trong một phạm vi không gian và nội dung nhỏ hẹp.

* **Tiêu chuẩn nghiệm thu tối thượng:** Một người chơi mới có thể khởi động game từ file `.exe` độc lập, trải nghiệm trọn vẹn 1 chu kỳ từ sáng đến đêm, thu thập tài nguyên, chế tác vũ khí, chiến đấu, bảo vệ ngôi làng qua đêm Huyết Nguyệt đầu tiên và lưu lại tiến độ mà không cần bất kỳ thao tác thủ công nào trong Unity Editor.

---

## 2. Những Gì CÓ Trong Phạm Vi MVP / Vertical Slice (In-Scope M1 — M8)

| Phân hệ | Nội dung cụ thể trong Vertical Slice | Ghi chú kỹ thuật |
| :--- | :--- | :--- |
| **Không gian & Thế giới** | • 1 khu vực nhỏ của **Biome 1 (Rừng Rậm Trường Sơn)**.<br>• Trung tâm là **Làng Khai Hoang** (Đình Làng, Kho lương, Cổng Làng).<br>• Khu rừng xung quanh với suối cạn và đường mòn. | Scene duy nhất: `Biome1_JungleVillage.unity`. Dùng địa hình phẳng + dốc nhẹ và NavMesh tĩnh. |
| **Nhân vật & Điều khiển** | • 1 nhân vật người chơi góc nhìn Isometric 3/4.<br>• Di chuyển 8 hướng, chạy nhanh, lăn né (`Roll`), xoay theo con trỏ chuột. | Prefab `Player` với `CharacterController` + `CinemachineVirtualCamera`. |
| **4 Thanh Trạng Thái** | • `HP`: Giảm khi trúng đòn/chết đói; hồi khi ăn no (`Hunger > 80%`).<br>• `Hunger`: Giảm theo thời gian; hồi khi ăn đồ ăn.<br>• `Stamina`: Tiêu hao khi chạy, lăn né, chém, đốn gỗ; tự hồi khi nghỉ.<br>• `Mana`: Dùng thi triển kỹ năng cơ bản. | `PlayerStatsComponent` + `SurvivalConfigSO`. Hiển thị trên HUD góc trái. |
| **Chiến đấu cơ bản** | • 1 vũ khí cận chiến: `Trường Kiếm Sắt Ấp Làng` (hoặc `Mã Tấu Đi Rừng`).<br>• Đánh thường (combo 2 nhát), Cooldown, Hitbox va chạm. | `HealthComponent`, `IDamageable`, `OverlapSphereNonAlloc`. |
| **Kẻ địch & Thú rừng** | • 1 loài thú rừng: `Lợn Rừng Trường Sơn` (bán hung dữ, cung cấp thịt).<br>• 1 loại quái vật: `Sói Bậc 1 (Huyết Lang Sơ Khởi)` (hoạt động ban đêm). | AI FSM cơ bản: `Patrol` $\to$ `Detect` $\to$ `Chase` $\to$ `Attack` $\to$ `Return`. |
| **Khai thác & Nhặt đồ** | • Chặt Cây Lấy Gỗ, Đập Đá Lấy Đá Cuội, Hái Quả Rừng.<br>• Tài nguyên có trạng thái cạn kiệt (`Depleted`) và tự mọc lại (`Respawn`). | `ResourceNode` + `WorldItemPickup`. |
| **Túi đồ & Chế tạo** | • Bảng Túi đồ $4 \times 5$ ô, trang bị vũ khí.<br>• Chế tạo cơ bản: Rìu Đá, Đuốc Sáng, Cơm Lam Ống Nứa. | `InventorySystem` (C# thuần) + `CraftingRecipeSO` + uGUI. |
| **Thời gian & Sự kiện Đêm** | • Đồng hồ Ngày/Đêm (1 ngày game = 15-20 phút thực tế, có nút tua nhanh test).<br>• Chu kỳ Trăng cơ bản (Trăng Thường vs Đêm Huyết Nguyệt).<br>• Sự kiện **Blood Moon Raid**: Sinh đợt Sói Bậc 1 tấn công cổng làng. | `GameTimeManager` + `NightRaidDirector`. Đổi màu ánh sáng sang đỏ rực khi có Huyết Nguyệt. |
| **NPC & Nhiệm vụ** | • 1 NPC **Trưởng Làng** đứng ở Đình Làng.<br>• Giao 3 nhiệm vụ tuần tự: (1) Khai thác & nấu Cơm Lam $\to$ (2) Đi săn Sói ban đêm $\to$ (3) Sống sót qua đêm Huyết Nguyệt. | Hệ thống hội thoại (`DialogueTrigger`) và theo dõi mục tiêu nhiệm vụ đơn giản. |
| **Giao diện & Lưu game** | • Main Menu, Pause Menu, Game Over (khi chết / Đình Làng sập), Victory Screen.<br>• Lưu/Tải JSON: Vị trí, 4 thanh chỉ số, Túi đồ, Ngày giờ thế giới. | `SaveLoadService` (Local JSON file). |

---

## 3. Những Tính Năng BẮT BUỘC HOÃN (Deferred to M9 — M15)

Tuyệt đối **không lập trình hoặc tạo asset** cho các tính năng dưới đây trước khi Vertical Slice (M8) được nghiệm thu:

1. **Hoãn đến M9:** Lịch sinh hoạt 24/7 của hàng loạt NPC dân làng, NPC tự đi chặt cây/nâng cấp công trình.
2. **Hoãn đến M10:** Người Sói giả dạng thương nhân, Bẫy hàng hóa, Đuốc Ngải Cứu, Gương Đồng Chiếu Yêu, Chuông Gió.
3. **Hoãn đến M11:** 5 nhánh thông thạo Albion Mastery (Khai Khoáng, Nông Gia, Kiến Trúc, Binh Khí, Huyết Thống), Khảm 5 Đá Nguyên Tố, Kỹ năng nhánh Pháp Sư / Thợ Săn.
4. **Hoãn đến M12:** Hệ thống xây dựng tự do tường thành 5 cấp, Bẫy tháp nỏ, Bẫy lưới bạc, AI Sói nhảy tường / phá tường.
5. **Hoãn đến M13:** 4 Biomes còn lại (Đầm Lầy, Sa Mạc, Hang Động, Núi Tuyết), Hệ thống 4 tầng Hầm mỏ, Tộc Huyết Thi, 9 Bậc Sói, Sói Vua Cấp 9.
6. **Hoãn đến M14:** Hệ thống Phe phái (Người Lùn 3 cấp, Phù Thủy, Hội Săn Sói), Hợp Thể Người-Sói, Kinh tế chênh lệch vùng miền, 4 Endings.
7. **Hoãn đến M15:** Lưu game đa vùng phức tạp, Tối ưu hóa sâu (Object Pooling diện rộng, Texture Streaming).

