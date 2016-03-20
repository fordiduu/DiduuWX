using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Helper
{
    public enum DBCommand
    {
        #region ResourceRequest
        NewResourceRequest,
        EnableResourceRequest,
        UpdateResourceRequest,
        UsefulRequest,
        GetSingleResourceRequest,
        GetListResourceRequest,
        Common_SP_Paging,
        GetTimesByIP,
        #endregion
        #region FreeVIP,
        CheckUserVilid,
        CheckAccountHasUsedTimes,
        GetNewAccount,
        WriteVIPLog,
        GetAccountById,
        UpdateAccountEnable,
        #endregion
    }
}
