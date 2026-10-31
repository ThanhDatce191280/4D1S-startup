# Phe ẩn, hồi sinh, Sói Vua và endgame

31/10/2026. Canon: `docs/vision/PRODUCT_CANON.md`. Các con số và edge-case dưới đây là **baseline thiết kế đề xuất để thử**, không phải câu trả lời nguyên văn của chủ dự án.

## 1. Phe và progression riêng

Tách `TrueFaction` (Human/Wolf), `CoverIdentity` (người khác thấy), `BodyForm` (Human/Hybrid/Wolf), equipment và mastery. Bán Sói không tự đồng nghĩa phe Sói: allegiance chỉ đổi khi hoàn thành lựa chọn/nhiệm vụ ẩn. Hai Người và hai Sói cùng phòng hợp lệ; world quest và combat không giả định party đồng minh.

Ví dụ nhiệm vụ ẩn: tìm dấu huyết ở gốc đa → giao một chuyến thịt cho cận vệ → chủ động nhận Huyết Thệ. Thưởng mở dạng bán Sói/Mana, đổi TrueFaction; bảng quest không hiện cho người khác. Mang thịt hoặc đi đêm chỉ là clue, không tự chứng minh phản bội. Chuyển dạng trước mặt người khác tạo reveal có thời hạn; cần thiết kế cách soi player riêng, không mặc định mọi tool NPC luôn phát hiện tuyệt đối player.

Demo baseline: cho phép damage có chủ ý giữa player, mang vật phẩm mồi đánh dấu kho, cắt tiếp tế, giữ cổng hoặc phá phòng thủ theo quyền công bố trong lobby. Tắt aim-assist theo phe thật, party tint, reward feed hoặc AI phản ứng công khai làm lộ phe ngoài ý muốn. Quyền rút kho, phá công trình và loot mộ phải hiển thị theo quyền công khai, không theo faction bí mật. Không thêm kick/vote reveal tự động khi đổi phe. Chốt PvP rules trước M10 test, không bật ngầm trên người chơi.

Mastery tăng từ hành động có ích được authority xác nhận: rìu khai thác node hoặc gây damage hợp lệ; không tăng vì swing vào không khí, đánh đồng đội farm hoặc spam revive. Vai trò phép mở từ thiết bị/mastery và nhiệm vụ học; không khóa class vĩnh viễn. Quest/reward thuộc PlayerId; một event chỉ nhận một lần, reconnect/migration không reset. Người offline nhận tóm tắt sự kiện khi quay lại, không được tự nhận mọi thành tựu không tham gia.

## 2. Chết và mộ

`Alive → DeadAwaitingResurrection → Alive`, hoặc `WorldEnded`. Không timer tự respawn. Trong một transaction: mark dead, chuyển toàn bộ inventory và equipment vào `GraveId`, xóa đồ khỏi người, lưu vị trí và DeathId. Nếu chết ở vị trí không thể tiếp cận, đề xuất snap mộ đến điểm hợp lệ gần nhất và thông báo; không rải đồ vào void. Đồ quest hữu hình và Linh Thạch cũng vào mộ; quest flags/mastery không phải đồ nên vẫn giữ.

Baseline quyền mộ: owner và người được owner cho phép được lấy, chưa bật cướp mộ toàn phòng. Hồi sinh tại mộ không tự chuyển đồ về người. Mộ tồn tại qua rời phòng/migration/load; nhiều người loot đồng thời không lấy trùng. Người chết được xem đồng đội đã cho phép, không free-camera soi phe/đội địch. Reconnect vẫn chết; offline không phải phép giải thoát.

## 3. Phép hồi sinh chống spam

| Tham số prototype | Giá trị đề xuất | Mục đích |
| --- | --- | --- |
| Điều kiện | Caster sống, mở nhánh phép, có pháp khí; target dead và đồng ý | Không hồi sinh người khác để farm/ép họ lộ vị trí. |
| Khoảng cách/channel | Trong 3 m mộ, channel 8 giây; đi xa/nhận damage/đổi dạng hủy | Đồng đội phải bảo vệ caster. |
| Giá | 60 Mana + 1 Bùa Hồi Hồn chế từ thảo dược thường | Không dùng đá hữu hạn làm nguồn hồi sinh bắt buộc. |
| Commit | Reserve cost khi bắt đầu, chỉ tiêu khi thành công; hủy trả reservation | Hai caster không tiêu hai lần hoặc sinh hai nhân vật. |
| Cooldown caster | 120 giây game; persistence theo player | Đổi pháp khí/relog/migrate không xóa cooldown. |
| Suy yếu target | 180 giây, giảm max HP 25%; không XP hồi sinh | Chết liên tiếp có chi phí; không cooldown chặn hồi sinh lần nữa vô thời hạn. |
| Khi sống lại | 30% HP, không tự nhặt đồ, không bất tử dài | Hồi sinh có rủi ro vị trí. |

Đề xuất cho nhóm học phép căn bản trước khu nguy hiểm đầu tiên, thảo dược hồi sinh không bị khóa sau boss. Nếu caster chết nhưng còn người sống chưa học phép, người sống vẫn có đường học/đổi trang bị và cứu; nếu họ từ chối, người chết phải chờ. Không tự thêm NPC cứu hộ hoặc gọi đó là wipe khi vẫn còn người sống.

## 4. Team wipe và người offline

Đã chốt: cả đội chết → bad ending. Baseline để tránh offline/invite lách luật:

- Team = roster player tham gia cuộc chơi, **mọi phe**, không chỉ phe Người. Hai người phe Người chết nhưng Sói player còn sống không phải team wipe.
- Đề xuất combat roster chụp lúc bắt đầu encounter; disconnect giữa giao tranh không xóa tên/HP khỏi roster và không né damage. Nhân vật offline trong combat vẫn do authority xử lý đến hết encounter.
- Player offline an toàn trước encounter không tính là người sống cứu cả phòng mãi mãi; roster chỉ xét người tham gia encounter. Quy tắc này cần thử với world dài và ghi rõ lobby.
- Khi toàn bộ roster hợp lệ dead, authority commit `PartyWipe` một lần; người mới join không đảo ngược. Các lượt damage chết cùng tick được resolve trước kiểm outcome.
- Sau endgame save kết quả; không tự load checkpoint hồi sinh. Khôi phục backup chỉ dành phục hồi lỗi kỹ thuật hoặc nhánh world mới được gắn nhãn, không ghi thành chiến thắng của world gốc.

## 5. Sói Vua thích nghi

Một vua tier 9 duy nhất, chỉ huy cận vệ tier thấp hơn. Triển khai đề xuất: `WolfKingDirector` chọn chiến thuật từ tập hữu hạn dựa trên kết quả đã quan sát; không đọc vị trí bí mật player hoặc blueprint chưa trinh sát.

Ví dụ: raid cổng Bắc thất bại vì bẫy → tăng trọng số trinh sát/cắt kho hoặc thử cổng Nam ở lần sau; đoàn thịt bị chặn hai lần → thêm hộ tống hoặc đổi một tuyến đã biết. Vua có thể rút lui khi yếu, nhưng khi thực sự chết thì endgame, không hồi sinh để học. Cận vệ bảo vệ/rút quân theo lệnh, không có vua tier 9 thứ hai.

Lưu `RaidId`, tuyến đi, casualty, food delivered, mục tiêu đã phá, chiến thuật đã dùng và trọng số thích nghi vào world snapshot. Giới hạn mức đổi mỗi raid, decay ký ức cũ, có seed để replay. Difficulty thay đổi qua tactics và growth được log; không tự tăng damage vì AI “đọc được” ý định người chơi. Học trong world đang sống; không có dịch vụ AI/cloud/token cost bắt buộc và chưa học xuyên world.

## 6. World outcome và thành tựu cá nhân

| Outcome | Điều kiện | Hậu quả |
| --- | --- | --- |
| `WolfKingSlain` | HP vua về 0 được authority xác nhận | Endgame; không cần 5 đá mới được kết thúc. Phe/thành tựu cá nhân quyết định epilogue mỗi người. |
| `WolfDominion` | Phe Sói đánh bại mục tiêu sinh tồn cuối của loài người | Vua còn sống tiến tier 10 rồi endgame, không mở vòng đánh boss tier 10 mới. |
| `PartyWipe` | Mọi người thuộc roster wipe hợp lệ đều chết | Bad ending cho nhóm, không phụ thuộc cùng phe. |
| `SessionExpired` / `EmptyRoom` | Hết giờ / hết người | Save/dừng kỹ thuật, không phải ending. |

**Objective phe Người thua — đề xuất cho demo:** sau báo động cuối, Đình Làng trung tâm bị phá hủy và không còn mục tiêu sơ tán đang hoạt động. Phá kho lương riêng chưa đủ; giết mọi human player cũng chưa đủ nếu làng còn. Rule phải có HUD/cảnh báo để người chơi biết cái giá. Campaign nhiều làng sẽ cần objective riêng.

Thứ tự cùng tick đề xuất: resolve damage giao dịch → ghi king dead trước; nếu vua chết và cả đội chết cùng tick, world outcome `WolfKingSlain` kèm cờ `PartyWiped`, kết cá nhân là hy sinh/bad ending, không tạo vua tier10 đã chết. Nếu vua sống và vừa human objective thua vừa wipe, world `WolfDominion` kèm `PartyWiped`. Chốt/test semantics này trước M11; không để thứ tự RPC ngẫu nhiên quyết định ending.

## 7. Linh Thạch hữu hạn

**Quota chính thức đã chốt ngày 31/10/2026: Hỏa 5, Thủy 4, Mộc 3, Thổ 2, Hàn 1 — tổng 15 viên mỗi world.** Không phải mỗi biome có thêm bộ mới. Mỗi viên có `StoneInstanceId` và state: Unclaimed/World/Inventory/Socket/Grave/Vault. Drop, mua, quest reward, mining cùng tham chiếu ledger; nhiều nguồn chỉ là nhiều đường tiếp cận, không mint thêm viên.

Đề xuất mỗi viên thu hồi được khi tháo khảm/phá trang bị; không tiêu mất vĩnh viễn vào recipe bulk/bụi nâng tường. Người offline cầm đá: đá vẫn trong inventory của họ và không spawn bù; cần UI thông tin tổng số đã tìm thấy nhưng không tiết lộ phe/vị trí bí mật. Không đặt quota thế giới thành điều kiện bắt buộc hồi sinh hay kết thúc chương để tránh một người offline khóa cả phòng. Số lượng thấp hơn đi cùng objective khó hơn, không chỉ giảm xác suất RNG.

## 8. Test bắt buộc

Death/loot/revive đồng thời; disconnect caster/target; migrate trong channel; toàn đội chết cùng tick; người Sói sống khi người Người chết; host là người chết; reconnect vẫn chết; đá trong mộ không mất; hết giờ khi đang raid không thành wipe; player offline quay lại sau world ended; AI thay tactic có log nhưng không respawn vua; PvP/HUD/spectator không vô tình lộ phe.
