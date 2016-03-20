using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Model
{
    public class ReSourceRequest_Reply : BaseModel
    {
        /// <summary>
        /// Id
        /// </summary>		
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// ReplyContent
        /// </summary>		
        private string _replycontent;
        public string ReplyContent
        {
            get { return _replycontent; }
            set { _replycontent = value; }
        }
        /// <summary>
        /// AddTime
        /// </summary>		
        private DateTime _addtime;
        public DateTime AddTime
        {
            get { return _addtime; }
            set { _addtime = value; }
        }
        /// <summary>
        /// RequestId
        /// </summary>		
        private int _requestid;
        public int RequestId
        {
            get { return _requestid; }
            set { _requestid = value; }
        }
    }
}
