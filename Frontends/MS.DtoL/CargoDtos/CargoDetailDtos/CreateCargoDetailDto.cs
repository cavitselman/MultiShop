namespace MS.DtoL.CargoDtos.CargoDetailDtos
{
    public class CreateCargoDetailDto
    {
        public string SenderCustomer { get; set; }
        public string ReceiverCustomer { get; set; }
        public string Barcode { get; set; }
        public string? Reason { get; set; }
        public string? Description { get; set; }
        public int OrderingId { get; set; }
        public int CargoCompanyId { get; set; }
        public bool IsSelected { get; set; }
        public string ProductId { get; set; }
    }
}
