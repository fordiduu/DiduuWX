using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Model
{
    public class FreeVIP:BaseModel
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
        /// TId
        /// </summary>		
        private int _tid;
        public int TId
        {
            get { return _tid; }
            set { _tid = value; }
        }
        /// <summary>
        /// Account
        /// </summary>		
        private string _account;
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }
        /// <summary>
        /// Pwd
        /// </summary>		
        private string _pwd;
        public string Pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }
        /// <summary>
        /// StartTime
        /// </summary>		
        private DateTime _starttime;
        public DateTime StartTime
        {
            get { return _starttime; }
            set { _starttime = value; }
        }
        /// <summary>
        /// ValidTime
        /// </summary>		
        private int _validtime;
        public int ValidTime
        {
            get { return _validtime; }
            set { _validtime = value; }
        }
        /// <summary>
        /// Notes
        /// </summary>		
        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }
        /// <summary>
        /// IsEnable
        /// </summary>		
        private bool _isenable;
        public bool IsEnable
        {
            get { return _isenable; }
            set { _isenable = value; }
        }

    }
}
