namespace pd311_web_api.BLL.DTOs.Car
{
    public class CarListDto
    {
        public List<CarDto> Cars { get; set; } = [];
        public int Page { get; set; } = 1;
        public int TotalCount { get; set; } = 0;
        public int PageCount { get; set; } = 1;
    }
}
