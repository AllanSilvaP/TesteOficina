public class Sale
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }

    public ICollection<SaleItem> saleItems {get; set;} = new List<SaleItem>();
}
