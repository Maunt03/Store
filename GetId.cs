namespace Store
{
    public class GetId
    {
        public static int GetUserId(string userName)
        {
            using var context = new AppDbContext();
            var records = from user in context.Users
                          where user.Login == userName
                          select user.Id;
            if (records.Count() != 1) return -1;
            int result = 0;
            foreach (var record in records)
            {
                result = record;
            }
            return result;
        }

        public static int GetProductId(string productName)
        {
            using var context = new AppDbContext();
            var records = from product in context.Products
                          where product.Name == productName
                          select product.Id;
            if (records.Count() != 1) return -1;
            int result = 0;
            foreach (var record in records)
            {
                result = record;
            }
            return result;
        }
    }
}
