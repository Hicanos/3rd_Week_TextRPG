using TextRPG.WeaponManagement;

namespace TextRPG.ItemSpawnManagement
{
    public class ItemSpawn
    {
        public static void SettingAllItems()
        {
            BuyAbleWeaponSpawn();
            NotBuyAbleWeaponSpawn();
            RewardItemSpawn();
        }

        public static void BuyAbleWeaponSpawn() // 상점에서 구매 가능한 아이템들
        {
            if (Weapons.Inventory.Count > 0) return; // 중복 생성 방지

            // 무기
            new Weapons(false, false, "최신 스마트폰", new Dictionary<string, int> { { "공격력", 12 }, { "명중율", 15 }, { "치명타 확률", 8 } }, "최신형 스마트폰. 강력한 성능!", "전체", "무기", 2500);
            new Weapons(false, false, "맥 북", new Dictionary<string, int> { { "공격력", 15 }, { "MaxMP", 30 }, { "치명타 배율", 25 } }, "귀엽고 아담한 중고 맥 북", "전체", "무기", 3400);
            new Weapons(false, false, "무선 마우스 키보드", new Dictionary<string, int> { { "공격력", 18 }, { "치명타 배율", 20 }, { "치명타 확률", 10 } }, "무선이라 자유롭다.", "전체", "무기", 4200);
            new Weapons(false, false, "그래픽카드5090", new Dictionary<string, int> { { "공격력", 20 }, { "치명타 배율", 50 }, { "MaxMP", 90 } }, "어 형은 5090오우너야~", "전체", "무기", 5090);
            new Weapons(false, false, "Moo나미볼펜", new Dictionary<string, int> { { "공격력", 2 }, { "치명타 확률", 3 } }, "사무실에 하나씩은 있는 그 볼펜", "전체", "무기", 500);
            new Weapons(false, false, "형광펜", new Dictionary<string, int> { { "공격력", 5 }, { "치명타 배율", 15 } }, "슥슥 잘 그려지는 색상 형광펜", "전체", "무기", 1100);
            new Weapons(false, false, "빗자루&쓰레받이", new Dictionary<string, int> { { "공격력", 7 }, { "MaxMP", 20 }, { "치명타 배율", 20 } }, "왜? 너도 청소해주랴?", "전체", "무기", 1800);
            new Weapons(false, false, "결제 화일", new Dictionary<string, int> { { "공격력", 17 }, { "방어력", -2 }, { "치명타 확률", -5 } }, "인사팀이 결제받을 때 쓰는 잇템.", "인사팀", "무기", 3000);
            new Weapons(false, false, "명함", new Dictionary<string, int> { { "공격력", 12 }, { "MaxMP", 40 }, { "치명타 확률", -8 } }, "홍보팀 표준 명함. 소재가 튼튼해서 날리면 은근 아프다.", "홍보팀", "무기", 3000);
            new Weapons(false, false, "계산기", new Dictionary<string, int> { { "공격력", 16 }, { "MaxMP", -15 }, { "회피율", -10 } }, "인간 계산기 경지에 오르면 무기로 밖에 쓸모없는 아이템.", "총무팀", "무기", 3000);
            new Weapons(false, false, "커터칼", new Dictionary<string, int> { { "공격력", 14 }, { "명중률", -15 }, { "치명타 배율", 10 } }, "주로 박스를 자를 때 쓴다. 가끔은 누구의 명줄을 자를때도....", "영업팀", "무기", 3000);
            new Weapons(false, false, "만년필", new Dictionary<string, int> { { "공격력", 18 }, { "MaxMP", -20 }, { "명중률", -5 } }, "누가 쓰는지는 모르겠지만 있어보이는 아이템.", "전산팀", "무기", 3000);
            new Weapons(false, false, "클립 집게", new Dictionary<string, int> { { "공격력", 14 }, { "명중률", -10 }, { "치명타 확률", 10 } }, "넌 이미 찝혀있다!", "기획팀", "무기", 3000);

            // 방어구
            new Weapons(false, false, "싸구려 정장 상의", new Dictionary<string, int> { { "방어력", 3 }, { "회피율", 4 } }, "저렴한 정장 상의.", "전체", "상의", 900);
            new Weapons(false, false, "통풍셔츠", new Dictionary<string, int> { { "방어력", 6 }, { "MaxHP", 10 }, { "MaxMP", 25 }, { "치명타 확률", 10 } }, "시원하고 방어도 가능한 셔츠.", "전체", "상의", 2400);
            new Weapons(false, false, "명품 정장 상의", new Dictionary<string, int> { { "공격력", 4 }, { "방어력", 10 }, { "치명타 확률", 10 }, { "MaxHP", 25 } }, "명품 브랜드의 최고 인기 상품.", "전체", "상의", 8000);
            new Weapons(false, false, "싸구려 정장 하의", new Dictionary<string, int> { { "방어력", 3 }, { "MaxMP", 15 } }, "저렴한 정장 바지.", "전체", "하의", 800);
            new Weapons(false, false, "반바지", new Dictionary<string, int> { { "방어력", 5 }, { "MaxHP", 10 }, { "명중률", 10 }, { "회피율", 10 } }, "여름에 좋은 반바지.", "전체", "하의", 2200);
            new Weapons(false, false, "명품 정장 하의", new Dictionary<string, int> { { "공격력", 5 }, { "방어력", 7 }, { "치명타 배율", 30 }, { "MaxHP", 20 } }, "명품 브랜드의 역대급 상품 .", "전체", "하의", 8000);
            new Weapons(false, false, "발가락 양말", new Dictionary<string, int> { { "방어력", 2 }, { "회피율", 5 } }, "편안한 양말.", "전체", "신발", 1000);
            new Weapons(false, false, "크록스", new Dictionary<string, int> { { "방어력", 3 }, { "MaxHP", 10 }, { "치명타 배율", 20 }, { "MaxMP", 15 } }, "편안한 크록스.", "전체", "신발", 2000);
            new Weapons(false, false, "냄새나는 구두", new Dictionary<string, int> { { "공격력", 4 }, { "명중률", -10 }, { "치명타 배율", 25 } }, "혹시 자신의 발냄새를 맡는 사람이 있다면 조심해라! 습...하아~", "전체", "신발", 1000);
            new Weapons(false, false, "명품 구두", new Dictionary<string, int> { { "공격력", 5 }, { "방어력", 4 }, { "명중률", 15 }, { "회피율", 15 } }, "명품 브랜드의 티가 날듯 안날듯한 구두.", "전체", "신발", 8000);

            // 악세서리
            new Weapons(false, false, "알없는 안경", new Dictionary<string, int> { { "방어력", 5 }, { "MaxMP", 20 }, { "명중률", -10 } }, "기본적인 안경.", "총무팀", "장신구", 2300);
            new Weapons(false, false, "서류철", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 2 }, { "명중률", 5 } }, "인사팀의 중요한 서류.", "인사팀", "장신구", 1500);
            new Weapons(false, false, "테이프", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 2 }, { "회피율", 3 } }, "홍보팀의 필수 아이템.", "홍보팀", "장신구", 1500);
            new Weapons(false, false, "C타입 충전기", new Dictionary<string, int> { { "공격력", 3 }, { "방어력", 3 }, { "치명타 확률", -5 } }, "총무팀의 중요한 장비.", "총무팀", "장신구", 1500);
            new Weapons(false, false, "USB", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 3 } }, "영업팀의 필수 장비.", "영업팀", "장신구", 1500);
            new Weapons(false, false, "보조배터리", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 2 }, { "MaxMP", 15 } }, "전산팀의 중요한 장비.", "전산팀", "장신구", 1500);
            new Weapons(false, false, "건전지", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 2 }, { "치명타 확률", 3 } }, "기획팀의 필수 아이템.", "기획팀", "장신구", 1500);
            new Weapons(false, false, "명품 시계", new Dictionary<string, int> { { "공격력", 5 }, { "방어력", 5 }, { "MaxMP", 50 }, { "치명타 배율", 30 } }, "명품 브랜드의 희귀 컬렉션 시계.", "전체", "장신구", 8000);
            new Weapons(false, false, "A4용지", new Dictionary<string, int> { { "MaxMP", 15 }, { "치명타 확률", 5 } }, "기본적인 A4용지.", "전체", "장신구", 900);
            new Weapons(false, false, "에어팟", new Dictionary<string, int> { { "MaxMP", 50 }, { "방어력", -3 }, { "회피율", 25 } }, "MZ 사원들의 필수템! 네? 부장님 머라고요?", "전체", "장신구", 1500);
            new Weapons(false, false, "노트북백팩", new Dictionary<string, int> { { "공격력", 4 }, { "방어력", 4 }, { "치명타 확률", 15 } }, "노트북과 같이 구매하기 좋은 세트 상품", "전체", "장신구", 3300);

            // 소모품
            new Weapons(false, false, "반창고", new Dictionary<string, int> { { "HP", 50 } }, "작은 상처는 반창고 하나면 충분!", "전체", "포션", 600);
            new Weapons(false, false, "멸균붕대", new Dictionary<string, int> { { "HP", 50 } }, "사무실마다 하나씩은 있는 그 붕대", "전체", "포션", 1000);
            new Weapons(false, false, "구급 상자", new Dictionary<string, int> { { "HP", 50 } }, "작은 상처는 반창고 하나면 충분!", "전체", "포션", 1500);
            new Weapons(false, false, "바까스", new Dictionary<string, int> { { "MP", 30 } }, "피로회복제 그 이름, 바까스!", "전체", "포션", 600);
            new Weapons(false, false, "아메리카노", new Dictionary<string, int> { { "MP", 100 } }, "직장인들의 힐링포션", "전체", "포션", 1500);
            new Weapons(false, false, "장어구이", new Dictionary<string, int> { { "공격력", 3 } }, "피로회복제 그 이름, 바까스!", "전체", "포션", 1000, true);
            new Weapons(false, false, "흑마늘", new Dictionary<string, int> { { "방어력", 3 } }, "피로회복제 그 이름, 바까스!", "전체", "포션", 1000, true);
            new Weapons(false, false, "삼계탕", new Dictionary<string, int> { { "MaxHP", 20 } }, "피로회복제 그 이름, 바까스!", "전체", "포션", 1000, true);
            new Weapons(false, false, "복분자주", new Dictionary<string, int> { { "MaxMP", 15 } }, "피로회복제 그 이름, 바까스!", "전체", "포션", 1000, true);
            new Weapons(false, false, "블루베리 스무디", new Dictionary<string, int> { { "명중률", 10 } }, "피로회복제 그 이름, 바까스!", "전체", "포션", 1000, true);
            new Weapons(false, false, "연어회", new Dictionary<string, int> { { "회피율", 8 } }, "피로회복제 그 이름, 바까스!", "전체", "포션", 1000, true);
            new Weapons(false, false, "다크 초콜렛", new Dictionary<string, int> { { "치명타 배율", 10 } }, "피로회복제 그 이름, 바까스!", "전체", "포션", 1000, true);
            new Weapons(false, false, "홍삼젤리", new Dictionary<string, int> { { "치명타 확률", 5 } }, "피로회복제 그 이름, 바까스!", "전체", "포션", 1000, true);


        }

        public static void NotBuyAbleWeaponSpawn()
        {
            if (Weapons.NotbuyAbleInventory.Count > 0) return;

            new Weapons(false, false, "텀블러", new Dictionary<string, int> { { "공격력", 10 }, { "치명타 확률", 10 } }, "개인 텀블러는 직장인 필수템!", "전체", "무기", "사장", 15, 1500);
            new Weapons(false, false, "브랜드 정장 상의", new Dictionary<string, int> { { "방어력", 5 }, { "명중률", 10 }, { "치명타", 20 } }, "고급스러움이 풍기는 상의.", "전체", "상의", "전무", 16, 1500);
            new Weapons(false, false, "브랜드 정장 하의", new Dictionary<string, int> { { "방어력", 5 }, { "MaxHP", 15}, { "치명타 확률", 10 } }, "편안함과 스타일을 동시에.", "전체", "하의", "상무", 17, 1500);
            new Weapons(false, false, "브랜드 구두", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 2 }, { "MaxMP", 15 } }, "고급 가죽으로 제작된 구두.", "전체", "신발", "이사", 18, 1500);
            new Weapons(false, false, "방석&등받이 쿠션", new Dictionary<string, int> { { "방어력", 2 }, { "MaxHP", 5 }, { "회피율", 10 } }, "오래 앉아 있어도 괜찮다!", "전체", "장신구", "실장", 19, 1500);
            new Weapons(false, false, "든든한 국밥", new Dictionary<string, int> { { "MaxHP", 10 }, { "MaxMP", 10 } }, "마음까지 든든해진다.", "전체", "포션", "전무, 사장, 회장", 5, 1000);
        }

        public static void RewardItemSpawn()
        {
            if (Weapons.RewardInventory.Count > 0) return;

            new Weapons(false, "대리의 빠때리", "풋풋한 대리의 그녀의 사랑이 담긴 물건", "전리품", "대리", 35, 300);
            new Weapons(false, "과장의 사원증", "평소에 안씻기로 유명한 과장님의 사원증", "전리품", "과장", 35, 600);
            new Weapons(false, "차장의 가발", "차장님의 숨겨진 비밀…어쩐지 측은하다.", "전리품", "차장", 30, 1000);
            new Weapons(false, "부장의 넥타이", "왠지 매일 화가나있는 부장님의 넥타이", "전리품", "부장", 30, 1600);
            new Weapons(false, "직원 평가표", "부하 직원들에게 훈수 두는 꼰대 실장님이 적어둔 직원 평가표", "전리품", "실장", 25, 2400);
            new Weapons(false, "유흥업소 명함", "술고래 이사님의 주머니에서 나온 명함", "전리품", "이사", 25, 3500);
            new Weapons(false, "한정판 굿즈 인형", "개인 사무실 한편에 전시된 여러 굿즈들 중 가장 비싸보이는 인형", "전리품", "상무", 20, 4800);
            new Weapons(false, "노또 용지", "전무님 책장에는 매주 노또 용지가 쌓여있다.", "전리품", "전무", 20, 6000);
            new Weapons(false, "자서전", "부하 직원들에게 사라고 추천하던 책\n나도 반강제적으로 하나 샀었다.", "전리품", "사장", 15, 8000);
            new Weapons(false, "직급 명패", "전 회장의 명패이니 필요 없겠지?", "전리품", "회장", 10, 10000);

        }
    }
}
