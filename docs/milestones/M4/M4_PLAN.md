> **Đối chiếu 31/10/2026:** COMPLETED theo CURRENT_STATE; chưa tái nghiệm thu trong lần cập nhật tài liệu. Nội dung bên dưới là plan prototype lịch sử; chưa bao phủ LAN/host migration/phe ẩn mới. Canon tại `docs/vision/PRODUCT_CANON.md`, phần còn thiếu được kiểm ở M5R và roadmap mới.

# Kế Hoạch Triển Khai Chi Tiết: Milestone M4 (Survival Core)

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Cột mốc:** `M4 — Survival Core`  
**Trạng thái:** Đang triển khai (IN_PROGRESS)

---

## A. Overview (Tổng Quan)

* **Mục tiêu:** 
  1. Xây dựng hệ thống 4 thanh trạng thái sinh tồn (`HP`, `Hunger`, `Stamina`, `Mana`) theo đúng Mục 3.1 GDD và kết nối đồng bộ với hành vi của người chơi:
     * `Hunger` (Độ no/đói): Tiêu hao dần theo thời gian. Khi đói cạn kiệt ($= 0\%$), người chơi bị rút máu liên tục đến chết. Khi no nê ($> 80\%$), tự động hồi phục máu nhẹ theo thời gian.
     * `Stamina` (Thể lực): Tiêu hao khi chạy nước rút (Sprint), chém kiếm (Attack), chặt cây, đập đá. Tự động hồi phục khi đi bộ hoặc đứng yên. Khi hết Stamina, người chơi buộc phải đi bộ chậm và không thể vung kiếm.
     * `Mana` (Năng lượng): Tự hồi phục tự nhiên, dùng cho thi triển kỹ năng hoặc làm năng lượng duy trì hóa Sói sau này.
  2. Tạo cấu hình cân bằng dữ liệu `SurvivalConfigSO` (`ScriptableObject`) để dễ dàng điều chỉnh tốc độ đói, lượng tiêu hao thể lực mà không sửa code C#.
  3. Xây dựng hệ thống tài nguyên môi trường (`ResourceNode`):
     * Cây Gỗ Rừng: Chặt 3 nhát $\to$ Đổ rạp, sinh ra Gỗ Khúc rơi ngoài đất, để lại gốc cây và hẹn giờ mọc lại.
     * Mỏ Đá Cuội: Đập 4 nhát $\to$ Vỡ vụn, sinh ra Đá Cuội rơi ngoài đất, để lại mỏ cạn và hẹn giờ mọc lại.
     * Bụi Quả Rừng / Thảo Dược: Tương tác nhặt trực tiếp $\to$ Biến mất và hẹn giờ ra quả mới.
  4. Lập trình vật phẩm rơi ngoài đất (`WorldItemPickup`): Tự xoay tròn nhẹ nhàng, phát sáng nhẹ, khi người chơi tiến lại gần trong bán kính nhặt ($1.8\text{m}$) sẽ bay vào người chơi và cộng vào số lượng.
  5. Thiết kế giao diện HUD Sinh Tồn (Survival HUD): 4 thanh màu trực quan đặt ở góc trên bên trái màn hình (`HP` - Đỏ, `Hunger` - Cam, `Stamina` - Vàng, `Mana` - Xanh Dương) có hiển thị số lượng và tỉ lệ phần trăm.
  6. Dựng scene thử nghiệm `Test_SurvivalCore.unity` với các rặng cây, mỏ đá, bụi quả dại và bộ Unit Tests kiểm thử logic sinh tồn.
* **Lý do cần milestone:** Sinh tồn là cái hồn của tựa game *Huyết Nguyệt Đại Ngàn*. Vòng lặp khai thác gỗ đá để sinh tồn ban ngày và giữ thể lực để chiến đấu ban đêm bắt đầu chính xác từ cột mốc này.
* **Phạm vi bao gồm:** 4 thanh trạng thái, logic đói/hồi phục/tiêu hao thể lực, Resource Nodes (Cây, Đá, Quả), WorldItemPickup, Survival HUD, Unit Tests.
* **Phạm vi không bao gồm:** Chưa làm bảng túi đồ mở ra nhiều ô (M5), chưa ghép chu kỳ ngày đêm làm thay đổi ánh sáng thế giới (M7).

---

## B. Prerequisites (Điều Kiện Tiên Quyết)

* **Milestone phụ thuộc:** `M2` (Di chuyển mượt mà), `M3` (Combat & HealthComponent đã hoàn thành).
* **Kiến trúc tham chiếu:** `docs/architecture/GLOSSARY.md` (định danh `CurrentHunger`, `CurrentStamina`, `ResourceNode`, `WorldItemPickup`).

---

## C. Learning Plan (Khái Niệm Cần Học Cho Java Web Developer)

### 1. Khái niệm: State Tick Loop vs Game Loop
* **Trong Java Web:** Bạn thường quen với việc cập nhật trạng thái theo sự kiện (User bấm submit form $\to$ trừ số dư tài khoản). Nếu có tác vụ định kỳ, bạn dùng Spring `@Scheduled(fixedRate = 5000)` (mỗi 5 giây chạy 1 lần).
* **Trong Game Engine:** Game chạy 60 frame mỗi giây. Nếu mỗi frame đều tính toán trừ thức ăn và hồi máu, bạn phải nhân với `Time.deltaTime` (khoảng $0.016\text{s}$ mỗi frame):
  $$\Delta \text{Hunger} = -\text{drainRate} \times \Delta t$$
  Nhờ đó, dù game chạy 30 FPS, 60 FPS hay 144 FPS trên máy của bạn, tốc độ tụt đói và hồi máu trong game hoàn toàn bằng nhau!

### 2. Khái niệm: State Machine của Tài Nguyên (`ResourceNode`)
* Một cây gỗ hoặc mỏ đá trong game thực chất là một máy trạng thái 3 pha:
  1. `Intact` (Nguyên vẹn): Hiển thị model cây đầy đủ lá, có Collider nhận nhát chém.
  2. `Harvesting` (Đang khai thác): Mỗi lần bị chém trúng, cây rung lắc nhẹ (`Wobble/Shake`) và rơi ra một vài mảnh dăm bào.
  3. `Depleted` (Cạn kiệt): Khi máu về 0 $\to$ Ẩn tán cây, sinh ra các khối gỗ rơi ra đất (`WorldItemPickup`), bật bộ đếm thời gian mọc lại (`RespawnTimer`).
  4. `Respawned`: Khi hết thời gian $\to$ Hiện lại cây nguyên vẹn.

---

## D. Implementation Tasks (Chi Tiết Các Nhiệm Vụ)

### `M4-T1`: Xây Dựng `SurvivalConfigSO.cs`
* ScriptableObject chứa:
  * `maxHunger = 100`, `hungerDrainPerSecond = 0.5` (200 giây tụt hết 100 điểm đói).
  * `starvationDamagePerSecond = 3.0` (khi đói = 0, mỗi giây trừ 3 máu).
  * `wellFedRegenPerSecond = 2.0` (khi no > 80%, mỗi giây hồi 2 máu).
  * `sprintStaminaDrainPerSecond = 12.0` (chạy nước rút tốn 12 thể lực/giây).
  * `attackStaminaCost = 15.0` (mỗi nhát chém kiếm tốn 15 thể lực).
  * `staminaRegenPerSecond = 20.0` (hồi 20 thể lực/giây khi đứng yên hoặc đi bộ).

### `M4-T2`: Lập Trình `PlayerStatsComponent.cs`
* Kế thừa hoặc kết nối với `HealthComponent`.
* Quản lý 4 chỉ số `HP`, `Hunger`, `Stamina`, `Mana`.
* Cung cấp các hàm kiểm tra và tiêu hao: `TryConsumeStamina(amount)`, `ConsumeMana(amount)`, `AddHunger(amount)`.
* Phát các event C# Action khi chỉ số thay đổi để UI cập nhật mà không cần thăm dò liên tục.

### `M4-T3`: Tích Hợp Thể Lực Vào `PlayerMovementController` & `PlayerCombatController`
* Trong `PlayerMovementController`: Nếu `input.IsSprinting`, kiểm tra `TryConsumeStamina()`. Nếu hết Stamina, ép tốc độ về `walkSpeed`!
* Trong `PlayerCombatController`: Trước khi chém kiếm, kiểm tra `TryConsumeStamina(15)`. Nếu không đủ thể lực, không cho vung kiếm!

### `M4-T4`: Lập Trình `ResourceNode.cs`
* Triển khai `IDamageable` để tận dụng hệ thống chém kiếm từ M3 (chém trúng cây $\to$ cây nhận sát thương và mất độ bền).
* Cấu hình loại tài nguyên: `Wood` (Cây), `Stone` (Đá), `Berry` (Quả).
* Rung lắc nhẹ khi trúng đòn (`Wobble`).
* Sinh ra `WorldItemPickup` khi cạn kiệt và hẹn giờ mọc lại (`respawnTime = 30s`).

### `M4-T5`: Lập Trình `WorldItemPickup.cs`
* GameObject nhỏ (thanh gỗ, viên đá, quả mọng) lơ lửng trên mặt đất.
* Tự động xoay tròn $90^\circ/\text{s}$ và bập bồng nhẹ hình sin (`Mathf.Sin`).
* Khi người chơi bước vào bán kính $1.8\text{m}$, tự động bay nhanh về phía người chơi (`Magnetic Pull`) và phát âm thanh/thông báo nhặt đồ.

### `M4-T6`: Thiết Kế `SurvivalHUD.cs`
* Vẽ 4 thanh trạng thái ở góc trên bên trái màn hình bằng Canvas hoặc uGUI/IMGUI tự động:
  * `HP`: Thanh đỏ (0 - 100).
  * `Hunger`: Thanh cam (0 - 100).
  * `Stamina`: Thanh vàng (0 - 100).
  * `Mana`: Thanh xanh dương (0 - 100).
  * Hiển thị số lượng tài nguyên đã nhặt: `Gỗ: X`, `Đá: Y`, `Quả: Z`.

### `M4-T7`: Dựng Scene `Test_SurvivalCore.unity` & Unit Tests
* Script 1-click tạo scene có rặng cây rừng rậm, mỏ đá xám, bụi quả đỏ mọc xen kẽ trên sàn cỏ.
* Bộ kiểm thử `SurvivalStatsTests.cs` (EditMode): Kiểm tra tiêu hao thể lực, đói tụt máu, no hồi máu.

---

## E. Acceptance Criteria (Tiêu Chí Nghiệm Thu M4)

- [x] 4 thanh trạng thái hiển thị rõ nét trên màn hình với 4 màu phân biệt (`SurvivalHUD.cs`).
- [x] Giữ `Shift` chạy nước rút thấy thanh Stamina vàng tụt dần. Khi hết Stamina, nhân vật tự động chuyển về đi bộ (`PlayerMovementController.cs`).
- [x] Vung kiếm chém tiêu hao 15 Stamina. Khi không đủ 15 Stamina không thể vung kiếm (`PlayerCombatController.cs`).
- [x] Khi đứng yên hoặc đi bộ, thanh Stamina tự động hồi phục đầy trở lại sau 0.8s trễ (`PlayerStatsComponent.cs`).
- [x] Đói tụt dần theo thời gian. Khi đói về 0, thanh máu đỏ bắt đầu bị rút dần (`PlayerStatsComponent.cs`).
- [x] Chém vào cây hoặc đập vào mỏ đá: Cây/đá rung lắc (`Wobble`), cạn kiệt khi hết độ bền, sinh ra khúc gỗ hoặc viên đá rơi ra đất (`ResourceNode.cs`).
- [x] Đi lại gần khúc gỗ/viên đá: Vật phẩm tự xoay bập bồng và hút vào người, tăng số lượng tài nguyên thu thập (`WorldItemPickup.cs`).
- [x] Cây/đá đã cạn kiệt tự động mọc lại sau thời gian quy định (`RespawnRoutine`).
- [x] Đạt 100% Pass bộ Unit Tests sinh tồn (`SurvivalStatsTests.cs`).
