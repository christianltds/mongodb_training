namespace mongodb.dtos
{
    public class TransferFundsDto
    {
        
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public double Amount { get; set; }
    }
}