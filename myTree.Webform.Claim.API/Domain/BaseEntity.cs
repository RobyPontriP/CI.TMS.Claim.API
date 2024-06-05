namespace CI.TMS.Claim.API.Domain
{
    public abstract class BaseEntity<T>
    {
        public abstract T Id { get; set; }
        public abstract bool IsActive { get; set; }
        public abstract DateTime? CreatedAt { get; set; }
        public abstract string CreatedBy { get; set; }
        public abstract DateTime? UpdatedAt { get; set; }
        public abstract string UpdatedBy { get; set; } 
    }
}
