namespace pd311_web_api.BLL.DTOs.Car
{
    public class CarDto
    {
        public string Id { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string? Color { get; set; }
        public string? Gearbox { get; set; }
        public string? Manufacture { get; set; }
        public List<string> Images { get; set; } = [];
    }
}
