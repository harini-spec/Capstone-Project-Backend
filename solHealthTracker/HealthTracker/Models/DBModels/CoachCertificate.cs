using System.ComponentModel.DataAnnotations;

namespace HealthTracker.Models.DBModels
{
    public class CoachCertificate
    {
        [Key]
        public int Id { get; set; }
        public int CoachId { get; set; }
        public string CertificateURL { get; set; }
    }
}
