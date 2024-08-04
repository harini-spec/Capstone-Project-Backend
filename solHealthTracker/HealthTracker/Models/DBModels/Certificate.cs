using System.ComponentModel.DataAnnotations;

namespace HealthTracker.Models.DBModels
{
    public class Certificate
    {
        [Key]
        public int Id { get; set; }
        public int CoachId { get; set; }
        public string CertificateURL { get; set; }
    }
}
