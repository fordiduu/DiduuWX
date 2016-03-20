using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Model
{
    public class ReSourceRequest : BaseModel
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
        /// Content
        /// </summary>		
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        /// <summary>
        /// Contact
        /// </summary>		
        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set { _contact = value; }
        }
        /// <summary>
        /// useful
        /// </summary>		
        private int _useful;
        public int useful
        {
            get { return _useful; }
            set { _useful = value; }
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
        /// UpdateTime
        /// </summary>		
        private DateTime _updatetime;
        public DateTime UpdateTime
        {
            get { return _updatetime; }
            set { _updatetime = value; }
        }
        /// <summary>
        /// IPAddr
        /// </summary>
        private string _iPAddr;
        public string IPAddr
        {
            get { return _iPAddr; }
            set { _iPAddr = value; }
        }
        /// <summary>
        /// IsEnable
        /// </summary>
        private bool _isEnable;
        public bool IsEnable
        {
            get { return _isEnable; }
            set { _isEnable = value; }
        }
        /// <summary>
        /// ReplyCount
        /// </summary>
        private int _replyCount;
        public int ReplyCount
        {
            get { return _replyCount; }
            set { _replyCount = value; }
        }


    }
}
