# MVP Scope — core M8 và demo một chương

Baseline 31/10/2026, thay ranh giới M1–M8 trước review. Canon: `PRODUCT_CANON.md`. Đây là kế hoạch tài liệu, chưa chứng nhận tính năng trong build.

## 1. Hai gate khác nhau

**M8 — core vertical slice nội bộ:** khai thác/chế tạo/sinh tồn 4 thanh và action combat, chạy LAN mục tiêu 4–6 người. Chặt cây, đập đá, nấu Cơm Lam, săn Sói bậc 1, dùng vũ khí/né đòn, ngày đêm, mộ/hồi sinh và save/migration hoạt động. Không quảng bá M8 là trải nghiệm đầy đủ nghi kỵ/phe phái.

**M12 — MVP demo công khai:** một biome chính Rừng Rậm, một chương hoàn chỉnh; người chơi trải nghiệm vòng sinh tồn xây làng → gặp đoàn buôn đáng ngờ → lựa chọn hợp tác/phản bội → raid và hậu quả/kết chương. Có mẫu nhỏ các hệ chủ đạo, không có toàn bộ nội dung GDD. Ưu tiên thu hút bằng lựa chọn có hậu quả, không bằng số lượng item.

## 2. In-scope

| Hệ | M8 core | Demo M12 |
| --- | --- | --- |
| Multiplayer | LAN join 4–6, authority, save/reconnect, graceful/abrupt host migration theo giới hạn công bố | 6 người, phiên dài, owner quay lại, private state và migration hoạt động cùng narrative |
| World | Một làng, rừng, suối/đường mòn, spawn, clock/moon | Một khu phụ nhỏ trong cùng biome nếu cần, không mở thêm 4 biomes |
| Player | 3D isometric, move/sprint/aim/roll, HP/Hunger/Stamina/Mana | Equipment specialization, mastery theo dùng; nhánh phép và bán Sói mẫu |
| Combat | Kiếm combo hai nhát, dodge, telegraph, Sói bậc 1 và lợn rừng | Thêm một lối đánh/cận vệ dùng chung rig; Sói Vua boss tối thiểu phục vụ endgame |
| Resource/craft | Gỗ/đá/quả/tre/gạo hữu hạn từ kho hoặc trader; rìu/đuốc/Cơm Lam | Chi phí sửa làng/thuốc xác thực/bùa hồi sinh có nguồn và sink |
| Inventory | 20 ô, equip/use, đầy túi, giao dịch nguyên tử | Kho chung có quyền công khai, finite stone ledger mẫu |
| Chết/hồi sinh | Rơi toàn đồ vào mộ, phép đồng đội, chống spam; team wipe bad ending | Cùng luật cho phe đối địch, không role/relog exploit |
| Làng | Cổng/Đình Làng nhận damage, sửa tại điểm định sẵn | Đặt tường/bẫy socket, kho lương và tối đa 3 vai trò NPC; chưa free building toàn bản đồ |
| Nghi kỵ | Chưa cần content đầy đủ; schema private state có từ M6N | Một đoàn buôn, hai clue, một cách xác thực tốn tài nguyên và hàng dẫn dụ |
| Phe/quest | Không khóa kiến trúc vào party đồng minh duy nhất | Phe thật/vỏ bọc riêng; một quest ẩn đổi phe, quest/achievement cá nhân; quyền PvP rõ |
| AI chiến lược | FSM Sói và raid cơ bản | Một tuyến tiếp tế, vua điều chỉnh giữa vài tactics sau thất bại; ghi nhớ trong world |
| Narrative | Tutorial và vòng ngày đêm | Một chương có mở–xung đột–quyết định–cao trào; vua chết/tier10 chiến thắng/team wipe có kết quả rõ |
| UX/audio/build | Menu, LAN errors, settings tối thiểu, cue telegraph, tooltip, save | Onboarding người mới, hướng dẫn phe ẩn không spoil, audio/readability, clean install |

Mọi con số balance trong spec đều prototype. Mana phải có công dụng thực qua phép hồi sinh, không chỉ là thanh trang trí. Không buộc xây hệ nông nghiệp đầy đủ để có gạo cho Cơm Lam. Kiếm sắt không tiếp tục dùng công thức gỗ/đá mà không gắn nhãn prototype.

## 3. Deferred khỏi demo

4 biome còn lại, 4 tầng mỏ đầy đủ, 9 tier có bộ asset riêng, 5 cấp văn minh hoàn chỉnh, toàn bộ 5 mastery tree, 10 loài thuần hóa, Huyết Thi đầy đủ, đấu giá/kinh tế vùng động, bốn ending cũ, MMO/public server, cloud persistence, Internet matchmaking, runtime LLM/online training. Host migration và phe ẩn **không thuộc deferred**.

M8 trước nội dung M9–M12; được thiết kế schema private faction/persistence sớm để tránh làm lại. Không dùng lệnh cấm cũ để trì hoãn networking/death/save/QA. M12 là demo, không tự đổi tên thành bản 1.0.

## 4. Nghiệm thu demo

Đề xuất 3 nhóm mới, mỗi nhóm 4–6 người; mỗi nhóm thử ≥90 phút và có save/mở lại. ≥75% người chơi hoàn tất tutorial không được người làm game chỉ trực tiếp; ≥75% giải thích được một lựa chọn sinh tồn/nghi kỵ gây hậu quả. Thu ý định chơi tiếp và lý do bỏ chơi, không coi đó là chứng minh retention dài hạn.

Không crash/softlock/blocker đã biết; không mất/nhân đôi đồ qua death/revive/migration; AI/clock/private flags giữ đúng qua đổi host. Có test một chương với các kết quả vua chết, Sói thắng/tier10 và team wipe. Không dùng debug skip làm bằng chứng người chơi mới tự đi hết chương.

Hiệu năng “ngang Albion” là mục tiêu tiếp cận chưa được xác minh. Trước M6N chọn máy client yếu nhất đội có thể mượn và host 6 người, ghi CPU/GPU/RAM/resolution/quality. Đề xuất client low 1080p p95 ≤33,3 ms, máy mục tiêu p95 ≤16,7 ms; fixture 6 player + 20 Sói + 10 NPC + 30 công trình. Chưa công bố cấu hình tối thiểu trước khi đo; nếu không đạt thì giảm entity/effect, không hứa AI-generated art tự nhẹ.
