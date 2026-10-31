# M6N — LAN authority, persistence và host migration spike

Trạng thái: PLANNED, chưa chọn networking framework, chưa có bằng chứng build. Phụ thuộc M5R và canon 31/10/2026. Owner: Lead; phối hợp Systems/World/UI/QA.

## Mục tiêu

Chứng minh 4–6 người LAN cùng chơi và khi máy authority tắt thì chuyển quyền an toàn trong điều kiện có đủ peer; xác định rõ ngoại lệ partition/ít peer trước khi xây thêm content. Không chọn framework chỉ vì AI sinh được ví dụ.

## Tasks theo thứ tự

| ID | Việc | Bằng chứng hoàn thành |
| --- | --- | --- |
| N01 | Kiểm controller/combat/inventory hiện có; lập authority matrix và ID schema | Liệt kê state local cần chuyển; không gửi GameObject reference làm identity qua mạng. |
| N02 | Spike tối đa hai lựa chọn networking phù hợp Unity hiện cài | Ghi tài liệu chính thức/version/license, LAN transport, migration support thực tế và effort; chọn một sau thử, không assume built-in migration. |
| N03 | Hai peer rồi sáu peer: movement/damage/pickup/craft | Build chạy ngoài Editor, log authority, same-world invariants; retry request không nhân đôi. |
| N04 | Snapshot/version/backup và journal giao dịch | Crash lúc ghi, corrupt file, restore; mộ/faction placeholder/private flags trong schema từ đầu. |
| N05 | Graceful handoff và chủ world quay lại | Epoch mới, authority cũ ngừng ghi, owner không overwrite latest save. |
| N06 | Abrupt shutdown, election và partition | Tắt process/máy, thiếu quorum pause; kiểm riêng phòng 2 người; không hai writer. |
| N07 | Reconnect/late join/last leave/session deadline | Cùng state/clock/cooldown, không offline progression; client thấy thông báo hợp lý. |
| N08 | Profile, báo cáo feasibility và re-estimate | Test report có build ID, máy/latency/loss/fixture, pass/fail và giới hạn; backlog refactor. |

## Acceptance

Dùng toàn bộ §5 SESSION_AND_PERSISTENCE.md; graceful/abrupt mỗi loại 10 lần khi đủ quorum, 100 giao dịch tranh đồ. Đề xuất test LAN bình thường và mô phỏng RTT100/200ms, loss1/3%; đây là stress test, không tuyên bố mạng LAN thường có các mức đó. Không đánh dấu pass migration chỉ bằng hai cửa sổ cùng process hoặc dev button.

Host migration có thể đòi hỏi spike kéo dài với đội mới. Nếu chưa đạt, dừng mở world expansion, cập nhật rủi ro/effort và đề xuất phương án có bằng chứng; không âm thầm đổi yêu cầu thành host luôn phải online.

## Ngoài phạm vi

Không public matchmaking, dedicated server 24/7, cloud account, runtime LLM, full NPC simulation hay asset cuối. Không viết test chỉ lặp lại implementation; ưu tiên invariant về save/item/authority/privacy.
