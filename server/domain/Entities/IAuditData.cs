using System;
using ReactAdvantage.Domain.Entities.Authorization.Users;

namespace ReactAdvantage.Domain.Entities
{
    //TODO: add this to the entities and provide the methods on the repository classes to add these items on the save of the data
    public interface IAuditData
    {
        DateTime CreateDateTime { get; set; }

        DateTime ModifiedDateTime { get; set; }

        User CreatedBy { get; set; }

        User ModifiedBy { get; set; }


    }
}