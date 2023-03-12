using Dapper;
using Erp_Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_Lib.Apt_Infor
{
    /// <summary>
    /// 아파트 기본정보
    /// </summary>
    public class Apt_Basic_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 단지 식별코드
        /// </summary>
        public string kaptCode { get; set; }//단지코드

        /// <summary>
        /// 단지명
        /// </summary>
        public string kaptName { get; set; }

        /// <summary>
        /// 법정동 주소
        /// </summary>
        public string kaptAddr { get; set; }//법정동 주소

        /// <summary>
        /// 시도
        /// </summary>
        public string sido { get; set; }

        /// <summary>
        /// 시군구
        /// </summary>
        public string sigungo { get; set; }

        /// <summary>
        /// 읍면
        /// </summary>
        public string eupmyeon { get; set; }

        /// <summary>
        /// 마을
        /// </summary>
        public string maeul { get; set; }

        /// <summary>
        /// 아파트 형태
        /// </summary>
        public string codeAptNm { get; set; }

        /// <summary>
        /// 분양형태
        /// </summary>
        public string codeSaleNm { get; set; }

        /// <summary>
        /// 난방방식
        /// </summary>
        public string codeHeatNm { get; set; }

        /// <summary>
        /// 연면적(건축물 대장 상)
        /// </summary>
        public double kaptTarea { get; set; }

        /// <summary>
        /// 동수
        /// </summary>
        public int kaptDongCnt { get; set; }

        /// <summary>
        /// 세대수
        /// </summary>
        public int kaptdaCnt { get; set; }

        /// <summary>
        /// 분양 세대수
        /// </summary>
        public int kaptdaByCnt { get; set; }

        /// <summary>
        /// 임대 세대
        /// </summary>
        public int kaptRentCnt { get; set; }

        /// <summary>
        /// 시공사
        /// </summary>
        public string kaptBcompany { get; set; }

        /// <summary>
        /// 시행사
        /// </summary>
        public string kaptAcompany { get; set; }

        /// <summary>
        /// 관리사무소 연락처
        /// </summary>
        public string kaptTel { get; set; }

        /// <summary>
        /// 관리사무소 팩스
        /// </summary>
        public string kaptFax { get; set; }

        /// <summary>
        /// 홈페이지 주소
        /// </summary>
        public string kaptUrl { get; set; }

        /// <summary>
        /// 도로명 주소
        /// </summary>
        public string droJuso { get; set; }

        /// <summary>
        /// 호수 (182)
        /// </summary>
        public int hoCnt { get; set; }

        /// <summary>
        /// 관리방식
        /// </summary>
        public string codeMgrNm { get; set; }

        /// <summary>
        /// 복도 유형
        /// </summary>
        public string codeHallNm { get; set; }

        /// <summary>
        /// 사용승인일
        /// </summary>
        public DateTime kaptUsedate { get; set; }

        /// <summary>
        /// 가입일
        /// </summary>
        public DateTime kaptJdate { get; set; }

        /// <summary>
        /// 관리비 부과 면적
        /// </summary>
        public double kaptMarea { get; set; }

        /// <summary>
        /// 전용면적별 세대현황(60 이하)
        /// </summary>
        public int kaptMparea_60 { get; set; }

        /// <summary>
        /// 전용면적 세대 현황(60~85)
        /// </summary>
        public int kaptMparea_85 { get; set; }

        /// <summary>
        /// 전용면적별 세대 현황(85~135)
        /// </summary>
        public int kaptMparea_135 { get; set; }

        /// <summary>
        /// 전용면적 세대 현황(135 초과)
        /// </summary>
        public int kaptMparea_136 { get; set; }

        /// <summary>
        /// 전용면적 합계(건축물 대장 상)
        /// </summary>
        public double privArea { get; set; }

        /// <summary>
        /// 법정동 코드
        /// </summary>
        public string bjdCode { get; set; }
    }

    /// <summary>
    /// 아파트 상세
    /// </summary>
    public class Apt_Details_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 아파트 식별코드
        /// </summary>
        public string kaptCode { get; set; }

        /// <summary>
        /// 공동주택 명칭
        /// </summary>
        public string kaptName { get; set; }

        /// <summary>
        /// 관리방식
        /// </summary>
        public string codeMgr { get; set; }

        /// <summary>
        /// 관리인원
        /// </summary>
        public int kaptMgrCnt { get; set; }

        /// <summary>
        /// 공동주택 위탁관리업체
        /// </summary>
        public string kaptCcompany { get; set; }

        /// <summary>
        /// 경비관리방식
        /// </summary>
        public string codeSec { get; set; }

        /// <summary>
        /// 경비인원
        /// </summary>
        public int kaptdScnt { get; set; }

        /// <summary>
        /// 경비 업체
        /// </summary>
        public string kaptdSecCom { get; set; }

        /// <summary>
        /// 청소관리방식
        /// </summary>
        public string codeClean { get; set; }

        /// <summary>
        /// 청소관리인원
        /// </summary>
        public int kaptdClcnt { get; set; }

        /// <summary>
        /// 음식물처리방법
        /// </summary>
        public string codeGarbage { get; set; }

        /// <summary>
        /// 소독관리방식
        /// </summary>
        public string codeDisinf { get; set; }

        /// <summary>
        /// 소독횟수
        /// </summary>
        public int kaptdDcnt { get; set; }

        /// <summary>
        /// 소독방법
        /// </summary>
        public string disposalType { get; set; }

        /// <summary>
        /// 건물구조
        /// </summary>
        public string codeStr { get; set; }

        /// <summary>
        /// 수전용량
        /// </summary>
        public string kaptdEcapa { get; set; }

        /// <summary>
        /// 전기계약방식
        /// </summary>
        public string codeEcon { get; set; }

        /// <summary>
        /// 전기안전관리자 법정 선임 여부
        /// </summary>
        public string codeEmgr { get; set; }

        /// <summary>
        /// 화재수신기 방식
        /// </summary>
        public string codeFalarm { get; set; }

        /// <summary>
        /// 급수방식
        /// </summary>
        public string codeWsupply { get; set; }

        /// <summary>
        /// 승강기관리형태
        /// </summary>
        public string codeElev { get; set; }

        /// <summary>
        /// 승강기 대수
        /// </summary>
        public int kaptdEcnt { get; set; }

        /// <summary>
        /// 화물 승강기 대수
        /// </summary>
        public int kaptdwhacnt { get; set; }

        /// <summary>
        /// 승객 + 화물 승강기 대수
        /// </summary>
        public int kaptdmenEcnt { get; set; }

        /// <summary>
        /// 승강기 장애인용
        /// </summary>
        public int kaptDbdEcnt { get; set; }

        /// <summary>
        /// 승강기 비상용
        /// </summary>
        public int kaptRencEcnt { get; set; }

        /// <summary>
        /// 승강기 기타
        /// </summary>
        public int kaptdetcEcnt { get; set; }

        /// <summary>
        /// 총 주차대수
        /// </summary>
        public int kaptTtPcnt { get; set; }

        /// <summary>
        /// 지상 주차대수
        /// </summary>
        public int kaptdPcnt { get; set; }

        /// <summary>
        /// 지하 주차대수
        /// </summary>
        public int kaptdPcntu { get; set; }

        /// <summary>
        /// 주차관제, 홈내트워크
        /// </summary>
        public string codeNet { get; set; }

        /// <summary>
        /// CCTV대수
        /// </summary>
        public int kaptdCccnt { get; set; }

        /// <summary>
        /// 관리사무소 주소
        /// </summary>
        public string kaptAddrM { get; set; }

        /// <summary>
        /// 부대 및 복리시설
        /// </summary>
        public string welfareFacility { get; set; }

        /// <summary>
        /// 버스정류장 거리
        /// </summary>
        public string kaptdWtimebus { get; set; }

        /// <summary>
        /// 지하철 호선
        /// </summary>
        public string subwayLine { get; set; }

        /// <summary>
        /// 지하철 역
        /// </summary>
        public string subwayStation { get; set; }

        /// <summary>
        /// 지하철역 거리
        /// </summary>
        public string kaptdWtimesub { get; set; }

        /// <summary>
        /// 편의시설
        /// </summary>
        public string convenientFacility { get; set; }

        /// <summary>
        /// 교육시설
        /// </summary>
        public string educationFacility { get; set; }

        public DateTime kaptJdate { get; set; }

        public DateTime PostDate { get; set; }
    }

    public class AptInfor_All
    {
        /// <summary>
        /// 단지 식별코드
        /// </summary>
        public string kaptCode { get; set; }//단지코드

        /// <summary>
        /// 단지명
        /// </summary>
        public string kaptName { get; set; }

        /// <summary>
        /// 법정동 주소
        /// </summary>
        public string kaptAddr { get; set; }//법정동 주소

        /// <summary>
        /// 시도
        /// </summary>
        public string sido { get; set; }

        /// <summary>
        /// 시군구
        /// </summary>
        public string sigungo { get; set; }

        /// <summary>
        /// 읍면
        /// </summary>
        public string eupmyeon { get; set; }

        /// <summary>
        /// 마을
        /// </summary>
        public string maeul { get; set; }

        /// <summary>
        /// 아파트 형태
        /// </summary>
        public string codeAptNm { get; set; }

        /// <summary>
        /// 분양형태
        /// </summary>
        public string codeSaleNm { get; set; }

        /// <summary>
        /// 난방방식
        /// </summary>
        public string codeHeatNm { get; set; }

        /// <summary>
        /// 연면적(건축물 대장 상)
        /// </summary>
        public double kaptTarea { get; set; }

        /// <summary>
        /// 동수
        /// </summary>
        public int kaptDongCnt { get; set; }

        /// <summary>
        /// 세대수
        /// </summary>
        public int kaptdaCnt { get; set; }

        /// <summary>
        /// 분양 세대수
        /// </summary>
        public int kaptdaByCnt { get; set; }

        /// <summary>
        /// 임대 세대
        /// </summary>
        public int kaptRentCnt { get; set; }

        /// <summary>
        /// 시공사
        /// </summary>
        public string kaptBcompany { get; set; }

        /// <summary>
        /// 시행사
        /// </summary>
        public string kaptAcompany { get; set; }

        /// <summary>
        /// 관리사무소 연락처
        /// </summary>
        public string kaptTel { get; set; }

        /// <summary>
        /// 관리사무소 팩스
        /// </summary>
        public string kaptFax { get; set; }

        /// <summary>
        /// 홈페이지 주소
        /// </summary>
        public string kaptUrl { get; set; }

        /// <summary>
        /// 도로명 주소
        /// </summary>
        public string droJuso { get; set; }

        /// <summary>
        /// 호수 (182)
        /// </summary>
        public int hoCnt { get; set; }

        /// <summary>
        /// 관리방식
        /// </summary>
        public string codeMgrNm { get; set; }

        /// <summary>
        /// 복도 유형
        /// </summary>
        public string codeHallNm { get; set; }

        /// <summary>
        /// 사용승인일
        /// </summary>
        public string kaptUsedate { get; set; }

        /// <summary>
        /// 가입일
        /// </summary>
        public string kaptJdate { get; set; }

        /// <summary>
        /// 관리비 부과 면적
        /// </summary>
        public double kaptMarea { get; set; }

        /// <summary>
        /// 전용면적별 세대현황(60 이하)
        /// </summary>
        public int kaptMparea_60 { get; set; }

        /// <summary>
        /// 전용면적 세대 현황(60~85)
        /// </summary>
        public int kaptMparea_85 { get; set; }

        /// <summary>
        /// 전용면적별 세대 현황(85~135)
        /// </summary>
        public int kaptMparea_135 { get; set; }

        /// <summary>
        /// 전용면적 세대 현황(135 초과)
        /// </summary>
        public int kaptMparea_136 { get; set; }

        /// <summary>
        /// 전용면적 합계(건축물 대장 상)
        /// </summary>
        public double privArea { get; set; }

        /// <summary>
        /// 법정동 코드
        /// </summary>
        public string bjdCode { get; set; }
                
        /// <summary>
        /// 관리방식
        /// </summary>
        public string codeMgr { get; set; }

        /// <summary>
        /// 관리인원
        /// </summary>
        public int kaptMgrCnt { get; set; }

        /// <summary>
        /// 공동주택 위탁관리업체
        /// </summary>
        public string kaptCcompany { get; set; }

        /// <summary>
        /// 경비관리방식
        /// </summary>
        public string codeSec { get; set; }

        /// <summary>
        /// 경비인원
        /// </summary>
        public int kaptdScnt { get; set; }

        /// <summary>
        /// 경비 업체
        /// </summary>
        public string kaptdSecCom { get; set; }

        /// <summary>
        /// 청소관리방식
        /// </summary>
        public string codeClean { get; set; }

        /// <summary>
        /// 청소관리인원
        /// </summary>
        public int kaptdClcnt { get; set; }

        /// <summary>
        /// 음식물처리방법
        /// </summary>
        public string codeGarbage { get; set; }

        /// <summary>
        /// 소독관리방식
        /// </summary>
        public string codeDisinf { get; set; }

        /// <summary>
        /// 소독횟수
        /// </summary>
        public int kaptdDcnt { get; set; }

        /// <summary>
        /// 소독방법
        /// </summary>
        public string disposalType { get; set; }

        /// <summary>
        /// 건물구조
        /// </summary>
        public string codeStr { get; set; }

        /// <summary>
        /// 수전용량
        /// </summary>
        public string kaptdEcapa { get; set; }

        /// <summary>
        /// 전기계약방식
        /// </summary>
        public string codeEcon { get; set; }

        /// <summary>
        /// 전기안전관리자 법정 선임 여부
        /// </summary>
        public string codeEmgr { get; set; }

        /// <summary>
        /// 화재수신기 방식
        /// </summary>
        public string codeFalarm { get; set; }

        /// <summary>
        /// 급수방식
        /// </summary>
        public string codeWsupply { get; set; }

        /// <summary>
        /// 승강기관리형태
        /// </summary>
        public string codeElev { get; set; }

        /// <summary>
        /// 승강기 대수
        /// </summary>
        public int kaptdEcnt { get; set; }

        /// <summary>
        /// 화물 승강기 대수
        /// </summary>
        public int kaptdwhacnt { get; set; }

        /// <summary>
        /// 승객 + 화물 승강기 대수
        /// </summary>
        public int kaptdmenEcnt { get; set; }

        /// <summary>
        /// 승강기 장애인용
        /// </summary>
        public double kaptDbdEcnt { get; set; }

        /// <summary>
        /// 승강기 비상용
        /// </summary>
        public double kaptRncEcnt { get; set; }

        /// <summary>
        /// 승강기 기타
        /// </summary>
        public double kaptdetcEcnt { get; set; }

        /// <summary>
        /// 총 주차대수
        /// </summary>
        public double kaptTtPcnt { get; set; }

        /// <summary>
        /// 지상 주차대수
        /// </summary>
        public string kaptdPcnt { get; set; }

        /// <summary>
        /// 지하 주차대수
        /// </summary>
        public string kaptdPcntu { get; set; }

        /// <summary>
        /// 주차관제, 홈내트워크
        /// </summary>
        public string codeNet { get; set; }

        /// <summary>
        /// CCTV대수
        /// </summary>
        public int kaptdCccnt { get; set; }

        /// <summary>
        /// 관리사무소 주소
        /// </summary>
        public string kaptAddrM { get; set; }

        /// <summary>
        /// 부대 및 복리시설
        /// </summary>
        public string welfareFacility { get; set; }

        /// <summary>
        /// 버스정류장 거리
        /// </summary>
        public string kaptdWtimebus { get; set; }

        /// <summary>
        /// 지하철 호선
        /// </summary>
        public string subwayLine { get; set; }

        /// <summary>
        /// 지하철 역
        /// </summary>
        public string subwayStation { get; set; }

        /// <summary>
        /// 지하철역 거리
        /// </summary>
        public string kaptdWtimesub { get; set; }

        /// <summary>
        /// 편의시설
        /// </summary>
        public string convenientFacility { get; set; }

        /// <summary>
        /// 교육시설
        /// </summary>
        public string educationFacility { get; set; }
    }   

    /// <summary>
    /// 아파트 정보 클레스
    /// </summary>
    public class Apt_Infor_Lib : IApt_Infor_Lib
    {
        private readonly IConfiguration _db;
        public Apt_Infor_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<List<AptInfor_All>> aptinforTest()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AptInfor_All>("Select Top 30 * From AptInfor");
            return lst.ToList();
        }

        public async Task<List<AptInfor_All>> aptinforList()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AptInfor_All>("Select * From AptInfor");
            return lst.ToList();
        }

        /// <summary>
        /// 공동주택 정보 추가하기
        /// </summary>
        public async Task Add()
        {
            //List<AptInfor_All> bnn = new List<AptInfor_All>();
            Apt_Basic_Entity ann = new Apt_Basic_Entity();
            Apt_Details_Entity dnn = new Apt_Details_Entity();
            
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));


            int Recount = await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From AptInfor");
            int Page = Recount / 30;

            for (int i = 0; i <= 201; i++)
            {
                var bnn = await db.QueryAsync<AptInfor_All>("Select Top 30 * From AptInfor Where Aid Not In (Select Top (30 * @i) Aid From AptInfor Order By kaptJdate Asc) Order by kaptJdate Asc", new { i });
                foreach (var b in bnn)
                {
                    ann.bjdCode = b.bjdCode;
                    ann.codeAptNm = b.codeAptNm;
                    ann.codeHallNm = b.codeHallNm;
                    ann.codeMgrNm = b.codeMgrNm;
                    ann.codeSaleNm = b.codeSaleNm;
                    ann.codeHeatNm = b.codeHeatNm;
                    ann.droJuso = b.droJuso;
                    ann.eupmyeon = b.eupmyeon;
                    ann.hoCnt = b.hoCnt;
                    ann.kaptAcompany = b.kaptAcompany;
                    ann.kaptAddr = b.kaptAddr;
                    ann.kaptBcompany = b.kaptBcompany;
                    ann.kaptCode = b.kaptCode;
                    ann.kaptdaByCnt = b.kaptdaByCnt;
                    ann.kaptdaCnt = b.kaptdaCnt;
                    ann.kaptDongCnt = b.kaptDongCnt;
                    ann.kaptFax = b.kaptFax;
                    ann.kaptMarea = b.kaptMarea;
                    ann.kaptMparea_135 = b.kaptMparea_135;
                    ann.kaptMparea_60 = b.kaptMparea_60;
                    ann.kaptMparea_136 = b.kaptMparea_136;
                    ann.kaptMparea_85 = b.kaptMparea_85;
                    ann.kaptName = b.kaptName;
                    ann.kaptRentCnt = b.kaptRentCnt;
                    ann.kaptTarea = b.kaptTarea;
                    ann.kaptTel = b.kaptTel;
                    ann.kaptUrl = b.kaptUrl;

                    if (!string.IsNullOrWhiteSpace(b.kaptUsedate))
                    {
                        string re1 = b.kaptUsedate.Replace(" ", "").Replace("-", "").Replace("_", "");
                        re1 = re1.Insert(4, "-");
                        re1 = re1.Insert(7, "-");
                        b.kaptUsedate = re1;
                    }
                    else
                    {
                        b.kaptUsedate = "2000-01-01";
                    }

                    if (!string.IsNullOrWhiteSpace(b.kaptJdate))
                    {
                        string re2 = b.kaptJdate.Replace(" ", "").Replace("-", "").Replace("_", "");
                        re2 = re2.Insert(4, "-");
                        re2 = re2.Insert(7, "-");
                        b.kaptJdate = re2;
                    }
                    else
                    {
                        b.kaptJdate = DateTime.Now.ToShortDateString();
                    }
                   
                    ann.kaptUsedate = Convert.ToDateTime(b.kaptUsedate);
                    
                    ann.kaptJdate = Convert.ToDateTime(b.kaptJdate);
                    dnn.kaptJdate = ann.kaptJdate;

                    ann.maeul = b.maeul;
                    ann.privArea = b.privArea;
                    ann.sido = b.sido;
                    ann.sigungo = b.sigungo;

                    try
                    {
                        dnn.kaptdPcntu = Convert.ToInt32(b.kaptdPcntu);
                    }
                    catch (Exception)
                    {
                        dnn.kaptdPcntu = 0;
                    }

                    try
                    {
                        dnn.kaptdPcnt = Convert.ToInt32(b.kaptdPcntu);
                    }
                    catch (Exception)
                    {
                        dnn.kaptdPcnt = 0;
                    }

                    dnn.codeClean = b.codeClean;
                    dnn.codeDisinf = b.codeDisinf;
                    dnn.codeEcon = b.codeEcon;
                    dnn.codeElev = b.codeElev;
                    dnn.codeEmgr = b.codeEmgr;
                    dnn.codeFalarm = b.codeFalarm;
                    dnn.codeGarbage = b.codeGarbage;
                    dnn.codeMgr = b.codeMgr;
                    dnn.codeNet = b.codeNet;
                    dnn.codeSec = b.codeSec;
                    dnn.codeStr = b.codeStr;
                    dnn.codeWsupply = b.codeWsupply;
                    dnn.convenientFacility = b.convenientFacility;
                    dnn.disposalType = b.disposalType;
                    dnn.educationFacility = b.educationFacility;
                    dnn.kaptAddrM = b.kaptAddrM;
                    dnn.kaptdCccnt = b.kaptdCccnt;
                    dnn.kaptdClcnt = b.kaptdClcnt;
                    dnn.kaptdDcnt = b.kaptdDcnt;
                    dnn.kaptCcompany = b.kaptCcompany;
                    dnn.kaptCode = b.kaptCode;
                    dnn.kaptdEcapa = b.kaptdEcapa;
                    dnn.kaptdEcnt = b.kaptdEcnt;
                    
                    try
                    {
                        dnn.kaptdetcEcnt = Convert.ToInt32(b.kaptdetcEcnt);
                    }
                    catch (Exception)
                    {
                        dnn.kaptdetcEcnt = 0;
                    }

                    try
                    {
                        dnn.kaptDbdEcnt = Convert.ToInt32(b.kaptDbdEcnt);
                    }
                    catch (Exception)
                    {
                        dnn.kaptDbdEcnt = 0;
                    }

                    dnn.kaptdScnt = b.kaptdScnt;
                    dnn.kaptdSecCom = b.kaptdSecCom;
                    dnn.kaptdwhacnt = b.kaptdwhacnt;
                    dnn.kaptdWtimebus = b.kaptdWtimebus;
                    dnn.kaptdWtimesub = b.kaptdWtimesub;
                    dnn.kaptdmenEcnt = b.kaptdmenEcnt;
                    dnn.kaptMgrCnt = b.kaptMgrCnt;
                    dnn.kaptName = b.kaptName;
                    //dnn.kaptmemEcnt = b.kaptmemEcnt;
                    try
                    {
                        dnn.kaptRencEcnt = Convert.ToInt32(b.kaptRncEcnt);
                    }
                    catch (Exception)
                    {
                        dnn.kaptRencEcnt = 0;
                    }
                    dnn.kaptTtPcnt = Convert.ToInt32(b.kaptTtPcnt);
                    dnn.subwayLine = b.subwayLine;
                    dnn.subwayStation = b.subwayStation;
                    dnn.welfareFacility = b.welfareFacility;

                    var sqlA = "Insert into Apt_Basis (kaptCode, kaptName, kaptAddr, sido, sigungo, eupmyeon, maeul, codeAptNm, codeSaleNm, codeHeatNm, KaptTarea, kaptDongCnt, kaptdaCnt, kaptdaByCnt, kaptRentCnt, kaptBcompany, kaptAcompany, kaptTel, kaptFax, kaptUrl, droJuso, hoCnt, codeMgrNm, codeHallNm, kaptUsedate, kaptMarea, kaptMparea_60, kaptMparea_85, kaptMparea_135, kaptMparea_136, privArea, bjdCode, kaptJdate) Values (@kaptCode, @kaptName, @kaptAddr, @sido, @sigungo, @eupmyeon, @maeul, @codeAptNm, @codeSaleNm, @codeHeatNm, @kaptTarea, @kaptDongCnt, @kaptdaCnt, @kaptdaByCnt, @kaptRentCnt, @kaptBcompany, @kaptAcompany, @kaptTel, @kaptFax, @kaptUrl, @droJuso, @hoCnt, @codeMgrNm, @codeHallNm, @kaptUsedate, @kaptMarea, @kaptMparea_60, @kaptMparea_85, @kaptMparea_135, @kaptMparea_136, @privArea, @bjdCode, @kaptJdate);";

                    var sqlB = "Insert Into Apt_Detail (kaptCode, kaptName, codeMgr, kaptMgrCnt, kaptCcompany, codeSec, kaptdScnt, kaptdSecCom, codeClean, kaptdClcnt, codeGarbage, codeDisinf, kaptdDcnt, disposalType, codeStr, kaptdEcapa, codeEcon, codeEmgr, codeFalarm, codeWsupply, codeElev, kaptdEcnt, kaptdwhacnt, kaptdmenEcnt, kaptDbdEcnt, kaptRencEcnt, kaptdetcEcnt, kaptTtPcnt, kaptdPcnt, kaptdPcntu, codeNet, kaptdCccnt, kaptAddrM, welfareFacility, kaptdWtimebus, subwayLine, subwayStation, kaptdWtimesub, convenientFacility, educationFacility, kaptJdate) Values (@kaptCode, @kaptName, @codeMgr, @kaptMgrCnt, @kaptCcompany, @codeSec, @kaptdScnt, @kaptdSecCom, @codeClean, @kaptdClcnt, @codeGarbage, @codeDisinf, @kaptdDcnt, @disposalType, @codeStr, @kaptdEcapa, @codeEcon, @codeEmgr, @codeFalarm, @codeWsupply, @codeElev, @kaptdEcnt, @kaptdwhacnt, @kaptdmenEcnt, @kaptDbdEcnt, @kaptRencEcnt, @kaptdetcEcnt, @kaptTtPcnt, @kaptdPcnt, @kaptdPcntu, @codeNet, @kaptdCccnt, @kaptAddrM, @welfareFacility, @kaptdWtimebus, @subwayLine, @subwayStation, @kaptdWtimesub, @convenientFacility, @educationFacility, @kaptJdate)";

                    //var sqlC = "Update Apt_Basis Set codeHeatNm = @codeHeatNm, codeHallNm = @codeHallNm Where kaptCode = @kaptCode";//난방, 복도식 
                    //await db.ExecuteAsync(sqlC, new { ann.codeHeatNm, ann.codeHallNm, ann.kaptCode }); //수정

                    //var sqlC = "Update Apt_Basis Set codeMgrNm = @codeMgrNm Where kaptCode = @kaptCode";//난방, 복도식 
                    //await db.ExecuteAsync(sqlC, new { ann.codeHeatNm, ann.codeHallNm, ann.kaptCode }); //수정

                    //var sqlD = "Update Apt_Detail Set codeMgr = @codeMgr, kaptMgrCnt = @kaptMgrCnt, kaptCcompany = @kaptCcompany, codeSec = @codeSec, kaptdScnt = @kaptdScnt, kaptdSecCom = @kaptdSecCom, codeClean = @codeClean, kaptdClcnt = @kaptdClcnt, codeGarbage = @codeGarbage, codeDisinf = @codeDisinf, codeEcon = @codeEcon, codeEmgr = @codeEmgr, codeElev = @EcodeElev, kaptdEcnt = @kaptdEcnt, kaptTtPcnt = @kaptTtPcnt codeNet = @codeNet, kaptdCccnt = @kaptdCccnt Where kaptCode = @kaptCode";//난방, 복도식 
                    //await db.ExecuteAsync(sqlD, dnn); //수정

                    string strKaptCode = b.kaptCode;
                    int acount = await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Basis Where kaptCode = @strKaptCode", new { strKaptCode });

                    if (acount < 1)
                    {
                        await db.ExecuteAsync(sqlA, ann);
                        await db.ExecuteAsync(sqlB, dnn);
                    }
                }
            }         
        }

        /// <summary>
        /// 공동주택 정보 수정하기
        /// </summary>
        public async Task Update()
        {
            //List<AptInfor_All> bnn = new List<AptInfor_All>();
            Apt_Basic_Entity ann = new Apt_Basic_Entity();
            Apt_Details_Entity dnn = new Apt_Details_Entity();

            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));


            int Recount = await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From AptInfor");
            int Page = Recount / 30;

            for (int i = 0; i <= Page; i++)
            {
                var bnn = await db.QueryAsync<AptInfor_All>("Select Top 30 * From AptInfor Where Aid Not In (Select Top (30 * @i) Aid From AptInfor Order By kaptJdate Asc) Order by kaptJdate Asc", new { i });
                foreach (var b in bnn)
                {
                    ann.bjdCode = b.bjdCode;
                    ann.codeAptNm = b.codeAptNm;
                    ann.codeHallNm = b.codeHallNm;
                    ann.codeMgrNm = b.codeMgrNm;
                    ann.codeSaleNm = b.codeSaleNm;
                    ann.codeHeatNm = b.codeHeatNm;
                    ann.droJuso = b.droJuso;
                    ann.eupmyeon = b.eupmyeon;
                    ann.hoCnt = b.hoCnt;
                    ann.kaptAcompany = b.kaptAcompany;
                    ann.kaptAddr = b.kaptAddr;
                    ann.kaptBcompany = b.kaptBcompany;
                    ann.kaptCode = b.kaptCode;
                    ann.kaptdaByCnt = b.kaptdaByCnt;
                    ann.kaptdaCnt = b.kaptdaCnt;
                    ann.kaptDongCnt = b.kaptDongCnt;
                    ann.kaptFax = b.kaptFax;
                    ann.kaptMarea = b.kaptMarea;
                    ann.kaptMparea_135 = b.kaptMparea_135;
                    ann.kaptMparea_60 = b.kaptMparea_60;
                    ann.kaptMparea_136 = b.kaptMparea_136;
                    ann.kaptMparea_85 = b.kaptMparea_85;
                    ann.kaptName = b.kaptName;
                    ann.kaptRentCnt = b.kaptRentCnt;
                    ann.kaptTarea = b.kaptTarea;
                    ann.kaptTel = b.kaptTel;
                    ann.kaptUrl = b.kaptUrl;

                    if (!string.IsNullOrWhiteSpace(b.kaptUsedate))
                    {
                        string re1 = b.kaptUsedate.Replace(" ", "").Replace("-", "").Replace("_", "");
                        re1 = re1.Insert(4, "-");
                        re1 = re1.Insert(7, "-");
                        b.kaptUsedate = re1;
                    }
                    else
                    {
                        b.kaptUsedate = "2000-01-01";
                    }

                    if (!string.IsNullOrWhiteSpace(b.kaptJdate))
                    {
                        string re2 = b.kaptJdate.Replace(" ", "").Replace("-", "").Replace("_", "");
                        re2 = re2.Insert(4, "-");
                        re2 = re2.Insert(7, "-");
                        b.kaptJdate = re2;
                    }
                    else
                    {
                        b.kaptJdate = DateTime.Now.ToShortDateString();
                    }

                    ann.kaptUsedate = Convert.ToDateTime(b.kaptUsedate);

                    ann.kaptJdate = Convert.ToDateTime(b.kaptJdate);
                    dnn.kaptJdate = ann.kaptJdate;

                    ann.maeul = b.maeul;
                    ann.privArea = b.privArea;
                    ann.sido = b.sido;
                    ann.sigungo = b.sigungo;

                    try
                    {
                        dnn.kaptdPcntu = Convert.ToInt32(b.kaptdPcntu);
                    }
                    catch (Exception)
                    {
                        dnn.kaptdPcntu = 0;
                    }

                    try
                    {
                        dnn.kaptdPcnt = Convert.ToInt32(b.kaptdPcntu);
                    }
                    catch (Exception)
                    {
                        dnn.kaptdPcnt = 0;
                    }

                    dnn.codeClean = b.codeClean;
                    dnn.codeDisinf = b.codeDisinf;
                    dnn.codeEcon = b.codeEcon;
                    dnn.codeElev = b.codeElev;
                    dnn.codeEmgr = b.codeEmgr;
                    dnn.codeFalarm = b.codeFalarm;
                    dnn.codeGarbage = b.codeGarbage;
                    dnn.codeMgr = b.codeMgr;
                    dnn.codeNet = b.codeNet;
                    dnn.codeSec = b.codeSec;
                    dnn.codeStr = b.codeStr;
                    dnn.codeWsupply = b.codeWsupply;
                    dnn.convenientFacility = b.convenientFacility;
                    dnn.disposalType = b.disposalType;
                    dnn.educationFacility = b.educationFacility;
                    dnn.kaptAddrM = b.kaptAddrM;
                    dnn.kaptdCccnt = b.kaptdCccnt;
                    dnn.kaptdClcnt = b.kaptdClcnt;
                    dnn.kaptdDcnt = b.kaptdDcnt;
                    dnn.kaptCcompany = b.kaptCcompany;
                    dnn.kaptCode = b.kaptCode;
                    dnn.kaptdEcapa = b.kaptdEcapa;
                    dnn.kaptdEcnt = b.kaptdEcnt;

                    try
                    {
                        dnn.kaptdetcEcnt = Convert.ToInt32(b.kaptdetcEcnt);
                    }
                    catch (Exception)
                    {
                        dnn.kaptdetcEcnt = 0;
                    }

                    try
                    {
                        dnn.kaptDbdEcnt = Convert.ToInt32(b.kaptDbdEcnt);
                    }
                    catch (Exception)
                    {
                        dnn.kaptDbdEcnt = 0;
                    }

                    dnn.kaptdScnt = b.kaptdScnt;
                    dnn.kaptdSecCom = b.kaptdSecCom;
                    dnn.kaptdwhacnt = b.kaptdwhacnt;
                    dnn.kaptdWtimebus = b.kaptdWtimebus;
                    dnn.kaptdWtimesub = b.kaptdWtimesub;
                    dnn.kaptdmenEcnt = b.kaptdmenEcnt;
                    dnn.kaptMgrCnt = b.kaptMgrCnt;
                    dnn.kaptName = b.kaptName;
                    //dnn.kaptmemEcnt = b.kaptmemEcnt;
                    try
                    {
                        dnn.kaptRencEcnt = Convert.ToInt32(b.kaptRncEcnt);
                    }
                    catch (Exception)
                    {
                        dnn.kaptRencEcnt = 0;
                    }
                    dnn.kaptTtPcnt = Convert.ToInt32(b.kaptTtPcnt);
                    dnn.subwayLine = b.subwayLine;
                    dnn.subwayStation = b.subwayStation;
                    dnn.welfareFacility = b.welfareFacility;



                    //var sqlC = "Update Apt_Basis Set sigungo = @sigungo Where kaptCode = @kaptCode";//난방, 복도식 
                    //await db.ExecuteAsync(sqlC, new { ann.sigungo, ann.kaptCode }); //수정

                    var sqlC = "Update Apt_Basis Set codeMgrNm = @codeMgrNm Where kaptCode = @kaptCode";//난방, 복도식 
                    await db.ExecuteAsync(sqlC, new { ann.codeMgrNm, ann.kaptCode }); //수정

                    var sqlD = "Update Apt_Detail Set codeMgr = @codeMgr, kaptMgrCnt = @kaptMgrCnt, kaptCcompany = @kaptCcompany, codeSec = @codeSec, kaptdScnt = @kaptdScnt, kaptdSecCom = @kaptdSecCom, codeClean = @codeClean, kaptdClcnt = @kaptdClcnt, codeGarbage = @codeGarbage, codeDisinf = @codeDisinf, codeEcon = @codeEcon, codeEmgr = @codeEmgr, codeElev = @EcodeElev, kaptdEcnt = @kaptdEcnt, kaptTtPcnt = @kaptTtPcnt codeNet = @codeNet, kaptdCccnt = @kaptdCccnt Where kaptCode = @kaptCode";//난방, 복도식 
                    await db.ExecuteAsync(sqlD, dnn); //수정

                }
            }
        }

        /// <summary>
        /// 기본정보 수정
        /// </summary>
        public async Task<Apt_Basic_Entity> Edd_Basis(Apt_Basic_Entity enn)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var sql = "Update Apt_Basis Set kaptName = @kaptName, kaptAddr = @kaptAddr, sido = @sido, sigungo = @sigungo, enpmyeon = @eupmyeon, maeul = @maeul, codeAptNm = @codeAptNm, codeSaleNm = @codeSaleNm, codeHeatNm = @codHeatNm, kaptTarea = @kaptTarea, kaptDongCnt = @kaptDongCnt, kaptdaCnt = @kaptdaCnt, kaptdaByCnt = @kaptdaByCnt, kaptRentCnt = @kaptRentCnt, kaptBcompany = @kaptBcompany, kaptAcompany = @kaptAcompany, kaptTel = @kaptTel, kaptFax = @kaptFax, kaptUrl = @kaptUrl, droJuso = @droJuso, hoCnt = @hoCnt, codeMgrNm = @codeMgrNm, codeHollNm = @codeHollNm, kaptUsedate = @kaptUsedate, kaptMarea = @kaptMarea, kaptMparea_60 = @kaptMparea_60, kaptMparea_85 = @kaptMparea_85, kaptMparea_135 = @kaptMparea_135, kaptMparea_136 = @kaptMparea_136, privArea = @privArea, bjdCode = @bjdCode Where Aid = @Aid;";
            await db.ExecuteAsync(sql, enn);
            return enn;
        }

        /// <summary>
        /// 상세정보 수정
        /// </summary>
        public async Task<Apt_Details_Entity> Edd_Detail(Apt_Details_Entity enn)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var sql = "Update Apt_Detail Set kaptName = @kaptName, codeMgr = @codeMgr, kaptMgrCnt = @kaptMgrCnt, kaptCcompany = @kaptCcompany, codeSec = @codeSec, kaptdScnt = @kaptdScnt, kaptdSeCom = @kaptdSecCom, codeClean = @codeClean, kaptdClcnt = @kaptdClcnt, codeGarbage = @codeGarbage, codeDisinf = @codeDisinf, kaptdDcnt = @kaptdDcnt, disposalType = @disposalType, codeStr = @codeStr, kaptdEcapa = @kaptdEcapa, codeEcon = @codeEcon, codeEmgr = @codeEmgr, codeFalarm = @codeFalarm, codeWsupply = @codeWsupply, codeElev = @codeElev, kaptdEcnt = @kaptdEcnt, kaptdwhacnt = @kaptdwhacnt, kaptdmenEcnt = @kaptdmenEcnt, kaptDbdEcnt = @kaptDbdEcnt, kaptRencEcnt = @kaptRencEcnt, kaptdetcEcnt = @kaptdetcEcnt, kaptTtPcnt = @kaptTtPcnt, kaptdPcnt = @kaptdPcnt, kaptdPcntu = @kaptdPcntu, codeNet = @codeNet, kaptdCccnt = @kaptdCccnt, kaptAddrM = @kaptAddrM, welfareFacility = @welfareFacility, kaptdWtimebus = @kaptdWtimebus, subwayLine = @subwayLine, subwayStation = @subwayStation, kaptdWtimesub = @kaptdWtimesub, convenientFacility = @convenientFacility, educationFacility = @educationFacility Where Aid = @Aid;";
            await db.ExecuteAsync(sql, enn);
            return enn;
        }

        /// <summary>
        /// 기본정보 목록
        /// </summary>
        public async Task<List<Apt_Basic_Entity>> GetList_Basis(int Page)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Apt_Basic_Entity>("Select Top 15 * From Apt_Basis Where Aid Not In (Select Top (15 * @Page) Aid From Apt_Basis Order By Aid Asc) Order by Aid Asc", new {Page});
            return lst.ToList();
        }

        /// <summary>
        /// 기본정보 목록 수
        /// </summary>
        public async Task<int> GetList_Basis_Count()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Basis");
        }

        /// <summary>
        /// 상세정보 목록
        /// </summary>
        public async Task<List<Apt_Details_Entity>> GetList_Detail(int Page)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Apt_Details_Entity>("Select Top 15 * From Apt_Detail Where Aid Not In (Select Top (15 * @Page) Aid From Apt_Detail Order By Aid Asc) Order by Aid Asc", new {Page});
            return lst.ToList();
        }

        /// <summary>
        /// 상세정보 목록 수
        /// </summary>
        public async Task<int> GetList_Detail_Count()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Detail");
        }

        /// <summary>
        /// 공동주택 기본정보 상세
        /// </summary>
        public async Task<Apt_Basic_Entity> GetBy_Basis(string kaptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<Apt_Basic_Entity>("Select Top 1 * From Apt_Basis Where kaptCode = @kaptCode Order by Aid Desc", new { kaptCode });
        }

        /// <summary>
        /// 공동주택 상세정보 상세
        /// </summary>
        public async Task<Apt_Details_Entity> GetBy_Detail(string kaptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<Apt_Details_Entity>("Select Top 1 * From Apt_Detail Where kaptCode = @kaptCode Order by Aid Desc", new { kaptCode });
        }

        /// <summary>
        /// 기본정보 시도 목록
        /// </summary>
        public async Task<List<Apt_Basic_Entity>> SearchList_Juso_Basis()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Apt_Basic_Entity>("Select DISTINCT sido From Apt_Basis");
            return lst.ToList();
        }

        /// <summary>
        /// 기본정보 시도로 찾은 시군구 목록
        /// </summary>
        public async Task<List<Apt_Basic_Entity>> SearchList_sodo_Basis(string sido)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Apt_Basic_Entity>("Select DISTINCT sigungo From Apt_Basis Where sido = @sido Order by sigungo Asc", new {sido});
            return lst.ToList();
        }

       

        /// <summary>
        /// 기본정보 시도 및 시군구로 찾은 시군구 목록
        /// </summary>
        public async Task<List<Apt_Basic_Entity>> SearchList_sigungo_Basis(string sido, string sigungo)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Apt_Basic_Entity>("Select DISTINCT maeul From Apt_Basis Where sido = @sido And sigungo = @sigungo", new {sido, sigungo});
            return lst.ToList();
        }

        /// <summary>
        /// 기본정보 시도 및 시군구로 찾은 시군구 목록
        /// </summary>
        public async Task<List<Apt_Basic_Entity>> SearchList_sigungo_maeul_Basis(string sido, string sigungo, string maeul)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Apt_Basic_Entity>("Select kaptCode, kaptName From Apt_Basis Where sido = @sido And sigungo = @sigungo And maeul = @maeul", new { sido, sigungo, maeul });
            return lst.ToList();
        }
    }
}
