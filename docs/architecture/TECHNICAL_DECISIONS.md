# Technical Decisions & Architecture Guidelines

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Tài liệu tham chiếu:** Architecture Decision Records (ADR)  
**Trạng thái:** ADR-01–08 là quyết định nền; cập nhật 31/10/2026 theo `docs/vision/PRODUCT_CANON.md`. Các phương án kỹ thuật mới phải qua spike, không coi toàn bộ nội dung đề xuất là Approved.

---

## 1. Danh Sách Quyết Định Kiến Trúc Cốt Lõi (Core ADRs)

### ADR-01: Game Engine & Render Pipeline
* **Quyết định:** Sử dụng **Unity 6 LTS (phiên bản đã cài: 6000.3.27f1)** kết hợp **Universal Render Pipeline (URP)** và ngôn ngữ **C# (.NET Standard 2.1)**.
* **Lý do kỹ thuật:**
  * C# cực kỳ gần gũi với nền tảng Java OOP, giúp đội ngũ tiếp cận nhanh chóng mà không gặp trở ngại lớn về cú pháp.
  * URP được chọn cho đồ họa 3D Stylized, hỗ trợ ánh sáng động Ngày/Đêm, sương mù Đầm Lầy và hiệu ứng nguyên tố với chi phí hiệu năng thấp trên GPU tầm trung (RTX 4060, GTX 1650+).
  * Unity 6 LTS sở hữu hiệu năng CPU cao hơn, cải tiến GPU Resident Drawer và hệ thống URP ổn định lâu dài.
* **Phương án loại bỏ:** Unreal Engine 5 (quá nặng, thời gian compile C++ lâu, khó review logic bằng AI qua Git).

### ADR-02: Phong Cách Đồ Họa & Góc Nhìn Camera
* **Quyết định:** Đồ họa **3D Stylized Low/Mid-Poly** với góc nhìn **Isometric / 3/4 cố định** (lấy cảm hứng từ *Albion Online*), sử dụng **Cinemachine Virtual Camera**. Camera không xoay tự do $360^\circ$.
* **Lý do kỹ thuật:**
  * Mục tiêu giảm khối lượng tạo dựng asset môi trường (chưa có tỷ lệ đo) vì chỉ cần tối ưu các mặt nhìn thấy từ góc $45^\circ - 55^\circ$.
  * Kiểm soát bóng đổ và ánh sáng mặt trời theo hướng cố định, tránh hiện tượng lóa nắng hoặc mất tương phản hình khối.
  * Giữ vững trải nghiệm quan sát bao quát chiến trường, đặt bẫy và nhận diện tầm nhìn ban đêm (Fog of War).

### ADR-03: Cấu Trúc Thế Giới & Cơ Chế Khai Khoáng 4 Tầng
* **Quyết định:** Mô hình **Zone-based World** (chia thành các khu vực kết nối qua cổng chuyển vùng) + **Cửa Hầm Mỏ Chuyển Tầng (Instanced Sub-scenes)** + **Cơ chế Độn Thổ (Burrow State)** cho quái vật đào ngầm.
* **Lý do kỹ thuật:**
  * Không làm địa hình Voxel đào bới tự do mọi nơi kiểu Minecraft vì chi phí dựng lưới 3D động và nướng NavMesh thời gian thực quá nặng cho dự án.
  * Bước vào cửa mỏ sẽ load Scene hang mỏ tương ứng (`Additive Scene Loading`), vừa đúng chất Albion Online, vừa dễ cô lập ánh sáng tối hoàn toàn và đảm bảo FPS.
  * `Độn Thổ Thi` lặn xuống đất (bật hiệu ứng bụi đất `VFX_Dirt`, tắt va chạm) và trồi lên bên trong làng nếu chưa nâng cấp móng tường thành, không cần đục lỗ sàn 3D thật.

### ADR-04: Sói Vua & Vòng Lặp Sinh Tồn Hardcore (Fail Condition)
* **Canon bổ sung:** Chỉ một vua tier 9; vua chết kết thúc world, phe Sói thắng loài người thì vua tiến tier 10 và kết thúc. Không respawn mỗi trăng. Cận vệ tier thấp hơn; adaptive tactics ghi nhớ trận thất bại trong world là phương án đề xuất, xem `docs/design/FACTIONS_DEATH_AND_ENDGAME.md`.
* **Quyết định:** Bầy sói tha mồi về hang nuôi Sói Vua tăng chỉ số vĩnh viễn mỗi đêm không đặt trần. Khi Sói Vua công làng, toàn quân nhận buff `x3 HP/Stamina`.
* **Lý do kỹ thuật & Gameplay:**
  * Đây là điều kiện thua cuộc chủ ý (`Fail Condition` của game Hardcore Survival). Người chơi buộc phải chủ động áp dụng chiến thuật **Đánh Phủ Đầu** chặn đường tiếp tế thịt ban đêm để kìm hãm đà tăng trưởng của Sói Vua.
  * Để thuận tiện cho việc cân bằng (Balancing) trong giai đoạn phát triển, toàn bộ hệ số tăng trưởng mỗi đêm và tùy chọn "Bật/Tắt Giới hạn theo Hồi (Act Cap)" được lưu trong **`ScriptableObject` (`WolfKingConfigSO`)**.

### ADR-05: Hệ Thống Nghi Kỵ & 3 Tầng Phát Hiện Người Sói Giả Dạng
* **Quyết định:** Tách biệt danh tính ẩn (`TraderIdentityComponent`) khỏi Model hiển thị. Phân chia hệ thống phát hiện thành 3 tầng:
  1. *Tầng Cảnh báo diện rộng:* `Chuông Gió Đồng Đen` hoặc thú cưng `Voọc Ngũ Sắc` hú báo động *"Có mùi máu trong đoàn buôn"*.
  2. *Tầng Nghi vấn:* Soi hàng hóa trong bảng giao dịch (bán thịt sấy bí ẩn, đá nhuộm máu giá rẻ).
  3. *Tầng Xác thực:* Đốt `Đuốc Ngải Cứu & Bồ Kết` (Sói giả dạng ho khói đen) hoặc soi `Gương Đồng Chiếu Yêu Đông Sơn` để vạch mặt đích danh.
* **Hình phạt đánh nhầm:** Nếu người chơi tấn công Người Du Mục thật, thương đoàn sẽ bỏ chạy và **ngừng ghé làng trong 7 ngày** (cắt đứt nguồn cung hàng hóa).

### ADR-06: Chuẩn Hóa Mana, Hóa Sói & Albion Mastery
* **Quyết định:** 
  * **Stamina:** Dùng cho vận động thể chất (chạy, lăn né, chém). Ở dạng Sói, giữ hướng Stamina không giới hạn của handoff hiện hành; phải kiểm chứng cân bằng với Mana hữu hạn, không đổi ngầm sang luật khác.
  * **Mana:** Đóng vai trò là **Thanh Năng Lượng Duy Trì Biến Hình (Duration / Blood Energy)**. Mỗi giây ở lốt Sói, Mana rút dần đều; khi Mana $= 0$, nhân vật kiệt sức biến trở lại thành người.
  * Khi ở dạng Sói, Mana được dùng làm **Huyết Năng** để tung kỹ năng Huyết Thống.
  * Mastery tăng theo hành động sử dụng hợp lệ (ví dụ rìu), không theo class khóa cứng; phe thật và dạng cơ thể tách khỏi trang bị.
  * Kỹ năng chiến đấu gắn liền với **Vũ khí & Giáp đang mặc** (triết lý Albion). 3 danh hiệu (`Kiếm Sĩ`, `Thợ Săn`, `Pháp Sư`) là các nhánh Chuyên môn hóa (`Specialization`).

### ADR-07: Mô Phỏng Thế Giới Lai (Hybrid Simulation)
* **Quyết định:** Sử dụng hệ thống **`WorldEventScheduler`** chạy ngầm để tính toán mô phỏng toán học:
  * Chỉ tick khi phiên Running; không tính bù thời gian offline. Authority duy nhất mô phỏng cho mọi người, activation là hợp phạm vi của các player, không spawn trùng theo client.
  * Phe Sói và quái vật liên tục tiến hóa, thu thập thịt và nhận vũ khí từ Người Lùn theo thời gian thế giới.
  * **Chỉ Spawn thực thể 3D vật lý** khi người chơi tiến vào phạm vi kích hoạt ($50\text{m}$) hoặc khi nhận tin tình báo từ Người Lùn.
  * Mục tiêu giảm tải CPU; FPS phải đo trên fixture 6 người, không bảo đảm chỉ nhờ scheduler.

### ADR-08: Pipeline art/audio AI và placeholder M1 — M8
* **Quyết định cập nhật:** Đội tự làm art/audio bằng AI, không thuê; M1–M8 ưu tiên primitive/placeholder, asset AI được kiểm tra hoặc nguồn có quyền sử dụng rõ. Mixamo là tùy chọn pipeline, không giả định AI sinh ra model đã rig/animation/game-ready. Giữ manifest nguồn và công đoạn import/LOD/audio normalization.
* **Lý do kỹ thuật:** Tách biệt hoàn toàn `VisualRoot` (Mesh 3D, Animator) khỏi tầng Logic (`CharacterController`, `HealthComponent`). Đội ngũ tập trung 100% hoàn thành lối chơi cốt lõi của Vertical Slice trước khi tạo hình chi tiết các model Việt Nam.

---

## 2. Quy Tắc Lập Trình & Thiết Kế Code (Coding & Design Principles)

1. **Tránh Overengineering (Rule 5):** Không tự tạo EventBus phức tạp, không nhúng microservices, không dùng ECS/DOTS khi chưa có lý do hiệu năng cụ thể.
2. **Data-Driven với `ScriptableObject`:** Không hard-code chỉ số máu, sát thương, công thức chế tạo trong script `MonoBehaviour`. Mọi chỉ số cấu hình phải nằm trong file `.asset` của `ScriptableObject`.
3. **Loose Coupling bằng C# Action/Events:** Giao tiếp giữa các module (ví dụ: `HealthComponent` $\to$ `UI_HealthBar`) sử dụng `System.Action` hoặc C# events.
4. **Không cấp phát bộ nhớ rác (Garbage Collection) trong `Update()`:** 
   * Tuyệt đối không dùng `new` đối tượng, không gọi `GetComponent()`, không cộng chuỗi `string + string` trong các hàm chạy liên tục (`Update`, `FixedUpdate`).
   * Sử dụng các hàm non-allocating cho vật lý: `Physics.OverlapSphereNonAlloc` thay cho `Physics.OverlapSphere`.


## 3. ADR bổ sung sau quyết định sản phẩm 31/10/2026

### ADR-09: LAN 4–6 và host migration
Yêu cầu đã chốt: world riêng, không 24/7; khi chủ phòng tắt máy phải tự chuyển authority sang peer còn lại. Thiết kế baseline/quorum/state sync và giới hạn phòng ít peer trong `SESSION_AND_PERSISTENCE.md`; package mạng chưa chọn. M6N phải chứng minh graceful và abrupt migration trước nội dung world lớn. Owner không đồng nghĩa current authority.

### ADR-10: Persistence và private state
Authority duy nhất xác nhận giao dịch, WorldId/epoch/commit chống ghi đè save cũ. Lưu cả world, player, mộ, phép hồi sinh, finite stones, AI memory, faction/quest riêng; snapshot/version/backup từ M6N. Host là máy được tin cậy, không hứa chống chủ máy đọc identity.

### ADR-11: Luật phe/chết/endgame
Theo PRODUCT_CANON và `docs/design/FACTIONS_DEATH_AND_ENDGAME.md`: player tự chọn/giấu phe, mộ chứa tất cả đồ, chỉ phép đồng đội hồi sinh, team wipe là bad ending; world outcome khác achievement cá nhân. Số cooldown/cost và các edge-case trong spec là baseline cần test, không là dữ kiện đã cân bằng.

### ADR-12: Hiệu năng và nhân lực
Mục tiêu phổ cập tương tự Albion chưa phải cấu hình đã đo. Profile riêng host 6 người/client; hạn chế AI/entity budget, không dùng runtime LLM mặc định. 6 người mới làm game, AI hỗ trợ gần toàn bộ code/art/audio; mọi thay đổi phải có bằng chứng build/test và owner con người.
