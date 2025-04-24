using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.WeaponManagemant;

namespace TextRPG.ItemSpawn
{
    public class ItemSpawn
    {
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
            new Weapons(false, false, "작은 회복 포션", new Dictionary<string, int> { { "HP회복", 50 } }, "작은 병에 담긴 회복 포션.", "전체", "포션", 300);
            new Weapons(false, false, "중간 회복 포션", new Dictionary<string, int> { { "HP회복", 150 } }, "중간 크기의 회복 포션.", "전체", "포션", 800);
            new Weapons(false, false, "마나 포션", new Dictionary<string, int> { { "MP회복", 100 } }, "마나를 회복시켜주는 포션.", "전체", "포션", 750);
        }

        public static void NotBuyAbleWeaponSpawn() 
        {
            
        }

        }
    }
}
