# Kế Hoạch Triển Khai Chi Tiết: Milestone M3 (Combat Prototype)

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Cột mốc:** `M3 — Combat Prototype`  
**Trạng thái:** Đang triển khai (IN_PROGRESS)

---

## A. Overview (Tổng Quan)

* **Mục tiêu:** 
  1. Xây dựng hệ thống Máu & Nhận Sát Thương tái sử dụng (`HealthComponent` + interface `IDamageable`) dùng chung cho cả Người chơi, Quái vật và Công trình phòng thủ về sau.
  2. Tạo cấu hình vũ khí `WeaponDataSO` (`ScriptableObject`) cho vũ khí cận chiến đầu tiên: **Trường Kiếm Sắt Ấp Làng** (Sát thương, tầm đánh, góc quét hình quạt, thời gian hồi chiêu Cooldown).
  3. Lập trình bộ điều khiển chiến đấu `PlayerCombatController`: Nhận tín hiệu chém từ chuột trái / Space, vung kiếm, kiểm tra trúng đòn bằng thuật toán hình quạt không cấp phát rác (`Physics.OverlapSphereNonAlloc`).
  4. Lập trình AI quái vật cơ bản bằng Máy trạng thái hữu hạn (**FSM - Finite State Machine**) kết hợp **`NavMeshAgent`**: Tuần tra ngẫu nhiên (`Patrol`) $\to$ Phát hiện người chơi (`Detect`) $\to$ Đuổi theo (`Chase`) $\to$ Tấn công cận chiến (`Attack`) $\to$ Quay về điểm gốc nếu người chơi chạy quá xa (`Return`).
  5. Thiết lập phản hồi chiến đấu trực quan: Nhấp nháy màu đỏ khi trúng đòn (`Hit Flash`), thanh máu lơ lửng trên đầu (`World Space Health Bar`), hiệu ứng ngã quỵ khi máu về 0.
  6. Dựng đấu trường kiểm thử `Test_CombatArena.unity` có bia tập đánh bất tử và Sói Bậc 1 Prototype.
* **Lý do cần milestone:** Đảm bảo vòng lặp chiến đấu cận chiến 3D thỏa mãn cảm giác "chặt chém" mượt mà, tính toán sát thương chính xác và AI quái vật không bị đơ hoặc lỗi va chạm trước khi kết hợp với hệ thống Sinh tồn (M4) và Túi đồ (M5).
* **Phạm vi bao gồm:** Health system, Weapon Data, Melee Hitbox, Basic Enemy FSM, Hit Feedback, World Space UI.
* **Phạm vi không bao gồm:** Chưa làm cây kỹ năng nghề nghiệp, chưa làm 9 Bậc Sói biến hình, chưa khảm Ngũ Linh Thạch (các tính năng này thuộc M10, M11).

---

## B. Prerequisites (Điều Kiện Tiên Quyết)

* **Milestone phụ thuộc:** `M2` (Đã hoàn thành, nhân vật di chuyển và camera bám mượt mà).
* **Kiến trúc tham chiếu:** `docs/architecture/GLOSSARY.md` (định danh `HealthComponent`, `IDamageable`, `DamageInfo`, `WerewolfTier.Tier1_Feral`).

---

## C. Learning Plan (Khái Niệm Cần Học Cho Java Web Developer)

### 1. Khái niệm: Loose Coupling qua Interface `IDamageable` & `DamageInfo`
* **Tại sao cần:** Trong game, người chơi có thể chém trúng: Quái vật Sói, Lợn Rừng, Cây Cối, Thùng gỗ, hoặc thậm chí là Tường Làng. Nếu bạn viết code:
  `if (target.GetComponent<Enemy>() != null) { ... } else if (target.GetComponent<Tree>() != null) { ... }`
  Code sẽ nhanh chóng biến thành thảm họa "mỳ ống" (`Spaghetti code`)!
* **Cách hoạt động (Chuẩn OOP):**
  * Định nghĩa interface:
    ```csharp
    public interface IDamageable
    {
        void TakeDamage(DamageInfo damageInfo);
    }
    ```
  * Mọi đối tượng có máu (Player, Quái, Cây, Tường) chỉ cần triển khai `IDamageable`. Khi người chơi vung kiếm, chỉ cần kiểm tra:
    ```csharp
    if (hitCollider.TryGetComponent<IDamageable>(out var damageable))
    {
        damageable.TakeDamage(info);
    }
    ```
* **Liên hệ Java/Web:** Giống hệt thiết kế `Repository` hoặc `NotificationService` interface trong Spring Boot — Controller chỉ gọi `service.sendNotification()` mà không cần quan tâm bên dưới là Email, SMS hay Push Notification!

### 2. Khái niệm: Quét Hitbox Không Cấp Phát Bộ Nhớ (`Physics.OverlapSphereNonAlloc`)
* **Tại sao cần:** Trong Unity, lệnh `Physics.OverlapSphere()` sẽ cấp phát một mảng `Collider[]` mới trên bộ nhớ Heap mỗi lần vung kiếm. Nếu 10 quái vật cùng chém liên tục 60 frame/giây, bộ nhớ rác (Garbage Collector) sẽ đầy rất nhanh và gây hiện tượng khựng khung hình (Micro-stutter).
* **Cách hoạt động:** Cấp phát sẵn một mảng cố định (`static readonly Collider[] HitBuffer = new Collider[16];`), và dùng hàm `Physics.OverlapSphereNonAlloc` để ghi đè kết quả vào mảng này mà không tốn thêm 1 byte RAM nào.
* **Thuật toán hình quạt (Cone/Arc Hitbox):**
  1. Tìm các vật thể trong bán kính tầm kiếm ($R = 2.5\text{m}$).
  2. Kiểm tra góc giữa hướng mặt nhân vật và hướng tới mục tiêu: `Vector3.Angle(transform.forward, toTarget) <= halfAngle (60°)`. Nếu góc $\le 60^\circ$ thì mục tiêu nằm trong vùng chém trước mặt!

### 3. Khái niệm: Máy Trạng Thái Hữu Hạn Cho AI Quái Vật (FSM)
* **Cách hoạt động:** Một con quái vật tại một thời điểm chỉ ở duy nhất 1 trạng thái:
  * `Patrol`: Đi loanh quanh các điểm mốc ngẫu nhiên với tốc độ chậm ($2.5\text{ m/s}$).
  * `Chase`: Khi khoảng cách tới người chơi $< 10\text{m}$, gầm lên và chạy nhanh về phía người chơi ($6.0\text{ m/s}$).
  * `Attack`: Khi khoảng cách $< 1.8\text{m}$, dừng lại và vung vuốt cào (Cooldown $1.5\text{s}$).
  * `Return`: Nếu người chơi chạy xa $> 18\text{m}$, ngừng truy đuổi và quay về điểm xuất phát.
* **Liên hệ Java/Web:** Giống như vòng đời của một Đơn Hàng trong thương mại điện tử (`OrderState`: `CREATED` $\to$ `PROCESSING` $\to$ `SHIPPED` $\to$ `DELIVERED`).

---

## D. Implementation Tasks (Chi Tiết Các Nhiệm Vụ)

### `M3-T1`: Xây Dựng `IDamageable`, `DamageInfo` & `HealthComponent`
* `DamageInfo`: Struct chứa `amount` (lượng sát thương), `damageType` (`Physical`, `Fire`, `Ice`...), `source` (`GameObject` gây sát thương), `hitPoint`.
* `HealthComponent`: Quản lý `CurrentHealth`, `MaxHealth`, `IsDead`, phát các sự kiện C# `OnDamaged`, `OnHealed`, `OnDied`.
* Viết EditMode Unit Tests cho toàn bộ logic máu (trừ máu, hồi máu không vượt quá max, chết khi máu $\le 0$).

### `M3-T2`: Tạo ScriptableObject Cấu Hình Vũ Khí (`WeaponDataSO`)
* Tạo class `WeaponDataSO` lưu trữ: Tên vũ khí, Phân loại (`WeaponCategory.Sword`), Sát thương cơ bản ($25\text{ dmg}$), Tầm chém ($2.2\text{m}$), Góc quét ($110^\circ$), Thời gian hồi chiêu Cooldown ($0.5\text{s}$), Lực đẩy lùi Knockback ($3.0$).
* Tạo asset file mẫu: `Assets/_Project/Data/Items/Weapon_IronSword_T1.asset`.

### `M3-T3`: Lập Trình `PlayerCombatController`
* Lắng nghe tín hiệu `AttackTriggered` từ `IPlayerInput`.
* Quản lý Cooldown đòn đánh.
* Thuật toán quét hình quạt `PerformMeleeAttack()` bằng `Physics.OverlapSphereNonAlloc`.
* Bật Gizmos vẽ vùng chém màu vàng/đỏ trong Editor để lập trình viên dễ quan sát và cân chỉnh.

### `M3-T4`: Lập Trình AI Quái Vật FSM (`EnemyStateMachine` + `NavMeshAgent`)
* Tạo `EnemyBrain.cs` kết hợp `NavMeshAgent` điều khiển Sói Bậc 1:
  * Trạng thái `Patrol` quanh bán kính $8\text{m}$.
  * Trạng thái `Chase` khi phát hiện Player trong $10\text{m}$.
  * Trạng thái `Attack` cào cấu khi cách Player $< 1.8\text{m}$ (gây $15\text{ dmg}$).
  * Trạng thái `Die`: Tắt AI, tắt va chạm, chờ 3 giây rồi Despawn.

### `M3-T5`: Phản Hồi Chiến Đấu Trực Quan (Hit Feedback & Floating Health Bar)
* Tạo script `HitFeedbackFlash`: Khi nhận sát thương, tạm thời đổi màu Material sang đỏ tươi trong $0.15\text{s}$ rồi phục hồi.
* Tạo `WorldSpaceHealthBar`: Canvas nhỏ lơ lửng trên đầu quái, luôn xoay mặt về phía Camera (`Billboard`), co dãn thanh máu theo tỷ lệ `CurrentHealth / MaxHealth`.

### `M3-T6`: Dựng Scene `Test_CombatArena.unity` Bằng Script 1-Click
* Tạo menu `Huyết Nguyệt Đại Ngàn` $\to$ `M3: Setup Combat Prototype Arena`.
* Dựng đấu trường có nướng NavMesh, đặt 1 Bia Tập Đánh bất tử (`Combat_Dummy`) và 1 Sói Bậc 1 Prototype biết đuổi cắn.

---

## E. Acceptance Criteria (Tiêu Chí Nghiệm Thu M3)

- [ ] Bấm chuột trái hoặc phím Space, nhân vật vung kiếm chém trúng mục tiêu trong tầm quét trước mặt.
- [ ] Mục tiêu trúng đòn bị nhấp nháy đỏ, thanh máu trên đầu tụt đúng số sát thương quy định trong `WeaponDataSO`.
- [ ] Không thể chém liên tục không giới hạn (tuân thủ thời gian hồi chiêu Cooldown).
- [ ] Quái vật Sói tự động phát hiện người chơi khi tới gần, chạy đuổi theo và ra đòn cào gây sát thương cho người chơi.
- [ ] Khi máu của quái vật về 0, quái chuyển sang trạng thái chết, ngừng di chuyển/tấn công và biến mất sau 3 giây.
- [ ] Đạt 100% Pass bộ Unit Tests của `HealthComponent`.

