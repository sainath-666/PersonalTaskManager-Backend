namespace PersonalTaskManager.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public string SessionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
