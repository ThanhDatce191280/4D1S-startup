# Roadmap — baseline LAN 4–6 và phe ẩn

Cập nhật 31/10/2026 theo `docs/vision/PRODUCT_CANON.md`. Roadmap này thay M6–M15 cũ; giữ lịch sử M0–M5. Các mốc sau M5 là kế hoạch, chưa triển khai/nghiệm thu.

## 1. Dependency và gate

M0 → M1 → M2 → M3 → M4 → M5 → **M5R** → **M6N** → **M6W** → **M7** → **M8 core** → **M9** → **M10** → **M11** → **M12 demo**.

M3→M4 thể hiện dependency Health/combat thực tế. Tài liệu/content brief có thể chuẩn bị song song, nhưng không mở rộng gameplay trước gate nền tảng. Save, networking, QA và profiling xuyên suốt, không đợi cuối roadmap.

| Mốc / owner trách nhiệm | Scope | Definition of Done và evidence |
| --- | --- | --- |
| M0–M4 | Nền tảng/controller/combat/survival | COMPLETED theo handoff cũ; không tái chứng nhận trong lần sửa tài liệu. |
| M5 — Systems + QA | Inventory/crafting hiện có | READY_FOR_VERIFICATION; chạy standalone nhặt→craft→equip→ăn và test đầy túi, có build ID. |
| M5R — Lead + QA | Chốt canon, gap M5/SCOPE, learning/spike backlog | Ghi rõ phần có/chưa có Roll/combo/Mana; checklist M5 có bằng chứng, không tick theo code tồn tại. |
| M6N — Lead + Systems | LAN authority, save/reconnect, host migration | 6 process trên ≥2 máy; 100 giao dịch tranh đồ không duplicate; graceful/abrupt migration mỗi loại 10 lần khi đủ quorum; test partition/phòng 2 người, owner save sync. Báo giới hạn/fallback, không giấu trường hợp không đủ quorum. |
| M6W — Systems + World + UI | Hoàn thiện combat/survival, mộ/phép hồi sinh/team wipe, mastery schema | 20 ca death/loot/revive đồng thời và reconnect/migrate không mất đồ/reset cooldown; không tự respawn; whole-team death ghi bad ending đúng một lần; nhánh phép tiếp cận được trước rủi ro đầu tiên. |
| M7 — World + Level | Một biome/clock/raid, village targets, tutorial | 4 rồi 6 người chơi vòng ngày đêm 15–20 phút, đủ nguồn nấu ăn/sửa cổng; wave và target có luật; save/loading không tái sinh đồ hoặc quên quest. |
| M8 — cả đội | Core vertical slice: sinh tồn/chế tạo + combat | 3 nhóm mới 4–6 người thử, ≥75% hoàn thành tutorial không trợ giúp; 2 session ×2 giờ nội bộ ổn định; migration cùng combat/save đạt test matrix. Chưa gọi đây là demo phe phái đầy đủ. |
| M9 — World + Level + UI | Sinh tồn xây làng và nghi kỵ | Socket tường/bẫy, kho/3 vai trò NPC; một đoàn buôn, hai clue, một xác thực và ba lựa chọn nhận/cách ly/từ chối đều có hậu quả; 20 seed/test case không softlock. |
| M10 — Systems + World + Narrative owner | Phe ẩn cá nhân, quest ẩn, equipment/mastery, quyền PvP | Nhóm 2 Người/2 Sói và nhóm 6 mixed chạy được; UI/AI/packet thường không lộ TrueFaction; 10 reconnect/migration giữ bí mật, thưởng một lần; không farm mastery bằng đánh đồng đội/hồi sinh. |
| M11 — World + Narrative + Systems | Một chương, vua tier9/cận vệ, adaptive tactics, đá hữu hạn, endings | 3 outcome đều có đường chơi/test; king death không respawn, WolfDominion→tier10; test cùng-tick wipe/king death; 3 lần raid có log tactic đổi theo evidence; quota đá bảo toàn qua mộ/trade/socket/offline/migration. |
| M12 — Lead/Producer + QA + Art | MVP demo công khai polish/QA/build | Đạt acceptance MVP_SCOPE; clean install→LAN→chơi chương→save→migrate→load; settings/audio/readability, asset source/license manifest; không blocker/critical đã biết, có hướng dẫn giới hạn migration và cấu hình đo. |
| M13+ — chưa cam kết | Biome tiếp, mastery sâu, free building, phe/NPC mở rộng | Mỗi biome/nhánh có estimate và gate riêng sau dữ liệu demo. Không còn gói “5 biomes + 9 tier” trong một milestone. |

M6N thất bại không được lặng lẽ bỏ migration: ghi vấn đề, bổ sung spike hoặc đề xuất đổi yêu cầu cho chủ dự án. Không chốt ngày phát hành trước khi biết networking khả thi.

## 2. Chuyển từ roadmap cũ

M6 cũ→M6N/M6W/M7; M7 cũ→M7; M8 giữ trọng tâm core nhưng có LAN. M9/M10 cũ→M9/M10; một phần M11/M12 cũ làm mẫu hẹp trong demo; M13/M14 cũ được chia thành chapter M11 và backlog sau demo. M15 cũ được phân bổ QA/save/performance vào từng mốc và đóng gói M12. Bốn ending cũ bị canon thay, không chỉ đổi lịch.

## 3. Người chịu trách nhiệm, AI và capacity

6 người đều chưa có kinh nghiệm game; các vai trò sau là trách nhiệm học và kiểm chứng, **không phải tuyên bố có senior chuyên môn**.

| Người / vai trò | Phạm vi | Sản phẩm bàn giao |
| --- | --- | --- |
| 1 — Lead/Producer | Core/networking, contract, tích hợp | Spike evidence, decision log, build gate; dành giờ học Unity/networking. |
| 2 — Systems | Combat/stats/inventory/mastery/death | Pure logic, config, invariant tests, integration scene. |
| 3 — World/AI | Navigation, raid, supply, king AI | FSM/director có replay log, tải đại diện. |
| 4 — Art/audio pipeline | AI asset, rig/animation, VFX/SFX | Asset nhẹ, nhất quán, import được; manifest nguồn/quyền sử dụng, không chỉ ảnh concept. |
| 5 — Level/Narrative implementation | World layout, NPC/quest graph | Một chương chơi được, clue placement, dialogue; ghi rõ thời gian narrative design. |
| 6 — UI/QA coordination | UI, onboarding, test matrix | Build test report, luồng lỗi, playtest; cả đội vẫn phải test chéo. |

288 giờ/tuần là tổng giờ danh nghĩa. Baseline phân bổ thử: 30% học/spike, 40% làm/tích hợp, 20% kiểm thử/review, 10% điều phối/tài liệu; đo lại hàng tuần. Không quy đổi 2 AI thành 2 dev kinh nghiệm. Ước lượng 8–12 tuần trong review trước đó không còn là baseline vì đã thêm 6-player migration, phe ẩn và biết đội mới hoàn toàn.

Hai tuần đầu sau quyết định: kiểm chứng M5, học ownership/state và spike M6N; cuối kỳ báo số giờ thực/khối lượng còn lại, chưa hứa hoàn thành M6N trong hai tuần. Chỉ estimate 1–2 milestone trước mắt sau spike, dùng range và buffer cho lỗi chưa biết.

## 4. DoD chung

- Compile không lỗi, standalone chạy ngoài Editor; mỗi task có owner/commit/build ID.
- Test ý nghĩa cho invariant: không nhân đôi/mất đồ, outcome một lần, private state không broadcast, save/migration giữ đúng dữ liệu.
- Playtest thật cho cảm giác đòn/nhận diện clue/readability; AI tự nói “pass” không thay log hoặc người chơi.
- Thay đổi contract/schema cần review trước tích hợp. Một người sở hữu scene tại một thời điểm, Prefab-based workflow; không cho nhiều AI cùng sửa một scene.
- Update CURRENT_STATE theo bằng chứng, không theo phần trăm code được AI sinh. Asset/audio phải kiểm tra import, chất lượng, kích thước và quyền sử dụng.
