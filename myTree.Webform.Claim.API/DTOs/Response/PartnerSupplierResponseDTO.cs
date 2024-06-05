namespace CI.TMS.Claim.API.DTOs.Response
{
    public class PartnerSupplierResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TaxSystem { get; set; }
        public string TaxSystemName { get; set; }
    }

    public class PartnerSupplierDropdown
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }
}
