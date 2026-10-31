# Glossary & Domain Model Mapping (Từ Điển Dự Án)

**Dự án:** Huyết Nguyệt Đại Ngàn

**Canon 31/10/2026:** `docs/vision/PRODUCT_CANON.md` ưu tiên về nghĩa/luật. Tên mới dưới đây là hợp đồng thiết kế đề xuất, chưa chứng minh class tồn tại; kiểm mã nguồn trước đổi API.
**Mục đích:** Nguồn tham chiếu danh pháp duy nhất (*Single Source of Truth*) ánh xạ giữa thuật ngữ tiếng Việt trong GDD và tên Class / Enum / ScriptableObject trong mã nguồn C#.

> **Quy tắc bắt buộc:** Cả 6 thành viên trong team, GPT-6 Astra và Gemini 4 Argon **phải sử dụng chính xác** các tên tiếng Anh dưới đây trong code để tránh xung đột mã nguồn.

---

## 1. Hệ Thống 4 Thanh Trạng Thái & Chỉ Số (Core Stats)

| Thuật ngữ GDD tiếng Việt | Tên Class / Enum C# | Kiểu dữ liệu / Mô tả |
| :--- | :--- | :--- |
| Thanh Máu | `Health` / `CurrentHealth`, `MaxHealth` | `float` trong `HealthComponent` |
| Thanh Thức Ăn | `Hunger` / `CurrentHunger`, `MaxHunger` | `float` trong `PlayerStatsComponent` |
| Thanh Thể Lực | `Stamina` / `CurrentStamina`, `MaxStamina` | `float` trong `PlayerStatsComponent` |
| Thanh Mana / Huyết Năng | `Mana` / `BloodEnergy` | `float` trong `PlayerStatsComponent` |
| Có Thể Nhận Sát Thương | `IDamageable` | `interface { void TakeDamage(DamageInfo info); }` |
| Thông Tin Đòn Đánh | `DamageInfo` | `struct { float amount; DamageType type; GameObject source; }` |

---

## 2. Hệ Thống 5 Biomes & Môi Trường

| Thuật ngữ GDD | Tên Enum `BiomeType` | Tên Scene Unity |
| :--- | :--- | :--- |
| Biome 1: Rừng Rậm Trường Sơn | `BiomeType.Jungle` | `Biome1_JungleVillage.unity` |
| Biome 2: Đầm Lầy Ngập Mặn | `BiomeType.Swamp` | `Biome2_Swamp.unity` |
| Biome 3: Sa Mạc Cát Trắng | `BiomeType.Desert` | `Biome3_Desert.unity` |
| Biome 4: Hang Động Karst | `BiomeType.Cave` | `Biome4_Cave.unity` |
| Biome 5: Núi Tuyết Hoàng Liên | `BiomeType.SnowMountain` | `Biome5_SnowMountain.unity` |

---

## 3. Hệ Thống 5 Đá Nguyên Tố (Ngũ Linh Thạch)

| Thuật ngữ GDD | Tên Enum `ElementType` | Hiệu Ứng Trạng Thái (`StatusEffectType`) |
| :--- | :--- | :--- |
| Đá Lửa (Hỏa) | `ElementType.Fire` | `StatusEffectType.Burn` (Sát thương theo thời gian) |
| Đá Nước (Thủy) | `ElementType.Water` | `StatusEffectType.Cleanse` / `StatusEffectType.Heal` |
| Đá Đất (Thổ) | `ElementType.Earth` | `StatusEffectType.Stun` / `ArmorBreak` |
| Đá Thực Vật (Mộc) | `ElementType.Wood` | `StatusEffectType.Root` (Trói chân) / `LifeSteal` |
| Đá Băng (Hàn) | `ElementType.Ice` | `StatusEffectType.Slow` / `Freeze` |

---

## 4. Hệ Thống Người Sói & 9 Bậc Sói

| Bậc GDD | Tên Danh Hiệu GDD | Tên Enum `WerewolfTier` | Tên Prefab / Logic |
| :--- | :--- | :--- | :--- |
| Bậc 1 | Huyết Lang Sơ Khởi | `WerewolfTier.Tier1_Feral` | `Enemy_Werewolf_Tier1` |
| Bậc 2 | Ngụy Nhân Lang | `WerewolfTier.Tier2_Disguised` | `Enemy_Werewolf_Tier2_Disguised` |
| Bậc 3 | Cuồng Trảo Lang | `WerewolfTier.Tier3_Berserker` | `Enemy_Werewolf_Tier3_Claw` (Kỹ năng nhảy tường) |
| Bậc 4 | Ám Ảnh Lang | `WerewolfTier.Tier4_Shadow` | `Enemy_Werewolf_Tier4_Stealth` (Tàng hình bóng tối) |
| Bậc 5 | Chiến Binh Lang | `WerewolfTier.Tier5_Armored` | `Enemy_Werewolf_Tier5_Warrior` (Có vũ khí Người Lùn) |
| Bậc 6 | Thống Lĩnh Lang | `WerewolfTier.Tier6_Chieftain` | `Enemy_Werewolf_Tier6_Leader` |
| Bậc 7 | Nguyên Tố Lang | `WerewolfTier.Tier7_Elemental` | `Enemy_Werewolf_Tier7_Elemental` |
| Bậc 8 | Cổ Huyết Lang Tổ | `WerewolfTier.Tier8_AncientAlpha`| `Enemy_Werewolf_Tier8_Behemoth` (Kháng bẫy, gầm hoảng sợ) |
| Bậc 9 | Sói Vua | `WerewolfTier.Tier9_WolfKing` | `Boss_WolfKing` (Sở hữu Mana, buff x3 bầy sói) |
| Tier 10 | Sói Vua tiến hóa khi phe Sói thắng | `WorldOutcome.WolfDominion` + `WolfKingAscended` (tên đề xuất) | Trạng thái kết thúc/cinematic của cùng vua; không spawn boss triệu hồi để đánh tiếp. `Tier10_AncientGod` cũ là legacy, không tự rename code/save. |

---

## 5. Thời Gian & Chu Kỳ Trăng

| Thuật ngữ GDD | Tên C# / Enum | Mô tả |
| :--- | :--- | :--- |
| Buổi trong ngày | `TimeOfDay` | `Dawn` (Bình minh), `Day` (Ngày), `Dusk` (Hoàng hôn), `Night` (Đêm) |
| Chu Kỳ Trăng | `MoonPhase` | `Crescent` (Trăng Khuyết), `HalfMoon` (Bán Nguyệt), `FullMoon` (Trăng Tròn), `BloodMoon` (Huyết Nguyệt) |
| Đêm Huyết Nguyệt | `BloodMoonEvent` | Đợt quái tập kích làng vào ban đêm |
| Quản lý thời gian | `GameTimeManager` | Điều khiển đồng hồ thế giới và chu kỳ ngày đêm |

---

## 6. Làng Mạc & Công Trình Phòng Thủ

| Thuật ngữ GDD | Tên Class / Enum C# | Tên Prefab / Mô tả |
| :--- | :--- | :--- |
| Đình Làng (Nhà Chính) | `TownHall` / `VillageCenter` | Công trình trung tâm quyết định Cấp Văn Minh |
| Lũy Tre Làng & Hào Chông | `BambooWall` / `SpikeMoat` | Tường cấp 2 khắc chế Sói nhảy rào |
| Giàn Ná Mây Bắn Lưới Bạc | `SilverNetBallista` | Cơ quan phòng thủ chống Sói nhảy qua tường |
| Tháp Nỏ Liên Châu | `RepeatingCrossbowTower` | Tháp bắn tên tự động |
| Lò Xông Khói Ớt / Bồ Kết | `HerbalSmokeBurner` | Cơ quan làm mù khứu giác Người Sói |
| Độn Thổ Thi (Quái đào ngầm) | `BurrowingGhoul` | Trạng thái `BurrowState` chui ngầm dưới tường |

---

## 7. Hệ Thống Nghi Kỵ & Phát Hiện

| Thuật ngữ GDD | Tên Class / Enum C# | Cơ chế |
| :--- | :--- | :--- |
| Thương Nhân Du Mục | `NomadTrader` | NPC vào làng buôn bán ban ngày |
| Danh Tính Thương Nhân | `TraderIdentityComponent` | Chứa cờ ẩn `IsDisguisedWerewolf` |
| Đuốc Ngải Cứu & Bồ Kết | `MugwortTorch` | Vật phẩm xông khói vạch mặt Sói giả dạng |
| Gương Đồng Chiếu Yêu | `BronzeDemonMirror` | Cổ vật soi danh tính thực của thương nhân |
| Chuông Gió Đồng Đen | `BlackBronzeWindChime` | Cảnh báo có Người Sói trong bán kính 50m |
| Bẫy Hàng Giả | `TaintedLureItem` | Món hàng giá rẻ dẫn dụ bầy Sói tấn công kho |

---

## 8. Binh Khí & Trang Bị Bản Địa

| Thuật ngữ GDD | Tên ScriptableObject / Item ID | Nhánh Vũ Khí (`WeaponCategory`) |
| :--- | :--- | :--- |
| Trường Kiếm Sắt Ấp Làng | `weapon_sword_iron_t1` | `WeaponCategory.Sword` |
| Mã Tấu Đi Rừng | `weapon_machete_jungle_t1` | `WeaponCategory.Sword` |
| Cung Tre Cánh Trâu | `weapon_bow_bamboo_t1` | `WeaponCategory.Bow` |
| Nỏ Thần Kim Quy | `weapon_crossbow_divineturtle_t3` | `WeaponCategory.Crossbow` |
| Áo Tơi Lá Cọ & Nón Lá | `armor_palm_leaf_t1` | `ArmorCategory.Light` |
| Giáp Vảy Đồng Đông Sơn | `armor_dongson_bronze_t2` | `ArmorCategory.Heavy` |
| Cơm Lam Ống Nứa | `consumable_bamboo_rice` | `ConsumableCategory.Food` |
| Dầu Tràm Cổ | `consumable_cajeput_oil` | `ConsumableCategory.Medicine` |


## 9. Session, phe và kết thúc — tên đề xuất mới

| Khái niệm | Tên đề xuất | Quy tắc |
| --- | --- | --- |
| Người tạo world | `WorldOwnerId` | Không đồng nghĩa máy đang chạy authority. |
| Máy mô phỏng | `AuthorityPeerId` | Một writer theo `AuthorityEpoch`/`CommitIndex`. |
| Nhân vật bền vững | `PlayerId` | Không đổi theo kết nối mới. |
| Phe thật | `TrueFaction` | State riêng; không dùng màu party công khai. |
| Vỏ bọc | `CoverIdentity` | Người khác quan sát được. |
| Hình dạng | `BodyForm` | Human/Hybrid/Wolf, tách allegiance. |
| Mộ | `GraveId`, `DeathId` | Inventory/equipment chuyển nguyên tử; không duplicate qua migration. |
| Hồi sinh | `ResurrectionState` | Channel/cost/cooldown authority quyết định. |
| Kết cục world | `WorldOutcome` | WolfKingSlain/WolfDominion/PartyWipe; có outcome flags cho sự kiện cùng tick. |
| Thành tựu cá nhân | `PersonalAchievementState` | Không đảo ngược outcome chung. |
| Sổ đá hữu hạn | `FiniteStoneLedger`, `StoneInstanceId` | Một quota/world cho mọi nguồn loot/quest/trade. |
| Chỉ huy Sói Vua | `WolfKingDirector` | Tactics hữu hạn, ký ức raid trong world; không đồng nghĩa LLM runtime. |

## 10. Schema cần đối chiếu mã nguồn trước triển khai

DamageInfo trong bảng cũ dùng `type`, M3 plan dùng `damageType` và `hitPoint`: chưa tự chốt tên bằng sửa tài liệu. M5R phải kiểm API hiện hành, chọn một schema và adapter network dùng entity ID thay tham chiếu GameObject qua mạng. ResourceNode/WorldItemPickup là thành phần M4; item, grave, player và event phải có ID bền vững trước migration.
