using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Model
{
    public class VIPLog:BaseModel
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
        /// IdentityType
        /// </summary>		
        private string _identitytype;
        public string IdentityType
        {
            get { return _identitytype; }
            set { _identitytype = value; }
        }
        /// <summary>
        /// IdentityNum
        /// </summary>		
        private string _identitynum;
        public string IdentityNum
        {
            get { return _identitynum; }
            set { _identitynum = value; }
        }
        /// <summary>
        /// VIPId
        /// </summary>		
        private int _vipid;
        public int VIPId
        {
            get { return _vipid; }
            set { _vipid = value; }
        }
        /// <summary>
        /// IPAddr
        /// </summary>		
        private string _ipaddr;
        public string IPAddr
        {
            get { return _ipaddr; }
            set { _ipaddr = value; }
        }
        /// <summary>
        /// UseTime
        /// </summary>		
        private DateTime _usetime;
        public DateTime UseTime
        {
            get { return _usetime; }
            set { _usetime = value; }
        }

    }
}
