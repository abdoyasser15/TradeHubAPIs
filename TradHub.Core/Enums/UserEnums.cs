using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Enums
{
    public enum UserRole
    {
        User,        
        CompanyOwner,    // مالك شركة تبيع
        CompanyStaff,    // موظف تابع لشركة
        Admin       // مدير النظام
    }

    public enum AccountType
    {
        Individual,  // فرد عادي
        Business     // شركة مشترية
    }
}
