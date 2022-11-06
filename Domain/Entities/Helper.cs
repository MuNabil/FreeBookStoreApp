using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Helper
    {
        public const string UserImgPath = "/Images/Users";
        public const string SaveUserImgPath = "Images/Users";
        public const string DefaultImage = "DefaultImage.jpg";

        public const string MsgType = "msgType";
        public const string Title = "title";
        public const string Msg = "msg";

        public const string Success = "success";
        public const string Error = "error";

        public const string Save = "Save";
        public const string Update = "Update";
        public const string Delete = "Delete";

        public enum eCurrentState
        {
            Delete = 0,
            Active = 1
        }
    }
}
