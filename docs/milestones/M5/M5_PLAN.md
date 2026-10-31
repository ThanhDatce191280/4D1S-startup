> **Đối chiếu 31/10/2026:** READY_FOR_VERIFICATION theo CURRENT_STATE; checklist cũ không thay bằng chứng build. Nội dung bên dưới là plan prototype lịch sử; chưa bao phủ LAN/host migration/phe ẩn mới. Canon tại `docs/vision/PRODUCT_CANON.md`, phần còn thiếu được kiểm ở M5R và roadmap mới.

# Kế Hoạch Triển Khai Chi Tiết: Milestone M5 (Inventory, Items & Crafting)

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Cột mốc:** `M5 — Inventory, Items & Crafting`  
**Trạng thái:** Đang triển khai (IN_PROGRESS)

---

## A. Overview (Tổng Quan)

* **Mục tiêu cốt lõi:**
  1. Xây dựng hệ thống định nghĩa vật phẩm chuẩn hóa (`ItemDataSO` - ScriptableObject) hỗ trợ 5 phân loại: `Resource` (Nguyên liệu thô), `Weapon` (Vũ khí), `Tool` (Công cụ), `Consumable` (Thức ăn/Thuốc), `Armor` (Trang bị bảo hộ).
  2. Xây dựng mô hình dữ liệu Túi Đồ (`InventorySystem` & `InventorySlot`):
     * Lưới $4 \times 5 = 20$ ô chứa đồ.
     * Hỗ trợ cộng dồn số lượng (`Stacking`) theo `maxStackSize`.
     * Tự động chia slot khi đầy stack hoặc rớt sang ô trống tiếp theo.
     * Cung cấp các thao tác C# thuần (Tách stack, đổi vị trí, sử dụng vật phẩm, xóa vật phẩm).
  3. Xây dựng hệ thống Chế Tạo (`CraftingSystem` & `CraftingRecipeSO`):
     * Kiểm tra nguyên liệu trong túi đồ và chế tạo vật phẩm mới.
     * Cung cấp 4 công thức sinh tồn cơ bản ban đầu của Vertical Slice:
       1. **Rìu Đá Thô Sơ (`tool_stone_axe_t1`)**: $3$ Gỗ + $2$ Đá.
       2. **Đuốc Sáng Đi Rừng (`tool_torch_t1`)**: $2$ Gỗ + $1$ Quả Rừng.
       3. **Cơm Lam Ống Nứa (`consumable_bamboo_rice`)**: $2$ Gỗ + $3$ Quả Rừng (Ăn hồi $40$ Hunger + $25$ HP).
       4. **Trường Kiếm Sắt Ấp Làng (`weapon_sword_iron_t1`)**: $5$ Gỗ + $5$ Đá.
  4. Tích hợp Trang Bị & Sử Dụng Vật Phẩm:
     * Nhấp chuột phải / nút Dùng: Nếu là Vũ Khí $\to$ Tự động gán vào `PlayerCombatController`. Nếu là Thức ăn $\to$ Hồi máu/độ đói trong `PlayerStatsComponent` và trừ 1 vật phẩm.
  5. Thiết kế Giao Diện Túi Đồ & Bàn Chế Tạo (Inventory & Crafting Window):
     * Phím tắt `I` hoặc `Tab` bật/tắt cửa sổ.
     * Thiết kế kính mờ trong suốt (Smoked Glass / Dark Fantasy Gold Rim) đồng bộ thẩm mỹ với HUD M4.
     * Khung bên trái: Lưới 20 ô túi đồ có hiển thị icon/tên/số lượng và tooltip chi tiết.
     * Khung bên phải: Danh sách công thức chế tạo, tô màu nổi bật các công thức đủ nguyên liệu và nút bấm "Chế Tạo".
  6. Tích hợp với `WorldItemPickup` từ M4:
     * Khi người chơi nhặt gỗ, đá, quả ngoài đất $\to$ Tự động thêm vào `InventorySystem` thay vì chỉ tăng biến số đếm tĩnh!
  7. Bộ Unit Tests (EditMode):
     * Kiểm thử trọn vẹn logic thêm đồ, stack đồ, kiểm tra nguyên liệu, chế tạo thành công và chế tạo thất bại khi thiếu đồ.

---

## B. Prerequisites & Dependencies (Điều Kiện Tiên Quyết)

* **Milestone phụ thuộc:** `M3` (Combat & WeaponDataSO), `M4` (Survival Core, PlayerStatsComponent, WorldItemPickup).
* **Kiến trúc tham chiếu:** `docs/architecture/GLOSSARY.md` (Mục 8: `weapon_sword_iron_t1`, `consumable_bamboo_rice`).

---

## C. Implementation Tasks (Chi Tiết Các Nhiệm Vụ)

### `M5-T1`: Định Nghĩa Vật Phẩm `ItemDataSO.cs`
* ScriptableObject chứa:
  * `string itemId`: Khóa định danh duy nhất (VD: `resource_wood`, `consumable_bamboo_rice`).
  * `string itemName`: Tên tiếng Việt hiển thị (VD: "Gỗ Khúc Rừng Già", "Cơm Lam Ống Nứa").
  * `string description`: Mô tả công dụng và cốt truyện dân gian.
  * `ItemType itemType`: `Resource`, `Consumable`, `Weapon`, `Tool`, `Armor`.
  * `int maxStackSize`: Giới hạn cộng dồn (Resource: 99, Consumable: 10, Weapon: 1).
  * `Sprite icon`: Hình ảnh đại diện (hỗ trợ vẽ placeholder nếu chưa có sprite).
  * `float healthRestore`: Lượng máu hồi khi dùng (nếu là Consumable).
  * `float hungerRestore`: Lượng thức ăn hồi khi dùng (nếu là Consumable).
  * `WeaponDataSO weaponData`: Dữ liệu chiến đấu (nếu là Weapon/Tool).

### `M5-T2`: Lập Trình Lõi Dữ Liệu `InventorySystem.cs` & `InventorySlot.cs`
* `InventorySlot`:
  * `ItemDataSO Item { get; }`
  * `int Quantity { get; }`
  * `bool IsEmpty => Item == null || Quantity <= 0;`
  * `int Add(int amount)` / `int Remove(int amount)`
* `InventorySystem`:
  * Quản lý mảng 20 slots (`InventorySlot[] slots = new InventorySlot[20];`).
  * Cung cấp các hàm C# Action sự kiện: `event Action OnInventoryUpdated;`.
  * `bool AddItem(ItemDataSO item, int count = 1, out int remainder);`
  * `bool RemoveItem(string itemId, int count = 1);`
  * `int GetItemCount(string itemId);`
  * `bool HasItem(string itemId, int count = 1);`
  * `void UseItem(int slotIndex);`

### `M5-T3`: Lập Trình Hệ Thống Chế Tạo `CraftingSystem.cs` & `CraftingRecipeSO.cs`
* `CraftingRecipeSO`:
  * `string recipeId`, `string recipeName`.
  * `List<RecipeIngredient> ingredients`: Danh sách nguyên liệu yêu cầu.
  * `ItemDataSO resultItem`, `int resultAmount`.
* `CraftingSystem`:
  * Kết nối trực tiếp với `InventorySystem`.
  * `bool CanCraft(CraftingRecipeSO recipe)`: Quét túi đồ kiểm tra đủ từng nguyên liệu không.
  * `bool TryCraft(CraftingRecipeSO recipe)`: Trừ nguyên liệu và nạp thành phẩm vào ô trống hoặc stack túi đồ.

### `M5-T4`: Cập Nhật `WorldItemPickup` & `PlayerStatsComponent`
* Nâng cấp `WorldItemPickup` để chứa `ItemDataSO` thay vì chỉ enum `ResourceType`.
* Khi hút vào người chơi: gọi `playerInventory.AddItem(itemData, amount)` trước khi cập nhật số đếm thống kê.

### `M5-T5`: Thiết Kế Giao Diện Túi Đồ & Chế Tạo `InventoryCraftingUI.cs`
* Phím `I` / `Tab` bật/tắt toàn bộ cửa sổ.
* Giao diện kính mờ trong suốt (Smoked Glass), chia 2 cột:
  * Cột trái: Lưới 20 ô ($4 \times 5$), viền ánh kim, số lượng góc phải, nhấp vào để Xem chi tiết hoặc Sử Dụng / Trang Bị.
  * Cột phải: Danh sách các công thức chế tạo khả dụng, hiển thị trực quan nguyên liệu hiện có / yêu cầu và nút "Chế Tạo".

### `M5-T6`: Bộ Kiểm Thử Unit Tests `InventoryTests.cs` & `CraftingTests.cs`
* Kiểm thử EditMode:
  * Thêm vật phẩm vào túi rỗng.
  * Cộng dồn số lượng vượt quá `maxStackSize` tự tràn sang ô mới.
  * Trừ số lượng khi sử dụng hoặc chế tạo.
  * Chế tạo thành công khi đủ nguyên liệu và thất bại khi thiếu nguyên liệu.

### `M5-T7`: Helper Dựng Scene M5 `M5_InventorySceneSetupHelper.cs`
* MenuItem `[MenuItem("Huyết Nguyệt Đại Ngàn/M5: Setup Inventory & Crafting Scene", false, 60)]`.
* Tạo đầy đủ Item ScriptableObjects mẫu, công thức chế tạo mẫu và Scene `Test_InventoryCrafting.unity`.

---

## D. Acceptance Criteria (Tiêu Chí Nghiệm Thu M5)

- [x] Phím `I` hoặc `Tab` bật/tắt cửa sổ Túi đồ & Chế tạo mượt mà.
- [x] Bảng túi đồ hiển thị chuẩn 20 ô ($4 \times 5$), nền kính mờ sang trọng không che kín tầm nhìn.
- [x] Nhặt gỗ, đá, quả ngoài thế giới: Vật phẩm bay vào túi đồ và hiển thị đúng ô và số lượng cộng dồn.
- [x] Khi nhặt quá `maxStackSize` (ví dụ 99), tự động tạo stack mới ở ô trống tiếp theo.
- [x] Nhấp chuột vào vật phẩm thức ăn (Quả Rừng, Cơm Lam) $\to$ Hồi máu/độ đói và trừ 1 món trong túi.
- [x] Bảng Chế tạo hiển thị danh sách 4 công thức chuẩn. Khi chưa đủ nguyên liệu, nút Chế tạo bị mờ; khi đủ nguyên liệu, nút Chế tạo sáng lên.
- [x] Bấm "Chế Tạo" Cơm Lam / Rìu Đá: Trừ đúng số lượng Gỗ/Đá trong túi và sinh ra Cơm Lam / Rìu Đá vào túi đồ.
- [x] Đạt 100% Pass bộ Unit Tests `InventoryCraftingTests.cs`.
