using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wtiau.Health.Web
{
    public enum TosterType
    {
        JustTitel,
        WithTitel,
        Maseage
    }

    public enum CodeGroup
    {
        Gender = 1,
        ResponseType = 2,
        Nationality = 4,
        Marriage = 5,
        Grad =6,
        BloodGroup = 7,
        Insurance = 8,
        HomeType = 9,
        BirthYear = 10,
    }

    public enum Forms
    {
        form1 = 3,
        form2 = 5
    }
}
