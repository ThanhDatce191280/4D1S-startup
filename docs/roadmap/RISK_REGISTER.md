# Risk Register & Mitigation Strategy

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mục đích:** Nhận diện sớm, đánh giá mức độ ảnh hưởng và xây dựng phương án dự phòng (Fallback) cho các rủi ro kỹ thuật, nội dung và phối hợp nhóm.

---

## 1. Bảng Đánh Giá Rủi Ro Tổng Thể

| Mã RR | Nhóm Rủi Ro | Mô tả rủi ro | Mức độ | Dấu hiệu cảnh báo sớm | Phương án giảm thiểu & Khắc phục (Mitigation) |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **R-01** | **Phạm vi (Scope Creep)** | Sa đà vào làm 9 Bậc Sói, 5 Biomes, cơ chế rèn đồ Người Lùn khi chưa có bản chơi thử M8. | **Chí mạng** | Chưa hoàn thành vòng lặp 1 ngày đêm nhưng đã viết code cho Biome 4 hoặc kỹ năng Pháp Sư. | **Quy tắc Cổng Chặn M8:** M8 là core gate; chỉ mở nội dung M9–M12 sau gate. Networking, migration, death/save và private-state schema phải làm trước M8 theo roadmap mới; không dùng ranh giới cũ để hoãn chúng. |
| **R-02** | **Xung Đột Git trong Team (Merge Conflict)** | 6 thành viên cùng sửa chung 1 file Scene (`.unity`) hoặc 1 file Prefab dẫn đến xung đột không thể merge. | **Cao** | Git báo `CONFLICT (content): Merge conflict in *.unity`. | • Tuyệt đối không sửa chung Scene chính.<br>• Sử dụng **Prefab-based Workflow**: Mỗi người làm việc trên Prefab/Sub-scene riêng.<br>• Phân chia rõ ràng vùng thư mục theo bảng phân vai trong `PROJECT_ROADMAP.md`. |
| **R-03** | **Đồ Họa & Animation (Art Bottleneck)** | Mất quá nhiều thời gian đi tìm hoặc dựng model thú Việt Nam / Sói 9 Bậc làm đình trệ tiến độ code. | **Cao** | Lập trình viên phải chờ đợi artist hoặc không có animation để test logic. | • Giai đoạn M1–M8: Dùng primitive/placeholder hoặc asset AI đã kiểm tra; Mixamo tùy chọn, không có nhân sự thuê ngoài. Phải đo thời gian rig/import/tối ưu thay vì chỉ sinh concept.<br>• Tách rời `VisualRoot` khỏi logic code để thay model 3D bất kỳ lúc nào mà không sửa script. |
| **R-04** | **Lệch Nhịp Phối Hợp 2 AI (Model Drift)** | GPT-6 Astra và Gemini 4 Argon đặt tên class, enum hoặc cấu trúc script không khớp nhau giữa các phiên. | **Cao** | Chuyển đổi giữa 2 AI làm project không compile được do sai tên phương thức hoặc kiểu dữ liệu. | • Bắt buộc dùng `docs/architecture/GLOSSARY.md` và `docs/handoff/CURRENT_STATE.md` cho danh pháp/trạng thái và `docs/vision/PRODUCT_CANON.md` cho luật hiện hành.<br>• AI mới phải chạy `Rule 1 — Inspect Before Editing` trước khi viết code. |
| **R-05** | **AI Quái Kẹt Đường (NavMesh Bottleneck)** | Quái vật đứng đơ khi người chơi xây tường bít kín cổng làng, hoặc nhiều quái chen chúc nhau ở lối hẹp. | **Trung bình** | Trong đêm Huyết Nguyệt, Sói đứng im ngoài tường hoặc chạy vòng quanh vô tận. | • Lập trình kiểm tra trạng thái đường đi `NavMeshPathStatus.PathPartial`.<br>• Nếu không tìm được đường tới mục tiêu bên trong, AI tự động chuyển mục tiêu sang tấn công đoạn tường/rào chắn gần nhất. |
| **R-06** | **Cân Bằng Gameplay & Thua Màn Chơi** | Sói Vua mạnh lên quá nhanh do người chơi phát triển chậm hoặc lơ là chặn đường tiếp tế, dẫn đến thua cuộc. | **Trung bình** | Làng bị áp đảo hoàn toàn trong các đêm Huyết Nguyệt sau. | • Giữ nguyên cơ chế Hardcore Survival (chấp nhận thua màn chơi).<br>• Hiển thị bảng thống kê chi tiết sau khi Game Over để người chơi rút kinh nghiệm.<br>• Đưa toàn bộ chỉ số tăng trưởng vào `WolfKingConfigSO` để dễ tinh chỉnh độ khó khi test. |
| **R-07** | **Lỗi Bộ Nhớ & Tụt FPS (Performance)** | Tạo quá nhiều GameObject (mũi tên, quái vật, số sát thương) làm sụt giảm FPS trong đêm công thành. | **Trung bình** | Trận đánh đông quái bị giật lag, thời gian thu gom rác (GC) tăng đột biến. | • Áp dụng `Object Pooling` cho đạn nỏ, hiệu ứng máu, quái vật.<br>• Dùng `Physics.OverlapSphereNonAlloc` để kiểm tra vùng chém thay vì tạo collider trigger tạm thời. |
| **R-08** | **Java OOP Bias (Overengineering)** | Áp dụng quá mức tư duy Enterprise Java (tạo hàng loạt Abstract Factory, Interface trung gian, Reflection) vào Unity. | **Trung bình** | Một hành vi nhặt đồ phải đi qua 6 class, không cấu hình được trên Inspector của Unity. | Tuân thủ **Rule 5**: Chỉ tạo Interface khi thực sự có $\ge 2$ đối tượng hoàn toàn khác nhau cùng triển khai (ví dụ: `IDamageable`, `IInteractable`). |


## 2. Rủi ro bổ sung theo canon 31/10/2026

Các owner là trách nhiệm được phân công, không giả định đội đã có chuyên môn.

| Mã | Rủi ro / mức | Owner | Trigger | Mitigation và gate |
| --- | --- | --- | --- | --- |
| R-09 | Host migration/split-brain — Cao | Lead | Hai writer, mất inventory hoặc không tiếp tục khi tắt máy host | M6N trước world lớn; epoch/quorum/checkpoint; test partition và phòng 2 người, công bố ngoại lệ thay vì ghi “migration luôn hoạt động”. |
| R-10 | Save mới ở máy successor — Cao | Lead/Systems | Chủ world mở file cũ làm rollback | WorldId/epoch/commit, sync khi owner quay lại, UI chỉ máy giữ bản mới; không merge theo timestamp. |
| R-11 | Phe ẩn bị lộ/host đọc truth — Cao | World/UI | Party tint, AI aggro, spectator hoặc snapshot lộ faction | Private replication, clue/reveal theo observer; host trusted được ghi rõ, không hứa chống chủ máy đọc memory. |
| R-12 | Deadlock không còn pháp sư — Cao | Systems/Level | Người sống không thể học phép/tìm nguyên liệu | Cho đường mở nhánh phép sớm và nguyên liệu thường; không tự thêm cứu hộ trái canon. Team wipe thật là bad ending. |
| R-13 | Farm revive/đá duplicate — Cao | Systems | Cooldown mất khi relog, quota tăng sau trade/migration | Transaction ID, ledger invariant, snapshot cooldown/grave; test đồng thời ở M6W/M11. |
| R-14 | 6 người mới + AI sinh code chưa hiểu — Cao | Lead/QA | Không tái hiện bug, checklist pass không có log | Dành giờ học và kiểm chứng; PR nhỏ, contract-first, test chéo; capacity 288 giờ danh nghĩa không là velocity. |
| R-15 | Art/audio AI không game-ready — Cao | Art owner | Model quá nặng, rig lỗi, style lệch, nguồn/quyền không rõ | Một mẫu hoàn chỉnh trước batch asset; budget/poly/texture/audio, manifest, import test; giảm số asset. |
| R-16 | “AI học” phình thành ML runtime — Cao | World | Phải gọi cloud/model mỗi tick hoặc vua biết dữ liệu chưa quan sát | Utility/rule memory trong world, tactic hữu hạn, log evidence; spike trước cân bằng, chưa LLM runtime. |
| R-17 | Phe đối địch làm hỏng buổi chơi — Cao | Narrative/UI | Đội không hiểu quyền phá làng, mộ bị cướp không báo, wipe bị lách disconnect | Lobby rules, quyền công khai, death roster và ending rules test mixed-party. |
| R-18 | Hứa cấu hình “ngang Albion” chưa đo — Cao | Lead/Art | FPS tụt khi host 6 người dù solo ổn | Máy test ghi thông số, host/client riêng, fixture; không công bố minimum trước M12 evidence. |
