# Product Canon — Huyết Nguyệt Đại Ngàn

Cập nhật: 31/10/2026. Nguồn quyết định: câu trả lời chủ dự án sau REVIEW_2026-10-31 và xác nhận host migration/team wipe. Đây là canon hiện hành về sản phẩm và luật thế giới; không phải chứng nhận đã triển khai.

## 1. Thứ tự ưu tiên tài liệu

1. Quyết định mới rõ ràng của chủ dự án, được ghi thành mục có ngày tại đây.
2. `docs/vision/PRODUCT_CANON.md`: mục tiêu và luật đã chốt.
3. `docs/architecture/TECHNICAL_DECISIONS.md`: cách triển khai; không được đổi luật canon.
4. `docs/vision/MVP_SCOPE.md` và `docs/roadmap/PROJECT_ROADMAP.md`: phạm vi và thứ tự làm, không biến deferred thành loại bỏ khỏi tầm nhìn.
5. Spec hệ thống: phân biệt **đã chốt**, **baseline thiết kế đề xuất**, **chưa quyết định**.
6. GDD v3 là kho lore/nội dung nền; phần xung đột được thay bằng bảng §4. Review là ảnh chụp lịch sử và khuyến nghị, không phải quyết định sản phẩm.

`CURRENT_STATE.md` là nguồn trạng thái triển khai, không phải nguồn thay đổi gameplay. Glossary định danh; tên đề xuất mới không chứng minh class đã tồn tại. Nếu phát hiện mâu thuẫn chưa có quyết định, ghi decision log, không tự đánh dấu Approved.

## 2. Các quyết định đã chốt

| ID | Quyết định | Hệ quả bắt buộc |
| --- | --- | --- |
| C01 | PC, phòng riêng LAN, mục tiêu 4–6 người. Không server chạy 24/7. | Kiểm thử đủ 6 máy/process và session dài. Phòng riêng không đồng nghĩa tất cả cùng phe. |
| C02 | World do người tạo khởi tạo/sở hữu; khi họ rời hoặc tắt máy, phiên tự chuyển sang host còn lại. Dừng khi người cuối rời hoặc hết thời hạn phiên. | Host migration bắt buộc; quyền sở hữu world khác quyền chạy simulation. Không tính tiến hóa offline sau khi phiên dừng. |
| C03 | 6 thành viên × 48 giờ/tuần; tất cả mới làm game, chưa có Unity/networking/art/QA. Code gần 100% dùng AI; art/audio dùng AI, không thuê ngoài. | 288 giờ danh nghĩa không phải 288 giờ sản xuất hiệu quả. Có học, kiểm chứng, sửa lỗi và tích hợp; không coi AI là nhân lực QA thay thế. |
| C04 | MVP là demo thu hút người chơi, một biome chính và một chương hoạt động tốt. | Người chơi trải nghiệm đầy đủ vòng lặp trong phạm vi demo, không hiểu là toàn bộ 5 biomes/4 ending cũ. |
| C05 | Ưu tiên sản phẩm: sinh tồn xây làng và nghi kỵ thương nhân; chiến tranh phe là hướng phát triển. M8 ưu tiên khai thác/chế tạo/sinh tồn 4 thanh và action combat. | M8 là gate core nội bộ, demo công khai có gate sau M8 cho làng/nghi kỵ/phe ẩn. |
| C06 | 3D stylized, zone-based; Mana duy trì hóa Sói; kỹ năng theo trang bị và mastery theo mức sử dụng. | Thay 2D/liền mạch/Sói không Mana/class khóa cứng trong GDD. Dùng rìu tăng mastery rìu; nhánh phép không khóa nhân vật vĩnh viễn. |
| C07 | Tier 9 là một Sói Vua AI duy nhất trong world, có cận vệ và chỉ huy phe Sói; biết rút kinh nghiệm sau thất bại. | Cận vệ không phải vua tier 9 thứ hai; thất bại raid không đồng nghĩa vua chết. Cách học AI xem spec, không mặc định là LLM. |
| C08 | Sói Vua chết là endgame. Khi phe Sói thắng loài người, vua tiến hóa tier 10 và endgame. | Không respawn vua mỗi chu kỳ; tier 10 không là boss triệu hồi tiếp tục đánh như ending cũ. Điều kiện xác nhận loài người thua phải thành objective rõ. |
| C09 | Quota chính thức: Hỏa 5, Thủy 4, Mộc 3, Thổ 2, Hàn 1; càng ít càng khó lấy. | Tổng 15 viên mỗi world, không theo người/biome/session. Chủ dự án đã chốt Thổ 2, Hàn 1 ngày 31/10/2026. |
| C10 | Chết rơi toàn bộ đồ vào mộ tại nơi chết; không tự hồi sinh. Đồng đội nhánh phép dùng thuật hồi sinh, phải chống spam. | Inventory và đồ trang bị chuyển nguyên tử sang mộ; reconnect không làm sống lại. Hồi sinh không tự hoàn trả đồ. |
| C11 | Cả đội chết hết là một bad ending. | Không tự revive/đình làng cứu hộ/checkpoint hồi sinh mặc định. Không còn pháp sư sống nhưng vẫn có người sống chưa phải team wipe. |
| C12 | Mỗi người tự chọn phe, có thể giấu phe; nhóm 2 dân làng + 2 Sói hợp lệ. Nhiệm vụ ẩn cho phép trở thành bán Sói theo phe Sói trong làng. | Phe thật, vỏ bọc, dạng cơ thể và trang bị là bốn trạng thái riêng; quest riêng không broadcast. Không yêu cầu cả nhóm bỏ phiếu chọn phe. |
| C13 | Thành tựu/cốt truyện cá nhân theo nhân vật; world vẫn chạy khi một người offline nhưng còn người khác chơi. | Tách personal progression khỏi world outcome. Người offline có thể bỏ lỡ sự kiện; không rewind world khi họ quay lại. |
| C14 | Nhóm tuổi 18–28, có thể chơi buổi dài như ARK; hiệu năng mong muốn tương đương khả năng tiếp cận của Albion. | Đây là định hướng, chưa phải cấu hình tối thiểu đã đo. Host 6 người có thể cần CPU/RAM cao hơn client. |

## 3. Những điểm chưa được chốt / baseline đề xuất

- Thời hạn phiên: đề xuất `sessionTimeLimitMinutes = 0` nghĩa là không giới hạn; host đặt khi tạo phiên. Deadline giữ nguyên qua migration, không reset. Hết giờ chỉ save/dừng, không phải bad ending.
- AI thích nghi: đề xuất utility/rule-based ghi nhớ trận thua trong cùng world, không online training/LLM runtime. Vua đã chết không được sống lại để học; chưa carry learning sang world mới.
- Phép hồi sinh: cost, channel time, cooldown và suy yếu đều là số thử nghiệm trong spec, chưa cân bằng chính thức.
- Team wipe với phe ẩn, người offline, người mới vào: baseline trong spec cần playtest; quyết định đã chốt chỉ là cả đội chết thì bad ending.
- PvP damage/loot mộ/quyền phá công trình: phe đối địch và phản bội đã được yêu cầu; phạm vi quyền cụ thể là baseline thiết kế, không tự mặc định full-loot PvP mọi lúc.
- Standard cap không được chủ dự án xác nhận. Giữ tăng trưởng không cap hiện hành; dữ liệu config phải có safety bounds kỹ thuật, không tăng vô hạn số AI hoạt động.
- Sau endgame: đề xuất world read-only kèm bảng kết quả; chơi tiếp bằng world mới. Không tự xóa save và chưa có sandbox hậu chiến được phê duyệt.

## 4. Các nội dung GDD v3 bị thay thế

| Nguồn cũ | Canon thay thế |
| --- | --- |
| §1–2: 2D Pixel Art, bản đồ liền mạch | C06: 3D zone-based. |
| §1.2: mỗi chu kỳ thức tỉnh Sói Vua mới; §4.2 tier 9 vua/cận vệ | C07–08: một vua sống duy nhất; cận vệ tier thấp hơn, trăng không spawn vua mới. |
| §3.1: Sói thường/đặc biệt không Mana | C06: Mana/Huyết Năng cho dạng Sói; spec định nghĩa duration/skill. |
| §4.1: tiến hóa class cố định | C06: equipment specialization + use-based mastery. |
| §4.3/5.5: săn 9 bậc triệu hồi boss tier 10, đánh tiếp; player lật vua để thành vua AI mới | C07–08: vua AI chết kết thúc world; tier 10 là tiến hóa chiến thắng của cùng vua. Hội Săn Sói có thể còn trong lore nhưng nghi thức không override endgame. |
| §5.5: 4 ending như bốn campaign độc lập | World outcome: vua chết / phe Sói thắng→tier10 / team wipe. Thành tựu phe cá nhân chồng lên kết quả; bốn tên ending cũ chưa được cam kết triển khai. |
| §3.2/9: đá cực hiếm nhưng nguồn rơi/mua chưa giới hạn | C09: mọi nguồn dùng chung finite ledger; gọi Ngũ Linh Thạch, không ngầm đồng nhất với ngũ hành cổ điển. |
| SCOPE cũ: một người chết→Game Over, local save chỉ có player/time | C10–13: mộ, phép hồi sinh, world outcome và snapshot có player/world/migration. |
| Review đề xuất 1–4 co-op, không phản bội, không migration, rescue tại đình | Bị thay bởi C01–02, C10–12; không dùng review làm acceptance hiện hành. |

## 5. Nhật ký quyết định

- 31/10/2026: nhập 10 câu trả lời chủ dự án; xác nhận tiếp theo **tự chuyển host khi máy chủ phòng tắt** và **cả đội chết hết → bad ending**.
- Lần cập nhật này chỉ sửa tài liệu; M5 vẫn READY_FOR_VERIFICATION, không tuyên bố networking hay các luật mới đã chạy trong build.

- 31/10/2026: chủ dự án chốt Thổ 2, Hàn 1; quota C09 chính thức là 5/4/3/2/1 theo Hỏa/Thủy/Mộc/Thổ/Hàn.
