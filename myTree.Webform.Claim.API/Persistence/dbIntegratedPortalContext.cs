using CI.TMS.Claim.API.Domain.Entities.IntegratedPortal;
using CI.TMS.Claim.API.Domain.Entities.K2;
using Microsoft.EntityFrameworkCore;

namespace CI.TMS.Claim.API.Persistence
{
    public partial class dbIntegratedPortalContext: DbContext
    {
        private readonly IConfiguration config;

        public dbIntegratedPortalContext(IConfiguration _config)
        {
            config = _config;
        }

        public dbIntegratedPortalContext(DbContextOptions<dbIntegratedPortalContext> options, IConfiguration _config)
            : base(options)
        {
            config = _config;
        }

        public virtual DbSet<K2ActivityUserList> K2ActivityUserList { get; set; }
        public virtual DbSet<OCSMFLQueue> OCSMFLQueueList { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Server=ciforhq-unisys.cifor-icraf.org;user=app_user;password=vVKem$bYGLz@;Database=dbCONTRACT_SQL02_AZURE_20220104;Trusted_Connection=False;");
                optionsBuilder.UseSqlServer(config["ConnectionStrings:dbIntegratedPortal"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
