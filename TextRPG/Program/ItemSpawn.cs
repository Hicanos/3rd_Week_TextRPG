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
            new Weapons(false, false, "최신 노트북", new Dictionary<string, int> { { "공격력", 25 }, { "치명타 배율", 80 }, { "치명타 확률", 30 } }, "최신형 노트북. 강력한 성능!", "전체", "무기", 10000);
            new Weapons(false, false, "구형 스마트폰", new Dictionary<string, int> { { "공격력", 15 }, { "치명타 배율", 20 }, { "치명타 확률", 10 } }, "낡았지만 아직 쓸 만함.", "전체", "무기", 3500);
            new Weapons(false, false, "무선 마우스", new Dictionary<string, int> { { "공격력", 15 }, { "마나", 30 }, { "치명타 배율", 25 } }, "무선이라 자유롭다.", "전체", "무기", 4400);
            new Weapons(false, false, "사무용 키보드", new Dictionary<string, int> { { "공격력", 20 }, { "명중률", 10 }, { "치명타 확률", 20 } }, "일하는 데 필요한 기본 장비.", "전체", "무기", 5200);
            new Weapons(false, false, "그래픽카드5090", new Dictionary<string, int> { { "공격력", 14 }, { "치명타 배율", 50 }, { "마나", 90 } }, "강력한 그래픽 카드. 성능 최고.", "전체", "무기", 5090);
            new Weapons(false, false, "Moo나미볼펜", new Dictionary<string, int> { { "공격력", 2 }, { "치명타 확률", 3 } }, "간편한 볼펜. 강력한 공격력.", "전체", "무기", 500);
            new Weapons(false, false, "형광펜", new Dictionary<string, int> { { "공격력", 5 }, { "치명타 배율", 15 } }, "형광펜의 빛, 강력한 타격!", "전체", "무기", 1100);
            new Weapons(false, false, "빗자루&쓰레받이", new Dictionary<string, int> { { "공격력", 7 }, { "마나", 20 }, { "치명타 배율", 20 } }, "청소 도구도 전투에 강력하게.", "전체", "무기", 1800);
            new Weapons(false, false, "결제 화일", new Dictionary<string, int> { { "공격력", 9 }, { "방어력", -2 }, { "치명타 확률", -3 } }, "인사팀의 중요한 서류.", "인사팀", "무기", 2000);
            new Weapons(false, false, "명함", new Dictionary<string, int> { { "공격력", 5 }, { "마나", 40 }, { "치명타 확률", -5 } }, "홍보팀에서 사용하는 명함.", "홍보팀", "무기", 2000);
            new Weapons(false, false, "계산기", new Dictionary<string, int> { { "공격력", 9 }, { "마나", -15 }, { "회피율", -10 } }, "총무팀의 중요한 계산 도구.", "총무팀", "무기", 2000);
            new Weapons(false, false, "커터칼", new Dictionary<string, int> { { "공격력", 8 }, { "방어력", 2 }, { "명중률", -10 } }, "영업팀의 중요한 도구.", "영업팀", "무기", 2000);
            new Weapons(false, false, "만년필", new Dictionary<string, int> { { "공격력", 8 }, { "마나", -20 }, { "명중률", -5 } }, "전산팀의 중요한 도구.", "전산팀", "무기", 2000);
            new Weapons(false, false, "수정 테이프", new Dictionary<string, int> { { "공격력", 7 }, { "명중률", -10 }, { "치명타 확률", 10 } }, "기획팀의 필수 아이템.", "기획팀", "무기", 2000);

            // 방어구
            new Weapons(false, false, "싸구려 정장 상의", new Dictionary<string, int> { { "방어력", 4 }, { "회피율", 4 } }, "저렴한 정장 상의.", "전체", "상의", 1500);
            new Weapons(false, false, "통풍셔츠", new Dictionary<string, int> { { "방어력", 5 }, { "마나", 25 }, { "치명타 확률", 10 } }, "시원하고 방어도 가능한 셔츠.", "전체", "상의", 3000);
            new Weapons(false, false, "명품 정장 상의", new Dictionary<string, int> { { "공격력", 3 }, { "방어력", 6 }, { "치명타 확률", 10 }, { "체력", 15 } }, "고급스러운 정장 상의.", "전체", "상의", 10000);
            new Weapons(false, false, "싸구려 정장 하의", new Dictionary<string, int> { { "방어력", 3 }, { "마나", 15 } }, "저렴한 정장 바지.", "전체", "하의", 1200);
            new Weapons(false, false, "반바지", new Dictionary<string, int> { { "방어력", 4 }, { "명중률", 10 }, { "회피율", 5 } }, "여름에 좋은 반바지.", "전체", "하의", 2800);
            new Weapons(false, false, "명품 정장 하의", new Dictionary<string, int> { { "공격력", 3 }, { "방어력", 5 }, { "치명타 배율", 20 }, { "체력", 10 } }, "고급스러운 정장 바지.", "전체", "하의", 10000);
            new Weapons(false, false, "발가락 양말", new Dictionary<string, int> { { "방어력", 2 }, { "회피율", 5 } }, "편안한 양말.", "전체", "신발", 1000);
            new Weapons(false, false, "크록스", new Dictionary<string, int> { { "방어력", 3 }, { "치명타 배율", 20 }, { "마나", 15 } }, "편안한 크록스.", "전체", "신발", 2400);
            new Weapons(false, false, "냄새나는 구두", new Dictionary<string, int> { { "방어력", 2 }, { "치명타 배율", 14 } }, "구두는 오래 신을수록 힘을 발휘한다.", "전체", "신발", 1600);
            new Weapons(false, false, "명품 구두", new Dictionary<string, int> { { "공격력", 4 }, { "방어력", 3 }, { "명중률", 10 }, { "회피율", 10 } }, "고급스러운 정장 구두.", "전체", "신발", 10000);

            // 악세서리
            new Weapons(false, false, "알없는 안경", new Dictionary<string, int> { { "공격력", 3 }, { "방어력", 2 } }, "기본적인 안경.", "총무팀", "장신구", 2000);
            new Weapons(false, false, "서류철", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 2 }, { "명중률", 5 } }, "인사팀의 중요한 서류.", "인사팀", "장신구", 1500);
            new Weapons(false, false, "테이프", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 2 }, { "회피율", 3 } }, "홍보팀의 필수 아이템.", "홍보팀", "장신구", 1500);
            new Weapons(false, false, "C타입 충전기", new Dictionary<string, int> { { "공격력", 3 }, { "방어력", 3 }, { "치명타 확률", -5 } }, "총무팀의 중요한 장비.", "총무팀", "장신구", 1500);
            new Weapons(false, false, "USB", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 3 } }, "영업팀의 필수 장비.", "영업팀", "장신구", 1500);
            new Weapons(false, false, "보조배터리", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 2 }, { "마나", 15 } }, "전산팀의 중요한 장비.", "전산팀", "장신구", 1500);
            new Weapons(false, false, "건전지", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 2 }, { "치명타 확률", 3 } }, "기획팀의 필수 아이템.", "기획팀", "장신구", 1500);
            new Weapons(false, false, "명품 시계", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 2 }, { "마나", 50 }, { "치명타 배율", 30 } }, "고급스러운 명품 시계.", "전체", "장신구", 10000);
            new Weapons(false, false, "A4용지", new Dictionary<string, int> { { "마나", 15 }, { "치명타 확률", 5 } }, "기본적인 A4용지.", "전체", "장신구", 900);
            new Weapons(false, false, "에어팟", new Dictionary<string, int> { { "마나", 50 }, { "방어력", -3 }, { "회피율", 25 } }, "전투 중 음악을 즐기자.", "전체", "장신구", 2500);
            new Weapons(false, false, "노트북백팩", new Dictionary<string, int> { { "공격력", 3 }, { "방어력", 3 }, { "치명타 확률", 15 } }, "노트북과 함께 싸우자!", "전체", "장신구", 3300);

            // 물약 (임시)
            new Weapons(false, false, "작은 회복 포션", new Dictionary<string, int> { { "HP", 50 } }, "작은 병에 담긴 회복 포션.", "전체", "포션", 300);
            new Weapons(false, false, "중간 회복 포션", new Dictionary<string, int> { { "HP", 150 } }, "중간 크기의 회복 포션.", "전체", "포션", 800);
            new Weapons(false, false, "마나 포션", new Dictionary<string, int> { { "MP", 100 } }, "마나를 회복시켜주는 포션.", "전체", "포션", 750);
        }

        public static void NotBuyAbleWeaponSpawn()
        {
            if (Weapons.NotbuyAbleInventory.Count > 0) return;

            new Weapons(false, false, "최신 스마트폰", new Dictionary<string, int> { { "공격력", 18 }, { "치명타 확률", 10 } }, "요즘껀 접히기도 하네..", "전체", "무기", "사장", 15, 2400);
            new Weapons(false, false, "브랜드 정장 상의", new Dictionary<string, int> { { "방어력", 6 }, { "명중률", 15 }, { "치명타", 20 } }, "고급스러움이 풍기는 상의.", "전체", "상의", "전무", 16, 2200);
            new Weapons(false, false, "브랜드 정장 하의", new Dictionary<string, int> { { "방어력", 5 }, { "회피율", 15 }, { "치명타 확률", 10 } }, "편안함과 스타일을 동시에.", "전체", "하의", "상무", 17, 2100);
            new Weapons(false, false, "브랜드 구두", new Dictionary<string, int> { { "공격력", 2 }, { "방어력", 2 }, { "마나", 15 } }, "고급 가죽으로 제작된 구두.", "전체", "신발", "이사", 18, 2000);
            new Weapons(false, false, "방석&등받이 쿠션", new Dictionary<string, int> { { "방어력", 2 }, { "마나", 20 }, { "회피율", 10 } }, "오래 앉아 있어도 괜찮다!", "전체", "장신구", "실장", 19, 2000);
            new Weapons(false, false, "반창고", new Dictionary<string, int> { { "HP", 50 } }, "작은 상처는 반창고 하나면 충분!", "전체", "포션", "전체", 18, 300);
            new Weapons(false, false, "바까스", new Dictionary<string, int> { { "MP", 30 } }, "피로회복제 그 이름, 바까스!", "전체", "포션", "전체", 18, 300);
            new Weapons(false, false, "든든한 국밥", new Dictionary<string, int> { { "MaxHP", 10 }, { "MP", 10 } }, "마음까지 든든해진다.", "전체", "포션", "전무, 사장, 회장", 5, 1000);
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
            new Weapons(true, "직급 명패", "전 회장의 명패이니 필요 없겠지?", "전리품", "회장", 10, 10000);

        }
    }
}
