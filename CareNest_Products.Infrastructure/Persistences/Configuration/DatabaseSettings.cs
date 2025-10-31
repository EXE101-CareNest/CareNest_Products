namespace CareNest_Products.Infrastructure.Persistences.Configuration
{
    public class DatabaseSettings
    {
        public string? Ip { get; set; }
        public int Port { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public string? Database { get; set; }
        public bool? Pooling { get; set; }
        public int? MaximumPoolSize { get; set; }
        public int? MinimumPoolSize { get; set; }
        public int? Timeout { get; set; }
        public string GetConnectionString()
        {
            var baseConn = $"Host={Ip};Port={Port};Database={Database};Username={User};Password={Password}";
            var pooling = Pooling.HasValue ? $";Pooling={(Pooling.Value ? "true" : "false")}" : string.Empty;
            var maxPool = MaximumPoolSize.HasValue ? $";Maximum Pool Size={MaximumPoolSize}" : string.Empty;
            var minPool = MinimumPoolSize.HasValue ? $";Minimum Pool Size={MinimumPoolSize}" : string.Empty;
            var timeout = Timeout.HasValue ? $";Timeout={Timeout}" : string.Empty;
            return baseConn + pooling + maxPool + minPool + timeout;
        }
        public Action Display => () =>
        {
            Console.WriteLine("----- Database Settings -----");
            Console.WriteLine($"IP       : {Ip}");
            Console.WriteLine($"Port     : {Port}");
            Console.WriteLine($"User     : {User}");
            Console.WriteLine($"Password : {Password}");
            Console.WriteLine($"Database : {Database}");
            Console.WriteLine($"Pooling  : {Pooling}");
            Console.WriteLine($"MaxPool  : {MaximumPoolSize}");
            Console.WriteLine($"MinPool  : {MinimumPoolSize}");
            Console.WriteLine($"Timeout  : {Timeout}");
            Console.WriteLine();
        };
    }
}
