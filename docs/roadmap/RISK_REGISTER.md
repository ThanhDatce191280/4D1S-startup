# Risk Register & Mitigation Strategy

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mục đích:** Nhận diện sớm, đánh giá mức độ ảnh hưởng và xây dựng phương án dự phòng (Fallback) cho các rủi ro kỹ thuật, nội dung và phối hợp nhóm.

---

## 1. Bảng Đánh Giá Rủi Ro Tổng Thể

| Mã RR | Nhóm Rủi Ro | Mô tả rủi ro | Mức độ | Dấu hiệu cảnh báo sớm | Phương án giảm thiểu & Khắc phục (Mitigation) |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **R-01** | **Phạm vi (Scope Creep)** | Sa đà vào làm 9 Bậc Sói, 5 Biomes, cơ chế rèn đồ Người Lùn khi chưa có bản chơi thử M8. | **Chí mạng** | Chưa hoàn thành vòng lặp 1 ngày đêm nhưng đã viết code cho Biome 4 hoặc kỹ năng Pháp Sư. | **Quy tắc Cổng Chặn M8:** Cấm viết code cho M9–M15 khi M8 chưa build `.exe` và nghiệm thu hoàn tất. |
| **R-02** | **Xung Đột Git trong Team (Merge Conflict)** | 6 thành viên cùng sửa chung 1 file Scene (`.unity`) hoặc 1 file Prefab dẫn đến xung đột không thể merge. | **Cao** | Git báo `CONFLICT (content): Merge conflict in *.unity`. | • Tuyệt đối không sửa chung Scene chính.<br>• Sử dụng **Prefab-based Workflow**: Mỗi người làm việc trên Prefab/Sub-scene riêng.<br>• Phân chia rõ ràng vùng thư mục theo bảng phân vai trong `PROJECT_ROADMAP.md`. |
| **R-03** | **Đồ Họa & Animation (Art Bottleneck)** | Mất quá nhiều thời gian đi tìm hoặc dựng model thú Việt Nam / Sói 9 Bậc làm đình trệ tiến độ code. | **Cao** | Lập trình viên phải chờ đợi artist hoặc không có animation để test logic. | • Giai đoạn M1–M8: Dùng **3D Placeholder / Low-Poly + Mixamo Animation** 100%.<br>• Tách rời `VisualRoot` khỏi logic code để thay model 3D bất kỳ lúc nào mà không sửa script. |
| **R-04** | **Lệch Nhịp Phối Hợp 2 AI (Model Drift)** | GPT-6 Astra và Gemini 4 Argon đặt tên class, enum hoặc cấu trúc script không khớp nhau giữa các phiên. | **Cao** | Chuyển đổi giữa 2 AI làm project không compile được do sai tên phương thức hoặc kiểu dữ liệu. | • Bắt buộc dùng `docs/architecture/GLOSSARY.md` và `docs/handoff/CURRENT_STATE.md` làm tham chiếu duy nhất.<br>• AI mới phải chạy `Rule 1 — Inspect Before Editing` trước khi viết code. |
| **R-05** | **AI Quái Kẹt Đường (NavMesh Bottleneck)** | Quái vật đứng đơ khi người chơi xây tường bít kín cổng làng, hoặc nhiều quái chen chúc nhau ở lối hẹp. | **Trung bình** | Trong đêm Huyết Nguyệt, Sói đứng im ngoài tường hoặc chạy vòng quanh vô tận. | • Lập trình kiểm tra trạng thái đường đi `NavMeshPathStatus.PathPartial`.<br>• Nếu không tìm được đường tới mục tiêu bên trong, AI tự động chuyển mục tiêu sang tấn công đoạn tường/rào chắn gần nhất. |
| **R-06** | **Cân Bằng Gameplay & Thua Màn Chơi** | Sói Vua mạnh lên quá nhanh do người chơi phát triển chậm hoặc lơ là chặn đường tiếp tế, dẫn đến thua cuộc. | **Trung bình** | Làng bị áp đảo hoàn toàn trong các đêm Huyết Nguyệt sau. | • Giữ nguyên cơ chế Hardcore Survival (chấp nhận thua màn chơi).<br>• Hiển thị bảng thống kê chi tiết sau khi Game Over để người chơi rút kinh nghiệm.<br>• Đưa toàn bộ chỉ số tăng trưởng vào `WolfKingConfigSO` để dễ tinh chỉnh độ khó khi test. |
| **R-07** | **Lỗi Bộ Nhớ & Tụt FPS (Performance)** | Tạo quá nhiều GameObject (mũi tên, quái vật, số sát thương) làm sụt giảm FPS trong đêm công thành. | **Trung bình** | Trận đánh đông quái bị giật lag, thời gian thu gom rác (GC) tăng đột biến. | • Áp dụng `Object Pooling` cho đạn nỏ, hiệu ứng máu, quái vật.<br>• Dùng `Physics.OverlapSphereNonAlloc` để kiểm tra vùng chém thay vì tạo collider trigger tạm thời. |
| **R-08** | **Java OOP Bias (Overengineering)** | Áp dụng quá mức tư duy Enterprise Java (tạo hàng loạt Abstract Factory, Interface trung gian, Reflection) vào Unity. | **Trung bình** | Một hành vi nhặt đồ phải đi qua 6 class, không cấu hình được trên Inspector của Unity. | Tuân thủ **Rule 5**: Chỉ tạo Interface khi thực sự có $\ge 2$ đối tượng hoàn toàn khác nhau cùng triển khai (ví dụ: `IDamageable`, `IInteractable`). |

