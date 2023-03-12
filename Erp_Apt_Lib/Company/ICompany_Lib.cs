using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company
{
    public interface ICompany_Lib
    {
        Task<int> Add(Company_Entity ci);
        Task<Company_Entity> edit(Company_Entity ci);
        Task<int> CorNum_Being(string CorporateResistration_Num);
        Task<Company_Entity> CorNum_Detail(string CorporateResistration_Num);
        Task<string> Cor_name(string CorporateResistration_Num);
       
        Task<List<Company_Entity>> GetList_Sido(string Sido);
        Task<int> GetList_Sido_Count(string Sido);
        Task<List<Company_Entity>> GetList();
        Task<List<Company_Entity>> List_Page(int Page);
        Task<int> GetList_Count();
        Task<List<Company_Entity>> Search_List(int Page, string Feild, string Query);
        Task<int> Search_List_Count(string Feild, string Query);
        Task<Company_Entity> Search_Cpr_Details(string Query);
        Task<List<Company_Entity>> List_Cpr_Details(string Query);
        Task<int> Num_Count();
        Task<Company_Entity> detail(string Cor_Code);
        //Task<List<Company_Entity>> Company_Name(string Cor_Name);
        Task<string> Company_Name_Code(string Cor_Code);

        /// <summary>
        /// 식별코드로 찾은 목록
        /// </summary>
        string Company_Name_Code_A(string Cor_Code);

        Task<string> Company_Code(string CorporateResistration_Num);
        Task Remove(string Cor_Code);
        Task Delete(string Cor_Code);
    }

    public interface ICompany_Sub_Lib
    {
        Task<Company_Sub_Entity> Add(Company_Sub_Entity _Entity);
        Task<Company_Sub_Entity> Edit(Company_Sub_Entity _Entity);
        Task<Company_Sub_Entity> Detail(string Company_Code);
        Task<List<Company_Sub_Entity>> List(string Company_Code);
        Task<int> being(string Cor_Code);   
    }

    public interface ICompany_Join_Lib
    {
        Task<List<Company_Join_Entity>> List_Join(string Sido, string GunGu);
        Task<Company_Join_Entity> Detail(string Company_Code);
        Task<List<Company_Join_Entity>> List_Page(int Page);
        Task<int> GetList_Count();
        Task<List<Company_Join_Entity>> List_Join_A(string Company_Sort);
        Task<List<Company_Join_Entity>> List_Join_B(string Type, string Condition, string Sido);
        Task<List<Company_Join_Entity>> List_Join_C(string Type, string Sido, string GunGu);
        Task<List<Company_Join_Entity>> List_Join_D(string Type, string Condition, string Sido, string GunGu);
        Task<List<Company_Join_Entity>> List_Join_E(string Company_Sort, string Sido);
        Task<List<Company_Join_Entity>> List_Join_F(string Company_Sort, string Sido, string GunGu);
    }

    public interface IContract_Sort_Lib
    {
        Task<int> Add(Contract_Sort_Entity cs);
        Task<List<Contract_Sort_Entity>> List_All(string ContractSort_Step);
        Task<Contract_Sort_Entity> Detail(string ContractSort_Code);
        Task<List<Contract_Sort_Entity>> List(string Up_Code);
        Task<List<Contract_Sort_Entity>> GetLists();
        Task<List<Contract_Sort_Entity>> GetLists_Page(int Page);
        Task<int> GetListsCount();
        Task<List<Contract_Sort_Entity>> List_Code(string ContractSort_Step, string Up_Code);
        Task<int> List_Code_Count(string ContractSort_Step, string Up_Code);
        Task<string> Name(string ContractSort_Code);
        string _Name(string ContractSort_Code);
        Task<int> LastNumber();
        Task Remove(int Aid);
    }

    public interface ICompany_Apt_Career_Lib
    {
        Task<Company_Career_Entity> add(Company_Career_Entity cc);
        Task<Company_Career_Entity> edit(Company_Career_Entity cc);
        Task<Company_Career_Entity> detail(string CC_Code);
        Task<Company_Career_Entity> detail_Apt(string Apt_Code, string ContractSort);
        Task<Company_Career_Entity> Details(int Aid);
        Task<List<Company_Career_Entity>> getlist_all(int Page, string Division);
        Task<int> Getcount_all(string Division);

        /// <summary>
        /// 계약 정보 전체 목록 2022-10-13
        /// </summary>
        Task<List<Company_Career_Entity>> getlist_apt_all(int Page, string Cor_Code);

        /// <summary>
        /// 계약 분류 식별코드로 계약정보 목록 만들기 2022-10-13
        /// </summary>
        Task<List<Company_Career_Entity>> getlist(int Page, string ContractSort, string Cor_Code);

        /// <summary>
        /// 계약 분류 식별코드로 계약정보 목록으로 찾은 수 2022-10-13
        /// </summary>
        Task<int> getlist_count(string ContractSort, string Cor_Code);

        /// <summary> 
        /// 계약 정보 전체 수 2022-10-13
        /// </summary>
        Task<int> Getcount_apt_all(string Cor_Code);

        /// <summary>
        /// 가장 최근 계약 정보 상세보기(위탁관리만)
        /// </summary>
        /// <param name="CC_Code"></param>
        /// <returns></returns>
       Task<int> detail_Apt_Count(string Apt_Code, string ContractSort, string Contract_start_date, string Contract_end_date);

        Task<List<Company_Career_Entity>> getlist_option(int Page, string Feild, string Query, string Division);
        Task<int> Getcount_option(string Feild, string Query, string Division);
        Task<List<Company_Career_Entity>> GetList_option_new(string Apt_Code, string Division);
        Task<int> Getcount_option_new(string Apt_Code, string Division);
        Task<List<Company_Career_Entity>> GetList_option_Sort_new(string Apt_Code, string Feild, string Query, string Division);
        Task<int> Getcount_option_Sort_new(string Apt_Code, string Feild, string Query, string Division);
        Task<List<Company_Career_Entity>> getlist_search(int Page, string Feild, string Query, string Division);
        Task<int> Getcount_search(string Feild, string Query, string Division);
        Task<List<Company_Career_Entity>> getlist_name_search(string Apt_Name);
        Task<List<Company_Career_Entity>> getlist_name(string Apt_Name);
        Task<int> apt_name_count(string Apt_Name);
        Task<int> be_date(string Apt_Code, string Cor_Code, string ContractSort, string Contract_start_date, string Contract_end_date);
        Task<int> be_Code(int Aid);

        /// <summary>
        /// 해당 공동주택 신원주택 위탁계약 정보 존재 여부 확인
        /// </summary>
        Task<int> BeApt(string Apt_Code, string Cor_Code);

        /// <summary>
        /// 위탁계약 코드 찾아 오기
        /// </summary>
        Task<Company_Career_Entity> BeAptCompany_Code(string Apt_Code, string Cor_Code);

        Task delete(string Aid, string Division);
        Task<List<Company_Career_Entity>> ListDrop(string Apt_Code, string ContractSort_Code);
        Task<string> LastCount();
        Task<string> svAgo(string Apt_Code, string Aid);
        Task<int> svAgoBe(string Apt_Code, string Aid);
        Task<string> svNext(string Apt_Code, string Aid);
        Task<int> svNextBe(string Apt_Code, string Aid);


        /// <summary>
        /// 계약 정보 옵션 목록
        /// </summary>
        Task<List<Company_Career_Entity>> getlist_option(int Page, string Feild, string Query);

        /// <summary>
        /// 날짜 계산
        /// </summary>
        int Date_scomp(string start, string end);

        /// <summary>
        /// 계약 정보 옵션 수
        /// </summary>
        Task<int> Getcount_option(string Feild, string Query);

        /// <summary>
        /// 첨부파일 추가
        /// </summary>
        Task File_Plus(int Aid);

        /// <summary>
        /// 첨부파일 추가
        /// </summary>
        Task File_Minus(int Aid);
    }
}

