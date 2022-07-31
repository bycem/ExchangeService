using System;

namespace Domain.Entities
{
    public class BaseEntity
    { 
        public BaseEntity(Guid id,DateTime createDate)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty"); 
            if (createDate == DateTime.MinValue)  throw new ArgumentException("Create date cannot be dateteime min value");
            
            Id = id;
            CreateDate = createDate;
        }

        public Guid Id { get; }
        
        public DateTime CreateDate { get; }
    }
}