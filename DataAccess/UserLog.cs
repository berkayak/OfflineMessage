//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserLog
    {
        public int ID { get; set; }
        public System.DateTime date { get; set; }
        public int userID { get; set; }
        public bool isError { get; set; }
        public string logMessage { get; set; }
    }
}
