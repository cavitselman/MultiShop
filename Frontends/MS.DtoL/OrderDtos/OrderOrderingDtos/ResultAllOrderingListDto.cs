namespace MS.DtoL.OrderDtos.OrderOrderingDtos
{
    public class ResultAllOrderingListDto
    {
        public int OrderingId { get; set; }
        public string NameSurname { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
