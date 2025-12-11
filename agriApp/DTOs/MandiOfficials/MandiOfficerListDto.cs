namespace agriApp.DTOs.MandiOfficials
{
    public class MandiOfficerListDto
    {
        public Guid OfficialId { get; set; }
        public string OfficialName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string EmployeeId { get; set; } = default!;
        public string RoleCode { get; set; } = default!;
    }
}
