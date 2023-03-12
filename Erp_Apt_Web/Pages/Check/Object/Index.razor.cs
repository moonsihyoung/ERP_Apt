﻿using Erp_Apt_Lib.Check;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Check.Object
{
    /// <summary>
    /// 시설물 점검 대상
    /// </summary>
    public partial class Index
    {
        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public int LevelCount { get; set; }
        public string Views { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        //public string Views { get; set; } = 
        
        #endregion

        #region 로드
        public Check_Object_Entity ann { get; set; } = new Check_Object_Entity();
        List<Check_Object_Entity> bnn = new List<Check_Object_Entity>();
        
        #endregion

        #region 인스턴스
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        
        [Inject] public ICheck_Object_Lib check_Object { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        #endregion

        /// <summary>
        /// 처음 로드시에 실행
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {
                //로그인 정보
                Apt_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Code")?.Value;
                User_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "User_Code")?.Value;
                Apt_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Name")?.Value;
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);

                if (LevelCount > 5)
                {
                    await DisplayData();

                    ann.PostDate = DateTime.Now;
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
                    MyNav.NavigateTo("/");
                }                
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 시설물 대상 목록 정보
        /// </summary>
        /// <returns></returns>
        private async Task DisplayData()
        {            
            bnn = await check_Object.CheckObject_Data_Index();
            StateHasChanged();
        }

        /// <summary>
        /// 수정하기
        /// </summary>
        private void ByEdit()
        {
            InsertViews = "B";
            StateHasChanged();
        }

        public async Task btnSave()
        {
            #region 아이피 입력
            string myIPAddress = "";
            var ipentry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in ipentry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    myIPAddress = ip.ToString();
                    break;
                }
            }
            //bnn.ModifyDate = DateTime.Now;
            
            ann.PostIP = myIPAddress;
            ann.Del = "A";
            #endregion

            if (ann.CheckObjectID > 0)
            {
                await check_Object.CheckObject_Data_Modify(ann);
            }
            else
            {
                ann.AptCode = "sw1";
                ann.Check_Object_Code = "Obj" + await check_Object.CheckObject_Last();
                await check_Object.CheckObject_Date_Insert(ann);
            }

            await DisplayData();

            InsertViews = "A";
            StateHasChanged();
        }

        /// <summary>
        /// 입력 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
            StateHasChanged();
        }

        /// <summary>
        /// 대상 수정 열기
        /// </summary>
        /// <param name="check_Object_Entity"></param>
        private void ByEdit(Check_Object_Entity check_Object_Entity)
        {
            ann = check_Object_Entity;
            InsertViews = "B";
            StateHasChanged();
        }

        /// <summary>
        /// 점검 대상 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task ByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 점검 사항을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await check_Object.CheckObject_Date_Delete(Aid);
                await DisplayData();
                StateHasChanged();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "삭제되지 않았습니다.");
            }
            StateHasChanged();
        }

        /// <summary>
        /// 대상 입력 열기
        /// </summary>
        private void OnInputbutton()
        {
            InsertViews = "B";
            ann = new Check_Object_Entity();
            ann.PostDate = DateTime.Now;
            
            StateHasChanged();
        }

    }
}
