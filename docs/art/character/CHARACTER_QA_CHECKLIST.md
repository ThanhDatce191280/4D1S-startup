# BẢNG TIÊU CHÍ NGHIỆM THU CHẤT LƯỢNG MỸ THUẬT NHÂN VẬT 3D
## (CHARACTER ART QA ACCEPTANCE CHECKLIST)

---

**Dự án:** Huyết Nguyệt Đại Ngàn  
**Mã tài liệu:** `CAB-SPEC-08`  
**Phiên bản:** `1.0.0`  
**Ngày cập nhật:** 31/10/2026  
**Trạng thái tài liệu:** `PROPOSED` (Tiêu chuẩn nghiệm thu chính thức cho toàn bộ tài sản 3D nhân vật)  
**Tài liệu cha:** [CHARACTER_ART_BIBLE.md](../CHARACTER_ART_BIBLE.md)  
**Tác giả:** Lead QA Coordinator & Technical Artist  

---

## MỤC LỤC
1. [Nguyên Tắc Vận Hành và Thang Đo Mức Độ Lỗi (Severity Definitions)](#1-nguyên-tắc-vận-hành-và-thang-đo-mức-độ-lỗi-severity-definitions)
2. [A. Concept & Thiết Kế Mỹ Thuật (Concept QA)](#a-concept--thiết-kế-mỹ-thuật-concept-qa)
3. [B. Kích Thước và Hình Thể Mô Hình (Modeling QA)](#b-kích-thước-và-hình-thể-mô-hình-modeling-qa)
4. [C. Cấu Trúc Lưới và Vòng Lặp Cạnh (Topology QA)](#c-cấu-trúc-lưới-và-vòng-lặp-cạnh-topology-qa)
5. [D. Trải Tọa Độ UV (UV Layout QA)](#d-trải-tọa-độ-uv-uv-layout-qa)
6. [E. Chất Lượng Bản Vẽ Bề Mặt (Texture QA)](#e-chất-lượng-bản-vẽ-bề-mặt-texture-qa)
7. [F. Thiết Lập Vật Liệu và Shader (Material QA)](#f-thiết-lập-vật-liệu-và-shader-material-qa)
8. [G. Khung Xương và Quét Trọng Lượng (Rig & Skinning QA)](#g-khung-xương-và-quét-trọng-lượng-rig--skinning-qa)
9. [H. Hoạt Cảnh và Đồng Bộ Đòn Đánh (Animation QA)](#h-hoạt-cảnh-và-đồng-bộ-đòn-đánh-animation-qa)
10. [I. Tiêu Chuẩn Xuất File FBX (FBX Export QA)](#i-tiêu-chuẩn-xuất-file-fbx-fbx-export-qa)
11. [J. Tích Hợp Dự Án Unity (Unity Integration QA)](#j-tích-hợp-dự-án-unity-unity-integration-qa)
12. [K. Khả Năng Tương Thích Trang Bị Modular (Equipment QA)](#k-khả-năng-tương-thích-trang-bị-modular-equipment-qa)
13. [L. Hiệu Năng và Giới Hạn Kỹ Thuật (Performance QA)](#l-hiệu-năng-và-giới-hạn-kỹ-thuật-performance-qa)
14. [M. Nhận Diện Ở Góc Nhìn Isometric (Visual Readability QA)](#m-nhận-diện-ở-góc-nhìn-isometric-visual-readability-qa)
15. [Quy Trình Ký Duyệt Xuất Xưởng (Sign-off Protocol)](#quy-trình-ký-duyệt-xuất-xưởng-sign-off-protocol)

---

## 1. NGUYÊN TẮC VẬN HÀNH VÀ THANG ĐO MỨC ĐỘ LỖI (SEVERITY DEFINITIONS)

Mọi tài sản nhân vật (Character Mesh, Equipment, Rig, Animation) trước khi được tích hợp vào nhánh chính (`main`) của dự án phải trải qua quá trình đánh giá độc lập bởi Chuyên viên QA và Technical Artist.

### Thang đo mức độ nghiêm trọng:
- **`BLOCKER` (Chí mạng):** Làm vỡ build, crash engine, sai lệch Avatar Mecanim không thể chuyển động, hoặc làm sai lệch trục quay $90^\circ$. **Khóa luồng bàn giao ngay lập tức.**
- **`CRITICAL` (Nghiêm trọng):** Rách lưới hở thịt khi cử động, thiếu texture map chính, đâm xuyên giáp sâu, hoặc vượt quá $30\%$ ngân sách đa giác. Phải sửa trước khi test gameplay.
- **`MAJOR` (Đáng kể):** UV bị méo nhẹ ở chi tiết phụ, bóng đổ không đồng nhất, thiếu âm thanh bước chân hoặc lệch frame hitbox nhỏ.
- **`MINOR` (Nhẹ):** Lệch màu nhẹ so với concept gốc, hoa văn viền hơi mờ ở cự ly cực cận, có thể sửa ở đợt Polish cuối.

### Trạng thái đánh giá chuẩn:
- **`PASS`**: Đạt đầy đủ tiêu chí yêu cầu.
- **`FAIL`**: Không đạt, bị từ chối và gửi trả về khâu sản xuất trước đó.
- **`BLOCKED`**: Bị tắc nghẽn do phụ thuộc vào một tài nguyên khác chưa hoàn thành.
- **`NOT APPLICABLE (N/A)`**: Tiêu chí không áp dụng cho loại tài nguyên này (ví dụ: vũ khí tĩnh không cần kiểm tra Skin Weight).

---

## 2. A. CONCEPT & THIẾT KẾ MỸ THUẬT (CONCEPT QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-A01** | Bản sắc văn hóa Đại Ngàn Đông Sơn | Khai thác đúng hoa văn trống đồng, giáp đồng/da sấy khói, tre nứa bản địa theo [CHARACTER_STYLE_GUIDE.md](CHARACTER_STYLE_GUIDE.md). | Mang phong cách Hiệp sĩ Trung Cổ châu Âu hoặc Cyberpunk/Sci-fi. | Kiểm tra đối chiếu thị giác bởi Art Director | `CRITICAL` |
| **QA-A02** | Tỷ lệ cơ thể Stylized | Tuân thủ tỷ lệ đầu/thân: $1:6.8$ (Nam), $1:6.5$ (Nữ), $1:4.5$ (Người Lùn). | Nhân vật bị biến thành Chibi đầu to hoặc siêu thực gầy guộc. | Đo lường tỷ lệ đầu so với chiều cao tổng thể | `MAJOR` |
| **QA-A03** | Khung hình chữ V & Silhouette | Khối vai và ngực nhô cao rõ ràng, phân biệt rõ vai trò chiến đấu từ góc nhìn $16\text{m}$. | Silhouette bị tròn vo như khối trụ, không rõ tay cầm vũ khí. | Đổ màu đen tuyền (Pure Black Silhouette Test) | `CRITICAL` |

---

## 3. B. KÍCH THƯỚC VÀ HÌNH THỂ MÔ HÌNH (MODELING QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-B01** | Chiều cao kỹ thuật tuyệt đối | Nhân vật Người Chơi cao đúng $1.80\text{m} \pm 0.02\text{m}$ (Từ đế giày đến đỉnh đầu). | Cao $< 1.70\text{m}$ hoặc $> 1.90\text{m}$ làm lệch Capsule Collider. | Đo bằng thước đo Blender Measure Tool | `BLOCKER` |
| **QA-B02** | Tư thế tạo hình chuẩn A-Pose | Hai tay xuôi nghiêng $45^\circ$, lòng bàn tay úp $45^\circ$ về trước, chân mở rộng $0.25\text{m}$. | Nhân vật ở tư thế T-Pose thẳng tuột hoặc buông thõng sát thân. | Đối chiếu góc xoay xương cánh tay | `CRITICAL` |
| **QA-B03** | Gốc tọa độ Pivot Point | Pivot nằm tại $(0, 0, 0)$ chính giữa hai gót chân tiếp xúc mặt đất. | Pivot nằm ở rốn, ở ngực hoặc bay lơ lửng ngoài cơ thể. | Kiểm tra Object Origin trong 3D Viewport | `BLOCKER` |
| **QA-B04** | Trạng thái Transform sạch | Position $(0,0,0)$, Rotation $(0,0,0)$, Scale $(1.0, 1.0, 1.0)$. | Scale bị âm ($-1$) hoặc khác $1.0$ (ví dụ: $0.01$ hay $100.0$). | Kiểm tra bảng Transform (`N-Panel` Blender) | `BLOCKER` |

---

## 4. C. CẤU TRÚC LƯỚI VÀ VÒNG LẶP CẠNH (TOPOLOGY QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-C01** | Tuyệt đối không có N-gons | $0$ mặt có số cạnh $\ge 5$. Lưới Quad chiếm $\ge 90\%$. | Tồn tại bất kỳ mặt N-gon nào trên mô hình. | Chạy lệnh `Select -> By Number of Vertices -> Greater than 4` | `BLOCKER` |
| **QA-C02** | Edge Loops khớp cử động | Tối thiểu 3 vòng edge loop tại đầu gối, khuỷu tay; vòng lặp kép tại vai. | Khớp chỉ có 1 cạnh đơn, khi uốn bị xẹp lép mất thể tích. | Uốn cong thử nghiệm khớp $90^\circ$ trong Blender | `CRITICAL` |
| **QA-C03** | Không có đỉnh cô lập / Đa giác trùng lặp | $0$ đỉnh rời rạc, $0$ mặt trùng đè (*Z-fighting*), $0$ cạnh không liền kề (*Non-manifold*). | Có mặt úp ngược hoặc cạnh chia sẻ bởi $\ge 3$ mặt. | Chạy lệnh Mesh Cleanup (`Merge by Distance`, `Select Non-Manifold`) | `CRITICAL` |
| **QA-C04** | Hướng pháp tuyến bề mặt (Face Normals) | $100\%$ mặt quay ra ngoài (Hiển thị màu xanh trong Face Orientation). | Tồn tại mặt quay ngược vào trong (Màu đỏ). | Bật hiển thị `Viewport Overlays -> Face Orientation` | `BLOCKER` |

---

## 5. D. TRẢI TỌA ĐỘ UV (UV LAYOUT QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-D01** | Độ phân bố mật độ điểm ảnh (Texel Density) | Đạt trung bình $10.24\text{ px/cm} \pm 10\%$ trên Master Atlas $2048 \times 2048$. | Các bộ phận chênh lệch mật độ quá $40\%$ (mặt quá nét, chân mờ căm). | Kiểm tra bằng Texel Density Checker Addon | `MAJOR` |
| **QA-D02** | Khoảng cách đệm đảo UV (UV Padding) | Tối thiểu $16\text{ pixels}$ padding giữa các đảo UV trên texture 2K. | Các đảo UV dính sát nhau làm lem màu khi render Mipmap. | Kiểm tra thủ công tại UV Editor ở mức zoom 1:1 | `CRITICAL` |
| **QA-D03** | Vị trí giấu đường cắt (UV Seams) | Đường Seam nằm ở mặt trong đùi, gầm nách, gáy tóc. | Đường Seam cắt đôi sống mũi, giữa ngực áo hoặc mặt khiên. | Quan sát đường viền Seam trên 3D Viewport | `MAJOR` |
| **QA-D04** | Tỷ lệ diện tích hữu dụng | Diện tích đảo UV phủ $\ge 80\%$ không gian texture sheet (Lãng phí $< 20\%$). | Khoảng trống màu đen vô nghĩa chiếm $> 25\%$ diện tích. | Đo lường tỷ lệ bao phủ của UV Pack | `MAJOR` |

---

## 6. E. CHẤT LƯỢNG BẢN VẼ BỀ MẶT (TEXTURE QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-E01** | Hiệu ứng nướng ánh sáng giả lập (Gradient Bake) | BaseMap có dải sáng giảm dần từ đỉnh đầu xuống chân ($100\% \to 60\%$). | Nhân vật sáng đều phẳng lì hoặc chân sáng hơn đầu. | Xem Texture ở chế độ Unlit trong Unity Scene | `CRITICAL` |
| **QA-E02** | Viền sáng mép cạnh (Edge Highlights) | Các gờ vảy đồng, mép kiếm có nét cọ viền sáng rõ nét $1-2\text{px}$. | Cạnh giáp mờ nhòe, không tách bạch khỏi bề mặt phẳng. | Quan sát ở góc nhìn Isometric $16\text{m}$ | `MAJOR` |
| **QA-E03** | Đóng gói kênh MaskMap chính xác | R=Metallic, G=AO, B=Detail, A=Smoothness đúng định dạng. | Kênh R và G bị đảo lộn làm da sáng lóa như sắt hoặc giáp tối xỉn. | Tách kênh (Split Channels) trong Photoshop / GIMP | `BLOCKER` |
| **QA-E04** | Chuẩn nén không vỡ hạt (Compression Artifacts) | Sử dụng chuẩn BC7 (PC), không xuất hiện vết rạn khối màu loang lổ. | Dùng chuẩn DXT1/BC1 làm vỡ nát các dải chuyển màu êm ái. | Kiểm tra Inspector Texture Format trong Unity | `CRITICAL` |

---

## 7. F. THIẾT LẬP VẬT LIỆU VÀ SHADER (MATERIAL QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-F01** | Tương thích Universal Render Pipeline | Sử dụng Shader `Universal Render Pipeline/Lit` hoặc Shader Graph chuẩn. | Dùng shader cũ Built-in (Standard Shader) bị báo lỗi màu tím hồng. | Kiểm tra thuộc tính Shader trên Material | `BLOCKER` |
| **QA-F02** | Số lượng Material trên nhân vật | Tối đa $1 - 2$ Material cho toàn bộ cơ thể và trang bị ghép lại. | Nhân vật bị chia nhỏ thành $\ge 4$ Material làm tăng Draw Call. | Đếm số lượng Material Slots trên SkinnedMeshRenderer | `CRITICAL` |
| **QA-F03** | Tương thích SRP Batcher | Khối Material hiển thị thông báo: `"Compatible with SRP Batcher"`. | Báo `"Not compatible with SRP Batcher"` do dùng biến ngoài chuẩn. | Mở Inspector của Material $\to$ Xem dòng SRP Batcher | `BLOCKER` |

---

## 8. G. KHUNG XƯƠNG VÀ QUÉT TRỌNG LƯỢNG (RIG & SKINNING QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-G01** | Tương thích Unity Mecanim Avatar | Avatar hiển thị khung xương xanh toàn bộ ($100\%$ Green Mapping), không lỗi. | Xuất hiện xương màu đỏ hoặc cảnh báo thiếu xương bắt buộc. | Mở cửa sổ cấu hình `Configure Avatar` trong Unity | `BLOCKER` |
| **QA-G02** | Giới hạn số xương tác động trên đỉnh | Tối đa 4 xương ảnh hưởng trên mỗi đỉnh (Max 4 influences). | Có đỉnh chịu ảnh hưởng của $\ge 5$ xương. | Kiểm tra bảng Mesh Statistics / Weight Table | `CRITICAL` |
| **QA-G03** | Không có đỉnh mang trọng lượng rác (Zero Weights) | Tổng trọng lượng xương trên mỗi đỉnh chính xác bằng $1.0$. | Đỉnh bị dính cứng tại tọa độ $(0,0,0)$ tạo gai nhọn kéo dài sàn. | Xoay xương Hips: Tìm đỉnh không chuyển động theo | `BLOCKER` |
| **QA-G04** | Khử biến dạng nách và đáy chậu | Khi hạ tay $45^\circ$, thịt nách không dính vào sườn; khi bước chân, háng không rách. | Thịt bị co rúm méo mó hoặc thủng lỗ hổng khi cử động. | Chạy thử nghiệm chuỗi tư thế cực đoan (Pose Stress Test) | `CRITICAL` |

---

## 9. H. HOẠT CẢNH VÀ ĐỒNG BỘ ĐÒN ĐÁNH (ANIMATION QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-H01** | Chính sách hoạt cảnh tại chỗ (In-Place Policy) | Toàn bộ clips Locomotion di chuyển tại chỗ, Root Bone đứng im tại $(0,0,0)$. | Nhân vật bị trôi vị trí ra khỏi tâm gốc tọa độ trong clip. | Bật hiển thị Gizmo trục tọa độ trong cửa sổ Animation | `BLOCKER` |
| **QA-H02** | Khử trượt chân trên mặt sàn (Foot Sliding) | Bàn chân trụ ghim chặt trên sàn trong chu kỳ bước chân tiếp đất. | Bàn chân bị rê trượt băng trên mặt đất khi đi bộ/chạy. | Cho nhân vật đi trên lưới kẻ ô vuông trong Scene | `MAJOR` |
| **QA-H03** | Mốc sự kiện chém đòn (Animation Events) | Sự kiện `OnAttackHitboxStart` và `End` kích hoạt đúng frame vung kiếm chém. | Hitbox bật quá sớm trước khi vung kiếm hoặc bật sau khi thu chiêu. | So sánh thời điểm Event với đường cung của vệt kiếm | `CRITICAL` |
| **QA-H04** | Nhịp đòn dứt khoát (Attack Telegraph & Snappiness) | Có pha lấy đà $0.1\text{s}$, pha chém cực nhanh $0.05\text{s}$, pha dừng khựng $0.1\text{s}$. | Đòn đánh trôi tuột đều đều như bơi lội, không có cảm giác sức nặng. | Đánh giá cảm giác bấm nút chém thực tế (Game feel) | `CRITICAL` |

---

## 10. I. TIÊU CHUẨN XUẤT FILE FBX (FBX EXPORT QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-I01** | Trục xoay không bị lệch $90^\circ$ | Nhân vật trong Unity đứng thẳng trục $Y$-Up, mặt nhìn thẳng hướng $+Z$-Forward. | Nhân vật bị nằm ngửa ra sàn hoặc quay ngược mặt vào trong màn hình. | Kéo FBX vào Scene trắng, kiểm tra Transform Rotation $(0,0,0)$ | `BLOCKER` |
| **QA-I02** | Tắt xương lá thừa (No Leaf Bones) | Tùy chọn `Add Leaf Bones` đã TẮT khi xuất từ Blender. | Xuất hiện hàng loạt xương cụt mang tên `_end` ở đầu ngón tay. | Mở cây phân cấp Hierarchy của Armature | `CRITICAL` |
| **QA-I03** | Tỷ lệ Scale Factor đồng nhất | Trong Unity Inspector, thuộc tính `Scale Factor = 1.0` và `File Scale = 1.0`. | Scale Factor bị nhảy thành $0.01$ hoặc $100.0$ do sai đơn vị xuất. | Kiểm tra tab Model trong FBX Import Settings | `BLOCKER` |

---

## 11. J. TÍCH HỢP DỰ ÁN UNITY (UNITY INTEGRATION QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-J01** | Cấu trúc cây thư mục chuẩn | Lưu đúng tại `Assets/_Project/Art/Characters/` và `Prefabs/Characters/`. | Lưu rải rác ngoài thư mục gốc `Assets/` hoặc trong thư mục lạ. | Kiểm tra đường dẫn Project Browser | `MAJOR` |
| **QA-J02** | Phân tách Logic Root & VisualRoot | Model 3D nằm trong `PlayerVisual`, không gắn `CharacterController` trực tiếp vào mesh. | Gắn script vật lý/máu trực tiếp vào GameObject chứa SkinnedMesh. | Kiểm tra Hierarchy của Prefab `Player_Prototype` | `BLOCKER` |
| **QA-J03** | Không sinh rác bộ nhớ khi hoạt động | Không có lệnh cấp phát `GC.Alloc` trong `PlayerVisualAnimationBridge.Update()`. | Bộ nhớ rác tăng vọt mỗi frame do gọi string hoặc GetComponent. | Chạy Unity Profiler tab CPU Usage | `CRITICAL` |

---

## 12. K. KHẢ NĂNG TƯƠNG THÍCH TRANG BỊ MODULAR (EQUIPMENT QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-K01** | Chống đâm xuyên giáp (Clipping Check) | Khi chạy, nhảy, chém kiếm: Thịt ngực và bụng không đâm thủng ra ngoài áo giáp. | Da thịt nhân vật lồi ra ngoài lớp áo giáp sắt/đồng. | Test chuyển động trong game với camera quay xung quanh | `CRITICAL` |
| **QA-K02** | Khớp khít ranh giới mép cắt (Boundary Seams) | Khi tráo đổi tay/chân: Không xuất hiện khe hở nhìn xuyên thấu vào trong thân thể. | Thấy vệt hở rỗng ruột ở cổ tay hoặc cổ chân. | Phóng to camera cực cận kiểm tra các vòng nối cổ tay | `CRITICAL` |
| **QA-K03** | Tự động ẩn tóc khi đội nón (Hair Interaction)| Mũ da sói hoặc nón lá che kín tóc không bị các lọn tóc dài đâm thủng vành mũ. | Tóc xơ xác đâm xuyên qua chóp nón lá. | Trang bị nón lá và kiểm tra các góc nhìn | `MAJOR` |

---

## 13. L. HIỆU NĂNG VÀ GIỚI HẠN KỸ THUẬT (PERFORMANCE QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-L01** | Ngân sách đa giác LOD0 | Nhân vật người chơi $\le 12,000$ Tris; Quái Sói $\le 4,500$ Tris. | Vượt quá ngân sách $> 15\%$. | Kiểm tra thông số Triangles trong Unity Rendering Statistics | `CRITICAL` |
| **QA-L02** | Chuyển đổi mức chi tiết LODGroup | Hoạt động trơn tru: LOD0 $\to$ LOD1 ($18\text{m}$) $\to$ LOD2 ($30\text{m}$) $\to$ Culled ($>45\text{m}$). | Không giảm đa giác ở xa hoặc bị lỗi giật hình (Popping) quá gắt. | Kéo thanh trượt camera trong Scene View để kiểm tra | `MAJOR` |
| **QA-L03** | Tải trọng kiểm thử Fixture 6 người + 20 sói | Chạy ổn định trong kịch bản stress-test không làm nghẽn CPU/GPU. | Tụt khung hình nghiêm trọng do vẽ quá nhiều xương hoặc draw call. | Chạy Scene kịch bản chuẩn với Frame Debugger | `BLOCKER` |

---

## 14. M. NHẬN DIỆN Ở GÓC NHÌN ISOMETRIC (VISUAL READABILITY QA)

| ID | Yêu Cầu Kỹ Thuật | Tiêu Chí Đạt (Pass Criteria) | Tiêu Chí Trượt (Fail Criteria) | Phương Pháp Kiểm Tra | Mức Độ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QA-M01** | Nhận diện vũ khí trong 0.2 giây | Người chơi nhận biết ngay đối phương cầm Kiếm hay Cung ở cự ly $16\text{m}$. | Không phân biệt được loại vũ khí do kích thước quá nhỏ. | Đánh giá mù (Blind Test) với người chơi mới | `CRITICAL` |
| **QA-M02** | Phân biệt đẳng cấp trang bị | Nhận diện rõ rệt trang bị Vải thô (T1) $\leftrightarrow$ Giáp Đồng (T2) $\leftrightarrow$ Giáp Bạc (T3). | Giáp cấp cao trông tầm thường như vải rách hoặc ngược lại. | So sánh song song 3 bộ giáp trên màn hình | `MAJOR` |
| **QA-M03** | Nổi bật trên nền rừng rậm và đầm lầy | Nhân vật không bị "chìm màu" vào tán lá hoặc bãi bùn lầy. | Nhân vật bị tàng hình vô ý thức do màu trùng khít với nền đất. | Đặt nhân vật vào Scene Rừng và Đầm lầy thực tế | `CRITICAL` |

---

## QUY TRÌNH KÝ DUYỆT XUẤT XƯỞNG (SIGN-OFF PROTOCOL)

Một tài sản chỉ được đóng dấu **APPROVED FOR PRODUCTION** khi thỏa mãn đồng thời:
1. **$100\%$ các tiêu chí BLOCKER đạt `PASS`**.
2. **$100\%$ các tiêu chí CRITICAL đạt `PASS`**.
3. **$\ge 90\%$ các tiêu chí MAJOR đạt `PASS`** (Các mục chưa đạt phải có kế hoạch khắc phục cụ thể trong Task Backlog).
4. Có chữ ký phê duyệt kép từ: **Lead Technical Artist** và **Lead QA Coordinator**.
