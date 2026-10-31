# Session, host migration và persistence

31/10/2026. Yêu cầu C01–02/C10–13 đã chốt trong PRODUCT_CANON. Thiết kế kỹ thuật dưới đây là baseline đề xuất cần được chứng minh ở M6N; chưa chọn networking package, chưa khẳng định triển khai hoàn tất.

## 1. Mô hình và vòng đời

Phòng LAN 4–6 người; một máy làm simulation authority tại một thời điểm. `WorldOwnerId` là người tạo world, `AuthorityPeerId` là máy đang chạy; hai ID không nhất thiết trùng. Client gửi ý định; authority xác nhận movement hợp lệ, combat, inventory, resource, AI, quest, giờ thế giới, chết và hồi sinh.

`Stopped → Loading → Running → Migrating → Running → Saving → Stopped`.
`Running → Ended` khi đạt world outcome; Ended vẫn lưu kết quả nhưng không tick gameplay.

| Sự kiện | Kết quả yêu cầu |
| --- | --- |
| Một client rời | Player state được lưu; world tiếp tục với người còn lại. |
| Chủ world rời, người khác còn chơi | Chuyển authority; không kết thúc gameplay chỉ vì chủ world rời. |
| Máy authority tắt đột ngột | Tạm dừng tick/input gây biến đổi, chọn successor, khôi phục checkpoint đã xác nhận; không tiếp tục hai bản world. |
| Người cuối rời | Save rồi dừng; không dùng giờ máy thật để bù tiến hóa sau đó. |
| Hết hạn phiên | Save/dừng có cảnh báo; không ghi team wipe và không reset timer khi migration. |
| Chủ world quay lại | Join state hiện hành; không lấy file cũ đè lên world đang chạy, không bắt buộc cướp lại authority. |

## 2. Chuyển quyền và tránh split-brain

- Snapshot chứa `WorldId`, `SaveVersion`, `AuthorityEpoch`, `CommitIndex`, world tick, roster, trạng thái gameplay và checksum. Chỉ một writer theo epoch.
- Graceful handoff: pause ngắn → flush snapshot/journal → successor xác nhận tải được → commit epoch mới → đổi endpoint → resume. Máy cũ ngừng writer trước resume.
- Unexpected shutdown: dùng checkpoint và journal đã replicate/ack. Đề xuất ack từ đa số roster authority hiện hành để commit hành động có giá trị; thử chi phí ở M6N. Loot/tiêu nguyên liệu chưa commit chỉ là pending, không báo thành công chắc chắn.
- Election chọn ứng viên đủ checkpoint mới nhất bằng thứ tự quyết định được, chỉ tiếp tục khi có quorum theo membership hiện hành. Thay đổi membership cũng cần commit. Network partition không đủ quorum thì pause, không tự tạo world thứ hai.
- Phòng còn 2 máy, authority đột ngột mất: không thể đồng thời hứa tự tiếp tục dưới mọi partition và bảo đảm không split-brain. **Ngoại lệ kỹ thuật cần spike**: pause và yêu cầu phục hồi snapshot đã xác nhận, hoặc cơ chế lease/witness được kiểm chứng. Không giả vờ host migration bảo đảm mọi tình huống. M6N phải báo giới hạn này trước công bố demo.
- Đề xuất checkpoint 5 giây, journal giao dịch quan trọng mỗi commit; mục tiêu rollback giao dịch đã ack = 0. Nếu không đủ state an toàn, dừng có thông báo và giữ backup, không âm thầm mất inventory.
- Mục tiêu spike: graceful migration ≤5 giây, abrupt migration khi đủ quorum ≤15 giây; chỉ là ngưỡng đề xuất, cần đo 6 người.

## 3. Save world thuộc ai sau migration

Successor lưu bản authoritative mới nhất và replicate cho peer dự phòng. Chủ world có thể đã tắt máy nên không thể nhận file ngay. Khi họ quay lại, đồng bộ bản mới theo WorldId/epoch/commit; khi phiên kết thúc lúc họ vắng, UI phải chỉ rõ máy nào giữ save mới và cách chuyển LAN/import. Không dùng timestamp file đơn thuần để quyết định bản thắng; bản rẽ nhánh phải có BranchId riêng, không tự merge.

Save dùng ghi file tạm → xác minh → thay snapshot, giữ backup gần nhất. Có version/migration schema; test mất điện khi ghi, file hỏng, disk full và build không tương thích. Không overwrite bản tốt nếu migration schema thất bại.

## 4. Dữ liệu phải lưu/đồng bộ

| World | Player | Thông tin hạn chế gửi |
| --- | --- | --- |
| Clock/moon, raid, supply, king growth/learning, công trình, kho, resource depletion, finite stones, quest world, outcome, session deadline | PlayerId, vị trí, 4 stats, inventory/equipment, mastery, quest/achievement, faction thật, cover, form, dead/alive, grave/revive cooldown | Faction thật/quest bí mật gửi cho owner; clue/reveal theo người quan sát; spectator người chết không được nhìn phe hoặc camera đối phương toàn map. |

Mỗi item/grave/quest reward có ID bền vững. Request có ID + sender + sequence, không thực hiện lại khi retry/migrate. Giao dịch craft/death/revive/trade/loot phải nguyên tử và bảo toàn vật phẩm.

**Giới hạn tin cậy:** máy authority phải có truth để mô phỏng. Người quản trị máy đó có thể đọc save/memory; host migration còn chuyển truth sang successor. Không cam kết chống host gian lận hoặc bí mật tuyệt đối trên máy người chơi. Trong phòng bạn bè, UI/network thường không lộ phe; không gửi full private snapshot dưới dạng state công khai cho mọi client. Mã hóa checkpoint dự phòng không tự giải quyết người nhận authority cố ý đọc truth.

## 5. Acceptance M6N và demo

- 6 process trên ≥2 máy LAN; sau đó chơi 6 máy thực nếu có: cùng clock/HP/inventory/quest, thử jitter/loss.
- Graceful handoff 10 lần; tắt process authority đột ngột 10 lần với đủ quorum; không có hai writer, không nhân đôi đồ/quest reward. Thử cả ngay trong combat, loot mộ, hồi sinh và save.
- Ngắt cáp/partition: bên thiếu quorum không tiếp tục tick; nối lại không ghi đè state mới. Kiểm riêng phòng 2 người và thông báo giới hạn/fallback.
- Migration giữ phe ẩn, cooldown, death roster, số đá và deadline; chủ world quay lại không rollback.
- Người cuối rời rồi mở lại: world không tăng ngày/growth vì thời gian offline.
- Không gọi đạt demo nếu chỉ có localhost hoặc handoff bằng nút dev; có log build ID/epoch/commit và bảng sai lệch.
