> **Cập nhật canon 31/10/2026:** Tài liệu này giữ nội dung nền v3 để truy vết, không còn là nguồn quyết định cao nhất. Xem [Product Canon](docs/vision/PRODUCT_CANON.md), đặc biệt bảng thay thế §4. Canon hiện hành: 3D zone-based; LAN 4–6 có host migration; phe cá nhân có thể ẩn; Mana dạng Sói và use-based mastery; vua tier9 duy nhất chết là endgame, Sói thắng thì vua tiến tier10 và endgame; chết rơi đồ vào mộ, phép đồng đội hồi sinh, team wipe là bad ending; Linh Thạch nhiều viên nhưng hữu hạn theo world. Các đoạn 2D, vua tái sinh mỗi trăng, Sói không Mana, triệu hồi boss tier10 đánh tiếp và 4 ending cũ phía dưới là **nội dung lịch sử bị thay thế**, không được triển khai khi trái canon. Phạm vi demo và gate M8 mới tại [MVP Scope](docs/vision/MVP_SCOPE.md).

# <span style="color:#fcd34d">GAME DESIGN DOCUMENT (GDD) & CỐT TRUYỆN HOÀN CHỈNH</span>

# HUYẾT NGUYỆT ĐẠI NGÀN

*Thể loại: 2D Pixel Art | Góc nhìn thứ 3 (Top-Down) | Sinh tồn - Nhập vai - Chiến thuật Phe phái (Sandbox Survival RPG)*

## <span style="color:#1b4332">1. TỔNG QUAN DỰ ÁN & BỐI CẢNH CỐT TRUYỆN (LORE BIBLE)</span>

### <span style="color:#7f1d1d">1.1. Thông số kỹ thuật & Phong cách chủ đạo</span>

- **Thể loại:** Game 2D Pixel Art kết hợp Sinh tồn thế giới mở (như Minecraft, Ark: Survival Evolved), Nhập vai hành động (Action RPG) và Nghi kỵ xã hội (Social Deduction).
- **Góc nhìn:** Góc nhìn thứ ba từ trên xuống (Top-Down 3/4 View), cho phép quan sát rõ địa hình rừng rậm, kiến trúc làng mạc, bẫy rập và tầm nhìn ban đêm (Fog of War).
- **Bối cảnh bản đồ:** Vùng đất cổ đại 'Đại Ngàn Văn Lang' được thiết kế dựa theo địa hình chữ S của Việt Nam, trải dài từ đỉnh núi tuyết phía Bắc, xuyên qua hệ thống hang động đá vôi, rừng rậm nguyên sinh trung tâm, cồn cát nắng cháy miền Trung đến đầm lầy ngập mặn phương Nam.

### <span style="color:#7f1d1d">1.2. Cốt truyện chính: Lời Nguyền Huyết Nguyệt & Ngũ Linh Thạch</span>

Hàng ngàn năm trước, vùng đất Đại Ngàn sống trong sự hòa hợp dưới sự bảo hộ của 5 viên Đá Nguyên Tố (Ngũ Linh Thạch: Hỏa, Thủy, Thổ, Mộc, Băng) do Tộc Người Lùn rèn giũa và các Phù Thủy Cổ Đại canh giữ. Thế nhưng, vào một đêm Nguyệt Thực Đỏ, một thực thể cổ xưa mang tên 'Thần Sói Cấp 10 (Huyết Lang Vương)' đã giáng xuống đại ngàn. Dù liên minh Nhân Tộc, Người Lùn và Phù Thủy đã phong ấn được xác thần của nó, dòng máu nguyền rủa của nó đã phát tán vào rừng sâu, biến những thợ săn và dã thú thành 'Người Sói'.

Mỗi chu kỳ trăng, một cá thể mang dòng máu thuần khiết nhất sẽ thức tỉnh thành 'Sói Vua'. Lúc mới sinh ra, Sói Vua rất yếu ớt, ẩn mình trong bóng tối và được bầy sói con đi săn mồi đem về nuôi dưỡng. Ngày qua ngày, bầy Người Sói không chỉ mạnh lên về cơ bắp mà còn tiến hóa trí khôn: chúng biết lột da những Người Du Mục xấu số để trà trộn vào các ngôi làng ban ngày, buôn bán và dò xét sơ hở của con người, chờ đêm xuống lại hóa thú đồ sát.

Người chơi khởi đầu là một Dân Làng trẻ tuổi vừa đặt chân đến ngôi làng khai hoang giữa Rừng Rậm. Đứng trước nguy cơ Sói Vua trưởng thành và thống lĩnh đại quân xóa sổ nhân loại, bạn nắm trong tay vận mệnh của cả vùng đất với 3 ngả rẽ: Bảo vệ ngôi làng và rèn thần binh diệt Sói; Phản bội nhân loại để bước lên ngôi vị Chúa Tể Bầy Sói; hoặc gia nhập Tổ Chức Săn Sói của Liên Hiệp Làng với âm mưu dùng 9 Bậc Sói để triệu hồi lại Thần Sói Cấp 10.


## <span style="color:#7f1d1d">1.3. Diễn biến Cốt truyện theo 4 Hồi (4-Act Progression)</span>

- **Hồi I - Bình Minh Khai Hoang (Ngày 1 - Ngày 15):** Người chơi cùng Trưởng Làng chặt gỗ, dựng nhà, trồng lúa, thuần hóa thú rừng tại Biome Rừng Rậm. Ban đêm chỉ có Sói Bậc 1-2 đi săn lẻ tẻ. Sói Vua còn đang trong kỳ 'Ấu Thể', ẩn náu rất kỹ. Người Du Mục bắt đầu ghé làng trao đổi hàng hóa.
- **Hồi II - Giả Nhân Giữa Chợ (Ngày 16 - Ngày 35):** Người Sói tiến hóa trí thông minh. Chúng ăn thịt Người Du Mục và đóng giả thành các đoàn thương buôn (2-4 người) vào làng ban ngày. Người Sói Đặc Biệt xuất hiện, biết tìm Người Lùn rèn Kiếm, Cung, Rìu, Búa và có thể hóa sói bất kỳ lúc nào. Người chơi bắt đầu khám phá Đầm Lầy, Sa Mạc và chọn nhánh tiến hóa (Kiếm Sĩ, Thợ Săn, Pháp Sư) hoặc tìm cách Hợp Thể với Sói.
- **Hồi III - Đại Dịch Huyết Nguyệt & Âm Mưu Liên Hiệp (Ngày 36 - Ngày 60):** Sói Vua đạt trạng thái trưởng thành nhờ lượng thức ăn tích lũy mỗi ngày. Những đợt công thành ban đêm (Blood Moon Raid) diễn ra dữ dội với buff x3 toàn quân khi Sói Vua xuất hiện. Cùng lúc đó, 'Tổ Chức Săn Sói' từ Liên Hiệp Làng lộ diện, dụ dỗ người chơi săn đủ 9 Bậc Sói để thực hiện nghi thức cấm.
- **Hồi IV - Chung Cuộc Đại Ngàn (Ngày 60+ / Endgame):** Tiến vào Hang Động sâu thẳm và đỉnh Núi Tuyết để thu thập đủ 5 Đá Nguyên Tố, tìm Vua Người Lùn rèn Vũ Khí Huyền Thoại, và quyết định kết cục của thế giới qua 4 Ending khác nhau.

## <span style="color:#1b4332">2. THIẾT KẾ THẾ GIỚI: 5 BIOMES & HỆ SINH THÁI VIỆT NAM</span>

### <span style="color:#7f1d1d">2.1. Phân cấp độ quái và đặc trưng 5 Biomes</span>

Bản đồ được thiết kế liền mạch theo trục địa lý Việt Nam. Độ khó của quái vật, cấp bậc Người Sói và sự khắc nghiệt của thời tiết tăng dần từ Biome 1 đến Biome 5:

<table>
<thead>
<tr><th>Cấp</th><th>Tên Biome (Cảm hứng VN)</th><th>Cấp Quái &#x26; Sói</th><th>Môi trường &#x26; Cơ chế đặc trưng</th><th>NPC / Tài nguyên chính</th></tr>
</thead>
<tbody>
<tr><td><b>Biome 1</b></td><td>Rừng Rậm Trường Sơn<br />(Vùng Khởi Đầu)</td><td>Cấp 1 - 15<br />(Sói Bậc 1 - 2)</td><td>Cây cối rậm rạp, mưa nhiệt đới, sông suối. Nơi đặt Làng Chính.</td><td>Trưởng Làng, Người Lùn Thường, Phù Thủy Tốt, Đá Thực Vật (Mộc).</td></tr>
<tr><td><b>Biome 2</b></td><td>Đầm Lầy Ngập Mặn<br />(Rừng Sác / U Minh)</td><td>Cấp 15 - 30<br />(Sói Bậc 3 - 4)</td><td>Bùn lầy làm giảm 30% tốc độ chạy, sương mù độc ban đêm, tầm nhìn hẹp.</td><td>Phù Thủy Ác, Thảo dược quý, Độc dược, Đá Nước (Thủy).</td></tr>
<tr><td><b>Biome 3</b></td><td>Sa Mạc Cát Trắng<br />(Tiểu Sa Mạc Mũi Né)</td><td>Cấp 30 - 45<br />(Sói Bậc 5 - 6)</td><td>Ban ngày nắng gắt hao Thể Lực &#x26; Nước x2; bão cát làm mù hướng.</td><td>Trại Người Du Mục lớn, Đá Lửa (Hỏa), Quặng Vàng, Bẫy cát lún.</td></tr>
</tbody>
</table>

<table>
<tr><td><b>Biome 4</b></td><td>Hang Động Karst Ngầm (Hệ thống Sơn Đoòng)</td><td>Cấp 45 - 65 (Sói Bậc 7 - 8)</td><td>Tối hoàn toàn (cần đuốc/đèn tinh thể), thạch nhũ rơi, mê cung nhiều tầng.</td><td>Người Lùn Hiếm (Nghệ Nhân), Quặng Huyền Thiết, Đá Đất (Thổ).</td></tr>
<tr><td><b>Biome 5</b></td><td>Núi Tuyết Hoàng Liên (Đỉnh Phan Xi Păng)</td><td>Cấp 65 - 90+ (Sói Bậc 9 &#x26; Sói Vua)</td><td>Hàn khí đóng băng (mất máu nếu không có áo lông sói/Đá Lửa), bão tuyết.</td><td>Vua Người Lùn (Lò Rèn Thần), Hang ổ Sói Vua, Đá Băng (Hàn).</td></tr>
</table>


## <span style="color:#7f1d1d">**2.2. Hệ sinh thái Động vật Rừng Rậm Việt Nam (10 Loài)**</span>


<span style="color:#1f2937">Tại Biome Rừng Rậm, hệ động vật gồm 10 loài đặc trưng của rừng nhiệt đới Việt Nam, chia thành nhóm Có thể thuần hóa/chăn nuôi và nhóm Hung dữ tấn công người chơi:</span>

<table>
<thead>
<tr><th>STT</th><th>Tên Động Vật</th><th>Phân Loại</th><th>Tập tính &#x26; Cơ chế tương tác</th><th>Lợi ích khi Nuôi / Săn</th></tr>
</thead>
<tbody>
<tr><td>1</td><td>Trâu Nước Bản Địa</td><td>Thuần hóa / Nuôi</td><td>Hiền lành, sống gần sông; đánh trả nếu bị tấn công.</td><td>Kéo xe gỗ số lượng lớn, cày ruộng lúa, đỡ đòn khi Sói 襲.</td></tr>
<tr><td>2</td><td>Gà Rừng Lông Đỏ</td><td>Thuần hóa / Nuôi</td><td>Chạy nhanh trong bụi rậm, dễ bẫy bằng thóc.</td><td>Đẻ trứng mỗi ngày (hồi Thức Ăn), gáy báo hiệu trời sáng.</td></tr>
<tr><td>3</td><td>Lợn Rừng Trường Sơn</td><td>Bán hung dữ / Nuôi</td><td>Húc người nếu lại gần con non; có thể nuôi nhốt.</td><td>Cung cấp nhiều thịt, tự động đào nấm và củ trong rừng.</td></tr>
<tr><td>4</td><td>Voọc Ngũ Sắc</td><td>Thuần hóa (Thú cưng)</td><td>Sống trên tán cây cao, thông minh, nhạy bén.</td><td>Ngồi trên vai người chơi, hú cảnh báo khi có Người Sói giả dạng!</td></tr>
<tr><td>5</td><td>Sao La (Kỳ Lân Á Châu)</td><td>Thuần hóa (Quý hiếm)</td><td>Cực hiếm, xuất hiện ở suối sâu, bỏ chạy khi thấy người.</td><td>Thú cưỡi linh thiêng: Tăng tốc chạy và hồi Mana/Máu tự nhiên.</td></tr>
<tr><td>6</td><td>Rùa Núi Vàng</td><td>Thuần hóa / Nuôi</td><td>Di chuyển chậm, mai cứng miễn nhiễm sát thương tầm xa.</td><td>Lấy mai chế khiên hoặc nuôi lấy hào quang tăng Giáp cho làng.</td></tr>
</tbody>
</table>

<table>
<tr><td><b>7</b></td><td>Hổ Đông Dương</td><td>Hung dữ (Săn mồi)</td><td>Săn đơn độc ban ngày lẫn đêm, vồ từ bụi cỏ (Gây chảy máu).</td><td>Da Hổ may giáp cao cấp, Nanh Hổ rèn vũ khí Sát Thương Chí Mạng.</td></tr>
<tr><td><b>8</b></td><td>Báo Hoa Mai</td><td>Hung dữ (Săn mồi)</td><td>Tốc độ cực nhanh, leo cây và phục kích từ trên cao.</td><td>Thịt tăng tốc độ đánh, Da Báo tăng % Né Tránh (Dodge).</td></tr>
<tr><td><b>9</b></td><td>Gấu Ngựa</td><td>Hung dữ (Lãnh thổ)</td><td>Bảo vệ tổ mật ong và hang động, máu trâu, tát choáng.</td><td>Mật Gấu chế thuốc đại bổ hồi Thể Lực, Lông Gấu chống lạnh.</td></tr>
<tr><td><b>10</b></td><td>Trăn Gấm / Rắn Hổ Chúa</td><td>Hung dữ (Độc/Siết)</td><td>Ẩn dưới thảm lá mục, cắn gây Trúng Độc hoặc quấn giữ chân.</td><td>Nọc rắn dùng chế tên độc (Thợ Săn) hoặc thuốc giải (Phù Thủy).</td></tr>
</table>


<span style="color:#1b4332">

# <span style="color:#1b4332">**3. HỆ THỐNG TRẠNG THÁI, CHỈ SỐ & 5 ĐÁ NGUYÊN TỐ**</span>

</span>


<span style="color:#7f1d1d">

## <span style="color:#7f1d1d">**3.1. Cơ chế 4 Thanh Trạng Thái Mặc Định & Sự Khác Biệt Phe Phái**</span>

</span>


<table>
<thead>
<tr>
<th>Đối tượng</th>
<th>Thanh Máu (HP)</th>
<th>Thanh Thức Ăn (Hunger)</th>
<th>Thanh Thể Lực (Stamina)</th>
<th>Thanh Mana (MP)</th>
</tr>
</thead>
<tbody>
<tr>
<td><b>Dân Làng / Người Chơi</b></td>
<td>100% cơ bản (Tăng theo Giáp &#x26; Cấp)</td>
<td>Giảm theo thời gian; >80% tự hồi HP, 0% mất HP dần.</td>
<td>Dùng để chặt cây, đào mỏ, chạy, lăn né, đánh cận chiến.</td>
<td>100% cơ bản; dùng để tung Skill nghề nghiệp &#x26; Đá Nguyên Tố.</td>
</tr>
<tr>
<td><b>Người Sói Thường (Khi hóa Sói)</b></td>
<td>Nhân đôi (x2 HP) ngay khi biến hình.</td>
<td>Thanh Độ Đói Săn Mồi: Càng đói càng hung hãn, buộc phải đi săn.</td>
<td>Nhân đôi (x2 Stamina), chạy và vồ mồi liên tục không mệt.</td>
<td>KHÔNG CÓ MANA (0 MP) - Thuần sát thương vật lý &#x26; tốc độ.</td>
</tr>
<tr>
<td><b>Người Sói Đặc Biệt (Khi hóa Sói)</b></td>
<td>Nhân đôi (x2 HP) + Giáp từ trang bị Người Lùn.</td>
<td>Thanh Độ Đói điều khiển hành vi săn hoặc cướp bóc.</td>
<td>Nhân đôi (x2 Stamina), dùng được kỹ năng vũ khí nặng.</td>
<td>KHÔNG CÓ MANA (0 MP) - Bù lại dùng được Vũ Khí Rèn.</td>
</tr>
</tbody>
</table>

<table>
<tr>
<td><b>Sói Vua (Wolf King) (Khi hóa Sói)</b></td>
<td>Nhân đôi (x2 HP) + Cộng dồn từ thịt cống nạp mỗi ngày.</td>
<td>Được cả bầy Sói đem thức ăn về nuôi mỗi ngày để tăng chỉ số vĩnh viễn.</td>
<td>Nhân đôi (x2 Stamina).</td>
<td>CÓ THANH MANA &#x26; Nhân đôi (x2 MP) khi hóa Sói!</td>
</tr>
<tr>
<td><b>Bầy Sói đi cùng Sói Vua (Hiệu ứng Lãnh Đạo)</b></td>
<td>Nhân ba (x3 HP) khi Sói Vua đích thân dẫn đầu công làng!</td>
<td>Chuyển sang trạng thái Cuồng Huyết (Không biết sợ).</td>
<td>Nhân ba (x3 Stamina) &#x26; Tăng tốc độ di chuyển.</td>
<td>Nhận Hào Quang Hắc Ám từ Mana của Sói Vua.</td>
</tr>
</table>


## <span style="color:#7f1d1d">**3.2. Hệ thống 5 Đá Nguyên Tố (Ngũ Linh Thạch)**</span>


<span style="color:#1f2937">5 loại Đá Nguyên Tố có tỷ lệ xuất hiện cực kỳ hiếm. Người chơi chỉ có thể thu thập thông qua: (1) Hạ gục</span> <span style="color:#1f2937">Người Sói Đặc Biệt / Sói Bậc Cao, (2) Khai thác mạch ngầm ẩn sâu trong 5 Biomes, hoặc (3) Hoàn thành</span> <span style="color:#1f2937">chuỗi nhiệm vụ sinh tử của NPC (Trưởng Làng, Phù Thủy Tốt, Người Lùn).</span>

<table>
<thead>
<tr><th>Đá Nguyên Tố</th><th>Nơi hiếm gặp</th><th>Hiệu ứng khi Khảm Vũ Khí (Người Lùn Hiếm/Vua)</th><th>Hiệu ứng Phép Thuật (Pháp Sư)</th></tr>
</thead>
<tbody>
<tr><td><b>Đá Lửa (Hỏa)</b></td><td>Sa Mạc / Sói Bậc 6+</td><td>Đòn đánh gây Thiêu Đốt theo giây; khắc chế Sói Băng &#x26; giữ ấm ở Núi Tuyết.</td><td>Hỏa Cầu Nổ, Tường Lửa chắn cổng làng ban đêm.</td></tr>
<tr><td><b>Đá Nước (Thủy)</b></td><td>Đầm Lầy / Nhiệm vụ Thủy Thần</td><td>Đòn đánh hồi một phần HP/Mana; dập lửa và giải độc tố Đầm Lầy.</td><td>Sóng Nước Đẩy Lùi, Mưa Hồi Phục diện rộng cho dân làng.</td></tr>
<tr><td><b>Đá Đất (Thổ)</b></td><td>Hang Động Sâu / Người Lùn</td><td>Tăng mạnh Sức Nặng vũ khí (Búa/Rìu), gây Choáng (Stun) và phá giáp Sói.</td><td>Khiên Đá Thạch Anh, Dựng Tường Đá khẩn cấp, Động Đất.</td></tr>
<tr><td><b>Đá Thực Vật (Mộc)</b></td><td>Rừng Rậm Cổ Thụ / Phù Thủy Tốt</td><td>Đòn đánh triệu hồi dây leo trói chân mục tiêu và hút máu nhẹ.</td><td>Dây Leo Gai trói bầy Sói, Thúc đẩy cây trồng/lúa chín tức thì.</td></tr>
<tr><td><b>Đá Băng (Hàn)</b></td><td>Đỉnh Núi Tuyết / Sói Bậc 9</td><td>Làm Chậm 50% tốc độ đánh &#x26; di chuyển của Sói; tích đủ cộng dồn gây Đóng Băng.</td><td>Bão Tuyết Vĩnh Cửu, Giáp Băng phản sát thương cận chiến.</td></tr>
</tbody>
</table>

# <span style="color:#1b4332">4. CHI TIẾT CÁC PHE PHÁI, NHÂN VẬT & HỆ THỐNG TIẾN HÓA</span>

## <span style="color:#7f1d1d">4.1. Phe Dân Làng, Người Du Mục & Hệ thống Tiến hóa Nghề nghiệp</span>

- **Trưởng Làng (Village Chief):** NPC trung tâm điều hành toàn bộ ngôi làng. Trưởng Làng phân công <span style="color:#1f2937">công việc cho NPC dân làng, mở khóa bản vẽ nâng cấp (Tường gỗ -> Tường đá -> Tháp canh tên), và</span> <span style="color:#1f2937">phát lệnh giới nghiêm khi đêm xuống.</span>
- **Vòng lặp Sinh tồn Ban Ngày & Ban Đêm của Dân Làng:** Ban ngày, người chơi và dân làng chặt gỗ <span style="color:#1f2937">xây nhà, trồng lúa, trồng cây, chăn nuôi động vật, săn thú rừng và giao thương. Ban đêm, dân làng</span> <span style="color:#1f2937">thường đi ngủ lấy sức; những dân làng đã huấn luyện và người chơi sẽ mặc giáp, cầm vũ khí đi săn</span> <span style="color:#1f2937">Người Sói để thu thập Da Sói, Nanh Sói đem bán và cày EXP.</span>
- **Tiến hóa 3 Nhánh Nghề Nghiệp (Class Evolution):** Khi tích lũy đủ EXP qua lao động và chiến đấu, <span style="color:#1f2937">Dân Làng / Người Chơi tiến hóa thành: (1) Kiếm Sĩ (cận chiến, máu & giáp cao, bảo vệ làng), (2) Thợ Săn</span> <span style="color:#1f2937">(bắn cung, đặt bẫy, thuần hóa thú, phát hiện dấu chân sói), hoặc (3) Pháp Sư (phải tìm được Phù Thủy</span> <span style="color:#1f2937">Tốt trong rừng sâu để học nghề, dùng Mana & Đá Nguyên Tố).</span>
- **Người Du Mục & Cơ chế Ngụy Trang của Sói:** Người Du Mục thật đi thành đoàn (2-4 người) hoặc lẻ <span style="color:#1f2937">bóng để buôn bán vật phẩm quý giữa các vùng. Tuy nhiên, Người Sói sau khi ăn thịt Người Du Mục và</span> <span style="color:#1f2937">Dân Làng sẽ giả dạng hoàn hảo thành Người Du Mục vào buổi sáng! Chúng vẫn giao dịch bình thường,</span> <span style="color:#1f2937">thái độ hiền lành và không thể phân biệt bằng mắt thường nếu không có kỹ năng/công cụ soi chiếu.</span>

## <span style="color:#7f1d1d">4.2. Phe Người Sói: Tiến hóa theo Ngày, 9 Bậc Sói & Sói Vua</span>


### <span style="color:#1b4332">CƠ CHẾ ĐỌC ĐÁO: SỰ TIẾN HÓA THEO SỐ NGÀY & SÓI VUA ĐƯỢC NUÔI DƯỠNG</span>


- Số ngày sinh tồn càng cao, AI của Người Sói càng khôn ngoan (biết giả làm thương nhân dụ dân làng ra khỏi cổng, biết phá bẫy, biết nhắm vào kho lương thực).


- Người Sói Đặc Biệt: Thừa hưởng toàn bộ sức mạnh của Người Sói thường nhưng biết tìm Tộc Người Lùn để rèn Cung, Kiếm, Rìu, Búa. Chúng có thể chủ động hóa Sói bất cứ lúc nào (cả ban ngày lẫn ban đêm), và chỉ duy trì hình dạng con người trong một khoảng thời gian giới hạn để trà trộn.


- Sói Vua (Duy nhất 1 cá thể): Khởi đầu ở Ngày 1 rất yếu ớt và dễ chết. Nhưng mỗi đêm trôi qua, bầy Người Sói sẽ đi săn và đem thịt về nuôi Sói Vua. Càng để lâu, Sói Vua càng tiến hóa khủng khiếp, sở hữu cả Thanh Mana (x2 Máu, Thể Lực, Mana) và Kỹ năng Lãnh Đạo: Khi Sói Vua xuất trận cùng bầy, toàn bộ Người Sói được buff x3 thay vì x2!

<table>
<thead>
<tr>
<th>Bậc Sói</th>
<th>Danh Hiệu</th>
<th>Hình Dạng Biến Hình</th>
<th>Khả Năng &#x26; Kỹ Năng Đặc Trưng</th>
</tr>
</thead>
<tbody>
<tr>
<td><b>Bậc 1</b></td>
<td>Huyết Lang Sơ Khởi</td>
<td>Sói Xám Hoang Dã</td>
<td>Chỉ hoạt động ban đêm, săn thú nhỏ và dân làng đi lẻ, tốc độ cơ bản.</td>
</tr>
</tbody>
</table>

<table>
<tr><td>Bậc 2</td><td>Nguỵ Nhân Lang</td><td>Người Du Mục / Sói Đen</td><td>Biết ăn thịt người để giả dạng thành Người Du Mục vào ban ngày (đi lẻ hoặc nhóm 2-4).</td></tr>
<tr><td>Bậc 3</td><td>Cuồng Trảo Lang</td><td>Bán Nhân Sói Cỡ Lớn</td><td>Móng vuốt xé rào gỗ, nhảy vượt tường thấp, đánh gây hiệu ứng Chảy Máu.</td></tr>
<tr><td>Bậc 4</td><td>Ám Ảnh Lang</td><td>Sói Sương Mù</td><td>Tàng hình ngắn hạn trong bóng tối/bụi rậm, chuyên ám sát mục tiêu ít máu.</td></tr>
<tr><td>Bậc 5</td><td>Chiến Binh Lang<br />(Người Sói Đặc Biệt)</td><td>Chiến Lang Mặc Giáp</td><td>Biết tìm Người Lùn rèn Kiếm/Rìu/Búa/Cung; biến hình tự do cả Ngày lẫn Đêm.</td></tr>
<tr><td>Bậc 6</td><td>Thống Lĩnh Lang<br />(Người Sói Đặc Biệt)</td><td>Cự Lang Hai Đầu / Cầm Binh Khí</td><td>Chỉ huy một tiểu đội 4-6 Sói Bậc thấp, sử dụng thành thạo kỹ năng vũ khí nặng.</td></tr>
<tr><td>Bậc 7</td><td>Nguyên Tố Lang</td><td>Ma Sói Khảm Linh Thạch</td><td>Hấp thụ sức mạnh 1 trong 5 Đá Nguyên Tố (Sói Lửa, Sói Băng, Sói Độc...).</td></tr>
<tr><td>Bậc 8</td><td>Cổ Huyết Lang Tổ</td><td>Quái Thú Khổng Lồ 4 Tay</td><td>Miễn nhiễm bẫy thường, tiếng hú làm hoảng loạn động vật và giảm giáp dân làng.</td></tr>
<tr><td>Bậc 9</td><td>Sói Vua (Wolf King)<br />/ Cận Vệ Tối Cao</td><td>Huyết Nguyệt Ma Vương</td><td>Sở hữu Mana (x2 HP/Stamina/MP), dùng mọi vũ khí, buff x3 toàn quân khi công làng.</td></tr>
<tr><td>CẤP 10<br />(Tối Thượng)</td><td>Thần Sói Cổ Đại<br />(Triệu hồi bởi Phe Săn Sói)</td><td>Thần Thú Khổng Lồ Đa Dạng</td><td>Thực thể thần thoại được Tổ Chức Săn Sói triệu hồi khi hiến tế đủ 9 Bậc Sói!</td></tr>
</table>

## <span style="color:#7f1d1d">**4.3. Cơ chế Hợp Thể (Người - Sói) & Phe Tổ Chức Săn Sói**</span>

- **Cơ chế Hợp Thể (Symbiosis Fusion - Chiến Binh Bán Sói):** Nếu người chơi (hoặc một dân làng tinh <span style="color:#1f2937">nhuệ) cảm hóa được một cá thể Sói chịu đứng về phe Dân Làng (thông qua chuỗi nhiệm vụ cứu mạng</span>

<span style="color:#1f2937">hoặc Huyết Thệ), cả hai có thể Hợp Thể! Dạng Hợp Thể cho phép giữ nguyên lý trí, Mana, phép thuật và</span> <span style="color:#1f2937">kỹ năng vũ khí của con người, đồng thời cộng hưởng lượng Máu/Thể Lực dồi dào và giác quan săn đêm</span> <span style="color:#1f2937">của loài Sói.</span>

- **Phe Tổ Chức Săn Sói (Wolf Hunter Syndicate):** Một lực lượng quân sự lạnh lùng trực thuộc 'Liên Hiệp <span style="color:#1f2937">Làng'. Bề ngoài họ giúp các làng tiêu diệt Người Sói, nhưng mục đích tối hậu của họ là bắt sống và thu</span> <span style="color:#1f2937">thập tinh huyết của đủ 9 Bậc Sói để kích hoạt đại trận Triệu Hồi Sói Cấp 10 làm vũ khí thống trị toàn cõi.</span>

## <span style="color:#7f1d1d">4.4. Tộc Người Lùn (3 Cấp) & Phù Thủy (Tốt / Ác)</span>

<table>
<thead>
<tr>
<th>Nhóm NPC</th>
<th>Phân Cấp / Nhánh</th>
<th>Vị Trí Xuất Hiện</th>
<th>Vai Trò &#x26; Cơ Chế Tương Tác</th>
</tr>
</thead>
<tbody>
<tr>
<td><b>Tộc Người Lùn</b><br /><b>(Phe Trung Lập)</b></td>
<td>1. Người Lùn Thường</td>
<td>Bìa rừng, gần Làng Chính (Biome 1)</td>
<td>Rèn công cụ khai thác và vũ khí phẩm chất Thường cho cả Dân Làng lẫn Sói Đặc Biệt.</td>
</tr>
<tr>
<td><b>Tộc Người Lùn</b><br /><b>(Phe Trung Lập)</b></td>
<td>2. Người Lùn Hiếm</td>
<td>Sâu trong Rừng &#x26; Hang Động (Biome 4)</td>
<td>Rèn vũ khí Hiếm (Rare/Epic) và mở khóa tính năng Khảm 5 Đá Nguyên Tố vào trang bị.</td>
</tr>
<tr>
<td><b>Tộc Người Lùn</b><br /><b>(Phe Trung Lập)</b></td>
<td>3. Vua Người Lùn<br />(Duy nhất 1 người)</td>
<td>Tầng sâu nhất Rừng / Đỉnh Tuyết (Biome 5)</td>
<td>Cực kỳ khó tìm. Chế tác Vũ Khí Huyền Thoại (Legendary) có kỹ năng độc quyền.</td>
</tr>
<tr>
<td><b>Phù Thủy</b></td>
<td>Phù Thủy Tốt<br />(Thảo Y Tiên)</td>
<td>Nhà gỗ ẩn sâu trong Rừng Rậm</td>
<td>Sống một mình; chế thuốc hồi phục/giải độc và dạy nghề Pháp Sư cho Dân Làng.</td>
</tr>
<tr>
<td><b>Phù Thủy</b></td>
<td>Phù Thủy Ác<br />(Cổ Độc Sư)</td>
<td>Đầm Lầy &#x26; Rừng Tối (Biome 2)</td>
<td>Chủ động đi săn Dân Làng, Người Sói, Người Lùn và Người Chơi để nấu thuốc độc &#x26; luyện tà thuật.</td>
</tr>
</tbody>
</table>

## <span style="color:#1b4332">5. ĐỀ XUẤT CƠ CHẾ MỚI MẺ & TỐI ƯU HÓA GAMEPLAY (GAME DESIGN UPGRADES)</span>

<span style="color:#1f2937">Để biến ý tưởng gốc của bạn thành một tựa game có chiều sâu chiến thuật, giàu tính chơi lại (replayability)</span> <span style="color:#1f2937">và cân bằng giữa các phe, dưới đây là 6 hệ thống nâng cấp đắt giá nhất nên đưa vào game:</span>

## <span style="color:#7f1d1d">5.1. Cơ chế 'Kính Chiếu Yêu / Khói Ngải Cứu' (Nhận diện Người Sói giả dạng)</span>

- **Vấn đề trong thiết kế gốc:** Người Sói giả làm Người Du Mục buổi sáng rất hiền lành và không thể nhận ra, nếu người chơi hoàn toàn không có cách nào kiểm tra thì sẽ bị thụ động hoặc chém nhầm Người Du Mục thật.
- **Giải pháp tối ưu:** Thêm các phương pháp 'Soi Bản Thể' đòi hỏi kỹ năng quan sát: (1) Nuôi Voọc Ngũ Sắc hoặc Chó Phú Quốc ở cổng làng – chúng sẽ gầm gừ nhẹ khi thương nhân có mùi máu đi qua; (2) Mua 'Bùa Ngải Cứu' từ Phù Thủy Tốt đốt ở trạm giao dịch (Người Sói giả dạng sẽ bị ho hoặc đổ mồ hôi đỏ); (3) Quan sát hàng hóa: Người Du Mục giả thường bán nhiều thịt tươi không rõ nguồn gốc hoặc quần áo rách của dân làng!

## <span style="color:#7f1d1d">5.2. Hệ thống 'Chu Kỳ Trăng & Đêm Huyết Nguyệt' (Moon Phase System)</span>

- **Chu kỳ 7 ngày:** Chia thời gian thành Trăng Khuyết (Sói yếu hơn 10%), Trăng Bán Nguyệt (Bình thường), Trăng Tròn (Sói tăng 25% tốc độ), và mỗi 14 ngày là một đêm 'Huyết Nguyệt' (Blood Moon) – khi Sói Vua cử đội quân tinh nhuệ tấn công trực diện vào Làng.
- **Chiến thuật Đánh Phủ Đầu (Pre-emptive Strike):** Vì Sói Vua ban đầu rất yếu nhưng mạnh lên mỗi ngày nhờ được bầy sói nuôi, người chơi có thể chọn chiến thuật mạo hiểm: Lập đội Thợ Săn chặn đường các con sói đang tha mồi về hang vào ban đêm để 'cắt nguồn lương thực' của Sói Vua, làm chậm tốc độ tiến hóa của hắn!

## <span style="color:#7f1d1d">5.3. Hệ thống 'Uy Tín Người Lùn' (Dwarven Neutrality & Sabotage)</span>

- **Cạnh tranh rèn vũ khí:** Vì Người Lùn là phe Trung Lập và cũng nhận rèn Kiếm/Cung/Rìu/Búa cho Người Sói Đặc Biệt, người chơi có thể hối lộ Người Lùn bằng Rượu Nếp Làng hoặc Khoáng Sản Quý để Người Lùn tiết lộ vị trí của Người Sói Đặc Biệt vừa đến đặt rèn vũ khí, hoặc nhờ Người Lùn 'ố tình yểm bùa nổ' vào vũ khí của Sói!

## <span style="color:#7f1d1d">5.4. Kiến trúc Làng Việt Cổ & Hệ thống Bẫy Dân Gian</span>

- **Xây dựng đậm chất Việt Nam:** Thay vì tường gỗ châu Âu thông thường, làng sử dụng Luỹ Tre Làng (cực kỳ dẻo dai, tự mọc lại và cản bước nhảy của Sói Bậc 3), Hào Nước Cắm Chông Tre, Nhà Sàn chống thú dữ và Đàn Tế Thần Nông.
- **Hệ thống Bẫy:** Bẫy Hầm Chông, Bẫy Thòng Lọng treo ngược lên cây, Bẫy Khói Ớt/Bồ Kết làm mù mắt Sói dựa trên cơ chế đánh hơi của chúng.

## <span style="color:#7f1d1d">5.5. Hệ thống 4 Kết Cục (4 Multiple Endings) dựa trên lựa chọn của Người Chơi</span>

<table>
<thead>
<tr><th>Tên Kết Cục</th><th>Con Đường Lựa Chọn</th><th>Điều Kiện Đạt Được</th><th>Diễn Biến Cuối Game</th></tr>
</thead>
<tbody>
<tr><td><b>Ending 1:<br />Bình Minh Đại Ngàn<br />(True/Good Ending)</b></td><td>Phe Dân Làng /<br />Hợp Thể Sói Thiện</td><td>Rèn Vũ Khí Huyền Thoại từ<br />Vua Người Lùn, bảo vệ<br />Làng và tiêu diệt Sói Vua</td><td>Lời nguyền Huyết Nguyệt<br />tan biến, Làng trở thành</td></tr>
</tbody>
</table>

<table>
<tr><td></td><td></td><td>trước khi hắn đạt đỉnh sức mạnh.</td><td>kinh đô phồn vinh giữa rừng già.</td></tr>
<tr><td><b>Ending 2:<br />Đế Chế Huyết Nguyệt<br />(Werewolf Ending)</b></td><td>Đứng về Phe Sói</td><td>Phản bội làng, tiến hóa qua 9 Bậc Sói, nuôi Sói Vua (hoặc thách đấu lật đổ Sói Vua) và hủy diệt Liên Hiệp Làng.</td><td>Người chơi trở thành Tân Sói Vương thống lĩnh muôn thú, màn đêm bao phủ vĩnh viễn.</td></tr>
<tr><td><b>Ending 3:<br />Cơn Thịnh Nộ Thần Thú<br />(Hunter Syndicate Ending)</b></td><td>Gia nhập Phe Săn Sói</td><td>Săn đủ 9 Bậc Sói giao nộp cho Tổ Chức Săn Sói để triệu hồi Sói Cấp 10.</td><td>Sói Cấp 10 thức tỉnh nhưng vượt khỏi sự kiểm soát của Liên Hiệp; người chơi phải tử chiến với Thần Sói Cấp 10.</td></tr>
<tr><td><b>Ending 4:<br />Hoàng Đế Cổ Độc<br />(Secret Dark Ending)</b></td><td>Theo Phù Thủy Ác</td><td>Học tà thuật từ Phù Thủy Ác, thâu tóm cả 5 Đá Nguyên Tố và độc chiếm máu của Sói Vua lẫn Vua Người Lùn.</td><td>Người chơi trở thành Đại Pháp Sư Hắc Ám, biến cả Dân Làng và Bầy Sói thành quân đoàn rối sống.</td></tr>
</table>

## <span style="color:#1b4332">6. HỆ THỐNG NỀN VĂN MINH, ĐÀO ĐÁ QUÝ & PHÒNG THỦ (MINECRAFT x CLASH OF CLANS)</span>

### <span style="color:#7f1d1d">6.1. Hệ thống Khai Khoáng & Đào Đá Quý (Minecraft-style Mining)</span>

<span style="color:#1f2937">Để đối phó với bầy Người Sói ngày càng thông minh, biết phá rào gỗ và nhảy qua bẫy, Dân Làng không thể</span> <span style="color:#1f2937">chỉ dùng gỗ tre sơ sài. Người chơi có thể đào sâu xuống lòng đất hoặc tiến vào các mạch mỏ ở 5 Biomes để</span> <span style="color:#1f2937">thu thập Khoáng Sản và Đá Quý phục vụ nâng cấp Nền Văn Minh:</span>

<table>
<thead>
<tr><th>Tầng Địa Chất</th><th>Khoáng Sản &#x26; Đá Quý</th><th>Công Cụ Cần Thiết</th><th>Công Dụng Trong Nâng Cấp Văn Minh</th></tr>
</thead>
<tbody>
<tr><td><b>Tầng 1: Bề Mặt &#x26; Hầm Nông</b></td><td>Đá Cuội, Đất Sét, Than Đá, Quặng Đồng</td><td>Cuốc Gỗ / Cuốc Đá</td><td>Nâng cấp Làng Cấp 2, đúc gạch nung và làm nền móng chống đào hầm.</td></tr>
<tr><td><b>Tầng 2: Mạch Đá Sâu</b></td><td>Quặng Sắt, Bạc Tinh Khiết, Ngọc Lục Bảo (Emerald)</td><td>Cuốc Sắt (Người Lùn Thường)</td><td>Nâng cấp Tường Cấp 3, chế tạo Bẫy Kẹp Bạc (khắc chế Sói tái tạo máu) &#x26; giao dịch.</td></tr>
</tbody>
</table>

<table>
<tr><td><b>Tầng 3: Mạch Tinh Thể</b></td><td>Hồng Ngọc (Ruby), Lam Ngọc (Sapphire), Thạch Anh Tím</td><td>Cuốc Tinh Luyện</td><td>Khảm vào Tháp Canh bắn tia lửa/băng và nâng cấp Tường Cấp 4.</td></tr>
<tr><td><b>Tầng 4: Đáy Vực &#x26; Hang Ngầm</b></td><td>Hắc Diện Thạch (Obsidian), Kim Cương Cổ Đại, Huyền Thiết</td><td>Cuốc Huyền Thiết (Người Lùn Hiếm)</td><td>Xây Tường Cấp 5 bất khả xâm phạm, rèn Giáp Trọng Kỵ và nâng cấp Kinh Đô.</td></tr>
</table>

## <span style="color:#7f1d1d">6.2. Tiến trình 5 Cấp Nền Văn Minh & Tường Thành (Clash of Clans-style Base Upgrade)</span>

<span style="color:#1f2937">Làng có công trình trung tâm là 'Đình Làng (Town Hall)'. Khi nâng cấp Đình Làng bằng Đá Quý, toàn bộ hệ</span> <span style="color:#1f2937">thống Tường, Bẫy và Tháp Phòng Thủ sẽ mở khóa cấp độ mới với cơ chế khắc chế trực tiếp kỹ năng phá</span> <span style="color:#1f2937">rào/nhảy vượt tường của Sói:</span>

<table>
<thead>
<tr><th>Cấp Văn Minh</th><th>Cấp Tường Thành</th><th>Nguyên Liệu Nâng Cấp</th><th>Độ Bền &#x26; Cơ Chế Khắc Chế Sói</th></tr>
</thead>
<tbody>
<tr><td><b>Cấp 1:</b><br /><b>Trại Khai Hoang</b></td><td>Tường Gỗ Sơ Khởi<br />(Level 1-2)</td><td>Gỗ Rừng + Dây Mây</td><td>Máu thấp (500 HP). Dễ bị Sói Bậc 3 cào nát hoặc nhảy vọt qua.</td></tr>
<tr><td><b>Cấp 2:</b><br /><b>Làng Định Cư</b></td><td>Lũy Tre Gai Kép &#x26; Hào Chông<br />(Level 3-4)</td><td>Tre Già + Đá Cuội + Chông Sắt</td><td>Máu khá (1.500 HP). Đỉnh tường vát nhọn làm Sói nhảy qua bị ghim chân và rơi xuống hào.</td></tr>
<tr><td><b>Cấp 3:</b><br /><b>Thành Trì Đá Xanh</b></td><td>Tường Đá Khảm Thạch Anh<br />(Level 5-6)</td><td>Đá Xanh + Sắt + Thạch Anh Tím</td><td>Máu cao (4.000 HP). Gờ tường uốn ngược chống leo trèo; phản 15% sát thương cận chiến.</td></tr>
<tr><td><b>Cấp 4:</b><br /><b>Pháo Đài Hắc Diện</b></td><td>Tường Hắc Diện Lõi Bạc<br />(Level 7-8)</td><td>Obsidian + Quặng Bạc + Ruby</td><td>Máu rất cao (10.000 HP). Tích hợp Lưới Bạc Tự Bắn trên đỉnh tường kéo Sói đang bay xuống đất.</td></tr>
<tr><td><b>Cấp 5:</b><br /><b>Kinh Đô Linh Thạch</b></td><td>Tường Kim Cương Ngũ Hành<br />(Level 9-10)</td><td>Kim Cương + Huyền Thiết + Bụi Linh Thạch</td><td>Máu tối đa (25.000 HP). Tạo kết giới năng lượng trên không chặn đứng mọi pha nhảy &#x26; đốt máu Sói.</td></tr>
</tbody>
</table>

- **Hệ thống Tháp & Bẫy Cải Tiến (Anti-Jump & Anti-Siege):** Bao gồm Tháp Nỏ Liên Châu Khảm Ruby <span style="color:#1f2937">(bắn tên lửa), Tháp Từ Trường Thạch Anh (làm chậm bầy Sói 50%), Giàn Ná Mây Bắn Lưới Bạc (chuyên</span> <span style="color:#1f2937">trị Sói nhảy qua tường), và Bẫy Đá Ngầm Đè Nát (chỉ kích hoạt khi Sói Bậc Cao giẫm lên, không thể bị</span> <span style="color:#1f2937">Sói Bậc 8 vô hiệu hóa).</span>

## <span style="color:#1b4332">7. HỆ THỐNG THÔNG THẠO NGHỀ NGHIỆP (ALBION MASTERY) & KINH TẾ ĐA CHIỀU</span>

### <span style="color:#7f1d1d">7.1. Thanh Kỹ Năng Nhánh Nghề 'Làm Gì Giỏi Nấy' (Destiny Mastery Board)</span>

<span style="color:#1f2937">Lấy cảm hứng từ Albion Online, nhân vật không bị bó buộc vào một khuôn mẫu cố định. Mọi hành động lặp</span> <span style="color:#1f2937">đi lặp lại đều tích lũy Điểm Thông Thạo (Mastery Fame) cho chính nhánh đó, thăng cấp từ Bậc Tập Sự (Tier</span> <span style="color:#1f2937">1) -> Thuần Thục (Tier 4) -> Chuyên Gia (Tier 6) -> Bậc Thầy / Master (Tier 8) -> Đại Tông Sư / Grandmaster</span> <span style="color:#1f2937">(Tier 10):</span>

<table>
<thead>
<tr><th>Nhánh Nghề (Mastery Branch)</th><th>Hành Động Tích Lũy EXP</th><th>Đặc Quyền Khi Đạt Cấp MASTER / GRANDMASTER</th></tr>
</thead>
<tbody>
<tr><td><b>1. Nhánh Khai Khoáng &#x26; Tiều Phu (Resource Gatherer)</b></td><td>Đào quặng, tìm đá quý, chặt cây cổ thụ ở 5 Biomes.</td><td>• Mở khóa 'Nhãn Quan Địa Chất' (nhìn thấy mạch Đá Quý phát sáng xuyên tường đá).<br />• Tỷ lệ 35% nhận gấp đôi Đá Quý và không làm hao bền Cuốc/Rìu.</td></tr>
<tr><td><b>2. Nhánh Nông Gia &#x26; Thuần Thú (Farming &#x26; Beastmaster)</b></td><td>Trồng lúa, chăm thảo dược, nuôi và thuần hóa động vật rừng.</td><td>• Thuần hóa được cả thú dữ cấp cao (Hổ Đông Dương, Báo Hoa Mai, Gấu Ngựa).<br />• Chỉ huy đàn thú chiến đấu bảo vệ làng vào ban đêm.</td></tr>
<tr><td><b>3. Nhánh Kiến Trúc &#x26; Rèn Đúc (Artisan &#x26; Architect)</b></td><td>Xây tường thành, chế bẫy, tự rèn giáp hoặc phụ việc cho Người Lùn.</td><td>• Giảm 30% lượng Đá Quý cần để nâng cấp Tường Thành.<br />• Tự mở khóa khả năng sửa chữa Vũ Khí Hiếm và chế tạo Bẫy Tinh Thể.</td></tr>
<tr><td><b>4. Nhánh Binh Khí Chuyên Sâu (Weapon Specialist)</b></td><td>Sử dụng Kiếm, Cung, Rìu, Búa hoặc Trượng Phép trong thực chiến.</td><td>• Mỗi loại vũ khí có cây kỹ năng riêng (VD: Master Cung bắn tên xuyên 5 mục tiêu; Master Búa đập nứt đất làm Choáng cả bầy Sói).</td></tr>
<tr><td><b>5. Nhánh Huyết Thống &#x26; Giả Kim (Mystic &#x26; Bloodline)</b></td><td>Chế thuốc cùng Phù Thủy, chiến đấu ở dạng Hợp Thể hoặc Hóa Sói.</td><td>• Kéo dài thời gian Hợp Thể Người - Sói gấp đôi, không bị mất kiểm soát lý trí vào đêm Huyết Nguyệt.</td></tr>
</tbody>
</table>

## <span style="color:#7f1d1d">**7.2. Hệ thống Giao Thương & Kinh Tế Đa Dạng (Dynamic Economy)**</span>


- **Chênh lệch Giá Vùng Miền (Biome Arbitrage):** Mỗi Biome có nguồn cung - cầu khác nhau. Ví dụ: Lúa gạo, Gỗ lim và Trứng gà ở Rừng Rậm rất dồi dào nhưng khi mang sang Trại Du Mục ở Sa Mạc hoặc Lò Rèn Người Lùn ở Hang Động sẽ đổi được Đá Quý và Quặng Bạc với giá lời gấp 3 - 5 lần.


- **3 Hình thức Giao dịch Độc đáo:** (1) Hàng đổi Hàng (Barter) với Dân Làng và Người Lùn dựa trên Độ Thân Thiết; (2) Giao dịch bằng Đá Quý & Đồng Cổ tại các Thương Đoàn; (3) Đấu Giá Chợ Đen Ban Đêm (nơi trao đổi Nanh Sói Vua, Cổ Độc của Phù Thủy Ác và Bản Đồ Kho Báu).


- **Hệ thống Mặc Cả & Bẫy Hàng Giả của Sói:** Khi giao dịch với Người Du Mục buổi sáng, giao diện sẽ hiển thị chi tiết nguồn gốc món hàng. Người Sói giả dạng thường rao bán 'Đá Quý Nhuộm Máu' hoặc 'Thịt Sấy Bí Ẩn' với giá rẻ bất ngờ — nếu người chơi tham rẻ mua về cất vào kho làng, món hàng đó sẽ phát tán mùi hương dẫn dụ bầy Sói tập kích đúng vị trí kho vào ban đêm!


## <span style="color:#1b4332">**8. BỔ SUNG 3 VAI TRÒ NPC MỚI (CÓ PHÂN CẤP ĐỘ) CHO 3 PHE**</span>


Để thế giới game thêm sống động và cân bằng với sự tiến hóa của 9 Bậc Sói và 3 Cấp Người Lùn, dưới đây là 3 lớp NPC hoàn toàn mới đại diện cho Phe Làng, Phe Trung Lập và Phe Ác, mỗi vai trò đều có hệ thống cấp bậc riêng:


### <span style="color:#7f1d1d">**8.1. [Phe Dân Làng] Công Trình Sư / Cơ Khí Sư (5 Cấp Độ) - Khắc tinh Công Thành**</span>


Nếu Kiếm Sĩ, Thợ Săn, Pháp Sư là lực lượng đánh trực diện thì Công Trình Sư là nhánh NPC/Nghề chuyên biệt bảo vệ nền văn minh của Làng trước khả năng phá tường và nhảy rào của Sói:

<table>
<thead>
<tr><th>Cấp Độ</th><th>Danh Hiệu NPC</th><th>Khả Năng &#x26; Công Trình Mở Khóa</th></tr>
</thead>
<tbody>
<tr><td><b>Cấp 1</b></td><td>Thợ Mộc Làng</td><td>Tự động đi thu gom gỗ và sửa chữa hàng rào, nhà cửa bị hư hại sau mỗi đêm.</td></tr>
<tr><td><b>Cấp 2</b></td><td>Thợ Đá Thành Trì</td><td>Xây dựng Tường Đá, Hào Nước Chông Tre và gia cố nền móng chống đào hầm.</td></tr>
<tr><td><b>Cấp 3</b></td><td>Thợ Thủ Công Cơ Quan Mộc</td><td>Chế tạo Tháp Nỏ Liên Châu tự động, Máy Bắn Lưới Bạc và Bẫy Dây Mây Tơ Nhện Cổ Thụ.</td></tr>
<tr><td><b>Cấp 4</b></td><td>Bậc Thầy Tinh Thể</td><td>Biết dùng Đá Quý đào được khảm vào Tường và Tháp Canh để tạo tháp bắn tia nguyên tố.</td></tr>
</tbody>
</table>

<table>
<tr><td>Cấp 5</td><td>Đại Kiến Trúc Sư<br />(Duy nhất 1 người)</td><td>Kích hoạt 'Đại Trận Hộ Thành' (tăng 100% Giáp toàn bộ tường làng trong đêm Huyết Nguyệt) và chế tạo Chiến Xa Mộc Ngưu Đồng Đen.</td></tr>
</table>

## <span style="color:#7f1d1d">**8.2. [Phe Trung Lập] Thương Hội Kỵ Binh Giáp Thú (3 Cấp Độ) - Giao Thương Xuyên Biome**</span>

<span style="color:#1f2937">Khác với những Người Du Mục đi bộ lẻ tẻ dễ bị Sói ăn thịt và giả dạng, Thương Hội Kỵ Binh là tổ chức buôn</span> <span style="color:#1f2937">bán hạng nặng cưỡi thú bọc thép di chuyển qua cả 5 Biomes:</span>

<table>
<thead>
<tr><th>Cấp Độ</th><th>Đoàn Thương Hội</th><th>Đặc Điểm &#x26; Dịch Vụ Cung Cấp</th></tr>
</thead>
<tbody>
<tr><td><b>Cấp 1</b></td><td>Thương Buôn Cưỡi Trâu Thiết Giáp</td><td>Đi lại giữa Rừng Rậm và Đầm Lầy. Bán sỉ Gỗ, Đá, Hạt giống quý và mua lại Da Sói, Thịt thú.</td></tr>
<tr><td><b>Cấp 2</b></td><td>Thương Đoàn Tê Giác / Voi Chiến</td><td>Đi xuyên Sa Mạc và cửa Hang Động. Không thể bị Sói giả dạng; trao đổi Đá Quý, Bản Vẽ Tường Cấp Cao và cho thuê Lính Đánh Thuê giữ làng.</td></tr>
<tr><td><b>Cấp 3</b></td><td>Đặc Sứ Thương Hội Hoàng Gia (Cực hiếm)</td><td>Chỉ xuất hiện vào ngày Trăng Khuyết. Mở phiên 'Đấu Giá Huyền Thoại' bán Đá Nguyên Tố, Trang bị Cổ Đại và manh mối vị trí Vua Người Lùn.</td></tr>
</tbody>
</table>

## <span style="color:#7f1d1d">**8.3. [Phe Ác Mới] Tộc Huyết Thi / Dạ Quỷ Cổ Mộ (5 Cấp Độ) - Hiểm Họa Dưới Lòng Đất**</span>

<span style="color:#1f2937">Khi người chơi và dân làng đào sâu xuống lòng đất để tìm Đá Quý nâng cấp tường thành, họ đã vô tình</span> <span style="color:#1f2937">đánh thức 'Tộc Huyết Thi' — những kẻ đào mộ cổ đại bị nhiễm tà khí của Phù Thủy Ác và máu Thần Sói.</span> <span style="color:#1f2937">Đây là phe ác thứ hai hoạt động dưới lòng đất và phối hợp với Người Sói:</span>

<table>
<thead>
<tr><th>Cấp Độ</th><th>Tên Quái / NPC Ác</th><th>Hành Vi &#x26; Sự Đe Dọa Đối Với Làng</th></tr>
</thead>
<tbody>
<tr><td><b>Cấp 1</b></td><td>Mộ Tặc Tha Hóa</td><td>Ẩn nấp trong các hầm mỏ nông; chuyên đánh lén và ăn cắp Đá Quý</td></tr>
</tbody>
</table>

<table>
<tr><td>Cấp 2</td><td>Độn Thổ Thi<br />(Thợ Đào Ngầm)</td><td>Ban đêm đào đường hầm xuyên qua bên dưới Tường Gỗ/Tường Đá cấp thấp để chui thẳng vào giữa làng phá kho lương thực!</td></tr>
<tr><td>Cấp 3</td><td>Thiết Giáp Thi Vương</td><td>Canh giữ các mỏ Kim Cương và Hồng Ngọc ở tầng sâu; lớp vỏ đá hóa cực cứng, chỉ nhận sát thương từ Búa hoặc Đá Đất/Đá Lửa.</td></tr>
<tr><td>Cấp 4</td><td>Huyết Pháp Thi<br />(Đồng minh Phù Thủy Ác)</td><td>Đi theo Phù Thủy Ác vào ban đêm, dùng tà thuật hồi sinh xác những con Sói vừa bị dân làng bắn hạ thành 'Sói Khô Xương'.</td></tr>
<tr><td>Cấp 5</td><td>Cổ Mộ Chúa Tể<br />(Boss Ngầm)</td><td>Thống lĩnh tầng sâu nhất của Hang Động; nắm giữ 'Lõi Địa Tâm' (vật phẩm bắt buộc để nâng cấp Tường Thành lên Cấp 5).</td></tr>
</table>

## **<span style="color:#1b4332">9. HỆ THỐNG VẬT PHẨM ĐA DẠNG: THUẦN FANTASY & VĂN HÓA CỔ TRUYỀN VIỆT NAM</span>**

<span style="color:#1f2937">Toàn bộ hệ thống vật phẩm trong game tuân thủ nguyên tắc 'Zero Modern Tech' (Tuyệt đối không có súng</span> <span style="color:#1f2937">đạn, thuốc nổ công nghiệp hay máy móc hiện đại). Mọi vũ khí tầm xa, công cụ phòng thủ và trang bị đều</span> <span style="color:#1f2937">bắt nguồn từ kỹ nghệ đúc đồng Đông Sơn, cơ quan tre trúc dân gian, thảo dược Nam Y và truyền thuyết</span> <span style="color:#1f2937">thần thoại Việt Nam.</span>

### **<span style="color:#7f1d1d">9.1. Hệ thống Vũ Khí Cổ Truyền theo 3 Cấp Người Lùn (5 Nhánh Binh Khí)</span>**

<span style="color:#1f2937">Cả Dân Làng (Kiếm Sĩ, Thợ Săn, Pháp Sư) lẫn Người Sói Đặc Biệt đều tìm đến Tộc Người Lùn để rèn vũ khí.</span> <span style="color:#1f2937">Dưới đây là lộ trình tiến hóa vũ khí mang đậm hồn Việt qua 3 bậc thợ rèn:</span>

<table>
<tr>
<td>Nhánh Vũ Khí</td>
<td>Bậc 1: Người Lùn Thường (Phẩm Chất Thường)</td>
<td>Bậc 2: Người Lùn Hiếm (Phẩm Chất Hiếm - Khảm Linh Thạch)</td>
<td>Bậc 3: Vua Người Lùn (Vũ Khí Huyền Thoại - Độc Nhất)</td>
</tr>
</table>

<table>
<tr><td><b>1. Nhánh Kiếm / Đao (Kiếm Sĩ &#x26; Sói Đặc Biệt)</b></td><td>• Trường Kiếm Sắt Ấp Làng<br />• Mã Tấu Đi Rừng (Chặt cây &#x26; chém gần)</td><td>• Cổ Kiếm Đồng Đen Đông Sơn<br />• Đoản Đao Nanh Hổ (Khảm Đá Lửa/Băng gây xuất huyết)</td><td>• Thuận Thiên Thần Kiếm: Chém ra kiếm khí ánh sáng khắc chế Ma Sói, +50% sát thương lên Sói Vua.</td></tr>
<tr><td><b>2. Nhánh Cung / Nỏ (Thợ Săn - Thay thế súng)</b></td><td>• Cung Tre Cánh Trâu<br />• Nỏ Gỗ Lẫy Tre (Bắn tên tre vót nhọn)</td><td>• Cung Sừng Sao La Dây Tơ Nhện<br />• Nỏ Liên Châu Cánh Đồng (Bắn 3 mũi tên liên tiếp)</td><td>• Nỏ Thần Kim Quy (Linh Quang Thần Cơ): Rèn từ vuốt Rùa Vàng, bắn 1 phát hóa thành mưa tên đồng khảm Ngũ Hành.</td></tr>
<tr><td><b>3. Nhánh Rìu Chiến (Tiều Phu &#x26; Cuồng Chiến)</b></td><td>• Rìu Sắt Tiều Phu<br />• Rìu Đá Xanh (Đốn gỗ &#x26; bổ giáp nhẹ)</td><td>• Rìu Chiến Lưỡi Xéo Hoa Văn Chim Lạc<br />• Song Phủ Huyền Thiết (Khảm Đá Thực Vật hút máu)</td><td>• Khai Sơn Thần Phủ: Bổ đôi tường đá và giáp của Thiết Giáp Thi Vương; chém tạo sóng chấn động.</td></tr>
<tr><td><b>4. Nhánh Búa / Chùy (Phá Giáp &#x26; Đập Đá)</b></td><td>• Chùy Gỗ Lim Bọc Đồng<br />• Búa Đập Đá Khai Mỏ</td><td>• Chùy Gai Đồng Đỏ<br />• Búa Chiến Ngàn Cân (Khảm Đá Đất gây Choáng diện rộng)</td><td>• Lôi Công Thần Chùy (Gậy Sắt Thánh Gióng): Đập xuống đất gọi sấm sét đánh văng cả bầy Sói đang nhảy.</td></tr>
<tr><td><b>5. Nhánh Pháp Khí (Chỉ dành cho Pháp Sư)</b></td><td>• Trượng Gỗ Đa Cổ Thụ<br />• Phất Trần Lông Gà Rừng</td><td>• Trượng Đầu Rồng Khảm Ngọc<br />• Hồ Lô Linh Thủy (Tích trữ Mana &#x26; phun sương độc/thuốc)</td><td>• Trống Đồng Ngọc Lũ Thu Nhỏ: Pháp khí tối thượng gọi hồn binh Văn Lang và tạo kết giới Ngũ Hành.</td></tr>
</table>


**<span style="color:#7f1d1d"># **9.2. Hệ thống Giáp Trụ & Trang Phục Bản Địa Việt Nam**</span>**

<table>
<thead>
<tr><th>Tên Trang Bị</th><th>Nguyên Liệu Chế Tác</th><th>Chỉ Số &#x26; Hiệu Ứng Đặc Trưng Theo Môi Trường</th></tr>
</thead>
<tbody>
<tr><td><b>Áo Tơi Lá Cọ &#x26; Nón Lá</b></td><td>Lá Cọ Rừng + Dây Mây</td><td>Chống ướt khi mưa rừng nhiệt đới (không bị giảm Thể Lực); đứng yên trong bụi cỏ sẽ tàng hình với Sói Bậc 1-2.</td></tr>
<tr><td><b>Giáp Mây Đan Sơn Ta</b></td><td>Mây Già + Nhựa Cây Sơn + Da Lợn Rừng</td><td>Giáp hạng nhẹ cho Thợ Săn: Tăng 20% tốc độ chạy, miễn nhiễm gai</td></tr>
</tbody>
</table>

<table>
<tr><td></td><td></td><td>độc Đầm Lầy và giảm 50% tiếng bước chân.</td></tr>
<tr><td><b>Giáp Vảy Đồng Văn Lang</b></td><td>Quặng Đồng + Sắt + Da Trâu Nước</td><td>Giáp hạng nặng cho Kiếm Sĩ: Giảm 35% sát thương từ móng vuốt Sói, chống hiệu ứng Chảy Máu từ Hổ Đông Dương.</td></tr>
<tr><td><b>Áo Bào Lông Hổ / Gấu Ngựa</b></td><td>Da Hổ Đông Dương + Lông Gấu Ngựa</td><td>Trang bị bắt buộc khi lên Núi Tuyết Hoàng Liên: Miễn nhiễm Hàn Khí đóng băng và tăng 25% Sát Thương Chí Mạng.</td></tr>
<tr><td><b>Áo Giao Lĩnh Ngũ Hành</b></td><td>Lụa Tơ Tằm + Bụi Đá Nguyên Tố</td><td>Trang phục Pháp Sư: Tăng 50% tốc độ hồi Mana và giảm 20% thời gian hồi chiêu thức nguyên tố.</td></tr>
<tr><td><b>Huyết Giáp Da Sói Vua</b></td><td>Da Sói Bậc Cao + Quặng Bạc + Huyền Thiết</td><td>Trang bị cuối game: Ban đêm phát sáng hào quang uy áp làm Sói Bậc 1-4 hoảng sợ không dám lại gần.</td></tr>
</table>

## <span style="color:#7f1d1d">**9.3. Hệ thống Ẩm Thực Dân Gian, Dược Liệu Nam Y & Độc Dược**</span>

<span style="color:#1f2937">Thay vì các bình máu/mana chung chung, vật phẩm tiêu hao được thiết kế thành các món ăn truyền thống</span> <span style="color:#1f2937">và bài thuốc dân gian Việt Nam, gắn liền với Thanh Thức Ăn, Thể Lực và cơ chế phát hiện Sói:</span>

<table>
<thead>
<tr><th>Nhóm Vật Phẩm</th><th>Tên Vật Phẩm Cụ Thể</th><th>Nguyên Liệu / Nguồn Gốc</th><th>Công Dụng Sinh Tồn &#x26; Chiến Đấu</th></tr>
</thead>
<tbody>
<tr><td><b>Ẩm Thực Làng Việt</b><br /><b>(Nuôi Thanh Thức Ăn)</b></td><td>1. Cơm Lam Ống Nứa<br />2. Bánh Chưng / Bánh Tét Đất Tổ<br />3. Thịt Trâu Gác Bếp<br />4. Nước Chè Xanh / Nước Mía</td><td>Lúa nếp trồng tại làng, ống tre, thịt thú rừng, lá dong.</td><td>• Cơm Lam: Nhẹ, lâu hỏng khi đi thám hiểm hang động.<br />• Bánh Chưng: Hồi 100% Thức Ăn, tăng 20% Máu tối đa trong 2 ngày.<br />• Nước Chè/Nước Mía: Chống mất nước x2 khi đi qua Sa Mạc.</td></tr>
<tr><td><b>Rượu Cổ Truyền</b><br /><b>(Buff &#x26; Ngoại Giao)</b></td><td>1. Rượu Cần Đại Ngàn<br />2. Rượu Nếp Cái Hoa Vàng<br />3. Rượu Ngâm Mật Gấu / Nọc Rắn</td><td>Ủ từ gạo nếp, men lá rừng và dược liệu quý.</td><td>• Uống trước khi ra trận: Tăng 25% sát thương cận chiến &#x26; chống lạnh.<br />• Mang tặng Người Lùn: Tăng độ thân thiết để mở</td></tr>
</tbody>
</table>

<table>
<tr><td></td><td></td><td></td><td>khóa rèn đồ Hiếm &#x26; mua tin tình báo về Sói Đặc Biệt.</td></tr>
<tr><td><b>Thảo Dược Nam Y<br />(Phù Thủy Tốt)</b></td><td>1. Cao Lá Rừng / Dầu Tràm Cổ<br />2. Đan Dược Tam Thất Mật Ong<br />3. Bó Đuốc Ngải Cứu &#x26; Bồ Kết</td><td>Hái trong Rừng Rậm hoặc mua từ Phù Thủy Tốt.</td><td>• Dầu Tràm: Bôi lên người giúp miễn nhiễm chướng khí Đầm Lầy.<br />• Đuốc Ngải Cứu &#x26; Bồ Kết: Đốt ở cổng làng làm Người Sói giả dạng Người Du Mục bị cay mắt, ho ra khói đen và lộ nguyên hình!</td></tr>
<tr><td><b>Cổ Độc &#x26; Tà Dược<br />(Phù Thủy Ác)</b></td><td>1. Nhựa Tên Độc Rắn Hổ Chúa<br />2. Bột Hoa Cà Độc Dược<br />3. Huyết Dịch Cuồng Lang</td><td>Chiết xuất từ nọc rắn, nấm độc Đầm Lầy và máu Sói.</td><td>• Tẩm vào tên/kiếm gây rút máu theo giây.<br />• Bột Cà Độc Dược: Ném vào bầy Sói gây ảo giác khiến chúng tự cắn xé lẫn nhau trong 15 giây.</td></tr>
</table>


## <span style="color:#7f1d1d">**9.4. Hệ thống Cơ Quan Tre Trúc & Cổ Vật Tâm Linh Việt Nam (Không Công Nghệ Hiện Đại)**</span>


<span style="color:#1f2937">Để chống lại Sói nhảy qua tường hoặc đào hầm mà không dùng công nghệ hiện đại, dân làng ứng dụng nghệ thuật Cơ Quan Mộc (tre, dây mây, đá tảng đối trọng) và Pháp Khí Phong Thủy:</span>

<table>
<thead>
<tr><th>Phân Loại</th><th>Tên Vật Phẩm / Công Trình</th><th>Cơ Chế Hoạt Động Thuần Cổ Truyền &#x26; Fantasy</th></tr>
</thead>
<tbody>
<tr><td><b>Cơ Quan Phòng Thủ<br />(Tre Trúc &#x26; Đá Quý)</b></td><td>Giàn Ná Mây Bắn Lưới Bạc</td><td>Dùng sức bật của thân tre già uốn cong kết hợp dây tơ nhện cổ thụ; tự động bật tung tấm lưới đan sợi bạc chụp gọn Sói Bậc 3+ khi chúng nhảy qua tường.</td></tr>
<tr><td><b>Cơ Quan Phòng Thủ<br />(Tre Trúc &#x26; Đá Quý)</b></td><td>Cối Đá Sập Ngầm &#x26; Hầm Chông Tre</td><td>Bẫy cơ học dùng lẫy gỗ lim: Khi Sói hạng nặng hoặc Độn Thổ Thi đào hầm đi qua, tảng đá xanh ngàn cân sẽ sập xuống đè nát đường hầm.</td></tr>
<tr><td><b>Cơ Quan Phòng Thủ<br />(Tre Trúc &#x26; Đá Quý)</b></td><td>Lò Xông Khói Ớt Hiểm &#x26; Bồ Kết</td><td>Đặt dọc theo tường làng; dùng quạt gió bằng tre thổi khói cay nồng ra</td></tr>
</tbody>
</table>

<table>
<tr><td></td><td></td><td>ngoài làm Sói bị mù khứu giác, không thể đánh hơi tìm dân làng.</td></tr>
<tr><td><b>Cổ Vật Tâm Linh<br />(Relics / Talisman)</b></td><td>Gương Đồng Chiếu Yêu Đông Sơn</td><td>Vật phẩm cầm tay đúc từ Đồng Đen và Ngọc Lục Bảo: Khi soi vào Người Du Mục buổi sáng, hình ảnh phản chiếu trong gương sẽ hiện rõ đầu Sói nếu đó là Sói giả dạng.</td></tr>
<tr><td><b>Cổ Vật Tâm Linh<br />(Relics / Talisman)</b></td><td>Chuông Gió Đồng Đen Treo Cổng</td><td>Tự động ngân vang âm thanh trầm đục khi trong bán kính 50m có Người Sói Đặc Biệt (đang ẩn hình người) hoặc Huyết Thi dưới lòng đất tiến lại gần.</td></tr>
<tr><td><b>Cổ Vật Tâm Linh<br />(Relics / Talisman)</b></td><td>Trống Đồng Hộ Thành (Đặt ở Đình Làng)</td><td>Khi người chơi hoặc Trưởng Làng dùi trống vào đêm Huyết Nguyệt, tiếng trống vang rền giúp xóa bỏ nỗi sợ từ tiếng hú Sói Bậc 8 và tăng 30% Sức Đánh cho toàn bộ dân làng.</td></tr>
</table>